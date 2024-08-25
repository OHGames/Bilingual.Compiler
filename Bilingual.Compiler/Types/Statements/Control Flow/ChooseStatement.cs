using Bilingual.Compiler.Types.Expressions;
using Newtonsoft.Json;

namespace Bilingual.Compiler.Types.Statements.ControlFlow
{
    public record class ChooseStatement(List<ChooseBlock> Blocks) : Statement;
    [JsonObject]
    public record class ChooseBlock(Expression Option, Block Block)
        : BlockedStatement(Block);
}
