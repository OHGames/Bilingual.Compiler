using Bilingual.Compiler.Types.Expressions;
using Newtonsoft.Json;

namespace Bilingual.Compiler.Types.Statements
{
    public record class DialogueStatement(string Name, string? Emotion, Expression Dialogue,
        uint? LineId, string? TranslationComment) : Statement
    {
        /// <summary>For use in localizing.</summary>
        [JsonIgnore]
        public int FileLine { get; init; }
    };
}
