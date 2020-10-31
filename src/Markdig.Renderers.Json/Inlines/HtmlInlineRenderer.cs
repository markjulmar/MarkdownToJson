using Markdig.Renderers.Json.Blocks;
using Markdig.Syntax.Inlines;

namespace Markdig.Renderers.Json.Inlines
{
    public class HtmlInlineRenderer : JsonObjectRenderer<HtmlInline>
    {
        protected override void Write(JsonRenderer renderer, HtmlInline obj)
        {
            // HTML inlines are not supported
        }
    }
}