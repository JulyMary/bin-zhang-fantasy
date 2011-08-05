using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace Syncfusion.Windows.Shared
{
    internal class VisibleItemsHandler
    {
        private const int AvailablePositionDigit = -1;
        private VisiblePanelItem[] positions;

        public VisibleItemsHandler(int visiblePositions)
        {
            this.positions = new VisiblePanelItem[visiblePositions];
        }

        public int GetFreePositionsLeft()
        {
            int freePositions = 0;
            for (int i = 0; i < this.Count; i++)
            {
                if (this[i] != null)
                {
                    return freePositions;
                }
                freePositions++;
            }
            return freePositions;
        }

        public int GetFreePositionsRight()
        {
            int freePositions = 0;
            for (int i = this.Count - 1; i >= 0; i--)
            {
                if (this[i] != null)
                {
                    return freePositions;
                }
                freePositions++;
            }
            return freePositions;
        }

        public VisiblePanelItem GetItemAtPosition(int positionIndex)
        {
            return this.positions[positionIndex];
        }

        public int GetLargestItemIndex()
        {
            int index = -1;
            for (int i = 0; i < this.Count; i++)
            {
                if ((this[i] != null) && (this[i].Index > index))
                {
                    index = this[i].Index;
                }
            }
            return index;
        }

        public int GetUsedPositions()
        {
            int usedPositions = 0;
            for (int i = 0; i < this.Count; i++)
            {
                if (this[i] != null)
                {
                    usedPositions++;
                }
            }
            return usedPositions;
        }

        public void SetItemAtPosition(int positionIndex, VisiblePanelItem item)
        {
            this.positions.SetValue(item, positionIndex);
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder(this.Count);
            for (int i = 0; i < this.Count; i++)
            {
                if (this[i] == null)
                {
                    //builder.Append(-1.ToString(CultureInfo.InvariantCulture));
                    builder.Append(AvailablePositionDigit.ToString(CultureInfo.InstalledUICulture));
                }
                else
                {
                    builder.Append(this[i].Index);
                }
            }
            return builder.ToString();
        }

        public int Count
        {
            get
            {
                return this.positions.Length;
            }
        }

        public VisiblePanelItem this[int positionIndex]
        {
            get
            {
                return this.positions[positionIndex];
            }
            set
            {
                this.positions.SetValue(value, positionIndex);
            }
        }
    }
}
