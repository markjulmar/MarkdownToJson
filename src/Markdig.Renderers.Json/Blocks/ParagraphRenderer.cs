using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;

namespace Markdig.Renderers.Json.Blocks
{
    abstract class DocsBlock
    {
        public abstract string Type { get; }
        public abstract string ToJson();
    }

    class DocsVideoBlock : DocsBlock
    {
        private readonly string url;
        public DocsVideoBlock(List<Inline> values)
        {
            if (values.Count < 2)
                throw new ArgumentException("Invalid number of values for DocsVideoBlock", nameof(values));

            var link = values[1] as LiteralInline;
            if (link == null)
                throw new ArgumentException("Failed to parse DocsVideoBlock link", nameof(values));

            url = link.Content.ToString().Split()[1].TrimEnd(']');
        }
        
        public override string Type { get; } = "video";
        public override string ToJson()
        {
            return $"{{ \"type\": \"extension\", \"subtype\": \"{Type}\", \"url\": \"{url}\" }}";
        }
        
    }
    class DocsIncludeBlock : DocsBlock
    {
        private readonly string text;
        private readonly string url;
        public DocsIncludeBlock(List<Inline> values)
        {
            if (values.Count < 4)
                throw new ArgumentException("Invalid number of values for DocsIncludeBlock", nameof(values));

            var link = values[2] as LinkInline;
            if (link == null)
                throw new ArgumentException("Failed to parse DocsIncludeBlock link", nameof(values));

            text = link.Title;
            
            if (string.IsNullOrEmpty(text) && link.FirstChild is LiteralInline li)
                text = HttpUtility.HtmlEncode(li.Content.ToString());

            url = link.GetDynamicUrl?.Invoke() ?? link.Url;
        }
        public override string Type { get; } = "include";
        public override string ToJson()
        {
            return $"{{ \"type\": \"extension\", \"subtype\": \"{Type}\", \"text\": \"{text}\", \"url\": \"{url}\" }}";
        }
    }
    
    public class ParagraphRenderer : JsonObjectRenderer<ParagraphBlock>
    {
        protected override void Write(JsonRenderer renderer, ParagraphBlock obj)
        {
            renderer.EnsureLine();

            var result = IsDocsEmbed(obj);
            if (result != null)
            {
                renderer.Write(result.ToJson());
            }
            else
            {
                renderer.Write("{ ")
                        .Write($"\"type\": \"paragraph\"");
                
                var inline = (Inline) obj.Inline;
                if (inline != null)
                {
                    renderer.Write(", \"items\": [");
                    renderer.Write(obj.Inline);
                    renderer.Write("]");
                }
                renderer.Write(" }");
            }
        }

        private DocsBlock IsDocsEmbed(ParagraphBlock block)
        {
            var firstChild = block.Inline.FirstChild;
            
            if (firstChild is LiteralInline start)
            {
                if (start.Content.Text != "[")
                    return null;

                if (!(block.Inline.Skip(1).FirstOrDefault() is LiteralInline type))
                    return null;

                string key = Regex.Replace(type.Content.ToString().Split()[0], @"[^0-9a-zA-Z\ ]+", "")
                                  .Trim().ToUpper();
                switch (key)
                {
                    case "INCLUDE":
                        return new DocsIncludeBlock(block.Inline.ToList());
                    case "VIDEO":
                        return new DocsVideoBlock(block.Inline.ToList());
                }

            }

            return null;
        }
    }
}