using Bilingual.Compiler.Types.Expressions;

namespace Bilingual.Compiler.Types.Statements.ControlFlow
{
    public record class WhileStatement(Expression Expression, Block Block) : Statement;
}
