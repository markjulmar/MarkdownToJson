using Markdig.Renderers.Json.Blocks;
using Markdig.Syntax.Inlines;

namespace Markdig.Renderers.Json.Inlines
{
    public class LineBreakInlineRenderer : JsonObjectRenderer<LineBreakInline>
    {
        protected override void Write(JsonRenderer renderer, LineBreakInline obj)
        {
            string type = obj.IsHard ? "hard" : "soft";
            renderer.EnsureLine();
            renderer.Write("{ ");
            renderer.Write($"\"type\": \"linebreak\", \"subtype\": \"{type}\"");
            renderer.Write("}");
        }
    }
}