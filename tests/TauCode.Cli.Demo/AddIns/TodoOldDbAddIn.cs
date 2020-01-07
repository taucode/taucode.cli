using System.Collections.Generic;
using TauCode.Cli.Demo.AddIns.TodoOldDbAddInWorkers;

namespace TauCode.Cli.Demo.AddIns
{
    public class TodoOldDbAddIn : CliAddInBase
    {
        public TodoOldDbAddIn()
            : base("db", "db-1.0", true)
        {
        }

        protected override IReadOnlyList<ICliWorker> CreateWorkers()
        {
            return new ICliWorker[]
            {       
                new TodoOldSerializeDataWorker(),
            };
        }
    }
}
