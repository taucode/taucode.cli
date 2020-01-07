using System.Collections.Generic;
using TauCode.Cli.Data;

namespace TauCode.Cli.Demo.Hosts.Tau.Db.Workers
{
    public class DropAllTablesWorker : CliWorkerBase
    {
        public DropAllTablesWorker()
            : base("todo", "todo", true)
        {
        }

        public override void Process(IList<ICliCommandEntry> entries)
        {
            throw new System.NotImplementedException();
        }
    }
}
