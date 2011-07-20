using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xaml;


namespace Fantasy.AddIns
{
    [System.Windows.Markup.ContentProperty("Template")]
    public class ObjectBuilder
    {
        public ObjectBuilder()
        {
            CurrentInstance = this;
        }
        public virtual object Build()
        {
            XamlReader xr = this.XamlNodes.GetReader();

            //object rs = System.Windows.Markup.XamlReader.Load(xr);
            //string[] ns = xr.SchemaContext.GetAllXamlNamespaces().ToArray();
            //XamlSchemaContext context = new XamlSchemaContext(xr.SchemaContext.ReferenceAssemblies);
            XamlObjectWriter xow = new XamlObjectWriter(System.Windows.Markup.XamlReader.GetWpfSchemaContext());

            XamlServices.Transform(xr, xow, true);

            return xow.Result;
            //return rs;
        }

        public virtual object Template { get; set; }

        internal XamlNodeList XamlNodes { get; set; }

        [ThreadStatic]
        internal static ObjectBuilder CurrentInstance;
    }
}
