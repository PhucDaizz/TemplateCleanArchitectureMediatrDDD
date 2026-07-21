using AuthService.Domain.Entities;
using AuthService.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Infrastructure.Data.Repositories
{
    public class ArticleRepository : BaseRepository<Article>, IArticleRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public ArticleRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Article?> GetByIdWithSectionsAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Articles
                .Include(a => a.Sections)
                .FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
        }
    }
}
