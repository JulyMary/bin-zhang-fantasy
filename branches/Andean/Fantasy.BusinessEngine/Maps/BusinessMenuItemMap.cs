using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.BusinessEngine.Collections;

namespace Fantasy.BusinessEngine.Maps
{
    public class BusinessMenuItemMap : BusinessEntityMap<BusinessMenuItem>
    {
        public BusinessMenuItemMap()
        {
            this.Table("BusinessMenuItem");
            this.Map(x => x.Name).Not.Nullable();
            this.Map(x => x.EntryObjectId);
            this.Map(x => x.ApplicationId);
            this.References(x => x.Parent).Column("ParentId");
            this.Map(x => x.PersistedIcon).CustomType("BinaryBlob").Column("Icon");
            this.Map(x => x.ExternalUrl);
            this.Map(x => x.PersistedRoles).Column("Roles");
            this.Map(x => x.IsEnabled);
            this.Map(x => x.Description);
            this.HasMany(x => x.PersistedChildItems).CollectionType<ObservableList<BusinessMenuItem>>().KeyColumn("ParentId").Cascade.AllDeleteOrphan().Inverse().OrderBy("DISPLAYORDER");


        }
    }
}
