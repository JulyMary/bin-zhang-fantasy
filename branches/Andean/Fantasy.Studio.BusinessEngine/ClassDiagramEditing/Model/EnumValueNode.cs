using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.BusinessEngine;

namespace Fantasy.Studio.BusinessEngine.ClassDiagramEditing.Model
{
    public class EnumValueNode
    {
        public EnumValueNode(BusinessEnumValue enumValue)
        {
            this.Entity = enumValue;
        }

        public BusinessEnumValue Entity { get; private set; }

    }
}
