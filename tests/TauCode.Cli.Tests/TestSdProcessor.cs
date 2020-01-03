using TauCode.Extensions;

namespace TauCode.Cli.Tests
{
    public class TestSdProcessor : CliSubCommandProcessorImpl
    {
        public TestSdProcessor()
            : base(typeof(TestSdProcessor).Assembly.GetResourceText("sd-grammar.lisp", true))
        {
        }
    }
}
