using System.Collections.Generic;
using TauCode.Cli.Data;
using TauCode.Extensions;

namespace TauCode.Cli.Tests.Common.Hosts.Tau.Db.Workers
{
    public class SerializeDataWorker : CliWorkerBase
    {
        public SerializeDataWorker()
            : base(
                typeof(SerializeDataWorker).Assembly.GetResourceText(".Tau.Db.SerializeData.lisp", true),
                "sd-1.0",
                true)
        {
        }

        public override void Process(IList<CliCommandEntry> entries)
        {
            this.Output.WriteLine("Serialize Data");
            var connection = entries.GetSingleEntryByAlias("connection").Value;
            var provider = entries.GetSingleEntryByAlias("provider").Value;
            var file = entries.GetSingleEntryByAlias("file").Value;

            this.Output.WriteLine($"Connection: {connection}");
            this.Output.WriteLine($"Provider: {provider}");
            this.Output.WriteLine($"File: {file}");
        }
    }
}
