using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Fantasy.XLogViewer
{
    public class UriHelper
    {
        public static string CreateAnchor(string text)
        {
            if (!string.IsNullOrEmpty(text))
            {
                Regex regex = new Regex(@"((mailto\:)|([a-z]\w*\:\/\/)|([a-z]:\\)|(\\\\))\S*", RegexOptions.IgnoreCase);
                StringBuilder rs = new StringBuilder();

                int pos = 0;

                foreach (Match match in regex.Matches(text))
                {
                    string normal = text.Substring(pos, match.Index - pos);
                    rs.Append(normal);
                    string anchor = string.Format("<a href=\"{0}\">{0}</a>", match.Value);
                    rs.Append(anchor); 
                    pos = match.Index + match.Length;

                }

                if (pos < text.Length)
                {
                    rs.Append(text.Substring(pos));
                }
                return rs.ToString();
            }
            else
            {
                return text;
            }
        }
    }
}
