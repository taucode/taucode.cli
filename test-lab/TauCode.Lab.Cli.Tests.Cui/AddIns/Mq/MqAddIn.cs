using System.Collections.Generic;
using TauCode.Cli;
using TauCode.Lab.Cli.Tests.Cui.AddIns.Mq.Executors;

namespace TauCode.Lab.Cli.Tests.Cui.AddIns.Mq
{
    public class MqAddIn : CliAddInBase
    {
        public MqAddIn()
            : base("mq", "1.0", true)
        {   
        }

        protected override IReadOnlyList<ICliExecutor> CreateExecutors()
        {
            return new List<ICliExecutor>
            {
                new StartPublisherExecutor(),
            };
        }
    }
}
