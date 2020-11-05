using System;
using System.Collections.Generic;

namespace SSCMS.Login.Core
{
    public static class CacheUtils
    {
        public static string GetCacheKey(string nameofClass, string nameofMethod, params string[] values)
        {
            var key = $"SSCMS.Login.Core.{nameofClass}.{nameofMethod}";
            if (values == null || values.Length <= 0) return key;
            foreach (var t in values)
            {
                key += "." + t;
            }
            return key;
        }


        public static T Get<T>(string key) where T : class
        {
            return default;
        }
    }
}
