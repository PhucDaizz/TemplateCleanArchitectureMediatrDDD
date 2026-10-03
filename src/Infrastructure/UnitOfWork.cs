using AuthService.Application.Common.Interfaces;
using AuthService.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;

namespace AuthService.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private IDbContextTransaction? _transaction;
        private readonly IArticleRepository _articleRepository; 
        private readonly ISubcriberRepository _subcriberRepository;

        public UnitOfWork(ApplicationDbContext context,
            IArticleRepository articleRepository,
            ISubcriberRepository subcriberRepository)
        {
            _context = context;
            _articleRepository = articleRepository;
            _subcriberRepository = subcriberRepository;
        }

        public IArticleRepository ArticleRepository => _articleRepository;
        public ISubcriberRepository SubcriberRepository => _subcriberRepository;

        public IDbConnection Connection => _context.Database.GetDbConnection();
        public IDbTransaction? Transaction => _transaction?.GetDbTransaction();


        public async Task BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.CommitAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public void Dispose()
        {
            _transaction?.Dispose();
        }

        public async Task RollbackTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
