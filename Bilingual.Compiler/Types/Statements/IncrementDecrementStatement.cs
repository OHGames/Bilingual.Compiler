using Bilingual.Compiler.Types.Expressions;

namespace Bilingual.Compiler.Types.Statements
{
    public record class IncrementDecrementStatement(OprExpression Expression) : Statement;
}
