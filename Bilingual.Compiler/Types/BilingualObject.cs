using System.Text.Json.Serialization;

namespace Bilingual.Compiler.Types
{
    public record class BilingualObject
    {
        /// <summary>This will add the C# type name to the JSON object.
        /// The JSON deserializer will have no idea what specific type
        /// some objects will be due to the polymorphism used in Bilingual.
        /// This property ensures the deserializer will find the right object.</summary>
        [JsonInclude]
        public string ObjectType => GetType().Name;
    }
}
