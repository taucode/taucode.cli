using System.Collections.Generic;
using System.Reflection;
using System.Xml.Serialization;
using TauCode.Lab.Xml;

namespace TauCode.Lab.Dev.Data
{
    public class Nuspec : ComplexElementBase, IDocument
    {
        #region Nested

        public class NuspecConverter : DefaultBoundPropertyValueConverter
        {
            public override string ToStringValue(PropertyInfo property, object value)
            {
                if (true.Equals(value))
                {
                    return "true";
                }

                if (false.Equals(value))
                {
                    return "false";
                }

                return base.ToStringValue(property, value);
            }
        }

        public class LicenseElement : TextNodeElementBase
        {
            [AttributeValue("type")]
            public string Type { get; set; }
        }

        public class DependencyElement : ComplexElementBase
        {
            [AttributeValue("id")]
            public string Id { get; set; }

            [AttributeValue("version")]
            public string Version { get; set; }
        }

        public class GroupElement : ComplexElementBase
        {
            [AttributeValue("targetFramework")]
            public string TargetFramework { get; set; }

            [Element("dependency")]
            public IList<DependencyElement> Dependencies { get; set; } = new List<DependencyElement>();
        }

        public class DependenciesElement : ComplexElementBase
        {
            [Element("group")]
            public IList<GroupElement> Groups { get; set; } = new List<GroupElement>();
        }

        public class RepositoryElement : ComplexElementBase
        {
            [AttributeValue("type")]
            public string Type { get; set; }

            [AttributeValue("url")]
            public string Url { get; set; }
        }

        public class MetadataElement : ComplexElementBase
        {
            [TextNodeElementValue("id")]
            public string Id { get; set; }

            [TextNodeElementValue("version")]
            public string Version { get; set; }

            [TextNodeElementValue("authors")]
            public string Authors { get; set; }

            [TextNodeElementValue("owners")]
            public string Owners { get; set; }

            [TextNodeElementValue("requireLicenseAcceptance")]
            public bool RequireLicenseAcceptance { get; set; }

            [Element("license")]
            public LicenseElement License { get; set; }

            [TextNodeElementValue("projectUrl")]
            public string ProjectUrl { get; set; }

            [Element("repository")]
            public RepositoryElement Repository { get; set; }

            [TextNodeElementValue("description")]
            public string Description { get; set; }

            [TextNodeElementValue("releaseNotes")]
            public string ReleaseNotes { get; set; }

            [TextNodeElementValue("tags")]
            public string Tags { get; set; }

            [Element("dependencies")]
            public DependenciesElement Dependencies { get; set; }
        }

        public class FileElement : ComplexElementBase
        {
            [AttributeValue("src")]
            public string Src { get; set; }

            [AttributeValue("target")]
            public string Target { get; set; }
        }

        public class FilesElement : ComplexElementBase
        {
            [Element("file")]
            public IList<FileElement> Files { get; set; } = new List<FileElement>(); // todo: must initialize by deserializer
        }

        #endregion

        [Element("metadata")]
        public MetadataElement Metadata { get; set; }

        [Element("files")]
        public FilesElement Files { get; set; }

        #region IDocument Members

        [XmlIgnore]
        public Declaration Declaration { get; set; } = new Declaration("1.0", "utf-8", "yes");

        [XmlIgnore]
        public string Xmlns { get; set; } = "http://schemas.microsoft.com/packaging/2013/05/nuspec.xsd";

        [XmlIgnore]
        public string RootElementName => "package";

        #endregion
    }
}
