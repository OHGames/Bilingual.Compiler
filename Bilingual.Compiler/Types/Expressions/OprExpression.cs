namespace Bilingual.Compiler.Types.Expressions
{
    public record class OprExpression(Expression? Left, Operator Operator, Expression? Right)
        : Expression;
}
