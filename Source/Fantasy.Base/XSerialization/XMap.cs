using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using Fantasy;
using System.Xml.Linq;
using System.Xml;


namespace Fantasy.XSerialization
{
    class XTypeMap
    {
        public XTypeMap(Type t)
        {
            this.TargetType = t;
            this.SerializableAttribute = (XSerializableAttribute)t.GetCustomAttributes(typeof(XSerializableAttribute), true).SingleOrDefault();
            this.LoadNamespaceMap();
            this.LoadAttributeMaps();
            this.LoadElementMaps();
            this.LoadArrayMaps();
            this.LoadValueMap();

            this._attributeMaps = new List<XAttributeMap>(this._attributeMaps.OrderBy(map => map.Order));
            this._elementMaps = new List<XMemberMap>(this._elementMaps.OrderBy(map => map.Order));
        }

        private void LoadNamespaceMap()
        {
            var query = from mi in this.GetAllMembers()
                        where mi.IsDefined(typeof(XNamespaceAttribute), true)
                            && (mi.MemberType == MemberTypes.Field || (mi.MemberType == MemberTypes.Property && ((PropertyInfo)mi).CanRead && ((PropertyInfo)mi).CanWrite))
                        select new XNamespaceMap() { Member = mi };
            this._namespaceMap = query.FirstOrDefault();
        }

        private XNamespaceMap _namespaceMap = null;

        private void LoadValueMap()
        {
            var query = from mi in this.GetAllMembers() 
                        where mi.IsDefined(typeof(XValueAttribute), true)
                            && (mi.MemberType == MemberTypes.Field || (mi.MemberType == MemberTypes.Property && ((PropertyInfo)mi).CanRead && ((PropertyInfo)mi).CanWrite))
                        select new { member = mi, attribute = (XValueAttribute)mi.GetCustomAttributes(typeof(XValueAttribute), true)[0] };
            var v = query.SingleOrDefault();
            if (v != null)
            {
                this.ValueMap = new XValueMap();
                this.ValueMap.Member = v.member;
                this.ValueMap.Value = v.attribute;
                Type mt = v.member.MemberType == MemberTypes.Field ? ((FieldInfo)v.member).FieldType : ((PropertyInfo)v.member).PropertyType;
                this.ValueMap.Converter = this.ValueMap.Value.XConverter != null ? (TypeConverter)Activator.CreateInstance(this.ValueMap.Value.XConverter) : XHelper.Default.CreateXConverter(mt);
            }

        }

        private IEnumerable<MemberInfo> GetAllMembers()
        {
            var query = from t in this.TargetType.Flatten(x=>x.BaseType)
                        let members = t.GetMembers(_memberBindingFlags)
                        from mi in members select mi;
            return query;

        }

        private string GetQualifiedName(string ns, string name)
        {
            return ns + "\n" + name;
        }

        private void LoadAttributeMaps()
        {
            var query = from mi in this.GetAllMembers() 
                        where mi.IsDefined(typeof(XAttributeAttribute), false)
                            && (mi.MemberType == MemberTypes.Field || (mi.MemberType == MemberTypes.Property && ((PropertyInfo)mi).CanRead && ((PropertyInfo)mi).CanWrite))
                        select new { member = mi, attribute = (XAttributeAttribute)mi.GetCustomAttributes(typeof(XAttributeAttribute), true)[0] };
            foreach (var v in query)
            {

                XAttributeMap map = new XAttributeMap();
                map.Member = v.member;
                map.Attribute = v.attribute;
                map.Order = v.attribute.Order;
                Type mt = v.member.MemberType == MemberTypes.Field ? ((FieldInfo)v.member).FieldType : ((PropertyInfo)v.member).PropertyType;
                if (!mt.IsDefined(typeof(XSerializableAttribute), false))
                {
                    map.Converter = map.Attribute.XConverter != null ? (TypeConverter)Activator.CreateInstance(map.Attribute.XConverter) : XHelper.Default.CreateXConverter(mt);

                }
                this.AttributeMaps.Add(map);
                this.AttributeQNameMaps.Add(this.GetQualifiedName(v.attribute.NamespaceUri, v.attribute.Name), map);
            }

        }

