namespace MyHomePage.Code;

public static class EnumExtensions
{
    public static TEnum ToEnum<TEnum>(this string value, TEnum defaultValue) where TEnum : struct, Enum
    {
        if (Enum.TryParse<TEnum>(value, true, out TEnum result) && Enum.IsDefined(typeof(TEnum), result))
        {
            return result;
        }
        return defaultValue;
    }
}
