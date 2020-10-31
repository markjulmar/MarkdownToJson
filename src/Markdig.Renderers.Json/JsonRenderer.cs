using System;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Markdig.Helpers;
using Markdig.Renderers.Json.Blocks;
using Markdig.Renderers.Json.Inlines;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;

namespace Markdig.Renderers.Json
{
    /// <summary>
    /// XAML renderer for a Markdown <see cref="MarkdownDocument"/> object.
    /// </summary>
    /// <seealso cref="TextRendererBase" />
    public class JsonRenderer : TextRendererBase<JsonRenderer>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JsonRenderer"/> class.
        /// </summary>
        /// <param name="writer">The writer.</param>
        public JsonRenderer(TextWriter writer) : base(writer)
        {
            ObjectRenderers.Add(new TableRenderer());
            ObjectRenderers.Add(new CodeBlockRenderer());
            ObjectRenderers.Add(new ListRenderer());
            ObjectRenderers.Add(new HeadingRenderer());
            //ObjectRenderers.Add(new HtmlBlockRenderer());
            ObjectRenderers.Add(new ParagraphRenderer());
            ObjectRenderers.Add(new QuoteBlockRenderer());
            //ObjectRenderers.Add(new ThematicBreakRenderer());
            ObjectRenderers.Add(new ContainerBlockRenderer());

            // Default inline renderers
            ObjectRenderers.Add(new AutolinkInlineRenderer());
            ObjectRenderers.Add(new CodeInlineRenderer());
            ObjectRenderers.Add(new DelimiterInlineRenderer());
            ObjectRenderers.Add(new EmphasisInlineRenderer());
            ObjectRenderers.Add(new LineBreakInlineRenderer());
            //ObjectRenderers.Add(new HtmlInlineRenderer());
            ObjectRenderers.Add(new HtmlEntityInlineRenderer());
            ObjectRenderers.Add(new LinkInlineRenderer());
            ObjectRenderers.Add(new LiteralInlineRenderer());
            //ObjectRenderers.Add(new LinkReferenceDefinitionRenderer());
            ObjectRenderers.Add(new ContainerInlineRenderer());
        }

        public new void WriteChildren(ContainerInline containerInline)
        {
            var renderer = new ContainerInlineRenderer();
            renderer.Write(this, containerInline);
        }

        public new void WriteChildren(ContainerBlock containerBlock)
        {
            var renderer = new ContainerBlockRenderer();
            renderer.Write(this, containerBlock);
        }
        
        public bool WillRender(MarkdownObject obj) 
            => ObjectRenderers.Any(renderer => renderer.Accept(this, obj));

        public override object Render(MarkdownObject markdownObject)
        {
            object result;
            if (markdownObject is MarkdownDocument)
            {
                Write("[");
                result = base.Render(markdownObject);
                Write("]");
            }
            else
            {
                result = base.Render(markdownObject);
            }

            return result;
        }

        /// <summary>
        /// Writes the content escaped for XAML.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <returns>This instance</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public JsonRenderer WriteEscape(string content)
        {
            if (string.IsNullOrEmpty(content))
                return this;

            WriteEscape(content, 0, content.Length);
            return this;
        }

        /// <summary>
        /// Writes the content escaped for XAML.
        /// </summary>
        /// <param name="slice">The slice.</param>
        /// <param name="softEscape">Only escape &lt; and &amp;</param>
        /// <returns>This instance</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public JsonRenderer WriteEscape(ref StringSlice slice, bool softEscape = false)
        {
            return slice.Start > slice.End 
                ? this 
                : WriteEscape(slice.Text, slice.Start, slice.Length, softEscape);
        }

        /// <summary>
        /// Writes the content escaped for XAML.
        /// </summary>
        /// <param name="slice">The slice.</param>
        /// <param name="softEscape">Only escape &lt; and &amp;</param>
        /// <returns>This instance</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public JsonRenderer WriteEscape(StringSlice slice, bool softEscape = false) => WriteEscape(ref slice, softEscape);

        /// <summary>
        /// Writes the content escaped for XAML.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <param name="offset">The offset.</param>
        /// <param name="length">The length.</param>
        /// <param name="softEscape">Only escape &lt; and &amp;</param>
        /// <returns>This instance</returns>
        private JsonRenderer WriteEscape(string content, int offset, int length, bool softEscape = false)
        {
            if (string.IsNullOrEmpty(content) || length == 0)
                return this;

            var end = offset + length;
            var previousOffset = offset;
            for (; offset < end; offset++)
            {
                switch (content[offset])
                {
                    case '<':
                        Write(content, previousOffset, offset - previousOffset);
                        Write("&lt;");
                        previousOffset = offset + 1;
                        break;
                    
                    case '\\':
                        Write(content, previousOffset, offset - previousOffset);
                        Write("\\\\");
                        previousOffset = offset + 1;
                        break;

                    case '>':
                        if (!softEscape)
                        {
                            Write(content, previousOffset, offset - previousOffset);
                            Write("&gt;");
                            previousOffset = offset + 1;
                        }
                        break;

                    case '&':
                        Write(content, previousOffset, offset - previousOffset);
                        Write("&amp;");
                        previousOffset = offset + 1;
                        break;

                    case '"':
                        if (!softEscape)
                        {
                            Write(content, previousOffset, offset - previousOffset);
                            Write("&quot;");
                            previousOffset = offset + 1;
                        }
                        break;
                }
            }

            Write(content, previousOffset, end - previousOffset);
            return this;
        }

        /// <summary>
        /// Writes the URL escaped for XAML.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <returns>This instance</returns>
        public JsonRenderer WriteEscapeUrl(string content)
        {
            if (content == null)
                return this;

            var previousPosition = 0;
            var length = content.Length;

            for (var i = 0; i < length; i++)
            {
                var c = content[i];

                if (c < 128)
                {
                    var escape = HtmlHelper.EscapeUrlCharacter(c);
                    if (escape != null)
                    {
                        Write(content, previousPosition, i - previousPosition);
                        previousPosition = i + 1;
                        Write(escape);
                    }
                }
                else
                {
                    Write(content, previousPosition, i - previousPosition);
                    previousPosition = i + 1;

                    byte[] bytes;
                    if (c >= '\ud800' && c <= '\udfff' && previousPosition < length)
                    {
                        bytes = Encoding.UTF8.GetBytes(new[] { c, content[previousPosition] });
                        // Skip next char as it is decoded above
                        i++;
                        previousPosition = i + 1;
                    }
                    else
                    {
                        bytes = Encoding.UTF8.GetBytes(new[] { c });
                    }

                    foreach (var t in bytes)
                    {
                        Write($"%{t:X2}");
                    }
                }
            }

            Write(content, previousPosition, length - previousPosition);
            return this;
        }
        
        /// <summary>
        /// Writes the lines of a <see cref="LeafBlock"/>
        /// </summary>
        /// <param name="leafBlock">The leaf block.</param>
        /// <returns>This instance</returns>
        public JsonRenderer WriteLeafRawLines(LeafBlock leafBlock)
        {
            if (leafBlock == null)
                throw new ArgumentNullException(nameof(leafBlock));
            
            if (leafBlock.Lines.Lines != null)
            {
                var lines = leafBlock.Lines;
                var slices = lines.Lines;

                for (int i = 0; i < lines.Count; i++)
                {
                    if (i > 0)
                        WriteLine(",");
                    Write("\"");
                    WriteEscape(ref slices[i].Slice);
                    Write("\"");
                }
            }
            return this;
        }
    }
}
