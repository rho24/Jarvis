using System;
using System.Collections.Generic;
using System.Linq;

namespace Jarvis.Core
{
    public static class Extensions
    {
        public static string Fmt(this string fmt, params object[] args) {
            return string.Format(fmt, args);
        }

        public static string ResolvePathAliases(this string path) {
            if (path == null) throw new ArgumentNullException("path");

            return Environment.ExpandEnvironmentVariables(path.Replace(@"~\", @"%USERPROFILE%\"));
        }

        public static IEnumerable<T> Fetch<T>(this IEnumerable<T> items) {
            return items.ToArray();
        }
    }
}