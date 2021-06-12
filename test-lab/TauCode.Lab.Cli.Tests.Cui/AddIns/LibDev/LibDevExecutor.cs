using TauCode.Cli;

namespace TauCode.Lab.Cli.Tests.Cui.AddIns.LibDev
{
    public abstract class LibDevExecutor : CliExecutorBase
    {
        protected LibDevExecutor(string grammar, string version, bool supportsHelp)
            : base(grammar, version, supportsHelp)
        {
        }

        internal LibDevContext LibDevContext => (LibDevContext)this.AddIn.Context;
    }
}
