using System.IO;
using System.Xml;
using TauCode.Lab.Dev.Data;
using TauCode.Lab.Xml;

namespace TauCode.Lab.Dev.Instruments.ProjectSerialization
{
    internal class ProjectReader
    {
        private readonly Serializer _serializer;

        internal ProjectReader()
        {
            _serializer = new Serializer();
        }
        
        internal Project Read(string projectDefinitionFilePath)
        {
            var doc = new XmlDocument();
            doc.Load(projectDefinitionFilePath);

            var project = _serializer.DeserializeXmlDocument<Project>(doc);
            project.Name = Path.GetFileNameWithoutExtension(projectDefinitionFilePath); // todo: get rid of 'Definition' substring

            // todo clean
            //var project = new Project();
            

            //root.WritePropertiesTo(project);

            //var propertyGroups = root.GetChildren()
            //    .Where(x => x is PropertyGroupElement)
            //    .Cast<PropertyGroupElement>()
            //    .ToList();

            //foreach (var propertyGroup in propertyGroups)
            //{
            //    propertyGroup.WritePropertiesTo(project);
            //}

            //var itemGroups = root.GetChildren()
            //    .Where(x => x is ItemGroupElement)
            //    .Cast<ItemGroupElement>()
            //    .ToList();


            //foreach (var itemGroup in itemGroups)
            //{
            //    foreach (var child in itemGroup.GetChildren())
            //    {
            //        if (child is PackageReferenceElement packageReferenceElement)
            //        {
            //            project.PackageReferences.Add(new Project.PackageReference(
            //                packageReferenceElement.Include,
            //                packageReferenceElement.Version));
            //        }
            //        else if (
            //            child is TextNodeElement textNodeElement1 &&
            //            textNodeElement1.GetName() == "ProjectReference")
            //        {
            //            project.ProjectReferences.Add(new Project.ProjectReference(textNodeElement1.GetAttribute("Include")));
            //        }
            //        else if (
            //            child is TextNodeElement textNodeElement2 &&
            //            textNodeElement2.GetName() == "None")
            //        {
            //            project.ExcludedFiles.Add(new LocalFileReference(textNodeElement2.GetAttribute("Remove")));
            //        }
            //        else if (
            //            child is TextNodeElement textNodeElement3 &&
            //            textNodeElement3.GetName() == "EmbeddedResource")
            //        {
            //            project.EmbeddedResources.Add(new LocalFileReference(textNodeElement3.GetAttribute("Include")));
            //        }
            //        else if (
            //            child is TextNodeElement textNodeElement4 &&
            //            textNodeElement4.GetName() == "Folder")
            //        {
            //            project.Folders.Add(new LocalFileReference(textNodeElement4.GetAttribute("Include")));
            //        }
            //        else
            //        {
            //            throw new NotImplementedException();
            //        }
            //    }
            //}

            return project;
        }
    }
}
