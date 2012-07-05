using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Fantasy.XSerialization
{
    public interface IXSerializable
    {
        void Load(IServiceProvider context, XElement element);

        void Save(IServiceProvider context, XElement element);
    }

}
