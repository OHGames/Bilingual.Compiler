using Bilingual.Compiler.Types.Expressions;

namespace Bilingual.Compiler.Types.Statements.ControlFlow
{
    public record class ChooseStatement(List<ChooseBlock> Blocks) : Statement;
    public record class ChooseBlock(Expression Option, Block Block)
        : BlockedStatement(Block);
}
