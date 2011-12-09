using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.BusinessEngine.Collections;

namespace Fantasy.BusinessEngine.Maps
{
    public class BusinessWebFolderMap : BusinessEntityMap<BusinessWebFolder>
    {
        public BusinessWebFolderMap()
        {
            this.Table("BUSINESSWEBFOLDER");
            this.Map(x => x.Name).Not.Nullable();
            this.Map(x => x.CodeName).Not.Nullable();
            this.References(x => x.Package).Column("PACKAGEID");
            this.References(x => x.ParentFolder).Column("PARENTFOLDERID");

            this.HasMany(x => x.PersistedChildFolders).CollectionType<ObservableList<BusinessWebFolder>>().KeyColumn("PARENTFOLDERID").Cascade.AllDeleteOrphan().Inverse().LazyLoad();

            this.HasMany(x => x.PersistedScripts).CollectionType<ObservableList<BusinessWebScript>>().KeyColumn("WebFolderID").Cascade.None().Inverse().LazyLoad();
        }
    }
}
