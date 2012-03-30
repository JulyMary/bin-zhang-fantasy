using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.XSerialization;
using System.Xml.Linq;
using System.Runtime.InteropServices;
using System.Windows.Media;
using System.Drawing;
using System.Windows.Interop;
using System.Windows;
using System.Windows.Media.Imaging;
using System.ComponentModel;
using System.IO;
using Ionic.Zip;

namespace Fantasy.Studio.BusinessEngine.CodeEditing
{
    [XSerializable("template", NamespaceUri= Consts.ScriptTemplateNamespace)] 
    public class ScriptTemplate : IXSerializable
    {
        [XAttribute("name")]
        public string Name { get; set; }

        [XAttribute("icon")]
        public string IconPath { get; set; }

        [XAttribute("description")]
        public string Description { get; set; }

        [XAttribute("defaultFileName")]
        public string DefaultFileName { get; set; }

        private IList<ScriptTemplateItem> _items = new List<ScriptTemplateItem>();
        public IList<ScriptTemplateItem> Items
        {
            get
            {
                return _items;
            }
        }

        public BitmapImage Icon { get; set; }

        

        #region IXSerializable Members

        void IXSerializable.Load(IServiceProvider context, System.Xml.Linq.XElement element)
        {
            XSerializer ser = new XSerializer(typeof(ScriptTemplateItem));
            XHelper.Default.LoadByXAttributes(context, element, this);
            foreach(XElement itemElement in element.Elements())
            {
                ScriptTemplateItem item = new ScriptTemplateItem();
                ser.Deserialize(itemElement, item);
                this.Items.Add(item);
            }
        }

        void IXSerializable.Save(IServiceProvider context, System.Xml.Linq.XElement element)
        {
            throw new NotSupportedException();
        }

        #endregion


        public static ScriptTemplate LoadFrom(string path)
        {
            ScriptTemplate rs;
            using (ZipFile zip = ZipFile.Read(path))
            {
                ZipEntry templateEntry = zip.Single(e => e.FileName.ToLower().EndsWith(".template.xml")); 
                MemoryStream templateStream = new MemoryStream();
                templateEntry.Extract(templateStream);
                templateStream.Position = 0;

                XSerializer ser = new XSerializer(typeof(ScriptTemplate));
                XElement templateElement;
                templateElement = XElement.Load(templateStream);
                rs = (ScriptTemplate)ser.Deserialize(templateElement);

                LoadIcon(rs, zip);

                LoadTTs(rs, zip);
            }
          
               
            return rs;
        }


        private static string NormalizeFileName(string path)
        {
            if (path.StartsWith(".\\"))
            {
                path = path.Substring(2);
            }

            return path.ToLower();
        }

        private static void LoadTTs(ScriptTemplate rs, ZipFile zip)
        {
            foreach (ScriptTemplateItem item in rs.Items)
            {
                if (!string.IsNullOrEmpty(item.Src))
                {
                    ZipEntry entry = zip.Single(e => NormalizeFileName(e.FileName) == NormalizeFileName(item.Src));
                    Stream stream = new MemoryStream();
                    entry.Extract(stream);
                    stream.Position = 0;
                    try
                    {
                        StreamReader reader = new StreamReader(stream);
                        item.Content = reader.ReadToEnd();
                    }
                    finally
                    {
                        stream.Close();
                    }
                }
            }
        }

        private static void LoadIcon(ScriptTemplate rs, ZipFile zip)
        {
            if (!string.IsNullOrEmpty(rs.IconPath))
            {
                ZipEntry entry = zip.Single(e => NormalizeFileName(e.FileName) == NormalizeFileName(rs.IconPath));
                Stream stream = new MemoryStream();
                entry.Extract(stream);
                stream.Position = 0;

                rs.Icon = new BitmapImage();
                rs.Icon.BeginInit();
                rs.Icon.StreamSource = stream;
                rs.Icon.EndInit();


            }
        }


        

    }
}
