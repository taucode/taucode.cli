using System;
using System.Collections.Generic;
using System.Linq;
using TauCode.Lab.Data.Graphs;

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
            Func<INode<FileSystemObjectInfo>, FileSystemObjectType, string, bool> filter)
        {
            // todo check args

            var baseDirectoryNode = FileUtility.ReadDirectory(baseDirectoryPath, filter);
            var checkDictionary = checks.ToDictionary(x => x.LocalPath, x => x);


            throw new NotImplementedException();

            //var baseDirectoryInfo = new DirectoryInfo(baseDirectory);

            //

            //foreach (var check in checkDictionary.Keys)
            //{
            //    throw new NotImplementedException();
            //}
        }
    }
}
