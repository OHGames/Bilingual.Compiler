using Bilingual.Compiler.Types.Containers;
using Newtonsoft.Json;

namespace Bilingual.Compiler.Types
{
    public record class BilingualFile(List<ScriptContainer> Containers) : BilingualObject
    {
        [JsonIgnore]
        public string FilePath { get; set; }
    }
}
