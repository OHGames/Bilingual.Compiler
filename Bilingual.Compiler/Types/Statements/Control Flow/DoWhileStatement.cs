using Bilingual.Compiler.Types.Expressions;

namespace Bilingual.Compiler.Types.Statements.ControlFlow
{
    public record class DoWhileStatement(Expression Expression, Block Block) : BlockedStatement(Block);
}
