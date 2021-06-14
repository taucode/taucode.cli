using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml;
using TauCode.Cli.Commands;
using TauCode.Lab.Dev;
using TauCode.Lab.Dev.Data;
using TauCode.Lab.Xml;

namespace TauCode.Lab.Cli.Tests.Cui.AddIns.LibDev
{
    internal static class LibDevHelper
    {
        internal static string ResolveDirectory(this LibDevExecutor executor, CliCommandSummary summary)
        {
            var directory = summary.Keys["directory"].SingleOrDefault();
            if (directory == null)
            {
                directory = executor.LibDevContext?.CurrentDirectory;
            }

            if (directory == null)
            {
                throw new Exception("Directory not provided, neither explicitly nor via context.");
            }

            return directory;
        }

        internal static string RunGitCommandReturnText(string workingDirectory, string arguments)
        {
            using var process = new Process
            {
                StartInfo =
                {
                    FileName = "git.exe",
                    Arguments = arguments,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    WorkingDirectory = workingDirectory,
                },
            };

            process.Start();

            var output = process.StandardOutput.ReadToEnd();

            process.WaitForExit();

            return output;
        }

        internal static string[] RunGitCommandReturnLines(string workingDirectory, string arguments)
        {
            var output = RunGitCommandReturnText(workingDirectory, arguments);

            var lines = output
                .Split('\r', '\n')
                .Select(x => x.Trim())
                .Where(x => x != string.Empty)
                .ToArray();

            return lines;
        }

        internal static bool AreEquivalentTo(this IList<string> lines1, params string[] lines2)
        {
            if (lines1.Count != lines2.Length)
            {
                return false;
            }

            for (var i = 0; i < lines1.Count; i++)
            {
                if (lines1[i] != lines2[i])
                {
                    return false;
                }
            }

            return true;
        }

        internal static Nuspec LoadNuspec(this Solution solution)
        {
            var nuget = solution.GetSolutionFolders().Single(x => x.Name == "nuget");
            var nugetFileLocalPath = nuget.IncludedFileLocalPaths.Single();
            var nugetFileFullPath = Path.Combine(solution.Directory, nugetFileLocalPath);
            var xmlDocument = new XmlDocument();
            xmlDocument.Load(nugetFileFullPath);
            var serializer = new Serializer();
            var nuspec = serializer.DeserializeXmlDocument<Nuspec>(xmlDocument);
            return nuspec;
        }
    }
}
