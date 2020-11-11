using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CommandLine;

namespace MDToJson
{
    public static class Program
    {
        public static int Main(string[] args)
        {
            CommandLineOptions options = null;
            new Parser(cfg => cfg.HelpWriter = Console.Error)
                .ParseArguments<CommandLineOptions>(args)
                .WithParsed(clo => options = clo);
            if (options == null)
            {
                PrintHelp();
                return -1; // bad arguments or help.
            }

            string outputFile = null;
            if (!string.IsNullOrEmpty(options.OutputFile))
            {
                outputFile = options.OutputFile;
                if (Path.GetInvalidFileNameChars().Any(ch => outputFile.Contains(ch))
                    || Path.GetInvalidPathChars().Any(ch => outputFile.Contains(ch)))
                {
                    Console.Error.WriteLine($"Invalid output filename - {outputFile}.");
                    return -1;
                }

                if (File.Exists(outputFile) && !options.OverwriteOutput)
                {
                    Console.Error.WriteLine($"Output file {outputFile} already exists, specify '-f' to overwrite.");
                    return -2;
                }
            }

            var markdownFiles = ParseFiles(options.FileOrFolder);
            if (markdownFiles.Count == 0)
            {
                PrintHelp();
                Console.Error.WriteLine($"No markdown files found from {options.FileOrFolder}.");
                return -3;
            }

            var converter = new ConvertMarkdownToJson();

            TextWriter writer = Console.Out;
            if (outputFile != null)
                writer = new StreamWriter(outputFile);

            bool hasErrors = false;

            try
            {
                // If it's a single file, then process that one file.
                if (markdownFiles.Count == 1)
                {
                    try
                    {
                        writer.WriteLine(converter.ProcessFile(markdownFiles[0]));
                        return 0;
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"{markdownFiles[0]} - {ex.Message}");
                        Console.Error.WriteLine(ex.Data["json"]);
                        return 2;
                    }
                }

                writer.WriteLine("[");
                bool first = true;
                foreach (var file in markdownFiles)
                {
                    try
                    {
                        string result = converter.ProcessFile(file);
                        if (!first) writer.WriteLine(",");
                        first = false;
                        writer.Write(result);
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"{markdownFiles[0]} - {ex.Message}");
                        Console.Error.WriteLine(ex.Data["json"]);
                        Console.Error.WriteLine("Press [ENTER] to continue onto next file.");
                        Console.ReadLine();
                        hasErrors = true;
                    }
                }

                writer.WriteLine();
                writer.WriteLine("]");
            }
            finally
            {
                if (outputFile != null)
                    writer.Dispose();
            }

            return !hasErrors ? 0 : 2;
        }

        private static List<string> ParseFiles(string fileOrFolder)
        {
            fileOrFolder = fileOrFolder.Trim('\"');

            List<string> files = new List<string>();
            if (File.Exists(fileOrFolder))
            {
                files.Add(fileOrFolder);
            }
            else
            {
                string folder = fileOrFolder;
                string filespec = "*.md";

                if (fileOrFolder.Contains('*') || fileOrFolder.Contains('?'))
                {
                    filespec = Path.GetFileName(fileOrFolder);
                    folder = filespec == fileOrFolder ? "." : Path.GetDirectoryName(fileOrFolder);
                }

                if (!Directory.Exists(folder))
                {
                    filespec = Path.GetFileName(folder);
                    folder = Path.GetDirectoryName(folder);

                    if (!Directory.Exists(folder))
                        return null;
                }

                files.AddRange(Directory.GetFiles(folder, filespec));
            }

            files.Sort();
            return files;
        }

        private static void PrintHelp()
        {
            Console.WriteLine("Usage: MDToJson [filename | wildcard | folder]");
            Console.WriteLine();
            Console.WriteLine("If multiple files are matched, a JSON array will be returned, otherwise it will be a single object representing the Markdown file.");
            Console.WriteLine();
        }
    }
}