package Fantasy.Jobs;

import Fantasy.XSerialization.*;
import Fantasy.Jobs.Properties.*;
import Fantasy.Jobs.Resources.*;

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
//[Instruction, XSerializable("using", NamespaceUri = Consts.XNamespaceURI)]
public class Using extends Sequence
{
	@Override
	public void Execute()
	{
		String[] strs = new String[] { this.Res, this.Res1, this.Res2, this.Res3, this.Res4, this.Res5, this.Res6, this.Res7, this.Res8, this.Res9 };
		java.util.ArrayList<ResourceParameter> parameters = new java.util.ArrayList<ResourceParameter>();
		for (String str : strs)
		{
			if (!String.IsNullOrWhiteSpace(str))
			{
				ResourceParameter parameter = CreateParameterFromString(str);
				parameters.add(parameter);
			}
		}

		IResourceService svc = this.getSite().<IResourceService>GetService();
		if (parameters.size() > 0 && svc != null)
		{
//C# TO JAVA CONVERTER NOTE: The following 'using' block is replaced by its Java equivalent:
//			using (IResourceHandle handle = svc.Request(parameters.ToArray()))
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

	private ResourceParameter CreateParameterFromString(String text)
	{

		ResourceParameter rs = new ResourceParameter();

		IStringParser parser = this.getSite().<IStringParser>GetRequiredService();
		java.util.HashMap<String, Object> ctx = new java.util.HashMap<String, Object>();
		Regex itemExpr = new Regex("(?<key>[^=]*)=(?<value>(\"(\\\\\"|[^\"])*\\\")|('(\\\\'|[^'])*\\')|([^;]*));?");
		Regex quotaExpr = new Regex("^'(?<value>(\\\\'|[^'])*)\\'$");
		Regex dquotaExpr = new Regex("^\"(?<value>(\\\\\"|[^\"])*)\\\"$");

		for (Match itemMatch : itemExpr.Matches(text))
		{
			String key = itemMatch.Groups["key"].getValue();
			String value = itemMatch.Groups["value"].getValue();
			boolean isCsString = false;

			for (Regex strReg : new Regex[] { quotaExpr, dquotaExpr })
			{
				Match strMatch = strReg.Match(value);
				if (strMatch.Success)
				{
					value = strMatch.Groups["value"].getValue();
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

			if (StringComparer.OrdinalIgnoreCase.Compare("name", key) == 0)
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
			throw new JobException(String.format(Properties.Resources.getMissingResourceNameText(), text));
		}

		return rs;


	}

	private String DecodeCsString(String text)
	{
		Regex reg = new Regex("\\\\(0x(?<hex4>[0-9a-fA-F]{4})|0x(?<hex2>[0-9a-fA-F]{2})|(?<oct>[0-7]{3})|(?<char>.?))");
		StringBuilder rs = new StringBuilder();
		int s = 0;
		while (s < text.length())
		{
			Match m = reg.Match(text, s);
			if (m.Success)
			{
				rs.append(text.substring(s, m.Index));

				char value;
				if (m.Groups["hex4"].Success)
				{
					value = Convert.ToChar(Integer.parseInt(m.Groups["hex4"].getValue(), 16));
				}
				else if (m.Groups["hex2"].Success)
				{
					value = Convert.ToChar(Integer.parseInt(m.Groups["hex2"].getValue(), 16));
				}
				else if (m.Groups["oct"].Success)
				{
					value = Convert.ToChar(Integer.parseInt(m.Groups["oct"].getValue(), 8));
				}
				else
				{
//C# TO JAVA CONVERTER NOTE: The following 'switch' operated on a string member and was converted to Java 'if-else' logic:
//					switch (m.Groups["char"].Value)
//ORIGINAL LINE: case "a":
					if (m.Groups["char"].getValue().equals("a"))
					{
							value = '\a';
					}
//ORIGINAL LINE: case "b":
					else if (m.Groups["char"].getValue().equals("b"))
					{
							value = '\b';
					}
//ORIGINAL LINE: case "f":
					else if (m.Groups["char"].getValue().equals("f"))
					{
							value = '\f';
					}
//ORIGINAL LINE: case "n":
					else if (m.Groups["char"].getValue().equals("n"))
					{
							value = '\n';
					}
//ORIGINAL LINE: case "r":
					else if (m.Groups["char"].getValue().equals("r"))
					{
							value = '\r';
					}
//ORIGINAL LINE: case "t":
					else if (m.Groups["char"].getValue().equals("t"))
					{
							value = '\t';
					}
//ORIGINAL LINE: case "v":
					else if (m.Groups["char"].getValue().equals("v"))
					{
							value = '\v';
					}
//ORIGINAL LINE: case "'":
					else if (m.Groups["char"].getValue().equals("'"))
					{
							value = '\'';
					}
//ORIGINAL LINE: case "\\":
					else if (m.Groups["char"].getValue().equals("\\"))
					{
							value = '\\';
					}
					else
					{
							throw new InvalidOperationException(String.format("Unrecognized escape sequences in string '%1$s'.", text));

					}
				}
				rs.append(value);
				s = m.Index + m.getLength();
			}
			else
			{
				rs.append(text.substring(s));
				s = text.length();
			}

		}
		return rs.toString();
	}

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[XAttribute("res")]
	public String Res = null;

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[XAttribute("res1")]
	public String Res1 = null;

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[XAttribute("res2")]
	public String Res2 = null;

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[XAttribute("res3")]
	public String Res3 = null;

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[XAttribute("res4")]
	public String Res4 = null;

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[XAttribute("res5")]
	public String Res5 = null;

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[XAttribute("res6")]
	public String Res6 = null;

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[XAttribute("res7")]
	public String Res7 = null;

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[XAttribute("res8")]
	public String Res8 = null;

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[XAttribute("res9")]
	public String Res9 = null;


}