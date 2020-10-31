using Markdig.Renderers.Json.Blocks;
using Markdig.Syntax.Inlines;

namespace Markdig.Renderers.Json.Inlines
{
    public class HtmlEntityInlineRenderer : JsonObjectRenderer<HtmlEntityInline>
    {
        protected override void Write(JsonRenderer renderer, HtmlEntityInline obj)
        {
            renderer.Write("{ \"type\": \"text\", ");

            if (EmphasisInlineRenderer.ActiveStyle != null)
            {
                string bold = EmphasisInlineRenderer.ActiveStyle.Bold ? "true" : "false";
                string italic = EmphasisInlineRenderer.ActiveStyle.Italic ? "true" : "false";
                renderer.Write($"\"bold\": \"{bold}\", \"italic\": \"{italic}\", ");
            }

            renderer.Write("\"value\": \"")
                .WriteEscape(obj.Transcoded)
                .Write("\" }");
        }
    }
}