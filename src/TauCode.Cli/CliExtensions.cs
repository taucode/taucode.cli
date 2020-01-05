using System;

namespace TauCode.Cli
{
    public static class CliExtensions
    {
        public static ICliFunctionalityProvider AddCustomHandler(
            this ICliFunctionalityProvider functionalityProvider,
            Action action,
            params string[] texts)
        {
            throw new NotImplementedException();
            return functionalityProvider;
        }

        public static ICliFunctionalityProvider AddVersion(this ICliFunctionalityProvider functionalityProvider)
        {
            if (functionalityProvider == null)
            {
                throw new ArgumentNullException(nameof(functionalityProvider));
            }

            if (functionalityProvider.Version == null)
            {
                throw new ArgumentException(
                    $"Functionality provider '{functionalityProvider.Name}' doesn't support version.",
                    nameof(functionalityProvider));
            }

            return functionalityProvider.AddCustomHandler(
                () => functionalityProvider.Output.WriteLine(functionalityProvider.Version), 
                "--version");
        }

        public static ICliFunctionalityProvider AddHelp(this ICliFunctionalityProvider functionalityProvider)
        {
            if (functionalityProvider == null)
            {
                throw new ArgumentNullException(nameof(functionalityProvider));
            }

            if (!functionalityProvider.SupportsHelp)
            {
                throw new ArgumentException(
                    $"Functionality provider '{functionalityProvider.Name}' doesn't support help.",
                    nameof(functionalityProvider));
            }

            return functionalityProvider.AddCustomHandler(
                () => functionalityProvider.Output.WriteLine(functionalityProvider.GetHelp()),
                "--help");
        }
    }
}
