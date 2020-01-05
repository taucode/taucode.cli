using System.IO;
using TauCode.Parsing;

namespace TauCode.Cli
{
    public interface ICliFunctionalityProvider
    {
        string Name { get; }
        TextWriter Output { get; set; }
        TextReader Input { get; set; }
        INode Node { get; }
        string Version { get; }
        bool SupportsHelp { get; }
        string GetHelp();
    }
}
