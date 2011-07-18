using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.AddIns;
using Fantasy.ServiceModel;

namespace Fantasy.Studio.Workbench.Layout
{
    public class LoadAvalonLayoutCommand : ICommand
    {

        #region ICommand Members

        public object Execute(object caller)
        {
            IWorkbench wb = ServiceManager.Services.GetRequiredService<IWorkbench>();
            AvalonLayout layout = new AvalonLayout();
            wb.Layout = layout;

            return null;
        }

        #endregion
    }
}
