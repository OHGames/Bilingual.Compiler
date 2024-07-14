using Bilingual.Compiler.Commands;
using Bilingual.Compiler.Extensions;
using Bilingual.Compiler.Types;
using Bilingual.Compiler.Types.Statements;
using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using System.IO.Compression;

using static Bilingual.Compiler.Program;

namespace Bilingual.Compiler.FileGeneration
{
    public class Localizer(LocalizeVerb verb)
    {
        public LocalizeVerb verb = verb;

        public CsvConfiguration csvConfiguration;

        /// <summary>Create or update localization files.</summary>
        /// <exception cref="InvalidOperationException">If the locale is empty.</exception>
        public void RunLocalizeVerb()
        {
            if (string.IsNullOrEmpty(verb.Locale)) 
                throw new InvalidOperationException("Must have a locale");

            // Now every single id is in the LineIdManager as well ad added to files.
            var idAdder = new LineIdAdder(new AddIdsVerb() { Input = verb.Input });
            var files = idAdder.RunAddIdsVerb();

            Log("\n\nGenerating localization file...");
            var temp = GenerateTempDirectory();
            csvConfiguration = new CsvConfiguration(CultureInfo.InvariantCulture);

            if (verb.Update)
            {
                UpdateExistingLocalizationFiles(files, temp);
            }
            else
            {
                CreateNewLocalizationFiles(temp, files);
            }
        }

        /// <summary>Update the translation files.</summary>
        /// <param name="files">The parsed files.</param>
        /// <param name="temp">The temp directory.</param>
        private void UpdateExistingLocalizationFiles(List<BilingualFile> files, string temp)
        {
            if (!File.Exists(verb.ZipPath)) 
                throw new InvalidOperationException("Translation file does not exist.");

            ZipFile.ExtractToDirectory(verb.ZipPath, temp);
            var extractedPath = temp;

            var indexFile = File.ReadAllLines(Path.Combine(extractedPath, "ids.index"));
            List<uint> ids = [];
            Dictionary<uint, string> idsAndScriptPaths = [];

            // read lines and cast to uints.
            for (int i = 0; i < indexFile.Length; i++)
            {
                // every line looks like this:
                // 00000001 This.Is.A.Path.To.The.Script

                var split = indexFile[i].Split(' ');
                var id = uint.Parse(split.First());
                var path = split.Last();

                ids.Add(id);
                idsAndScriptPaths.Add(id, path);
            }

            // find the difference between the ids.
            var added = ids.GetAdded(LineIdManager.LineIds);
            var removed = ids.GetRemoved(LineIdManager.LineIds);

            if (added.Count != 0)
                AddNewDialogueLines(files, extractedPath, added);
            if (removed.Count != 0)
                RemoveOldDialogueLines(idsAndScriptPaths, removed, extractedPath);

            // dont check if added or removed has changed because the order
            // may have changed even if no lines were added or deleted.
            UpdateCsvLineNumbers(files, extractedPath);

            ZipFolder(extractedPath);
        }

        /// <summary>Update the file line value in the csv to the new line numbers.</summary>
        private void UpdateCsvLineNumbers(List<BilingualFile> files, string extractedPath)
        {
            // update file lines in csv
            for (int i = 0; i < files.Count; i++)
            {
                var biFile = files[i];
                for (int j = 0; j < biFile.Containers.Count; j++)
                {
                    var container = biFile.Containers[j];
                    for (int k = 0; k < container.Scripts.Count; k++)
                    {
                        var script = container.Scripts[k];
                        var csvPath = Path.Combine(extractedPath, container.Name.Replace('.', '/'), script.Name + ".csv");
                        var csvLines = File.ReadAllLines(csvPath);
                        var dialogueStatements = script.Block.Statements.Where(s => s is DialogueStatement)
                            .Cast<DialogueStatement>();
                        var newLines = new List<string>(csvLines);
                        bool fileAltered = false;

                        // skip header so start at 1.
                        for (int l = 1; l < csvLines.Length; l++)
                        {
                            // The csv always ends with the line number
                            // The csv always starts with the 8 digit line id.
                            var line = csvLines[l];
                            var lineId = uint.Parse(line[..8]);
                            var split = line.Split(',');
                            var lineNumber = int.Parse(split.Last());

                            var dialogueLine = dialogueStatements.Where(d => d.LineId == lineId).First();
                            if (dialogueLine.FileLine != lineNumber)
                            {
                                var newString = "";
                                // skip the last segment, it is the number we want to change.
                                for (int m = 0; m < split.Length - 1; m++)
                                {
                                    // put the line back together again
                                    newString += split[m] + ",";
                                }
                                newString += dialogueLine.FileLine.ToString();
                                newLines[l] = newString;
                                fileAltered = true;
                            }
                        }

                        if (fileAltered)
                            File.WriteAllLines(csvPath, newLines);
                    }
                }
            }
        }

