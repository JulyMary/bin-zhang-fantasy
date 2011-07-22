using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xaml;
using System.Diagnostics;
using System.Reflection;

using Fantasy.IO;


namespace Fantasy.AddIns
{
    public class AddInParser
    {
        public AddIn Parse(XmlReader reader)
        {
            XamlXmlReaderSettings readerSettings = new XamlXmlReaderSettings() { ProvideLineInfo = true };
            string baseDir = LongPath.GetDirectoryName(reader.BaseURI);
            XamlXmlReader xxr = new XamlXmlReader(reader, readerSettings);
            XamlObjectWriterSettings writerSettings = new XamlObjectWriterSettings()
            {
                AfterPropertiesHandler = new EventHandler<XamlObjectEventArgs>((o, e) =>
                {
                    object obj = _currentInstance.Pop();
                    if (obj is Assembly)
                    {
                        Assembly asm = (Assembly)obj;
                        if (!string.IsNullOrEmpty(asm.Name))
                        {
#pragma warning disable 0618
                            System.Reflection.Assembly.LoadWithPartialName(asm.Name);
#pragma warning restore 0618
                        }
                        else if (!string.IsNullOrEmpty(asm.Path))
                        {
                            string file = LongPath.Combine(baseDir, asm.Path);
                            System.Reflection.Assembly.LoadFrom(file);
                        }

                    }
                }),
                BeforePropertiesHandler = new EventHandler<XamlObjectEventArgs>((o, e) =>
                {
                    _currentInstance.Push(e.Instance); 
                })
            };

            XamlObjectWriter xow = new XamlObjectWriter(xxr.SchemaContext, writerSettings );

            ParseObject(xxr, xow);
           
            return (AddIn)xow.Result;

        }

        private Stack<List<NamespaceDeclaration>> _namespaces = new Stack<List<NamespaceDeclaration>>();

        private Stack<object> _currentInstance = new Stack<object>();

        private void ParseSub(XamlReader reader, XamlObjectWriter writer)
        {
            Read(reader);
           
            Write(reader, writer);
            ParseObject(reader, writer);
            while (Read(reader))
            {
                Write(reader, writer);
            }
        }

        private static void Write(XamlReader reader, XamlObjectWriter writer)
        {
            if (reader.NodeType == XamlNodeType.StartObject)
            {
                Debug.WriteLine(String.Format("{0} : {1} ", reader.NodeType, reader.Type.UnderlyingType));
            }
            else if (reader.NodeType == XamlNodeType.StartMember)
            {

                Debug.WriteLine(String.Format("{0} : {1}", reader.NodeType, reader.Member.Name));
            }
            else if (reader.NodeType == XamlNodeType.Value)
            {
                Debug.WriteLine(String.Format("{0} : {1}", reader.NodeType, reader.Value));
            }
            else
            {
                Debug.WriteLine(reader.NodeType);
            }
            writer.WriteNode(reader);
        }

        private void ParseObject(XamlReader reader, XamlObjectWriter writer)
        {
            _namespaces.Push(new List<NamespaceDeclaration>());
            reader.Read();
           
            while (!reader.IsEof)
            {
               
               
                if (reader.NodeType == XamlNodeType.StartObject || reader.NodeType == XamlNodeType.GetObject)
                {
                    XamlReader subreader = reader.ReadSubtree();
                    ParseSub(subreader, writer);

                }
                else if (reader.NodeType == XamlNodeType.StartMember)
                {

                    TemplateAttribute tempAttr = null;
                    if (reader.Member.UnderlyingMember != null)
                    {
                        tempAttr = reader.Member.UnderlyingMember.GetCustomAttributes<TemplateAttribute>(true).FirstOrDefault();
                    }

                    XamlReader subreader = reader.ReadSubtree();

                    if (tempAttr != null)
                    {
                        ParseBuilderMember(subreader, writer, tempAttr);
                    }
                    else
                    {
                        ParseSub(subreader, writer);
                    }
                }
                else
                {
                    if (reader.NodeType == XamlNodeType.NamespaceDeclaration)
                    {
                        List<NamespaceDeclaration> frame = this._namespaces.Peek();
                        frame.Add(new NamespaceDeclaration(reader.Namespace.Namespace, reader.Namespace.Prefix));
                      
                   
                    }
                    Write(reader, writer);
                    reader.Read();
                }
            }
            _namespaces.Pop();
        }

        private void ParseBuilderMember(XamlReader reader, XamlObjectWriter writer, TemplateAttribute tempAttr)
        {
            Read(reader);

            Write(reader, writer);
            Read(reader);
            if (reader.NodeType == XamlNodeType.StartObject || reader.NodeType == XamlNodeType.GetObject)
            {
                XamlReader subreader = reader.ReadSubtree();

                ParseBuilder(subreader, tempAttr);
            }
            while (!reader.IsEof)
            {
                Write(reader, writer);
                Read(reader);
            }
        }

        private void ParseBuilder(XamlReader reader, TemplateAttribute tempAttr)
        {
           
            XamlNodeList nodes = new XamlNodeList(reader.SchemaContext);
            XamlWriter writer = nodes.Writer;

            var namespaces = from frame in this._namespaces.ToArray().Reverse()
                             from ns in frame
                             select ns;
            foreach (NamespaceDeclaration ns in namespaces)
            {
                writer.WriteNamespace(ns);
            }

            while (Read(reader))
            {
                writer.WriteNode(reader);
            }

            writer.Close();

            ObjectBuilder builder = new ObjectBuilder(nodes);

            object current = _currentInstance.Peek();
 
            Type t = current.GetType();

            MemberInfo mi = t.GetMember(tempAttr.BuildMember, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.GetProperty | BindingFlags.SetProperty | BindingFlags.GetField | BindingFlags.SetField | BindingFlags.Instance).First();
            if (mi.MemberType == MemberTypes.Field)
            {
                ((FieldInfo)mi).SetValue(current, builder);
            }
            else
            {
                ((PropertyInfo)mi).SetValue(current, builder, null); 
            }
        }

        private bool Read(XamlReader reader)
        {
            bool rs = reader.Read();

            return rs;
        }

    }
}
