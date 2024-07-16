namespace Bilingual.Compiler.Types.Expressions
{
    public record class InterpolatedString(List<Expression> Expressions) : Expression;
}
