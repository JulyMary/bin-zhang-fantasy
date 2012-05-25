using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fantasy.BusinessEngine.Events
{
    public class EntityCreateEventArgs : EntityEventArgs
    {
        public EntityCreateEventArgs(IEntity entity) 
            : base(entity)
        {

        }

        public object Key { get; set; }
    }
}
