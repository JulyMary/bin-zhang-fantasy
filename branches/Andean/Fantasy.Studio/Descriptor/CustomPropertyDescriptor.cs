using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Reflection;

namespace Fantasy.Studio.Descriptor
{
    public class CustomPropertyDescriptor : PropertyDescriptor, IObjectWithSite 
    {
        private CustomTypeDescriptor _owner;

        public IServiceProvider Site { get; set; }

        public CustomTypeDescriptor Owner
        {
            get { return _owner; }
            set { _owner = value; }
        }

        public CustomPropertyDescriptor(string name, string displayName, string category, string description,
             bool browsable, bool readOnly, Type propertyType, IGetAction get, ISetAction set,
             object editor, TypeConverter converter, bool canResetValue, IDefaultValueProvider defaultValueProvider, IServiceProvider site)
            :
            base(name, new Attribute[0])
        {
            this.Site = site;
            _displayName = displayName;
            _category = category;
            _description = description;
            _browsable = browsable;
            _readOnly = readOnly;
            _propertyType = propertyType;
            _getAction = get;
            _setAction = set;
            _editor = editor;
            _converter = converter;
            _canResetValue = canResetValue;
            _defaultValueProvider = defaultValueProvider;
        }



        public override Type ComponentType
        {
            get { return _owner.Component.GetType(); }
        }


        private bool _readOnly = false;


        public override bool IsReadOnly
        {
            get
            {
                return _readOnly || (this._setAction == null && ! this.PropertyInfo.CanWrite); 
            }
        }

        private Type _propertyType = null;

        public override Type PropertyType
        {
            get
            {
                return _propertyType == null ? this.PropertyInfo.PropertyType
                    : _propertyType;
            }
        }





        private IGetAction _getAction = null;

        public override object GetValue(object component)
        {
            object val = _getAction != null ? _getAction.Run(component, Name)
                : this.PropertyInfo.GetValue(component, null);
            return val;

        }

        private PropertyInfo _propertyInfo = null;
        private PropertyInfo PropertyInfo

        {
            get
            {
                if (_propertyInfo == null)
                {
                    Type t = _owner.Component.GetType();

                    while (t != typeof(object) && _propertyInfo == null)
                    {
                        _propertyInfo = t.GetProperty(Name, BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                        t = t.BaseType;
                    }

                    if (_propertyInfo == null)
                    {
                        throw new PropertNotFoundException(_owner.Component.GetType(), Name);
                    }


                }

                return _propertyInfo;
            }

        }

        private bool _canResetValue;

        public override bool CanResetValue(object component)
        {
            return _canResetValue;
        }
        private IDefaultValueProvider _defaultValueProvider;
        public override void ResetValue(object component)
        {
            if (CanResetValue(component))
            {
                object value = _defaultValueProvider.GetValue(component, this.Name, this.PropertyType);
                SetValue(component, value);
            }
        }

        private ISetAction _setAction = null;

        public override void SetValue(object component, object value)
        {
            if (_setAction != null)
            {
                _setAction.Run(component, Name, value);
            }
            else
            {
                
                this.PropertyInfo.SetValue(component, value, null);
            }
        }

        public override bool ShouldSerializeValue(object component)
        {
            return false;
        }


        internal string _category = null;

        public override string Category
        {
            get { return _category; }
        }

        private TypeConverter _converter = null;

        public override TypeConverter Converter
        {
            get { return _converter; }
        }

        private string _description = null;

        public override string Description
        {
            get { return _description; }
        }

        private string _displayName = null;

        public override string DisplayName
        {
            get { return _displayName; }
        }


        private bool _browsable = true;


        public override bool IsBrowsable
        {
            get { return _browsable; }
        }


        private object _editor = null;

        public override object GetEditor(Type editorBaseType)
        {
            return _editor;
        }

    }
}
