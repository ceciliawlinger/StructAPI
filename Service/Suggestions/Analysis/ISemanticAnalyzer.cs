using StructAPI.Application.Dtos;
using StructAPI.Domain.Entities;

namespace StructAPI.Service.Suggestions.Analysis
{
    public interface ISemanticAnalyzer
    {
        Task<SemanticAnalysis> AnalyzeAsync(string newContent, KnowledgeEntry target);
    }
}