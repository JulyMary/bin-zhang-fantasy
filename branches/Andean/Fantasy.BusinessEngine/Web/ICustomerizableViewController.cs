using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Fantasy.Web
{
    public interface ICustomerizableViewController
    {
        void LoadSettings(XElement settings);
    }
}
