namespace Bilingual.Compiler.Types.Containers
{
    public record class ScriptContainer(string Name, List<Script> Scripts) 
        : BilingualObject;
}
