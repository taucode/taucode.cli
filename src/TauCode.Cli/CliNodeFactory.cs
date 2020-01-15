using System.Collections.Generic;
using TauCode.Cli.TextClasses;
using TauCode.Parsing;
using TauCode.Parsing.Building;
using TauCode.Parsing.TextClasses;

namespace TauCode.Cli
{
    public class CliNodeFactory : NodeFactoryBase
    {
        protected CliNodeFactory(
            string nodeFamilyName,
            IList<ITextClass> textClasses,
            bool isCaseSensitive)
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
    }
}
