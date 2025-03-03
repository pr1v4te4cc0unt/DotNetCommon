namespace Common.EnumUtil
{
    public static class EnumHelper
    {
        public static T TryParse<T>(string val)
        {
            object enumeration = null;

            Enum.TryParse(typeof(T), val, true, out enumeration);

            return (T)enumeration;
        }
    }
}
