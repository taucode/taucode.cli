using TauCode.Cli.Tests.Common.Hosts.Tau;

namespace TauCode.Cli.Tests.UnitTests.Hosts.Tau
{
    public abstract class TauTestBase : HostTestBase
    {
        protected override ICliHost CreateHost() => new TauHost();
    }
}
