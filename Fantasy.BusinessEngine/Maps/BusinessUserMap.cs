using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.BusinessEngine.Collections;

namespace Fantasy.BusinessEngine.Maps
{
    public class BusinessUserMap : BusinessEntityMap<BusinessUserData>
    {
        public BusinessUserMap()
        {
            this.Table("BUSINESSUSER");
            this.Map(x => x.Name).Not.Nullable();
            this.Map(x => x.FullName).Not.Nullable();
            this.Map(x => x.Password);
            this.Map(x => x.Description);
            this.Map(x => x.IsDisabled);
            this.Map(x => x.CodeName).Not.Nullable();
            this.Map(x => x.Script);
            this.Map(x => x.ExternalType);
            this.Map(x => x.ScriptOptions).CustomType<Int32>();
            this.References(x => x.Package).Column("PackageId");
            this.HasManyToMany<BusinessRoleData>(x => x.PersistedRoles).Table("BUSINESSUSERROLE").CollectionType<ObservableList<BusinessRoleData>>().ParentKeyColumn("UserID").ChildKeyColumn("RoleID").Inverse().Cascade.None();
        }
    }
}
