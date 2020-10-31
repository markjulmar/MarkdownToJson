using Markdig.Renderers.Json.Blocks;
using Markdig.Syntax.Inlines;

namespace Markdig.Renderers.Json.Inlines
{
    public class LinkInlineRenderer : JsonObjectRenderer<LinkInline>
    {
        protected override void Write(JsonRenderer renderer, LinkInline obj)
        {
            var url = obj.GetDynamicUrl?.Invoke() ?? obj.Url;
            string type = obj.IsImage ? "image" : "link";

            renderer.EnsureLine();
            renderer.Write("{ ");
            renderer.Write($"\"type\": \"{type}\", \"url\": \"");
            renderer.WriteEscapeUrl(url);
            renderer.Write("\"");

            string title = obj.Title;
            if (string.IsNullOrEmpty(title))
            {
                if (obj.FirstChild is LiteralInline literal)
                {
                    title = literal.Content.ToString();
                }
            }

            if (!string.IsNullOrEmpty(title))
            {
                renderer.Write($", \"title\": \"");
                renderer.WriteEscape(title);
                renderer.Write("\"");
                
                if (EmphasisInlineRenderer.ActiveStyle != null)
                {
                    string bold = EmphasisInlineRenderer.ActiveStyle.Bold ? "true" : "false";
                    string italic = EmphasisInlineRenderer.ActiveStyle.Italic ? "true" : "false";
                    renderer.Write($", \"bold\": \"{bold}\", \"italic\": \"{italic}\"");
                }
            }

            renderer.Write("}");
            // if (!renderer.IsLastInContainer)
            //     renderer.Write(",");
        }
    }
}