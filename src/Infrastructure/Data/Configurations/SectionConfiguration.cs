using AuthService.Domain.Entities;
using Infrastructure.Data.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthService.Infrastructure.Data.Configurations
{
    public class SectionConfiguration : BaseEntityConfiguration<Section, Guid>
    {
        public override void Configure(EntityTypeBuilder<Section> builder)
        {
            base.Configure(builder);

            builder.ToTable("Section");

            builder.Property(x => x.Id)
                .ValueGeneratedNever();

            builder.Property(x => x.ArticleId)
                .IsRequired()
                .HasColumnName("ArticleId");

            builder.Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(255)
                .HasColumnName("Title");

            builder.Property(x => x.Content)
                .IsRequired()
                .HasMaxLength(4000)
                .HasColumnName("Content");

            builder.Property(x => x.Order)
                .IsRequired()
                .HasDefaultValue(0)
                .HasColumnName("Order");

            // Indexes
            builder.HasIndex(x => new { x.ArticleId, x.Order })
                .IsUnique()
                .HasDatabaseName("IX_Section_ArticleId_Order");

            builder.HasIndex(x => x.ArticleId)
                .HasDatabaseName("IX_Section_ArticleId");
        }
    }
}
