using System.Collections.Generic;
using System.Linq;
using TauCode.Cli.Commands;
using TauCode.Extensions;
using TauCode.Lab.Cli.Tests.Cui.AddIns.LibDev.ContentGenerators;
using TauCode.Lab.Dev;
using TauCode.Lab.Dev.Instruments;

namespace TauCode.Lab.Cli.Tests.Cui.AddIns.LibDev.Executors
{
    public class NewLibraryExecutor : LibDevExecutor
    {
        public NewLibraryExecutor()
            : base(
                typeof(NewLibraryExecutor).Assembly.GetResourceText($"{nameof(NewLibraryExecutor)}.lisp", true),
                null,
                false)
        {
        }

        public override void Process(IList<CliCommandEntry> entries)
        {
            var summary = (new CliCommandSummaryBuilder()).Build(this.Descriptor, entries);
            var directory = this.ResolveDirectory(summary);
            var name = summary.Keys["name"].Single();

            var ide = new Ide();
            ide.CreateSolution(name, directory);

            var folders = new[]
            {
                "build",
                "misc",
                "nuget",
                "src",
                "test",
            };

            foreach (var folder in folders)
            {
                ide.CreateSolutionFolder(folder);
                ide.CreateDirectoryInSolution(folder);
            }

            var build = ide.Solution.GetSolutionFolders().Single(x => x.Name == "build");
            var misc = ide.Solution.GetSolutionFolders().Single(x => x.Name == "misc");
            var nuget = ide.Solution.GetSolutionFolders().Single(x => x.Name == "nuget");
            var src = ide.Solution.GetSolutionFolders().Single(x => x.Name == "src");
            var test = ide.Solution.GetSolutionFolders().Single(x => x.Name == "test");

            ide.CreateProject(src, name, IdeProjectType.Library, "src");
            ide.CreateProject(test, $"{name}.Tests", IdeProjectType.NUnitTestProject, "test");

            ide.SaveSolution();

            #region creating files

            #region root items

            ide.CreateFileInSolution(
                @"LICENSE.txt",
                this.GetType().Assembly.GetResourceText("LICENSE.txt", true));

            ide.CreateFileInSolution(
                @"nuget.config",
                this.GetType().Assembly.GetResourceText("nuget.config.xml", true));

            ide.CreateFileInSolution(
                @"nuget.config.dev.xml",
                this.GetType().Assembly.GetResourceText("nuget.config.dev.xml", true));

            ide.CreateFileInSolution(
                @"nuget.config.prod.xml",
                this.GetType().Assembly.GetResourceText("nuget.config.prod.xml", true));



            #endregion

            #region build items

            ide.CreateFileInSolution(
                @"build\azure-pipelines-dev.yml",
                this.GetType().Assembly.GetResourceText("azure-pipelines-dev.yml", true));

            ide.CreateFileInSolution(
                @"build\azure-pipelines-main.yml",
                this.GetType().Assembly.GetResourceText("azure-pipelines-main.yml", true));

            #endregion

            #region misc items

            ide.CreateFileInSolution(
                @"misc\changelog.txt",
                new ChangeLogFileGenerator());

            ide.CreateFileInSolution(
                @"misc\links.txt",
                "");

            ide.CreateFileInSolution(
                @"misc\todo.txt",
                new ToDoFileGenerator());

            #endregion

            #region nuget items

            ide.CreateFileInSolution(
                $@"nuget\{name}.nuspec",
                new NuspecFileGenerator(ide.Solution));

            #endregion

            #endregion

            #region add files to solution folders

            ide.AddSolutionItems(
                build,
                @"build\azure-pipelines-dev.yml",
                @"build\azure-pipelines-main.yml");

            ide.AddSolutionItems(
                misc,
                @"misc\changelog.txt",
                @"misc\links.txt",
                @"misc\todo.txt");

            ide.AddSolutionItems(
                nuget,
                $@"nuget\{ide.Solution.Name}.nuspec");

            #endregion

            ide.SaveSolution();
        }
    }
}
