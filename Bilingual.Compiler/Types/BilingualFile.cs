using Bilingual.Compiler.Types.Containers;

namespace Bilingual.Compiler.Types
{
    public record class BilingualFile(List<ScriptContainer> Containers) : BilingualObject;
}
