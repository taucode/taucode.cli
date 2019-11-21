using System.Collections.Generic;

namespace TauCode.Cli.Building
{
    public class EnumValueSyntaxBuilder : ValueSyntaxBuilderBase
    {
        internal EnumValueSyntaxBuilder(params string[] values)
        {
            this.Values = values;
        }

        public IReadOnlyList<string> Values { get; }
    }
}
