using System.Collections.Generic;

namespace TauCode.Cli.Data
{
    public class CliWorkerDescriptor
    {
        public string Verb { get; set; }
        public string Description { get; set; }
        public List<string> UsageSamples { get; set; } = new List<string>();
        public List<CliWorkerKeyDescriptor> Keys { get; set; }
        public List<CliWorkerArgumentDescriptor> Arguments { get; set; }
        public List<CliWorkerOptionDescriptor> Options { get; set; }
    }
}
