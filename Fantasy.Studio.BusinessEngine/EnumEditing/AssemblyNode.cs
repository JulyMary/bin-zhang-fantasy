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
        public AssemblyNode(Assembly assembly, AssemblyLoader loader)
        {
            this.Assembly = assembly;
            this._loader = loader;

        }

        private AssemblyLoader _loader;

        public Assembly Assembly { get; private set; }


        public string Name
        {
            get
            {
                return this.Assembly.GetName().Name;
            }
        }


        private NamespaceNode[] _namespaces = null;
        public NamespaceNode[] Namespaces
        {
            get
            {
                if (_namespaces == null)
                {
                    var query = from type in this._loader.ReflectionOnlyGetTypes(this.Assembly)
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
