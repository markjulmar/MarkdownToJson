using Markdig.Syntax;

namespace Markdig.Renderers.Json.Blocks
{
    public class CodeBlockRenderer : JsonObjectRenderer<CodeBlock>
    {
        protected override void Write(JsonRenderer renderer, CodeBlock obj)
        {
            renderer.EnsureLine();
            
            var fencedCodeBlock = obj as FencedCodeBlock;
            renderer.Write($"{{ \"type\": \"code\", \"language\": \"{fencedCodeBlock?.Info??string.Empty}\", \"lines\": [");
            renderer.WriteLeafRawLines(obj);
            renderer.Write("] }");
        }
    }
}