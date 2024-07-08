namespace Bilingual.Compiler.Types.Expressions
{
    public record class FunctionCallExpression(string Name, List<Accessor> Accessors, Params Params)
        : Expression;
}
