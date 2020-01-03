using System.Collections.Generic;
using TauCode.Cli.Data;
using TauCode.Parsing;

namespace TauCode.Cli
{
    public interface ICliProcessor
    {
        ICliAddIn AddIn { get; }
        string Alias { get; }
        INode Node { get; }
        void Process(IList<ICliCommandEntry> entries);
        string GetHelp();
    }
}
