using StructAPI.Domain.Enums;
using StructAPI.Domain.Exceptions;
using Pgvector;

namespace StructAPI.Domain.Entities
{
    public class KnowledgeEntry
    {
        public Guid Id { get; private set; }
        public string Content { get; private set; }
        public EntryStatus Status { get; private set; }
        public DateTimeOffset CreatedAt { get; private set; } = DateTimeOffset.UtcNow;
        public string  User { get; private set; }
        public Guid? ReplacesEntryId { get; private set; }

        public Vector Embedding { get; private set; }

        public KnowledgeEntry(string content, string user, Vector embedding)
        {
            ValidateContent(content);
            ValidateUser(user);
            ValidateEmbedding(embedding);

            this.Id = Guid.NewGuid();
            this.Content = content;
            this.User = user;
            this.Status = EntryStatus.Active;
            this.CreatedAt = DateTimeOffset.UtcNow;
            this.Embedding = embedding;
        }

        private void ValidateEmbedding(Vector embedding)
        {
            if (embedding == null)
                throw new DomainException("Embedding cannot be null.");
        }

        private void ValidateUser(string user)
        {
            if (string.IsNullOrEmpty(user)) 
                throw new ArgumentNullException(nameof(user));
        }

        public static KnowledgeEntry CreateReplacement(string content, string user, Guid replacesEntryId, Vector embedding)
        {
            if (replacesEntryId == Guid.Empty)
                throw new DomainException("ReplacesEntryId is invalid.");

            return new KnowledgeEntry(content, user, embedding)
            {
                ReplacesEntryId = replacesEntryId
            };
        }

        public void SetStatus(EntryStatus status)
        {
            this.Status = status;
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

        public void SetID(Guid id)
        {
            if (id == Guid.Empty) throw new DomainException("Id must be a valid GUID.");
            this.Id = id;
        }
    }
}
