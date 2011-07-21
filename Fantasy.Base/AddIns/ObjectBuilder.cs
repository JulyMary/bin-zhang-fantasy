using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xaml;


namespace Fantasy.AddIns
{
  
    public class ObjectBuilder
    {
        internal ObjectBuilder(XamlNodeList xamlNodes)
        {
            this._xamlNodes = xamlNodes;
        }
        public T Build<T>()
        {
            XamlReader xr = this._xamlNodes.GetReader();
            XamlObjectWriter writer = new XamlObjectWriter(xr.SchemaContext);


            XamlServices.Transform(xr, writer, true);
            xr.Close();
            return (T)writer.Result;
            
        }
        private XamlNodeList _xamlNodes;

    }
}
