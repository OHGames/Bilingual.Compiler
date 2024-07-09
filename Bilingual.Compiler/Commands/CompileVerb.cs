using CommandLine;

namespace Bilingual.Compiler.Commands
{
    [Verb("compile", HelpText = "Compile bilingual scripts.")]
    internal class CompileVerb
    {
        [Option('p', "pretty", Required = false, Default = false, HelpText = "If the compiled JSON should be pretty-printed." +
            "This option is disregarded if compiled output is in BSON.")]
        public bool Pretty { get; set; } = false;

        [Option('b', "bson", Required = false, Default = false, HelpText = "If the compiled output should be in BSON " +
            "(binary JSON)")]
        public bool Bson { get; set; } = false;

        [Option('c', "change-extrnsion", Default = false, Required = false, HelpText = "By default, the compiled output has " +
            "the file extension of '.json'. If this value is set to true, the compiled extension will be" +
            "'.bic'.")]
        public bool ChangeExtension { get; set; } = false;

        [Option('i', "input", Required = true, HelpText = "The input directory of uncompiled scripts. Files must end " +
            "in '.bi' to be considered.")]
        public string Input { get; set; }

        [Option('o', "output", Required = true, HelpText = "The out directory of compiled scripts. Note: the input " +
            "directory file structure will be mirrored.")]
        public string Output { get; set; }
    }
}
