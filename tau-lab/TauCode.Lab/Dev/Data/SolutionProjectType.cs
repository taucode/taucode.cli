using System;

namespace TauCode.Lab.Dev.Data
{
    public class SolutionProjectType
    {
        public static readonly SolutionProjectType SolutionFolder = new SolutionProjectType(new Guid("2150e333-8fdc-42a3-9474-1a3956d46de8"), "Solution folder");
        public static readonly SolutionProjectType DotNetCore = new SolutionProjectType(new Guid("9a19103f-16f7-4668-be54-9a1e7a4f7556"), ".NET Core");
        public static readonly SolutionProjectType CSharp = new SolutionProjectType(new Guid("fae04ec0-301f-11d3-bf4b-00c04f79efbc"), "C#");

        private SolutionProjectType(Guid guid, string description)
        {
            this.Guid = guid;
            this.Description = description;
        }

        public Guid Guid { get; set; }
        public string Description { get; set; }
    }
}
