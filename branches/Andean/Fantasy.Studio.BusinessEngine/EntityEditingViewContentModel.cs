using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Fantasy.Studio.BusinessEngine
{
    class EntityEditingViewContentModel
    {
        public EntityEditingViewContentModel()
        {
            this.EditingPanels = new List<IEntityEditingPanel>();
        }

        public List<IEntityEditingPanel> EditingPanels { get; private set; }

        public Visibility TabStripVisibility
        {
            get
            {
                return this.EditingPanels.Count > 1 ? Visibility.Visible : Visibility.Collapsed;
            }
        }
    }
}
