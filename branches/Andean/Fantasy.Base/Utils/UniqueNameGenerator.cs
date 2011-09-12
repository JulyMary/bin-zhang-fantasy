using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Fantasy.Utils
{
    public static class UniqueNameGenerator
    {
        public static  string GetName(string defaultName, IEnumerable<string> excludes, StringComparison comparisonType = StringComparison.OrdinalIgnoreCase)
        {
            for (int i = 0; ; i++)
            {
                string rs = i == 0 ? defaultName :string.Format("{0} ({1})", defaultName, i);
                if (!excludes.Any(ex => string.Compare(rs, ex, comparisonType) == 0))
                {
                    return rs;
                }
                
            }
            
        }

        private static Regex codeRegex = new Regex(@"[^_\d\w]");

        public static string GetCodeName(string name)
        {
            if (name == null)
            {
                return null;
            }
            return codeRegex.Replace(name, string.Empty);
        }
    }
}
