using System.Collections.Generic;

namespace TauCode.Cli
{
    public class CliCommand
    {
        public string Name { get; set; }
        public IList<ICliCommandEntry> Entries { get; set; } = new List<ICliCommandEntry>();
    }
}
