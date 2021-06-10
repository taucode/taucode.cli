using TauCode.Lab.Dev.Data;
using TauCode.Lab.Xml;

namespace TauCode.Lab.Dev.Instruments.ProjectSerialization
{
    internal class ProjectWriter
    {
        private readonly Serializer _serializer;

        internal ProjectWriter()
        {
            _serializer = new Serializer();
        }

        // todo: get rid of 'Definition' substring
        internal void Write(Project project, string projectFilePath)
        {
            var doc = _serializer.SerializeDocument(project);


            // todo clean
            //var projectElement = new ProjectElement(NetCoreCsProjSchemaHolder.Schema);
            //projectElement.SetAttribute(nameof(Project.Sdk), project.Sdk);

            //var propertyGroup = (IComplexElement)projectElement.AddChildElement("PropertyGroup");
            //propertyGroup.AddTextNodeProperty(nameof(Project.TargetFramework), project.TargetFramework);
            //propertyGroup.AddTextNodeProperty(nameof(Project.UserSecretsId), project.UserSecretsId);
            //propertyGroup.AddTextNodeProperty(nameof(Project.DockerDefaultTargetOS), project.DockerDefaultTargetOS);
            //propertyGroup.AddTextNodeProperty(nameof(Project.DockerfileContext), project.DockerfileContext);

            //var doc = _serializer.Serialize(projectElement);

            doc.Save(projectFilePath);
        }
    }
}
