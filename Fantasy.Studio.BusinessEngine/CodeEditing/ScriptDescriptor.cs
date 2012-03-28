using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.BusinessEngine;
using System.Xml.Linq;

namespace Fantasy.Studio.BusinessEngine.CodeEditing
{
    public class ScriptDescriptor
    {
        public ScriptDescriptor(BusinessScript script)
        {
            this._script = script;
            _metadata = XElement.Parse(this._script.MetaData);
        }


        BusinessScript _script;
        XElement _metadata;


        public string FileName
        {
            get
            {
                return this._script.Name;
            }
        }

        public string BuildAction
        {
            get
            {
                return this._metadata.Name.LocalName;
            }
            set
            {
                XElement newMetadata = new XElement(value);
                foreach (XElement child in _metadata.Elements().ToArray())
                {
                    child.Remove();
                    newMetadata.Add(child);
                }

                this._metadata = newMetadata;

                this._script.MetaData = this._metadata.ToString();
            }

        }

        public string CopyTo
        {
            get
            {
                return (string)this._metadata.Element("CopyToOutputDirectory");
            }
            set
            {
                this._metadata.SetElementValue("CopyToOutputDirectory", value);
                this._script.MetaData = this._metadata.ToString();
            }
        }

        public string Generator
        {
            get
            {
                return (string)this._metadata.Element("Generator");
            }
            set
            {
                this._metadata.SetElementValue("Generator", value);
                this._script.MetaData = this._metadata.ToString();
            }
        }

        public string CustomToolNamespace
        {
            get
            {
                return (string)this._metadata.Element("CustomToolNamespace");
            }
            set
            {
                this._metadata.SetElementValue("CustomToolNamespace", value);
                this._script.MetaData = this._metadata.ToString();
            }
        }

    }
}
