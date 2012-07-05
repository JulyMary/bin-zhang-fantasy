using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.Jobs.Properties;
using System.Text.RegularExpressions;
using Fantasy.ServiceModel;

namespace Fantasy.Jobs
{
    public interface IStringParser
    {
        string Parse(string value, IDictionary<string, object> context = null);
      
    }

    public class StringParseService : AbstractService , IStringParser  
    {
        public StringParseService()  
        {
            this._providers = AddIn.CreateObjects<ITagValueProvider>("jobEngine/tagValueProviders/provider");
        }

        protected internal ITagValueProvider[] _providers;

        public string Parse(string value, IDictionary<string, object> context)
        {

            if (value != null)
            {
                if (context == null)
                {
                    context = new Dictionary<string, object>();
                }
                StringBuilder prefixes = new StringBuilder();
                IEnumerable<ITagValueProvider> providers = this._providers.Where(x => x.IsEnabled(context));

                foreach (char prefix in providers.Select(p => p.Prefix).Distinct())
                {
                    prefixes.Append('\\');
                    prefixes.Append(prefix);
                }
                string tagRegex = @"(?<prefix>[" + prefixes.ToString() + @"])\((?<tag>[^)]+)\)"; 
                string rs = InnerParse(value, context, providers, tagRegex );
                return rs;
            }
            else
            {
                return string.Empty;
            }

        }

        private string InnerParse(string value, IDictionary<string, object> context, IEnumerable<ITagValueProvider> providers, string tagRegex)
        {
            Regex reg = new Regex(tagRegex);
            StringBuilder rs = new StringBuilder();
            int s = 0;
            while (s < value.Length)
            {
                Match m = reg.Match(value, s);
                if (m.Success)
                {
                    rs.Append(value.Substring(s, m.Index - s));
                    string tagValue = this.GetTagValue(m.Groups["prefix"].Value, m.Groups["tag"].Value, context, providers);
                    if (tagValue != null)
                    {
                        rs.Append(this.InnerParse(tagValue, context, providers, tagRegex));
                    }
                    s = m.Index + m.Length;
                }
                else
                {
                    rs.Append(value.Substring(s));
                    s = value.Length;
                }
            }

            return rs.ToString();
        }

        private string GetTagValue(string prefix, string tag, IDictionary<string, object> context, IEnumerable<ITagValueProvider> providers)
        {
            foreach (ITagValueProvider provider in providers)
            {
                if (provider.Prefix.ToString() == prefix && provider.HasTag(tag, context))
                {
                    string rs = provider.GetTagValue(tag, context);
                    if (!string.IsNullOrEmpty(rs) && (bool)context.GetValueOrDefault("c#-style-string", false) )
                    {
                        StringBuilder sb = new StringBuilder();
                        foreach (char c in rs)
                        {
                            switch (c)
                            {
                                case '\a':
                                    sb.Append("\\a");
                                    break;
                                case '\b':
                                    sb.Append("\\b");
                                    break;
                                case '\f':
                                    sb.Append("\\f");
                                    break;
                                case '\n':
                                    sb.Append("\\n");
                                    break;
                                case '\r':
                                    sb.Append("\\r");
                                    break;
                                case '\t':
                                    sb.Append("\\t");
                                    break;
                                case '\v':
                                    sb.Append("\\v");
                                    break;
                                case '\'':
                                    sb.Append("\\\'");
                                    break;
                                case '\\':
                                    sb.Append("\\\\");
                                    break;
                                default:
                                    sb.Append(c);
                                    break;
                            }
                        }
                        rs = sb.ToString();
                    }
                    return rs;

                }
            }

            if (this.Logger != null)
            {
                this.Logger.LogWarning("StringParse", Properties.Resources.StringParserUndefinedTagWarningText, prefix, tag); 
            }

            return string.Empty;
        }

        private ILogger _logger;
        public ILogger Logger
        {
            get
            {
                if (_logger == null && this.Site != null)
                {
                    _logger = (ILogger)this.Site.GetService(typeof(ILogger));
                }
                return _logger;
            }
        }

        public  override  void InitializeService()
        {
            foreach (object o in this._providers)
            {
                if (o is IObjectWithSite)
                {
                    ((IObjectWithSite)o).Site = this.Site;
                }
            }
            base.InitializeService(); 
        }

    }



}
