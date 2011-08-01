using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.BusinessEngine.Collections;

namespace Fantasy.BusinessEngine.Maps
{
    public class BusinessClassMap : BusinessEntityMap<BusinessClass>
    {
        public BusinessClassMap ()
	    {
            this.Table("BusinessClass");
            this.Map(x => x.Name).Not.Nullable();
            this.Map(x => x.CodeName).Not.Nullable();
            this.References(x => x.Package).Column("PackageId");
            this.References(x => x.ParentClass).Column("ParentClassId");
            this.HasMany(x => x.ChildClasses).CollectionType<ObservableList<BusinessClass>>().KeyColumn("ParentClassId").Cascade.None();
            this.HasMany(x => x.Properties).CollectionType<ObservableList<BusinessProperty>>().KeyColumn("ClassId").Cascade.AllDeleteOrphan().Inverse();

            this.Map(x => x.TableName).Not.Nullable();
            this.Map(x => x.TableSchema).Not.Nullable();
            this.Map(x => x.TableSpace);
            this.Map(x => x.IsSimple).Not.Nullable();
	    }
           
    }
}
