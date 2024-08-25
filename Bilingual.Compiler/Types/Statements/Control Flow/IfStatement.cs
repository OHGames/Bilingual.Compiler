using Bilingual.Compiler.Types.Expressions;

namespace Bilingual.Compiler.Types.Statements.ControlFlow
{
    public record class IfStatement(Expression Expression, Block Block,
        List<ElseIfStatement> ElseIfStatements, ElseStatement? ElseStatement) : BlockedStatement(Block);

    public record class ElseIfStatement(Expression Expression, Block Block) : BlockedStatement(Block);
    public record class ElseStatement(Block Block) : BlockedStatement(Block);
}
