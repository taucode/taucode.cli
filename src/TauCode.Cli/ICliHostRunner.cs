namespace TauCode.Cli
{
    public interface ICliHostRunner
    {
        int Run(string[] args);
        ICliHost Host { get; }
    }
}
