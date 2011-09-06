﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;

namespace Fantasy.BusinessEngine.Maps
{
    public class BusinessEntityMap<T> :  ClassMap<T> where T : IBusinessEntity
    {
        public BusinessEntityMap()
        {
            this.Id(x => x.Id).GeneratedBy.Assigned();
            this.Map(x => x.ModificationTime).Not.Nullable();
            this.Map(x => x.CreationTime).Not.Nullable();
            this.Map(x => x.IsSystem).Not.Nullable();
        }

        
    }
}