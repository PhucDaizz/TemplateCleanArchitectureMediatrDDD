using AuthService.Application.Common.Models;
using AuthService.Application.Common.Response;
using AuthService.Application.DTOs;
using MediatR;

namespace AuthService.Application.Features.Article.Queries.GetPublishedArticles
{
    public class GetPublishedArticlesQuery : IRequest<Result<PagedResult<PublishedArticleDto>>>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? SearchTerm { get; set; }
        public string? SortBy { get; set; } // Title, PublishedAt, AuthorEmail
        public bool IsDescending { get; set; } = true;

        public GetPublishedArticlesQuery(int pageNumber = 1, int pageSize = 10, string? searchTerm = null)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            SearchTerm = searchTerm;
        }
    }
}
