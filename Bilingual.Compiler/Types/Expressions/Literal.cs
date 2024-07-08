namespace Bilingual.Compiler.Types.Expressions
{
    public record class Literal(object Value) : Expression
    {
        public bool IsDouble() => Value is double;
        public bool IsBool() => Value is bool;
    }
}
