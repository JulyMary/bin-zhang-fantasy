using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Markup;

[assembly: XmlnsDefinition(XamlNamespaces.Namespace, "Fantasy.Studio.BusinessEngine.Conditions")]
[assembly: XmlnsDefinition(XamlNamespaces.Namespace, "Fantasy.Studio.BusinessEngine")]
[assembly: XmlnsDefinition(XamlNamespaces.Namespace, "Fantasy.Studio.BusinessEngine.Properties")]
[assembly: XmlnsDefinition(XamlNamespaces.Namespace, "Fantasy.Studio.BusinessEngine.PackageEditing")]
[assembly: XmlnsDefinition(XamlNamespaces.Namespace, "Fantasy.Studio.BusinessEngine.ClassEditing")]
[assembly: XmlnsDefinition(XamlNamespaces.Namespace, "Fantasy.Studio.BusinessEngine.ClassDiagramEditing")]
[assembly: XmlnsDefinition(XamlNamespaces.Namespace, "Fantasy.Studio.BusinessEngine.PropertyEditing")]
[assembly: XmlnsDefinition(XamlNamespaces.Namespace, "Fantasy.Studio.BusinessEngine.EnumEditing")]
[assembly: XmlnsDefinition(XamlNamespaces.Namespace, "Fantasy.Studio.BusinessEngine.AssemblyReference")]
[assembly: XmlnsDefinition(XamlNamespaces.Namespace, "Fantasy.Studio.BusinessEngine.AssociationEditing")]


[assembly: XmlnsDefinition(XamlNamespaces.DiagramModel, "Fantasy.Studio.BusinessEngine.ClassDiagramEditing.Model")]

internal class XamlNamespaces
{
    public const string Namespace = "urn:schema-fantasy:studio-business-engine";

    public const string DiagramModel = "urn:schema-fantasy:studio-business-engine-class-diagram-model";
}






