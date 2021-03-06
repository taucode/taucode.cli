﻿using TauCode.Extensions;

namespace TauCode.Cli.Tests.Common.Hosts.Tau.Db.Executors
{
    public class ConvertMetadataExecutor : CommonExecutor
    {
        public ConvertMetadataExecutor()
            : base(
                typeof(DbAddIn).Assembly.GetResourceText("ConvertMetadata.lisp", true),
                null,
                true)
        {
        }
    }
}
