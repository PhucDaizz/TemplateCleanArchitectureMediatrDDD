using AuthService.Application.Common.Response;
using AuthService.Application.DTOs;
using MediatR;

namespace AuthService.Application.Features.Article.Queries.GetArticleById
{
    public class GetArticleByIdQuery : IRequest<Result<ArticleDetailDto>>
    {
        public Guid ArticleId { get; set; }

        public GetArticleByIdQuery(Guid articleId)
        {
            ArticleId = articleId;
        }
    }
}