using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.OpenApi;
using Scalar.AspNetCore;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Xml.XPath;

namespace AuthService.API.StartUp
{
    public static class OpenApiConfig
    {
        public static void AddOpenApiServices(this IServiceCollection services)
        {

            services.AddEndpointsApiExplorer();
            services.AddOpenApi(options =>
            {
                // 1. Document Transformer (Khai báo Bearer token)
                options.AddDocumentTransformer((document, context, cancellationToken) =>
                {
                    document.Components ??= new OpenApiComponents();
                    document.Components.SecuritySchemes ??= new Dictionary<string, IOpenApiSecurityScheme>();

                    document.Components.SecuritySchemes["Bearer"] = new OpenApiSecurityScheme
                    {
                        Type = SecuritySchemeType.Http,
                        Scheme = "bearer",
                        BearerFormat = "JWT",
                        In = ParameterLocation.Header,
                        Description = "Nhập JWT token của bạn (không cần chữ 'Bearer ' phía trước)."
                    };
                    return Task.CompletedTask;
                });

                // 2. Operation Transformer: Gắn ổ khóa cho API có [Authorize]
                options.AddOperationTransformer((operation, context, cancellationToken) =>
                {
                    var metadata = context.Description.ActionDescriptor.EndpointMetadata;
                    var requiresAuthorization = metadata.OfType<AuthorizeAttribute>().Any();

                    if (requiresAuthorization)
                    {
                        operation.Security ??= new List<OpenApiSecurityRequirement>();
                        operation.Security.Add(new OpenApiSecurityRequirement
                        {
                            [new OpenApiSecuritySchemeReference("Bearer", context.Document)] = new List<string>()
                        });
                    }
                    return Task.CompletedTask;
                });

                // 3. Operation Transformer: ĐỌC XML COMMENT CHUẨN (Summary + Param)
                options.AddOperationTransformer((operation, context, cancellationToken) =>
                {
                    var methodInfo = (context.Description.ActionDescriptor as ControllerActionDescriptor)?.MethodInfo;
                    if (methodInfo == null) return Task.CompletedTask;

                    var entryAssembly = Assembly.GetEntryAssembly();
                    if (entryAssembly == null) return Task.CompletedTask;

                    var xmlFileName = $"{entryAssembly.GetName().Name}.xml";
                    var xmlFile = Path.Combine(AppContext.BaseDirectory, xmlFileName);
                    if (!File.Exists(xmlFile)) return Task.CompletedTask;

                    var xpath = new XPathDocument(xmlFile);
                    var navigator = xpath.CreateNavigator();

                    // Tạo chuỗi XPath tìm đúng method
                    var memberId = $"M:{methodInfo.DeclaringType?.FullName}.{methodInfo.Name}";
                    var paramTypes = methodInfo.GetParameters().Select(p => p.ParameterType.FullName);
                    if (paramTypes.Any())
                    {
                        memberId += $"({string.Join(",", paramTypes)})";
                    }
                    memberId = memberId.Replace('+', '.');

                    var memberNode = navigator.SelectSingleNode($"/doc/members/member[@name='{memberId}']");
                    if (memberNode != null)
                    {
                        // Lấy mô tả chính (<summary>)
                        var summaryNode = memberNode.SelectSingleNode("summary");
                        if (summaryNode != null)
                        {
                            // Dùng Regex để xóa các khoảng trắng/dòng thừa do format XML sinh ra
                            string summaryText = Regex.Replace(summaryNode.InnerXml.Trim(), @"\s+", " ");
                            operation.Summary = summaryText;
                            operation.Description = summaryText; 
                        }

                        // Lấy mô tả tham số (<param>)
                        var paramNodes = memberNode.Select("param");
                        if (paramNodes != null && operation.Parameters != null)
                        {
                            foreach (XPathNavigator paramNode in paramNodes)
                            {
                                var paramName = paramNode.GetAttribute("name", "");
                                var paramDesc = Regex.Replace(paramNode.InnerXml.Trim(), @"\s+", " ");

                                // Tìm tham số trùng tên trong OpenAPI operation để gán mô tả
                                var openApiParam = operation.Parameters.FirstOrDefault(p => p.Name == paramName);
                                if (openApiParam != null)
                                {
                                    openApiParam.Description = paramDesc;
                                }
                            }
                        }
                    }

                    return Task.CompletedTask;
                });
            });
        }
        public static void UseOpenApiConfiguration(this WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();

                app.MapScalarApiReference(options =>
                {
                    options.PersistentAuthentication = true;
                    options.Layout = ScalarLayout.Classic;
                    options.Theme = ScalarTheme.DeepSpace;
                });
            }
        }
    }
}
