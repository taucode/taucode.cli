using System;
using System.IO;
using System.Linq;

namespace TauCode.Cli
{
    public class CliProgramBase : ICliProgram
    {
        private string[] _arguments;

        public CliProgramBase(
            string name,
            string description)
        {
            this.Name = name; // can be null
            //this.Description = description ?? 
            _arguments = new string[] { };
        }

        public string Name { get; }

        public string[] Arguments
        {
            get => _arguments;
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value));
                }

                if (value.Any(x => x == null))
                {
                    throw new ArgumentException($"'{nameof(Arguments)}' cannot contain nulls.");
                }

                _arguments = value;
            }
        }

        public string Description { get; }
        public bool SupportsHelp { get; }
        public bool SupportsVersion { get; }
        public string GetVersion()
        {
            throw new System.NotImplementedException();
        }

        public string GetHelp()
        {
            throw new System.NotImplementedException();
        }

        public int Run()
        {
            throw new System.NotImplementedException();
        }

        public TextWriter Output { get; set; }
    }
}
