using System.Collections.Generic;
using TauCode.Cli.Data;
using TauCode.Extensions;

namespace TauCode.Cli.Demo.AddIns.TodoOldDbAddInWorkers
{
    public class TodoOldSerializeDataWorker : CliWorkerBase
    {
        public TodoOldSerializeDataWorker()
            : base(
                typeof(Program).Assembly.GetResourceText("sd-grammar.lisp", true),
                "sd-1.0", 
                true)
        {
        }

        public override void Process(IList<CliCommandEntry> entries)
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
