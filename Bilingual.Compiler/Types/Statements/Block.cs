namespace Bilingual.Compiler.Types.Statements
{
    public record class Block(List<Statement> Statements) : BilingualObject;
}
