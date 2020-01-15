using System.Collections.Generic;
using System.Linq;
using TauCode.Cli.Data;
using TauCode.Extensions;

namespace TauCode.Cli.Tests.Common.Hosts.Tau.Db.Workers
{
    public class DropAllTablesWorker : CliWorkerBase
    {
        public DropAllTablesWorker()
            : base(
                typeof(DropAllTablesWorker).Assembly.GetResourceText(".Tau.Db.DropAllTables.lisp", true),
                "dat-1.0",
                true)
        {
        }

        public override void Process(IList<CliCommandEntry> entries)
        {
            var connection = entries.GetSingleEntryByAlias("connection").Value;
            var provider = entries.GetSingleEntryByAlias("provider").Value;
            var exclude = entries.GetEntriesByAlias("exclude").Select(x => x.Value).ToList();

            this.Output.WriteLine($"Connection: [{connection}]");
            this.Output.WriteLine($"Provider: [{provider}]");
            this.Output.WriteLine($"Exclude: [{string.Join(", ", exclude)}]");
        }
    }
}
