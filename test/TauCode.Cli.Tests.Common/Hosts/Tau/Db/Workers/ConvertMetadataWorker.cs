using TauCode.Extensions;

namespace TauCode.Cli.Tests.Common.Hosts.Tau.Db.Workers
{
    public class ConvertMetadataWorker : CommonExecutor
    {
        public ConvertMetadataWorker()
            : base(
                typeof(DbAddIn).Assembly.GetResourceText("ConvertMetadata.lisp", true),
                null,
                true)
        {
        }
    }
}
