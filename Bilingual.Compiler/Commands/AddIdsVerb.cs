using CommandLine;

namespace Bilingual.Compiler.Commands
{
    [Verb("add-ids", HelpText = "Add line ids to .bi files.")]
    public class AddIdsVerb
    {
        [Option('i', "input", Required = true, HelpText = "Input directory to .bi files.")]
        public string Input { get; set; }
    }
}
