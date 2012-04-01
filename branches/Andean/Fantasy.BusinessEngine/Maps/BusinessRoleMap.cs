using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.BusinessEngine.Collections;

namespace Fantasy.BusinessEngine.Maps
{
    public class BusinessRoleMap : BusinessEntityMap<BusinessRoleData>
    {
        public BusinessRoleMap()
        {
            this.Table("BusinessRole");
            this.Map(x => x.Name);
            this.Map(x => x.Description);
            this.Map(x => x.IsComputed);
            this.Map(x => x.IsDisabled);
            this.Map(x => x.CodeName).Not.Nullable();
            this.Map(x => x.Script);
            this.Map(x => x.ScriptOptions).CustomType<Int32>();
            this.References(x => x.Package).Column("PackageId");
            this.HasManyToMany<BusinessUserData>(x => x.PersistedUsers).Table("BUSINESSUSERROLE").CollectionType<ObservableList<BusinessUserData>>().ParentKeyColumn("ROLEID").ChildKeyColumn("USERID").Cascade.None();
        }
    }
}
