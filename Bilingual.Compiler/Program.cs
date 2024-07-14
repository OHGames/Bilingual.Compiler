using Bilingual.Compiler.Commands;
using Bilingual.Compiler.FileGeneration;
using CommandLine;

namespace Bilingual.Compiler
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var types = new Type[] { typeof(CompileVerb), typeof(LocalizeVerb), typeof(AddIdsVerb) };
            Parser.Default.ParseArguments(args, types)
                .WithParsed(Run)
                .WithNotParsed(CommandLineError);
        }

        public static void Run(object obj)
        {
            Log("-- Bilingual Compiler --\n");

            switch (obj)
            {
                case CompileVerb c:
                    Log("Compiling!");
                    new CompileFiles().RunCompileVerb(c);
                    break;
                case LocalizeVerb l:
                    // TODO: update files.
                    new Localizer(l).RunLocalizeVerb();
                    break;
                case AddIdsVerb a:
                    var fileAdd = new LineIdAdder(a);
                    _ = fileAdd.RunAddIdsVerb();
                    break;
                default:
                    throw new InvalidOperationException("Unknown command line verb.");
            }

            Log("\n-- Done! Thank you :) --");
        }

        public static void Log(string message, ConsoleColor bg = ConsoleColor.Black, 
            ConsoleColor fg = ConsoleColor.White)
        {
            Console.BackgroundColor = bg;
            Console.ForegroundColor = fg;
            Console.WriteLine(message);
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
        }

        public static void CommandLineError(IEnumerable<Error> exceptions)
        {
            Log("\n\nThere was an error reading the command line arguments. See above usage.\n\n", fg: ConsoleColor.DarkRed);
        }
    }
}
