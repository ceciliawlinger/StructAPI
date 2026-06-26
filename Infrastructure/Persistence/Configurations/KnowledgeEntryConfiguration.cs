using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StructAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Text.Json;

namespace StructAPI.Infrastructure.Persistence.Configurations
{
    public class KnowledgeEntryConfiguration : IEntityTypeConfiguration<KnowledgeEntry>
    {
        public void Configure(EntityTypeBuilder<KnowledgeEntry> builder)
        {
            builder.ToTable("KnowledgeEntries");
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).ValueGeneratedNever();
            builder.Property(e => e.Content).IsRequired();
            builder.Property(e => e.User).HasMaxLength(200).IsRequired();
            builder.Property(e => e.CreatedAt).IsRequired();
            builder.Property(e => e.Status).HasConversion<string>().IsRequired();
            builder.Property(e => e.Embedding)
                .HasColumnType("vector(1536)")
                .IsRequired();
            builder.Property(e => e.ReplacesEntryId).IsRequired(false);
            builder.HasIndex(e => e.Status);
            builder.HasIndex(e => e.CreatedAt);
        }
    }
}
