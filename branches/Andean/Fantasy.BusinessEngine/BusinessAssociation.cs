using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.BusinessEngine.Collections;
using System.Xml.Linq;
using Fantasy.XSerialization;

namespace Fantasy.BusinessEngine
{
    public class BusinessAssociation : BusinessEntity, IGernateCodeBusinessEntity
    {

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


        public virtual string FullName
        {
            get
            {
                return this.Package != null && this.Package.Id != BusinessPackage.RootPackageId ? this.Package.Name + "." + this.Name : this.Name;
            }
        }


        public virtual string FullCodeName
        {
            get
            {
                return this.Package != null ? this.Package.FullCodeName + "." + this.CodeName : this.CodeName;
            }

        }

        public virtual string CodeName
        {
            get
            {
                return (string)this.GetValue("CodeName", null);
            }
            set
            {
                this.SetValue("CodeName", value);
            }
        }

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


        public virtual string TableName
        {
            get
            {
                return (string)this.GetValue("TableName", null);
            }
            set
            {
                this.SetValue("TableName", value);
            }
        }

        public virtual string TableSchema
        {
            get
            {
                return (string)this.GetValue("TableSchema", null);
            }
            set
            {
                this.SetValue("TableSchema", value);
            }
        }

        public virtual string TableSpace
        {
            get
            {
                return (string)this.GetValue("TableSpace", null);
            }
            set
            {
                this.SetValue("TableSpace", value);
            }
        }

        public virtual BusinessClass LeftClass
        {
            get
            {
                return (BusinessClass)this.GetValue("LeftClass", null);
            }
            set
            {
                this.SetValue("LeftClass", value);
            }
        }


        public virtual string LeftRoleName
        {
            get
            {
                return (string)this.GetValue("LeftRoleName", null);
            }
            set
            {
                this.SetValue("LeftRoleName", value);
            }
        }

        public virtual string LeftRoleCode
        {
            get
            {
                return (string)this.GetValue("LeftRoleCode", null);
            }
            set
            {
                this.SetValue("LeftRoleCode", value);
            }
        }

        public virtual string LeftCardinality
        {
            get
            {
                return (string)this.GetValue("LeftCardinality", "1");
            }
            set
            {
                this.SetValue("LeftCardinality", value);
            }
        }


        public virtual bool LeftNavigatable
        {
            get
            {
                return (bool)this.GetValue("LeftNavigatable", true);
            }
            set
            {
                this.SetValue("LeftNavigatable", value);
            }
        }

        public virtual long LeftRoleDisplayOrder
        {
            get
            {
                return (long)this.GetValue("LeftRoleDisplayOrder", 0L);
            }
            set
            {
                this.SetValue("LeftRoleDisplayOrder", value);
            }
        }

        public virtual long RightRoleDisplayOrder
        {
            get
            {
                return (long)this.GetValue("RightRoleDisplayOrder", 0L);
            }
            set
            {
                this.SetValue("RightRoleDisplayOrder", value);
            }
        }


        public virtual BusinessClass RightClass
        {
            get
            {
                return (BusinessClass)this.GetValue("RightClass", null);
            }
            set
            {
                this.SetValue("RightClass", value);
            }
        }


        public virtual string RightRoleName
        {
            get
            {
                return (string)this.GetValue("RightRoleName", null);
            }
            set
            {
                this.SetValue("RightRoleName", value);
            }
        }

        public virtual string RightRoleCode
        {
            get
            {
                return (string)this.GetValue("RightRoleCode", null);
            }
            set
            {
                this.SetValue("RightRoleCode", value);
            }
        }


        public virtual string RightCardinality
        {
            get
            {
                return (string)this.GetValue("RightCardinality", "*");
            }
            set
            {
                this.SetValue("RightCardinality", value);
            }
        }


        public virtual bool RightNavigatable
        {
            get
            {
                return (bool)this.GetValue("RightNavigatable", true);
            }
            set
            {
                this.SetValue("RightNavigatable", value);
            }
        }

