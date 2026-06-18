using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StructAPI.Domain.Entities;

namespace StructAPI.Infrastructure.Persistence.Configurations
{
    public class KnowledgeEntryLifecycleLogConfiguration : IEntityTypeConfiguration<KnowledgeEntryLifecycleLog>
    {
        public void Configure(EntityTypeBuilder<KnowledgeEntryLifecycleLog> builder)
        {
            builder.HasKey(k => new { k.KnowledgeEntryId, k.OccurredAt });
            builder.Property(k => k.OldStatus)
                .HasConversion<string>()
                .IsRequired();
            builder.Property(k => k.NewStatus)
                .HasConversion<string>()
                .IsRequired();
            builder.Property(k => k.Reason)
                .HasMaxLength(500);
            builder.Property(k => k.User)
                .HasMaxLength(200)
                .IsRequired();
            builder.Property(k => k.OccurredAt)
                .IsRequired();
            builder.HasIndex(k => k.OldStatus);
            builder.HasIndex(k => k.NewStatus);
            builder.HasIndex(k => k.OccurredAt);
        }
    }
}
