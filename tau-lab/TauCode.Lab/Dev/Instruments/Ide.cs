using System;
using System.IO;
using TauCode.Lab.Dev.Data;
using TauCode.Lab.Dev.Instruments.ProjectSerialization;
using TauCode.Lab.Dev.Instruments.SolutionSerialization;
using TauCode.Lab.Utility;

namespace TauCode.Lab.Dev.Instruments
{
    public class Ide
    {
        public Solution Solution { get; private set; }

        public void LoadSolution(string solutionFilePath)
        {
            var reader = new SolutionReader();
            this.Solution = reader.Read(solutionFilePath);

            this.Solution.LoadProjects();
        }

        public void SaveSolution()
        {
            // solution itself
            var writer = new SolutionWriter();
            writer.WriteSolution(this.Solution, this.Solution.Directory);

            var solutionProjects = this.Solution.GetSolutionProjects();
            foreach (var solutionProject in solutionProjects)
            {
                throw new NotImplementedException();
            }
        }

        public void CloseSolution()
        {
            this.Solution = null;
        }

        /// <summary>
        /// Relocates solution structure (sub-directories, *.sln, *.csproj) to new directory.
        /// You should not use this method, it is for internal use only.
        /// </summary>
        /// <param name="newSolutionDirectory">Destination solution directory</param>
        public void RelocateSolutionStructureTo(string newSolutionDirectory)
        {
            this.Solution.Directory = newSolutionDirectory;
            var solutionWriter = new SolutionWriter();
            solutionWriter.WriteSolution(this.Solution, this.Solution.Directory);

            var solutionFolders = this.Solution.GetSolutionFolders();

            foreach (var solutionFolder in solutionFolders)
            {
                foreach (var localPath in solutionFolder.IncludedFileLocalPaths)
                {
                    throw new NotImplementedException();
                }
            }

            var solutionProjects = this.Solution.GetSolutionProjects();
            var projectWriter = new ProjectWriter();

            foreach (var solutionProject in solutionProjects)
            {
                var projectFilePath = Path.Combine(
                    this.Solution.Directory,
                    solutionProject.LocalProjectDefinitionFilePath);

                FileUtility.CreateDirectoryForFile(projectFilePath);

                projectWriter.Write(solutionProject.Project, projectFilePath);
            }
        }
    }
}
