using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.BusinessEngine.Collections;
using System.Drawing;
using System.IO;
using System.Xml.Linq;
using Fantasy.XSerialization;
using Fantasy.BusinessEngine.EntityExtensions;

namespace Fantasy.BusinessEngine
{
    public class BusinessClass : BusinessEntity, IGernateCodeBusinessEntity, IScriptable
    {
        public BusinessClass()
        {
  
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


        public virtual bool IsAbstract
        {
            get
            {
                return (bool)this.GetValue("IsAbstract", false);
            }
            set
            {
                this.SetValue("IsAbstract", value);
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

        public virtual BusinessClass ParentClass 
        {
            get
            {
                return (BusinessClass)this.GetValue("ParentClass", null);
            }
            set
            {
                this.SetValue("ParentClass", value);
            }
        }

        private IObservableList<BusinessClass> _persistedChildClasses = new ObservableList<BusinessClass>();
        protected internal virtual IObservableList<BusinessClass> PersistedChildClasses
        {
            get { return _persistedChildClasses; }
            private set
            {
                if (_persistedChildClasses != value)
                {
                    _persistedChildClasses = value;
                    _childClasses.Source = value;
                }
            }
        }

        private ObservableListView<BusinessClass> _childClasses;
        public virtual IObservableList<BusinessClass> ChildClasses
        {
            get
            {
                if (this._childClasses == null)
                {
                    this._childClasses = new ObservableListView<BusinessClass>(this._persistedChildClasses);
                }
                return _childClasses;
            }
        }



        private IObservableList<BusinessProperty> _persistedProperties = new ObservableList<BusinessProperty>();
        protected internal virtual IObservableList<BusinessProperty> PersistedProperties
        {
            get { return _persistedProperties; }
            private set
            {
                if (_persistedProperties != value)
                {
                    _persistedProperties = value;
                    _properties.Source = value;
                }
            }
        }

        private ObservableListView<BusinessProperty> _properties;
        public virtual IObservableList<BusinessProperty> Properties
        {
            get
            {
                if (this._properties == null)
                {
                    this._properties = new ObservableListView<BusinessProperty>(this._persistedProperties);
                }
                return _properties;
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

        public virtual bool IsSimple
        {
            get
            {
                return (bool)this.GetValue("IsSimple", false);
            }
            set
            {
                this.SetValue("IsSimple", value);
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

        public virtual string AutoScript
        {
            get
            {
                return (string)this.GetValue("AutoScript", null);
            }
            set
            {
                this.SetValue("AutoScript", value);
            }
        }

        public static readonly Guid RootClassId = new Guid("bf0aa7f4-588f-4556-963d-33242e649d57");

        public override EntityState EntityState
        {
            get
            {
                return base.EntityState;
            }
            protected set
            {
                base.EntityState = value;
            }
        }

        private IObservableList<BusinessAssociation> _persistedLeftAssociations = new ObservableList<BusinessAssociation>();
        protected internal virtual IObservableList<BusinessAssociation> PersistedLeftAssociations
        {
            get { return _persistedLeftAssociations; }
            private set
            {
                if (_persistedLeftAssociations != value)
                {
                    _persistedLeftAssociations = value;
                    if (_leftAssocations != null)
                    {
                        _leftAssocations.Source = value;
                    }
                }
            }
        }

        private ObservableListView<BusinessAssociation> _leftAssocations;
        public virtual IObservableList<BusinessAssociation> LeftAssociations
        {
            get
            {
                if (this._leftAssocations == null)
                {
                    this._leftAssocations = new ObservableListView<BusinessAssociation>(this._persistedLeftAssociations);
                }
                return _leftAssocations;
            }
        }




        private IObservableList<BusinessAssociation> _persistedRightAssociations = new ObservableList<BusinessAssociation>();
        protected internal virtual IObservableList<BusinessAssociation> PersistedRightAssociations
        {
            get { return _persistedRightAssociations; }
            private set
            {
                if (_persistedRightAssociations != value)
                {
                    _persistedRightAssociations = value;
                    if (_rightAssociations != null)
                    {
                        _rightAssociations.Source = value;
                    }
                }
            }
        }

        private ObservableListView<BusinessAssociation> _rightAssociations;
        public virtual IObservableList<BusinessAssociation> RightAssociations
        {
            get
            {
                if (this._rightAssociations == null)
                {
                    this._rightAssociations = new ObservableListView<BusinessAssociation>(this._persistedRightAssociations);
                }
                return _rightAssociations;
            }
        }


        public virtual ScriptOptions ScriptOptions
        {
            get
            {
                return (ScriptOptions)this.GetValue("ScriptOptions", ScriptOptions.Default);
            }
            set
            {
                this.SetValue("ScriptOptions", value);
            }
        }

        public virtual string ExternalType
        {
            get
            {
                return (string)this.GetValue("ExternalType", null);
            }
            set
            {
                this.SetValue("ExternalType", value);
            }
        }


        protected internal virtual byte[] PersistedIcon
        {
            get
            {
                return (byte[])this.GetValue("PersistedIcon", null);
            }
            set
            {
                this.SetValue("PersistedIcon", value);
            }
        }

        private Image _icon;
        public virtual Image Icon
        {
            get
            {
                return _icon;
            }
            set
            {
                if (this._icon != value)
                {

                    this._icon = value;
                  
                    if (value != null)
                    {
                        MemoryStream stream = new MemoryStream();
                        value.Save(stream, value.RawFormat);

                        this.PersistedIcon = stream.GetBuffer();
                    }
                    else
                    {
                        this.PersistedIcon = null;
                    }
                    this.OnNotifyPropertyChangedPropertyChanged("Icon");
                     
                }
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            if (this.PersistedIcon != null)
            {
                _icon = Image.FromStream(new MemoryStream(this.PersistedIcon));
            }

            LoadExtensions();

            base.OnLoad(e);
        }

        private void LoadExtensions()
        {
            XName typeAttr = (XNamespace)Consts.ExtensionsNamespace + "type"; 
            if (!string.IsNullOrEmpty(this.ExtensionsData))
            {
                XElement element = XElement.Parse(this.ExtensionsData);
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
                                this.Extensions.Add(extension);
                            }
                            catch
                            {
                                
                            }                           
                        }
                        
                    }
                }
            }

            foreach (IEntityExtension extension in this.Extensions)
            {
                extension.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(Extension_PropertyChanged);
            }

            this._extensions.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(Extensions_CollectionChanged);

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
            foreach (IEntityExtension extension in this.Extensions)
            {
                XElement child = new XElement(ns + "extension");
                child.SetAttributeValue(ns + "type", extension.GetType().VersionFreeTypeName());

                XSerializer ser = new XSerializer(extension.GetType());
                ser.Serialize(child, extension);
                element.Add(child);
            }
            this.ExtensionsData = element.ToString(SaveOptions.OmitDuplicateNamespaces); 
        }

        protected override void OnCreate(EventArgs e)
        {
            this.LoadExtensions();
            base.OnCreate(e);
        }


        protected internal virtual string ExtensionsData
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

        private ObservableList<IEntityExtension> _extensions = new ObservableList<IEntityExtension>();

        public virtual IList<IEntityExtension> Extensions
        {
            get { return _extensions; }
        }

       

    }
}
