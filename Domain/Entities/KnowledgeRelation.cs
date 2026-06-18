using StructAPI.Domain.Enums;
using StructAPI.Domain.Exceptions;

namespace StructAPI.Domain.Entities
{
    public class KnowledgeRelation
    {
        public Guid Id { get; private set; } 
        public Guid SourceEntryId { get; }
        public Guid RelatedEntryId { get; }
        public KnowledgeRelationType RelationType { get; }
        public double Confidence { get; }
        public DateTimeOffset CreatedAt { get; private set; } = DateTimeOffset.UtcNow;

        public KnowledgeRelation(Guid sourceEntryId, Guid relatedEntryId, KnowledgeRelationType relationType, double confidence)
        {
            Id = Guid.NewGuid();

            if (sourceEntryId == Guid.Empty)
                throw new DomainException("Source entry ID must be a valid GUID.");
            SourceEntryId = sourceEntryId;

            if (relatedEntryId == Guid.Empty)
                throw new DomainException("Related entry ID must be a valid GUID.");
            RelatedEntryId = relatedEntryId;

            if (sourceEntryId == relatedEntryId)
                throw new DomainException("Source entry ID and related entry ID cannot be the same.");

            Confidence = confidence;
            RelationType = relationType;
        }
    }
}
