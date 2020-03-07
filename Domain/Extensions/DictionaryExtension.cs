using System.Collections.Generic;

namespace Domain.Extensions
{
    public static class DictionaryExtension
    {
        public static object GetValue(this IReadOnlyDictionary<string, object> dictionary, string key)
        {
            dictionary.TryGetValue(key, out object value);
            if (value == null) return "null";
            if(value != null && value.GetType() == typeof(string))
            {
                value = value.ToString().Replace('\"', '\'');
                value = value.ToString().Replace('\r', ' ');
                value = value.ToString().Replace('\n', ' ');
                value = "\""+value+"\"";
            }
            return value;
        }
    }
}
