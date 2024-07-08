using Bilingual.Compiler.Types.Expressions;

namespace Bilingual.Compiler.Types.Statements
{
    public record class VariableAssignment(Variable Name, Expression Expression) : Statement;
}
