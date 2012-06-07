using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fantasy.ComponentModel
{
    public class EditorAttribute : ReferenceTypeAttributeBase
    {

        public EditorAttribute(Type type) 
            : base(type)
        {

          
        }

        public EditorAttribute(string typeName)
            : base(typeName)
        {

           
        }

      

        public Type EditorType
        {
            get
            {
                return this.ReferencedType;
            }
        }
    }
}
