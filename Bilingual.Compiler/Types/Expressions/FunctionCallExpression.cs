namespace Bilingual.Compiler.Types.Expressions
{
    public record class FunctionCallExpression(string Name, Params Params, bool Await) : Expression;
}
