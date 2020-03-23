namespace TauCode.Cli
{
    public abstract class CliHostRunnerBase : ICliHostRunner
    {
        public abstract int Run(string[] args);
    }
}
