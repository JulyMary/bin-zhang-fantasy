using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Windows;

namespace Syncfusion.Windows.Shared
{
    internal class PathFractionRangeHandler : IEnumerable<VisiblePanelItem>, IEnumerable
    {
        private LinkedList<VisiblePanelItem> _ChildIndexPair;
        private int firstVisibleItemIndex;
        private int lastVisibleItemIndex;
        private const int MinimumIndexValue = -1;
        private LinkedList<VisiblePanelItem> visibleItems;

        public PathFractionRangeHandler()
        {
            this.firstVisibleItemIndex = -1;
            this.lastVisibleItemIndex = -1;
            this.visibleItems = new LinkedList<VisiblePanelItem>();
            this.ToCleanUp = new LinkedList<VisiblePanelItem>();
        }

        internal PathFractionRangeHandler(LinkedList<VisiblePanelItem> visibleItems)
            : this()
        {
            this.visibleItems = visibleItems;
        }

        public void AddFirst(LinkedList<VisiblePanelItem> pairs)
        {
            if (pairs == null)
            {
                throw new ArgumentNullException("pairs");
            }
            if (pairs.Count > 0)
            {
                for (LinkedListNode<VisiblePanelItem> pair = pairs.Last; pair != null; pair = pair.Previous)
                {
                    this.AddFirst(pair.Value);
                }
            }
        }

        public void AddFirst(VisiblePanelItem pair)
        {
            if (pair == null)
            {
                throw new ArgumentNullException("pair");
            }
            this.visibleItems.AddFirst(pair);
        }

        public void AddLast(LinkedList<VisiblePanelItem> pairs)
        {
            if (pairs == null)
            {
                throw new ArgumentNullException("pairs");
            }
            if (pairs.Count > 0)
            {
                foreach (VisiblePanelItem pair in pairs)
                {
                    this.AddLast(pair);
                }
            }
        }

        public void AddLast(VisiblePanelItem pair)
        {
            if (pair == null)
            {
                throw new ArgumentNullException("pair");
            }
            this.visibleItems.AddLast(pair);
        }

        public void Clear()
        {
            this.visibleItems.Clear();
            this.ToCleanUp.Clear();
        }

        public void ClearCleanUp()
        {
            this.ToCleanUp.Clear();
        }

        public IEnumerator<VisiblePanelItem> GetEnumerator()
        {
            return this.visibleItems.GetEnumerator();
        }

        public int GetVisibleItemsCount()
        {
            if (this.HasVisibleItems)
            {
                return ((this.lastVisibleItemIndex - this.firstVisibleItemIndex) + 1);
            }
            return 0;
        }

        public bool IsInVisibleRange(int index)
        {
            if ((index < 0) || !this.HasVisibleItems)
            {
                return false;
            }
            return ((index >= this.firstVisibleItemIndex) && (index <= this.lastVisibleItemIndex));
        }

        public void Remove(VisiblePanelItem pair)
        {
            this.visibleItems.Remove(pair);
        }

        public void ScheduleClean(IList<VisiblePanelItem> pairs)
        {
            foreach (VisiblePanelItem pair in pairs)
            {
                if (this.visibleItems.Remove(pair))
                {
                    this.ToCleanUp.AddFirst(pair);
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.visibleItems.GetEnumerator();
        }

        public void UpdateVisibleRange(VisibleRangeAction action, LinkedList<VisiblePanelItem> pairs)
        {
            LinkedList<VisiblePanelItem> newPairs = new LinkedList<VisiblePanelItem>();
            foreach (VisiblePanelItem pair in pairs)
            {
                if (!this.visibleItems.Contains(pair))
                {
                    newPairs.AddFirst(pair);
                }
            }
            if (action == VisibleRangeAction.AddFromEnd)
            {
                this.AddLast(newPairs);
            }
            if (action == VisibleRangeAction.AddFromStart)
            {
                this.AddFirst(newPairs);
            }
            if ((action == VisibleRangeAction.RemoveFromEnd) || (action == VisibleRangeAction.RemoveFromStart))
            {
                throw new NotImplementedException();
            }
        }

        public int Count
        {
            get
            {
                return this.visibleItems.Count;
            }
        }

        public VisiblePanelItem First
        {
            get
            {
                if (this.visibleItems.First != null)
                {
                    return this.visibleItems.First.Value;
                }
                return null;
            }
        }

        public int FirstVisibleItemIndex
        {
            get
            {
                if (this.First == null)
                {
                    return -1;
                }
                return this.First.Index;
            }
        }

        public bool HasVisibleItems
        {
            get
            {
                return (this.visibleItems.Count > 0);
            }
        }

        public VisiblePanelItem Last
        {
            get
            {
                if (this.visibleItems.Last != null)
                {
                    return this.visibleItems.Last.Value;
                }
                return null;
            }
        }

        public int LastVisibleItemIndex
        {
            get
            {
                if (this.Last == null)
                {
                    return -1;
                }
                return this.Last.Index;
            }
        }

        public LinkedList<VisiblePanelItem> ToCleanUp
        {
            get
            {
                return this._ChildIndexPair;
            }
            private set
            {
                this._ChildIndexPair = value;
            }
        }
    }

    internal class VisiblePanelItem
    {
        private UIElement _Child;
        private int _Index;

        public VisiblePanelItem(UIElement child, int index)
        {
            if (index < 0)
            {
                return;
            }
            this.Child = child;
            this.Index = index;
        }

        public override bool Equals(object obj)
        {
            VisiblePanelItem otherPair = obj as VisiblePanelItem;
            return ((otherPair != null) && (this.Index == otherPair.Index));
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public UIElement Child
        {
            get
            {
                return this._Child;
            }
            set
            {
                this._Child = value;
            }
        }

        public int Index
        {
            get
            {
                return this._Index;
            }
            set
            {
                this._Index = value;
            }
        }
    }

    internal enum VisibleRangeAction
    {
        RemoveFromStart,
        RemoveFromEnd,
        AddFromStart,
        AddFromEnd
    }
}
