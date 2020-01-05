namespace TauCode.Cli.Tests.TestCli
{
    public class TestDbAddIn : CliAddInBase
    {
        public TestDbAddIn(ICliHost host)
            : base(
                host,
                "db",
                "db-1488",
                true)
        {
        }

        //public override IReadOnlyList<ICliWorker> CreateWorkers()
        //{
        //    return new ICliWorker[]
        //    {
        //        new TestSdWorker(this), 
        //    };
        //}
        public override ICliWorker[] CreateWorkers()
        {
            throw new System.NotImplementedException();
        }
    }
}
