using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.IO;
using System.Xml.XPath;
using System.Xml;
using System.Configuration;
using Fantasy.IO;
using System.Windows.Forms;

namespace Fantasy.Configuration
{
    public class SettingStorage
    {

        private static XElement Root;



        private static string _location = null;
        public static string Location
        {
            get
            {
                if (_location == null)
                {

                    _location = ConfigurationManager.AppSettings["clickviewSettingsFile"];
                    string cfg = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;
                    if (string.IsNullOrEmpty(_location))
                    {
                        _location = Path.ChangeExtension(cfg, "xsettings");

                    }
                    _location = Environment.ExpandEnvironmentVariables(_location);

                    string defaultDir = LongPath.GetDirectoryName(cfg);

                    _location = LongPath.Combine(defaultDir, _location);


                }

                return _location;
            }

        }


        public static SettingsBase Load(SettingsBase data)
        {
            if (Root == null)
            {
                if (File.Exists(Location))
                {
                    Root = LongPathXNode.LoadXElement(Location);
                }
                else
                {
                    Root = new XElement("settingsFile",
                        new XAttribute(XNamespace.Xmlns + "xsi", "http://www.w3.org/2001/XMLSchema-instance"),
                        new XAttribute(XNamespace.Xmlns + "xsd", "http://www.w3.org/2001/XMLSchema"),
                        new XAttribute("version", 0));
                }
            }

            XElement settings = Root.XPathSelectElement(String.Format("settings[@type=\"{0}\"]", data.GetType().FullName));
            if (settings != null)
            {
                data.Load(settings.ToString());
            }
            return data;
        }
        private static XmlWriterSettings _xmlWriterSettings = new XmlWriterSettings() { Encoding = Encoding.UTF8, Indent = true, IndentChars = "    ", OmitXmlDeclaration = false, NewLineChars="\r\n", NewLineHandling= NewLineHandling.Replace, NewLineOnAttributes= true};
        public static void Save(SettingsBase data)
        {
            XElement newSettings = XElement.Parse(data.ToXml(), LoadOptions.None);
            XElement settings = Root.XPathSelectElement(String.Format("settings[@type=\"{0}\"]", data.GetType().FullName));
            if (settings != null)
            {
                settings.ReplaceWith(newSettings);
            }
            else
            {
                Root.Add(newSettings);
            }

            XmlWriter writer = XmlWriter.Create(Location, _xmlWriterSettings);
            try
            {
                StringWriter sw = new StringWriter();

                Root.Save(sw, SaveOptions.OmitDuplicateNamespaces);
               
                string xml = sw.GetStringBuilder().ToString();

                XElement savingData = XElement.Parse(xml, LoadOptions.None);

                savingData.Save(writer);
            }
            finally
            {
                writer.Close();
            }

        }

        public static void BeginUpdate()
        {
            throw new NotImplementedException();
        }

        public static void EndUpdate()
        {
            throw new NotImplementedException();
        }
    }
}
