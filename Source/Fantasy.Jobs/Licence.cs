using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.XSerialization;
using System.Xml.Linq;
using System.Reflection;

namespace Fantasy.Jobs
{
    [XSerializable("jobservice",NamespaceUri= Consts.LicenceNamespaceURI)]
    internal class Licence
    {

        [XAttribute("expire")]
        internal DateTime ExpireTime = DateTime.MinValue;

        [XAttribute("machine")]
        internal string MachineCode = string.Empty;

        [XAttribute("dev")]
        internal bool IsDevelopVersion = false;

        [XAttribute("slave")]
        internal int SlaveCount = 0;

    }
}
