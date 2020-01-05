using System.Collections.Generic;

namespace TauCode.Cli.Tests.TestCli
{
    public class Host : CliHostBase
    {
        public Host()
            : base("host", "host-1.0", true)
        {   
        }

        protected override IEnumerable<ICliAddIn> CreateAddIns()
        {
            return new ICliAddIn[]
            {
                    new DbAddIn(this),
            };
        }
    }
}
