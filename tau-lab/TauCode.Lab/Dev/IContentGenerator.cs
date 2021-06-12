using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace TauCode.Lab.Dev
{
    public interface IContentGenerator
    {
        void WriteContent(Stream stream);

        Task WriteContentAsync(Stream stream, CancellationToken cancellationToken = default);
    }
}