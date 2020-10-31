using System;
using System.Collections.Generic;
using System.IO;

namespace MDToJson
{
    public static class Program
    {
        static int Main(string[] args)
        {
            if (args.Length == 0)
            {
                PrintHelp();
                Console.Error.WriteLine("Missing: markdown source file.\r\n");
                return 1;
            }

            if (args.Length == 1)
            {
                if (args[0].Trim().ToLower() == "--help"
                    || args[0].Trim().ToLower() == "-h")
                {
                    PrintHelp();
                    return 1;
                }
            }
            
            List<string> markdownFiles = ParseFiles(args);

            if (markdownFiles.Count == 0)
            {
                PrintHelp();
                Console.Error.WriteLine("No markdown files found.\r\n");
                return 1;
            }
            
            ConvertMarkdownToJson converter = new ConvertMarkdownToJson();
           
            // If it's a single file, then process that one file.
            if (markdownFiles.Count == 1)
            {
                try
                {
                    Console.WriteLine(converter.ProcessFile(markdownFiles[0]));
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"{markdownFiles[0]} - {ex.Message}");
#if DEBUG
                    Console.WriteLine("=====");
                    Console.Error.WriteLine(ex.Data["json"]);
                    Console.WriteLine("=====");
#endif
                    return 2;
                }

                return 0;
            }

            Console.WriteLine("[");
            bool first = true;
            foreach (var file in markdownFiles)
            {
                try
                {
                    string result = converter.ProcessFile(file);
                    if (!first) Console.WriteLine(",");
                    first = false;
                    Console.Write(result);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"{markdownFiles[0]} - {ex.Message}");
#if DEBUG
                    Console.Error.WriteLine("=====");
                    Console.Error.WriteLine(ex.Data["json"]);
                    Console.Error.WriteLine("=====");
                    Console.ReadLine();
#endif
                }
            }

            Console.WriteLine();
            Console.WriteLine("]");
            return 0;
        }

        private static List<string> ParseFiles(string[] args)
        {
            List<string> files = new List<string>();
            foreach (var item in args)
            {
                if (File.Exists(item) && !item.Contains('*') && !item.Contains('?'))
                    files.Add(item);
                else
                {
                    // Folder or wildcard
                    string folder = item;
                    string filespec = "*.md";
                    if (!Directory.Exists(folder))
                    {
                        filespec = Path.GetFileName(folder);
                        folder = Path.GetDirectoryName(folder);
                    }
                    files.AddRange(Directory.GetFiles(folder, filespec));
                }
            }

            files.Sort();
            return files;
        }

        private static void PrintHelp()
        {
            Console.WriteLine("MDToJson: convert a Markdown file to JSON");
            Console.WriteLine("Usage:");
            Console.WriteLine();
            Console.WriteLine("MDToJson {filename} or {wildcard} or {folder}");
            Console.WriteLine();
            Console.WriteLine("If multiple files are matched, a JSON array will be returned, otherwise it will be a single object representing the Markdown file.");
            Console.WriteLine();
        }
    }
}