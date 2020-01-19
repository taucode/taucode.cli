using System;
using System.Collections.Generic;

namespace TauCode.Cli.Data
{
    public class CliCommandSummary
    {
        public string AddInName { get; set; }
        public string WorkerName { get; set; }

        /// <summary>
        /// Key-value-pairs of the command
        /// Key of the dictionary: CLI key's alias
        /// Value of the dictionary: Item1 - key itself (e.g. --provider), Item2 - value (e.g. sqlserver)
        /// </summary>
        public Dictionary<string, Tuple<string, string>> Keys { get; set; } =
            new Dictionary<string, Tuple<string, string>>();

        public Dictionary<string, string> Arguments { get; set; } = new Dictionary<string, string>();

        /// <summary>
        /// Options of the command
        /// Key of the dictionary: CLI option's alias
        /// Value of the dictionary: key itself (e.g. --verbose)
        /// </summary>
        public Dictionary<string, string> Options { get; set; } = new Dictionary<string, string>();
    }
}
