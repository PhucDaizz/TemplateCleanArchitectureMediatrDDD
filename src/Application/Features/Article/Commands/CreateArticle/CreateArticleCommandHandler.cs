using AuthService.Application.Common.Interfaces;
using AuthService.Application.Common.Response;
using MediatR;

namespace AuthService.Application.Features.Article.Commands.CreateArticle
{
    public class CreateArticleCommandHandler : IRequestHandler<CreateArticleCommand, Result<Domain.Entities.Article>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateArticleCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Domain.Entities.Article>> Handle(CreateArticleCommand request, CancellationToken cancellationToken)
        {
            var article = Domain.Entities.Article.Create(request.Title, request.AuthorEmail);

            await _unitOfWork.ArticleRepository.AddAsync(article);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success(article);
        }
    }
}
