using System;
using System.Collections.Generic;
using TauCode.Cli.Data;
using TauCode.Extensions;

namespace TauCode.Cli.Tests.TestCli
{
    public class TestSdWorker : CliWorkerBase
    {
        public TestSdWorker(ICliAddIn addIn)
            : base(
                addIn,
                typeof(TestSdWorker).Assembly.GetResourceText("sd-grammar.lisp", true), 
                "sd-1599", 
                true)
        //: base(addIn, typeof(TestSdWorker).Assembly.GetResourceText("sd-grammar.lisp", true))
        {
        }

        public override void Process(IList<ICliCommandEntry> entries)
        {
            throw new NotImplementedException();
            //var writer = this.AddIn.Program.Output;
            //var connection = ((KeyValueCliCommandEntry)entries.Single(x => x.Alias == "CONNECTION")).Value;
            //var provider = ((KeyValueCliCommandEntry)entries.Single(x => x.Alias == "PROVIDER")).Value;
            //var file = ((KeyValueCliCommandEntry)entries.Single(x => x.Alias == "FILE")).Value;

            //writer.WriteLine("Serialize Data");
            //writer.WriteLine($"Connection: {connection}");
            //writer.WriteLine($"Provider: {provider}");
            //writer.WriteLine($"File: {file}");
        }
    }
}
