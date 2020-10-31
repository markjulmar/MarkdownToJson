using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Markdig.Renderers.Json.Blocks;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;

namespace Markdig.Renderers.Json.Inlines
{
    public class ContainerInlineRenderer : JsonObjectRenderer<ContainerInline>
    {
        protected override void Write(JsonRenderer renderer, ContainerInline obj)
        {
            if (obj == null)
                return;

            var elements = GatherInlines(renderer, obj).ToList();
            for (int index = 0; index < elements.Count; index++)
            {
                if (index > 0)
                    renderer.Write(",");
                
                var child = elements[index];
                renderer.Write(child);
                
                // Skip any next literals.
                if (child is LiteralInline && child.NextSibling != null)
                {
                    var next = child.NextSibling;
                    while (next?.GetType() == typeof(LiteralInline))
                    {
                        index++;
                        next = next.NextSibling;
                    }
                }
            }
        }

        private IEnumerable<Inline> GatherInlines(JsonRenderer renderer, ContainerInline container)
        {
            Type[] ignoreTypes = {typeof(LinkReferenceDefinition)}; 

            var inline = container.FirstChild;
            while (inline != null)
            {
                if (renderer.WillRender(inline) && !ignoreTypes.Contains(inline.GetType()))
                {
                    if (inline is LiteralInline li)
                    {
                        // Skip empty literals.
                        if (!li.Content.IsEmpty)
                            yield return inline;
                    }
                    else yield return inline;
                }
                #if DEBUG
                else Debug.WriteLine($"Found {inline.GetType().Name} with no renderer.");
                #endif
                inline = inline.NextSibling;
            }
        }
    }
}