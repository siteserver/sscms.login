using System;
using System.Collections.Generic;

namespace SSCMS.Login.Core
{
    public class CacheUtils
    {
        public static string GetCacheKey(string nameofClass, string nameofMethod, params string[] values)
        {
            var key = $"SiteServer.Home.Core.{nameofClass}.{nameofMethod}";
            if (values == null || values.Length <= 0) return key;
            foreach (var t in values)
            {
                key += "." + t;
            }
            return key;
        }

        public static T GetCache<T>(string cacheKey) where T : class
        {
            return Get<T>(cacheKey);
        }

        public static int GetIntCache(string cacheKey)
        {
            return GetInt(cacheKey, -1);
        }

        public static DateTime GetDateTimeCache(string cacheKey)
        {
            return GetDateTime(cacheKey, DateTime.MinValue);
        }

        public static void ClearAll()
        {
            
        }

        public static void RemoveByStartString(string startString)
        {
            if (!string.IsNullOrEmpty(startString))
            {
                RemoveByPattern(startString + "([w+]*)");
            }
        }

        public static void RemoveByPattern(string pattern)
        {
            
        }

        /// <summary>
        /// Removes the specified key from the cache
        /// </summary>
        /// <param name="key"></param>
        public static void Remove(string key)
        {
            
        }

        public static void Insert(string key, object obj)
        {
        }

        public static void InsertMinutes(string key, object obj, int minutes)
        {
            InnerInsert(key, obj, null, TimeSpan.FromMinutes(minutes));
        }

        public static void InsertHours(string key, object obj, int hours)
        {
            InnerInsert(key, obj, null, TimeSpan.FromHours(hours));
        }

        public static void Insert(string key, object obj, string filePath)
        {
        }

        public static void Insert(string key, object obj, TimeSpan timeSpan, string filePath)
        {
            InnerInsert(key, obj, filePath, timeSpan);
        }

        private static void InnerInsert(string key, object obj, string filePath, TimeSpan timeSpan)
        {
            if (!string.IsNullOrEmpty(key) && obj != null)
            {
            }
        }

        public static object Get(string key)
        {
            return null;
        }

        public static int GetInt(string key, int notFound)
        {
            var retval = Get(key);
            if (retval == null)
            {
                return notFound;
            }
            return (int)retval;
        }

        public static DateTime GetDateTime(string key, DateTime notFound)
        {
            var retval = Get(key);
            if (retval == null)
            {
                return notFound;
            }
            return (DateTime)retval;
        }

        public static T Get<T>(string key) where T : class
        {
            return default;
        }

        public static List<string> AllKeys
        {
            get
            {
                var keys = new List<string>();

                return keys;
            }
        }
    }
}
