using MDToJson;
using Newtonsoft.Json;
using Xunit.Abstractions;

namespace Markdig.Renderers.Json.Tests
{
    public class JsonTestClass
    {
        protected ITestOutputHelper Output { get; }

        public JsonTestClass(ITestOutputHelper output)
        {
            Output = output;
        }

        protected string RawRender(string text) => new ConvertMarkdownToJson().ProcessText(text);

        protected dynamic DynamicRender(string text) => JsonConvert.DeserializeObject(RawRender(text));
    }
}