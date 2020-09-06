using TauCode.Extensions;

namespace TauCode.Cli.Tests.Common.Hosts.Work.Executors
{
    public class StopWorkerExecutor : CommandWorkerExecutorBase
    {
        public StopWorkerExecutor()
            : base(
                typeof(WorkerExecutorBase).Assembly.GetResourceText($".{nameof(StopWorkerExecutor)}.lisp", true),
                WorkerCommand.Stop)
        {
        }
    }
}
