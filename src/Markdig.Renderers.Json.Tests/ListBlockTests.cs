using System.Linq;
using Newtonsoft.Json.Linq;
using Xunit;
using Xunit.Abstractions;

namespace Markdig.Renderers.Json.Tests
{
    public class ListBlockTests : JsonTestClass
    {
        public ListBlockTests(ITestOutputHelper output) : base(output)
        {
        }
        
        [Fact]
        public void TestBasicOrderedListExtension()
        {
            string input = @"
1. Item 1
1. Item 2
1. Item 3";
            Output.WriteLine(RawRender(input));
            dynamic output = DynamicRender(input);
            Assert.Equal("ordered-list", output.document.entries[0].type.Value);
            Assert.Equal("1", output.document.entries[0].start.Value);

            JArray items = output.document.entries[0].items;
            Assert.Equal(3, items.Count);
            Assert.True(items.Cast<JObject>().All(e => e["type"].ToString() == "paragraph"));

            int count = 1;
            foreach (var obj in items.Cast<JObject>())
            {
                JArray arr = (JArray) obj["items"];
                Assert.Single(arr);
                dynamic item = arr[0];
                Assert.Equal("text", item.type.Value);
                Assert.Equal($"Item {count++}", item.value.Value);
            }
        }
        
        [Fact]
        public void TestBasicBulletedListExtension()
        {
            string input = @"
- Item 1
- Item 2
- Item 3";
            Output.WriteLine(RawRender(input));
            dynamic output = DynamicRender(input);
            Assert.Equal("bulleted-list", output.document.entries[0].type.Value);

            JArray items = output.document.entries[0].items;
            Assert.Equal(3, items.Count);
            Assert.True(items.Cast<JObject>().All(e => e["type"].ToString() == "paragraph"));

            int count = 1;
            foreach (var obj in items.Cast<JObject>())
            {
                JArray arr = (JArray) obj["items"];
                Assert.Single(arr);
                dynamic item = arr[0];
                Assert.Equal("text", item.type.Value);
                Assert.Equal($"Item {count++}", item.value.Value);
            }
        }
    }
}