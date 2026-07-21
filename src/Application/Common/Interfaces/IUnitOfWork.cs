using AuthService.Domain.Repositories;

namespace AuthService.Application.Common.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IArticleRepository ArticleRepository { get; }
        ISubcriberRepository SubcriberRepository { get; }   

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }
}