        private void LoadElementMaps()
        {
            var query = from mi in this.GetAllMembers()
                        where mi.IsDefined(typeof(XElementAttribute), true)
                            && (mi.MemberType == MemberTypes.Field || (mi.MemberType == MemberTypes.Property && ((PropertyInfo)mi).CanRead && ((PropertyInfo)mi).CanWrite))
                        select new { member = mi, attribute = (XElementAttribute)mi.GetCustomAttributes(typeof(XElementAttribute), true)[0] };
            foreach (var v in query)
            {
                XElementMap map = new XElementMap();
                map.Member = v.member;
                map.Element = v.attribute;
                map.Order = v.attribute.Order;
                Type mt = v.member.MemberType == MemberTypes.Field ? ((FieldInfo)v.member).FieldType : ((PropertyInfo)v.member).PropertyType;

                if (!mt.IsDefined(typeof(XSerializableAttribute), false))
                {
                    map.Converter = map.Element.XConverter != null ? (TypeConverter)Activator.CreateInstance(map.Element.XConverter) : XHelper.Default.CreateXConverter(mt);
                }
                this.ElementMaps.Add(map);
            }
        }

        private Type GetGenericListType(Type type)
        {

            if (type.GetGenericTypeDefinition() == typeof(ICollection<> ))
            {
                return type;
            }
            foreach (Type intf in type.GetInterfaces())
            {
                if (intf.IsGenericType)
                {
                    if (intf.GetGenericTypeDefinition() == typeof(ICollection<>))
                    {
                        // if needed, you can also return the type used as generic argument 
                        return intf;
                    }
                }
            }
            return null;
        }

        private bool IsValidCollectionMemeber(MemberInfo mi)
        {
            Type memberType = null;
            if (mi is FieldInfo)
            {
                FieldInfo fi = (FieldInfo)mi;
                if (fi.FieldType.IsArray)
                {
                    return true;
                }
                else
                {
                    memberType = fi.FieldType;
                }
                 
            }
            else
            {
                PropertyInfo pi = (PropertyInfo)mi;
                if (pi.CanRead == false)
                {
                    return false;
                }

                if (pi.PropertyType.IsArray)
                {
                    return pi.CanWrite;
                }

                memberType = pi.PropertyType;
            }

            return typeof(IList).IsAssignableFrom(memberType) || GetGenericListType(memberType) != null; 
        }

        private const BindingFlags _memberBindingFlags = BindingFlags.Instance | BindingFlags.GetField | BindingFlags.GetProperty | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.SetField | BindingFlags.SetProperty | BindingFlags.DeclaredOnly;

        private void LoadArrayMaps()
        {
            var members = this.GetAllMembers(); 
            var query = from mi in members 
                        where mi.IsDefined(typeof(XArrayAttribute), true)
                            && IsValidCollectionMemeber(mi)
                        select new { member = mi, attribute = (XArrayAttribute)mi.GetCustomAttributes(typeof(XArrayAttribute), true)[0] };

            foreach (var v in query)
            {
                XArrayMap map = new XArrayMap();
                map.Member = v.member;
                map.Array = v.attribute;
                map.Order = v.attribute.Order;

                foreach (XArrayItemAttribute item in map.Member.GetCustomAttributes(typeof(XArrayItemAttribute), true))
                {
                    map.Items.Add(item);
                }

                this.ElementMaps.Add(map);
            }

        }

        public void LoadByXAttributes(IServiceProvider context, XElement element, object instance)
        {
            this.LoadNamespace(element, instance);
            this.LoadAttributes(element, instance);
            this.LoadChildElements(context, element, instance);
            this.LoadArrays(context, element, instance);
            this.LoadValue(element, instance);
        }

        private void LoadNamespace(XElement element, object instance)
        {
            if (this._namespaceMap != null)
            {
                XmlNamespaceManager mngr = XHelper.CreateXmlNamespaceManager(element, true);
                
                this.SetValue(this._namespaceMap.Member, instance, mngr);
            }
        }

        private void LoadValue(XElement element, object instance)
        {
            if (this.ValueMap != null && element.Value != string.Empty)
            {
                Type t = this.ValueMap.Member is PropertyInfo ? ((PropertyInfo)this.ValueMap.Member).PropertyType : ((FieldInfo)this.ValueMap.Member).FieldType;
                object value = this.ValueMap.Converter.ConvertTo(element.Value, t);
                this.SetValue(this.ValueMap.Member, instance, value);
            }
        }

