using TauCode.Cli.Data;

namespace TauCode.Cli
{
    // todo clean up
    public interface ICliHost : ICliFunctionalityProvider
    {
        //string Name { get; }
        ICliAddIn[] CreateAddIns();
        CliCommand ParseCommand(params string[] input);
        void DispatchCommand(CliCommand command);
        //TextReader Input { get; set; }
        //TextWriter Output { get; set; }
    }
}
