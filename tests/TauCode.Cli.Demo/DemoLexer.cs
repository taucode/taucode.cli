using System;
using System.Collections.Generic;
using System.Text;
using TauCode.Parsing.Lexing;

namespace TauCode.Cli.Demo
{
    public class DemoLexer : CliLexer
    {
        protected override TokenExtractorBase CreateKeyExtractor() => new DemoKeyExtractor(this.Environment);
    }
}
