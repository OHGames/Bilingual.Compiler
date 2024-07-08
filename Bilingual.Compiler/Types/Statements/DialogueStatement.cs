using Bilingual.Compiler.Types.Expressions;

namespace Bilingual.Compiler.Types.Statements
{
    public record class DialogueStatement(string Name, string? Emotion, Expression Dialogue,
        uint? LineId, string? TranslationComment)  : Statement;
}
