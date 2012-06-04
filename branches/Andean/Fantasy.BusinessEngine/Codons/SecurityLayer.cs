using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.AddIns;
using Fantasy.BusinessEngine.Security;

namespace Fantasy.BusinessEngine.Codons
{
    public class SecurityLayer : CodonBase
    {

        public override object BuildItem(object owner, System.Collections.IEnumerable subItems, ConditionCollection condition, IServiceProvider services)
        {
            return new Fantasy.BusinessEngine.Security._SecurityLayer(subItems.Cast<ISecurityProvider>());
        }
    }
}
