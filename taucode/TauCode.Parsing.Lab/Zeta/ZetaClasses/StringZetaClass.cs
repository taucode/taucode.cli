using System;

namespace TauCode.Parsing.Lab.Zeta.ZetaClasses
{
    public sealed class StringZetaClass : IZetaClass
    {
        public static StringZetaClass Instance { get; } = new StringZetaClass();

        private StringZetaClass()
        {
        }

        public bool IsConvertibleFrom(IZetaClass anotherClass)
        {
            switch (anotherClass)
            {
                case null:
                    throw new ArgumentNullException(nameof(anotherClass));
                case StringZetaClass dummy:
                    return true;
                default:
                    return false;
            }
        }
    }
}
