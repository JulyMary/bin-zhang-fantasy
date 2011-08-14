using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.Collection;
using Fantasy.Windows;
using Fantasy.Studio.Codons;

namespace Fantasy.Studio.Controls
{
    public class ToolBoxModel : NotifyPropertyChangedObject
    {

        public ToolBoxModel()
        {
            
        }

        private IEnumerable<ToolBoxItemModel>  _items;

        public virtual IEnumerable<ToolBoxItemModel>  Items
        {
            get { return _items; }
            set
            {
                if (_items != value)
                {
                    _items = value;
                    this.OnPropertyChanged("Items");
                }
            }
        }

        private ToolBoxView _view = ToolBoxView.List;

        public ToolBoxView View
        {
            get { return _view; }
            set
            {
                if (_view != value)
                {
                    _view = value;
                    this.OnPropertyChanged("View");
                }
            }
        }

    }
}
