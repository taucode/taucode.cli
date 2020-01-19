using System.Collections.Generic;

namespace TauCode.Cli.Data
{
    public class CliWorkerKeyDescriptor
    {
        public string Alias { get; set; }
        public List<string> Keys { get; set; }
        public CliWorkerValueDescriptor ValueDescriptor { get; set; }
    }
}
