using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace System.Xml
{
    public static class XmlNormalizer
    {
        private static  Regex regex = new Regex(@"[^\x09\x0A\x0D\x20-\uD7FF\uE000-\uFFFD]");

        public static string Nomarlize(string text)
        {
            return regex.Replace(text, string.Empty);
        }
    }
}
