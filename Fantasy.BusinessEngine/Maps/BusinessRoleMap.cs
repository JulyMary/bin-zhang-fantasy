using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.BusinessEngine.Collections;

namespace Fantasy.BusinessEngine.Maps
{
    public class BusinessRoleMap : BusinessEntityMap<BusinessRole>
    {
        public BusinessRoleMap()
        {
            this.Table("BusinessRole");
            this.Map(x => x.Name);
            this.Map(x => x.Description);
            this.Map(x => x.IsComputed);
            this.References(x => x.Package).Column("PackageId");
            this.HasManyToMany<BusinessUser>(x => x.PersistedUsers).Table("BUSINESSUSERROLE").CollectionType<ObservableList<BusinessUser>>().ParentKeyColumn("ROLEID").ChildKeyColumn("USERID").Cascade.None();
        }
    }
}
