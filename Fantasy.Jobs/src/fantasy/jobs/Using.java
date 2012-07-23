package fantasy.jobs;

import java.util.regex.*;

import org.apache.commons.lang3.StringUtils;

import fantasy.xserialization.*;
import fantasy.jobs.resources.*;
import fantasy.jobs.properties.*;
import fantasy.*;

@Instruction
@XSerializable(name = "using", namespaceUri = Consts.XNamespaceURI)
public class Using extends Sequence
{
	@Override
	public void Execute() throws Exception
	{
		String[] strs = new String[] { this.Res, this.Res1, this.Res2, this.Res3, this.Res4, this.Res5, this.Res6, this.Res7, this.Res8, this.Res9 };
		java.util.ArrayList<ResourceParameter> parameters = new java.util.ArrayList<ResourceParameter>();
		for (String str : strs)
		{
			if (!StringUtils2.isNullOrWhiteSpace(str))
			{
				ResourceParameter parameter = CreateParameterFromString(str);
				parameters.add(parameter);
			}
		}

		IResourceService svc = this.getSite().getService(IResourceService.class);
		if (parameters.size() > 0 && svc != null)
		{

			IResourceHandle handle = svc.Request(parameters.toArray(new ResourceParameter[]{}));
			try
			{
				this.ExecuteSequence();
			}
			finally
			{
				handle.dispose();
			}
		}
		else
		{
			this.ExecuteSequence();
		}
	}

	private ResourceParameter CreateParameterFromString(String text) throws Exception
	{

		ResourceParameter rs = new ResourceParameter();

		IStringParser parser = this.getSite().getRequiredService(IStringParser.class);
		java.util.HashMap<String, Object> ctx = new java.util.HashMap<String, Object>();
		Pattern itemExpr = Pattern.compile("(?<key>[^=]*)=(?<value>(\"(\\\\\"|[^\"])*\\\")|('(\\\\'|[^'])*\\')|([^;]*));?");
		Pattern quotaExpr = Pattern.compile("^'(?<value>(\\\\'|[^'])*)\\'$");
		Pattern dquotaExpr = Pattern.compile("^\"(?<value>(\\\\\"|[^\"])*)\\\"$");

		Matcher itemMatch = itemExpr.matcher(text);
		while (itemMatch.find())
		{
			String key = itemMatch.group("key");
			String value = itemMatch.group("value");
			boolean isCsString = false;

			for (Pattern strReg : new Pattern[] { quotaExpr, dquotaExpr })
			{
				Matcher strMatch = strReg.matcher(value);
				if (strMatch.matches())
				{
					value = strMatch.group("value");
					isCsString = true;
					break;
				}
			}

			ctx.put("c#-style-string", isCsString);

			value = parser.Parse(value, ctx);

			if (isCsString)
			{
				value = DecodeCsString(value);
			}

			if (StringUtils.equalsIgnoreCase("name", key))
			{
				rs.setName(value);
			}
			else
			{
				rs.getValues().put(key, value);
			}
		}

		if (rs.getName() == null)
		{
			throw new JobException(String.format(Resources.getMissingResourceNameText(), text));
		}

		return rs;


	}

	private String DecodeCsString(String text)
	{
		Pattern reg = Pattern.compile("\\\\(0x(?<hex4>[0-9a-fA-F]{4})|0x(?<hex2>[0-9a-fA-F]{2})|(?<oct>[0-7]{3})|(?<char>.?))");
		StringBuilder rs = new StringBuilder();
		int s = 0;
		while (s < text.length())
		{
			Matcher m = reg.matcher(text.substring(s));
			if (m.lookingAt())
			{
				rs.append(text.substring(s, m.start()));

				char value;
				if (m.group("hex4") != null)
				{
					value = (char)Integer.parseInt(m.group("hex4"), 16);
				}
				else if (m.group("hex2") != null)
				{
					value =(char)Integer.parseInt(m.group("hex2"), 16);
				}
				else if (m.group("oct") != null)
				{
					value = (char)Integer.parseInt(m.group("oct"), 8);
				}
				else
				{

					if (m.group("char").equals("a"))
					{
							value = '\u0007';
					}
					else if (m.group("char").equals("b"))
					{
							value = '\b';
					}
					else if (m.group("char").equals("f"))
					{
							value = '\f';
					}
					else if (m.group("char").equals("n"))
					{
							value = '\n';
					}
					else if (m.group("char").equals("r"))
					{
							value = '\r';
					}
					else if (m.group("char").equals("t"))
					{
							value = '\t';
					}
					else if (m.group("char").equals("v"))
					{
							value = '\u000b';
					}
					else if (m.group("char").equals("'"))
					{
							value = '\'';
					}
					else if (m.group("char").equals("\\"))
					{
							value = '\\';
					}
					else
					{
							throw new IllegalStateException(String.format("Unrecognized escape sequences in string '%1$s'.", text));

					}
				}
				rs.append(value);
				s = m.end();
			}
			else
			{
				rs.append(text.substring(s));
				s = text.length();
			}

		}
		return rs.toString();
	}

	@XAttribute(name = "res")
	public String Res = null;

	@XAttribute(name ="res1")
	public String Res1 = null;

	@XAttribute(name ="res2")
	public String Res2 = null;


	@XAttribute(name ="res3")
	public String Res3 = null;

	@XAttribute(name ="res4")
	public String Res4 = null;


	@XAttribute(name ="res5")
	public String Res5 = null;


	@XAttribute(name ="res6")
	public String Res6 = null;


	@XAttribute(name ="res7")
	public String Res7 = null;


	@XAttribute(name ="res8")
	public String Res8 = null;

	@XAttribute(name ="res9")
	public String Res9 = null;


}