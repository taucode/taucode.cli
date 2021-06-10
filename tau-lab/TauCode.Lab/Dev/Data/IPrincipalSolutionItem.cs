using System;
using TauCode.Lab.Dev.Data.SolutionItems;

namespace TauCode.Lab.Dev.Data
{
    public interface IPrincipalSolutionItem
    {
        Guid TypeGuid{ get; }
        string Name{ get; }
        Guid Guid { get; }
        SolutionFolder ParentSolutionFolder { get; }
    }
}
