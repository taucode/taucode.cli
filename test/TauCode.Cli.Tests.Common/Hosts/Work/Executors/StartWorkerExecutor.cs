using TauCode.Extensions;

namespace TauCode.Cli.Tests.Common.Hosts.Work.Executors
{
    public class StartWorkerExecutor : CommandWorkerExecutorBase
    {
        public StartWorkerExecutor()
            : base(
                typeof(WorkerExecutorBase).Assembly.GetResourceText($".{nameof(StartWorkerExecutor)}.lisp", true),
                WorkerCommand.Start)
        {
        }
    }
}
