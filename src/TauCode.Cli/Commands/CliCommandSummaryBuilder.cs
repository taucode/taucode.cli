using System.Collections.Generic;
using System.Linq;
using TauCode.Cli.Descriptors;
using TauCode.Cli.Exceptions;

namespace TauCode.Cli.Commands
{
    public class CliCommandSummaryBuilder
    {
        public CliCommandSummary Build(CliExecutorDescriptor descriptor, IList<CliCommandEntry> entries)
        {
            var keys = new Dictionary<string, IList<string>>();

            #region keys

            foreach (var keyDescriptor in descriptor.Keys)
            {
                var keyEntries = entries.GetKeyEntries(keyDescriptor.Alias);
                var keyValues = keyEntries.Select(x => x.Value).ToList();
                if (keyEntries.Length == 0)
                {
                    if (keyDescriptor.IsMandatory)
                    {
                        throw new CliException(
                            $"Mandatory key with alias '{keyDescriptor.Alias}' ({string.Join(", ", keyDescriptor.Keys)}) was not provided.");
                    }
                }

                if (keyEntries.Length > 1 && !keyDescriptor.AllowsMultiple)
                {
                    throw new CliException(
                        $"Key with alias '{keyDescriptor.Alias}' ({string.Join(", ", keyDescriptor.Keys)}) does not allow multiple entries. Your provided: {string.Join(", ", keyValues)}.");
                }

                if (keyDescriptor.ValueDescriptor.Values != null)
                {
                    foreach (var keyValue in keyValues)
                    {
                        if (!keyDescriptor.ValueDescriptor.Values.Contains(keyValue))
                        {
                            throw new CliException(
                                $"Provided value '{keyValue}' for key with alias '{keyDescriptor.Alias}' ({string.Join(", ", keyDescriptor.Keys)}) is not acceptable. Acceptable values are: {string.Join(", ", keyDescriptor.ValueDescriptor.Values)}");
                        }
                    }
                }

                keys.Add(keyDescriptor.Alias, keyEntries.Select(x => x.Value).ToList());
            }

            #endregion

            var arguments = new Dictionary<string, IList<string>>();

            #region arguments

            foreach (var argumentDescriptor in descriptor.Arguments)
            {
                var argumentValues = entries.GetArguments(argumentDescriptor.Alias);
                if (argumentValues.Length == 0 && argumentDescriptor.IsMandatory)
                {
                    throw new CliException($"Mandatory argument with alias '{argumentDescriptor.Alias}' was not provided.");
                }

                if (argumentValues.Length > 1 && !argumentDescriptor.AllowsMultiple)
                {
                    throw new CliException(
                        $"Argument with alias '{argumentDescriptor.Alias}' was provided more than once: {string.Join(", ", argumentValues)}.");
                }

                arguments.Add(argumentDescriptor.Alias, argumentValues);
            }

            #endregion

            var options = new HashSet<string>();

            #region options

            foreach (var optionDescriptor in descriptor.Options)
            {
                var containsOption = entries.ContainsOption(optionDescriptor.Alias);
                if (containsOption)
                {
                    options.Add(optionDescriptor.Alias);
                }
            }

            #endregion

            return new CliCommandSummary(keys, arguments, options);
        }
    }
}
