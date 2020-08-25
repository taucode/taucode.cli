using System;
using System.Collections.Generic;
using System.Linq;

namespace TauCode.Cli.Descriptors
{
    public class CliExecutorDescriptor
    {
        public CliExecutorDescriptor(
            string name,
            string verb,
            string description,
            IEnumerable<string> usageSamples,
            IEnumerable<CliExecutorKeyDescriptor> keys,
            IEnumerable<CliExecutorArgumentDescriptor> arguments,
            IEnumerable<CliExecutorOptionDescriptor> options)
        {
            this.Name = name;
            this.Verb = verb;
            this.Description = description;

            #region usage samples

            usageSamples = usageSamples ?? new List<string>();
            var usageSampleList = usageSamples.ToList();
            if (usageSampleList.Any(x => x == null))
            {
                throw new ArgumentException($"'{nameof(usageSamples)}' cannot contain nulls.");
            }
            this.UsageSamples = usageSampleList;

            #endregion

            #region keys

            if (keys == null)
            {
                throw new ArgumentNullException(nameof(keys));
            }

            var keyList = keys.ToList();
            if (keyList.Any(x => x == null))
            {
                throw new ArgumentException($"'{nameof(keys)}' cannot contain nulls.");
            }

            this.Keys = keyList;

            #endregion

            #region arguments

            if (arguments == null)
            {
                throw new ArgumentNullException(nameof(arguments));
            }

            var argumentList = arguments.ToList();
            if (argumentList.Any(x => x == null))
            {
                throw new ArgumentException($"'{nameof(arguments)}' cannot contain nulls.");
            }

            this.Arguments = argumentList;

            #endregion

            #region options

            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            var optionList = options.ToList();
            if (optionList.Any(x => x == null))
            {
                throw new ArgumentException($"'{nameof(options)}' cannot contain nulls.");
            }

            this.Options = optionList;

            #endregion
        }

        public string Name { get; }
        public string Verb { get; }
        public string Description { get; }
        public IReadOnlyList<string> UsageSamples { get; }
        public IReadOnlyList<CliExecutorKeyDescriptor> Keys { get; }
        public IReadOnlyList<CliExecutorArgumentDescriptor> Arguments { get; }
        public IReadOnlyList<CliExecutorOptionDescriptor> Options { get; }
    }
}
