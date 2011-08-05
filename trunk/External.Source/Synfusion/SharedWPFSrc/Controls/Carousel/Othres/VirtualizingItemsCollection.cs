using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Syncfusion.Windows.Shared
{
    /// <summary>
    /// 
    /// </summary>
    internal class VirtualizingItemsCollection
    {
        private int _ItemsCount;
        private int _ItemsPerPage;
        private int firstVisibleIndex;
        private int lastVisibleIndex;
        private int offset;

        /// <summary>
        /// Initializes a new instance of the <see cref="VirtualizingItemsCollection"/> class.
        /// </summary>
        /// <param name="itemsPerPage">The items per page.</param>
        /// <param name="itemsCount">The items count.</param>
        public VirtualizingItemsCollection(int itemsPerPage, int itemsCount)
        {
            this.Offset = 0;
            this.ItemsPerPage = itemsPerPage;
            this.ItemsCount = itemsCount;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VirtualizingItemsCollection"/> class.
        /// </summary>
        /// <param name="itemsPerPage">The items per page.</param>
        /// <param name="itemsCount">The items count.</param>
        /// <param name="offset">The offset.</param>
        public VirtualizingItemsCollection(int itemsPerPage, int itemsCount, int offset)
        {
            this.ItemsPerPage = itemsPerPage;
            this.ItemsCount = itemsCount;
            this.Offset = offset;
        }

        /// <summary>
        /// Adjusts the offset after item added.
        /// </summary>
        /// <param name="newIndexPosition">The new index position.</param>
        /// <param name="count">The count.</param>
        public void AdjustOffsetAfterItemAdded(int newIndexPosition, int count)
        {
            if (newIndexPosition <= this.GetFirstVisibleIndex())
            {
                this.Offset += count;
            }
            this.ItemsCount += count;
            this.Offset = this.Offset;
        }

        /// <summary>
        /// Adjusts the offset after item removed.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="count">The count.</param>
        public void AdjustOffsetAfterItemRemoved(int index, int count)
        {
            if (index <= this.GetFirstVisibleIndex())
            {
                this.Offset -= count;
            }
            this.ItemsCount -= count;
            this.Offset = this.Offset;
        }

        /// <summary>
        /// Gets the first index of the visible.
        /// </summary>
        /// <returns></returns>
        private int GetFirstVisibleIndex()
        {
            for (int i = this.ItemsPerPage; i > 0; i--)
            {
                int index = this.Offset - i;
                if ((index >= 0) && (index < this.ItemsCount))
                {
                    return index;
                }
            }
            return -1;
        }

        /// <summary>
        /// Gets the items.
        /// </summary>
        /// <returns></returns>
        internal VisibleItemsHandler GetItems()
        {
            VisibleItemsHandler arrangement = new VisibleItemsHandler(this.ItemsPerPage);
            if (this.HasVisibleItems)
            {
                for (int i = this.FirstVisibleIndex; i <= this.LastVisibleIndex; i++)
                {
                    int position = this.GetPosition(i);
                    VisiblePanelItem pair = new VisiblePanelItem(null, i);
                    arrangement.SetItemAtPosition(position, pair);
                }
            }
            return arrangement;
        }

        /// <summary>
        /// Gets the last index of the visible.
        /// </summary>
        /// <returns></returns>
        private int GetLastVisibleIndex()
        {
            for (int i = 1; i <= this.ItemsPerPage; i++)
            {
                int index = this.Offset - i;
                if (index < this.ItemsCount)
                {
                    return index;
                }
            }
            return -1;
        }

        /// <summary>
        /// Gets the position.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        public int GetPosition(int index)
        {
            if ((index >= 0) && CarouselPanelHelperMethods.IsInRange(index, this.FirstVisibleIndex, this.LastVisibleIndex))
            {
                return Math.Abs((int)(index - (this.Offset - 1)));
            }
            return -1;
        }

        /// <summary>
        /// Determines whether [is before visible range] [the specified index].
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns>
        /// 	<c>true</c> if [is before visible range] [the specified index]; otherwise, <c>false</c>.
        /// </returns>
        private bool IsBeforeVisibleRange(int index)
        {
            int firstIndex = this.Offset - this.ItemsPerPage;
            return (index < firstIndex);
        }

        /// <summary>
        /// Moves the specified displacement.
        /// </summary>
        /// <param name="displacement">The displacement.</param>
        public void Move(int displacement)
        {
            if (displacement > 0)
            {
                this.MoveRight(displacement);
            }
            else
            {
                this.MoveLeft(displacement);
            }
        }

        /// <summary>
        /// Moves the left.
        /// </summary>
        /// <param name="displacement">The displacement.</param>
        public void MoveLeft(int displacement)
        {
            int absDisplacement = Math.Abs(displacement);
            int newOffset = this.Offset - absDisplacement;
            this.Offset = CarouselPanelHelperMethods.CoerceValueBetweenRange(newOffset, 0, this.ItemsPerPage + this.ItemsCount);
        }

        /// <summary>
        /// Moves the right.
        /// </summary>
        /// <param name="displacement">The displacement.</param>
        public void MoveRight(int displacement)
        {
            Math.Abs(displacement);
            int newOffset = this.Offset + displacement;
            this.Offset = CarouselPanelHelperMethods.CoerceValueBetweenRange(newOffset, 0, this.ItemsPerPage + this.ItemsCount);
        }

        /// <summary>
        /// Tries the fill.
        /// </summary>
        public void TryFill()
        {
            if (!this.PageFull)
            {
                int countBefore = this.CountBefore;
                int countAfter = this.CountAfter;
                if (countAfter >= countBefore)
                {
                    this.Offset += Math.Min(Math.Max(this.FreePosition, countAfter), this.FreePosition);
                }
                else
                {
                    this.Offset -= Math.Min(Math.Max(this.FreePosition, countBefore), this.FreePosition);
                }
            }
        }

        /// <summary>
        /// Gets the count after.
        /// </summary>
        /// <value>The count after.</value>
        public int CountAfter
        {
            get
            {
                if (this.LastVisibleIndex >= 0)
                {
                    return ((this.ItemsCount - this.LastVisibleIndex) - 1);
                }
                if (this.Offset == 0)
                {
                    return this.ItemsCount;
                }
                return 0;
            }
        }

        /// <summary>
        /// Gets the count before.
        /// </summary>
        /// <value>The count before.</value>
        public int CountBefore
        {
            get
            {
                if (this.FirstVisibleIndex > 0)
                {
                    return this.FirstVisibleIndex;
                }
                if (this.OffsetIsMaximum)
                {
                    return this.ItemsPerPage;
                }
                return 0;
            }
        }

        /// <summary>
        /// Gets the first index of the visible.
        /// </summary>
        /// <value>The first index of the visible.</value>
        public int FirstVisibleIndex
        {
            get
            {
                return this.firstVisibleIndex;
            }
        }

        /// <summary>
        /// Gets the free position.
        /// </summary>
        /// <value>The free position.</value>
        public int FreePosition
        {
            get
            {
                return (this.ItemsPerPage - this.VisibleItemsCount);
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance has more items.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance has more items; otherwise, <c>false</c>.
        /// </value>
        public bool HasMoreItems
        {
            get
            {
                return (this.VisibleItemsCount < this.ItemsCount);
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance has reached end.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance has reached end; otherwise, <c>false</c>.
        /// </value>
        public bool HasReachedEnd
        {
            get
            {
                return (this.Offset >= (this.ItemsCount + this.ItemsPerPage));
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance has visible items.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance has visible items; otherwise, <c>false</c>.
        /// </value>
        public bool HasVisibleItems
        {
            get
            {
                return ((this.LastVisibleIndex >= 0) && (this.FirstVisibleIndex >= 0));
            }
        }

        /// <summary>
        /// Gets or sets the items count.
        /// </summary>
        /// <value>The items count.</value>
        public int ItemsCount
        {
            get
            {
                return this._ItemsCount;
            }
            private set
            {
                this._ItemsCount = value;
            }
        }

        /// <summary>
        /// Gets or sets the items per page.
        /// </summary>
        /// <value>The items per page.</value>
        public int ItemsPerPage
        {
            get
            {
                return this._ItemsPerPage;
            }
            private set
            {
                this._ItemsPerPage = value;
            }
        }

        /// <summary>
        /// Gets the last index of the visible.
        /// </summary>
        /// <value>The last index of the visible.</value>
        public int LastVisibleIndex
        {
            get
            {
                return this.lastVisibleIndex;
            }
        }

        /// <summary>
        /// Gets or sets the offset.
        /// </summary>
        /// <value>The offset.</value>
        public int Offset
        {
            get
            {
                return this.offset;
            }
            private set
            {
                this.offset = value;
                this.firstVisibleIndex = this.GetFirstVisibleIndex();
                this.lastVisibleIndex = this.GetLastVisibleIndex();
            }
        }

        /// <summary>
        /// Gets a value indicating whether [offset is maximum].
        /// </summary>
        /// <value><c>true</c> if [offset is maximum]; otherwise, <c>false</c>.</value>
        public bool OffsetIsMaximum
        {
            get
            {
                return (this.Offset == (this.ItemsCount + this.ItemsPerPage));
            }
        }

        /// <summary>
        /// Gets a value indicating whether [page full].
        /// </summary>
        /// <value><c>true</c> if [page full]; otherwise, <c>false</c>.</value>
        public bool PageFull
        {
            get
            {
                return (this.VisibleItemsCount == this.ItemsPerPage);
            }
        }

        /// <summary>
        /// Gets the visible items count.
        /// </summary>
        /// <value>The visible items count.</value>
        public int VisibleItemsCount
        {
            get
            {
                if (this.HasVisibleItems)
                {
                    return ((this.LastVisibleIndex - this.FirstVisibleIndex) + 1);
                }
                return 0;
            }
        }


    }
}
