using AuthService.Domain.Entities;

namespace AuthService.Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        public IQueryable<Article> ArticlesQuery { get; }
        public IQueryable<Section> SectionsQuery { get; }
        public IQueryable<Subscriber> SubscriberQuery { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
