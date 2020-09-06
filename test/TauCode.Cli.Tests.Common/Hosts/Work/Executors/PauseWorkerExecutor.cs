using TauCode.Extensions;

namespace TauCode.Cli.Tests.Common.Hosts.Work.Executors
{
    public class PauseWorkerExecutor : CommandWorkerExecutorBase
    {
        public PauseWorkerExecutor()
            : base(
                typeof(WorkerExecutorBase).Assembly.GetResourceText($".{nameof(PauseWorkerExecutor)}.lisp", true),
                WorkerCommand.Pause)
        {
        }
    }
}
