using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml;

namespace System.Xml.Linq
{
    public static class XNodeExtension
    {
        public static XmlDocument ToXmlDocument(this XDocument xdoc) { var xmldoc = new XmlDocument(); xmldoc.Load(xdoc.CreateReader()); return xmldoc; }
        /// <summary>  
        /// Converts an XmlDocument to an XDocument.  
        /// </summary>  
        /// <param name="xmldoc">The XmlDocument to convert.</param>  
        /// <returns>The equivalent XDocument.</returns>  
        public static XDocument ToXDocument(this XmlDocument xmldoc) { return XDocument.Load(xmldoc.CreateNavigator().ReadSubtree()); }
        /// <summary>  
        /// Converts an XElement to an XmlElement.  
        /// </summary>  
        /// <param name="xelement">The XElement to convert.</param>  
        /// <returns>The equivalent XmlElement.</returns>  
        public static XmlElement ToXmlElement(this XElement xelement) 
        {
            return xelement.ToXmlElement(new XmlDocument());
        }

        public static XmlElement ToXmlElement(this XElement xelement, XmlDocument ownerDocument)
        {
            return ownerDocument.ReadNode(xelement.CreateReader()) as XmlElement;
        }
        /// <summary>  
        /// Converts an XmlElement to an XElement.  
        /// </summary>  
        /// <param name="xmlelement">The XmlElement to convert.</param>  
        /// <returns>The equivalent XElement.</returns>  
        public static XElement ToXElement(this XmlElement xmlelement) { return XElement.Load(xmlelement.CreateNavigator().ReadSubtree()); }
    }


}
