using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TauCode.Lab.Dev.Data;
using TauCode.Lab.Dev.Data.SolutionItems;
using TauCode.Lab.Dev.Instruments.ProjectSerialization;

namespace TauCode.Lab.Dev
{
    public static class DevExtensions
    {
        public static SolutionProject GetSolutionProjectByGuid(this Solution solution, Guid projectGuid)
        {
            // todo checks

            return solution.PrincipalSolutionItems
                .Where(x => x is SolutionProject)
                .Cast<SolutionProject>()
                .Single(x => x.Guid == projectGuid);
        }

        public static SolutionFolder GetSolutionFolderByGuid(this Solution solution, Guid folderGuid)
        {
            // todo checks

            return solution.PrincipalSolutionItems
                .Where(x => x is SolutionFolder)
                .Cast<SolutionFolder>()
                .Single(x => x.Guid == folderGuid);
        }

        public static IPrincipalSolutionItem GetPrincipalSolutionItemByGuid(
            this Solution solution,
            Guid principalItemGuid)
        {
            // todo checks

            return solution.PrincipalSolutionItems
                .Single(x => x.Guid == principalItemGuid);
        }

        public static bool IsNestedInto(this SolutionFolder testChild, SolutionFolder testParent)
        {
            // todo check for nulls

            var circularRefCheck = new HashSet<SolutionFolder>();
            var current = testChild;

            while (true)
            {
                circularRefCheck.Add(current);

                var realParent = current.ParentSolutionFolder;
                if (realParent == testParent)
                {
                    return true;
                }

                if (realParent == null)
                {
                    return false;
                }

                current = realParent;

                if (circularRefCheck.Contains(current))
                {
                    // we are having an error here: circular reference. but this error isn't this function's business; we just answer 'false" to the function's question 'is_nested_into'
                    return false;
                }
            }
        }

        public static IList<SolutionProject> GetSolutionProjects(this Solution solution) =>
            solution.PrincipalSolutionItems
                .Where(x => x is SolutionProject)
                .Cast<SolutionProject>()
                .ToList();

        public static IList<SolutionFolder> GetSolutionFolders(this Solution solution) =>
            solution.PrincipalSolutionItems
                .Where(x => x is SolutionFolder)
                .Cast<SolutionFolder>()
                .ToList();

        public static IList<SolutionProject> GetSolutionFolderProjects(this SolutionFolder solutionFolder)
        {
            return solutionFolder
                .ChildPrincipalSolutionItems
                .Where(x => x is SolutionProject)
                .Cast<SolutionProject>()
                .ToList();
        }

        public static void LoadProjects(this Solution solution)
        {
            var solutionProjects = solution.GetSolutionProjects();

            var projectReader = new ProjectReader();

            foreach (var solutionProject in solutionProjects)
            {
                var filePath = Path.Combine(solution.Directory, solutionProject.LocalProjectDefinitionFilePath);

                var project = projectReader.Read(filePath);
                solutionProject.Project = project;
            }
        }

        public static byte[] GetContent(this IContentGenerator contentGenerator)
        {
            using var stream = new MemoryStream();
            contentGenerator.WriteContent(stream);
            var bytes = stream.ToArray();
            return bytes;
        }
    }
}
