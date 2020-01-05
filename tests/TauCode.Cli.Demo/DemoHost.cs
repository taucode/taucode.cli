using System;
using System.Collections.Generic;
using System.Text;
using TauCode.Cli.Demo.AddIns;

namespace TauCode.Cli.Demo
{
    public class DemoHost : CliHostBase
    {
        public override ICliAddIn[] CreateAddIns()
        {
            return new ICliAddIn[]
            {
                new DbAddIn(this),
            };
        }
    }
}
