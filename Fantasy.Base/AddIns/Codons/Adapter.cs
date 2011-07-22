using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.Adaption;
using System.Windows.Markup;

namespace Fantasy.AddIns.Codons
{
    [ContentProperty("Adaptees")]
    public class Adapter : CodonBase
    {
       

        public override object BuildItem(object owner, System.Collections.IEnumerable subItems, ConditionCollection condition)
        {
            return this;
        }
        private List<Type> _adaptees = new List<Type>();
        public IList<Type> Adaptees { get { return _adaptees; } }


        public IAdapterFactory Factory { get; set; }
    }
}
