using Bilingual.Compiler.Types.Expressions;
using Newtonsoft.Json;

namespace Bilingual.Compiler.Types.Statements.ControlFlow
{
    [JsonObject]
    public record class WhileStatement(Expression Expression, Block Block) : BlockedStatement(Block);
}
