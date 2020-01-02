using System;
using System.Text;
using TauCode.Cli.TextClasses;
using TauCode.Cli.TextDecorations;
using TauCode.Parsing.Tokens;

namespace TauCode.Cli
{
    public static class CliHelper
    {
        public static string GetTextTokenRepresentation(TextToken textToken)
        {
            if (textToken.Class == KeyTextClass.Instance)
            {
                HyphenTextDecoration hyphenTextDecoration = (HyphenTextDecoration)textToken.Decoration;
                var sb = new StringBuilder();
                for (var i = 0; i < hyphenTextDecoration.HyphenCount; i++)
                {
                    sb.Append("-");
                }

                sb.Append(textToken.Text);
                return sb.ToString();
            }
            else if (textToken.Class == TermTextClass.Instance)
            {
                return textToken.Text;
            }

            throw new NotImplementedException();
        }
    }
}
