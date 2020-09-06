namespace TauCode.Cli.Tests.Common.Hosts.Work.Mock
{
    public class MockWorker
    {
        public MockWorker(string name)
        {
            this.Name = name;
        }

        public MockWorkerState State { get; set; } = MockWorkerState.Stopped;
        public int Timeout { get; set; } = 100;
        public string Name { get; }
    }
}
