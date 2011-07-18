using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows.Controls;

namespace Fantasy.Studio.BusinessEngine
{
    public interface IDocumentTreeViewItem
    {
        string Name { get;}

        ImageSource Icon { get; }

        IEnumerable<IDocumentTreeViewItem> ChildItems { get; }

        ContextMenu ContextMenu { get; }

        void Refresh();

        void Open();

       

    }
}
