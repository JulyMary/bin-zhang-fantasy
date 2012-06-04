using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.BusinessEngine.Properties;

namespace Fantasy.BusinessEngine
{
    public class EntityNotFoundException : Exception
    {
        public EntityNotFoundException(Type type, object id)
            : base(String.Format(Resources.EntityNotFoundExceptionMessage, type.Name, id))
        {
            this.EntityType = type;
            this.EntityId = id;
        }

        public Type EntityType { get; private set; }

        public object EntityId { get; private set; }
    }
}
