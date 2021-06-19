using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml;
using TauCode.Cli.Commands;
using TauCode.Extensions;
using TauCode.Lab.Data;
using TauCode.Lab.Dev;
using TauCode.Lab.Dev.Data;
using TauCode.Lab.Extensions;
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
                directory = Directory.GetCurrentDirectory();
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
                .Split('\n')
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .ToArray();

            return lines;
        }

        // todo wtf name
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

        internal static string ToPassedString(this bool b)
        {
            return b ? "PASSED" : "FAILED";
        }

        internal static bool ReportCondition(TextWriter textWriter, bool condition, string conditionDescription)
        {
            textWriter.WriteLine($"# Checking condition: '{conditionDescription}'");
            textWriter.WriteLine($"# Result: '{condition.ToPassedString()}'");

            textWriter.WriteLine();

            return condition;
        }

        internal static bool ReportGitResult(
            TextWriter textWriter,
            string workingDirectory,
            string gitArgs,
            string expectedOutput)
        {
            textWriter.WriteLine($"# Checking git command: 'git {gitArgs}'");

            var gitOutput = RunGitCommandReturnText(workingDirectory, gitArgs);
            var condition = gitOutput == expectedOutput;

            if (!condition)
            {
                textWriter.WriteLine("- Expected output was:");
                textWriter.WriteLine(expectedOutput);
                textWriter.WriteLine("- But actually was:");
                textWriter.WriteLine(gitOutput);
            }

            textWriter.WriteLine($"# Result: '{condition.ToPassedString()}'");

            return condition;
        }

        internal static bool ReportGitResult(
            TextWriter textWriter,
            string workingDirectory,
            string gitArgs,
            string[] expectedOutputLines)
        {
            textWriter.WriteLine($"# Checking git command: 'git {gitArgs}'");

            var gitOutputLines = RunGitCommandReturnLines(workingDirectory, gitArgs);
            var condition = CollectionsAreEquivalent(gitOutputLines, expectedOutputLines);

            if (!condition)
            {
                textWriter.WriteLine("- Expected output was:");
                foreach (var expectedOutputLine in expectedOutputLines)
                {
                    textWriter.WriteLine(expectedOutputLine);
                }

                textWriter.WriteLine("- But actually was:");
                foreach (var gitOutputLine in gitOutputLines)
                {
                    textWriter.WriteLine(gitOutputLine);
                }
            }

            textWriter.WriteLine($"# Result: '{condition.ToPassedString()}'");

            return condition;
        }

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

        public static bool IsValidDevVersion(string version)
        {
            var isParsed = SemanticVersion.TryParse(version, out var v);
            if (isParsed)
            {
                var suffixSegments = v.GetSuffixSegments();

                if (
                    suffixSegments.Length == 4 &&
                    suffixSegments[0].IsIn("alpha", "beta", "rc") &&
                    suffixSegments[1].IsInt32() &&
                    suffixSegments[2].IsRegexMatch(@"\d\d\d\d-\d\d-\d\d") &&
                    suffixSegments[3].IsRegexMatch(@"\d\d-\d\d") &&
                    true
                )
                {
                    return true;
                }
            }

            return false;
        }
    }
}
