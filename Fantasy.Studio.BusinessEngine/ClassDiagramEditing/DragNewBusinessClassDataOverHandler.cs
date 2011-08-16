using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Fantasy.Studio.BusinessEngine.ClassDiagramEditing
{
    public class DragNewBusinessClassDataOverHandler : ObjectWithSite, IEventHandler<DragEventArgs>
    {
        #region IEventHandler<DragEventArgs> Members

        public void HandleEvent(object sender, DragEventArgs e)
        {
            DragNewBusinessClassData data = (DragNewBusinessClassData)e.Data.GetData(typeof(DragNewBusinessClassData));
            if (data != null)
            {
                e.Effects = DragDropEffects.Copy;
                e.Handled = true;
            }

        }

        #endregion
    }
}
