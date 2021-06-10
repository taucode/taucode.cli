using System.Collections.Generic;
using TauCode.Cli;
using TauCode.Lab.Cli.Tests.Cui.AddIns.Db;
using TauCode.Lab.Cli.Tests.Cui.AddIns.LibDev;
using TauCode.Lab.Cli.Tests.Cui.AddIns.Mq;

namespace TauCode.Lab.Cli.Tests.Cui
{
    public class Host : CliHostBase
    {
        public Host()
            : base("cui", "1.0", true)
        {
        }

        protected override IReadOnlyList<ICliAddIn> CreateAddIns()
        {
            return new List<ICliAddIn>
            {
                new DbAddIn(),
                new LibDevAddIn(),
                new MqAddIn(),
            };
        }
    }
}
