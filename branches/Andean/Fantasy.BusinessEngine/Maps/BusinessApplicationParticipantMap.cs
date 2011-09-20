﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fantasy.BusinessEngine.Maps
{
    public class BusinessApplicationParticipantMap : BusinessEntityMap<BusinessApplicationParticipant>
    {
        public BusinessApplicationParticipantMap()
        {
            this.Table("BusinessApplicationParticipant");
            this.Map(x => x.IsEntry);
            this.References(x => x.Class).Column("CLASSID").Not.Nullable().Cascade.SaveUpdate();
            this.References(x => x.Application).Column("APPLICATIONID").Not.Nullable();
        }
    }
}