using System;
using System.Collections.Generic;
using TauCode.Cli.Data;
using TauCode.Extensions;

namespace TauCode.Cli.Demo.Hosts.Git.Workers
{
    public class CheckoutWorker : CliWorkerBase
    {
        public CheckoutWorker()
            : base(
                typeof(CheckoutWorker).Assembly.GetResourceText(".Git.NoName.Checkout.lisp", true),
                null,
                true)
        {
        }

        public override void Process(IList<CliCommandEntry> entries)
        {
            throw new NotImplementedException();
        }
    }
}
