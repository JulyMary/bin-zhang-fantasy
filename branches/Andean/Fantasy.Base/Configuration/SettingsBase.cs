using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Xml;
using System.Reflection;
using System.Drawing;
using System.Xml.Serialization;
using System.Configuration;
using System.Collections;
using System.IO;
using System.Xml.Linq;

namespace Fantasy.Configuration
{
    public class SettingsBase : INotifyPropertyChanged
    {
        private Dictionary<string, object> _values = new Dictionary<string, object>();

        private object _syncRoot = new object();


        public virtual object GetValue(string name)
        {
            object rs;
            lock (this._syncRoot)
            {
                if (!this._values.TryGetValue(name, out rs))
                {
                    PropertyInfo prop = this.GetType().GetProperty(name);
                    string text = null;
                    DefaultSettingValueAttribute attr = prop.GetCustomAttributes<DefaultSettingValueAttribute>(true).SingleOrDefault();
                    if (attr != null)
                    {
                        text = attr.Value;
                    }
                    if (string.IsNullOrEmpty(text))
                    {
                        if (IsReadOnlyList(prop))
                        {
                            rs = Activator.CreateInstance(prop.PropertyType);
                            

                        }
                        else if (!IsNullableType(prop.PropertyType) && prop.PropertyType.IsValueType)
                        {
                            rs = Activator.CreateInstance(prop.PropertyType);
                        }
                    }
                    else
                    {
                        if (prop.PropertyType == typeof(XElement))
                        {
                            rs = XElement.Parse(text);
                        }else if (Array.IndexOf(PrimitiveTypes, prop.PropertyType) >= 0)
                        {
                            rs = Parse(text, prop.PropertyType);
                        }
                        else
                        {
                            XmlSerializer ser = new XmlSerializer(prop.PropertyType);
                            StringReader reader = new StringReader(text);
                            rs = ser.Deserialize(reader);
                        }
                    }
                    this._values.Add(name, rs);
                }
                
            }
            return rs;
        }


        private bool IsReadOnlyList(PropertyInfo prop)
        {
            return !prop.CanWrite && prop.PropertyType != typeof(string) && typeof(IEnumerable).IsAssignableFrom(prop.PropertyType) ; 
        }

        public virtual void SetValue(string name, object value)
        {
            object old = this.GetValue(name);
            if (!object.Equals(value, old))
            {
                lock (this._syncRoot)
                {
                    this._values[name] = value;
                }
                this.OnPropertyChanged(name);
            }
        }


        public virtual void Renew(string xml)
        {
            List<string> changedNames = new List<string>();
            lock (_syncRoot)
            {
                
                Dictionary<string, object> newValues = new Dictionary<string, object>();
                LoadXmlToDictionary(xml, newValues);

                var query = from p in this.GetType().GetProperties() where p.DeclaringType.IsSubclassOf(typeof(SettingsBase)) && p.IsDefined(typeof(UserScopedSettingAttribute), true) select p;   

                foreach (PropertyInfo prop in query)
                {
                    bool cx = _values.ContainsKey(prop.Name);
                    bool cy = newValues.ContainsKey(prop.Name);

                    bool changed = false;
                    if (cx && cy)
                    {
                        object x = _values[prop.Name];
                        object y = newValues[prop.Name];
                       
                        if (x!= null && y != null && 
                            typeof(IEnumerable).IsAssignableFrom(prop.PropertyType) && prop.PropertyType != typeof(string))
                        {
                            

                            IEnumerator xe = ((IEnumerable)x).GetEnumerator();
                            IEnumerator ye = ((IEnumerable)y).GetEnumerator();

                            bool nextX, nextY;
                            do
                            {
                                nextX = xe.MoveNext();
                                nextY = ye.MoveNext();
                                if (nextX && nextY)
                                {
                                    if (!object.Equals(xe.Current, ye.Current))
                                    {
                                        changed = true;
                                        break;
                                    }
                                }
                            } while (nextX && nextY);

                            if (nextX ^ nextY)
                            {
                                changed = true;
                            }
                        }
                        else
                        {
                            if (!object.Equals(x, y))
                            {
                                changed = true;
                            }
                        }
                    }
                    else if (cx ^ cy)
                    {
                        changed = true;
                    }
                    if (changed)
                    {
                        changedNames.Add(prop.Name);
                        this._values[prop.Name] = newValues[prop.Name];
                    }
                   
                }
                
            }

            foreach (string name in changedNames)
            {
                this.OnPropertyChanged(name);
            }

        }

