using Bilingual.Compiler.Types.Expressions;

namespace Bilingual.Compiler.Types.Statements
{
    public record class FunctionCallStatement(FunctionCallExpression Expression) : Statement;
}
