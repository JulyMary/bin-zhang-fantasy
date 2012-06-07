using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xaml;


namespace Fantasy.AddIns
{

    public class ObjectBuilder
    {
        internal ObjectBuilder(XamlNodeList xamlNodes, string baseDir)
        {
            this._xamlNodes = xamlNodes;
        }

        private string _baseDir = string.Empty;

        public T Build<T>()
        {
            XamlReader xr = this._xamlNodes.GetReader();
            AddInParser parser = new AddInParser();
            object rs;
            try
            {
                rs = parser.Parse(xr, this._baseDir);
            }
            finally
            {
                xr.Close();
            }
            return (T)rs;

        }
        private XamlNodeList _xamlNodes;

    }
}
