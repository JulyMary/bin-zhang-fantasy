using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Xml.Linq;
using System.Xml;

namespace Fantasy.Jobs.Web.Models
{
    public class SettingsTreeConfiguration : IConfigurationSectionHandler
    {
        #region IConfigurationSectionHandler Members

        public object Create(object parent, object configContext, System.Xml.XmlNode section)
        {
            return ((XmlElement)section).ToXElement();
        }

        #endregion


        public static XElement Settings
        {
            get
            {
                return (XElement)ConfigurationManager.GetSection("fantasy/jobs.web/settingsTree");
            }
        }
    }

}