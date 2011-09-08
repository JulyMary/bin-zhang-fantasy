using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fantasy.Studio.BusinessEngine.EnumEditing
{
    public class NamespaceNode
    {
        public NamespaceNode(string name, Type[] enumTypes)
        {
            this.Name = name;

            var query = from type in enumTypes orderby type.Name select new EnumNode(type);
            this.Enums = query.ToArray();

        }

        public string Name { get; private set; }

        public EnumNode[] Enums { get; private set; }
    }
}
