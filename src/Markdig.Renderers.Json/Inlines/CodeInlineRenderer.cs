using Markdig.Renderers.Json.Blocks;
using Markdig.Syntax.Inlines;

namespace Markdig.Renderers.Json.Inlines
{
    public class CodeInlineRenderer : JsonObjectRenderer<CodeInline>
    {
        protected override void Write(JsonRenderer renderer, CodeInline obj)
        {
            renderer.EnsureLine();
            renderer.Write("{ \"type\": \"code\", \"value\": \"")
                .WriteEscape(obj.Content)
                .Write("\" }");
        }
    }
}