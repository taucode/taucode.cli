using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TauCode.Lab.Utility.Rho
{
    public static class RhoStorageExtensions
    {
        public static RhoDirectoryNode GetParentDirectory(this RhoStorageNode node)
        {
            return node.IncomingEdges.SingleOrDefault()?.From as RhoDirectoryNode;
        }

        public static RhoDirectoryNode LoadDirectory(
            string directoryPath,
            Func<RhoDirectoryNode, bool, string, bool> filter,
            out IList<RhoStorageNode> plainNodes)
        {
            var directoryInfo = new DirectoryInfo(directoryPath);
            var list = new List<RhoStorageNode>();

            var baseNode = LoadDirectoryInternal(directoryInfo, true, filter, list);

            foreach (var node in list)
            {
                node.ResolveLocalPath();
            }

            plainNodes = list;
            return baseNode;
        }

        private static RhoDirectoryNode LoadDirectoryInternal(
            DirectoryInfo directoryInfo,
            bool isBaseDirectory,
            Func<RhoDirectoryNode, bool, string, bool> filter,
            List<RhoStorageNode> plainNodes)
        {
            var name = isBaseDirectory ? directoryInfo.FullName : directoryInfo.Name;

            var directoryNode = new RhoDirectoryNode(name, isBaseDirectory);
            plainNodes.Add(directoryNode);

            var subDirectories = directoryInfo.GetDirectories();

            foreach (var subDirectory in subDirectories)
            {
                var passed = filter?.Invoke(directoryNode, true, subDirectory.Name) ?? true;
                if (passed)
                {
                    var subDirectoryNode = LoadDirectoryInternal(subDirectory, false, filter, plainNodes);
                    directoryNode.DrawEdgeTo(subDirectoryNode);
                }
            }

            var files = directoryInfo.GetFiles();

            foreach (var file in files)
            {
                var passed = filter?.Invoke(directoryNode, false, file.Name) ?? true;
                if (passed)
                {
                    var fileNode = new RhoFileNode(file.Name);
                    directoryNode.DrawEdgeTo(fileNode);

                    plainNodes.Add(fileNode);
                }
            }

            return directoryNode;
        }

        private static void ResolveLocalPath(this RhoStorageNode node)
        {
            if (node is RhoDirectoryNode directoryNode && directoryNode.IsBase)
            {
                return;
            }

            if (node.LocalPath != null)
            {
                return; // already resolved
            }

            var parent = node.GetParentDirectory();
            string localPath;

            if (parent.IsBase)
            {
                localPath = node.Name;

                // do nothing, not such thing as 'local path' for base directory
            }
            else
            {
                parent.ResolveLocalPath();
                localPath = @$"{parent.LocalPath}\{node.Name}";
            }

            node.LocalPath = localPath;
        }

        public static IReadOnlyList<RhoStorageNode> ToFlatList(this RhoDirectoryNode directoryNode)
        {
            var list = new List<RhoStorageNode>();

            ToFlatListInternal(directoryNode, list);

            return list;
        }

        private static void ToFlatListInternal(RhoDirectoryNode directoryNode, List<RhoStorageNode> list)
        {
            list.Add(directoryNode);

            var subDirectories = directoryNode
                .OutgoingEdges
                .Select(x => x.To)
                .Where(x => x is RhoDirectoryNode)
                .Cast<RhoDirectoryNode>();

            foreach (var subDirectory in subDirectories)
            {
                ToFlatListInternal(subDirectory, list);
            }

            var files = directoryNode
                .OutgoingEdges
                .Select(x => x.To)
                .Where(x => x is RhoFileNode)
                .Cast<RhoFileNode>();

            foreach (var file in files)
            {
                list.Add(file);
            }
        }
    }
}
