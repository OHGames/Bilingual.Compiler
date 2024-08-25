using Bilingual.Compiler.Commands;
using Bilingual.Compiler.Types;
using Bilingual.Compiler.Types.Statements;
using Bilingual.Compiler.Types.Statements.ControlFlow;
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
                        for (int k = 0; k < script.StatementCount; k++)
                        {
                            var line = script[k];

                            if (line is BlockedStatement blocked)
                            {
                                AssignIdsToBlocked(blocked, scriptPath, biFile);
                                continue;
                            }
                            else if (line is ChooseStatement choose)
                            {
                                foreach (var chooseBlock in choose.Blocks)
                                {
                                    AssignIdsToBlocked(chooseBlock, scriptPath, biFile);
                                }
                                continue;
                            }
                            else if (line is not DialogueStatement) continue;

                            // assign ids to dialogue statements in script scope.
                            script[k] = AssignIds(line, scriptPath, biFile);
                        }
                    }
                }
            }

            Log("Done searching!");
            return parsedFiles;
        }

        /// <summary>Assign the dialogue lines ids to be added to files.</summary>
        /// <param name="line">The dialogue line.</param>
        /// <param name="scriptPath">The path to the script.</param>
        /// <param name="biFile">The bilingual file.</param>
        private DialogueStatement AssignIds(Statement line, string scriptPath, BilingualFile biFile)
        {
            var dialogue = (DialogueStatement)line;
            // id already exists and was already found by FindExistingFileLines()
            if (dialogue.LineId != null) return dialogue;

            // Get the new id and reassign the line in the parsed files.
            var nextId = LineIdManager.Generate(scriptPath);
            var newDialogue = dialogue with { LineId = nextId };

            if (LinesThatNeedIds.TryGetValue(biFile.FilePath, out List<DialogueStatement>? value))
            {
                value.Add(newDialogue);
            }
            else
            {
                LinesThatNeedIds.Add(biFile.FilePath, [newDialogue]);
            }
            return newDialogue;
        }

        /// <summary>Assign line ids to blocked statements.</summary>
        /// <param name="blocked">The blocked statement.</param>
        /// <param name="scriptPath">The script path.</param>
        /// <param name="biFile">The file.</param>
        private void AssignIdsToBlocked(BlockedStatement blocked, string scriptPath, BilingualFile biFile)
        {
            // assign ids to dialogue statemens in blocked scopes
            for (int stmtIndex = 0; stmtIndex < blocked.StatementCount; stmtIndex++)
            {
                var stmt = blocked[stmtIndex];
                if (stmt is BlockedStatement innerBlock)
                {
                    AssignIdsToBlocked(innerBlock, scriptPath, biFile);
                }
                else if (stmt is ChooseStatement choose)
                {
                    foreach (var chooseBlock in choose.Blocks)
                    {
                        AssignIdsToBlocked(chooseBlock, scriptPath, biFile);
                    }
                }
                else
                {
                    if (stmt is not DialogueStatement) continue;
                    blocked[stmtIndex] = AssignIds(stmt, scriptPath, biFile);
                }
            }

            if (blocked is IfStatement ifStatement)
            {
                // if there are else and if else blocks, assign those.
                // the main if block has already been assigned from code above.
                foreach (var ifElse in ifStatement.ElseIfStatements)
                {
                    for (int ifElseIndex = 0; ifElseIndex < ifElse.StatementCount; ifElseIndex++)
                    {
                        var statement = ifElse[ifElseIndex];
                        if (statement is BlockedStatement innerBlock)
                        {
                            AssignIdsToBlocked(innerBlock, scriptPath, biFile);
                        }
                        else if (statement is ChooseStatement choose)
                        {
                            foreach (var chooseBlock in choose.Blocks)
                            {
                                AssignIdsToBlocked(chooseBlock, scriptPath, biFile);
                            }
                        }
                        else
                        {
                            if (statement is not DialogueStatement) continue;
                            ifElse[ifElseIndex] = AssignIds(statement, scriptPath, biFile);
                        }
                    }
                }

                var elseStmt = ifStatement.ElseStatement;
                if (elseStmt is not null)
                {
                    for (int elseIndex = 0; elseIndex < elseStmt.StatementCount; elseIndex++)
                    {
                        var elseLine = elseStmt[elseIndex];
                        if (elseLine is BlockedStatement innerBlock)
                        {
                            AssignIdsToBlocked(innerBlock, scriptPath, biFile);
                        }
                        else if (elseLine is ChooseStatement choose)
                        {
                            foreach (var chooseBlock in choose.Blocks)
                            {
                                AssignIdsToBlocked(chooseBlock, scriptPath, biFile);
                            }
                        }
                        else
                        {
                            if (elseLine is not DialogueStatement) continue;
                            elseStmt[elseIndex] = AssignIds(elseLine, scriptPath, biFile);
                        }
                    }
                }
            }
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
                        var statements = new List<DialogueStatement>();
                        statements.AddRange(FindDialogueLines(script.Block.Statements));

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

        /// <summary>Locate all the dialogue statements in a list.</summary>
        /// <param name="statements">The statements.</param>
        /// <returns>A list of <see cref="DialogueStatement"/>.</returns>
        public static List<DialogueStatement> FindDialogueLines(List<Statement> statements)
        {
            List<DialogueStatement> foundStatements = [];
            foreach (var statement in statements)
            {
                if (statement is DialogueStatement dialogue) foundStatements.Add(dialogue);
                else if (statement is BlockedStatement blocked)
                {
                    foundStatements.AddRange(FindDialogueLines(blocked.Block.Statements));

                    if (blocked is IfStatement ifStatement)
                    {
                        foreach (var elseIf in ifStatement.ElseIfStatements)
                        {
                            foundStatements.AddRange(FindDialogueLines(elseIf.Block.Statements));
                        }

                        if (ifStatement.ElseStatement is not null)
                        {
                            foundStatements.AddRange(FindDialogueLines(ifStatement.ElseStatement.Block.Statements));
                        }
                    }
                }
                else if (statement is ChooseStatement choose)
                {
                    foreach (var block in choose.Blocks)
                    {
                        foundStatements.AddRange(FindDialogueLines(block.Block.Statements));
                    }
                }
            }
            return foundStatements;
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
