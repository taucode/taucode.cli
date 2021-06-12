using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TauCode.Extensions;
using TauCode.Lab.Dev;
using TauCode.Lab.Dev.Data;
using TauCode.Lab.Xml;

namespace TauCode.Lab.Cli.Tests.Cui.AddIns.LibDev.ContentGenerators
{
    public class NuspecFileGenerator : TextContentGenerator
    {
        public NuspecFileGenerator(Solution solution)
        {
            this.Solution = solution;
        }

        public Solution Solution { get; }

        protected override void WriteContentImpl(StreamWriter streamWriter)
        {
            var dateString = DateTimeOffset.UtcNow.ToString("yyyy-MM-dd-HH-mm");

            var nuspec = new Nuspec();
            var metaData = nuspec.Metadata = new Nuspec.MetadataElement
            {
                Id = this.Solution.Name,
                Version = $"0.0.1-alpha-{dateString}",
                Authors = "TauCode",
                Owners = "TauCode",
                RequireLicenseAcceptance = false,
                License = new Nuspec.LicenseElement
                {
                    Type = "file",
                    Value = "LICENSE.txt",
                },
                ProjectUrl = $"https://github.com/taucode/{this.Solution.Name.ToLowerInvariant()}",
                Repository = new Nuspec.RepositoryElement
                {
                    Type = "git",
                    Url = $"https://github.com/taucode/{this.Solution.Name.ToLowerInvariant()}",
                },
                Description = string.Join(
                    Environment.NewLine,
                    File.ReadAllLines(
                        $@"{this.Solution.Directory}\readme.md",
                        Encoding.UTF8).Skip(1)),
                ReleaseNotes = @$"{Environment.NewLine}      Initial version.{Environment.NewLine}    ",
                Tags = "taucode todo",
                Dependencies = new Nuspec.DependenciesElement
                {
                    Groups = new List<Nuspec.GroupElement>
                    {
                        new Nuspec.GroupElement
                        {
                            TargetFramework = ".NETStandard2.1",
                        },
                    },
                },
            };

            nuspec.Files = new Nuspec.FilesElement
            {
                Files = new List<Nuspec.FileElement>
                {
                    new Nuspec.FileElement
                    {
                        Src = @"..\LICENSE.txt",
                        Target = @"",
                    },
                    new Nuspec.FileElement
                    {
                        Src = $@"..\src\{this.Solution.Name}\bin\Release\netstandard2.1\{this.Solution.Name}.dll",
                        Target = @"lib\netstandard2.1",
                    },
                    new Nuspec.FileElement
                    {
                        Src = $@"..\src\{this.Solution.Name}\bin\Release\netstandard2.1\{this.Solution.Name}.pdb",
                        Target = @"lib\netstandard2.1",
                    },
                },
            };

            var serializer = new Serializer();
            serializer.Settings = new SerializationSettings
            {
                BoundPropertyValueConverter = new Nuspec.NuspecConverter(),
            };

            var doc = serializer.SerializeDocument(nuspec);
            var docString = doc.ToXmlString();

            streamWriter.WriteLine(docString);
        }

        protected override Task WriteContentAsyncImpl(StreamWriter streamWriter, CancellationToken cancellationToken)
        {
            throw new NotSupportedException();
        }
    }
}