        public void Load(IServiceProvider context, XElement element, object instance)
        {
            if (instance is IXSerializable)
            {
                ((IXSerializable)instance).Load(context, element);
            }
            else
            {
                this.LoadByXAttributes(context, element, instance);
            }
        }

        private void LoadArrays(IServiceProvider context, XElement element, object instance)
        {
            foreach (XArrayMap map in this.ElementMaps.Where(m => m is XArrayMap))
            {
                XElement collectionElement = null;

                if (!String.IsNullOrEmpty(map.Array.Name))
                {
                    XNamespace ns = !String.IsNullOrEmpty(map.Array.NamespaceUri) ? map.Array.NamespaceUri : element.Name.NamespaceName;
                    XName name = ns + map.Array.Name;
                    collectionElement = element.Element(name);
                }
                else
                {
                    collectionElement = element;
                }

                if (collectionElement != null)
                {

                    Type collectionType = map.Member is PropertyInfo ? ((PropertyInfo)map.Member).PropertyType : ((FieldInfo)map.Member).FieldType;
                    object list = this.GetValue(map.Member, instance);

                    if (list != null || collectionType.IsArray)
                    {
                        ArrayList tempList = new ArrayList();
                        if (map.Array.Serializer != null)
                        {
                            IXCollectionSerializer ser = (IXCollectionSerializer)Activator.CreateInstance(map.Array.Serializer);
                            foreach (object o in ser.Load(context, collectionElement))
                            {
                                tempList.Add(o);
                            }
                        }
                        else
                        {



                            foreach (XElement itemElement in collectionElement.Elements())
                            {
                                XArrayItemAttribute ia = (from attr in map.Items
                                                          where attr.Name == itemElement.Name.LocalName &&
                                                              (
                                                               (attr.NamespaceUri == itemElement.Name.NamespaceName)
                                                               || ((String.IsNullOrEmpty(attr.NamespaceUri) && itemElement.Name.NamespaceName == collectionElement.Name.NamespaceName))
                                                              )
                                                          select attr).SingleOrDefault();
                                if (ia != null)
                                {
                                    TypeConverter cvt = null;
                                    if (!ia.Type.IsDefined(typeof(XSerializableAttribute), false))
                                    {
                                        cvt = ia.XConverter != null ? (TypeConverter)Activator.CreateInstance(ia.XConverter) : XHelper.Default.CreateXConverter(ia.Type);
                                    }
                                    object value = this.ValueFromElement(context, itemElement, ia.Type, cvt);
                                    tempList.Add(value);
                                }
                            }


                        }

                        if (collectionType.IsArray)
                        {
                            Array arr = Array.CreateInstance(collectionType.GetElementType(), tempList.Count);
                            tempList.CopyTo(arr, 0);
                            this.SetValue(map.Member, instance, arr);
                        }
                        else if (typeof(IList).IsAssignableFrom(collectionType))
                        {
                            foreach (object o in tempList)
                            {
                                ((IList)list).Add(o);
                            }
                        }
                        else
                        {
                            Type t = GetGenericListType(collectionType);
                            if (t != null)
                            {
                                foreach (object o in tempList)
                                {
                                    t.InvokeMember("Add", BindingFlags.Instance | BindingFlags.InvokeMethod | BindingFlags.NonPublic | BindingFlags.Public, null, list, new object[] { o });
                                }
                            }
                        }
                    }
                  
                }
            }
        }

        private void LoadChildElements(IServiceProvider context, XElement element, object instance)
        {
            foreach (XElementMap map in this.ElementMaps.Where(m => m is XElementMap))
            {

                XNamespace ns = !String.IsNullOrEmpty(map.Element.NamespaceUri) ? map.Element.NamespaceUri : element.Name.NamespaceName;
                XName name = ns + map.Element.Name;
                XElement childElement = element.Element(name);
                if (childElement != null)
                {
                    object value;
                    Type t = map.Member is PropertyInfo ? ((PropertyInfo)map.Member).PropertyType : ((FieldInfo)map.Member).FieldType;
                    value = this.ValueFromElement(context, childElement, t, map.Converter);
                    this.SetValue(map.Member, instance, value);
                }
            }
        }

