using System;
using System.Linq;

namespace TauCode.Cli.Building
{
    public static class SyntaxBuildingExtensions
    {
        public static NamedParameterSyntaxBuilder Mandatory(this NamedParameterSyntaxBuilder parameterBuilder)
        {
            parameterBuilder.IsMandatory = true;
            return parameterBuilder;
        }

        public static NamedParameterSyntaxBuilder Enum(this NamedParameterSyntaxBuilder namedParameterSyntaxBuilder, params string[] values)
        {
            namedParameterSyntaxBuilder.AddValueBuilderImpl(new EnumValueSyntaxBuilder(values));
            return namedParameterSyntaxBuilder;
        }

        public static CommandSyntaxBuilder AddCommand(this RootSyntaxBuilder rootSyntaxBuilder, string name)
        {
            return rootSyntaxBuilder.AddCommandImpl(name);
        }

        public static CommandSyntaxBuilder AddDefaultCommand(this RootSyntaxBuilder rootSyntaxBuilder)
        {
            return rootSyntaxBuilder.AddDefaultCommandImpl();
        }

        public static NamedParameterSyntaxBuilder AddNamedParameter(
            this CommandSyntaxBuilder commandSyntaxBuilder,
            string name,
            params string[] aliases)
        {
            return commandSyntaxBuilder.AddNamedParameterImpl(name, aliases);
        }

        public static NamedParameterSyntaxBuilder AddNamedParameter(
            this NamedParameterSyntaxBuilder parameterBuilder,
            string name,
            params string[] aliases)
        {
            return parameterBuilder.CommandBuilder.AddNamedParameterImpl(name, aliases);
        }

        public static NamedParameterSyntaxBuilder Any(this NamedParameterSyntaxBuilder parameterBuilder)
        {
            parameterBuilder.AddValueBuilderImpl(new AnyValueSyntaxBuilder());
            return parameterBuilder;
        }

        public static NamedParameterSyntaxBuilder Default(this NamedParameterSyntaxBuilder parameterBuilder, string defaultValue)
        {
            parameterBuilder.DefaultValue = defaultValue;
            return parameterBuilder;
        }

        public static CommandSyntaxBuilder AddCommand(this NamedParameterSyntaxBuilder parameterBuilder, string commandName)
        {
            return parameterBuilder.CommandBuilder.SyntaxBuilder.AddCommand(commandName);
        }

        internal static bool IsValidAlias(this string alias)
        {
            return alias.All(RootSyntaxBuilder.IsValidParameterChar) && alias.StartsWith("-");
        }

        internal static bool HasDefaultCommand(this RootSyntaxBuilder rootSyntaxBuilder)
        {
            return rootSyntaxBuilder.CommandSyntaxBuilders.Any(x => x.Name == null);
        }

        internal static void CheckCanModify(this NamedParameterSyntaxBuilder namedParameterSyntaxBuilder)
        {
            namedParameterSyntaxBuilder.CommandBuilder.SyntaxBuilder.CheckCanModify();
        }

        internal static void CheckCanModify(this CommandSyntaxBuilder commandSyntaxBuilder)
        {
            commandSyntaxBuilder.SyntaxBuilder.CheckCanModify();
        }

        internal static void CheckCanModify(this RootSyntaxBuilder rootSyntaxBuilder)
        {
            if (rootSyntaxBuilder.IsCompleted)
            {
                throw new InvalidOperationException("Completed syntax cannot be modified.");
            }
        }

        public static RootSyntaxBuilder GetRoot(this RootSyntaxBuilder rootSyntaxBuilder)
        {
            return rootSyntaxBuilder;
        }

        public static RootSyntaxBuilder GetRoot(this CommandSyntaxBuilder commandSyntaxBuilder)
        {
            return commandSyntaxBuilder.SyntaxBuilder;
        }

        public static RootSyntaxBuilder GetRoot(this NamedParameterSyntaxBuilder namedParameterSyntaxBuilder)
        {
            return namedParameterSyntaxBuilder.CommandBuilder.SyntaxBuilder;
        }
    }
}
