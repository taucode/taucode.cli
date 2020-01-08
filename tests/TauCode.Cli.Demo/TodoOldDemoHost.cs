using System.Collections.Generic;
using TauCode.Cli.Demo.AddIns;

namespace TauCode.Cli.Demo
{
    public class TodoOldDemoHost : CliHostBase
    {
        public TodoOldDemoHost()
            : base("demo", "demo-1.0", true)
        {

        }

        protected override IReadOnlyList<ICliAddIn> CreateAddIns()
        {
            return new ICliAddIn[]
            {
                new TodoOldDbAddIn(),
            };
        }
    }
}
