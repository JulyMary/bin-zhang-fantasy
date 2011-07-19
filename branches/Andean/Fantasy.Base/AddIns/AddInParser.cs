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

            XamlObjectWriterSettings settings = new XamlObjectWriterSettings();
            settings.XamlSetValueHandler = (sender, e) =>
                {
                    Debug.WriteLine("{0} : {1}", e.Member.Name, e.Value); 
                };
          
            XamlXmlReader xxr = new XamlXmlReader(reader);
            XamlObjectWriter xow = new XamlObjectWriter(xxr.SchemaContext, settings);

            ParseCommon(xxr, xow);

            return (AddIn)xow.Result;

        }


        private void ParseCommon(XamlXmlReader reader, XamlObjectWriter writer)
        {
            while (reader.Read())
            {
                writer.WriteNode(reader); 
                if (reader.NodeType == XamlNodeType.StartObject && reader.Type != null && reader.Type.UnderlyingType == typeof(ObjectBuilder))
                {
                    this.ParseBuilder(reader, writer);
                }
            }
        }

        private void ParseBuilder(XamlXmlReader reader, XamlObjectWriter writer)
        {
            while(reader.Read())
            {
                writer.WriteNode(reader); 
                if (reader.NodeType == XamlNodeType.StartMember)
                {
                    this.ParseBuilderContent(reader);

                }
                else if (reader.NodeType == XamlNodeType.EndObject)
                {
                    return;
                }
                
            }
        }



        private void ParseBuilderContent(XamlXmlReader reader)
        {
            int deep = 0;
            XamlNodeList nodes = new XamlNodeList(reader.SchemaContext);
            XamlWriter writer = nodes.Writer;
            while(reader.Read()) 
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
