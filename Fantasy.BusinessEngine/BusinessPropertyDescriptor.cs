using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Fantasy.BusinessEngine
{
    public class BusinessPropertyDescriptor
    {

        public BusinessObjectDescriptor Owner { get; internal set; }

        public string Name { get; internal set; }

        public string CodeName { get; internal set; }

        public object Value
        {
            get
            {
                return Invoker.Invoke(Owner.Object, CodeName);
            }
        }


        public Type PropertyType { get; internal set; }

        public bool CanRead { get; internal set; }

        public bool CanWrite { get; internal set; }

        public BusinessObjectMemberTypes MemberType { get; internal set; }


        public bool IsScalar { get; internal set; }


        public BusinessClass ReferencedClass { get; internal set; }

        public BusinessEnum ReferencedEnum { get; internal set; }


        public BusinessProperty Property { get; internal set; }

        public BusinessAssociation Association { get; internal set; }

        public long DisplayOrder { get; internal set; }




    }
}
