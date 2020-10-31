using Markdig.Syntax;

namespace Markdig.Renderers.Json.Blocks
{
    public class QuoteBlockRenderer : JsonObjectRenderer<QuoteBlock>
    {
        protected override void Write(JsonRenderer renderer, QuoteBlock obj)
        {
            renderer.EnsureLine();
            renderer.Write("{ ");
            renderer.Write($"\"type\": \"quote\", \"value\": [");
            renderer.WriteChildren(obj);
            renderer.Write("] }");
        }
    }
}