using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Markup;

[assembly: XmlnsDefinition(XamlNamespaces.Namespace, "Fantasy.BusinessEngine.Events")]
[assembly: XmlnsDefinition(XamlNamespaces.Namespace, "Fantasy.BusinessEngine.Services")]
[assembly: XmlnsDefinition(XamlNamespaces.Namespace, "Fantasy.BusinessEngine")]

internal class XamlNamespaces
{
    public const string Namespace = "urn:schema-fantasy:business-engine";
}






