using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Fantasy.BusinessEngine.Events
{
    public class DefaultEntityInsertingHandler : IEntityCancelEventHandler
    {
        public void Execute(EntityCancelEventArgs e)
        {
            CancelEventArgs args = new CancelEventArgs();
            e.Entity.OnInserting(args);
            if (args.Cancel)
            {
                e.Cancel = true;
            }
        }
       
    }
}
