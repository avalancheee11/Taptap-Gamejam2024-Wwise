
public sealed class LocalizedUtils
{
    /// <summary>
    /// 获取国际化key对应的value
    /// </summary>
    /// <returns>The string for key.</returns>
    /// <param name="key">Key.</param>
    /// <param name="toUpper">If set to <c>true</c> to upper.</param>
    // public static string StringForKey(string key) { return LocalizableDataHandler.Instance.LocalizedStringForKey(key); }
    public static string StringForKey(string key)
    {
        return key;
        // var value = LocalizationHandler.Instance.getValue(key);
        // if (!value.IsNullOrEmpty()) {
        //     return value;
        // }
        // return key;
    }
    public static string Format(string format, object arg0) { return string.Format(StringForKey(format), arg0); }
    // public static string Format(string format, params object[] args) { return string.Format(StringForKey(format), args); }
    public static string Format(string format, object arg0, object arg1) { return string.Format(StringForKey(format), arg0, arg1); }
    public static string Format(string format, object arg0, object arg1, object arg2) { return string.Format(StringForKey(format), arg0, arg1, arg2); }
}