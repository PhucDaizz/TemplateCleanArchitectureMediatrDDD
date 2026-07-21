using AuthService.Application.Common.Response;
using MediatR;

namespace AuthService.Application.Features.Article.Commands.PublishArticle
{
    public class PublishArticleCommand : IRequest<Result<Domain.Entities.Article>>
    {
        public PublishArticleCommand(Guid articleId)
        {
            ArticleId = articleId;
        }

        public Guid ArticleId { get; set; }
    }
}
