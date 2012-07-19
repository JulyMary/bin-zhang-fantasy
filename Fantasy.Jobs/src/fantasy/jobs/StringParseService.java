package fantasy.jobs;

import fantasy.jobs.Properties.*;
import fantasy.servicemodel.*;

public class StringParseService extends AbstractService implements IStringParser
{
	public StringParseService()
	{
		this._providers = AddIn.<ITagValueProvider>CreateObjects("jobEngine/tagValueProviders/provider");
	}

	protected ITagValueProvider[] _providers;

	public final String Parse(String value, java.util.Map<String, Object> context)
	{

		if (value != null)
		{
			if (context == null)
			{
				context = new java.util.HashMap<String, Object>();
			}
			StringBuilder prefixes = new StringBuilder();
//C# TO JAVA CONVERTER TODO TASK: There is no Java equivalent to LINQ queries:
//C# TO JAVA CONVERTER TODO TASK: Lambda expressions and anonymous methods are not converted by C# to Java Converter:
			Iterable<ITagValueProvider> providers = this._providers.Where(x => x.IsEnabled(context));

//C# TO JAVA CONVERTER TODO TASK: There is no Java equivalent to LINQ queries:
//C# TO JAVA CONVERTER TODO TASK: Lambda expressions and anonymous methods are not converted by C# to Java Converter:
			for (char prefix : providers.Select(p => p.Prefix).Distinct())
			{
				prefixes.append('\\');
				prefixes.append(prefix);
			}
			String tagRegex = "(?<prefix>[" + prefixes.toString() + "])\\((?<tag>[^)]+)\\)";
			String rs = InnerParse(value, context, providers, tagRegex);
			return rs;
		}
		else
		{
			return "";
		}

	}

	private String InnerParse(String value, java.util.Map<String, Object> context, Iterable<ITagValueProvider> providers, String tagRegex)
	{
		Regex reg = new Regex(tagRegex);
		StringBuilder rs = new StringBuilder();
		int s = 0;
		while (s < value.length())
		{
			Match m = reg.Match(value, s);
			if (m.Success)
			{
				rs.append(value.substring(s, m.Index));
				String tagValue = this.GetTagValue(m.Groups["prefix"].getValue(), m.Groups["tag"].getValue(), context, providers);
				if (tagValue != null)
				{
					rs.append(this.InnerParse(tagValue, context, providers, tagRegex));
				}
				s = m.Index + m.getLength();
			}
			else
			{
				rs.append(value.substring(s));
				s = value.length();
			}
		}

		return rs.toString();
	}

	private String GetTagValue(String prefix, String tag, java.util.Map<String, Object> context, Iterable<ITagValueProvider> providers)
	{
		for (ITagValueProvider provider : providers)
		{
			if ((new Character(provider.getPrefix())).toString().equals(prefix) && provider.HasTag(tag, context))
			{
				String rs = provider.GetTagValue(tag, context);
				if (!DotNetToJavaStringHelper.isNullOrEmpty(rs) && (boolean)context.GetValueOrDefault("c#-style-string", false))
				{
					StringBuilder sb = new StringBuilder();
					for (char c : rs)
					{
						switch (c)
						{
							case '\a':
								sb.append("\\a");
								break;
							case '\b':
								sb.append("\\b");
								break;
							case '\f':
								sb.append("\\f");
								break;
							case '\n':
								sb.append("\\n");
								break;
							case '\r':
								sb.append("\\r");
								break;
							case '\t':
								sb.append("\\t");
								break;
							case '\v':
								sb.append("\\v");
								break;
							case '\'':
								sb.append("\\\'");
								break;
							case '\\':
								sb.append("\\\\");
								break;
							default:
								sb.append(c);
								break;
						}
					}
					rs = sb.toString();
				}
				return rs;

			}
		}

		if (this.getLogger() != null)
		{
			this.getLogger().LogWarning("StringParse", Properties.Resources.getStringParserUndefinedTagWarningText(), prefix, tag);
		}

		return "";
	}

	private ILogger _logger;
	public final ILogger getLogger()
	{
		if (_logger == null && this.Site != null)
		{
			_logger = (ILogger)this.Site.GetService(ILogger.class);
		}
		return _logger;
	}

	@Override
	public void InitializeService()
	{
		for (Object o : this._providers)
		{
			if (o instanceof IObjectWithSite)
			{
				((IObjectWithSite)o).Site = this.Site;
			}
		}
		super.InitializeService();
	}

}