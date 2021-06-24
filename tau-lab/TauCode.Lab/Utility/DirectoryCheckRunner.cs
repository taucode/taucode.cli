using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TauCode.Lab.Utility.Rho;

namespace TauCode.Lab.Utility
{
    public class DirectoryCheckRunner
    {
        public DirectoryCheckRunner()
        {
        }

        public void Run(
            string baseDirectoryPath,
            IEnumerable<FileSystemObjectCheck> checks,
            Func<RhoDirectoryNode, bool, string, bool> filter)
        {
            // todo check args

            var baseDirectoryNode = RhoStorageExtensions.LoadDirectory(baseDirectoryPath, filter, out var plainNodes);
            var checkDictionary = checks.ToDictionary(x => x.LocalPath, x => x);

            var keys = checkDictionary.Keys.ToList();

            foreach (var node in plainNodes)
            {
                if (node is RhoDirectoryNode directoryNode && directoryNode.IsBase)
                {
                    continue;
                }

                if (checkDictionary.ContainsKey(node.LocalPath))
                {
                    if (node is RhoDirectoryNode)
                    {
                        // ok
                        checkDictionary.Remove(node.LocalPath);
                        continue;
                    }

                    var fileNode = (RhoFileNode)node;
                    var fileCheck = (FileCheck)checkDictionary[node.LocalPath];
                    if (fileCheck.ContentGetter != null)
                    {
                        var expectedContent = fileCheck.ContentGetter(node.LocalPath);
                        var fullPath = @$"{baseDirectoryNode.Name}\{node.LocalPath}";
                        var realContent = File.ReadAllBytes(fullPath);

                        if (!FileUtility.CollectionsAreEquivalent(realContent, expectedContent))
                        {
                            throw new NotImplementedException();
                        }
                    }

                    checkDictionary.Remove(node.LocalPath);
                }
                else
                {
                    throw new NotImplementedException();
                }
            }

            // not found items
            foreach (var pair in checkDictionary)
            {
                var key = pair.Key;
                var value = pair.Value;

                if (value.IsMandatory)
                {
                    throw new NotImplementedException();
                }
            }
        }
    }
}
