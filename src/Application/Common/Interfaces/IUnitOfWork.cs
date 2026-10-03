using AuthService.Domain.Repositories;
using System.Data;

namespace AuthService.Application.Common.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IArticleRepository ArticleRepository { get; }
        ISubcriberRepository SubcriberRepository { get; }  
        
        IDbConnection Connection { get; }
        IDbTransaction Transaction { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }
}