        private PropertyInfo GetPropertyInfo(string name)
        {
            PropertyInfo rs = this.GetType().GetProperty(name);
            return rs;
        }

        private void LoadXmlToDictionary(string xml, Dictionary<string, object> dict)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            foreach (XmlElement element in doc.DocumentElement.SelectNodes("*"))
            {
                PropertyInfo prop = this.GetPropertyInfo(element.LocalName);
                if (prop != null)
                {
                    Type t = prop.PropertyType;
                    object value = LoadValueFromElement(t, element);
                    dict[prop.Name] = value;
                }
            }
        }

        private const string xsiUrl = "http://www.w3.org/2001/XMLSchema-instance";
        private static readonly Type[] PrimitiveTypes = new Type[] {typeof(bool), typeof(byte), typeof(char), typeof(decimal), typeof(double), typeof(float), typeof(int), typeof(long), typeof(sbyte), typeof(short), typeof(string), typeof(Color), typeof(DateTime), typeof(Font),
            typeof(Point), typeof(Size), typeof(Guid), typeof(TimeSpan), typeof(ushort), typeof(uint), typeof(ulong), typeof(XElement)};


        private object Parse(string text, Type conversionType)
        {  
            TypeConverterAttribute a = conversionType.GetCustomAttributes<TypeConverterAttribute>(true).FirstOrDefault();
            if (a == null)
            {

                MethodInfo method = conversionType.GetMethod("Parse", BindingFlags.Static | BindingFlags.Public, null, new Type[] { typeof(string) }, null);
                if (method != null)
                {
                    return method.Invoke(null, new object[] { text });
                }
                else
                {
                    return Convert.ChangeType(text, conversionType);
                }
            }
            else
            {
                TypeConverter cvt = (TypeConverter)Activator.CreateInstance(Type.GetType(a.ConverterTypeName));
                return cvt.ConvertFrom(text);
            }
           
        }

        private bool IsList(Type t, out Type elementType)
        {
           
            elementType = null;
            if (t.IsGenericType && t.GetGenericTypeDefinition()== typeof(List<>))
            {
                 elementType = t.GetGenericArguments()[0];
                 return true;
            }
            return false;
        }

        private object LoadValueFromElement(Type t, XmlElement element)
        {

            if (element.GetAttribute("nil", xsiUrl) == "true")
            {
                return Convert.ChangeType(null, t);
            }

            if (IsNullableType(t))
            {
                t = Nullable.GetUnderlyingType(t);  
            }

            if (t == typeof(string))
            {
                return element.InnerText;
            }
            if (t == typeof(XElement))
            {
               XmlElement child = (XmlElement)element.SelectSingleNode("*");
                if (child != null)
                {
                    return child.ToXElement();
                }
                else
                {
                    return null;
                }
            }
            
            if (Array.IndexOf(PrimitiveTypes, t) >= 0)
            {
                string s = element.InnerText;
                return Parse(s, t);
            }

            Type elementType;
            if(this.IsList(t, out elementType))
            {
                XmlSerializer ser = new XmlSerializer(elementType);
                IList rs = (IList)Activator.CreateInstance(t);
                foreach (XmlElement child in element.SelectNodes("*"))
                {
                    XmlNodeReader reader = new XmlNodeReader(child);
                    try
                    {
                        object o = ser.Deserialize(reader);
                        rs.Add(o);
                    }
                    finally
                    {
                        reader.Close();
                    }
                    
                }
                return rs;
            }
            else
            {
                XmlRootAttribute root = new XmlRootAttribute(element.LocalName);
                root.Namespace = element.NamespaceURI;
                XmlSerializer ser = new XmlSerializer(t, root);
                XmlNodeReader reader = new XmlNodeReader(element);
                try
                {
                    return ser.Deserialize(reader);
                }
                finally
                {
                    reader.Close();
                }
            }
        }

