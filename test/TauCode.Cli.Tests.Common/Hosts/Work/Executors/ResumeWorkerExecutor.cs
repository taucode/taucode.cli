using TauCode.Extensions;

namespace TauCode.Cli.Tests.Common.Hosts.Work.Executors
{
    public class ResumeWorkerExecutor : CommandWorkerExecutorBase
    {
        public ResumeWorkerExecutor()
            : base(
                typeof(WorkerExecutorBase).Assembly.GetResourceText($".{nameof(ResumeWorkerExecutor)}.lisp", true),
                WorkerCommand.Resume)
        {
        }
    }
}
