using Bilingual.Compiler.Types.Containers;
using Newtonsoft.Json;

namespace Bilingual.Compiler.Types
{
    public record class BilingualFile(List<ScriptContainer> Containers) : BilingualObject
    {
        /// <summary>For use in localizing.</summary>
        [JsonIgnore]
        public string FilePath { get; set; }
    }
}
