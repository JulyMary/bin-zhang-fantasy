using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fantasy
{
    public static class DictionaryExtensions
    {
        public static TValue GetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key, TValue @default)
        {
            TValue rs;
            if (dict.TryGetValue(key, out rs))
            {
                return rs;
            }
            else
            {
                return @default;
            }
        }

        public static TValue GetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key)
        {
            TValue rs;
            if (dict.TryGetValue(key, out rs))
            {
                return rs;
            }
            else
            {
                return default(TValue);
            }
        }
   
    }
}
