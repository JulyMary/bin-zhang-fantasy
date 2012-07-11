using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.IO;
using System.Security.Cryptography;
using Fantasy.LicenceSigner.Properties;

namespace Fantasy.LicenceSigner
{
    class Program
    {
        static void Main(string[] args)
        {
            XNamespace ns = "urn:schemas-fantasy-com:licensing";
            SHA1CryptoServiceProvider sha = new SHA1CryptoServiceProvider();
            DSACryptoServiceProvider dsa = new DSACryptoServiceProvider();
            dsa.FromXmlString(Resources.FantasyPtyKey);

            DSASignatureFormatter formatter = new DSASignatureFormatter(dsa);
            formatter.SetHashAlgorithm("SHA1");

            foreach (string f in args)
            {
                string output = Path.ChangeExtension(f, "lic");

                XElement content = XElement.Load(f);
                XElement licence = new XElement(ns + "licence");
                licence.Add(content);

                string text = GetContentString(licence);
                byte[] tb = Encoding.Unicode.GetBytes(text);

                byte[] hash = sha.ComputeHash(tb);

                byte[] sb = formatter.CreateSignature(hash);

                string sig = Convert.ToBase64String(sb);

                licence.SetAttributeValue("signature", sig);

                licence.Save(output, SaveOptions.OmitDuplicateNamespaces);
            }
        }

        private static string GetContentString(XElement element)
        {
            StringBuilder rs = new StringBuilder();
            foreach (XElement child in element.Descendants())
            {
                foreach (XAttribute attr in child.Attributes())
                {
                    if (!attr.IsNamespaceDeclaration)
                    {
                        rs.Append(attr.Name.ToString() + attr.Value.Trim());
                    }
                }
                if (!child.HasElements)
                {
                    rs.Append(child.Name.ToString() + child.Value.Trim());
                }
            }
            return rs.ToString();
        }
    }
}
