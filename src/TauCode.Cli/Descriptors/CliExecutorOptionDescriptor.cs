using System;
using System.Collections.Generic;
using System.Linq;

namespace TauCode.Cli.Descriptors
{
    public class CliExecutorOptionDescriptor
    {
        public CliExecutorOptionDescriptor(
            string alias,
            IEnumerable<string> options,
            string description)
        {
            this.Alias = alias ?? throw new ArgumentNullException(nameof(alias));

            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            var optionList = options.ToList();
            if (!optionList.Any())
            {
                throw new ArgumentException($"'{nameof(options)}' cannot be empty.");
            }

            if (optionList.Any(x => x == null))
            {
                throw new ArgumentException($"'{nameof(options)}' cannot contain nulls.");
            }

            if (optionList.Distinct().Count() != optionList.Count)
            {
                throw new ArgumentException($"'{nameof(options)}' must contain unique values.");
            }

            this.Options = optionList;

            this.Description = description;
        }

        public string Alias { get; }
        public List<string> Options { get; }
        public string Description { get; }
    }
}
