using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.BusinessEngine.Collections;

namespace Fantasy.BusinessEngine.Maps
{
    public class BusinessApplicationMap : BusinessEntityMap<BusinessApplication>
    {
        public BusinessApplicationMap()
        {
            this.Map(x => x.Name).Not.Nullable();

            this.Map(x => x.EntryObjectId);

            this.References(x => x.Package).Column("PackageId").Not.Nullable();

            this.HasMany(x=>x.PersistedParticipants).CollectionType<ObservableList<BusinessApplicationParticipant>>().KeyColumn("APPLICATIONID").Cascade.AllDeleteOrphan().Inverse();
        }
    }
}
