using System;
using System.IO;

namespace TauCode.Lab.Utility
{
    public static class FileUtility
    {
        public static void PurgeDirectory(string directoryPath)
        {
            // todo checks

            var di = new DirectoryInfo(directoryPath);
            di.PurgeDirectory();
        }

        public static void PurgeDirectory(this DirectoryInfo directoryInfo)
        {
            if (directoryInfo == null)
            {
                throw new ArgumentNullException(nameof(directoryInfo));
            }

            foreach (var file in directoryInfo.GetFiles())
            {
                file.Delete();
            }

            foreach (var dir in directoryInfo.GetDirectories())
            {
                dir.Delete(true);
            }
        }

        // todo: async 'overloads'?
        public static void CreateDirectoryForFile(string filePath)
        {
            var directoryPath = Path.GetDirectoryName(filePath);
            Directory.CreateDirectory(directoryPath); // todo resharper checks
        }
    }
}
