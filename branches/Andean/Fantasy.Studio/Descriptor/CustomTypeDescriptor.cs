

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;

using System.Reflection;
using Fantasy.ServiceModel;
using Fantasy.Adaption;

namespace Fantasy.Studio.Descriptor
{

	/// <summary>
	/// Represents a collection of custom properties that can be selected into a
	/// PropertyGrid to provide functionality beyond that of the simple reflection
	/// normally used to query an object's properties.
	/// </summary>

	public class CustomTypeDescriptor : ObjectWithSite, ICustomTypeDescriptor
	{

		
		/// <summary>
		/// Initializes a new instance of the PropertyBag class.
		/// </summary>
		public CustomTypeDescriptor(object value)
		{
            this._component = value;
		}

       

		private object _component;

		public object Component
		{
			get
			{
				return _component;
			}
		}


        private string _defaultProperty = null;

		/// <summary>
		/// Gets or sets the name of the default property in the collection.
		/// </summary>
		public string DefaultProperty
		{
			get { return _defaultProperty; }
			set { _defaultProperty = value; }
		}

		private List<CustomPropertyDescriptor> _properties = new List<CustomPropertyDescriptor>();


		/// <summary>
		/// Gets the collection of properties contained within this PropertyBag.
		/// </summary>
		public List<CustomPropertyDescriptor> Properties
		{
			get { return _properties; }
		}

		public AttributeCollection GetAttributes()
		{
			return new AttributeCollection();
		}

		private string _className = null;

		public string ClassName
		{
			get { return _className; }
			set { _className = value; }
		}

		public string GetClassName()
		{
            return ClassName;
		}

		public string GetComponentName()
		{
            return ServiceManager.Services.GetRequiredService<IAdapterManager>().GetAdapter<string>(this._component);
		}

		private TypeConverter _converter = null;

		public TypeConverter Converter
		{
			get { return _converter; }
			set { _converter = value; }
		}


		public TypeConverter GetConverter()
		{
			return _converter;
		}

		public EventDescriptor GetDefaultEvent()
		{
			return null;
		}

		public PropertyDescriptor GetDefaultProperty()
		{
			foreach(CustomPropertyDescriptor prop in this.Properties)
			{
				if (prop.Name == _defaultProperty)
				{
					return prop;
				}
			}

			return null;
		}

		private object _editor = null;

		public object Editor
		{
			get { return _editor; }
			set { _editor = value; }
		}

		public object GetEditor(Type editorBaseType)
		{
			return _editor;
		}

		public EventDescriptorCollection GetEvents()
		{
			return GetEvents(new Attribute[0]);
		}

		public EventDescriptorCollection GetEvents(Attribute[] attributes)
		{
			
			return new EventDescriptorCollection(new EventDescriptor[0]);
		}

		public PropertyDescriptorCollection GetProperties()
		{
			return ((ICustomTypeDescriptor)this).GetProperties(new Attribute[0]);
		}

		public PropertyDescriptorCollection GetProperties(Attribute[] attributes)
		{
			return new PropertyDescriptorCollection(Properties.ToArray());
		}

		public object GetPropertyOwner(PropertyDescriptor pd)
		{
			return _component;
		}
	}

	

	public class PropertNotFoundException : Exception
	{
		public PropertNotFoundException(Type t, string prop)
			: base(t.FullName + " has not property named " + prop + ".")
		{
		}
	}
}
