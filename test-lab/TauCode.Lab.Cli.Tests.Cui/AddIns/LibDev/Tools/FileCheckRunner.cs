using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TauCode.Lab.Cli.Tests.Cui.AddIns.LibDev.Tools
{
    public class FileCheckRunner
    {
        public FileCheckRunner()
        {
        }

        public IReadOnlyDictionary<string, FileCheckResult> Run(string directory, IEnumerable<FileCheck> fileChecks)
        {
            var di = new DirectoryInfo(directory);
            var files = di.GetFiles();

            var fileChecksDictionary = fileChecks.ToDictionary(x => x.LocalName, x => x);

            var result = new Dictionary<string, FileCheckResult>();

            foreach (var fileInfo in files)
            {
                FileCheckResult fileCheckResult;

                if (fileChecksDictionary.ContainsKey(fileInfo.Name))
                {
                    var fileCheck = fileChecksDictionary[fileInfo.Name];
                    if (fileCheck.ExpectedContentGetter == null)
                    {
                        fileCheckResult = FileCheckResult.Ok;
                    }
                    else
                    {
                        var expectedContent = fileCheck.ExpectedContentGetter();

                        var fileContent = File.ReadAllBytes(fileInfo.FullName);
                        var match = fileContent.SequenceEqual(expectedContent);

                        if (match)
                        {
                            fileCheckResult = FileCheckResult.Ok;
                        }
                        else
                        {
                            fileCheckResult = FileCheckResult.ContentMismatch;
                        }
                    }
                }
                else
                {
                    fileCheckResult = FileCheckResult.Unexpected;
                }

                result.Add(fileInfo.Name, fileCheckResult);
            }

            foreach (var key in fileChecksDictionary.Keys)
            {
                if (!result.ContainsKey(key))
                {
                    var fileCheck = fileChecksDictionary[key];
                    if (fileCheck.IsMandatory)
                    {
                        result.Add(key, FileCheckResult.Missing);
                    }
                }
            }

            return result;
        }
    }
}
