using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace Syncfusion.Windows.Shared
{
    internal class CarouselPanelHelper
    {
        /// <summary>
        /// 
        /// </summary>
        private CarouselPanel carouselPanel;

        /// <summary>
        /// Initializes a new instance of the <see cref="CarouselPanelHelper"/> class.
        /// </summary>
        /// <param name="panel">The panel.</param>
        public CarouselPanelHelper(CarouselPanel panel)
        {
            this.carouselPanel = panel;
        }

        /// <summary>
        /// Gets the items count.
        /// </summary>
        /// <returns></returns>
        public int GetItemsCount()
        {
            int children = this.carouselPanel.Children.Count;
            if (this.carouselPanel.IsItemsHost)
            {
                children = this.GetParentItemsControl().Items.Count;
            }
            return children;
        }

        /// <summary>
        /// Gets the parent items control.
        /// </summary>
        /// <returns></returns>
        private ItemsControl GetParentItemsControl()
        {
            ItemsControl parent = null;
            if (this.carouselPanel.IsItemsHost)
            {
                parent = ItemsControl.GetItemsOwner(this.carouselPanel);
            }
            return parent;
        }

        /// <summary>
        /// Gets the items count.
        /// </summary>
        /// <value>The items count.</value>
        public int ItemsCount
        {
            get
            {
                return this.GetItemsCount();
            }
        }

        /// <summary>
        /// Gets the size of the page.
        /// </summary>
        /// <value>The size of the page.</value>
        public int PageSize
        {
            get
            {
                return this.carouselPanel.ItemsPerPage;
            }
        }

        /// <summary>
        /// Gets the position.
        /// </summary>
        /// <value>The position.</value>
        public int Position
        {
            get
            {
                return (int)this.carouselPanel.PanelOffset;
            }
        }
    }
}
