using System;
using System.Collections.Generic;
using TauCode.Cli.TextClasses;
using TauCode.Parsing;
using TauCode.Parsing.Building;
using TauCode.Parsing.TextClasses;
using TauCode.Parsing.TinyLisp;
using TauCode.Parsing.TinyLisp.Data;

namespace TauCode.Cli
{
    public class CliNodeFactory : NodeFactoryBase
    {
        public CliNodeFactory(
            string nodeFamilyName)
            : base(
                nodeFamilyName,
                new List<ITextClass>
                {
                    KeyTextClass.Instance,
                    PathTextClass.Instance,
                    TermTextClass.Instance,
                    StringTextClass.Instance,
                    UrlTextClass.Instance,
                }, 
                true)
        {
        }

        public override INode CreateNode(PseudoList item)
        {
            var car = item.GetCarSymbolName().ToLowerInvariant();
            if (car == "worker")
            {
                throw new NotImplementedException();
            }

            return base.CreateNode(item);
        }
    }
}
