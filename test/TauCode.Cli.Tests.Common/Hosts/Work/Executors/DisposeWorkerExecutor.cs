using TauCode.Extensions;

namespace TauCode.Cli.Tests.Common.Hosts.Work.Executors
{
    public class DisposeWorkerExecutor : CommandWorkerExecutorBase
    {
        public DisposeWorkerExecutor()
            : base(
                typeof(WorkerExecutorBase).Assembly.GetResourceText($".{nameof(DisposeWorkerExecutor)}.lisp", true),
                WorkerCommand.Dispose)
        {
        }
    }
}
