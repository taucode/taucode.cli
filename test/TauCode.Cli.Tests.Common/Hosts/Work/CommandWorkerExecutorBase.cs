using System.Collections.Generic;
using System.Linq;
using TauCode.Cli.Commands;

namespace TauCode.Cli.Tests.Common.Hosts.Work
{
    public class CommandWorkerExecutorBase : WorkerExecutorBase
    {
        public CommandWorkerExecutorBase(string grammar, WorkerCommand command)
            : base(grammar)
        {
            this.Command = command;
        }

        public WorkerCommand Command { get; }

        protected virtual WorkerCommandRequest CreateRequest(CliCommandSummary summary)
        {
            var workerName = summary.Arguments["worker-name"].Single();

            var request = new WorkerCommandRequest
            {
                WorkerName = workerName,
                Command = this.Command,
            };

            return request;
        }

        public override void Process(IList<CliCommandEntry> entries)
        {
            var bus = this.GetBus();

            var summary = (new CliCommandSummaryBuilder()).Build(this.Descriptor, entries);
            var request = this.CreateRequest(summary);
            var response = bus.Request(request);

            this.ShowResult(response.Result, response.Exception);
        }
    }
}
