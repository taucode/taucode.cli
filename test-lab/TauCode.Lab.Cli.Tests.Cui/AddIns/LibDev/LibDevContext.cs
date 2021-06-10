using TauCode.Cli;

namespace TauCode.Lab.Cli.Tests.Cui.AddIns.LibDev
{
    public class LibDevContext : CliContextBase
    {
        public string CurrentDirectory { get; set; }

        public override string ToString() => this.CurrentDirectory;
    }
}
