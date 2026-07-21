using AuthService.Application.Common.Interfaces;
using AuthService.Application.Common.Response;
using AuthService.Domain.Exceptions;
using AuthService.Domain.Repositories;
using MediatR;

namespace AuthService.Application.Features.Article.Commands.PublishArticle
{
    public class PublishArticleCommandHandler : IRequestHandler<PublishArticleCommand, Result<Domain.Entities.Article>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public PublishArticleCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Domain.Entities.Article>> Handle(PublishArticleCommand request, CancellationToken cancellationToken)
        {
            var article = await _unitOfWork.ArticleRepository.GetByIdWithSectionsAsync(request.ArticleId, cancellationToken);

            if (article == null)
                throw new DomainException($"Article with Id {request.ArticleId} not found.");

            article.Publish();

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success(article);
        }
    }
}
