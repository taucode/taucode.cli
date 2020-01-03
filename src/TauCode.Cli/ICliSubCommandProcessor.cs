using TauCode.Cli.Data;
using TauCode.Parsing;

namespace TauCode.Cli
{
    public interface ICliSubCommandProcessor
    {
        string GetGrammar();
        INode BuildNode();
        bool AcceptsCommand(CliCommand command);
        void ProcessCommand(CliCommand command);
        string GetHelp();
    }
}
