using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Markup;

namespace Fantasy.AddIns
{
    [ContentProperty("Name")]
    public class Assembly : CodonBase
    {

        public override object BuildItem(object owner, System.Collections.IEnumerable subItems, ConditionCollection condition)
        {
#pragma warning disable 618
            return System.Reflection.Assembly.LoadWithPartialName(this.Name);
#pragma warning restore 618
        }

        public string Name { get; set; }
    }
}
