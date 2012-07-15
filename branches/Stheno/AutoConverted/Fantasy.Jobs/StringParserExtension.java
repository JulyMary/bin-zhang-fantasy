package Fantasy.Jobs;

public final class StringParserExtension
{
//C# TO JAVA CONVERTER TODO TASK: Extension methods are not available in Java:
//ORIGINAL LINE: public static XElement Parse(this IStringParser parser, XElement element, IDictionary<string, object> context = null)
//C# TO JAVA CONVERTER TODO TASK: C# optional parameters are not converted to Java:
	public static XElement Parse(IStringParser parser, XElement element, java.util.Map<String, Object> context)
	{
		XElement rs = new XElement(element);

//C# TO JAVA CONVERTER TODO TASK: Lambda expressions and anonymous methods are not converted by C# to Java Converter:
		for (XElement content : rs.Flatten(e => e.Elements()))
		{
			for (XAttribute attr : content.Attributes())
			{
				attr.setValue(XmlNormalizer.Nomarlize(parser.Parse(attr.getValue())));
			}
			if (!content.HasElements)
			{
				content.setValue(XmlNormalizer.Nomarlize(parser.Parse(content.getValue())));
			}
		}

		return rs;
	}

}