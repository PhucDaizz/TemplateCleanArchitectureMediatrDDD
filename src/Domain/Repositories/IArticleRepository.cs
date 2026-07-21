using AuthService.Domain.Entities;

namespace AuthService.Domain.Repositories
{
    public interface IArticleRepository: IRepository<Article>
    {
        Task<Article?> GetByIdWithSectionsAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
