using System;
using System.Collections.Generic;

namespace TauCode.Cli.CommandSummary
{
    public class CliCommandSummary
    {
        public CliCommandSummary(
            IDictionary<string, IList<string>> keys,
            IDictionary<string, IList<string>> arguments,
            IEnumerable<string> options)
        {
            this.Keys = new Dictionary<string, IList<string>>(keys ?? throw new ArgumentNullException(nameof(keys)));
            this.Arguments = new Dictionary<string, IList<string>>(arguments ?? throw new ArgumentNullException(nameof(arguments)));
            this.Options = new HashSet<string>(options ?? throw new ArgumentNullException(nameof(options)));
        }

        public IReadOnlyDictionary<string, IList<string>> Keys { get; }

        public IReadOnlyDictionary<string, IList<string>> Arguments { get; }

        public HashSet<string> Options { get; }
    }
}
