using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fantasy.BusinessEngine.Events
{
    public class EntityEventArgs : EventArgs
    {
        public EntityEventArgs(IEntity entity)
        {
            this.Entity = entity;
        }
        
        public IEntity Entity { get; private set; }
    }
}
