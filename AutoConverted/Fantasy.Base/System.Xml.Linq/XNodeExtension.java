package System.Xml.Linq;

public final class XNodeExtension
{
//C# TO JAVA CONVERTER TODO TASK: Extension methods are not available in Java:
//ORIGINAL LINE: public static XmlDocument ToXmlDocument(this XDocument xdoc)
	public static XmlDocument ToXmlDocument(XDocument xdoc)
	{
		XmlDocument xmldoc = new XmlDocument();
		xmldoc.Load(xdoc.CreateReader());
		return xmldoc;
	}
	/**   
	 Converts an XmlDocument to an XDocument.  
	   
	 @param xmldoc The XmlDocument to convert.  
	 @return The equivalent XDocument.  
	*/
//C# TO JAVA CONVERTER TODO TASK: Extension methods are not available in Java:
//ORIGINAL LINE: public static XDocument ToXDocument(this XmlDocument xmldoc)
	public static XDocument ToXDocument(XmlDocument xmldoc)
	{
		return XDocument.Load(xmldoc.CreateNavigator().ReadSubtree());
	}
	/**   
	 Converts an XElement to an XmlElement.  
	   
	 @param xelement The XElement to convert.  
	 @return The equivalent XmlElement.  
	*/
//C# TO JAVA CONVERTER TODO TASK: Extension methods are not available in Java:
//ORIGINAL LINE: public static XmlElement ToXmlElement(this XElement xelement)
	public static XmlElement ToXmlElement(XElement xelement)
	{
		return xelement.ToXmlElement(new XmlDocument());
	}

//C# TO JAVA CONVERTER TODO TASK: Extension methods are not available in Java:
//ORIGINAL LINE: public static XmlElement ToXmlElement(this XElement xelement, XmlDocument ownerDocument)
	public static XmlElement ToXmlElement(XElement xelement, XmlDocument ownerDocument)
	{
		Object tempVar = ownerDocument.ReadNode(xelement.CreateReader());
		return (XmlElement)((tempVar instanceof XmlElement) ? tempVar : null);
	}
	/**   
	 Converts an XmlElement to an XElement.  
	   
	 @param xmlelement The XmlElement to convert.  
	 @return The equivalent XElement.  
	*/
//C# TO JAVA CONVERTER TODO TASK: Extension methods are not available in Java:
//ORIGINAL LINE: public static XElement ToXElement(this XmlElement xmlelement)
	public static XElement ToXElement(XmlElement xmlelement)
	{
		return XElement.Load(xmlelement.CreateNavigator().ReadSubtree());
	}
}