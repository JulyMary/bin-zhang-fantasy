using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Markup;

[assembly: XmlnsDefinition(XamlNamespaces.Namespace, "Fantasy.Adaption")]
[assembly: XmlnsDefinition(XamlNamespaces.Namespace, "Fantasy.AddIns")]
[assembly: XmlnsDefinition(XamlNamespaces.Namespace, "Fantasy.AddIns.Codons")]
[assembly: XmlnsDefinition(XamlNamespaces.Namespace, "Fantasy.AddIns.Conditions")]
[assembly: XmlnsDefinition(XamlNamespaces.Namespace, "Fantasy.ServiceModel")]
[assembly: XmlnsDefinition(XamlNamespaces.Namespace, "Fantasy.AddIns.Services")]
[assembly: XmlnsDefinition(XamlNamespaces.Namespace, "Fantasy.Windows")]

internal class XamlNamespaces
{
    public const string Namespace = "urn:schema-fantasy:xaml";
}






