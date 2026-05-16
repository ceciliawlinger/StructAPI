using System.ComponentModel;
using System.Text.RegularExpressions;
using StructAPI.Domain;
using StructAPI.Domain.Dtos;
using StructAPI.Domain.Enums;
using StructAPI.Domain.Exceptions;
using StructAPI.Repository;
using static System.Net.Mime.MediaTypeNames;

namespace StructAPI.Tests
{
    public class InMemoryKnowledgeEntryRepository : IKnowledgeEntryRepository
    {
        private readonly List<KnowledgeEntry> _entries = new();
        private readonly List<KnowledgeEntryLifecycleLog> _logs = new();
        private int _nextId = 1;


        public async Task<KnowledgeEntry> CreateAsync(KnowledgeEntry request)
        {
            if (request is null) throw new ArgumentNullException(nameof(request));
            if (string.IsNullOrEmpty(request.Content)) 
                throw new DomainException("Content cannot be empty.");
            
            KnowledgeEntry newEntry = new KnowledgeEntry(request.Content, request.User);
            newEntry.SetID(_nextId);
            _nextId++;
            _entries.Add(newEntry);

            return newEntry;
        }

        public async Task DeleteAsync(KnowledgeEntry entry)
        {
            if (entry is null) throw new ArgumentNullException(nameof(entry));
            _entries.Remove(entry);
            await Task.CompletedTask;
        }

        public async Task<List<KnowledgeEntry>> GetAllAsync()
        {
            return await Task.FromResult(_entries.ToList());
        }

        public Task<KnowledgeEntry?> GetByIdAsync(int id)
        {
            if (id <= 0) throw new DomainException("Id must be greater than zero.");
            var entry = _entries.FirstOrDefault(e => e.Id == id);
            return Task.FromResult(entry);
        }

        public Task<List<KnowledgeEntry>> GetByStatusAsync(EntryStatus status)
        {
            if (!Enum.IsDefined(typeof(EntryStatus), status))
                throw new DomainException("Invalid entry status.");

            var entries = _entries.Where(e => e.Status == status).ToList();
            return Task.FromResult(entries);
        }

        public async Task<KnowledgeEntry> ReplaceAsync(int entryId, string newContent, string user)
        {
            var existingEntry = await GetByIdAsync(entryId);

            if (existingEntry is null)
                throw new DomainException("Entry not found.");

            var newEntry = KnowledgeEntry.CreateReplacement(
                newContent,
                user,
                existingEntry.Id
            );

            var log = existingEntry.Deprecate(
                "Replaced by newer entry.",
                user
            );

            await CreateAsync(newEntry);
            await UpdateAsync(existingEntry);
            return newEntry;
        }

        public async Task<List<KnowledgeEntry>> SearchAsync(string content, int top)
        {
            if (string.IsNullOrEmpty(content))
                throw new DomainException("Content cannot be empty.");

            var contentTokens = TokenizeContent(content);

            var entries = await GetAllAsync();
            if (entries is null || entries.Count == 0)
                return new List<KnowledgeEntry>();

            var relatedEntries = new List<(KnowledgeEntry Entry, int Score)>();

            foreach (var entry in entries.Where(e => e.Status == EntryStatus.Active))
            {
                var tokenizedEntry = TokenizeContent(entry.Content);
                if (tokenizedEntry.Count == 0)
                    continue;
                var commonCount = tokenizedEntry.Intersect(contentTokens).Count();
                if (commonCount > 0)
                {
                    relatedEntries.Add((entry, commonCount));
                } 
            }

            if (relatedEntries.Count > 0)
            {
                return relatedEntries
                    .OrderByDescending(x => x.Score)
                    .Take(top)
                    .Select(x => x.Entry)
                    .ToList();
            }
            return new List<KnowledgeEntry>();
        }

        private List<string> TokenizeContent(string content)
        {
            List<string> contentTokens = new List<string>();
            content = Regex.Replace(content, @"[^\w\s]", "");
            return content.ToLower().Split(' ', StringSplitOptions.RemoveEmptyEntries).ToList();
        }

        public Task<KnowledgeEntry> UpdateAsync(KnowledgeEntry entry)
        {
            var existingEntry = _entries.FirstOrDefault(e => e.Id == entry.Id);

            if (existingEntry is null)
                throw new DomainException("Entry not found.");

            return Task.FromResult(existingEntry);
        }
    }
}