        /// <summary>Remove the old dialogue lines from the files.</summary>
        private void RemoveOldDialogueLines(Dictionary<uint, string> idsAndScriptPaths, List<uint> removed, 
            string extractedPath)
        {
            var indexFilePath = Path.Combine(extractedPath, "ids.index");
            var indexFileLines = File.ReadAllLines(indexFilePath).ToList();
            var newIdLines = new List<string>(indexFileLines);

            foreach (var line in indexFileLines)
            {
                if (removed.Contains(uint.Parse(line.Split(' ').First())))
                {
                    newIdLines.Remove(line);
                }
            }

            // update index file
            File.WriteAllLines(indexFilePath, newIdLines);

            foreach (var keyValuePair in idsAndScriptPaths)
            {
                var id = keyValuePair.Key;
                if (!removed.Contains(id)) continue;

                // the file name is in the path already
                var scriptPath = keyValuePair.Value.Replace('.', '/');
                var csvFilePath = Path.Combine(extractedPath, scriptPath + ".csv");
                if (!File.Exists(csvFilePath)) throw new InvalidOperationException($"{csvFilePath} does not exist.");

                var lines = File.ReadAllLines(csvFilePath);
                List<string> newLines = new List<string>(lines);
                // start at index 1 to skip the csv header
                for (int i = 1; i < lines.Length; i++)
                {
                    var line = lines[i];
                    // csv starts with the id.
                    if (line.StartsWith(id.ToString("D8")))
                    {
                        newLines.Remove(line);
                    }
                }

                // no change
                if (lines.Length == newLines.Count) continue;

                File.WriteAllLines(csvFilePath, newLines);
            }
        }

        /// <summary>Append the new lines to csv files.</summary>
        private void AddNewDialogueLines(List<BilingualFile> files, string extractedPath, 
            List<uint> added)
        {
            var indexFilePath = Path.Combine(extractedPath, "ids.index");
            using var appendWriter = File.AppendText(indexFilePath);

            // Add the new lines to the file.
            for (int i = 0; i < files.Count; i++)
            {
                var biFile = files[i];
                for (int j = 0; j < biFile.Containers.Count; j++)
                {
                    var container = biFile.Containers[j];
                    for (int k = 0; k < container.Scripts.Count; k++)
                    {
                        var script = container.Scripts[k];
                        // get only the lines added.
                        var dialogueLines = script.Block.Statements
                            .Where(s => s is DialogueStatement).Cast<DialogueStatement>()
                            .Where(d => added.Contains(d.LineId!.Value)).ToList();

                        if (dialogueLines.Count == 0) continue;

                        foreach (var line in dialogueLines)
                        {
                            // add to id file.
                            appendWriter.WriteLine($"{line.LineId:D8} {container.Name}.{script.Name}");
                        }

                        // add new line.
                        var csvPath = Path.Combine(extractedPath, container.Name.Replace('.', '/'), script.Name + ".csv");
                        using var stream = File.Open(csvPath, FileMode.Append);
                        using var writer = new StreamWriter(stream);
                        using var csv = new CsvWriter(writer, 
                            new CsvConfiguration(CultureInfo.InvariantCulture) { HasHeaderRecord = false });
                        csv.Context.RegisterClassMap<CsvMap>();

                        csv.WriteRecords(dialogueLines);

                        // writer will flush and save file when scope is over.
                    }
                }
            }
        }

