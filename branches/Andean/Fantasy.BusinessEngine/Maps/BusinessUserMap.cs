using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.BusinessEngine.Collections;

namespace Fantasy.BusinessEngine.Maps
{
    public class BusinessUserMap : BusinessEntityMap<BusinessUser>
    {
        public BusinessUserMap()
        {
            this.Table("BUSINESSUSER");
            this.Map(x => x.Name).Not.Nullable();
            this.Map(x => x.Password);
            this.Map(x => x.Description);
            this.References(x => x.Package).Column("PackageId");
            this.HasManyToMany<BusinessRole>(x => x.PersistedRoles).Table("BUSINESSUSERROLE").CollectionType<ObservableList<BusinessRole>>().ParentKeyColumn("UserID").ChildKeyColumn("RoleID").Inverse().Cascade.None();
        }
    }
}
