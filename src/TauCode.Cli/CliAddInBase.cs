using System;
using System.Collections.Generic;

namespace TauCode.Cli
{
    // todo clean up
    public abstract class CliAddInBase : ICliAddIn
    {
        protected CliAddInBase(
            ICliProgram program,
            string name)
        {
            // todo checks
            this.Program = program;
            this.Name = name ?? throw new ArgumentNullException(nameof(name));
            //_processors = processors.ToArray(); // todo checks
        }

        public ICliProgram Program { get; }
        public string Name { get; }
        public string Description => throw new NotImplementedException();
        public bool SupportsHelp => throw new NotImplementedException();
        public bool SupportsVersion => throw new NotImplementedException();
        public string GetVersion()
        {
            throw new NotImplementedException();
        }

        public string GetHelp()
        {
            throw new NotImplementedException();
        }

        public abstract IReadOnlyList<ICliProcessor> GetProcessors();
    }
}
