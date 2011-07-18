using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fantasy.BusinessEngine.Events
{
    public class EntityPropertyChangingEventArgs : EventArgs
    {
        public EntityPropertyChangingEventArgs(IEntity entity,string propertyName, object newValue, object oldValue)
        {
            this.Entity = entity;
            this.PropertyName = propertyName;
            this.NewValue = NewValue;
            this.OldValue = oldValue;
        }

        public IEntity Entity { get; private set; }

        public string PropertyName { get; private set; }

        public object OldValue { get; private set; }

        public object NewValue { get; private set; }

        public bool Cancel { get; set; }
    }
}
