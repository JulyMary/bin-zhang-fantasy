using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Xml;
using System.Xml.Linq;

namespace Fantasy.Configuration
{
    public class XSettingsConfigurationHandler : IConfigurationSectionHandler
    {
        #region IConfigurationSectionHandler Members

        public object Create(object parent, object configContext, System.Xml.XmlNode section)
        {
            return ((XmlElement)section).ToXElement();
        }

        #endregion
    }
}
