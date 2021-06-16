using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TauCode.Lab.Cli.Tests.Cui.AddIns.LibDev.Tools
{
    public class DirectoryCheckRunner
    {
        public IReadOnlyDictionary<string, DirectoryCheckResult> Run(
            string directory,
            IEnumerable<DirectoryCheck> directoryChecks)
        {
            var di = new DirectoryInfo(directory);
            var directoryChecksDictionary = directoryChecks.ToDictionary(x => x.LocalName, x => x);

            var subDirs = di.GetDirectories();

            var result = new Dictionary<string, DirectoryCheckResult>();

            foreach (var subDir in subDirs)
            {
                DirectoryCheckResult directoryCheckResult;

                if (directoryChecksDictionary.ContainsKey(subDir.Name))
                {
                    directoryCheckResult = DirectoryCheckResult.Ok;
                }
                else
                {
                    directoryCheckResult = DirectoryCheckResult.Unexpected;
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
                        result.Add(key, DirectoryCheckResult.Missing);
                    }
                }
            }

            return result;
        }
    }
}
