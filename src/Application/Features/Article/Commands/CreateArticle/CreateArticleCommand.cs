using AuthService.Application.Common.Response;
using MediatR;

namespace AuthService.Application.Features.Article.Commands.CreateArticle
{
    public class CreateArticleCommand: IRequest<Result<Domain.Entities.Article>>
    {
        public string Title { get; set; }
        public string AuthorEmail { get; set; }
    }
}
