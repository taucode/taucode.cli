using System;
using System.IO;
using System.Text;

namespace TauCode.Extensions.Lab
{
    public sealed class StringWriterWithEncodingLab : StringWriter
    {
        public StringWriterWithEncodingLab()
            : this(Encoding.UTF8)
        {
        }

        public StringWriterWithEncodingLab(Encoding encoding)
        {
            this.Encoding = encoding ?? throw new ArgumentNullException(nameof(encoding));
        }

        public override Encoding Encoding { get; }
    }

    public static class StringExceptions
    {
        public static string Repeat(this string s, int count)
        {
            // todo check args
            var sb = new StringBuilder();
            for (var i = 0; i < count; i++)
            {
                sb.Append(s);
            }

            return sb.ToString();
        }
    }
}
