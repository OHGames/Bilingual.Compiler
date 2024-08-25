using Bilingual.Compiler.Types.Expressions;
using Newtonsoft.Json;

namespace Bilingual.Compiler.Types.Statements.ControlFlow
{
    [JsonObject]
    public record class ForEachStatement(string Item, Expression Collection, Block Block)
        : BlockedStatement(Block);
}
