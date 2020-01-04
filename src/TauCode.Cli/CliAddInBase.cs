using System;
using System.Collections.Generic;

namespace TauCode.Cli
{
    // todo clean up
    public abstract class CliAddInBase : ICliAddIn
    {
        private readonly string _version;

        protected CliAddInBase(
            ICliProgram program,
            string name,
            string version,
            bool supportsHelp)
        {
            // todo checks
            this.Program = program;
            this.Name = name ?? throw new ArgumentNullException(nameof(name));
            this.SupportsHelp = supportsHelp;
            _version = version;
        }

        public ICliProgram Program { get; }
        public string Name { get; }
        public string Description => throw new NotImplementedException();
        public bool SupportsHelp { get; }
        public string GetVersion() => _version;

        public string GetHelp()
        {
            throw new NotImplementedException();
        }

        public abstract IReadOnlyList<ICliProcessor> GetProcessors();
    }
}
