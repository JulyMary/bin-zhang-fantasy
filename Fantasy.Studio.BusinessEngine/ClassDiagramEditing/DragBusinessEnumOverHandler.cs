using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Fantasy.BusinessEngine;

namespace Fantasy.Studio.BusinessEngine.ClassDiagramEditing
{
    public class DragBusinessEnumOverHandler : ObjectWithSite, IEventHandler<DragEventArgs>
    {

        #region IEventHandler<DragEventArgs> Members

        public void HandleEvent(object sender, DragEventArgs e)
        {
            BusinessEnum data = (BusinessEnum)e.Data.GetData(typeof(BusinessEnum));
            if (data != null)
            {
                e.Effects = DragDropEffects.Copy;
                e.Handled = true;
            }
        }

        #endregion
    }
}
