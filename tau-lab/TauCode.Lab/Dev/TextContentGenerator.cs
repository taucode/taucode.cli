using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TauCode.Lab.Dev
{
    // todo arrange nicely
    public abstract class TextContentGenerator : IContentGenerator
    {
        public void WriteContent(Stream stream)
        {
            using var streamWriter = new StreamWriter(stream, Encoding.UTF8, 10000, true); // todo const hardcoded
            this.WriteContentImpl(streamWriter);
        }

        protected abstract void WriteContentImpl(StreamWriter streamWriter);

        public async Task WriteContentAsync(Stream stream, CancellationToken cancellationToken = default)
        {
            await using var streamWriter = new StreamWriter(stream, Encoding.UTF8, 10000, true); // todo const hardcoded
            await this.WriteContentAsyncImpl(streamWriter, cancellationToken);
        }

        protected abstract Task WriteContentAsyncImpl(StreamWriter streamWriter, CancellationToken cancellationToken);
    }
}