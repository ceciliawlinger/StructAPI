using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StructAPI.Domain.Entities;

namespace StructAPI.Infrastructure.Persistence.Configurations
{
    public class KnowledgeRelationConfiguration
    {
        public void Configure(EntityTypeBuilder<KnowledgeRelation> builder)
        {
            builder.ToTable("KnowledgeRelation");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.SourceEntryId)
                .IsRequired();

            builder.Property(x => x.RelatedEntryId)
                .IsRequired();

            builder.Property(x => x.RelationType)
                .IsRequired();

            builder.Property(x => x.Confidence)
                .IsRequired();

            builder.Property(x => x.CreatedAt)
                .IsRequired();

            builder.HasOne<KnowledgeEntry>()
                .WithMany()
                .HasForeignKey(x => x.SourceEntryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<KnowledgeEntry>()
                .WithMany()
                .HasForeignKey(x => x.RelatedEntryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(x => x.SourceEntryId);

            builder.HasIndex(x => x.RelatedEntryId);
            builder.HasIndex(x => new
            {
                x.SourceEntryId,
                x.RelationType
            });
            builder.HasIndex(x => new
            {
                x.SourceEntryId,
                x.RelatedEntryId,
                x.RelationType
            })
            .IsUnique();
        }
    }
}
