using Antlr4.Runtime;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Bilingual.Compiler
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var fileText = File.ReadAllText(args[0]);
            ICharStream stream = CharStreams.fromString(fileText);
            ITokenSource lexer = new BilingualLexer(stream);
            ITokenStream tokens = new CommonTokenStream(lexer);
            BilingualParser parser = new BilingualParser(tokens);
            BilingualVisitor visitor = new BilingualVisitor();

            var file = visitor.VisitFile(parser.file());

            var settings = new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented
            };
            settings.Converters.Add(new StringEnumConverter());

            var json = JsonConvert.SerializeObject(file, settings);
            File.WriteAllText(Path.Combine(Environment.CurrentDirectory, "file.json"), json);
        }
    }
}
