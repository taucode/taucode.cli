namespace TauCode.Cli.Tests.Common.Hosts.Work
{
    public class WorkerCommandRequest
    {
        public string WorkerName { get; set; }
        public WorkerCommand Command { get; set; }
        public int? Timeout { get; set; }
    }
}
