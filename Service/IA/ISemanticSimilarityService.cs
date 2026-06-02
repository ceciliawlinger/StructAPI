namespace StructAPI.Service.IA
{
    public interface ISemanticSimilarityService
    {
        double CalculateSimilarityAsync(float[] source, float[] target);

        Task<float[]> GenerateEmbeddingAsync(string content);
    }
}
