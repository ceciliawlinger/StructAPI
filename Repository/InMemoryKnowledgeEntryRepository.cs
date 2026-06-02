using System.Text.RegularExpressions;
using StructAPI.Domain;
using StructAPI.Domain.Enums;
using StructAPI.Domain.Exceptions;

namespace StructAPI.Repository
{
    public class InMemoryKnowledgeEntryRepository : IKnowledgeEntryRepository
    {
        private readonly List<KnowledgeEntry> _entries = new();

        private int _nextId = 1;

        public async Task<KnowledgeEntry> CreateAsync(
            KnowledgeEntry entry)
        {
            if (entry is null)
                throw new ArgumentNullException(nameof(entry));

            entry.SetID(_nextId);

            _nextId++;

            _entries.Add(entry);

            return await Task.FromResult(entry);
        }

        public async Task<KnowledgeEntry> UpdateAsync(
            KnowledgeEntry entry)
        {
            var existingEntry = _entries
                .FirstOrDefault(x => x.Id == entry.Id);

            if (existingEntry is null)
                throw new DomainException("Entry not found.");

            return await Task.FromResult(existingEntry);
        }

        public async Task DeleteAsync(KnowledgeEntry entry)
        {
            if (entry is null)
                throw new ArgumentNullException(nameof(entry));

            _entries.Remove(entry);

            await Task.CompletedTask;
        }

        public async Task<KnowledgeEntry?> GetByIdAsync(int id)
        {
            var entry = _entries
                .FirstOrDefault(x => x.Id == id);

            return await Task.FromResult(entry);
        }

        public async Task<List<KnowledgeEntry>> GetAllAsync()
        {
            return await Task.FromResult(_entries.ToList());
        }

        public async Task<List<KnowledgeEntry>> GetByStatusAsync(
            EntryStatus status)
        {
            var entries = _entries
                .Where(x => x.Status == status)
                .ToList();

            return await Task.FromResult(entries);
        }

        public async Task<List<KnowledgeEntry>> SearchAsync(
            string content,
            int top)
        {
            if (string.IsNullOrWhiteSpace(content))
                return new List<KnowledgeEntry>();

            var searchTokens = Tokenize(content);

            var relatedEntries = new List<(KnowledgeEntry Entry, int Score)>();

            foreach (var entry in _entries
                         .Where(x => x.Status == EntryStatus.Active))
            {
                var entryTokens = Tokenize(entry.Content);

                var commonTokens = entryTokens
                    .Intersect(searchTokens)
                    .Count();

                if (commonTokens > 0)
                {
                    relatedEntries.Add((entry, commonTokens));
                }
            }

            var result = relatedEntries
                .OrderByDescending(x => x.Score)
                .Take(top)
                .Select(x => x.Entry)
                .ToList();

            return await Task.FromResult(result);
        }

        private List<string> Tokenize(string content)
        {
            content = Regex.Replace(
                content,
                @"[^\w\s]",
                "");

            return content
                .ToLower()
                .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .ToList();
        }
    }
}
