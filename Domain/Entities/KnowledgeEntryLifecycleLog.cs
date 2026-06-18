using System.Net.NetworkInformation;
using System.Reflection.Metadata;
using StructAPI.Domain.Enums;
using StructAPI.Domain.Exceptions;

namespace StructAPI.Domain.Entities
{
    public class KnowledgeEntryLifecycleLog
    {
        public Guid KnowledgeEntryId { get; private set; }
        public EntryStatus OldStatus { get; private set; }
        public EntryStatus NewStatus { get; private set; }
        public string Reason { get; private set; } = string.Empty;
        public DateTimeOffset OccurredAt { get; private set; } = DateTimeOffset.UtcNow;
        public string User { get; private set; } = string.Empty;
        

        public KnowledgeEntryLifecycleLog(Guid knowledgeEntryId, EntryStatus oldStatus, EntryStatus newStatus, string reason, string user)
        {
            if (string.IsNullOrWhiteSpace(reason))
                throw new DomainException("Reason cannot be empty");

            if (string.IsNullOrWhiteSpace(user))
                throw new DomainException("User cannot be empty");

            if (knowledgeEntryId == Guid.Empty)
                throw new DomainException("Knowledge entry ID must be a valid GUID.");

            if (oldStatus == newStatus)
                throw new DomainException("Old status and new status cannot be the same");

            this.Reason = reason;
            this.OldStatus = oldStatus;
            this.NewStatus = newStatus;
            this.User = user;
            this.KnowledgeEntryId = knowledgeEntryId;
        }
    }
}
