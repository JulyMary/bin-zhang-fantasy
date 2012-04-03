using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.IO;
using System.Xml.Linq;

namespace Fantasy.BusinessEngine
{
    public class BusinessScript : BusinessEntity, IGernateCodeBusinessEntity, IScriptable
    {
        public virtual BusinessPackage Package
        {
            get
            {
                return (BusinessPackage)this.GetValue("Package", null);
            }
            set
            {
                this.SetValue("Package", value);
            }
        }


        public virtual string Script
        {
            get
            {
                return (string)this.GetValue("Script", null);
            }
            set
            {
                this.SetValue("Script", value);
            }
        }

       

        public virtual string MetaData
        {
            get
            {
                return (string)this.GetValue("MetaData", null);
            }
            set
            {
                this.SetValue("MetaData", value);
            }
        }

        public virtual string this[string name]
        {
            get
            {
                if (String.IsNullOrEmpty(this.MetaData))
                {
                    return null;
                }
                XElement ele = XElement.Parse(this.MetaData);
                if (string.Equals(name, "BuildAction", StringComparison.OrdinalIgnoreCase))
                {
                    return ele.Name.LocalName;
                }
                else
                {
                    XElement child = ele.Elements().Where(c => String.Equals(c.Name.LocalName, name, StringComparison.OrdinalIgnoreCase)).SingleOrDefault();
                    return child != null ? (string)child : null;
                }
            }
            set
            {
                XElement newMetadata;
                if (string.Equals(name, "BuildAction", StringComparison.OrdinalIgnoreCase))
                {
                    newMetadata = new XElement(value);
                    if (!string.IsNullOrEmpty(this.MetaData))
                    {
                        XElement old = XElement.Parse(this.MetaData);
                        foreach (XElement child in old.Elements().ToArray())
                        {
                            child.Remove();
                            newMetadata.Add(child);
                        }
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(this.MetaData))
                    {
                        newMetadata = XElement.Parse(this.MetaData);
                    }
                    else
                    {
                        newMetadata = new XElement("None");
                    }
                    XElement child = newMetadata.Elements().Where(c => String.Equals(c.Name.LocalName, name, StringComparison.OrdinalIgnoreCase)).SingleOrDefault();
                    if (child == null && !string.IsNullOrEmpty(value))
                    {
                        newMetadata.Add(new XElement("name", value));
                    }
                    else
                    {
                        if (!String.IsNullOrEmpty(value))
                        {

                            child.Value = value;
                        }
                        else
                        {
                            child.Remove();
                        }
                    }

                }

                this.MetaData = newMetadata.ToString(SaveOptions.OmitDuplicateNamespaces); 

                
            }
        }

        public virtual string Name
        {
            get
            {
                return (string)this.GetValue("Name", null);
            }
            set
            {
                this.SetValue("Name", value);
            }
        }

        public virtual string CodeName
        {
            get
            {
                return LongPath.GetFileNameWithoutExtension(this.Name);
            }
        }

        public virtual string FullName
        {
            get
            {
                return this.Package != null ? this.Package.Name + "." + this.Name : this.Name;
            }
        }


        public virtual string FullCodeName
        {
            get
            {
                return this.Package != null ? this.Package.FullCodeName + "." + this.CodeName : this.CodeName;
            }

        }

        #region IScriptable Members

      
        ScriptOptions IScriptable.ScriptOptions
        {
            get
            {
                return ScriptOptions.Default;
            }
            set
            {
               
            }
        }

        string IScriptable.ExternalType { get; set; }
       

        #endregion
    }
}
