using Markdig.Syntax;
using Markdig.Syntax.Inlines;

namespace Markdig.Renderers.Json.Blocks
{
    /// <summary>
    /// A base class for XAML rendering <see cref="Block"/> and <see cref="Inline"/> Markdown objects.
    /// </summary>
    /// <typeparam name="TObject">The type of the object.</typeparam>
    /// <seealso cref="IMarkdownObjectRenderer" />
    public abstract class JsonObjectRenderer<TObject> : MarkdownObjectRenderer<JsonRenderer, TObject>
        where TObject : MarkdownObject
    {
    }
}