using Markdig.Renderers.Json.Blocks;
using Markdig.Syntax.Inlines;

namespace Markdig.Renderers.Json.Inlines
{
    public class DelimiterInlineRenderer : JsonObjectRenderer<DelimiterInline>
    {
        protected override void Write(JsonRenderer renderer, DelimiterInline obj)
        {
            renderer.WriteEscape(obj.ToLiteral());
            renderer.WriteChildren(obj);
        }
    }
}