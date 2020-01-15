using System.Collections.Generic;
using TauCode.Cli.Data;
using TauCode.Extensions;

namespace TauCode.Cli.Demo.Hosts.Tau.Db.Workers
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
            throw new System.NotImplementedException();
        }
    }
}
