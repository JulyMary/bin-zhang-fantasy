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


            XamlXmlReaderSettings settings = new XamlXmlReaderSettings() { ProvideLineInfo = true };
            XamlXmlReader xxr = new XamlXmlReader(reader, settings);
            XamlObjectWriter xow = new XamlObjectWriter(xxr.SchemaContext);

            ParseCommon(xxr, xow);

            return (AddIn)xow.Result;

        }


        private void ParseCommon(XamlXmlReader reader, XamlObjectWriter writer)
        {
            while (reader.Read())
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
            while(reader.Read())
            {
                int deep = 0;
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
