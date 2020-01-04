using TauCode.Cli.Data;
using TauCode.Parsing.Lab;
using TauCode.Parsing.Lab.Exceptions;

namespace TauCode.Cli
{
    public class CliParser : ParserLab
    {
        protected override object[] ProcessStopParsingExceptionLab(StopParsingExceptionLab ex)
        {
            var version = (string)ex.Info;

            return new object[]{new CliCommand
            {
                AddInName = "VersionGetter",
            }};
        }
    }
}
