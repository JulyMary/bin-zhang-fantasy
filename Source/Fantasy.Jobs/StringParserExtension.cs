using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml;

namespace Fantasy.Jobs
{
    public static class  StringParserExtension
    {
        public static XElement Parse(this IStringParser parser, XElement element, IDictionary<string, object> context = null)
        {
            XElement rs = new XElement(element);

            foreach (XElement content in rs.Flatten(e => e.Elements()))
            {
                foreach (XAttribute attr in content.Attributes())
                {
                    attr.Value = XmlNormalizer.Nomarlize(parser.Parse(attr.Value));
                }
                if (!content.HasElements)
                {
                    content.Value = XmlNormalizer.Nomarlize(parser.Parse(content.Value));
                }
            }

            return rs;
        }

    }
}
