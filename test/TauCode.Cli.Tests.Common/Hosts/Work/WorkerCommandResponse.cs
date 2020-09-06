namespace TauCode.Cli.Tests.Common.Hosts.Work
{
    public class WorkerCommandResponse
    {
        public string Result { get; set; }
        public int? Timeout { get; set; }
        public ExceptionInfo Exception { get; set; }
    }
}
