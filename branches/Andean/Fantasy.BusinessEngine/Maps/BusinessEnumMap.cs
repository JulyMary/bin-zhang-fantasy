using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.BusinessEngine.Collections;

namespace Fantasy.BusinessEngine.Maps
{
    public class BusinessEnumMap : BusinessEntityMap<BusinessEnum>
    {
        public BusinessEnumMap()
        {
            this.Table("BUSINESSENUM");
            this.Map(x => x.Name).Not.Nullable();
            this.Map(x => x.CodeName).Not.Nullable();
            this.References(x => x.Package).Column("PackageId");
            this.Map(x => x.IsFlags).Not.Nullable();
            this.Map(x => x.IsExternal).Not.Nullable();
            this.Map(x => x.ExternalAssemblyName, "ExternalAssembly");
            this.Map(x => x.ExternalNamespace); 
            this.HasMany<BusinessEnumValue>(x => x.PersistedEnumValues).CollectionType<ObservableList<BusinessEnumValue>>().KeyColumn("ENUMID").Cascade.AllDeleteOrphan().Inverse();
        }
    }
}
