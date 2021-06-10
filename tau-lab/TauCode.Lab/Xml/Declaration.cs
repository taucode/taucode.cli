using System;
using System.Text;

namespace TauCode.Lab.Xml
{
    public class Declaration
    {
        public Declaration(string version, string encoding, string standalone)
        {
            if (version != "1.0")
            {
                throw new NotImplementedException();
            }

            this.Version = version;
            this.Encoding = encoding;
            this.Standalone = standalone;
        }
        public string Version { get; }
        public string Encoding { get; }
        public string Standalone { get; }

        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.Append("<?xml");

            if (this.Version != null)
            {
                sb.Append($" version=\"{this.Version}\"");
            }

            if (this.Encoding != null)
            {
                sb.Append($" encoding=\"{this.Encoding}\"");
            }

            if (this.Standalone != null)
            {
                sb.Append($" standalone=\"{this.Standalone}\"");
            }

            sb.Append("?>");

            return sb.ToString();
        }
    }
}
