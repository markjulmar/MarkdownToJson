using Xunit;
using Xunit.Abstractions;

namespace Markdig.Renderers.Json.Tests
{
    public class TableTests : JsonTestClass
    {
        public TableTests(ITestOutputHelper output) : base(output)
        {
        }
        
        [Fact]
        public void TestBasicTable()
        {
            string input = @"
|Direction|Rule types| Description|
|----|----|----|
|Outbound connectivity|Network rules and applications rules|If you configure both network rules and application rules, network rules are applied in priority order before application rules. The rules are terminating; If a match is found in a network rule, no other rules are processed. If there is no network rule match, and if the protocol is HTTP, HTTPS, or MSSQL, then the packet is evaluated by the application rules in priority order. If still no match is found, then the packet is evaluated against the infrastructure rule collection. If there is still no match, then the packet is denied by default.|
";
            Output.WriteLine(RawRender(input));
            // TODO:
        }
    }
}