        public virtual void Load(string xml)
        {
            lock (this._syncRoot)
            {
                this.LoadXmlToDictionary(xml, this._values);
            }
        }

        public virtual string ToXml(bool includeAppSettings = false)
        {
            XmlDocument doc = new XmlDocument();
            doc.AppendChild(doc.CreateElement("settings"));
            doc.DocumentElement.SetAttribute("type", this.GetType().FullName);
            doc.DocumentElement.SetAttribute("xmlns:xsi", xsiUrl);
            lock (this._syncRoot)
            {
                foreach (KeyValuePair<string, object> kv in this._values)
                {
                    PropertyInfo prop = this.GetType().GetProperty(kv.Key);
                    if (prop != null && (prop.IsDefined(typeof(UserScopedSettingAttribute), true) || includeAppSettings))
                    {
                        XmlElement element = this.SaveValueToElement(prop.Name, prop.PropertyType, kv.Value, doc);
                        doc.DocumentElement.AppendChild(element);
                    }
                }
            }
            return doc.OuterXml;

        }

        private XmlElement SaveValueToElement(string name, Type t, object value, XmlDocument doc)
        {
            if (value == null)
            {
                XmlElement rs = doc.CreateElement(name);
                XmlAttribute nil = doc.CreateAttribute("xsi:nil", xsiUrl);
                nil.Value = "true";
                rs.Attributes.Append(nil);
                return rs;
            }

            if (IsNullableType(t))
            {
                t = Nullable.GetUnderlyingType(t);
            }

            if (Array.IndexOf(PrimitiveTypes, t) >= 0)
            {
                XmlElement rs = doc.CreateElement(name);
                if (t == typeof(XElement))
                {
                    XElement x = (XElement)value;
                    XmlElement child = x.ToXmlElement(doc);
                    rs.AppendChild(child);
                }
                else
                {
                    TypeConverterAttribute a = t.GetCustomAttributes<TypeConverterAttribute>(true).FirstOrDefault();
                    string text;
                    if (a == null)
                    {
                        text = Convert.ToString(value);
                    }
                    else
                    {
                        TypeConverter cvt = (TypeConverter)Activator.CreateInstance(Type.GetType(a.ConverterTypeName));
                        text = cvt.ConvertToString(value);
                    }

                    
                    rs.InnerText = text;
                }
                return rs;

            }
            else
            {
                Type elementType;
                if(IsList(t, out elementType))
                {
                    XmlElement rs = doc.CreateElement(name);
                    XmlSerializer ser = new XmlSerializer(elementType);
                    
                    foreach (object o in (IEnumerable)value)
                    {
                        StringWriter w = new StringWriter ();
                        ser.Serialize(w, o);
                        XmlDocument doc2 = new XmlDocument();
                        doc2.LoadXml(w.GetStringBuilder().ToString());
                        XmlNode child = doc.ImportNode(doc2.DocumentElement, true);
                        rs.AppendChild(child);
                    }
                    return rs;
                }
                else
                {

                    XmlRootAttribute root = new XmlRootAttribute(name);
                    root.Namespace = doc.DocumentElement.NamespaceURI;
                    XmlSerializer ser = new XmlSerializer(t, root);
                    StringWriter w = new StringWriter();
                    ser.Serialize(w, value);

                    XmlDocument tempDoc = new XmlDocument();
                    tempDoc.LoadXml(w.ToString());
                    XmlElement rs = (XmlElement)doc.ImportNode(tempDoc.DocumentElement, true);
                    return rs;
                }
            }

        }

        private static bool IsNullableType(Type t)
        {
            return t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Nullable<>);
        }

        

        protected virtual void OnPropertyChanged(string name)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }


        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        

        public void Save()
        {
            SettingStorage.Save(this);
        }
    }
}
