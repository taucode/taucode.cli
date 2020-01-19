using System;

namespace TauCode.Cli.Descriptors
{
    public class CliWorkerArgumentDescriptor
    {
        public CliWorkerArgumentDescriptor(
            string alias,
            string description,
            string docSubstitution)
        {
            this.Alias = alias ?? throw new ArgumentNullException(nameof(alias));
            this.Description = description;
            this.DocSubstitution = docSubstitution;
        }

        public string Alias { get; }
        public string Description { get; }
        public string DocSubstitution { get; }
    }
}
