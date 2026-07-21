using AuthService.Domain.Entities;
using AuthService.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Infrastructure.Data.Repositories
{
    public class SubcriberRepository : BaseRepository<Subscriber>, ISubcriberRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public SubcriberRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
