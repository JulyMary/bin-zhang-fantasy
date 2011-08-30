﻿using System;
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
            this.HasMany(x => x.ChildPackages).CollectionType<ObservableList<BusinessPackage>>().KeyColumn("ParentPackageId").Cascade.None();
            this.HasMany(x => x.Classes).CollectionType<ObservableList<BusinessClass>>().KeyColumn("PackageID").Cascade.None().Inverse();
            this.HasMany(x => x.ClassDiagrams).CollectionType<ObservableList<BusinessClassDiagram>>().KeyColumn("PackageID").Cascade.AllDeleteOrphan().Inverse();
            this.Map(x => x.BuildAsAssembly).Not.Nullable();
        }
    }
}
