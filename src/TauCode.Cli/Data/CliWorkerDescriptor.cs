using System.Collections.Generic;

namespace TauCode.Cli.Data
{
    public class CliWorkerDescriptor
    {
        public string Verb { get; set; }
        public string Description { get; set; }
        public List<string> UsageSamples { get; set; } = new List<string>();
        public List<CliWorkerKeyDescriptor> Keys { get; set; } = new List<CliWorkerKeyDescriptor>();
        public List<CliWorkerArgumentDescriptor> Arguments { get; set; } = new List<CliWorkerArgumentDescriptor>();
        public List<CliWorkerOptionDescriptor> Options { get; set; } = new List<CliWorkerOptionDescriptor>();
    }
}
