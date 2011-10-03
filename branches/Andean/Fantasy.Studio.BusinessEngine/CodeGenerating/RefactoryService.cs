using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.ServiceModel;
using ICSharpCode.NRefactory.CSharp;
using System.IO;
using ICSharpCode.NRefactory.PatternMatching;

namespace Fantasy.Studio.BusinessEngine.CodeGenerating
{

    public class RefactoryService : ServiceBase, IRefactoryService
    {
        public string RenameNamespace(string content, string newNamespace)
        {
            string[] segments = newNamespace.Split('.'); 
            CompilationUnit compilationUnit = Parse(content);
            //INode[] nodes = (from node in compilationUnit.Flatten<INode>(n => n.ChildNodes()) where node is NamespaceDeclaration || node is UsingDeclaration select node).ToArray();

            //foreach (UsingDeclaration @using in nodes.OfType<UsingDeclaration>().Where(u=>u.Namespace.StartsWith(oldNamespace)))
            //{
            //    AstType[] astTypes = @using.Import.Flatten(t => (t is MemberType) ? ((MemberType)t).Target : null).Reverse().ToArray();
            //    for (int i = 0; i < segments.Length; i++)
            //    {
            //        if (astTypes[i] is MemberType)
            //        {
            //            ((MemberType)astTypes[i]).MemberName = segments[i];
            //        }
            //        else
            //        {
            //            ((SimpleType)astTypes[i]).Identifier = segments[i];
            //        }
            //    }

            //}


            NamespaceDeclaration nd = compilationUnit.Flatten<INode>(n => n.ChildNodes()).OfType<NamespaceDeclaration>().FirstOrDefault();

            if(nd != null)
            {
                nd.Name = newNamespace;
            }
            

            return GetContent(compilationUnit);

           
        }

        private string GetContent(CompilationUnit compilationUnit)
        {
            StringWriter rs = new StringWriter();
            CSharpOutputVisitor output = new CSharpOutputVisitor(rs, new CSharpFormattingOptions());
            compilationUnit.AcceptVisitor(output, null);
            return rs.ToString();
        }

        private CompilationUnit Parse(string content)
        {
            CSharpParser parser = new CSharpParser();
            CompilationUnit rs = parser.Parse(new StringReader(content));
            return rs;
        }

        public string RenameClass(string content, string newClassName)
        {
            CompilationUnit compilationUnit = Parse(content);
            TypeDeclaration td = compilationUnit.Flatten<INode>(n => n.ChildNodes()).OfType<TypeDeclaration>().FirstOrDefault();
            if (td != null)
            {
                td.Name = newClassName;
                return GetContent(compilationUnit);
            }
            else
            {
                return content;
            }

        }

     
    }

}
