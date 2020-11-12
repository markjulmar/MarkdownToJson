using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using Markdig;
using Markdig.Renderers.Json;
using YamlDotNet.Serialization;

namespace MDToJson
{
    public class ConvertMarkdownToJson
    {
        private string currentFilename;
        public bool StrictEncoding { get; set; }

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

        private static readonly string ByteOrderMarkUtf8 = Encoding.UTF8.GetString(Encoding.UTF8.GetPreamble());
        private static string StripBom(string input)
        {
            if (input.StartsWith(ByteOrderMarkUtf8, StringComparison.Ordinal))
            {
                input = input.Remove(0, ByteOrderMarkUtf8.Length);
            }
            return input;
        }
        public string ProcessText(string contents)
        {
            contents = StripBom(contents);
            
            var yamlHeader = ReadYamlHeader(ref contents);

            var output = new StringWriter();
            output.Write("{");

            if (!string.IsNullOrEmpty(currentFilename))
                output.WriteLine($"\"filename\": \"{UnifyFilename(currentFilename)}\", ");

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

        private static string UnifyFilename(string filename) 
            => filename.Replace('\\', '/').Substring((Path.GetPathRoot(filename)??"").Length);

        private string PrettyJson(string unPrettyJson)
        {
            var options = new JsonSerializerOptions {WriteIndented = true};

            if (!StrictEncoding)
            {
                options.Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
            }

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
                kvp => ToJsonValue(kvp.Value)));
        }

        private static object ToJsonValue(object value)
        {
            return value switch
            {
                null => string.Empty,
                string _ => value,
                _ => value is IEnumerable ie
                    ? (object) (from object obj in ie select ToJsonValue(obj).ToString()).ToList()
                    : value.ToString()
            };
        }
    }
}