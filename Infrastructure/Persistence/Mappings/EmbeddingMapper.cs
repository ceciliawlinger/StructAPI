using Pgvector;

namespace StructAPI.Infrastructure.Persistence.Mappings
{
    public static class EmbeddingMapper
    {
        public static Vector ToVector(this float[] embedding)
        {
            return new Vector(embedding);
        }

        public static float[] ToFloatArray(this Vector vector)
        {
            return vector.ToArray();
        }
    }
}
