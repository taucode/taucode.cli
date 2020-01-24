using System.IO;
using TauCode.Cli.Exceptions;
using TauCode.Parsing;

namespace TauCode.Cli
{
    public abstract class CliFunctionalityProviderBase : ICliFunctionalityProvider
    {
        #region Fields

        private INode _root;

        #endregion

        #region Constructor

        protected CliFunctionalityProviderBase(
            string name,
            string version,
            bool supportsHelp)
        {
            this.Name = name; // can be null
            this.Version = version; // can be null
            this.SupportsHelp = supportsHelp;
        }

        #endregion

        #region Private

        #endregion

        #region Protected

        protected abstract INode CreateNodeTree();

        protected abstract string GetHelpImpl();

        protected virtual void OnNodeCreated()
        {
            // idle
        }

        #endregion

        #region ICliFunctionalityProvider Members

        public string Name { get; }
        public abstract TextWriter Output { get; set; }
        public abstract TextReader Input { get; set; }


        public INode Node
        {
            get
            {
                if (_root == null)
                {
                    _root = this.CreateNodeTree();
                    this.OnNodeCreated();
                }

                return _root;
            }
        }

        public string Version { get; }

        public bool SupportsHelp { get; }

        public string GetHelp()
        {
            if (!this.SupportsHelp)
            {
                throw new CliException("Help is not supported.");
            }

            var help = this.GetHelpImpl();
            return help;
        }

        #endregion
    }
}
