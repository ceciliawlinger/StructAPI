using System.Net.NetworkInformation;
using System.Reflection.Metadata;
using StructAPI.Domain.Enums;
using StructAPI.Domain.Exceptions;

namespace StructAPI.Domain
{
    public class KnowledgeEntryLifecycleLog
    {
        public int KnowledgeEntryId { get; private set; }
        public EntryStatus OldStatus { get; private set; }
        public EntryStatus NewStatus { get; private set; }
        public string Reason { get; private set; } = string.Empty;
        public DateTimeOffset OccurredAt { get; private set; } = DateTimeOffset.UtcNow;
        public string User { get; private set; } = string.Empty;
        

        public KnowledgeEntryLifecycleLog(int knowledgeEntryId, EntryStatus oldStatus, EntryStatus newStatus, string reason, string user)
        {
            if (string.IsNullOrWhiteSpace(reason))
                throw new DomainException("Reason cannot be empty");

            if (string.IsNullOrWhiteSpace(user))
                throw new DomainException("User cannot be empty");

            if (knowledgeEntryId <= 0)
                throw new DomainException("Knowledge entry ID must be positive");

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
