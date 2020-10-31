using Markdig.Renderers.Json.Blocks;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;

namespace Markdig.Renderers.Json.Inlines
{
    public class LiteralInlineRenderer : JsonObjectRenderer<LiteralInline>
    {
        protected override void Write(JsonRenderer renderer, LiteralInline obj)
        {
            if (!obj.Content.IsEmpty)
            {
                bool isHeader = obj.Parent?.ParentBlock?.GetType() == typeof(HeadingBlock); 
                
                if (!isHeader)
                {
                    renderer.Write("{ \"type\": \"text\", ");

                    if (EmphasisInlineRenderer.ActiveStyle != null)
                    {
                        string bold = EmphasisInlineRenderer.ActiveStyle.Bold ? "true" : "false";
                        string italic = EmphasisInlineRenderer.ActiveStyle.Italic ? "true" : "false";
                        renderer.Write($"\"bold\": \"{bold}\", \"italic\": \"{italic}\", ");
                    }
                    
                    renderer.Write("\"value\": \"");
                }
               
                renderer.WriteEscape(ref obj.Content);

                var nextItem = obj.NextSibling;
                while (nextItem?.GetType() == typeof(LiteralInline))
                {
                    renderer.WriteEscape(((LiteralInline) nextItem).Content);
                    nextItem = nextItem.NextSibling;
                }
                
                if (!isHeader)
                {
                    renderer.Write("\"");
                    renderer.Write("}");
                    // if (!renderer.IsLastInContainer)
                    //     renderer.Write(",");
                }
            }
        }        
    }
}