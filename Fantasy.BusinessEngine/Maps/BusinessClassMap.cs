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
            this.HasMany(x => x.PersistedChildClasses).CollectionType<ObservableList<BusinessClass>>().KeyColumn("ParentClassId").Cascade.None();
            this.HasMany(x => x.PersistedProperties).CollectionType<ObservableList<BusinessProperty>>().KeyColumn("ClassId").Cascade.AllDeleteOrphan().Inverse().OrderBy("DISPLAYORDER");
            this.Map(x => x.TableName).Not.Nullable();
            this.Map(x => x.TableSchema).Not.Nullable();
            this.Map(x => x.TableSpace);
            this.Map(x => x.IsSimple).Not.Nullable();
            this.Map(x => x.IsAbstract).Not.Nullable();
            this.Map(x => x.Script).LazyLoad();
            this.Map(x => x.AutoScript).LazyLoad();
            this.Map(x => x.ScriptOptions).CustomType<Int32>() ;
            this.Map(x => x.ExternalType);

            this.HasMany(x => x.PersistedLeftAssociations).CollectionType<ObservableList<BusinessAssociation>>().KeyColumn("LEFTCLASSID").Cascade.AllDeleteOrphan().Inverse();
            this.HasMany(x => x.PersistedRightAssociations).CollectionType<ObservableList<BusinessAssociation>>().KeyColumn("RIGHTCLASSID").Cascade.AllDeleteOrphan().Inverse();
	    }
           
    }
}
