using System.Collections.Generic;
using TauCode.Cli.Demo.AddIns;
using TauCode.Parsing.Lexing;

namespace TauCode.Cli.Demo
{
    public class DemoHost : CliHostBase
    {
        public DemoHost()
            : base("demo", "demo-1488", true)
        {

        }

        protected override IEnumerable<ICliAddIn> CreateAddIns()
        {
            return new ICliAddIn[]
            {
                new DbAddIn(this),
            };
        }

        protected override ILexer CreateLexer() => new DemoLexer();
    }
}
