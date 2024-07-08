using System.Text.Json.Serialization;

namespace Bilingual.Compiler.Types
{
    public record class BilingualObject
    {
        [JsonInclude]
        public string ObjectType => GetType().Name;
    }
}
