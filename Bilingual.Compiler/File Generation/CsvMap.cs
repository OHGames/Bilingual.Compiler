using Bilingual.Compiler.Types;
using Bilingual.Compiler.Types.Expressions;
using Bilingual.Compiler.Types.Statements;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

namespace Bilingual.Compiler.FileGeneration
{
    /// <summary>
    /// Map the statement to a csv file when serializing.
    /// </summary>
    public class CsvMap : ClassMap<DialogueStatement>
    {
        public CsvMap()
        {
            Map(m => m.LineId).Index(0).Name("LineId").TypeConverter(new UintToStringConverter());
            Map(m => m.Name).Index(1).Name("Name");
            Map(m => m.Dialogue).Index(2).Name("Dialogue").TypeConverter(new StringExpressionToString());
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

    public class StringExpressionToString : ITypeConverter
    {
        public object? ConvertFromString(string? text, IReaderRow row, MemberMapData memberMapData)
        {
            throw new NotImplementedException();
        }

        public string? ConvertToString(object? value, IWriterRow row, MemberMapData memberMapData)
        {
            if (value is Literal literal)
            {
                return literal.Value.ToString();
            }
            else if (value is InterpolatedString interpolated)
            {
                var str = "";
                for (int i = 0; i < interpolated.Expressions.Count; i++)
                {
                    var expr = interpolated.Expressions[i];
                    if (expr is Literal lit)
                    {
                        str += lit.Value.ToString();
                    }
                    else if (expr is LocalizedQuanity quanity)
                    {
                        str += $"={{{i} ";
                        str += quanity.Cardinal ? "pl " : "ord ";
                        foreach (var plural in quanity.Plurals)
                        {
                            str += $"{plural.Key.ToString().ToLower()}='{plural.Value}', ";
                        }

                        // Get rid of the last space and comma.
                        str = str[..^2];
                        str += "}=";
                        return str;
                    }
                    else
                    {
                        str += $"=[{i}]=";
                    }
                }

                return str;
            }

            throw new InvalidOperationException("Unrecognized type for dialogue string.");
        }
    }
}
