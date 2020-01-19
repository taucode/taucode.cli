using System.Collections.Generic;

namespace TauCode.Cli.Data
{
    public class CliWorkerOptionDescriptor
    {
        public string Alias { get; set; }
        public List<string> Options { get; set; } = new List<string>();
        public string Description { get; set; }
    }
}
