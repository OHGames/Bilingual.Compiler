namespace Bilingual.Compiler.Types.Expressions
{
    public record class ArrayAccess(BilingualObject Object, Expression Indexer) : BilingualObject;
}
