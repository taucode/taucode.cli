using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TauCode.Lab.Cli.Tests.Cui.AddIns.LibDev.Tools
{
    public class FileCheckRunnerTodo
    {
        public FileCheckRunnerTodo()
        {
        }

        public IReadOnlyDictionary<string, FileCheckResultTodo> Run(string directory, IEnumerable<FileCheckTodo> fileChecks)
        {
            var di = new DirectoryInfo(directory);
            var files = di.GetFiles();

            var fileChecksDictionary = fileChecks.ToDictionary(x => x.LocalName, x => x);

            var result = new Dictionary<string, FileCheckResultTodo>();

            foreach (var fileInfo in files)
            {
                FileCheckResultTodo fileCheckResult;

                if (fileChecksDictionary.ContainsKey(fileInfo.Name))
                {
                    var fileCheck = fileChecksDictionary[fileInfo.Name];
                    if (fileCheck.ExpectedContentGetter == null)
                    {
                        fileCheckResult = FileCheckResultTodo.Ok;
                    }
                    else
                    {
                        var expectedContent = fileCheck.ExpectedContentGetter();

                        var fileContent = File.ReadAllBytes(fileInfo.FullName);
                        var match = fileContent.SequenceEqual(expectedContent);

                        if (match)
                        {
                            fileCheckResult = FileCheckResultTodo.Ok;
                        }
                        else
                        {
                            fileCheckResult = FileCheckResultTodo.ContentMismatch;
                        }
                    }
                }
                else
                {
                    fileCheckResult = FileCheckResultTodo.Unexpected;
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
                        result.Add(key, FileCheckResultTodo.Missing);
                    }
                }
            }

            return result;
        }
    }
}
