using AuthService.Application.Common.Interfaces;
using AuthService.Application.Common.Response;
using AuthService.Application.DTOs;
using AuthService.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Application.Features.Article.Queries.GetArticleById
{
    public class GetArticleByIdQueryHandler : IRequestHandler<GetArticleByIdQuery, Result<ArticleDetailDto>>
    {
        private readonly IApplicationDbContext _context;

        public GetArticleByIdQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result<ArticleDetailDto>> Handle(GetArticleByIdQuery request, CancellationToken cancellationToken)
        {
            var article = await _context.ArticlesQuery
                .AsNoTracking()
                .Include(a => a.Sections)
                .Where(a => a.Id == request.ArticleId)
                .Select(a => new ArticleDetailDto
                {
                    Id = a.Id,
                    Title = a.Title,
                    AuthorEmail = a.AuthorEmail,
                    Status = a.Status.ToString(),
                    PublishedAt = a.PublishedAt,
                    CreatedAt = a.CreatedAt,
                    UpdatedAt = a.UpdatedAt,
                    Sections = a.Sections
                        .OrderBy(s => s.Order)
                        .Select(s => new SectionDto
                        {
                            Id = s.Id,
                            Title = s.Title,
                            Content = s.Content,
                            Order = s.Order
                        }).ToList()
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (article == null)
                throw new DomainException($"Article with Id {request.ArticleId} not found.");

            return Result.Success(article);
        }
    }
}
