using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Fantasy.Studio.BusinessEngine.ClassDiagramEditing
{
    public class DragNewBusinessEnumDataOverHandler : ObjectWithSite, IEventHandler<DragEventArgs>
    {
        #region IEventHandler<DragEventArgs> Members

        public void HandleEvent(object sender, DragEventArgs e)
        {
            DragNewBusinessEnumData data = (DragNewBusinessEnumData)e.Data.GetData(typeof(DragNewBusinessEnumData));
            if (data != null)
            {
                e.Effects = DragDropEffects.Copy;
                e.Handled = true;
            }

        }

        #endregion
    }
}
