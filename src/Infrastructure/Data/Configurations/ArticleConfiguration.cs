using AuthService.Domain.Entities;
using AuthService.Domain.Enum;
using Infrastructure.Data.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthService.Infrastructure.Data.Configurations
{
    public class ArticleConfiguration : BaseEntityConfiguration<Article, Guid>
    {
        public override void Configure(EntityTypeBuilder<Article> builder)
        {
            base.Configure(builder);

            builder.ToTable("Article");

            builder.Property(x => x.Id)
                .ValueGeneratedNever();

            builder.Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(200)
                .HasColumnName("Title");

            builder.Property(x => x.AuthorEmail)
               .IsRequired()
               .HasMaxLength(128)
               .HasColumnName("AuthorEmail");

            builder.Property(x => x.Status)
                .IsRequired()
                .HasMaxLength(50)
                .HasConversion<string>() 
                .HasColumnName("Status");

            builder.Property(x => x.PublishedAt)
                .IsRequired(false)
                .HasColumnName("PublishedAt");

            builder.HasIndex(x => x.AuthorEmail)
                .HasDatabaseName("IX_Article_AuthorEmail");

            builder.HasIndex(x => x.Status)
                .HasDatabaseName("IX_Article_Status");

            builder.HasIndex(x => new { x.AuthorEmail, x.Status })
                .HasDatabaseName("IX_Article_AuthorEmail_Status");

            builder.HasMany(x => x.Sections)
                .WithOne()
                .HasForeignKey(x => x.ArticleId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
