using System.Text;
using TauCode.Cli.CommandSummary;

namespace TauCode.Cli.Tests.Common
{
    public static class CommonHelper
    {
        public static string FormatCommandSummary(this CliCommandSummary commandSummary)
        {
            var sb = new StringBuilder();

            #region keys

            sb.Append("Keys:");
            if (commandSummary.Keys.Count == 0)
            {
                sb.AppendLine(" none");
            }
            else
            {
                sb.AppendLine();
                foreach (var pair in commandSummary.Keys)
                {
                    sb.AppendLine($"{pair.Key} : {string.Join(", ", pair.Value)}");
                }
            }

            #endregion

            #region arguments

            sb.Append("Arguments:");
            if (commandSummary.Arguments.Count == 0)
            {
                sb.AppendLine(" none");
            }
            else
            {
                sb.AppendLine();
                foreach (var pair in commandSummary.Arguments)
                {
                    sb.AppendLine($"{pair.Key} : {string.Join(", ", pair.Value)}");
                }
            }

            #endregion

            #region options

            sb.Append("Options:");
            if (commandSummary.Arguments.Count == 0)
            {
                sb.AppendLine(" none");
            }
            else
            {
                sb.AppendLine();
                foreach (var option in commandSummary.Options)
                {
                    sb.AppendLine(option);
                }
            }

            #endregion

            return sb.ToString();
        }
    }
}
