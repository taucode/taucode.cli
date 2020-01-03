using System;
using System.Collections.Generic;
using System.Linq;
using TauCode.Parsing;

namespace TauCode.Cli
{
    public abstract class CliAddInImpl : ICliAddIn
    {
        private readonly ICliSubCommandProcessor[] _processors;

        protected CliAddInImpl(
            string name,
            ICliSubCommandProcessor[] processors)
        {
            this.Name = name ?? throw new ArgumentNullException(nameof(name));
            _processors = processors.ToArray(); // todo checks
        }

        public string Name { get; }
        public string Description => throw new NotImplementedException();
        public bool SupportsHelp => throw new NotImplementedException();
        public bool SupportsVersion => throw new NotImplementedException();
        public INode BuildNode()
        {
            throw new NotImplementedException();
        }

        public string GetVersion()
        {
            throw new NotImplementedException();
        }

        public string GetHelp()
        {
            throw new NotImplementedException();
        }

        public IReadOnlyList<ICliSubCommandProcessor> Processors => _processors;
    }
}
