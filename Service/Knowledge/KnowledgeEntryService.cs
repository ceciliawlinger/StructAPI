using System.ComponentModel;
using System.Diagnostics;
using System.Text.RegularExpressions;
using StructAPI.Domain;
using StructAPI.Domain.Dtos;
using StructAPI.Domain.Enums;
using StructAPI.Domain.Exceptions;
using StructAPI.Repository;
using StructAPI.Service.Suggestions.Analysis;

namespace StructAPI.Service.Knowledge
{
    public class KnowledgeEntryService
    {
        private readonly IKnowledgeEntryRepository _repository;
        private readonly ISemanticAnalyzer _analyzer;
        private readonly SemanticMatchService _matchService;

        public KnowledgeEntryService(IKnowledgeEntryRepository repository, ISemanticAnalyzer analyzer, SemanticMatchService matchService)
        {
            if (repository == null) throw new ArgumentNullException("Repository cannot be null");
            _repository = repository;

            if (analyzer == null) throw new ArgumentNullException("Analyzer cannot be null");
            _analyzer = analyzer;

            if (matchService == null) throw new ArgumentNullException("Match service cannot be null");
            _matchService = matchService;
        }
        public async Task<KnowledgeEntryResponse> CreateKnowledgeEntry(CreateKnowledgeEntryRequest request) 
        {
            KnowledgeEntry entry = new KnowledgeEntry(request.Content, request.User);
            var matches = await _matchService.FindMatchesAsync(request.Content);

            if (matches.Any(x => x.Analysis.IsRedundant))
                throw new DuplicateKnowledgeEntryException();

            await _repository.CreateAsync(entry);
            return new KnowledgeEntryResponse(entry);
        }

        public async Task<KnowledgeEntryResponse> ReplaceKnowledgeEntry(ReplaceKnowledgeEntryRequest request) 
        {
           var existingEntry = await _repository.GetByIdAsync(request.OldEntryID);
            if (existingEntry == null)
                throw new DomainException("Entry to be replaced not found.");

            var matches = await _matchService.FindMatchesAsync(request.Content);

            if (matches.Any(x => x.Analysis.IsRedundant))
                throw new DuplicateKnowledgeEntryException();

            var newEntry = KnowledgeEntry.CreateReplacement( // Always create a new one and deprecate the old one.
                 request.Content,
                 request.User,
                 request.OldEntryID
             );

            var log = existingEntry.Deprecate("Replaced by new entry", request.User);
            await _repository.CreateAsync(newEntry);
            await _repository.UpdateAsync(existingEntry);
            return new KnowledgeEntryResponse(newEntry);
        }
            
        public async void DeleteKnowledgeEntry(int entryId) 
        {
            var existingEntry = await _repository.GetByIdAsync(entryId);
            if (existingEntry == null)
                throw new DomainException("Entry not found.");
            await _repository.DeleteAsync(existingEntry);
        }
    }
}