        public virtual string ExtensionsData
        {
            get
            {
                return (string)this.GetValue("ExtensionsData", null);
            }
            set
            {
                this.SetValue("ExtensionsData", value);
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            this.LoadExtensions();
            base.OnLoad(e);
        }

       

        protected override void OnCreate(EventArgs e)
        {
            this.LoadExtensions();
            base.OnCreate(e);
        }



        private void LoadExtensions()
        {
            XNamespace ns = Consts.ExtensionsNamespace;

            if (!string.IsNullOrEmpty(this.ExtensionsData))
            {
                XElement root = XElement.Parse(this.ExtensionsData);
                XElement left = root.Element(ns + "left");
                LoadExtensions(left, this._leftExtensions);
                XElement right = root.Element(ns + "right");
                LoadExtensions(right, this._rightExtensions);
            }

            this._leftExtensions.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(Extensions_CollectionChanged);
            this._rightExtensions.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(Extensions_CollectionChanged);
        }

        private void LoadExtensions(XElement element, ObservableList<IEntityExtension> extensions)
        {
            XName typeAttr = (XNamespace)Consts.ExtensionsNamespace + "type";
            foreach (XElement child in element.Elements())
            {
                string typeName = (string)child.Attribute(typeAttr);
                if (typeName != null)
                {
                    Type type = Type.GetType(typeName);
                    if (type != null)
                    {
                        XSerializer ser = new XSerializer(type);
                        try
                        {
                            IEntityExtension extension = (IEntityExtension)ser.Deserialize(child);
                            extensions.Add(extension);
                        }
                        catch
                        {

                        }
                    }

                }
            }


            foreach (IEntityExtension extension in extensions)
            {
                extension.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(Extension_PropertyChanged);
            }

           
        }

        private ObservableList<IEntityExtension> _leftExtensions = new ObservableList<IEntityExtension>();

        public virtual IList<IEntityExtension> LeftExtensions
        {
            get { return _leftExtensions; }
        }

        private void Extensions_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case System.Collections.Specialized.NotifyCollectionChangedAction.Add:
                    foreach (IEntityExtension extension in e.NewItems)
                    {
                        extension.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(Extension_PropertyChanged);
                    }
                    break;

                case System.Collections.Specialized.NotifyCollectionChangedAction.Remove:
                    foreach (IEntityExtension extension in e.OldItems)
                    {
                        extension.PropertyChanged -= new System.ComponentModel.PropertyChangedEventHandler(Extension_PropertyChanged);
                    }
                    break;
            }
            this.UpdateExtensionData();
        }

        private void Extension_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            this.UpdateExtensionData();
        }


        private void UpdateExtensionData()
        {
            XNamespace ns = Consts.ExtensionsNamespace;
            XElement element = new XElement(ns + "extensions");

            XElement left = new XElement(ns + "left");
            element.Add(left);
            UpdateExtensionData(left, this._leftExtensions);

            XElement right = new XElement(ns + "right");
            element.Add(right);
            UpdateExtensionData(right, this._rightExtensions);

            
            this.ExtensionsData = element.ToString(SaveOptions.OmitDuplicateNamespaces);
        }

        private void UpdateExtensionData(XElement element, ObservableList<IEntityExtension> extensions)
        {
            XNamespace ns = Consts.ExtensionsNamespace;
            foreach (IEntityExtension extension in extensions)
            {
                XElement child = new XElement(ns + "extension");
                child.SetAttributeValue(ns + "type", extension.GetType().VersionFreeTypeName());

                XSerializer ser = new XSerializer(extension.GetType());
                ser.Serialize(child, extension);
                element.Add(child);
            }
        }


        private ObservableList<IEntityExtension> _rightExtensions = new ObservableList<IEntityExtension>();

        public virtual IList<IEntityExtension> RightExtensions
        {
            get { return _rightExtensions; }
        }
    }
}
