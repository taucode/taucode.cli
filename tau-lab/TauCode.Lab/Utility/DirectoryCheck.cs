namespace TauCode.Lab.Utility
{
    public class DirectoryCheck : FileSystemObjectCheck
    {
        public DirectoryCheck(string localPath, bool isMandatory)
            : base(FileSystemObjectType.Directory, localPath, isMandatory)
        {
        }
    }
}
