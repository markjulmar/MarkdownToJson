using Xunit;
using Xunit.Abstractions;

namespace Markdig.Renderers.Json.Tests
{
    public class ExtensionTests : JsonTestClass
    {
        public ExtensionTests(ITestOutputHelper output) : base(output)
        {
        }
        
        [Fact]
        public void TestBasicIncludeExtension()
        {
            string input = "[!INCLUDE [func-name-discover](./func-name-discover.md)]";
            dynamic output = DynamicRender(input);

            Assert.Equal("extension", output.document.entries[0].type.Value);
            Assert.Equal("include", output.document.entries[0].subtype.Value);
            Assert.Equal("func-name-discover", output.document.entries[0].text.Value);
            Assert.Equal("./func-name-discover.md", output.document.entries[0].url.Value);
        }
        
        [Fact]
        public void TestLowercaseIncludeExtension()
        {
            string input = "[!include [func-name-discover](./func-name-discover.md)]";
            dynamic output = DynamicRender(input);

            Assert.Equal("extension", output.document.entries[0].type.Value);
            Assert.Equal("include", output.document.entries[0].subtype.Value);
            Assert.Equal("func-name-discover", output.document.entries[0].text.Value);
            Assert.Equal("./func-name-discover.md", output.document.entries[0].url.Value);
        }

        [Fact]
        public void TestEmbeddedIncludeExtension()
        {
            string input = @"1. Select our function, [!INCLUDE [func-name-discover](./func-name-discover.md)], in the Function Apps portal.

            1. Expand the **View files** menu on the right of the screen.";

            Output.WriteLine(RawRender(input));
            // TODO: can we parse out embedded includes?
        }

        [Fact]
        public void TestVideoExtension()
        {
            string input = "> [!VIDEO https://www.microsoft.com/videoplayer/embed/RWr1XF]";
            Output.WriteLine(RawRender(input));
            
            dynamic output = DynamicRender(input);
            Assert.Equal("quote", output.document.entries[0].type.Value);

            dynamic quote = output.document.entries[0].value[0];
            
            Assert.Equal("extension", quote.type.Value);
            Assert.Equal("video", quote.subtype.Value);
            Assert.Equal("https://www.microsoft.com/videoplayer/embed/RWr1XF", quote.url.Value);
        }
        
        [Fact]
        public void TestYouTubeVideoExtension()
        {
            string input = ">[!VIDEO https://www.youtube.com/embed/Q3kx4cmRkCA]";
            Output.WriteLine(RawRender(input));
            
            dynamic output = DynamicRender(input);
            Assert.Equal("quote", output.document.entries[0].type.Value);
            dynamic quote = output.document.entries[0].value[0];
            
            Assert.Equal("extension", quote.type.Value);
            Assert.Equal("video", quote.subtype.Value);
            Assert.Equal("https://www.youtube.com/embed/Q3kx4cmRkCA", quote.url.Value);
        }        
    }
}