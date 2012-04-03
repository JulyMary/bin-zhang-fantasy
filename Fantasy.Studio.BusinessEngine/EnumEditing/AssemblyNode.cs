using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Fantasy.Reflection;

namespace Fantasy.Studio.BusinessEngine.EnumEditing
{
    public class AssemblyNode
    {
        public AssemblyNode(AssemblyName assemblyRef, AssemblyLoader loader)
        {
            this.AssemblyRef = assemblyRef;
            this._loader = loader;

        }

        private AssemblyLoader _loader;

        public AssemblyName AssemblyRef { get; private set; }


        public string Name
        {
            get
            {
                return AssemblyRef.Name;
            }
        }


        private NamespaceNode[] _namespaces = null;
        public NamespaceNode[] Namespaces
        {
            get
            {
                if (_namespaces == null)
                {
                    var query = from type in this._loader.GetTypes(this.AssemblyRef)
                                where type.IsEnum
                                group type by type.Namespace into g
                                orderby g.Key
                                select new NamespaceNode(g.Key, g.ToArray());

                    this._namespaces = query.ToArray();

                }
                return _namespaces;
            }
        }

    }
}
