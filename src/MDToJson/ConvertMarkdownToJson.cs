using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using Markdig;
using Markdig.Renderers.Json;
using YamlDotNet.Serialization;

namespace MDToJson
{
    public class ConvertMarkdownToJson
    {
        private string currentFilename;

        public string ProcessFile(string filename)
        {
            currentFilename = filename;
            try
            {
                return ProcessText(File.ReadAllText(filename));
            }
            catch (Exception ex)
            {
                ex.Data.Add("filename", filename);
                throw;
            }
            finally
            {
                currentFilename = null;
            }
        }

        public string ProcessText(string contents)
        {
            var yamlHeader = ReadYamlHeader(ref contents);

            var output = new StringWriter();
            output.Write("{");

            if (!string.IsNullOrEmpty(currentFilename))
                output.WriteLine($"\"filename\": \"{currentFilename.Replace("\\", "\\\\")}\", ");

            if (yamlHeader != null) output.WriteLine($"\"metadata\": {yamlHeader},");

            output.WriteLine("\"document\": {");
            output.Write("\"entries\": ");

            var pipelineBuilder = new MarkdownPipelineBuilder();
            var pipeline = pipelineBuilder.UseAdvancedExtensions().Build();
            var jsonWriter = new JsonRenderer(output);
            pipeline.Setup(jsonWriter);
            var results = Markdown.Parse(contents, pipeline);

            jsonWriter.Render(results);
            output.WriteLine("}}");
            output.Flush();

            var finalJson = output.ToString();

            try
            {
                return PrettyJson(finalJson);
            }
            catch (Exception ex)
            {
                var parserException = new Exception(ex.Message, ex);
                parserException.Data.Add("json", finalJson);
                Debug.WriteLine(finalJson);

                throw parserException;
            }
        }

        private static string PrettyJson(string unPrettyJson)
        {
            var options = new JsonSerializerOptions {WriteIndented = true};
            var jsonElement = JsonSerializer.Deserialize<JsonElement>(unPrettyJson);
            return JsonSerializer.Serialize(jsonElement, options);
        }

        private static object ReadYamlHeader(ref string contents)
        {
            var reader = new StringReader(contents);

            var line = reader.ReadLine();
            while (string.IsNullOrWhiteSpace(line))
                line = reader.ReadLine();

            // YAML marker?
            if (line.Trim().Any(c => c != '-'))
                return null;

            var yamlContents = new StringBuilder();
            while (true)
            {
                line = reader.ReadLine();
                if (line?.Trim().All(c => c == '-') != false)
                    break;
                yamlContents.AppendLine(line);
            }

            contents = reader.ReadToEnd();

            if (yamlContents.Length == 0)
                return null;

            var result =
                (Dictionary<object, object>) new Deserializer().Deserialize(new StringReader(yamlContents.ToString()));
            return JsonSerializer.Serialize(result.ToDictionary(kvp => kvp.Key.ToString(),
                kvp => kvp.Value?.ToString() ?? ""));
        }
    }
}