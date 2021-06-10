using System;

namespace TauCode.Lab.Dev.Data
{
    public class ProjectType
    {
        public static readonly ProjectType SolutionFolder = new ProjectType(new Guid("2150e333-8fdc-42a3-9474-1a3956d46de8"), "Solution folder");
        public static readonly ProjectType DotNetCore = new ProjectType(new Guid("9a19103f-16f7-4668-be54-9a1e7a4f7556"), ".NET Core");
        public static readonly ProjectType CSharp = new ProjectType(new Guid("fae04ec0-301f-11d3-bf4b-00c04f79efbc"), "C#");

        private ProjectType(Guid guid, string description)
        {
            this.Guid = guid;
            this.Description = description;
        }

        public Guid Guid { get; set; }
        public string Description { get; set; }
    }
}
