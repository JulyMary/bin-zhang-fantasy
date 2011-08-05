using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Diagnostics;

namespace Syncfusion.Windows.Shared
{
    internal abstract class VirtualizingPanelHandler
    {
        private VisibleItemsHandler initialPathPositionStates;
        private bool isInitialized;

        /// <summary>
        /// Begins the animation.
        /// </summary>
        /// <param name="beginTime">The begin time.</param>
        public virtual void BeginItemMovement(TimeSpan beginTime)
        {
            if (!this.isInitialized)
            {
                return;
            }
            if (this.state != ItemMovementState.NotStarted)
            {
                return;
            }
            this.lastRender = beginTime;
            this.state = ItemMovementState.Started;
        }

        public VirtualizingPanelHandler()
        {
        }

        public void Initialize(CarouselPathHelper PathHelper, VisibleItemsHandler positionStates)
        {
            if (PathHelper == null)
            {
                throw new ArgumentNullException("path");
            }
            if (positionStates == null)
            {
                throw new ArgumentNullException("positionStates");
            }

            this.CarouselPathHelper = PathHelper;
            if (this.initialPathPositionStates == null)
            {
                this.initialPathPositionStates = positionStates;
            }
            this.Initialize();
            this.isInitialized = true;
        }

        protected virtual void Initialize()
        {
        }

        public abstract void CalculateItemsToAdd(out VisibleRangeAction action, out LinkedList<VisiblePanelItem> itemsToAdd);
        public abstract void AddItemToMove(VisiblePanelItem item);
        protected abstract void Animate(double percentageDone);
        public abstract void EndItemMovement();
        public abstract void Reverse();
        public abstract IList<VisiblePanelItem> GetItemsToRemoveEndofArrangeOverride();

        private static Point CalculateNewPosition(UIElement item, CarouselPathHelper carouselPathHelper)
        {
            Point newItemPosition;
            Point newItemTangent;
            double pathFraction = CarouselPanel.GetPathFraction(item);
            carouselPathHelper.Geometry.GetPointAtFractionLength(pathFraction, out newItemPosition, out newItemTangent);
            return newItemPosition;
        }

        public static MatrixTransform RecalculateItemPosition(UIElement item, CarouselPathHelper carouselPathHelper)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }
            if (carouselPathHelper == null)
            {
                throw new ArgumentNullException("animationPath");
            }
            Matrix newMaxtrix = Matrix.Identity;

