using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Fantasy.Jobs.Web.WSXGenerator.Properties;
using System.IO;

namespace Fantasy.Jobs.Web.WSXGenerator
{
    class WSXGenerator
    {
        static int Main(string[] args)
        {

            int rs = 0;
            string inputFile = CommandArgumentsHelper.GetValue("i");
            string outputFile = CommandArgumentsHelper.GetValue("o");
            string profile = CommandArgumentsHelper.GetValue("p");


            try
            {
                new WSXGenerator().Run(inputFile, outputFile, profile); 
            }
            catch
            {
                rs = 1;
            }

            return rs;

        }

  

        public void Run(string inputFile, string outputFile, string profileName)
        {
            XElement input = XElement.Load(inputFile);
            XElement profile = input.Elements("publishProfile").Where(x => String.Equals((string)x.Attribute("profileName"), profileName, StringComparison.OrdinalIgnoreCase)).Single();
            XElement output = XElement.Parse(Resources.Template);
            XNamespace ns = output.Name.Namespace;
            XElement rootDirectory = output.Descendants(ns + "DirectoryRef").Where(x => String.Equals((string)x.Attribute("Id"), "FantasyJobWeb", StringComparison.OrdinalIgnoreCase)).Single();
            string publishPath=(string)profile.Attribute("publishUrl");

            
            foreach (XElement src in profile.Elements("file"))
            {
                string srcPath = ((string)src.Attribute("relUrl")).Replace('/', '\\');
                string fileName = Path.GetFileName(srcPath);
                string srcDir = Path.GetDirectoryName(srcPath);

                XElement component = GetComponentElement(srcDir, rootDirectory);

                XElement file = new XElement(ns + "File");
                file.SetAttributeValue("Id", (string)component.Parent.Attribute("Id") + "_" + fileName);
                file.SetAttributeValue("Source", Path.Combine(publishPath, srcPath));
                file.SetAttributeValue("Name", fileName);
                component.Add(file);
            }


            XElement feature = output.Descendants(ns + "FeatureRef").Where(x => String.Equals((string)x.Attribute("Id"), "FantasyJobWeb", StringComparison.OrdinalIgnoreCase)).Single();
            foreach (XElement component in rootDirectory.Descendants(ns + "Component").ToArray())
            {
                XElement componentRef = new XElement(ns + "ComponentRef");
                componentRef.SetAttributeValue("Id", (string)component.Attribute("Id"));
                feature.Add(componentRef); 
            }

            output.Save(outputFile, SaveOptions.OmitDuplicateNamespaces); 

        }

        private XElement GetComponentElement(string path, XElement rootDirectory)
        {
            XNamespace ns = rootDirectory.Name.Namespace;
            XElement dir = this.GetDirectoryElement(path, rootDirectory);
            XElement rs = dir.Element(ns + "Component");
            if (rs == null)
            {
                rs = new XElement(ns + "Component");
                rs.SetAttributeValue("Guid", Guid.NewGuid().ToString("B"));
                rs.SetAttributeValue("Id", (string)dir.Attribute("Id") + "_CMP");
                dir.Add(rs);
            }
            return rs;
 
        }

        private XElement GetDirectoryElement(string path, XElement rootDirectory)
        {
            XNamespace ns = rootDirectory.Name.Namespace;
            XElement rs = rootDirectory;
            XElement parent;
            foreach (string name in path.Split(new char[] {'/', '\\'}, StringSplitOptions.RemoveEmptyEntries))
            {
                parent = rs;
                rs = rs.Elements(ns + "Directory").Where(x=>(string)x.Attribute("Name") == name).SingleOrDefault();
                if(rs == null)
                {
                    rs = new XElement(ns + "Directory");
                    rs.SetAttributeValue("Name", name);
                    rs.SetAttributeValue("Id", Guid.NewGuid().ToString("N"));
                    parent.Add(rs);
                   
                }
            }
            return rs;
        }
    }
}
