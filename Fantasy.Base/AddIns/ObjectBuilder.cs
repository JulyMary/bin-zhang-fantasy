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
        public object Build()
        {
            XamlReader xr = this.XamlNodes.GetReader();
            XamlObjectWriter xow = new XamlObjectWriter(xr.SchemaContext);
            while (xr.Read())
            {
                xow.WriteNode(xr);
            }
            xr.Close();
            return xow.Result;
        }

        public object Template { get; set; }

        internal XamlNodeList XamlNodes { get; set; }

        [ThreadStatic]
        internal static ObjectBuilder CurrentInstance;
    }
}
