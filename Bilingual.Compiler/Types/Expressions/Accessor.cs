namespace Bilingual.Compiler.Types.Expressions
{
    public record class Accessor(string MemberName, Expression? Indexer, Params? Params) : BilingualObject;
}
