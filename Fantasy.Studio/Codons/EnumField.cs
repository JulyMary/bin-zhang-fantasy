using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.AddIns;
using System.Collections;

namespace Fantasy.Studio.Codons
{
    public class EnumField : CodonBase
    {

        public override object BuildItem(object owner, IEnumerable subItems, ConditionCollection conditions, IServiceProvider services)
        {
            return this;
        }

        public string Name { get; set; }

        public string Caption { get; set; }

    }

    public class EnumFieldCodonException : Exception
    {
        public EnumFieldCodonException(Type t, string field)
            : base(t.FullName + " has not field which named " + field)
        {
        }
    }
}
