using AuthService.Domain.Entities;
using Infrastructure.Data.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthService.Infrastructure.Data.Configurations
{
    public class SubscriberConfiguration : BaseEntityConfiguration<Subscriber, Guid>
    {
        public override void Configure(EntityTypeBuilder<Subscriber> builder)
        {
            base.Configure(builder);

            builder.ToTable("Subscriber");

            builder.Property(x => x.Id)
                .ValueGeneratedNever()
                .HasColumnName("Id");

            builder.Property(x => x.Email)
                .IsRequired()
                .HasMaxLength(128)
                .HasColumnName("Email");

            builder.Property(x => x.FullName)
                .IsRequired()
                .HasMaxLength(255)
                .HasColumnName("FullName");

            builder.Property(x => x.IsActive)
                .IsRequired()
                .HasDefaultValue(true)
                .HasColumnName("IsActive");

            builder.HasIndex(x => x.Email)
                .IsUnique()
                .HasDatabaseName("IX_Subscriber_Email");

            builder.HasIndex(x => new { x.Email, x.IsActive })
                .HasDatabaseName("IX_Subscriber_Email_IsActive");
        }
    }
}
