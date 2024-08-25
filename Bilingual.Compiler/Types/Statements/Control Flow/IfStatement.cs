using Bilingual.Compiler.Types.Expressions;
using Newtonsoft.Json;

namespace Bilingual.Compiler.Types.Statements.ControlFlow
{
    [JsonObject]
    public record class IfStatement(Expression Expression, Block Block,
        List<ElseIfStatement> ElseIfStatements, ElseStatement? ElseStatement) : BlockedStatement(Block);

    [JsonObject]
    public record class ElseIfStatement(Expression Expression, Block Block) : BlockedStatement(Block);
    [JsonObject]
    public record class ElseStatement(Block Block) : BlockedStatement(Block);
}
