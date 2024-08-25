using Bilingual.Compiler.Types.Expressions;

namespace Bilingual.Compiler.Types.Statements.ControlFlow
{
    public record ForStatement(VariableDeclaration VariableDeclaration, Expression LoopCondition, 
        Expression AlterIndex, Block Block) : BlockedStatement(Block);
}
