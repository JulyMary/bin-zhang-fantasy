using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fantasy.BusinessEngine.Events
{
    public interface IEntityCancelEventHandler
    {
        void Execute(EntityCancelEventArgs e);
    }
}
