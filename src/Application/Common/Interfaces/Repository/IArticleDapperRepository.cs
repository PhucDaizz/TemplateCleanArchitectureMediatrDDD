using AuthService.Application.DTOs;

namespace AuthService.Application.Common.Interfaces.Repository
{
    public interface IArticleDapperRepository
    {
        Task<IEnumerable<ArticleDto>> GetArticlesByStatusAsync(int status);
        Task InsertArticleAsync(ArticleDto article);
    }
}
