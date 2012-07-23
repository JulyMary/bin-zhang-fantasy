package fantasy.jobs;

import java.util.regex.*;

import org.jdom2.*;
import fantasy.collections.*;

public final class StringParserUtils
{
	public static Element Parse(IStringParser parser, Element element, java.util.Map<String, Object> context) throws Exception
	{
		Element rs = element.clone();
		for (Element content : Flatterner.flattern(rs, new Selector<Element, Iterable<Element>>(){

			@Override
			public Iterable<Element> select(Element item) {
				return item.getChildren();
			}}))
		{
			for (Attribute attr : content.getAttributes())
			{
				attr.setValue(normalize(parser.Parse(attr.getValue())));
			}
			if (content.getChildren().size() == 0)
			{
				content.setText(normalize(parser.Parse(content.getText())));
			}
		}

		return rs;
	}
	
	private static String normalize(String input)
	{
		Matcher  m = _normalizePattern.matcher(input);
		
		String rs = m.replaceAll("");
		return rs;
		
	}
	
	private static Pattern _normalizePattern = Pattern.compile("[\\u0009\\u000A\\u000D\\u0020-\\uD7FF\\uE000-\\uFFFD]");

}