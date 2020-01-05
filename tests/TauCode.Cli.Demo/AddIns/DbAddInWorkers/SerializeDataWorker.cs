using System;
using System.Collections.Generic;
using TauCode.Cli.Data;
using TauCode.Extensions;

namespace TauCode.Cli.Demo.AddIns.DbAddInWorkers
{
    public class SerializeDataWorker : CliWorkerBase
    {
        public SerializeDataWorker(ICliAddIn addIn)
        //: base(addIn, typeof(Program).Assembly.GetResourceText("sd-grammar.lisp", true))
            : base(
                addIn,
                typeof(Program).Assembly.GetResourceText("sd-grammar.lisp", true),
                "todo-sd", 
                true)
        {
        }

        public override void Process(IList<ICliCommandEntry> entries)
        {
            throw new NotImplementedException();
            //var writer = this.AddIn.Program.Output;
            //var connection = this.GetSingleValue(entries, "connection");
            //var provider = this.GetSingleValue(entries, "provider");
            //var file = this.GetSingleValue(entries, "file");

            //writer.WriteLine("Serialize Data");
            //writer.WriteLine($"Connection: {connection}");
            //writer.WriteLine($"Provider: {provider}");
            //writer.WriteLine($"File: {file}");
        }
    }
}
