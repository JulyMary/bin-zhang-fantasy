using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.XSerialization;
using System.Text.RegularExpressions;
using Fantasy.Jobs.Properties;
using Fantasy.Jobs.Resources;

namespace Fantasy.Jobs
{
    [Instruction]
    [XSerializable("using", NamespaceUri = Consts.XNamespaceURI)]
    internal class Using : Sequence
    {
        public override void Execute()
        {
            string[] strs = new string[] { this.Res, this.Res1, this.Res2, this.Res3, this.Res4, this.Res5, this.Res6, this.Res7, this.Res8, this.Res9 };
            List<ResourceParameter> parameters = new List<ResourceParameter>();
            foreach (string str in strs)
            {
                if (!string.IsNullOrWhiteSpace(str))
                {
                    ResourceParameter parameter = CreateParameterFromString(str);
                    parameters.Add(parameter);
                }
            }

            IResourceService svc = this.Site.GetService<IResourceService>();
            if (parameters.Count > 0 && svc != null)
            {
                using (IResourceHandle handle = svc.Request(parameters.ToArray()))
                {
                    this.ExecuteSequence();
                }
            }
            else
            {
                this.ExecuteSequence();
            }
        }

        private ResourceParameter CreateParameterFromString(string text)
        {

            ResourceParameter rs = new ResourceParameter();

            IStringParser parser = this.Site.GetRequiredService<IStringParser>();
            Dictionary<string, object> ctx = new Dictionary<string, object>();
            Regex itemExpr = new Regex(@"(?<key>[^=]*)=(?<value>(""(\\""|[^""])*\"")|('(\\'|[^'])*\')|([^;]*));?");
            Regex quotaExpr = new Regex(@"^'(?<value>(\\'|[^'])*)\'$");
            Regex dquotaExpr = new Regex(@"^""(?<value>(\\""|[^""])*)\""$");

            foreach (Match itemMatch in itemExpr.Matches(text))
            {
                string key = itemMatch.Groups["key"].Value;
                string value = itemMatch.Groups["value"].Value;
                bool isCsString = false;
                
                foreach (Regex strReg in new Regex[] { quotaExpr, dquotaExpr })
                {
                    Match strMatch = strReg.Match(value);
                    if (strMatch.Success)
                    {
                        value = strMatch.Groups["value"].Value;
                        isCsString = true;
                        break;
                    }
                }
               
                ctx["c#-style-string"] = isCsString;

                value = parser.Parse(value, ctx);

                if (isCsString)
                {
                    value = DecodeCsString(value);
                }

                if (StringComparer.OrdinalIgnoreCase.Compare("name", key) == 0)
                {
                    rs.Name = value;
                }
                else
                {
                    rs.Values.Add(key, value);
                }
            }

            if (rs.Name == null)
            {
                throw new JobException(string.Format(Properties.Resources.MissingResourceNameText, text));
            }

            return rs;


        }

        private string DecodeCsString(string text)
        {
            Regex reg = new Regex(@"\\(0x(?<hex4>[0-9a-fA-F]{4})|0x(?<hex2>[0-9a-fA-F]{2})|(?<oct>[0-7]{3})|(?<char>.?))");
            StringBuilder rs = new StringBuilder();
            int s = 0;
            while (s < text.Length)
            {
                Match m = reg.Match(text, s);
                if (m.Success)
                {
                    rs.Append(text.Substring(s, m.Index - s));

                    char value;
                    if (m.Groups["hex4"].Success)
                    {
                        value = Convert.ToChar(Convert.ToInt32(m.Groups["hex4"].Value, 16));
                    }
                    else if (m.Groups["hex2"].Success)
                    {
                        value = Convert.ToChar(Convert.ToInt32(m.Groups["hex2"].Value, 16));
                    }
                    else if (m.Groups["oct"].Success)
                    {
                        value = Convert.ToChar(Convert.ToInt32(m.Groups["oct"].Value, 8));
                    }
                    else
                    {
                        switch (m.Groups["char"].Value)
                        {
                            case "a":
                                value = '\a';
                                break;
                            case "b":
                                value = '\b';
                                break;
                            case "f":
                                value = '\f';
                                break;
                            case "n":
                                value = '\n';
                                break;
                            case "r":
                                value = '\r';
                                break;
                            case "t":
                                value = '\t';
                                break;
                            case "v":
                                value = '\v';
                                break;
                            case "'":
                                value = '\'';
                                break;
                            case "\\":
                                value = '\\';
                                break;
                            default:
                                throw new InvalidOperationException(String.Format("Unrecognized escape sequences in string '{0}'.", text));

                        }
                    }
                    rs.Append(value);
                    s = m.Index + m.Length;
                }
                else
                {
                    rs.Append(text.Substring(s));
                    s = text.Length;
                }

            }
            return rs.ToString();
        }

        [XAttribute("res")]
        public string Res { get; set; }

        [XAttribute("res1")]
        public string Res1 { get; set; }

        [XAttribute("res2")]
        public string Res2 { get; set; }

        [XAttribute("res3")]
        public string Res3 { get; set; }

        [XAttribute("res4")]
        public string Res4 { get; set; }

        [XAttribute("res5")]
        public string Res5 { get; set; }

        [XAttribute("res6")]
        public string Res6 { get; set; }

        [XAttribute("res7")]
        public string Res7 { get; set; }

        [XAttribute("res8")]
        public string Res8 { get; set; }

        [XAttribute("res9")]
        public string Res9 { get; set; }


    }
}
