using System.IO;
using TauCode.Extensions;

namespace TauCode.Cli.Tests.UnitTests
{
    public abstract class HostTestBase
    {
        protected const string DQ = "\"";

        protected abstract ICliHost CreateHost();
        protected ICliHost Host { get; set; }
        protected TextWriter Output;

        protected void OneTimeSetUpBase()
        {
        }

        protected void SetUpBase()
        {
            this.Output = new StringWriterWithEncoding();
            this.Host = this.CreateHost();
            this.Host.Output = this.Output;
        }

        protected string GetOutput()
        {
            this.Host.Output.Flush();
            var sb = ((StringWriterWithEncoding)this.Host.Output).GetStringBuilder();
            return sb.ToString();
        }

        protected THost GetHostAs<THost>() where THost : ICliHost
            => (THost)this.Host;
    }
}
