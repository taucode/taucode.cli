namespace TauCode.Lab.Dev.Data.SolutionItems
{
    public class SolutionProjectConfigurationPlatform
    {
        public SolutionProjectConfigurationPlatform(string name, string suffix, string solutionConfigurationPlatform)
        {
            // todo checks
            this.Name = name;
            this.Suffix = suffix;
            this.SolutionConfigurationPlatform = solutionConfigurationPlatform;
        }

        public string Name { get; }
        public string Suffix { get; }
        public string SolutionConfigurationPlatform { get; }
    }
}