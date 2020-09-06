using TauCode.Extensions;

namespace TauCode.Cli.Tests.Common.Hosts.Work.Executors
{
    public class GetWorkerInfoExecutor : CommandWorkerExecutorBase
    {
        public GetWorkerInfoExecutor()
            : base(
                typeof(WorkerExecutorBase).Assembly.GetResourceText($".{nameof(GetWorkerInfoExecutor)}.lisp", true),
                WorkerCommand.GetInfo)
        {
        }
    }
}
