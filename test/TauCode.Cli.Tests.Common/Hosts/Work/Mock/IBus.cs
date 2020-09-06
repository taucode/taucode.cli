namespace TauCode.Cli.Tests.Common.Hosts.Work.Mock
{
    public interface IBus
    {
        WorkerCommandResponse Request(WorkerCommandRequest request);
    }
}
