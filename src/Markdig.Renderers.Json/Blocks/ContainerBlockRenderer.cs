using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Markdig.Syntax;

namespace Markdig.Renderers.Json.Blocks
{
    public class ContainerBlockRenderer : JsonObjectRenderer<ContainerBlock>
    {
        protected override void Write(JsonRenderer renderer, ContainerBlock obj)
        {
            if (obj != null)
            {
                var children = GatherBlocks(renderer,obj).ToList();
                for (int i = 0; i < children.Count; i++)
                {
                    if (i > 0)
                        renderer.Write(",");
                    renderer.Write(children[i]);
                }
            }
        }
        
        private IEnumerable<Block> GatherBlocks(JsonRenderer renderer, ContainerBlock container)
        {
            Type[] ignoreTypes = {typeof(LinkReferenceDefinitionGroup)}; 
            
            foreach (var child in container)
            {
                if (renderer.WillRender(child) && !ignoreTypes.Contains(child.GetType()))
                    yield return child;
#if DEBUG
                else Debug.WriteLine($"Found {child.GetType().Name} with no renderer.");
#endif
            }
        }
        
    }
}