using System;
using System.Collections.Generic;
using TauCode.Cli.Data;

namespace TauCode.Cli.Demo.Hosts.Tau.Db.Workers
{
    public class SerializeDataWorker : CliWorkerBase
    {
        public SerializeDataWorker()
            : base("todo", "todo", true)
        {
        }

        public override void Process(IList<ICliCommandEntry> entries)
        {
            throw new NotImplementedException();
        }
    }
}
