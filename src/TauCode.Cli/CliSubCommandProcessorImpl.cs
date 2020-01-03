using TauCode.Cli.Data;
using TauCode.Parsing;

namespace TauCode.Cli
{
    public class CliSubCommandProcessorImpl : ICliSubCommandProcessor
    {
        private readonly string _grammar;

        public CliSubCommandProcessorImpl(string grammar)
        {
            _grammar = grammar; // todo checks
        }

        public string GetGrammar() => _grammar;

        public INode BuildNode()
        {
            throw new System.NotImplementedException();
        }

        public bool AcceptsCommand(CliCommand command)
        {
            throw new System.NotImplementedException();
        }

        public void ProcessCommand(CliCommand command)
        {
            throw new System.NotImplementedException();
        }

        public string GetHelp()
        {
            throw new System.NotImplementedException();
        }
    }
}
