using System.Collections.Generic;
using System;
using System.Linq;
using System.Text;
using System.Windows;

namespace Syncfusion.Windows.Shared
{
    internal class VirtualizingPanelItemMoveHandler : VirtualizingPanelHandler
    {
        public VirtualizingPanelItemMoveHandler()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        private VirtualizingItemsCollection _VirtualizingItemsCollection;
        private List<VisiblePanelItem> newItemsToExit;
        private List<VisiblePanelItem> newItemsToStay;
        private List<VisiblePanelItem> oldItemsToExit;
        private List<VisiblePanelItem> oldItemsToStay;

        /// <summary>
        /// Initializes a new instance of the <see cref="VirtualizingPanelItemMoveHandler"/> class.
        /// </summary>
        /// <param name="displacement">The displacement.</param>
        /// <param name="infoProvider">The info provider.</param>
        public VirtualizingPanelItemMoveHandler(int displacement, CarouselPanelHelper infoProvider)
        {
            this.pathDisplacement = displacement;
            this.oldItemsToExit = new List<VisiblePanelItem>();
            this.newItemsToExit = new List<VisiblePanelItem>();
            this.oldItemsToStay = new List<VisiblePanelItem>();
            this.newItemsToStay = new List<VisiblePanelItem>();
            this.Collection = new VirtualizingItemsCollection(infoProvider.PageSize, infoProvider.ItemsCount, infoProvider.Position);
        }

