using System;
using System.Collections.Generic;
using System.Text;
using TauCode.Cli.Help.Tokens;
using TauCode.Extensions;
using TauCode.Parsing;
using TauCode.Parsing.Lexing;

namespace TauCode.Cli.Help
{
    internal class HelpBuilder
    {
        public IList<IToken> Lexize(string text)
        {
            ILexer lexer = new HelpLexer();
            var tokens = lexer.Lexize(text);
            return tokens;
        }

        public IList<string> SquashText(string text, int maxLineLength)
        {
            var lines = new List<string>();

            var tokens = this.Lexize(text);
            var index = 0;

            while (true)
            {
                if (index == tokens.Count)
                {
                    break;
                }

                var sbLine = new StringBuilder();
                var lineLength = 0;
                var firstToken = true;
                while (true)
                {
                    if (index == tokens.Count)
                    {
                        break;
                    }

                    var token = tokens[index];
                    if (token is WhiteSpaceToken)
                    {
                        if (firstToken)
                        {
                            firstToken = false;
                            index++;
                            continue;
                        }

                        sbLine.Append(" ");
                        lineLength += 1;
                    }
                    else if (token is HelpTextToken helpToken)
                    {
                        sbLine.Append(helpToken.Text);
                        lineLength += helpToken.ConsumedLength;
                    }
                    else
                    {
                        throw new NotImplementedException(); // cannot be.
                    }

                    firstToken = false;
                    index++;

                    if (lineLength > maxLineLength)
                    {
                        break;
                    }
                }

                lines.Add(sbLine.ToString());
            }

            return lines;
        }

        public void WriteHelp(StringBuilder sb, string helpText, int margin, int maxLength)
        {
            var paddingSpaces = new string(' ', margin);


            var lines = this.SquashText(helpText, maxLength);
            var lastLineLength = GetLastLineLength(sb);

            string firstSpaces;

            if (lastLineLength >= margin - 1)
            {
                sb.AppendLine();
                firstSpaces = paddingSpaces;
            }
            else
            {
                firstSpaces = new string(' ', margin - lastLineLength);
            }

            for (var i = 0; i < lines.Count; i++)
            {
                var line = lines[i];
                var spaces = i == 0 ? firstSpaces : paddingSpaces;
                sb.Append(spaces);
                sb.AppendLine(line);
            }
        }

        public static int GetLastLineLength(StringBuilder sb)
        {
            var initial = sb.Length - 1;
            var index = initial;
            while (true)
            {
                if (index == -1)
                {
                    break;
                }

                var c = sb[index];
                if (c.IsIn(LexingHelper.CR, LexingHelper.LF))
                {
                    break;
                }

                index--;
            }

            var length = initial - index;
            return length;
        }
    }
}
