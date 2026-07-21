using AuthService.Application.Common.Interfaces;
using AuthService.Domain.Common;
using AuthService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Infrastructure
{
    public class ApplicationDbContext: DbContext, IApplicationDbContext
    {
        private readonly IDomainEventService _domainEventService;
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IDomainEventService domainEventService) : base(options)
        {
            _domainEventService = domainEventService;
        }
        
        public DbSet<Article> Articles { get; set; }
        public DbSet<Section> Sections { get; set; }
        public DbSet<Subscriber> Subscribers { get; set; }

        public IQueryable<Article> ArticlesQuery => Articles.AsQueryable();
        public IQueryable<Section> SectionsQuery => Sections.AsQueryable();
        public IQueryable<Subscriber> SubscriberQuery => Subscribers.AsQueryable();


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // write your customizations here    
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var domainEntities = ChangeTracker
                .Entries<IHasDomainEvent>()
                .Where(x => x.Entity.DomainEvents.Any())
                .Select(x => x.Entity)
                .ToList();

            var domainEvents = domainEntities
                .SelectMany(x => x.DomainEvents)
                .ToList();

            domainEntities.ForEach(entity => entity.ClearDomainEvents());

            var result = await base.SaveChangesAsync(cancellationToken);

            foreach (var domainEvent in domainEvents)
            {
                await _domainEventService.PublishAsync(domainEvent, cancellationToken);
            }

            return result;
        }
    }
}
