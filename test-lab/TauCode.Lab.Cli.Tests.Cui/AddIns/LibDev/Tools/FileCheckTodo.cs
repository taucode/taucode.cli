using System;

namespace TauCode.Lab.Cli.Tests.Cui.AddIns.LibDev.Tools
{
    public class FileCheckTodo
    {
        public FileCheckTodo(
            string localName,
            bool isMandatory,
            Func<byte[]> expectedContentGetter)
        {
            this.LocalName = localName;
            this.IsMandatory = isMandatory;
            this.ExpectedContentGetter = expectedContentGetter;
        }

        public string LocalName { get; }
        public bool IsMandatory { get; }
        public Func<byte[]> ExpectedContentGetter { get; }
    }
}
