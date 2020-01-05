using System.Collections.Generic;

namespace TauCode.Cli.Tests.TestCli
{
    public class TestDbAddIn : CliAddInBase
    {
        public TestDbAddIn(ICliProgram program)
            : base(
                program,
                "db",
                "db-1488",
                true)
        {
        }

        public override IReadOnlyList<ICliWorker> GetWorkers()
        {
            return new ICliWorker[]
            {
                new TestSdWorker(this), 
            };
        }
    }
}
