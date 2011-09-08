using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fantasy.Studio.BusinessEngine.EnumEditing
{
    public class EnumNode
    {
        public EnumNode(Type type)
        {
            this.Type = type;
        }

        public Type Type { get; private set; }

        public Array Values
        {
            get
            {
                return Enum.GetValues(this.Type);
            }
        }


        public string Name
        {
            get
            {
                return this.Type.Name;
            }
        }
    }
}
