using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TauCode.Lab.Cli.Tests.Cui.AddIns.LibDev.Tools
{
    public class DirectoryCheckRunnerTodo
    {
        public IReadOnlyDictionary<string, DirectoryCheckResultTodo> Run(
            string directory,
            IEnumerable<DirectoryCheckTodo> directoryChecks)
        {
            var di = new DirectoryInfo(directory);
            var directoryChecksDictionary = directoryChecks.ToDictionary(x => x.LocalName, x => x);

            var subDirs = di.GetDirectories();

            var result = new Dictionary<string, DirectoryCheckResultTodo>();

            foreach (var subDir in subDirs)
            {
                DirectoryCheckResultTodo directoryCheckResult;

                if (directoryChecksDictionary.ContainsKey(subDir.Name))
                {
                    directoryCheckResult = DirectoryCheckResultTodo.Ok;
                }
                else
                {
                    directoryCheckResult = DirectoryCheckResultTodo.Unexpected;
                }

                result.Add(subDir.Name, directoryCheckResult);
            }

            foreach (var key in directoryChecksDictionary.Keys)
            {
                if (!result.ContainsKey(key))
                {
                    var directoryCheck = directoryChecksDictionary[key];
                    if (directoryCheck.IsMandatory)
                    {
                        result.Add(key, DirectoryCheckResultTodo.Missing);
                    }
                }
            }

            return result;
        }
    }
}
