using TauCode.Extensions;

namespace TauCode.Cli.Tests.UnitTests
{
    public abstract class HostTestBase
    {
        protected const string DQ = "\"";

        protected abstract ICliHost CreateHost();
        protected ICliHost Host { get; private set; }

        protected void OneTimeSetUpBase()
        {
            this.Host = this.CreateHost();
        }

        protected void SetUpBase()
        {
            this.Host.Output = new StringWriterWithEncoding();
        }

        protected string GetOutput()
        {
            this.Host.Output.Flush();
            var sb = ((StringWriterWithEncoding)this.Host.Output).GetStringBuilder();
            return sb.ToString();
        }
    }
}