            //Point newItemPosition;
            //Point newItemTangent;
            //PathAnimationData data = (PathAnimationData)CarouselPanel.GetPathAnimationData(item);// .GetValue(ItemMovementAnimationDataProperty);
            //double pathFraction = data.AnimationEndFraction; //(double)child.GetValue(PathFractionProperty);
            //animationPath.Geometry.GetPointAtFractionLength(pathFraction, out newItemPosition, out newItemTangent);
            Point newItemPosition = CalculateNewPosition(item, carouselPathHelper);
            newMaxtrix.Translate(newItemPosition.X, newItemPosition.Y);
            CenterItemOnPath(item, ref newMaxtrix);
            return new MatrixTransform(newMaxtrix);
        }

        private static void CenterItemOnPath(UIElement item, ref Matrix itemTransform)
        {
            Size itemSize = item.RenderSize;
            itemTransform.Translate(-(itemSize.Width / 2.0), -(itemSize.Height / 2.0));
        }

        /// <summary>
        /// Sets the animation data.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="startPathFraction">The start path fraction.</param>
        /// <param name="endPathFraction">The end path fraction.</param>
        protected static void SetPathFractionManagerForItem(UIElement item, double startPathFraction, double endPathFraction)
        {
            if ((startPathFraction == 0 && endPathFraction == 1) || ((startPathFraction == 1 && endPathFraction == 0)))
            {
                endPathFraction = startPathFraction;
                //Debug.WriteLine(startPathFraction.ToString(), endPathFraction.ToString());
            }
            PathFractionManager newAnimationData = new PathFractionManager(endPathFraction, startPathFraction);
            CarouselPanel.SetPathFractionManager(item, newAnimationData);
        }

        protected static void EndItemMovement(List<VisiblePanelItem> collection, CarouselPathHelper animationPath)
        {
            foreach (VisiblePanelItem currentElement in collection)
            {
                PathFractionManager data = CarouselPanel.GetPathFractionManager(currentElement.Child);
                CarouselPanel.SetPathFraction(currentElement.Child, data.NewPathFraction);
                RecalculateItemPosition(currentElement.Child, animationPath);
            }
        }

        public void Update(TimeSpan currentTime)
        {
            if (this.state == ItemMovementState.Started)
            {
                TimeSpan deltaTime = currentTime.Subtract(this.lastRender);
                this.totalRunningTime = this.totalRunningTime.Add(deltaTime);
                this.lastRender = currentTime;
                if (this.totalRunningTime < this.duration)
                {
                    this.Animate(this.GetPercentageDone());
                }
                else
                {
                    this.EndItemMovement();
                    this.state = ItemMovementState.Finished;
                }
            }
        }

        public double GetPercentageDone()
        {
            return (this.totalRunningTime.TotalSeconds / this.duration.TotalSeconds);
        }

        public TimeSpan GetTimeLeft()
        {
            if (this.duration <= this.totalRunningTime)
            {
                return TimeSpan.Zero;
            }
            return this.duration.Subtract(this.totalRunningTime);
        }

        protected static void ReverseAnimationdata(List<VisiblePanelItem> collection)
        {
            foreach (VisiblePanelItem currentElement in collection)
            {
                PathFractionManager data = CarouselPanel.GetPathFractionManager(currentElement.Child);
                PathFractionManager newData = new PathFractionManager(data.CurrentPathFraction, data.NewPathFraction);
                CarouselPanel.SetPathFractionManager(currentElement.Child, newData);
            }
        }

        protected static void UpdateItemPathFraction(UIElement item, double animationPercentageDone)
        {
            PathFractionManager data = CarouselPanel.GetPathFractionManager(item);
            double distance = data.NewPathFraction - data.CurrentPathFraction;
            CarouselPanel.SetPathFraction(item, data.CurrentPathFraction + (distance * animationPercentageDone));
        }

        protected void UpdateItemTransformation(UIElement item)
        {
            Matrix matrix = RecalculateItemPosition(item, this.CarouselPathHelper).Matrix;
            matrix.Translate(0.0, 0.0);
            item.RenderTransform = new MatrixTransform(matrix);
        }

        private CarouselPathHelper carouselPathHelper;
        /// <summary>
        /// Gets or sets the carousel path helper.
        /// </summary>
        /// <value>The carousel path helper.</value>
        public CarouselPathHelper CarouselPathHelper
        {
            get
            {
                return this.carouselPathHelper;
            }
            protected set
            {
                this.carouselPathHelper = value;
            }
        }

        private TimeSpan duration = new TimeSpan(0, 0, 0, 0, 300);
        private TimeSpan lastRender = new TimeSpan(0, 0, 0);
        private TimeSpan totalRunningTime = new TimeSpan(0, 0, 0);
        private ItemMovementState state = ItemMovementState.NotStarted;

        public TimeSpan Duration
        {
            get
            {
                return this.duration;
            }
            set
            {
                this.duration = value;
                if ((this.duration <= this.totalRunningTime) && (this.state == ItemMovementState.Started))
                {
                    this.EndItemMovement();
                    this.state = ItemMovementState.Finished;
                }
            }
        }

        public VisibleItemsHandler InitialPathPositionStates
        {
            get
            {
                return this.initialPathPositionStates;
            }
            set
            {
                this.initialPathPositionStates = value;
            }
        }

        public bool IsInitialized
        {
            get
            {
                return this.isInitialized;
            }
        }

        public TimeSpan LastRender
        {
            get
            {
                return this.lastRender;
            }
            protected set
            {
                this.lastRender = value;
            }
        }

        public ItemMovementState State
        {
            get
            {
                return this.state;
            }
            protected set
            {
                this.state = value;
            }
        }

        public TimeSpan TotalRunningTime
        {
            get
            {
                return this.totalRunningTime;
            }
            protected set
            {
                this.totalRunningTime = value;
            }
        }
    }

    internal enum ItemMovementState
    {
        NotStarted,
        Started,
        Finished
    }
}
