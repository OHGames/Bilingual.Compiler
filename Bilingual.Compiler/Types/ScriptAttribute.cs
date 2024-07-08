using Bilingual.Compiler.Types.Expressions;

namespace Bilingual.Compiler.Types
{
    public record class ScriptAttribute(string Name, Expression Value) : BilingualObject;
}
