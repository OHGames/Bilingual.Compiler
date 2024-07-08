using Bilingual.Compiler.Types.Statements;

namespace Bilingual.Compiler.Types.Containers
{
    public record class Script(string Name, Block Block, List<ScriptAttribute> Attributes) : BilingualObject;
}
