using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.BusinessEngine.Collections;

namespace Fantasy.BusinessEngine.Maps
{
    public class BusinessApplicationMap : BusinessEntityMap<BusinessApplicationData>
    {
        public BusinessApplicationMap()
        {
            this.Table("BUSINESSAPPLICATION");

            this.Map(x => x.Name).Not.Nullable();

            this.Map(x => x.EntryObjectId);

            this.Map(x => x.CodeName).Not.Nullable();

            this.Map(x => x.Script).CustomType("StringClob").LazyLoad();
            this.Map(x => x.ExternalType);
            this.Map(x => x.ScriptOptions).CustomType<Int32>();
            this.Map(x => x.ViewSettings);

            this.References(x => x.Package).Column("PackageId").Not.Nullable();

            this.HasMany(x=>x.PersistedParticipants).CollectionType<ObservableList<BusinessApplicationParticipant>>().KeyColumn("APPLICATIONID").Cascade.AllDeleteOrphan().Inverse();
        }
    }
}
