package fantasy.jobs;

import java.util.*;
import java.util.regex.*;

import fantasy.collections.*;
import fantasy.jobs.properties.*;
import fantasy.servicemodel.*;
import fantasy.*;

public class StringParseService extends AbstractService implements IStringParser
{
	/**
	 * 
	 */
	private static final long serialVersionUID = -6646487132759689523L;

	public StringParseService() throws Exception
	{
		
	}
	

	
	@Override
	public void initializeService() throws Exception
	{
		this._providers = AddIn.CreateObjects(ITagValueProvider.class, "jobEngine/tagValueProviders/provider");
		for (Object o : this._providers)
		{
			if (o instanceof IObjectWithSite)
			{
				((IObjectWithSite)o).setSite(this.getSite());
			}
		}
		super.initializeService();
	}

	protected ITagValueProvider[] _providers;

	public final String Parse(String value, Map<String, Object> context) throws Exception
	{

		if (value != null)
		{
			
			final Map<String, Object> context2 = context == null ? new java.util.HashMap<String, Object>() : context;
			
			
			StringBuilder prefixes = new StringBuilder();


			Enumerable<ITagValueProvider> enabledProviders = new Enumerable<ITagValueProvider>(this._providers).where(new Predicate<ITagValueProvider>(){

				@Override
				public boolean evaluate(ITagValueProvider obj) throws Exception {
					return obj.IsEnabled(context2);
				}});
			
			Enumerable<Character> chars = enabledProviders.select(new Selector<ITagValueProvider, Character>(){

				@Override
				public Character select(ITagValueProvider item) {
					return item.getPrefix();
				}}).Distinct();

			for (Character prefix : chars)
			{
				prefixes.append('\\');
				prefixes.append(prefix);
			}
			String tagRegex = "(?<prefix>[" + prefixes.toString() + "])\\((?<tag>[^)]+)\\)";
			String rs = InnerParse(value, context, enabledProviders, tagRegex);
			return rs;
		}
		else
		{
			return "";
		}

	}

	private String InnerParse(String value, java.util.Map<String, Object> context, Iterable<ITagValueProvider> providers, String tagRegex) throws Exception
	{
		Pattern reg = Pattern.compile(tagRegex);
		StringBuilder rs = new StringBuilder();
		int s = 0;
		while (s < value.length())
		{
			Matcher m = reg.matcher(value.substring(s));
			if (m.lookingAt())
			{
				rs.append(value.substring(s, m.start()));
				String tagValue = this.GetTagValue(m.group("prefix"), m.group("tag"), context, providers);
				if (tagValue != null)
				{
					rs.append(this.InnerParse(tagValue, context, providers, tagRegex));
				}
				s = m.end();
			}
			else
			{
				rs.append(value.substring(s));
				s = value.length();
			}
		}

		return rs.toString();
	}

	private String GetTagValue(String prefix, String tag, java.util.Map<String, Object> context, Iterable<ITagValueProvider> providers) throws Exception
	{
		for (ITagValueProvider provider : providers)
		{
			if ((new Character(provider.getPrefix())).toString().equals(prefix) && provider.HasTag(tag, context))
			{
				String rs = provider.GetTagValue(tag, context);
				if (!StringUtils2.isNullOrEmpty(rs) &&  MapUtils.getValueOrDefault(context, "c#-style-string", false))
				{
					StringBuilder sb = new StringBuilder();
					for (char c : rs.toCharArray())
					{
						switch (c)
						{
							case '\u0007':
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
							case '\u000b':
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
			this.getLogger().LogWarning("StringParse", Resources.getStringParserUndefinedTagWarningText(), prefix, tag);
		}

		return "";
	}

	private ILogger _logger;
	public final ILogger getLogger() throws Exception
	{
		if (_logger == null && this.getSite() != null)
		{
			_logger = (ILogger)this.getSite().getService(ILogger.class);
		}
		return _logger;
	}



	@Override
	public String Parse(String value) throws Exception {
		
		return this.Parse(value, null);
	}

	

}