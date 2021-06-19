using System.Diagnostics;

namespace TauCode.Lab.Utility
{
    [DebuggerDisplay("{" + nameof(LocalPath) + "}")]
    public class FileSystemObjectCheck
    {
        protected FileSystemObjectCheck(FileSystemObjectType type, string localPath, bool isMandatory)
        {
            // todo check args
            this.Type = type;
            this.LocalPath = localPath;
            this.IsMandatory = isMandatory;
        }

        public FileSystemObjectType Type { get; }
        public string LocalPath { get; }
        public bool IsMandatory { get; }
    }
}
