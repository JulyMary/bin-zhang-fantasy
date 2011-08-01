using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Markup;

[assembly: XmlnsDefinition(XamlNamespaces.Namespace, "Fantasy.Studio.Codons")]
[assembly: XmlnsDefinition(XamlNamespaces.Namespace, "Fantasy.Studio.Controls")]
[assembly: XmlnsDefinition(XamlNamespaces.Namespace, "Fantasy.Studio.Descriptor")]
[assembly: XmlnsDefinition(XamlNamespaces.Namespace, "Fantasy.Studio.Services")]
[assembly: XmlnsDefinition(XamlNamespaces.Namespace, "Fantasy.Studio.Workbench.Layout")]
[assembly: XmlnsDefinition(XamlNamespaces.Namespace, "Fantasy.Studio")]
[assembly: XmlnsDefinition(XamlNamespaces.Namespace, "Fantasy.Studio.Properties")]
[assembly: XmlnsDefinition(XamlNamespaces.Namespace, "Fantasy.Studio.Conditions")]

internal class XamlNamespaces
{
    public const string Namespace = "urn:schema-fantasy:xaml";
}






