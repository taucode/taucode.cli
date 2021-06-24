namespace TauCode.Lab.Utility.Rho
{
    public class RhoDirectoryNode : RhoStorageNode
    {
        public RhoDirectoryNode(string name, bool isBase)
        {
            // todo checks

            this.Name = name;
            this.IsBase = isBase;
        }

        public bool IsBase { get; }
    }
}
