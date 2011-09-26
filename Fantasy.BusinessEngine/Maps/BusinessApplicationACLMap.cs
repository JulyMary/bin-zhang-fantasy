using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fantasy.BusinessEngine.Maps
{
    public class BusinessApplicationACLMap : BusinessEntityMap<BusinessApplicationACL>
    {
        public BusinessApplicationACLMap()
        {
            this.Table("BUSINESSAPPLICATIONACL");
            this.References(x => x.Role).Column("ROLEID").Not.Nullable().Cascade.SaveUpdate();
            this.References(x => x.State).Column("STATEID");
            this.References(x => x.Participant).Column("PARTICIPANTID").Not.Nullable();
            this.Map(x => x.PersistedACL).Column("ACL");
        }
    }
}
