using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xaml;
using System.Diagnostics;


namespace Fantasy.AddIns
{
    public class AddInParser
    {
        public AddIn Parse(XmlReader reader)
        {

            _namespaces.Clear();
            _namespaces.Push(new List<NamespaceDeclaration>());
            XamlXmlReaderSettings settings = new XamlXmlReaderSettings() { ProvideLineInfo = true };
            XamlXmlReader xxr = new XamlXmlReader(reader, settings);
            XamlObjectWriter xow = new XamlObjectWriter(xxr.SchemaContext);
            ParseCommon(xxr, xow);
            //_namespaces.Pop();
            return (AddIn)xow.Result;

        }

        private Stack<List<NamespaceDeclaration>> _namespaces = new Stack<List<NamespaceDeclaration>>();

        private void ParseCommon(XamlXmlReader reader, XamlObjectWriter writer)
        {
            while (Read(reader))
            {
                writer.WriteNode(reader);
                if (reader.NodeType == XamlNodeType.StartObject && reader.Type != null && reader.Type.UnderlyingType.IsSubclassOf(typeof(ObjectBuilder)))
                {
                    this.ParseBuilder(reader, writer);
                }
            }
        }

        private void ParseBuilder(XamlXmlReader reader, XamlObjectWriter writer)
        {
            int deep = 0;
            while (Read(reader))
            {
                
                writer.WriteNode(reader);
                
                if (reader.NodeType == XamlNodeType.StartMember)
                {
                    deep ++;
                    if(reader.Member.Name == "Template")
                    {
                        this.ParseBuilderContent(reader);
                    }

                }
                else if(reader.NodeType == XamlNodeType.EndMember) 
                {
                    deep--;
                }
                else if (reader.NodeType == XamlNodeType.EndObject && deep == 0)
                {
                    return;
                }
                
            }
        }
        int i = 0;
        //int j = 0;
        private  bool Read(XamlXmlReader reader)
        {
            bool rs = reader.Read();
            
            if (rs)
            {
               
                //Debug.WriteLine(reader.NodeType);
                switch (reader.NodeType)
                {
                    case XamlNodeType.EndMember:
                    case XamlNodeType.EndObject:

                        if (this._namespaces.Count > 0)
                        {
                            this._namespaces.Pop();
                        }
                         
                        break;
                    case XamlNodeType.StartMember:
                    case XamlNodeType.StartObject:
                        
                        i++;
                      
                       // Debug.WriteLine("{0} : {1} : {2}", reader.NodeType, reader.NodeType == XamlNodeType.StartMember ? (object)reader.Member.Name : (object)reader.Type.UnderlyingType, i);
                        this._namespaces.Push(new List<NamespaceDeclaration>());
                       
                        break;
                    case XamlNodeType.NamespaceDeclaration:
                        List<NamespaceDeclaration> frame = this._namespaces.Peek();
                        frame.Add(new NamespaceDeclaration(reader.Namespace.Namespace, reader.Namespace.Prefix));
                        break;
                   default:
                        //Debug.WriteLine(reader.NodeType);
                        break;
                    
                }
            }
            return rs;
        }



        private void ParseBuilderContent(XamlXmlReader reader)
        {
            int deep = 0;
            XamlNodeList nodes = new XamlNodeList(reader.SchemaContext);
            XamlWriter writer = nodes.Writer;

            var namespaces = from frame in this._namespaces.ToArray().Reverse()
                        from ns in frame
                        select ns;
            foreach (NamespaceDeclaration ns in namespaces)
            {
                writer.WriteNamespace(ns);
            }

            while(Read(reader)) 
            {
                if (reader.NodeType == XamlNodeType.StartObject)
                {
                    deep++;
                }
                else if (reader.NodeType == XamlNodeType.EndObject)
                {
                    deep--;
                }
                writer.WriteNode(reader);


                if (deep == 0)
                {
                    break;
                }

            }

            writer.Close();

            ObjectBuilder.CurrentInstance.XamlNodes = nodes;
            ObjectBuilder.CurrentInstance = null;
        }

       
    }
}
