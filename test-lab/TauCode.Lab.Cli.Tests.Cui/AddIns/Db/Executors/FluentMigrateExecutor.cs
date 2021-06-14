using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TauCode.Cli;
using TauCode.Cli.Commands;
using TauCode.Db;
using TauCode.Db.Extensions;
using TauCode.Db.FluentMigrations;
using TauCode.Extensions;

namespace TauCode.Lab.Cli.Tests.Cui.AddIns.Db.Executors
{
    public class FluentMigrateExecutor : CliExecutorBase
    {
        public FluentMigrateExecutor(Func<string, IDbUtilityFactory> resolver, Assembly migrationsAssembly)
            : base(
                typeof(FluentMigrateExecutor).Assembly.GetResourceText($"{nameof(FluentMigrateExecutor)}.lisp", true),
                null,
                false)
        {
            this.Resolver = resolver ?? throw new ArgumentNullException(nameof(resolver));
            this.MigrationsAssembly = migrationsAssembly;
        }

        public Func<string, IDbUtilityFactory> Resolver { get; }
        public Assembly MigrationsAssembly { get; }

        public override void Process(IList<CliCommandEntry> entries)
        {
            var summary = (new CliCommandSummaryBuilder()).Build(this.Descriptor, entries);

            var connectionString = summary.Keys["connection"].Single();
            var schemaName = summary.Keys["schema"].SingleOrDefault();
            var provider = summary.Keys["provider"].Single();
            var realProviderName = DbProviderNames.Find(provider);

            IDbUtilityFactory factory = this.Resolver(realProviderName);

            var reset = summary.Options.Contains("reset");

            using var connection = factory.CreateConnection();
            connection.ConnectionString = connectionString;
            connection.Open();
            

            if (reset)
            {
                var schemaExplorer = factory.CreateSchemaExplorer(connection);
                schemaExplorer.DropAllTables(schemaName); // todo: this makes schemaName mandatory.
            }

            var migrator = new FluentDbMigrator(
                factory.GetDialect().Name,
                connectionString,
                schemaName,
                this.MigrationsAssembly);

            migrator.Migrate();
        }
    }
}
