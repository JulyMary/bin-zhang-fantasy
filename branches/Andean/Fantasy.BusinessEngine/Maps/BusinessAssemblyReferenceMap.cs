using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fantasy.BusinessEngine.Maps
{
    public class BusinessAssemblyReferenceMap : BusinessEntityMap<BusinessAssemblyReference>
    {
        public BusinessAssemblyReferenceMap()
        {
            this.Table("ASSEMBLYREFERENCE");
            this.Map(x => x.FullName).Not.Nullable();
            this.Map(x => x.Source).Not.Nullable();
            this.Map(x => x.PersitedRawAssembly).Column("RAWASSEMBLY").LazyLoad();
            this.Map(x => x.RawHash);
            this.References(x => x.Group).Column("GROUPID").Not.Nullable();
        }
    }
}
