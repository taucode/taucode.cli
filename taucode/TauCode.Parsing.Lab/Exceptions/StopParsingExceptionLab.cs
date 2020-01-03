using System;

namespace TauCode.Parsing.Lab.Exceptions
{
    [Serializable]
    public class StopParsingExceptionLab : Exception
    {
        public StopParsingExceptionLab(object info)
            : this("Stop parsing signal thrown.", info)
        {
        }

        public StopParsingExceptionLab(string message, object info)
            : base(message)
        {
            this.Info = info;
        }

        public object Info { get; }
    }
}
