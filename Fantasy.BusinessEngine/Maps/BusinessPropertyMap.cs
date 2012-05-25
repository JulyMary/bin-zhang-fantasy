using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fantasy.BusinessEngine.Maps
{
    public class BusinessPropertyMap : BusinessEntityMap<BusinessProperty> 
    {
        public BusinessPropertyMap()
        {
            this.Table("BUSINESSPROPERTY");
            this.Map(x => x.Name).Not.Nullable();
            this.Map(x => x.CodeName).Not.Nullable();
            this.References<BusinessClass>(x => x.Class, "CLASSID").Not.Nullable();
            this.References<BusinessDataType> (x=>x.DataType, "DATATYPEID").Not.Nullable();
            this.References<BusinessClass>(x => x.DataClassType, "CLASSTYPEID");
            this.References<BusinessEnum>(x => x.DataEnumType, "ENUMTYPEID");
            this.Map(x => x.FieldName);
            this.Map(x => x.FieldType);
            this.Map(x => x.Length);
            this.Map(x => x.Precision);
            this.Map(x => x.IsNullable);
            this.Map(x => x.DefaultValue);
            this.Map(x => x.IsCalculated);
            this.Map(x => x.DisplayOrder).Column("DisplayOrder").Not.Nullable();
            this.Map(x => x.ExtensionsData).Column("Extensions").CustomType("StringClob");
          
        }
    }
}
