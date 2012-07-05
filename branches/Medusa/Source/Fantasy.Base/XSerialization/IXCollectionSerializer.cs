using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Xml.Linq;

namespace Fantasy.XSerialization
{
    public interface IXCollectionSerializer
    {
        void Save(IServiceProvider context, XElement element, IEnumerable collection);
        IEnumerable Load(IServiceProvider context, XElement element);
    }
}
