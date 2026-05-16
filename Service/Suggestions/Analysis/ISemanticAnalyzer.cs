using StructAPI.Domain;
using StructAPI.Domain.Dtos;

namespace StructAPI.Service.Suggestions.Analysis
{
    public interface ISemanticAnalyzer
    {
        Task<SemanticAnalysis> AnalyzeAsync(string newContent, KnowledgeEntry target);
    }
}