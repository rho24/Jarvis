using System;

namespace Jarvis.Core
{
    public static class Extensions
    {
        public static string ResolvePathAliases(this string path) {
            if (path == null) throw new ArgumentNullException("path");

            return Environment.ExpandEnvironmentVariables(path.Replace(@"~\", @"%USERPROFILE%\"));
        }
    }
}