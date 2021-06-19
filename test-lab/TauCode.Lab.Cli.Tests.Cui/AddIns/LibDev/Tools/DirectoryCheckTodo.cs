namespace TauCode.Lab.Cli.Tests.Cui.AddIns.LibDev.Tools
{
    public class DirectoryCheckTodo
    {
        public DirectoryCheckTodo(string localName, bool isMandatory)
        {
            this.LocalName = localName;
            this.IsMandatory = isMandatory;
        }

        public string LocalName { get; }
        public bool IsMandatory { get; }
    }
}
