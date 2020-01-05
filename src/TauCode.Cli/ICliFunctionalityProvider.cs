using System.IO;
using TauCode.Parsing;

namespace TauCode.Cli
{
    public interface ICliFunctionalityProvider
    {
        string Name { get; }
        TextWriter Output { get; }
        TextReader Input { get; }
        INode Node { get; }
        string Version { get; }
        bool SupportsHelp { get; }
        string GetHelp();
    }
}
