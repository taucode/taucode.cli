using System.Collections.Generic;

namespace TauCode.Cli.Reading
{
    public class Command
    {
        private readonly List<string> _parameterValues;
        private readonly List<NamedParameter> _namedParameters;

        internal Command(string name)
        {
            this.Name = name;
            _namedParameters = new List<NamedParameter>();
            _parameterValues = new List<string>();
        }

        public string Name { get; }
        public IReadOnlyList<string> ParameterValues => _parameterValues;
        public IReadOnlyList<NamedParameter> NamedParameters => _namedParameters;

        internal void AddNamedParameter(NamedParameter namedParameter)
        {
            _namedParameters.Add(namedParameter);
        }

        internal void AddParameterValue(string value)
        {
            _parameterValues.Add(value);
        }
    }
}
