using Bilingual.Compiler.Types.Expressions;
using Newtonsoft.Json;

namespace Bilingual.Compiler.Types.Statements.ControlFlow
{
    [JsonObject]
    public record ForStatement(VariableDeclaration VariableDeclaration, Expression LoopCondition, 
        Expression AlterIndex, Block Block) : BlockedStatement(Block);
}
