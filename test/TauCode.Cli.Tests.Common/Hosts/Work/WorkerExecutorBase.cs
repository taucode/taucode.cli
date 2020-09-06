using TauCode.Cli.Tests.Common.Hosts.Work.Mock;

namespace TauCode.Cli.Tests.Common.Hosts.Work
{
    public abstract class WorkerExecutorBase : CliExecutorBase
    {
        protected WorkerExecutorBase(
            string grammar)
            : base(grammar, "1.0", true)
        {
        }

        protected IBus GetBus() => ((WorkerHost)this.AddIn.Host).Bus;

        protected void ShowResult(string result, ExceptionInfo exception)
        {
            if (exception == null)
            {
                this.Output.WriteLine(result);
            }

            if (exception != null)
            {
                this.Output.WriteLine("Server returned exception:");
                this.Output.WriteLine(exception.TypeName);
                this.Output.WriteLine(exception.Message);
            }
        }
    }
}
