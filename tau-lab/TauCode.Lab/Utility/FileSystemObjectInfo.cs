using System.Diagnostics;

namespace TauCode.Lab.Utility
{
    [DebuggerDisplay("{" + nameof(Name) + "}")]
    public class FileSystemObjectInfo
    {
        private FileSystemObjectInfo(FileSystemObjectType type, bool? isBaseDirectory, string name)
        {
            // todo checks
            this.Type = type;
            this.IsBaseDirectory = isBaseDirectory;
            this.Name = name;
        }

        public FileSystemObjectType Type { get; }
        public bool? IsBaseDirectory { get; }
        public string Name { get; }


        public static FileSystemObjectInfo CreateDirectory(bool isBaseDirectory, string name)
        {
            return new FileSystemObjectInfo(FileSystemObjectType.Directory, isBaseDirectory, name);
        }

        public static FileSystemObjectInfo CreateFile(string name)
        {
            return new FileSystemObjectInfo(FileSystemObjectType.File, null, name);
        }
    }
}