        private object ValueFromElement(IServiceProvider context, XElement element, Type targetType, TypeConverter converter)
        {
            object value;

            if (targetType.IsDefined(typeof(XSerializableAttribute), false))
            {
                XTypeMap map = XHelper.Default.GetTypeMap(targetType);
                value = Activator.CreateInstance(targetType);
                map.Load(context, element, value);
            }
            else
            {
                value = converter.ConvertTo(element.Value, targetType);
            }
            return value;
        }

        private void LoadAttributes(XElement element, object instance)
        {
            foreach (XAttributeMap map in this.AttributeMaps)
            {
                XNamespace  ns = map.Attribute.NamespaceUri ?? string.Empty;
                XAttribute node = element.Attribute(ns + map.Attribute.Name);
                if (node != null)
                {
                    TypeConverter cvt = map.Converter;
                    Type t = map.Member is PropertyInfo ? ((PropertyInfo)map.Member).PropertyType : ((FieldInfo)map.Member).FieldType;
                    object value = cvt.ConvertTo(node.Value, t);
                    this.SetValue(map.Member, instance, value);
                }
            }
        }

        public void SaveByXAttributes(IServiceProvider context,XElement element, object instance)
        {
            this.SaveNamespace(element, instance);

            foreach (XAttributeMap attrMap in this._attributeMaps.OrderBy(m=>m.Order) )
            {
                this.SaveAttribute(element, attrMap, instance);
            }
            foreach (XMemberMap memberMap in this._elementMaps.OrderBy(m=>m.Order))
            {
                if (memberMap is XElementMap)
                {
                    this.SaveElement(context, element, (XElementMap)memberMap, instance);
                }
                else
                {
                    this.SaveArray(context, element, (XArrayMap)memberMap, instance);
                }
            }
            this.SaveValue(element, instance);

        }

        private void SaveNamespace(XElement element, object instance)
        {
            if (this._namespaceMap != null)
            {
                XmlNamespaceManager mngr = (XmlNamespaceManager)this.GetValue(this._namespaceMap.Member, instance);
                if (mngr != null)
                {
                    XHelper.AddNamespace(mngr, element);
                }
            }
        }

        private void SaveValue(XElement element, object instance)
        {
            if (this.ValueMap != null)
            {
                object value = this.GetValue(this.ValueMap.Member, instance);

                if (value != null)
                {
                    string s = (string)this.ValueMap.Converter.ConvertFrom(value);
                    element.Value = s;
                }
            }
        }

        public void Save(IServiceProvider context, XElement element, object instance)
        {
            if (instance is IXSerializable)
            {
                ((IXSerializable)instance).Save(context, element);
            }
            else
            {
                this.SaveByXAttributes(context, element, instance);
            }
        }

        private void SaveArray(IServiceProvider context, XElement element, XArrayMap arrMap, object instance)
        {

            IEnumerable collection = this.GetValue(arrMap.Member, instance) as IEnumerable;
            if (collection != null)
            {
                XElement collectionElement;
                if (!String.IsNullOrEmpty(arrMap.Array.Name))
                {
                    collectionElement = this.CreateElement(element, arrMap.Array.NamespaceUri, arrMap.Array.Name);
                }
                else
                {
                    collectionElement = element;
                }

                if (arrMap.Array.Serializer != null)
                {
                    IXCollectionSerializer serializer = (IXCollectionSerializer)Activator.CreateInstance(arrMap.Array.Serializer);
                    serializer.Save(context, collectionElement, collection);
                }
                else
                {
                    foreach (object child in collection)
                    {
                        if (child != null)
                        {
                            Type vt = child.GetType();

                            XArrayItemAttribute ia = (from attribute in arrMap.Items where attribute.Type == vt select attribute).SingleOrDefault();
                            if (ia != null)
                            {
                                XElement childElement = this.CreateElement(collectionElement, ia.NamespaceUri, ia.Name);
                                TypeConverter cvt = null;
                                if (!vt.IsDefined(typeof(XSerializableAttribute), false))
                                {
                                    cvt = ia.XConverter != null ? (TypeConverter)Activator.CreateInstance(ia.XConverter) : XHelper.Default.CreateXConverter(vt);
                                }
                                this.SaveValueToElement(context, childElement, child, cvt);
                            }
                            else
                            {

                            }
                        }
                    }
                }

            }
        }

