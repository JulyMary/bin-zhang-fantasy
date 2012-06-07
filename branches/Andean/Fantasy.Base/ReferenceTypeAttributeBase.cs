using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fantasy
{
    public class ReferenceTypeAttributeBase : Attribute
    {
        public ReferenceTypeAttributeBase(Type type)
        {
            this.TypeName = type.AssemblyQualifiedName;
            
        }

        public ReferenceTypeAttributeBase(string typeName)
        {
            this.TypeName = typeName;
        }

      

        public string TypeName { get; private set; }

        protected virtual Type ReferencedType
        {
            get
            {
                return Type.GetType(this.TypeName, true);
            }
        }
    }
}
