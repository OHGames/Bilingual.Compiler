using Bilingual.Compiler.Commands;
using Bilingual.Compiler.Types;
using Bilingual.Compiler.Types.Statements;

using static Bilingual.Compiler.Program;

namespace Bilingual.Compiler.FileGeneration
{
    /// <summary>
    /// Find the ids and add them to the files.
    /// </summary>
    public class LineIdAdder(AddIdsVerb verb)
    {
        public AddIdsVerb verb = verb;

        /// <summary>String: file path, int: file line</summary>
        public Dictionary<string, List<DialogueStatement>> LinesThatNeedIds = [];

        /// <summary>Go through files and get their ids.</summary>
        public List<BilingualFile> GetIds()
        {
            Log("Looking for existing line ids....");
            if (!Directory.Exists(verb.Input)) throw new InvalidOperationException("Input directory does not exist");

            var parsedFiles = FindExistingFileLines();

            foreach (var biFile in parsedFiles)
            {
                Log($"\tLooking in {Path.GetFileName(biFile.FilePath)}", fg: ConsoleColor.Blue);
                // loop through every script and find dialogue lines.
                for (int i = 0; i < biFile.Containers.Count; i++)
                {
                    var container = biFile.Containers[i];
                    for (int j = 0; j < container.Scripts.Count; j++)
                    {
                        var script = container.Scripts[j];
                        var scriptPath = $"{container.Name}.{script.Name}";
                        for (int k = 0; k < script.Block.Statements.Count; k++)
                        {
                            var line = script.Block.Statements[k];
                            if (line is not DialogueStatement) continue;

                            var dialogue = (DialogueStatement)line;
                            // id already exists and was already found by FindExistingFileLines()
                            if (dialogue.LineId != null) continue;

                            // Get the new id and reassign the line in the parsed files.
                            var nextId = LineIdManager.Generate(scriptPath);
                            var newDialogue = dialogue with { LineId = nextId };
                            script.Block.Statements[k] = newDialogue;

                            if (LinesThatNeedIds.TryGetValue(biFile.FilePath, out List<DialogueStatement>? value))
                            {
                                value.Add(newDialogue);
                            }
                            else
                            {
                                LinesThatNeedIds.Add(biFile.FilePath, [newDialogue]);
                            }
                        }
                    }
                }
            }

            Log("Done searching!");
            return parsedFiles;
        }

        /// <summary>Find all the existing file lines by parsing the files.</summary>
        /// <returns>The parsed bilingual files.</returns>
        private List<BilingualFile> FindExistingFileLines()
        {
            var compiler = new CompileFiles();
            var files = Directory.GetFiles(verb.Input, "*.bi", SearchOption.AllDirectories);
            List<BilingualFile> parsedFiles = [];

            // Add all the existing ids to the list beforehand.
            foreach (var file in files)
            {
                var biFile = compiler.ParseFile(file);
                parsedFiles.Add(biFile);
                foreach (var container in biFile.Containers)
                {
                    foreach (var script in container.Scripts)
                    {
                        var statements = script.Block.Statements.Where(s => s is DialogueStatement)
                            .Cast<DialogueStatement>();
                        foreach (var statement in statements)
                        {
                            if (statement.LineId == null) continue;
                            LineIdManager.AddId(statement.LineId!.Value, $"{container.Name}.{script.Name}");
                        }
                    }
                }
            }
            return parsedFiles;
        }

        /// <summary>
        /// Update the files.
        /// </summary>
        public void AddLinesToFiles(List<BilingualFile> files)
        {
            if (LinesThatNeedIds.Count == 0) return;

            Log("\n\nAdding line ids to files that need them....");
            foreach (var filesAndLines in LinesThatNeedIds)
            {
                var filePath = filesAndLines.Key;
                var fileLines = File.ReadAllLines(filePath);
                Log($"\tAdding line ids to {Path.GetFileName(filePath)}", fg: ConsoleColor.Blue);

                for (int i = 0; i < filesAndLines.Value.Count; i++)
                {
                    var dialogueLine = filesAndLines.Value[i];
                    var idText = LineIdManager.Pad(dialogueLine.LineId!.Value);
                    var fileLine = dialogueLine.FileLine;

                    // TODO: line them up and make them pretty.
                    // right now its 40 spaces (about 8 tabs) of padding.
                    // 29 is total lenghth of padding + id.
                    fileLines[fileLine - 1] += $"#{idText}".PadLeft(49);
                }

                File.WriteAllLines(filePath, fileLines);
            }
            Log("Done adding ids!");
        }

        /// <summary>Add ids and find ids.</summary>
        /// <returns>Parsed bilingual files.</returns>
        public List<BilingualFile> RunAddIdsVerb()
        {
            var files = GetIds();
            AddLinesToFiles(files);
            return files;
        }

    }
}
