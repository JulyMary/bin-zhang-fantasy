using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Windows;
using Fantasy.AddIns;

namespace Fantasy.Studio
{
    public class DoDragDropHandler : ObjectWithSite, IEventHandler<DoDragDropEventArgs>
    {

        #region IEventHandler<DoDragDropEventArgs> Members

        public void HandleEvent(object sender, DoDragDropEventArgs e)
        {
            
            e.AllowedEffects = this.AllowedEffects;

            object data = _dataBuilder != null ? _dataBuilder.Build<object>() : this.Data;

            e.Data = data;
        }

        #endregion


        private ObjectBuilder _dataBuilder = null;

        [Template("_dataBuilder")]
        public Object Data { get; set; }
        public DragDropEffects AllowedEffects { get; set; }
    }
}
