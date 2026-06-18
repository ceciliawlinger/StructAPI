namespace StructAPI.Service.IA
{
    public interface ISemanticSimilarityService
    {
        double CalculateSimilarity(float[] source, float[] target);

        Task<float[]> GenerateEmbeddingAsync(string content);
    }
}
