using System;
using System.Collections;
using System.IO;
using System.Linq;
using TauCode.Lab.Data.Graphs;

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

        public static INode<FileSystemObjectInfo> ReadDirectory(
            string directoryPath,
            Func<INode<FileSystemObjectInfo>, FileSystemObjectType, string, bool> filter)
        {
            var directoryInfo = new DirectoryInfo(directoryPath);

            return ReadDirectoryInternal(directoryInfo, true, filter);
        }

        private static INode<FileSystemObjectInfo> ReadDirectoryInternal(
            DirectoryInfo directoryInfo,
            bool isBaseDirectory,
            Func<INode<FileSystemObjectInfo>, FileSystemObjectType, string, bool> filter)
        {
            var name = isBaseDirectory ? directoryInfo.FullName : directoryInfo.Name;

            var node = new Node<FileSystemObjectInfo>(FileSystemObjectInfo.CreateDirectory(isBaseDirectory, name));

            var subDirectories = directoryInfo.GetDirectories();

            foreach (var subDirectory in subDirectories)
            {
                var passed = filter?.Invoke(node, FileSystemObjectType.Directory, subDirectory.Name) ?? true;
                if (passed)
                {
                    var subDirectoryNode = ReadDirectoryInternal(subDirectory, false, filter);
                    node.DrawEdgeTo(subDirectoryNode);
                }
            }

            var files = directoryInfo.GetFiles();

            foreach (var file in files)
            {
                var passed = filter?.Invoke(node, FileSystemObjectType.File, file.Name) ?? true;
                if (passed)
                {
                    var info = FileSystemObjectInfo.CreateFile(file.Name);
                    var fileNode = new Node<FileSystemObjectInfo>(info);
                    node.DrawEdgeTo(fileNode);
                }
            }

            return node;
        }

        // todo: very not optimized.
        internal static bool CollectionsAreEquivalent(IEnumerable coll1, IEnumerable coll2)
        {
            var list1 = coll1.Cast<object>().ToList();
            var list2 = coll2.Cast<object>().ToList();

            if (list1.Count == list2.Count)
            {
                for (var i = 0; i < list1.Count; i++)
                {
                    if (Equals(list1[i], list2[i]))
                    {
                        continue;
                    }

                    return false;
                }

                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
