using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Xml.Linq;
using Fantasy.BusinessEngine.Collections;
using Fantasy.XSerialization;
using Fantasy.BusinessEngine.Properties;
using Fantasy.BusinessEngine.EntityExtensions;

namespace Fantasy.BusinessEngine
{
    public class BusinessProperty : BusinessEntity
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


        public virtual BusinessClass Class
        {
            get
            {
                return (BusinessClass)this.GetValue("Class", null);
            }
            set
            {
                this.SetValue("Class", value);
            }
        }


        public virtual string TableSchema { get; private set; }
        public virtual string TableName { get; private set; }


      


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

        public virtual BusinessDataType DataType
        {
            get
            {
                return (BusinessDataType)this.GetValue("DataType", null);
            }
            set
            {
                this.SetValue("DataType", value);
            }
        }

        public virtual event EventHandler DataTypeNameChanged;

        protected virtual void OnDataTypeNameChanged(EventArgs e)
        {
            if (this.DataTypeNameChanged != null)
            {
                this.DataTypeNameChanged(this, e);
            }
        }

        protected override void OnPropertyChanged(EntityPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            switch (e.PropertyName)
            {
                case "DataType":
                case "DataClassType":
                case "DataEnumType":
                    OnNotifyPropertyChangedPropertyChanged("DataTypeName");
                    break;
            }
            
        }

        public virtual string DataTypeName
        {
            get
            {
                if (this.DataType != null)
                {
                    if (this.DataType.Id == BusinessDataType.WellknownIds.Class && this.DataClassType != null)
                    {
                        return this.DataClassType.FullName;
                    }
                    else if (this.DataType.Id == BusinessDataType.WellknownIds.Enum && this.DataEnumType != null)
                    {
                        return this.DataEnumType.FullName;
                    }
                    else
                    {
                        return this.DataType.Name;
                    }

                }
                else
                {
                    return null;
                }

            }
        }

        public override EntityState EntityState
        {
            get
            {
                return base.EntityState;
            }
            protected set
            {
                base.EntityState = value;
                if (value == BusinessEngine.EntityState.Clean)
                {
                    this.TableName = this.Class.TableName;
                    this.TableSchema = this.Class.TableSchema; 
                }
            }
        }
       

        public virtual BusinessClass DataClassType
        {
            get
            {
                return (BusinessClass)this.GetValue("DataClassType", null);
            }
            set
            {
                this.SetValue("DataClassType", value);
               
            }
        }

        public virtual BusinessEnum DataEnumType
        {
            get
            {
                return (BusinessEnum)this.GetValue("DataEnumType", null);
            }
            set
            {
                this.SetValue("DataEnumType", value);
               
            }
        }

        public virtual string FieldName
        {
            get
            {
                return (string)this.GetValue("FieldName", null);
            }
            set
            {
                this.SetValue("FieldName", value);
            }
        }

        public virtual string FieldType
        {
            get
            {
                return (string)this.GetValue("FieldType", null);
            }
            set
            {
                this.SetValue("FieldType", value);
            }
        }

        public virtual int Length
        {
            get
            {
                return (int)this.GetValue("Length", 0);
            }
            set
            {
                this.SetValue("Length", value);
            }
        }

        public virtual int Precision
        {
            get
            {
                return (int)this.GetValue("Precision", 0);
            }
            set
            {
                this.SetValue("Precision", value);
            }
        }

        public virtual bool IsNullable
        {
            get
            {
                return (bool)this.GetValue("IsNullable", true);
            }
            set
            {
                this.SetValue("IsNullable", value);
            }
        }

        public virtual string DefaultValue
        {
            get
            {
                return (string)this.GetValue("DefaultValue", null);
            }
            set
            {
                this.SetValue("DefaultValue", value);
            }
        }

        public virtual bool IsCalculated
        {
            get
            {
                return (bool)this.GetValue("IsCalculated", false);
            }
            set
            {
                this.SetValue("IsCalculated", value);
            }
        }

        public virtual long DisplayOrder
        {
            get
            {
                return (long)this.GetValue("DisplayOrder", 0L);
            }
            set
            {
                this.SetValue("DisplayOrder", value);
            }
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


        public class WellKnownIds
        {
            public readonly static Guid Id = new Guid("C9B092BE-FCE4-4793-9BBA-9F3300AC9427");

            public readonly static Guid ClassId = new Guid("57B4A057-35B6-4E09-89F4-9F3300AC942F");

            public readonly static Guid CreationTime = new Guid("0BE3B780-5B3D-4840-8AAA-9F3300AC942F");

            public readonly static Guid ModificationTime = new Guid("4ED247A4-7E5A-4B2D-9444-9F3300AC942F");

            public readonly static Guid Name = new Guid("E5434005-55E8-482F-A46A-9F3300AC942F");

            public readonly static Guid IsSystem = new Guid("9171981D-4D61-499D-B8A9-9F3300AC942F");
               
        }

    }
}
