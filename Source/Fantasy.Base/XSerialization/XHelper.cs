using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.ComponentModel;
using Fantasy.Properties;
using System.Xml.Linq;
using System.Xml;

namespace Fantasy.XSerialization
{
    public class XHelper
    {
        private  XHelper ()
	    {
            lock (_syncRoot)
            {
                foreach (Assembly asm in AppDomain.CurrentDomain.GetAssemblies())
                {
                    this.LoadXConverters(asm);
                }
                AppDomain.CurrentDomain.AssemblyLoad += new AssemblyLoadEventHandler(CurrentDomain_AssemblyLoad);
            }
	    }

        void CurrentDomain_AssemblyLoad(object sender, AssemblyLoadEventArgs args)
        {
            lock (_syncRoot)
            {
                this.LoadXConverters(args.LoadedAssembly);
            }
        }

        private void LoadXConverters(Assembly assembly)
        {
            var query = from type in assembly.GetTypes() where type.IsDefined(typeof(XConverterAttribute), false) select type;
            foreach (Type t in query)
            {
                XConverterAttribute attr = (XConverterAttribute)t.GetCustomAttributes(typeof(XConverterAttribute), false)[0];
                this._converters.Add(attr.TargetType, t);
            }
        }

        object _syncRoot = new object();
        

        private void LoadXPrefixes(Assembly assembly)
        {
            foreach(XPrefixAttribute attribute in assembly.GetCustomAttributes(typeof(XPrefixAttribute), true)) 
            {
                this._prefixes[attribute.Namesapce] = attribute.Prefix;
            }
        }

        private Dictionary<string, string> _prefixes = new Dictionary<string, string>();

        private Dictionary<Type, XTypeMap> _maps = new Dictionary<Type, XTypeMap>();
        private Dictionary<Type, Type> _converters = new Dictionary<Type, Type>();


        internal XTypeMap GetTypeMap(Type t)
        {
            lock (_syncRoot)
            {
                XTypeMap rs;
                if (this._maps.TryGetValue(t, out rs))
                {
                    return rs;

                }

                else if (t.IsDefined(typeof(XSerializableAttribute), false))
                {
                    rs = new XTypeMap(t);
                    this._maps.Add(t, rs);
                    return rs;
                }
                else
                {

                    throw new XException(String.Format(Resources.TypeIsNotXSerializableText, t));
                }
            }
        }


        public void LoadByXAttributes(IServiceProvider context, XElement element, object instance)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element"); 
            }
            if (instance == null)
            {
                throw new ArgumentNullException("instance"); 
            }
            XTypeMap map = this.GetTypeMap(instance.GetType() );
            map.LoadByXAttributes(context, element, instance);
        }

        public void SaveByXAttributes(IServiceProvider context, XElement element, object instance)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }
            if (instance == null)
            {
                throw new ArgumentNullException("instance");
            }
            XTypeMap map = this.GetTypeMap(instance.GetType());
            map.SaveByXAttributes(context, element, instance);
        }


        private bool IsNullableType(Type theType)
        {
            return (theType.IsGenericType && theType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)));
        }

        private NullableConverter _nullableConvert = new NullableConverter();

        public TypeConverter CreateXConverter(Type t)
        {
            if (IsNullableType(t))
            {
                return _nullableConvert;
            }

            Type converterType = null;
            do
            {
                this._converters.TryGetValue(t, out converterType);
                t = t.BaseType;
            } while (converterType == null);

            return (TypeConverter)Activator.CreateInstance(converterType); 
        }

        private static XHelper _default = null;

        public static XHelper Default
        {
            get
            {
                if (_default == null)
                {
                    _default = new XHelper();
                }
                return _default;
            }
        }

        public static XmlNamespaceManager CreateXmlNamespaceManager(XElement element, bool includeAncestor)
        {
            XmlNamespaceManager rs = new XmlNamespaceManager(new NameTable());
            List<string> added = new List<string>();
            do
            {
                foreach (XAttribute attr in element.Attributes().Where(a => a.IsNamespaceDeclaration))
                {

                    string prefix = null;

                    if (attr.Name.LocalName == "xmlns" && attr.Name.NamespaceName == "")
                    {
                        prefix = "";

                    }
                    else if (attr.Name.Namespace == XNamespace.Xmlns)
                    {
                        prefix = attr.Name.LocalName;
                    }

                    if (prefix != null && added.IndexOf(prefix) < 0)
                    {
                        added.Add(prefix);
                        rs.AddNamespace(prefix, attr.Value);
                    }

                }

                element = element.Parent;
            } while (element != null && includeAncestor);
            return rs;
        }

        public static void AddNamespace(XmlNamespaceManager mngr, XElement element)
        {
            foreach (string prefix in mngr)
            {
                XNamespace ns = mngr.LookupNamespace(prefix);
                if (prefix == string.Empty)
                {
                    element.Add(new XAttribute("xmlns", ns));
                }

                else if (ns != XNamespace.Xmlns && ns != XNamespace.Xml)
                {
                    element.Add(new XAttribute(XNamespace.Xmlns + prefix, mngr.LookupNamespace(prefix)));
                }
            }
        }

    }
}
