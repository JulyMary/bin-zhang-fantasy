using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Syncfusion.Windows.Shared.Controls
{
    public delegate void DragDropEventHandler(object sender, DragDropEventArgs args);

    public class DragDropEventArgs : EventArgs
    {
        public object PayLoad { get; set; }

        public UIElement DragIcon { get; set; }

        public string DropDescription { get; set; }

        public object DragSource { get; set; }

        public object DropTarget { get; set; }

        public bool IsDragArrowEnabled { get; set; }

        public Status Status { get; set; }

        public Key DragKey { get; internal set; }

        public object OriginalSource { get; internal set; }

        public MouseEventArgs MouseEventArgs { get; internal set; }

     }

    public enum Status
    {
        Cancel,

        Impossible,

        DragStarted,

        DragInProgress,

        DropSuccess
    }

}
