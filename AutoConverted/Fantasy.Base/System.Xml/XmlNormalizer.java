package System.Xml;

public final class XmlNormalizer
{
	private static Regex regex = new Regex("[^\\u0009\\u000A\\u000D\\u0020-\\uD7FF\\uE000-\\uFFFD]");

	public static String Nomarlize(String text)
	{
		return regex.Replace(text, "");
	}
}