        private XElement CreateElement(XElement parentElement, string @namespace, string name)
        {
            XNamespace ns = string.IsNullOrEmpty(@namespace) ? parentElement.Name.Namespace : (XNamespace)@namespace;


            XElement rs = new XElement(ns + name);

            
            parentElement.Add(rs);

            return rs;
        }



        private void SaveElement(IServiceProvider context, XElement element, XElementMap eleMap, object instance)
        {
            object value = this.GetValue(eleMap.Member, instance);

            if (value != null)
            {
                XElement valEle = this.CreateElement(element, eleMap.Element.NamespaceUri, eleMap.Element.Name);
                this.SaveValueToElement(context, valEle, value, eleMap.Converter);
            }
        }

        private void SaveValueToElement(IServiceProvider context, XElement element, object value, TypeConverter converter)
        {

            Type vt = value.GetType();
            if (vt.IsDefined(typeof(XSerializableAttribute), false))
            {
                XTypeMap map = XHelper.Default.GetTypeMap(vt);
                map.Save(context, element, value);
            }
            else
            {
                string s = (string)converter.ConvertFrom(value);
                element.Value = s;
            }

        }


        private object GetValue(MemberInfo mi, object instance)
        {
            if (mi is PropertyInfo)
            {
                return ((PropertyInfo)mi).GetValue(instance, null);
            }
            else
            {
                return ((FieldInfo)mi).GetValue(instance);
            }
        }

        private void SetValue(MemberInfo mi, object instance, object value)
        {
            if (mi is PropertyInfo)
            {
                ((PropertyInfo)mi).SetValue(instance, value, null);
            }
            else
            {
                ((FieldInfo)mi).SetValue(instance, value);
            }
        }


        private void SaveAttribute(XElement element, XAttributeMap attrMap, object instance)
        {
            object value = this.GetValue(attrMap.Member, instance);

            if (value != null)
            {
                string s = (string)attrMap.Converter.ConvertFrom(value);

                string ns = attrMap.Attribute.NamespaceUri;

                XAttribute node = new XAttribute(ns + attrMap.Attribute.Name, s);
                element.Add(node);
            }
        }

        public Type TargetType { get; private set; }

        public XSerializableAttribute SerializableAttribute { get; private set; }

        private List<XMemberMap> _elementMaps = new List<XMemberMap>();
        public IList<XMemberMap> ElementMaps
        {
            get
            {
                return _elementMaps;
            }
        }

        private List<XAttributeMap> _attributeMaps = new List<XAttributeMap>();
        public IList<XAttributeMap> AttributeMaps
        {
            get
            {
                return _attributeMaps;
            }
        }

        public XValueMap ValueMap { get; private set; }


        public IDictionary<string, XMemberMap> _elementQNameMaps = new Dictionary<string, XMemberMap>();

        public IDictionary<string, XMemberMap> ElementQNameMaps
        {
            get
            {
                return _elementQNameMaps;
            }
        }

        public IDictionary<string, XAttributeMap> _attributeQNameMaps = new Dictionary<string, XAttributeMap>();

        public IDictionary<string, XAttributeMap> AttributeQNameMaps
        {
            get
            {
                return _attributeQNameMaps;
            }

        }
    }

        abstract class XMemberMap
        {
            public XMemberMap()
            {
                Order = Int32.MaxValue;
            }
            public int Order { get; set; }
            public MemberInfo Member { get; set; }
        }

        class XNamespaceMap : XMemberMap 
        {

        }

        class XAttributeMap : XMemberMap
        {
            public XAttributeAttribute Attribute { get; set; }
            public TypeConverter Converter { get; set; }


        }

        class XElementMap : XMemberMap
        {

            public XElementAttribute Element { get; set; }
            public TypeConverter Converter { get; set; }

        }

        class XValueMap : XMemberMap
        {
            public XValueAttribute Value { get; set; }
            public TypeConverter Converter { get; set; }
        }

        class XArrayMap : XMemberMap
        {
            public XArrayAttribute Array { get; set; }

            private IList<XArrayItemAttribute> _items = new List<XArrayItemAttribute>();

            public IList<XArrayItemAttribute> Items
            {
                get
                {
                    return _items;
                }
            }
        }
   
}
