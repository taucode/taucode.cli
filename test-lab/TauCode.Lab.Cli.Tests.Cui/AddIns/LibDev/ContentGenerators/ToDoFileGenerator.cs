using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using TauCode.Lab.Dev;

namespace TauCode.Lab.Cli.Tests.Cui.AddIns.LibDev.ContentGenerators
{
    public class ToDoFileGenerator : TextContentGenerator
    {
        protected override void WriteContentImpl(StreamWriter streamWriter)
        {
            const string dateFormat = "yyyy-MM-dd";

            streamWriter.WriteLine(DateTimeOffset.UtcNow.ToString(dateFormat));
            streamWriter.WriteLine(new string('=', dateFormat.Length));
            streamWriter.WriteLine("    1. Prepare for deployment.");
        }

        protected override Task WriteContentAsyncImpl(StreamWriter streamWriter, CancellationToken cancellationToken)
        {
            throw new NotSupportedException();
        }
    }
}