        /// <summary>Create the zip file or uncompressed folders in the temp directory then save to output.</summary>
        /// <param name="temp">The temp directory.</param>
        /// <param name="files">The bilingual files to serialize.</param>
        private void CreateNewLocalizationFiles(string temp, List<BilingualFile> files)
        {
            foreach (var file in files)
            {
                Log("\tLocalizing ");
                GenerateCsv(file, temp);
            }

            // create index file
            var idLines = LineIdManager.LineIds.Select(LineIdManager.Pad).ToList();
            List<string> idLinesAndPath = [];
            idLines.ForEach(s => idLinesAndPath.Add(s + " " + LineIdManager.LineIdsAndScriptPath[uint.Parse(s)]));
            File.WriteAllLines(Path.Combine(temp, "ids.index"), idLinesAndPath);

            if (!verb.DontZip)
                ZipFolder(temp);
            else
            {
                var newPath = Path.Combine(verb.Output, verb.Locale);

                // force delete
                if (Directory.Exists(newPath) && verb.Force)
                {
                    Directory.Delete(newPath, true);
                }

                // move() requires the path not to exist.
                if (Directory.Exists(newPath) && Directory.GetFiles(newPath).Length != 0 && !verb.Force)
                {
                    Log($"\n\nWARNING! There are files inside the {newPath} directory! \nThe directory must be empty " +
                        $"in order to create an unzipped version of the localization files. " +
                        $"\nTo prevent this from showing again and to always delete the folder, use the --force flag." +
                        $"\nTo delete the files and move on, type either y or yes. To cancel, type n or no. ",
                        fg: ConsoleColor.Red);
                    var input = Console.ReadLine();

                    if (input.ToLowerInvariant() == "y" || input.ToLowerInvariant() == "yes")
                    {
                        Directory.Delete(newPath, true);
                    }
                    else if (input.ToLowerInvariant() == "n" || input.ToLowerInvariant() == "no")
                    {
                        Directory.Delete(temp, true);
                        Log("Localizing cancelled. Note: line ids have been added to the files, but nothing was deleted.",
                            fg: ConsoleColor.Green);
                        return;
                    }
                    else
                    {
                        Log("Unrecognized response, nothing was deleted. Note: line ids have been added though.",
                            fg: ConsoleColor.Yellow);
                        return;
                    }
                }

                // move to new directory when clear.
                Directory.Move(temp, newPath);
            }
        }

        /// <summary>Zip up the temp folder, save in output dir, and delete the temp.</summary>
        /// <param name="tempDirectory">The directory of the temp folder.</param>
        private void ZipFolder(string tempDirectory)
        {
            var archiveFilePath = verb.Update ? verb.ZipPath : Path.Combine(verb.Output, verb.Locale + ".zip");

            // GetDirectoryName gets rid of the file name.
            Directory.CreateDirectory(Path.GetDirectoryName(archiveFilePath));
            if (File.Exists(archiveFilePath)) File.Delete(archiveFilePath);
            ZipFile.CreateFromDirectory(tempDirectory, archiveFilePath, CompressionLevel.Optimal, false);

            Directory.Delete(tempDirectory, true);
        }

        /// <summary>Create the csv file</summary>
        /// <param name="file">The file to serialze.</param>
        /// <param name="tempDirectory">The directory of the temp folder.</param>
        private void GenerateCsv(BilingualFile file, string tempDirectory)
        {
            foreach (var container in file.Containers)
            {
                foreach (var script in container.Scripts)
                {
                    if (!script.Block.Statements.Where(s => s is DialogueStatement).Any())
                        continue;

                    // Containers act like C# namespaces so seperate each part into its own directory.
                    // This.is.a.container => This/is/a/container
                    var csvPath = Path.Combine(tempDirectory, container.Name.Replace('.', '/'));
                    Directory.CreateDirectory(csvPath);

                    csvPath = Path.Combine(csvPath, script.Name + ".csv");

                    // create the writer using a class mapper to properly serialize
                    using var writer = new StreamWriter(csvPath);
                    using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
                    csv.Context.RegisterClassMap<CsvMap>();

                    // next record must be called manually after header.
                    csv.WriteHeader<DialogueStatement>();
                    csv.NextRecord();

                    foreach (var dialogue in script.Block.Statements.Where(s => s is DialogueStatement))
                    {
                        csv.WriteRecord((DialogueStatement)dialogue);
                        csv.NextRecord();
                    }

                    // once scope is over, csv will automatically flush and save.
                }
            }
        }

        /// <summary>Makes a temp directory in the system temp path.</summary>
        /// <returns>The path to the temp dir.</returns>
        private string GenerateTempDirectory()
        {
            var path = Path.Combine(Path.GetTempPath(), verb.Locale);
            Directory.CreateDirectory(path);
            return path;
        }
    }
}
