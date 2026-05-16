namespace StructAPI.Service.Knowledge
{
    [Serializable]
    internal class DuplicateKnowledgeEntryException : Exception
    {
        public DuplicateKnowledgeEntryException()
        {
        }

        public DuplicateKnowledgeEntryException(string? message) : base(message)
        {
        }

        public DuplicateKnowledgeEntryException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}