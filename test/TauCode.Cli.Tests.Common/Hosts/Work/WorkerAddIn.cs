using System.Collections.Generic;
using TauCode.Cli.Tests.Common.Hosts.Work.Executors;

namespace TauCode.Cli.Tests.Common.Hosts.Work
{
    /// <summary>
    /// Nameless add-in
    /// </summary>
    public class WorkerAddIn : CliAddInBase
    {
        public WorkerAddIn()
            : base(null, null, false)
        {
        }

        protected override void OnNodeCreated()
        {
        }

        protected override IReadOnlyList<ICliExecutor> CreateExecutors()
        {
            return new ICliExecutor[]
            {
                new GetWorkerInfoExecutor(),
                new StartWorkerExecutor(),
                new PauseWorkerExecutor(),
                new ResumeWorkerExecutor(),
                new StopWorkerExecutor(),
                new DisposeWorkerExecutor(),

                new TimeoutWorkerExecutor(),
            };
        }
    }
}
