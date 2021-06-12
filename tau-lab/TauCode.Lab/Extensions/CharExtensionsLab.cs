namespace TauCode.Lab.Extensions
{
    public static class CharExtensionsLab
    {
        public static bool IsDecimalDigit(this char c)
        {
            return c >= '0' && c <= '9';
        }
    }
}