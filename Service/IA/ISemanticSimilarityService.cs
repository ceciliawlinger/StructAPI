namespace StructAPI.Service.IA
{
    public interface ISemanticSimilarityService
    {
        Task<double> CalculateSimilarityAsync(
        string source,
        string target);
    }
}
