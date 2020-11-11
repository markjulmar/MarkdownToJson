using System;
using System.Collections.Generic;
using System.Text;
using CommandLine;

namespace MDToJson
{
    public sealed class CommandLineOptions
    {
        [Value(0, HelpText = "Markdown file or folder with Markdown content", Required = true)]
        public string FileOrFolder { get; set; }

        [Option('o', "OutputFilename", HelpText = "Output filename")]
        public string OutputFile { get; set; }

        [Option('f', "Overwrite", HelpText = "Overwrite any existing file")]
        public bool OverwriteOutput { get; set; }
    }
}
