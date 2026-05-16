using StructAPI.Domain.Enums;
using StructAPI.Domain.Exceptions;

namespace StructAPI.Domain
{
    public class KnowledgeEntry
    {
        public int Id { get; private set; }
        public string Content { get; private set; }
        public EntryStatus Status { get; private set; }
        public DateTimeOffset CreatedAt { get; private set; } = DateTimeOffset.UtcNow;
        public string  User { get; private set; }
        public int ReplacesEntryId { get; private set; }

        public KnowledgeEntry(string content, string user)
        {
            ValidateContent(content);

            this.Content = content;
            this.User = user;
            this.Status = EntryStatus.Active;
        }

        public static KnowledgeEntry CreateReplacement(string content, string user, int replacesEntryId)
        {
            if (replacesEntryId <= 0)
                throw new DomainException("ReplacesEntryId is invalid.");

            return new KnowledgeEntry(content, user)
            {
                ReplacesEntryId = replacesEntryId
            };
        }

        private void ValidateContent(string content)
        {
            if (string.IsNullOrWhiteSpace(content))
                throw new DomainException("Content cannot be empty.");
            if (content.Length < 10)
                throw new DomainException("Content must be at least 10 characters long.");
        }   

        public KnowledgeEntryLifecycleLog Deprecate(string reason, string user)
        {
            if (Status == EntryStatus.Deprecated)
                throw new DomainException("Already deprecated");

            var oldStatus = Status;
            Status = EntryStatus.Deprecated;

            return new KnowledgeEntryLifecycleLog(
                Id,
                oldStatus,
                Status,
                reason,
                user
            );
        }

        public void SetID(int id)
        {
            if (id <= 0) throw new DomainException("Id must be greater than zero.");
            this.Id = id;
        }
    }
}