        public override void AddItemToMove(VisiblePanelItem item)
        {
            if (item != null)
            {
                if (CarouselPanel.GetPathFraction(item.Child) < 0.0)
                {
                    this.AddNewItem(item);
                }
                else
                {
                    this.AddExistingItem(item);
                }
            }
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        protected override void Initialize()
        {
            this.Collection.Move(this.pathDisplacement);
        }

        #region AddExistingItem

        private void AddExistingItem(VisiblePanelItem pair)
        {
            UIElement item = pair.Child;
            double currentPathFraction = CarouselPanel.GetPathFraction(item);
            double targetFraction = this.CalculateNextPathFraction(pair.Index);
            VirtualizingPanelHandler.SetPathFractionManagerForItem(item, currentPathFraction, targetFraction);
            CarouselPanel.SetPathFraction(item, targetFraction);
            //switch (targetFraction)
            //{
            //    case 0.0:
            //    case 1.0:
            //        this.oldItemsToExit.Add(pair);
            //        return;
            //}
            switch (targetFraction.ToString())
            {
                case "0.0":
                case "1.0":
                    this.oldItemsToExit.Add(pair);
                    return;
            }
            this.oldItemsToStay.Add(pair);
        }

        public double CalculateNextPathFraction(int index)
        {
            int viewrangePosition = this.Collection.GetPosition(index);
            if (viewrangePosition == -1)
            {
                return this.GetExitPathFraction();
            }
            return base.CarouselPathHelper.GetVisiblePathFraction(viewrangePosition).PathFraction;
        }

        public double GetExitPathFraction()
        {
            if (this.PathDisplacement >= 0)
            {
                return 1.0;
            }
            return 0.0;
        }
        #endregion

        #region AddNewItem

        private void AddNewItem(VisiblePanelItem pair)
        {
            UIElement item = pair.Child;
            this.SetStartingPathFraction(item);
            this.CalculateNewItemAnimation(pair);
            this.newItemsToStay.Add(pair);
        }

        private void SetStartingPathFraction(UIElement item)
        {
            double startingPathFraction = -1.0;
            if (this.pathDisplacement <= 0)
            {
                startingPathFraction = 1.0;
            }
            else if (this.pathDisplacement > 0)
            {
                startingPathFraction = 0.0;
            }
            CarouselPanel.SetPathFraction(item, startingPathFraction);
        }

        private void CalculateNewItemAnimation(VisiblePanelItem pair)
        {
            double currentPathFraction = CarouselPanel.GetPathFraction(pair.Child);
            double nextFraction = this.CalculateNextPathFraction(pair.Index);
            VirtualizingPanelHandler.SetPathFractionManagerForItem(pair.Child, currentPathFraction, nextFraction);
            CarouselPanel.SetPathFraction(pair.Child, nextFraction);
        }
        #endregion

        /// <summary>
        /// Calculates the items to add.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="itemsToAdd">The items to add.</param>
        public override void CalculateItemsToAdd(out VisibleRangeAction action, out LinkedList<VisiblePanelItem> itemsToAdd)
        {
            itemsToAdd = new LinkedList<VisiblePanelItem>();
            if (this.PathDisplacement >= 0)
            {
                action = VisibleRangeAction.AddFromEnd;
                for (int i = 0; i < this.Collection.VisibleItemsCount; i++)
                {
                    VisiblePanelItem pair = new VisiblePanelItem(null, this.Collection.FirstVisibleIndex + i);
                    itemsToAdd.AddFirst(pair);
                }
            }
            else
            {
                action = VisibleRangeAction.AddFromStart;
                for (int i = 0; i < this.Collection.VisibleItemsCount; i++)
                {
                    VisiblePanelItem pair = new VisiblePanelItem(null, this.Collection.FirstVisibleIndex + i);
                    itemsToAdd.AddFirst(pair);
                }
            }
        }

        public override IList<VisiblePanelItem> GetItemsToRemoveEndofArrangeOverride()
        {
            return this.oldItemsToExit;
        }

        protected override void Animate(double percentageDone)
        {
            this.Animate(this.oldItemsToExit, percentageDone);
            this.Animate(this.oldItemsToStay, percentageDone);
            this.Animate(this.newItemsToExit, percentageDone);
            this.Animate(this.newItemsToStay, percentageDone);
        }

        private void Animate(List<VisiblePanelItem> collection, double percentageDone)
        {
            foreach (VisiblePanelItem currentElement in collection)
            {
                VirtualizingPanelHandler.UpdateItemPathFraction(currentElement.Child, percentageDone);
                base.UpdateItemTransformation(currentElement.Child);
            }
        }

        public override void EndItemMovement()
        {
            VirtualizingPanelHandler.EndItemMovement(this.oldItemsToExit, base.CarouselPathHelper);
            VirtualizingPanelHandler.EndItemMovement(this.oldItemsToStay, base.CarouselPathHelper);
            VirtualizingPanelHandler.EndItemMovement(this.newItemsToExit, base.CarouselPathHelper);
            VirtualizingPanelHandler.EndItemMovement(this.newItemsToStay, base.CarouselPathHelper);
        }

        public bool IsOpposite(int displacement)
        {
            return (this.PathDisplacement == -displacement);
        }

        public override void Reverse()
        {
            this.pathDisplacement = -this.pathDisplacement;
            base.TotalRunningTime = base.Duration.Subtract(base.TotalRunningTime);
            VirtualizingPanelHandler.ReverseAnimationdata(this.oldItemsToExit);
            VirtualizingPanelHandler.ReverseAnimationdata(this.oldItemsToStay);
            VirtualizingPanelHandler.ReverseAnimationdata(this.newItemsToExit);
            VirtualizingPanelHandler.ReverseAnimationdata(this.newItemsToStay);
            this.ReverseOldAndNewItems();
        }

        private void ReverseOldAndNewItems()
        {
            List<VisiblePanelItem> refHolder = this.oldItemsToExit;
            this.oldItemsToExit = new List<VisiblePanelItem>(this.newItemsToExit);
            this.oldItemsToExit.AddRange(this.newItemsToStay);
            this.newItemsToExit.Clear();
            this.newItemsToStay = new List<VisiblePanelItem>(refHolder);
        }

        public VirtualizingItemsCollection Collection
        {
            get
            {
                return this._VirtualizingItemsCollection;
            }
            set
            {
                this._VirtualizingItemsCollection = value;
            }
        }

        private int pathDisplacement;
        public int PathDisplacement
        {
            get
            {
                return this.pathDisplacement;
            }
        }
    }
}
