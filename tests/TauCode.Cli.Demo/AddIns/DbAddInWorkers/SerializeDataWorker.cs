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
            var connection = this.GetSingleValue(entries, "connection");
            var provider = this.GetSingleValue(entries, "provider");
            var file = this.GetSingleValue(entries, "file");

            this.Output.WriteLine("Serialize Data");
            this.Output.WriteLine($"Connection: {connection}");
            this.Output.WriteLine($"Provider: {provider}");
            this.Output.WriteLine($"File: {file}");
        }
    }
}
