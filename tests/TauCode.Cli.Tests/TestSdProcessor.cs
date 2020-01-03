using System.Collections.Generic;
using System.Linq;
using TauCode.Cli.Data;
using TauCode.Cli.Data.Entries;
using TauCode.Extensions;

namespace TauCode.Cli.Tests
{
    public class TestSdProcessor : CliProcessorBase
    {
        public TestSdProcessor(ICliAddIn addIn)
            : base(addIn, typeof(TestSdProcessor).Assembly.GetResourceText("sd-grammar.lisp", true))
        {
        }

        public override void Process(IList<ICliCommandEntry> entries)
        {
            var writer = this.AddIn.Program.Output;
            var connection = ((KeyValueCliCommandEntry)entries.Single(x => x.Alias == "CONNECTION")).Value;
            var provider = ((KeyValueCliCommandEntry)entries.Single(x => x.Alias == "PROVIDER")).Value;
            var file = ((KeyValueCliCommandEntry)entries.Single(x => x.Alias == "FILE")).Value;

            writer.WriteLine("Serialize Data");
            writer.WriteLine($"Connection: {connection}");
            writer.WriteLine($"Provider: {provider}");
            writer.WriteLine($"File: {file}");
        }
    }
}
