using AuthService.Application.Common.Interfaces;
using AuthService.Application.Common.Models;
using AuthService.Application.Common.Response;
using AuthService.Application.DTOs;
using AuthService.Domain.Enum;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Application.Features.Article.Queries.GetPublishedArticles
{
    public class GetPublishedArticlesQueryHandler : IRequestHandler<GetPublishedArticlesQuery, Result<PagedResult<PublishedArticleDto>>>
    {
        private readonly IApplicationDbContext _context;

        public GetPublishedArticlesQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result<PagedResult<PublishedArticleDto>>> Handle(
            GetPublishedArticlesQuery request,
            CancellationToken cancellationToken)
        {
            IQueryable<Domain.Entities.Article> query = _context.ArticlesQuery
                .AsNoTracking()
                .Where(a => a.Status == ArticleStatus.Published);

            // 2. Apply search filter
            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            {
                var searchTerm = request.SearchTerm.Trim().ToLower();
                query = query.Where(a =>
                    a.Title.ToLower().Contains(searchTerm) ||
                    a.AuthorEmail.ToLower().Contains(searchTerm));
            }

            // 3. Get total count before pagination
            var totalCount = await query.CountAsync(cancellationToken);

            // 4. Apply sorting
            query = ApplySorting(query, request.SortBy, request.IsDescending);

            // 5. Apply pagination
            var articles = await query
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(a => new PublishedArticleDto
                {
                    Id = a.Id,
                    Title = a.Title,
                    AuthorEmail = a.AuthorEmail,
                    PublishedAt = a.PublishedAt ?? DateTime.UtcNow,
                    SectionCount = a.Sections.Count,
                    CreatedAt = a.CreatedAt
                })
                .ToListAsync(cancellationToken);

            // 6. Build paginated result
            var result = new PagedResult<PublishedArticleDto>
            {
                Items = articles,
                TotalCount = totalCount,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize
            };

            return Result.Success(result);
        }

        private IQueryable<Domain.Entities.Article> ApplySorting(
            IQueryable<Domain.Entities.Article> query,
            string? sortBy,
            bool isDescending)
        {
            return (sortBy?.ToLower()) switch
            {
                "title" => isDescending
                    ? query.OrderByDescending(a => a.Title)
                    : query.OrderBy(a => a.Title),
                "authoremail" => isDescending
                    ? query.OrderByDescending(a => a.AuthorEmail)
                    : query.OrderBy(a => a.AuthorEmail),
                "publishedat" => isDescending
                    ? query.OrderByDescending(a => a.PublishedAt)
                    : query.OrderBy(a => a.PublishedAt),
                _ => isDescending
                    ? query.OrderByDescending(a => a.PublishedAt)
                    : query.OrderBy(a => a.PublishedAt)
            };
        }
    }
}