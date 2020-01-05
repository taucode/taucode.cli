using System.Collections.Generic;
using System.Linq;
using TauCode.Cli.Data;
using TauCode.Cli.Data.Entries;
using TauCode.Extensions;

namespace TauCode.Cli.Tests.TestCli
{
    public class SdWorker : CliWorkerBase
    {
        public SdWorker(ICliAddIn addIn)
            : base(
                addIn,
                typeof(SdWorker).Assembly.GetResourceText("sd-grammar.lisp", true), 
                "sd-1.0", 
                true)
        {
        }

        public override void Process(IList<ICliCommandEntry> entries)
        {
            var connection = ((KeyValueCliCommandEntry)entries.Single(x => x.Alias == "CONNECTION")).Value;
            var provider = ((KeyValueCliCommandEntry)entries.Single(x => x.Alias == "PROVIDER")).Value;
            var file = ((KeyValueCliCommandEntry)entries.Single(x => x.Alias == "FILE")).Value;

            this.Output.WriteLine("Serialize Data");
            this.Output.WriteLine($"Connection: {connection}");
            this.Output.WriteLine($"Provider: {provider}");
            this.Output.WriteLine($"File: {file}");
        }
    }
}
