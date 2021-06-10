using System;
using System.Runtime.Serialization;

namespace TauCode.Cli.Exceptions
{
    [Serializable]
    public class ShellRequestedException : Exception
    {
        public ShellRequestedException(ICliFunctionalityProvider functionalityProvider)
            : base("Shell requested.")
        {
            this.FunctionalityProvider =
                functionalityProvider ?? throw new ArgumentNullException(nameof(functionalityProvider));
        }

        public ICliFunctionalityProvider FunctionalityProvider { get; }

        protected ShellRequestedException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}
