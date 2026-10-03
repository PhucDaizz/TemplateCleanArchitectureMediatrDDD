using AuthService.Application.Common.Interfaces;
using AuthService.Application.Common.Interfaces.Repository;
using AuthService.Application.Common.Response;
using AuthService.Application.DTOs;
using MediatR;

namespace AuthService.Application.Features.Article.Commands.CreateArticleDapper
{
    public class CreateArticleDapperCommandHandler : IRequestHandler<CreateArticleDapperCommand, Result<Domain.Entities.Article>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IArticleDapperRepository _articleDapperRepository;

        public CreateArticleDapperCommandHandler(IUnitOfWork unitOfWork, IArticleDapperRepository articleDapperRepository)
        {
            _unitOfWork = unitOfWork;
            _articleDapperRepository = articleDapperRepository;
        }

        public async Task<Result<Domain.Entities.Article>> Handle(CreateArticleDapperCommand request, CancellationToken cancellationToken)
        {
            await _unitOfWork.BeginTransactionAsync();

            try
            {
                var article = Domain.Entities.Article.Create(request.Title, request.AuthorEmail);
                await _unitOfWork.ArticleRepository.AddAsync(article, cancellationToken);
                await _articleDapperRepository.InsertArticleAsync(new ArticleDto { 
                    Id = article.Id,
                    Title = request.Title,
                    AuthorEmail = request.AuthorEmail,
                    Status = article.Status.ToString(),
                    PublishedAt = DateTime.UtcNow,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                });

                await _unitOfWork.SaveChangesAsync(cancellationToken);
                await _unitOfWork.CommitTransactionAsync();

                return Result.Success(article);
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }
    }
}
