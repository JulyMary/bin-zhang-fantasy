using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fantasy.BusinessEngine.Events
{
    public class DefaultEntityInsertedHandler : IEntityEventHandler
    {
        #region IEntityEventHandler Members

        public void Execute(EntityEventArgs e)
        {
            e.Entity.OnInserted(EventArgs.Empty);
        }

        #endregion
    }
}
