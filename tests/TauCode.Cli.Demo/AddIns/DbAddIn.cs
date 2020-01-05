using System.Collections.Generic;
using TauCode.Cli.Demo.AddIns.DbAddInWorkers;

namespace TauCode.Cli.Demo.AddIns
{
    public class DbAddIn : CliAddInBase
    {
        public DbAddIn(ICliProgram program)
            : base(program, "db", "db-1488", true)
        {
        }

        public override IReadOnlyList<ICliWorker> GetWorkers()
        {
            return new ICliWorker[]
            {
                new SerializeDataWorker(this),
            };
        }
    }
}
