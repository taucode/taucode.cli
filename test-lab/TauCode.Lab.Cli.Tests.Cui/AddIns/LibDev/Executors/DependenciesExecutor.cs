using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TauCode.Cli.Commands;
using TauCode.Extensions;
using TauCode.Lab.Algorithms.Graphs;
using TauCode.Lab.Cli.Tests.Cui.AddIns.LibDev.Data;
using TauCode.Lab.Dev;
using TauCode.Lab.Dev.Data;
using TauCode.Lab.Dev.Instruments;
using TauCode.Lab.Extensions;

namespace TauCode.Lab.Cli.Tests.Cui.AddIns.LibDev.Executors
{
    public class DependenciesExecutor : LibDevExecutor
    {
        public DependenciesExecutor()
            : base(
                typeof(DependenciesExecutor).Assembly.GetResourceText($"{nameof(DependenciesExecutor)}.lisp", true),
                null,
                false)
        {
        }

        public override void Process(IList<CliCommandEntry> entries)
        {
            var summary = (new CliCommandSummaryBuilder()).Build(this.Descriptor, entries);

            var directory = summary.Keys["directory"].SingleOrDefault();
            if (directory == null)
            {
                directory = this.LibDevContext?.CurrentDirectory;
            }

            if (directory == null)
            {
                throw new Exception("Directory not provided, neither explicitly nor via context.");
            }

            var dirInfo = new DirectoryInfo(directory);

            var ide = new Ide();
            var solutionGraph = new SolutionGraph();

            foreach (var subDirInfo in dirInfo.GetDirectories())
            {
                if (!subDirInfo.Name.StartsWith("taucode."))
                {
                    continue;
                }

                var slnFileInfos = subDirInfo.GetFiles("*.sln");
                if (slnFileInfos.Length != 1)
                {
                    this.Output.WriteLine($"Warning: '{subDirInfo.FullName}' does not contain exactly one solution.");
                    continue;
                }

                ide.LoadSolution(slnFileInfos.Single().FullName);
                solutionGraph.AddSolution(ide.Solution);
                ide.CloseSolution();
            }

            foreach (var node in solutionGraph.Nodes.Cast<SolutionNode>())
            {
                var projects = node.Value.GetSolutionProjects().Select(x => x.Project).ToList();

                var packageReferences = projects
                    .SelectMany(x => x.ItemGroups)
                    .SelectMany(x => x.PackageReferences)
                    .Where(x => x.Include.StartsWith("TauCode."))
                    .ToList();

                node.Dependencies = packageReferences;

                foreach (var packageReference in packageReferences)
                {
                    var referencedSolution = solutionGraph.FindSolutionNode(packageReference.Include);
                    if (referencedSolution == null)
                    {
                        this.Output.WriteLine($"Warning: referenced solution '{packageReference.Include}' not found.");
                    }

                    node.DrawEdgeTo(referencedSolution);
                }
            }

            var algorithm = new GraphSlicingAlgorithm<Solution>();
            algorithm.Input = solutionGraph;
            algorithm.Run();

            var slices = algorithm.Output;

            for (var i = 0; i < slices.Count; i++)
            {
                var slice = slices[i];
                var list = slice.Nodes
                    .Cast<SolutionNode>()
                    .ToSortedList(x => x.Value.Name);

                this.Output.WriteLine($"=============== Generation {i} ===============");

                for (var j = 0; j < list.Count; j++)
                {
                    var node = list[j];

                    this.Output.WriteLine(node.Value.Name);

                    if (node.Dependencies.Count > 0)
                    {
                        var dependencies = node.Dependencies
                            .Select(x => x.Include)
                            .Distinct()
                            .ToSortedList(x => x);

                        foreach (var dependency in dependencies)
                        {
                            this.Output.Write("    ");
                            this.Output.WriteLine(dependency);
                        }
                    }

                    this.Output.WriteLine();

                    if (j < list.Count - 1)
                    {
                        this.Output.WriteLine("----------------------------------------------");
                    }
                }
            }
        }
    }
}
