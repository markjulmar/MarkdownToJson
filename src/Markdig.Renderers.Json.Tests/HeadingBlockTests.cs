using Xunit;
using Xunit.Abstractions;

namespace Markdig.Renderers.Json.Tests
{
    public class HeadingBlockTests : JsonTestClass
    {
        public HeadingBlockTests(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void RendersLevel1Heading()
        {
            string input = "# Test Header\r\n";
            dynamic output = DynamicRender(input);
            Assert.Equal("heading", output.document.entries[0].type.Value);
            Assert.Equal("Test Header", output.document.entries[0].value.Value);
            Assert.Equal("1", output.document.entries[0].level.Value);
        }
        
        [Fact]
        public void RendersLevel2Heading()
        {
            string input = "## Test Header 2\r\n";
            dynamic output = DynamicRender(input);
            Assert.Equal("heading", output.document.entries[0].type.Value);
            Assert.Equal("Test Header 2", output.document.entries[0].value.Value);
            Assert.Equal("2", output.document.entries[0].level.Value);
        }
    }
}