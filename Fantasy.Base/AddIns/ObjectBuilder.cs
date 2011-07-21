using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xaml;


namespace Fantasy.AddIns
{
  
    public class ObjectBuilder
    {
        public ObjectBuilder(XamlNodeList nodes)
        {
            this.XamlNodes = nodes;
        }
        public virtual object Build()
        {
            XamlReader xr = this.XamlNodes.GetReader();
            object rs = System.Windows.Markup.XamlReader.Load(xr);
            xr.Close();
            return rs;
            
        }

     

        public XamlNodeList XamlNodes { get; private set; }

    }
}
