using Markdig.Renderers.Json.Blocks;
using Markdig.Syntax.Inlines;

namespace Markdig.Renderers.Json.Inlines
{
    public class LiteralStyle
    {
        public bool Bold { get; set; }
        public bool Italic { get; set; }

        public LiteralStyle(LiteralStyle style)
        {
            if (style != null)
            {
                this.Bold = style.Bold;
                this.Italic = style.Italic;
            }
        }
    }
    
    public class EmphasisInlineRenderer : JsonObjectRenderer<EmphasisInline>
    {
        internal static LiteralStyle ActiveStyle;
        
        protected override void Write(JsonRenderer renderer, EmphasisInline obj)
        {
            var oldStyle = ActiveStyle;
            
            ActiveStyle = new LiteralStyle(oldStyle);
            if (obj.DelimiterChar == '*' || obj.DelimiterChar == '_')
            {
                if (obj.DelimiterCount == 2)
                {
                    ActiveStyle.Bold = true;
                }
                else
                {
                    ActiveStyle.Italic = true;
                }
            }
            
            renderer.WriteChildren(obj);

            ActiveStyle = oldStyle;
        }
    }
}