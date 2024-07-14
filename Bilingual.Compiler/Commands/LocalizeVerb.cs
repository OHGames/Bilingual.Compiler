using CommandLine;

namespace Bilingual.Compiler.Commands
{
    [Verb("localize", HelpText = "Generate localization files. This will add line ids.")]
    public class LocalizeVerb
    {
        [Option('i', "input", Required = true, HelpText = "Input directory for .bi files.")]
        public string Input { get; set; }

        [Option('o', "output", Required = false, HelpText = "The output directory of the localization file.")]
        public string Output { get; set; }

        [Option('p', "zip-path", HelpText = "If --update is true, this is the path to the zipped localization files.")]
        public string ZipPath { get; set; }

        [Option('u', "update", HelpText = "If true, a new file will not be created and an existing locale will be updated" +
            " with new ids and lines.", Default = false)]
        public bool Update { get; set; }

        [Option('l', "locale", Required = true, HelpText = "The locale to create or update. Must be a language" +
            "/region code. Example: en, en_US, es_ES, etc." +
            " See this link for a list of codes: https://saimana.com/list-of-country-locale-code/")]
        public string Locale { get; set; }

        [Option('z', "dont-zip", HelpText = "If the localizations should not be zipped up.")]
        public bool DontZip { get; set; } = false;

        [Option("force", HelpText = "If 'dont-zip' is true, the output directory + locale must be empty." +
            " If this is true, the files will be deleted without prompt from user. Otherwise, the user is asked.")]
        public bool Force { get; set; } = false;
    }
}
