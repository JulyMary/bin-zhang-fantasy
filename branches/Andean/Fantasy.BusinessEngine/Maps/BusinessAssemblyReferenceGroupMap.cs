using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.BusinessEngine.Collections;

namespace Fantasy.BusinessEngine.Maps
{
    public class BusinessAssemblyReferenceGroupMap : BusinessEntityMap<BusinessAssemblyReferenceGroup>
    {
        public BusinessAssemblyReferenceGroupMap()
        {
            this.Table("ASSEMBLYREFERENCEGROUP");
            this.HasMany(x => x.PersistedReferences).CollectionType<ObservableList<BusinessAssemblyReference>>().KeyColumn("GROUPID").Cascade.AllDeleteOrphan().Inverse();
        }


       
    }
}
