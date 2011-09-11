using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.BusinessEngine.Collections;

namespace Fantasy.BusinessEngine.Maps
{
    public class BusinessPackageMap : BusinessEntityMap<BusinessPackage>
    {
        public BusinessPackageMap()
        {
            this.Table("BusinessPackage");
            this.Map(x => x.Name).Not.Nullable();
            this.Map(x => x.CodeName).Not.Nullable();
            this.References(x => x.ParentPackage).Column("ParentPackageId");
            this.HasMany(x => x.PersistedChildPackages).CollectionType<ObservableList<BusinessPackage>>().KeyColumn("ParentPackageId").Cascade.None().Inverse();
            this.HasMany(x => x.PersistedClasses).CollectionType<ObservableList<BusinessClass>>().KeyColumn("PackageID").Cascade.None().Inverse();

            this.HasMany(x => x.PersistedClassDiagrams).CollectionType<ObservableList<BusinessClassDiagram>>().KeyColumn("PackageID").Cascade.AllDeleteOrphan().Inverse();
            this.HasMany(x => x.PersistedEnums).CollectionType<ObservableList<BusinessEnum>>().KeyColumn("PackageId").Cascade.None().Inverse();
            this.HasMany(x => x.PersistedAssociations).CollectionType<ObservableList<BusinessAssociation>>().KeyColumn("PackageID").Cascade.None().Inverse();
           
        }
    }
}
