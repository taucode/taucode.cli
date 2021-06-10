using System.Collections.Generic;
using System.Xml.Serialization;
using TauCode.Lab.Xml;

namespace TauCode.Lab.Dev.Data
{
    public class Project : ComplexElementBase, IDocument
    {
        #region Nested

        public class PropertyGroup : ComplexElementBase
        {
            [TextNodeElementValue]
            public bool? IsPackable { get; set; }

            [TextNodeElementValue]
            public string OutputType { get; set; }

            [TextNodeElementValue]
            public string TargetFramework { get; set; }

            [TextNodeElementValue]
            public string UserSecretsId { get; set; }

            [TextNodeElementValue]
            public string DockerDefaultTargetOS { get; set; }

            [TextNodeElementValue]
            public string DockerfileContext { get; set; }
        }

        public class PackageReference : ComplexElementBase
        {
            public string Include { get; set; }
            public string Version { get; set; }
        }

        public class Folder : ComplexElementBase
        {
            public string Include { get; set; }
        }

        public class ProjectReference : ComplexElementBase
        {
            public string Include { get; set; }
        }


        public class ItemGroup : ComplexElementBase
        {
            public IList<PackageReference> PackageReferences { get; set; } = new List<PackageReference>();
            public IList<Folder> Folders { get; set; } = new List<Folder>();
            public IList<ProjectReference> ProjectReferences { get; set; } = new List<ProjectReference>();
        }

        #endregion

        [XmlIgnore]
        public string Name { get; internal set; }

        public string Sdk { get; set; }

        public IList<PropertyGroup> PropertyGroups { get; set; } = new List<PropertyGroup>();

        public IList<ItemGroup> ItemGroups { get; set; } = new List<ItemGroup>();

        //public IList<PackageReference> PackageReferences { get; } = new List<PackageReference>();
        //public IList<ProjectReference> ProjectReferences { get; } = new List<ProjectReference>();
        //public IList<LocalFileReference> ExcludedFiles { get; } = new List<LocalFileReference>();
        //public IList<LocalFileReference> EmbeddedResources { get; } = new List<LocalFileReference>();
        //public IList<LocalFileReference> Folders { get; } = new List<LocalFileReference>();

        [XmlIgnore]
        public Declaration Declaration { get; set; }

        [XmlIgnore]
        public string Xmlns { get; set; }

        [XmlIgnore]
        public string RootElementName => "Project";
    }
}
