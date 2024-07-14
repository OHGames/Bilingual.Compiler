using Bilingual.Compiler.Types.Expressions;
using Bilingual.Compiler.Types.Statements;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

namespace Bilingual.Compiler.FileGeneration
{
    public class CsvMap : ClassMap<DialogueStatement>
    {
        public CsvMap()
        {
            Map(m => m.LineId).Index(0).Name("LineId").TypeConverter(new UintToStringConverter());
            Map(m => m.Name).Index(1).Name("Name");
            Map(m => m.Dialogue).Index(2).Name("Dialogue").TypeConverter(new LiteralToValue());
            Map(m => m.Emotion).Index(3).Name("Emotion");
            Map(m => m.TranslationComment).Index(4).Name("TranslationComment");
            Map(m => m.FileLine).Index(5).Name("FileLine");
        }
    }

    public class UintToStringConverter : ITypeConverter
    {
        public object? ConvertFromString(string? text, IReaderRow row, MemberMapData memberMapData)
        {
            throw new NotImplementedException();
        }

        public string? ConvertToString(object? value, IWriterRow row, MemberMapData memberMapData)
        {
            return ((uint)value!).ToString("D8");
        }
    }

    public class LiteralToValue : ITypeConverter
    {
        public object? ConvertFromString(string? text, IReaderRow row, MemberMapData memberMapData)
        {
            throw new NotImplementedException();
        }

        public string? ConvertToString(object? value, IWriterRow row, MemberMapData memberMapData)
        {
            // TODO: save expressions
            return ((Literal)value!).Value.ToString();
        }
    }
}
