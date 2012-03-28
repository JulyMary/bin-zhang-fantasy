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
using System.IO.Packaging;
using System.IO;

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
                ser.Deserialize(element, item);
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
            Package zip = ZipPackage.Open(path, System.IO.FileMode.Open, System.IO.FileAccess.Read);
            try
            {
                PackagePartCollection parts = zip.GetParts();
                PackagePart templatePart = parts.Single(p => p.Uri.ToString().EndsWith(".template.xml"));
                
                
                Stream templateStream =  templatePart.GetStream();
                try
                {
                    XSerializer ser = new XSerializer(typeof(ScriptTemplate));
                    XElement templateElement;
                    templateElement = XElement.Load(templateStream);
                    rs = (ScriptTemplate)ser.Deserialize(templateElement);
                }
                finally
                {
                    templateStream.Close();
                }

                LoadIcon(rs, parts);

                LoadTTs(rs, parts);


                
            }
            finally
            {
                zip.Close();
            }

            return rs;
        }

        private static void LoadTTs(ScriptTemplate rs, PackagePartCollection parts)
        {
            foreach (ScriptTemplateItem item in rs.Items)
            {
                if (!string.IsNullOrEmpty(item.TT))
                {
                    PackagePart part = parts.Single(p => p.Uri == new Uri(rs.IconPath, UriKind.RelativeOrAbsolute));
                    Stream stream = part.GetStream();
                    try
                    {
                        StreamReader reader = new StreamReader(stream);
                        item.TTContent = reader.ReadToEnd();
                    }
                    finally
                    {
                        stream.Close();
                    }
                }
            }
        }

        private static void LoadIcon(ScriptTemplate rs, PackagePartCollection parts)
        {
            if (!string.IsNullOrEmpty(rs.IconPath))
            {
                PackagePart part = parts.Single(p => p.Uri == new Uri(rs.IconPath, UriKind.RelativeOrAbsolute));
                MemoryStream ms = new MemoryStream();
                Stream partStream = part.GetStream();
                int b;
                do
                {
                    b = partStream.ReadByte();
                    if (b != -1)
                    {
                        ms.WriteByte((byte)b);
                    }

                } while (b >= 0);
                partStream.Close();

                ms.Position = 0;

                rs.Icon = new BitmapImage();
                rs.Icon.BeginInit();
                rs.Icon.StreamSource = ms;
                rs.Icon.EndInit();


            }
        }


        

    }
}
