using Markdig.Renderers.Json.Blocks;
using Markdig.Syntax;

namespace Markdig.Renderers.Json.Inlines
{
    public class LinkReferenceDefinitionRenderer : JsonObjectRenderer<LinkReferenceDefinition>
    {
        protected override void Write(JsonRenderer renderer, LinkReferenceDefinition linkDef)
        {
            renderer.EnsureLine();
            renderer.Write("{ \"type\": \"link-ref\"");
            if (!string.IsNullOrEmpty(linkDef.Label))
            {
                renderer.Write(", \"label\": \"");
                renderer.Write(linkDef.Label);
                renderer.Write("\"");
            }

            if (!string.IsNullOrEmpty(linkDef.Url))
            {
                renderer.Write(", \"url\": \"");
                renderer.Write(linkDef.Url);
                renderer.Write("\"");
            }

            if (!string.IsNullOrEmpty(linkDef.Title))
            {
                renderer.Write(", \"title\": \"");
                renderer.WriteEscape(linkDef.Title);
                renderer.Write("\"");
            }

            renderer.WriteLine("}");
        }
    }
}