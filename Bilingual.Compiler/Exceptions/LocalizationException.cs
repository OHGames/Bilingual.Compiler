namespace Bilingual.Compiler.Exceptions
{
    /// <summary>
    /// When the localize command has an error.
    /// </summary>
    [Serializable]
    public class LocalizationException : Exception
    {
        public LocalizationException() { }
        public LocalizationException(string message) : base(message) { }
        public LocalizationException(string message, Exception inner) : base(message, inner) { }
        protected LocalizationException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
