using System.Linq;
using TauCode.Cli.Building;

namespace TauCode.Cli.Reading
{
    public static class ReadingExtensions
    {
        internal static bool CommandContainsParameterWithName(this Command command, string parameterName)
        {
            return command.NamedParameters.Any(x => x.Name == parameterName);
        }

        internal static bool RequiresValue(this NamedParameterSyntaxBuilder namedParameterSyntaxBuilder)
        {
            return namedParameterSyntaxBuilder.ValueBuilders.Any();
        }

        public static NamedParameter GetSingleParameter(this Command command, string parameterName)
        {
            return command.NamedParameters.Single(x => x.Name == parameterName);
        }
    }
}
