using Bilingual.Compiler.Types.Expressions;

namespace Bilingual.Compiler.Types.Statements
{
    public record class VariableDeclaration(string Name, bool Global, Expression Expression)
        : Statement;
}
