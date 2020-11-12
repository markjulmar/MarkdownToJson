using System.Diagnostics.CodeAnalysis;
using Markdig.Syntax;

namespace Markdig.Renderers.Json.Blocks
{
    public class ListRenderer : JsonObjectRenderer<ListBlock>
    {
        protected override void Write(JsonRenderer renderer, [NotNull] ListBlock listBlock)
        {
            renderer.EnsureLine();
            string type = listBlock.IsOrdered ? "ordered-list" : "bulleted-list";
           
            renderer.Write($"{{ \"type\": \"{type}\"");
            
            if (listBlock.IsOrdered)
            {
                if (listBlock.OrderedStart != null && listBlock.DefaultOrderedStart != listBlock.OrderedStart)
                {
                    renderer.Write($", \"start\": \"{listBlock.OrderedStart}\"");
                }
                else
                {
                    renderer.Write($", \"start\": \"1\"");
                }
            }

            renderer.Write($", \"items\": [");

            for (var index = 0; index < listBlock.Count; index++)
            {
                var item = listBlock[index];
                var listItem = (ListItemBlock) item;
                if (index > 0)
                    renderer.Write(", ");
                renderer.EnsureLine();
                renderer.WriteChildren(listItem);
            }

            renderer.Write("] }");
        }
    }
}