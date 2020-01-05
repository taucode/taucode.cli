using System.Collections.Generic;

namespace TauCode.Cli.Data
{
    public class CliCommand
    {
        public string AddInName { get; set; }
        public string WorkerName { get; set; }
        public IList<ICliCommandEntry> Entries { get; set; } = new List<ICliCommandEntry>();
    }
}
