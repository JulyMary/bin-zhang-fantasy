using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Fantasy.BusinessEngine.Events
{
    public class DefaultEntityUpdatingHandler : IEntityCancelEventHandler
    {
        #region IEntityCancelEventHandler Members

        public void Execute(EntityCancelEventArgs e)
        {
            CancelEventArgs args = new CancelEventArgs();
            e.Entity.OnUpdating(args);
            if (args.Cancel)
            {
                e.Cancel = true;
            }
        }

        #endregion
    }
}
