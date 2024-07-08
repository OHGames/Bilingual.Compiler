using Bilingual.Compiler.Types.Expressions;

namespace Bilingual.Compiler.Types.Statements.ControlFlow
{
    public record class ForEachStatement(Expression Item, Expression Collection, Block Block)
        : Statement;
}
