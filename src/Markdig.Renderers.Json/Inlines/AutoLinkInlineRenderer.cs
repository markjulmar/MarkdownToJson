using Markdig.Renderers.Json.Blocks;
using Markdig.Syntax.Inlines;

namespace Markdig.Renderers.Json.Inlines
{
    public class AutolinkInlineRenderer : JsonObjectRenderer<AutolinkInline>
    {
        protected override void Write(JsonRenderer renderer, AutolinkInline obj)
        {
            renderer.EnsureLine();
            renderer.Write("{ \"type\": \"link\", \"url\": \"")
                .WriteEscapeUrl(obj.Url)
                .Write("\" }");
        }
    }
}