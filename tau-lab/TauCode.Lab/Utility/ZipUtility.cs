using Ionic.Zip;
using System;
using System.IO;

namespace TauCode.Lab.Utility
{
    public class ZipUtility
    {
        public static void UnZip(byte[] zipContent, string targetDirectoryPath)
        {
            // todo checks
            // todo: note about encodings installed. (catch)
            // https://stackoverflow.com/questions/25993251/ibm437-is-not-a-supported-encoding-name-from-zipfile-read-method
            // https://stackoverflow.com/questions/44659499/epplus-error-reading-file

            using var stream = new MemoryStream(zipContent);
            using var zipStream = new ZipInputStream(stream);

            Span<byte> buffer = stackalloc byte[1000];

            while (true)
            {
                var entry = zipStream.GetNextEntry();
                if (entry == null)
                {
                    break;
                }

                if (entry.IsDirectory)
                {
                    var dirPath = Path.GetFullPath($"{targetDirectoryPath}\\{entry.FileName}");
                    Directory.CreateDirectory(dirPath);
                    continue;
                }

                var filePath = Path.GetFullPath($"{targetDirectoryPath}\\{entry.FileName}");
                using var fileStream = new FileStream(filePath, FileMode.Create);
                while (true)
                {
                    var bytesRead = zipStream.Read(buffer);
                    if (bytesRead == 0)
                    {
                        break;
                    }

                    fileStream.Write(buffer.Slice(0, bytesRead));
                }
            }
        }

        public static void UnZip(Stream zipStream, string targetDirectoryPath)
        {
            throw new NotImplementedException();
        }

        public static void UnZip(string zipFilePath, string targetDirectoryPath)
        {
            throw new NotImplementedException();
        }

        public static void ZipFile(string filePath, string targetZipFilePath)
        {
            throw new NotImplementedException();
        }

        public static void ZipDirectory(string directoryPath, string targetZipFilePath)
        {
            throw new NotImplementedException();
        }

        // todo: async implementations.
    }
}
