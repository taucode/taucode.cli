namespace TauCode.Cli
{
    // todo get rid of.
    public interface ICliHostRunner
    {
        int Run(string[] args);
        ICliHost Host { get; }
    }
}
