using System;
using TauCode.Extensions;
using TauCode.Lab.Dev.Data;

namespace TauCode.Lab.Dev
{
    internal static class Helper
    {
        internal static bool IsProjectTypeGuid(this Guid guid)
        {
            return guid.IsIn(
                SolutionProjectType.DotNetCore.Guid,
                SolutionProjectType.CSharp.Guid);
        }

        internal static string ToSolutionString(this Guid guid) => $"{guid.ToString("B").ToUpperInvariant()}";
    }
}
