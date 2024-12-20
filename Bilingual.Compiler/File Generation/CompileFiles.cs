﻿using Antlr4.Runtime;
using Bilingual.Compiler.Commands;
using Bilingual.Compiler.Exceptions;
using Bilingual.Compiler.Types;
using Newtonsoft.Json.Bson;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;

// for Log();
using static Bilingual.Compiler.Program;

namespace Bilingual.Compiler.FileGeneration
{
    public class CompileFiles
    {
        /// <summary>
        /// Start to compile
        /// </summary>
        public void RunCompileVerb(CompileVerb verb)
        {
            var path = verb.Input;
            if (!Directory.Exists(path)) throw new Exception("Input directory doesn't exist.");

            var files = Directory.GetFiles(path, "*.bi", SearchOption.AllDirectories);
            if (files.Length == 0)
            {
                Log($"No .bi files in '{Path.GetFullPath(path)}'!", fg: ConsoleColor.Yellow);
                return;
            }

            Dictionary<string, string> invalidSyntaxFiles = [];

            foreach (var file in files)
            {
                try
                {
                    CompileFile(file, verb);
                }
                catch (BilingualParsingException parse)
                {
                    Log($"\t{Path.GetFileName(file)} had a parsing error! Moving on to next file...", fg: ConsoleColor.Red);
                    Log($"\t{parse.Message}", fg: ConsoleColor.Red);
                    invalidSyntaxFiles.Add(file, parse.Message);
                    continue;
                }
                catch (Exception e)
                {
                    // something went really wrong.
                    Log("\n\n==== FATAL: An exception was thrown. ====", fg: ConsoleColor.Red);
                    Log($"Message: {e.Message}", fg: ConsoleColor.Red);
                    Log("\nStack trace: ", fg: ConsoleColor.Red);
                    Log(e.StackTrace, fg: ConsoleColor.Red);
                    Log("==== End stack trace. The application will not move on. " +
                        "There might be a bug, please report. ====", fg: ConsoleColor.Red);
                }
            }

            if (invalidSyntaxFiles.Count != 0)
            {
                Log("\n\nThe following files had parsing errors, go and fix them:", fg: ConsoleColor.Yellow);
                foreach (var keyPair in invalidSyntaxFiles)
                {
                    Log($"\t{Path.GetFullPath(keyPair.Key)}: {keyPair.Value.Replace("\n", "")}");
                }
            }
        }

        /// <summary>Parse and compile a specific file into JSON.</summary>
        public void CompileFile(string filePath, CompileVerb verb)
        {
            var file = ParseFile(filePath);

            // dont use json if compiling into binary, we will use bic instead.
            if (verb.Bson) verb.ChangeExtension = true;

            // mirrors the directory structure of the input directory 
            // in the output directory.
            var outputPath = Path.GetRelativePath(verb.Input, filePath);
            outputPath = Path.Combine(verb.Output, outputPath);
            outputPath = Path.ChangeExtension(outputPath, verb.ChangeExtension ? "bic" : "json");

            Log($"\tCompiling {Path.GetRelativePath(verb.Input, filePath)}!", fg: ConsoleColor.DarkCyan);

            if (verb.Bson)
                CompileFileBinary(file, outputPath);
            else
                CompileFileJson(file, outputPath, verb);

        }

        /// <summary>Use ANTLR to parse and visit the file.</summary>
        public BilingualFile ParseFile(string filePath)
        {
            var fileText = File.ReadAllText(filePath);
            ICharStream stream = CharStreams.fromString(fileText);
            ITokenSource lexer = new BilingualLexer(stream);
            ITokenStream tokens = new CommonTokenStream(lexer);
            BilingualParser parser = new BilingualParser(tokens);
            BilingualVisitor visitor = new BilingualVisitor();

            var file = visitor.VisitFile(parser.file());
            file.FilePath = filePath;
            return file;
        }

        /// <summary>Serialize the C# representation of a bilingual file into JSON.</summary>
        public void CompileFileJson(BilingualFile? file, string outputPath, CompileVerb verb)
        {
            var settings = new JsonSerializerSettings()
            {
                Formatting = verb.Pretty ? Formatting.Indented : Formatting.None
            };
            settings.Converters.Add(new StringEnumConverter());

            var json = JsonConvert.SerializeObject(file, settings);

            // GetDirectoryName gets rid of file name.
            var directory = Path.GetDirectoryName(outputPath);
            if (!Directory.Exists(directory)) Directory.CreateDirectory(directory!);

            File.WriteAllText(outputPath, json);
        }

        /// <summary>Serialize the C# representation of a bilingual file into BSON.</summary>
        public void CompileFileBinary(BilingualFile? file, string outputPath)
        {
            using MemoryStream stream = new MemoryStream();
            using BsonDataWriter writer = new BsonDataWriter(stream);

            JsonSerializer serializer = new JsonSerializer();
            serializer.Serialize(writer, file);

            // GetDirectoryName gets rid of file name.
            var directory = Path.GetDirectoryName(outputPath);
            if (!Directory.Exists(directory)) Directory.CreateDirectory(directory!);

            var bytes = stream.ToArray();
            File.WriteAllBytes(outputPath, bytes);
        }


    }
}
