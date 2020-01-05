using System.Collections.Generic;
using TauCode.Cli.Demo.AddIns;

namespace TauCode.Cli.Demo
{
    public class DemoHost : CliHostBase
    {
        public DemoHost()
            : base("demo", "demo-1488", true)
        {

        }

        protected override IReadOnlyList<ICliAddIn> CreateAddIns()
        {
            return new ICliAddIn[]
            {
                new DbAddIn(),
            };
        }
    }
}
