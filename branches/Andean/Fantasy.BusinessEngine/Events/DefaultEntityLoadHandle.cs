using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fantasy.BusinessEngine.Events
{
    public class DefaultEntityLoadHandle : IEntityEventHandler
    {
        #region IEntityEventHandler Members

        public void Execute(EntityEventArgs e)
        {
            e.Entity.OnLoad(EventArgs.Empty);
        }

        #endregion
    }
}
