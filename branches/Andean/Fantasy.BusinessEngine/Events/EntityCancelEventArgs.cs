using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fantasy.BusinessEngine.Events
{
    public class EntityCancelEventArgs : EventArgs 
    {
        public EntityCancelEventArgs(IEntity entity)
        {
            this.Entity = entity;
        }
        
        public IEntity Entity { get; private set; }

        public bool Cancel {get;set;}
    }
}
