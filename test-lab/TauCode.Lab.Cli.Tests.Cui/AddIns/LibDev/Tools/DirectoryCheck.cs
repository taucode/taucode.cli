namespace TauCode.Lab.Cli.Tests.Cui.AddIns.LibDev.Tools
{
    public class DirectoryCheck
    {
        public DirectoryCheck(string localName, bool isMandatory)
        {
            this.LocalName = localName;
            this.IsMandatory = isMandatory;
        }

        public string LocalName { get; }
        public bool IsMandatory { get; }
    }
}
