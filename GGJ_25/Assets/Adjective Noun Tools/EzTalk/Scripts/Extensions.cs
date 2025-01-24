namespace ANT.EzTalk
{
    public static partial class Extensions
    {
        public static bool IsEmptyString(this string message)
        {
            if (message == null) return true;
            message.Trim(new char[] { ' ', '\n' });
            return message == "";
        }
    }
}