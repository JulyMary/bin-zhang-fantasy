package fantasy;


import java.io.InputStream;
import java.io.OutputStream;
import java.io.Reader;
import java.io.StringReader;

import org.jdom2.*;
import org.jdom2.input.*;
import org.jdom2.output.*;

public final class JDomUtils {


	private JDomUtils()
	{

	}

	public static Element loadElement(String path) throws Exception
	{
		SAXBuilder builder = new SAXBuilder();
		java.io.File xmlFile = new java.io.File(path);
		Document document = (Document) builder.build(xmlFile);
		Element rs = document.getRootElement();

		return rs;
	}
	
	public static Element loadElement(InputStream stream) throws Exception
	{
		SAXBuilder builder = new SAXBuilder();
		
		Document document = (Document) builder.build(stream);
		Element rs = document.getRootElement();

		return rs;
	}


	public static Element parseElement(String xml) throws Exception
	{
		SAXBuilder builder = new SAXBuilder();
		Reader in = new StringReader(xml);
		Document document = (Document) builder.build(in);
		Element rs = document.getRootElement();
		return rs;

	}

	private static Format _defaultFormat = Format.getPrettyFormat().setLineSeparator(LineSeparator.SYSTEM);

	public static void saveElement(Element Element, String path, Format format) throws Exception
	{
		OutputStream stream = fantasy.io.File.Create(path);
		try
		{
			new XMLOutputter(format).output(Element, stream);
		}
		finally
		{
			stream.close();
		}
	}
	
	public static void saveElement(Element element, OutputStream stream , Format format) throws Exception
	{
		Document doc = new Document();
		doc.addContent(element.clone());
	    new XMLOutputter(format).output(doc, stream);
		
	}
	
	public static void saveElement(Element element, OutputStream stream) throws Exception
	{
		saveElement(element, stream, _defaultFormat);
		
	}

	public static void saveElement(Element element, String path) throws Exception
	{
		saveElement(element, path, _defaultFormat);
	}
}
