using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using TauCode.Lab.Dev;

namespace TauCode.Lab.Cli.Tests.Cui.AddIns.LibDev.ContentGenerators
{
    public class ChangeLogFileGenerator : TextContentGenerator
    {
        protected override void WriteContentImpl(StreamWriter streamWriter)
        {
            const string dateFormat = "yyyy-MM-dd";

            streamWriter.WriteLine(DateTimeOffset.UtcNow.ToString(dateFormat));
            streamWriter.WriteLine(new string('=', dateFormat.Length));
            streamWriter.WriteLine("    1. Initial version.");
        }

        protected override Task WriteContentAsyncImpl(StreamWriter streamWriter, CancellationToken cancellationToken)
        {
            throw new NotSupportedException();
        }
    }
}
