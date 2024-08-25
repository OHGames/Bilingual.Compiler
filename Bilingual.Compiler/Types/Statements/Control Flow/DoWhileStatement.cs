using Bilingual.Compiler.Types.Expressions;
using Newtonsoft.Json;

namespace Bilingual.Compiler.Types.Statements.ControlFlow
{
    [JsonObject]
    public record class DoWhileStatement(Expression Expression, Block Block) : BlockedStatement(Block);
}
