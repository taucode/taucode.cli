using System.Linq;
using TauCode.Cli.Commands;
using TauCode.Extensions;

namespace TauCode.Cli.Tests.Common.Hosts.Work.Executors
{
    public class TimeoutWorkerExecutor : CommandWorkerExecutorBase
    {
        public TimeoutWorkerExecutor()
            : base(
                typeof(WorkerExecutorBase).Assembly.GetResourceText($".{nameof(TimeoutWorkerExecutor)}.lisp", true),
                WorkerCommand.Timeout)
        {
        }

        protected override WorkerCommandRequest CreateRequest(CliCommandSummary summary)
        {
            var request = base.CreateRequest(summary);
            var timeout = summary.Arguments["timeout-value"].FirstOrDefault();

            if (timeout != null)
            {
                request.Timeout = timeout.ToInt32();
            }

            return request;
        }
    }
}
