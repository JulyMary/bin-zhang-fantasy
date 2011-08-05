using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Controls.Primitives;
using System.Collections.Specialized;
using System.Collections.ObjectModel;

namespace Syncfusion.Windows.Shared
{
    /// <summary>
    /// 
    /// </summary>
    public class CarouselPanel : VirtualizingPanel
    {
        #region Properties

        private System.Windows.Shapes.Path _Path;
        //new System.Windows.Shapes.Path()
        //{
        //    Data = PathGeometry.Parse("M639,-115.5 C702,-106.5 666.49972,-35 491.49972,-35 300.4994,-35 293.49973,-116 343.50004,-116")
        //};

        /// <summary>
        /// Gets or sets the path.
        /// </summary>
        /// <value>The path.</value>
        public System.Windows.Shapes.Path Path
        {
            get { return _Path; }
            set { _Path = value; }
        }

        /// <summary>
        /// Gets or sets the items per page.
        /// </summary>
        /// <value>The items per page.</value>
        public int ItemsPerPage
        {
            get { return (int)GetValue(ItemsPerPageProperty); }
            set { SetValue(ItemsPerPageProperty, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty ItemsPerPageProperty =
            DependencyProperty.Register("ItemsPerPage", typeof(int), typeof(CarouselPanel), new PropertyMetadata(7, (s, a) => ((CarouselPanel)s).OnItemsPerPageChanged(s)));

        /// <summary>
        /// Called when [items per page changed].
        /// </summary>
        /// <param name="obj">The obj.</param>
        protected void OnItemsPerPageChanged(DependencyObject obj)
        {
            CarouselPanel panel = obj as CarouselPanel;
            if (panel != null)
            {
                panel.SetPaths();
                panel.Invalidate(false);
            }
        }

        public double TopItemPosition
        {
            get { return (double)GetValue(TopItemPositionProperty); }
            set { SetValue(TopItemPositionProperty, value); }
        }

        public static readonly DependencyProperty TopItemPositionProperty =
            DependencyProperty.Register("TopItemPosition", typeof(double), typeof(CarouselPanel), new PropertyMetadata(0.5, new PropertyChangedCallback(OnTopItemPositionChanged), new CoerceValueCallback(CoerceTopItemPosition)));

        private static void OnTopItemPositionChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            CarouselPanel panel = (CarouselPanel)obj;
            if (panel != null)
            {
                panel.Invalidate(false);
            }
        }

        private static object CoerceTopItemPosition(DependencyObject obj,object value)
        {
            double newVal = (double)value;
            if (newVal > 1 || newVal < 0)
            {
                newVal = 0.5;
            }
            return newVal;
        }
        
        #endregion

        #region Constructor

        /// <summary>
        /// 
        /// </summary>
        private static readonly DependencyProperty PathFractionProperty;

        /// <summary>
        /// Initializes the <see cref="CarouselPanel"/> class.
        /// </summary>
        static CarouselPanel()
        {
            PathFractionProperty = DependencyProperty.RegisterAttached("PathFraction", typeof(double), typeof(CarouselPanel), new PropertyMetadata(-1.0));
            ItemPathFractionManagerProperty = DependencyProperty.RegisterAttached("ItemMovementAnimationDataFraction", typeof(PathFractionManager), typeof(CarouselPanel));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CarouselPanel"/> class.
        /// </summary>
        private CarouselPanelHelper CarouselPanelHelper;
        public CarouselPanel()
        {
            CarouselPanelHelper = new CarouselPanelHelper(this);
            base.AddHandler(UIElement.MouseDownEvent, new RoutedEventHandler(this.SelectedItemChanged));
            this.Loaded += new RoutedEventHandler(CarouselPanel_Loaded);
        }

        void CarouselPanel_Loaded(object sender, RoutedEventArgs e)
        {
            this.FillPathWithItems(this.ItemsPerPage);
        }

        private bool ShouldLoadItems
        {
            get { return true; }
        }
        #endregion

        #region Overrides

        protected override Size MeasureOverride(Size availableSize)
        {
            this.SetPaths();
            this.carouselPathHelper.UpdateGeometryPath(availableSize, new Thickness(0, 0, 0, 0));
            this.CleanUpItems();
            this.InitializeItemMovement();
            this.UpdateVisibleItems();

            foreach (UIElement child in base.InternalChildren)
            {
                child.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            }
            this.SetMaximumandViewPanelOffset();
            this.Start_ItemMovement();
            return availableSize;
        }

        protected override UIElementCollection CreateUIElementCollection(FrameworkElement logicalParent)
        {
            ObservableUIElementCollection elementCollection = new ObservableUIElementCollection(this, logicalParent);
            //elementCollection.CollectionChanged += new NotifyCollectionChangedEventHandler(OnChildrenCollectionChanged);
            return elementCollection;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            //this.SetPaths();
            double childHeightStartPoint = 0.0;
            if (base.IsItemsHost)
            {
                foreach (UIElement child in base.InternalChildren)
                {
                    //Point newItemPosition;
                    //Point newItemTangent;
                    //double pathFraction = (double)child.GetValue(PathFractionProperty);
                    //carouselPathHelper.Geometry.GetPointAtFractionLength(pathFraction, out newItemPosition, out newItemTangent);
                    //TranslateTransform transform = new TranslateTransform(newItemPosition.X - child.DesiredSize.Width / 2, newItemPosition.Y - child.DesiredSize.Height / 2);
                    //TransformGroup finalTransform = new TransformGroup();
                    //finalTransform.Children.Add(transform);
                    //finalTransform.Freeze();
                    //child.RenderTransform = finalTransform;
                    ////child.Arrange(new Rect(newItemPosition.X - child.DesiredSize.Width / 2, newItemPosition.Y - child.DesiredSize.Height / 2, child.DesiredSize.Width, child.DesiredSize.Height));
                    //child.Arrange(new Rect(0, 0, child.DesiredSize.Width, child.DesiredSize.Height));
                    //UpdateVisualization();
                    childHeightStartPoint = ArrangeVisibleChild(childHeightStartPoint, child);
                    this.RecalculatePosition(child);
                }
            }
            else
            {
                foreach (UIElement child in base.Children)
                {
                    Point newItemPosition;
                    Point newItemTangent;
                    double pathFraction = (double)child.GetValue(PathFractionProperty);
                    carouselPathHelper.Geometry.GetPointAtFractionLength(pathFraction, out newItemPosition, out newItemTangent);
                    TranslateTransform transform = new TranslateTransform(newItemPosition.X - child.DesiredSize.Width/2, newItemPosition.Y - child.DesiredSize.Height/2);
                    TransformGroup finalTransform = new TransformGroup();
                    finalTransform.Children.Add(transform);
                    finalTransform.Freeze();
                    child.RenderTransform = finalTransform;
                    child.Arrange(new Rect(0, 0, child.DesiredSize.Width, child.DesiredSize.Height));
                    //child.Arrange(new Rect(newItemPosition.X - child.DesiredSize.Width / 2, newItemPosition.Y - child.DesiredSize.Height / 2, child.DesiredSize.Width, child.DesiredSize.Height));
                }
            }
            return finalSize;
        }

        private static double ArrangeVisibleChild(double childHeightStartPoint, UIElement child)
        {
            child.Arrange(new Rect(new Point(0.0, 0.0), child.DesiredSize));
            childHeightStartPoint += child.DesiredSize.Height;
            return childHeightStartPoint;
        }

        internal void RecalculatePosition(UIElement child)
        {
            MatrixTransform newTransform = null;
            if (this.CurrentVirtualizingPanelHandler == null)
            {
                newTransform = VirtualizingPanelHandler.RecalculateItemPosition(child, this.carouselPathHelper);
                this.ApplyOffsetTransform(child, newTransform);
            }
            this.UpdateVisualization();
        }

        private void ApplyOffsetTransform(UIElement item, MatrixTransform transform)
        {
            Point pathOffset = new Point(0, 0);

            Matrix matrix = transform.Matrix;
            matrix.Translate(pathOffset.X, pathOffset.Y);
            MatrixTransform newtransform = new MatrixTransform(matrix);
            item.RenderTransform = newtransform;
        }

        protected override void OnItemsChanged(object sender, System.Windows.Controls.Primitives.ItemsChangedEventArgs args)
        {
            base.OnItemsChanged(sender, args);
        }

        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);
            //DrawPath(dc);
        }

        public void RenderAgain()
        {
            for (int i = 0; i < base.InternalChildren.Count; i++)
            {
                UIElement child = base.InternalChildren[i];
                child.SetValue(PathFractionProperty, carouselPathHelper.PathFractions[i].PathFraction);
            }
        }
        #endregion
        
        #region Path

        internal Path DrawingPath;
        internal CarouselPathHelper carouselPathHelper;
        private void SetPaths()
        {
            if (this.Path == null)
            {
                this.SetDrawingPathFromPath(CarouselPanelHelperMethods.GetDefaultPath());
            }
            else
            {
                this.SetDrawingPathFromPath(this.Path);
            }
            //if (this.animationPath.ItemsOnPath != this.ItemsPerPage)
            //{
            //    this.animationPath = new CarouselPath(this.RenderedPath, this.ItemsPerPage);
            //    this.animationPath.SetTopElementControlPoint(new ControlPoint(this.TopItemPathFraction));
            //}
        }

        private void SetDrawingPathFromPath(Path drawingPath)
        {
            this.DrawingPath = drawingPath;
            this.SetFractionPathDrawing(drawingPath);
        }

        private void SetFractionPathDrawing(Path drawingPath)
        {
            if (drawingPath != null)
            {
                this.carouselPathHelper = new CarouselPathHelper(drawingPath, ItemsPerPage);
                this.carouselPathHelper.SetTopElementPathFraction(new PathFractions(this.TopItemPosition));
            }
        }

        private void DrawPath(DrawingContext dc)
        {
            Path RenderedPath = this.Path;
            if (((dc != null) && (RenderedPath != null)))
            {
                Pen pathPen = new Pen();
                pathPen.Brush = new SolidColorBrush(Colors.Red); //RenderedPath.Stroke;
                pathPen.DashCap = RenderedPath.StrokeDashCap;
                pathPen.EndLineCap = RenderedPath.StrokeEndLineCap;
                pathPen.LineJoin = RenderedPath.StrokeLineJoin;
                pathPen.MiterLimit = RenderedPath.StrokeMiterLimit;
                pathPen.StartLineCap = RenderedPath.StrokeStartLineCap;
                pathPen.Thickness = 2;// RenderedPath.StrokeThickness;
                dc.DrawGeometry(RenderedPath.Fill, pathPen, this.carouselPathHelper.Geometry);
            }
        }
        #endregion

        private void SelectedItemChanged(object sender, RoutedEventArgs e)
        {
            //this.BringItemIntoView(e.OriginalSource as UIElement, true);
        }

        private PathFractionRangeHandler m_PathFractionRangeHandler = new PathFractionRangeHandler();

        #region VirtualizingPanelHandler

        internal VisibleItemsHandler GetCurrentItemPathArrangement()
        {
            if (this.carouselPathHelper == null)
            {
                return null;
            }
            VisibleItemsHandler arrangement = new VisibleItemsHandler(this.carouselPathHelper.GetVisiblePathFractionCount());
            if (arrangement.Count > 0)
            {
                foreach (VisiblePanelItem pair in this.m_PathFractionRangeHandler)
                {
                    double pathFraction = GetPathFraction(pair.Child);
                    if (CarouselPathHelper.IsVisible(pathFraction))
                    {
                        int controlPointIndex = this.carouselPathHelper.GetPathFractionIndex(pathFraction);
                        if (controlPointIndex != -1)
                        {
                            arrangement.SetItemAtPosition(controlPointIndex - 1, pair);
                        }
                    }
                }
            }
            return arrangement;
        }

        private void FillPathWithItems(int numberOfItems)
        {
            if (((numberOfItems > 0) && this.ShouldLoadItems) && !this.m_PathFractionRangeHandler.HasVisibleItems)
            {
                int movementDisplacement = Math.Min(this.ItemsPerPage, numberOfItems);
                int itemsAfter = CarouselPanelHelperMethods.GetItemCountAfter(this.m_PathFractionRangeHandler, this.CarouselPanelHelper.ItemsCount);
                int itemsBefore = CarouselPanelHelperMethods.GetItemCountBefore(this.m_PathFractionRangeHandler);
                if (itemsAfter >= movementDisplacement)
                {
                    this.MoveBy(movementDisplacement);
                }
                else if (itemsBefore >= movementDisplacement)
                {
                    this.MoveBy(-movementDisplacement);
                }
            }
        }

        public void MoveBy(int displacement)
        {
            if (displacement != 0)
            {
                this.MoveItemInternallyBy(displacement);
                this.UpdatePanelOffset(displacement);
            }
        }


        #endregion

        #region CleanUp and GenerateItems

        private void CleanUpItems()
        {
            if (base.IsItemsHost)
            {
                this.CleanGeneratedItems();
            }
            this.m_PathFractionRangeHandler.ClearCleanUp();
        }

        private void CleanGeneratedItems()
        {
            UIElementCollection internalChildren = base.InternalChildren;
            IItemContainerGenerator generator = base.ItemContainerGenerator;
            foreach (VisiblePanelItem pair in this.m_PathFractionRangeHandler.ToCleanUp)
            {
                GeneratorPosition childGeneratorPos = generator.GeneratorPositionFromIndex(pair.Index);
                generator.Remove(childGeneratorPos, 1);
                int index = base.InternalChildren.IndexOf(pair.Child);
                base.RemoveInternalChildRange(index, 1);
            }
        }

        private void UpdateVisibleItems()
        {
            if (base.IsItemsHost)
            {
                this.GenerateItems();
            }
            else
            {
                this.UpdateChildPairs();
            }
        }

        private void UpdateChildPairs()
        {
            if (!base.IsItemsHost)
            {
                foreach (VisiblePanelItem pair in this.m_PathFractionRangeHandler)
                {
                    pair.Child = base.InternalChildren[pair.Index];
                }
            }
        }

        private void GenerateItems()
        {
            if (base.IsItemsHost)
            {
                this.GenerateChildrenWithItemContainerGenerator();
            }
        }

        private void GenerateChildrenWithItemContainerGenerator()
        {
            if (this.m_PathFractionRangeHandler.HasVisibleItems)
            {
                UIElementCollection internalChildren = base.InternalChildren;
                IItemContainerGenerator generator = base.ItemContainerGenerator;
                foreach (VisiblePanelItem item in this.m_PathFractionRangeHandler)
                {
                    GeneratorPosition startPos = generator.GeneratorPositionFromIndex(item.Index);
                    using (generator.StartAt(startPos, GeneratorDirection.Forward, true))
                    {
                        bool newlyRealized;
                        UIElement child = generator.GenerateNext(out newlyRealized) as UIElement;
                        if (newlyRealized)
                        {
                            item.Child = child;
                            base.InsertInternalChild(base.InternalChildren.Count, child);
                            generator.PrepareItemContainer(child);
                        }
                        continue;
                    }
                }
            }
        }
        #endregion

        #region UpdateVisualization

        private void UpdateVisualization()
        {
            if (this.m_PathFractionRangeHandler.HasVisibleItems)
            {
                this.UpdateZorder();
                foreach (VisiblePanelItem pair in this.m_PathFractionRangeHandler)
                {
                    ScaleTransform scaleTransform;
                    SkewTransform skewAngleXTransform;
                    SkewTransform skewAngleYTransform;
                    UIElement item = pair.Child;
                    item.RenderTransformOrigin = new Point(0.5, 0.5);
                    this.UpdateOpacityFractions(item);
                    this.UpdateSkewAngleXFractions(item, out skewAngleXTransform);
                    this.UpdateSkewAngleYFractions(item, out skewAngleYTransform);
                    this.UpdateScaleFractions(item, out scaleTransform);
                    TranslateTransform translateTransform = new TranslateTransform(item.RenderTransform.Value.OffsetX, item.RenderTransform.Value.OffsetY);
                    MatrixTransform newItemTransform = new MatrixTransform(((scaleTransform.Value * skewAngleXTransform.Value) * skewAngleYTransform.Value) * translateTransform.Value);
                    item.RenderTransform = newItemTransform;
                }
            }
        }

        #region ZOrder

        private void UpdateZorder()
        {
            UIElement child = this.FindClosestElementToPathFraction(this.TopItemPosition);
            //base.SetValue(TopContainerPropertyKey, child);
            if (child != null)
            {
                int childIndex = base.Children.IndexOf(child);
                int zIndexCounter = base.Children.Count - 1;
                for (int i = childIndex; i >= 0; i--)
                {
                    Panel.SetZIndex(base.Children[i], zIndexCounter);
                    zIndexCounter--;
                }
                for (int i = childIndex + 1; i < base.Children.Count; i++)
                {
                    Panel.SetZIndex(base.Children[i], zIndexCounter);
                    zIndexCounter--;
                }
            }
        }

        internal UIElement FindClosestElementToPathFraction(double point)
        {
            double currentShortestDistance = double.PositiveInfinity;
            UIElement currentClosestElement = null;
            foreach (UIElement element in base.InternalChildren)
            {
                double distanceToPoint = Math.Abs((double)(GetPathFraction(element) - point));
                if (distanceToPoint < currentShortestDistance)
                {
                    currentShortestDistance = distanceToPoint;
                    currentClosestElement = element;
                }
            }
            if (currentClosestElement == null)
            {
                return null;
            }
            return currentClosestElement;
        }
        #endregion

        #region Opacity

        public bool OpacityEnabled
        {
            get { return (bool)GetValue(OpacityEnabledProperty); }
            set { SetValue(OpacityEnabledProperty, value); }
        }

        public static readonly DependencyProperty OpacityEnabledProperty =
            DependencyProperty.Register("OpacityEnabled", typeof(bool), typeof(CarouselPanel), new PropertyMetadata(true));

        public PathFractionCollection OpacityFractions
        {
            get { return (PathFractionCollection)GetValue(OpacityFractionsProperty); }
            set { SetValue(OpacityFractionsProperty, value); }
        }

        public static readonly DependencyProperty OpacityFractionsProperty =
            DependencyProperty.Register("OpacityFractions", typeof(PathFractionCollection), typeof(CarouselPanel), new PropertyMetadata(null));

        internal PathFractionCollection internalOpacityFractions;
        internal PathFractionCollection InternalOpacityFractions
        {
            get
            {
                if (CanResetPathFractionCollection(this.internalOpacityFractions, this.OpacityFractions))
                {
                    this.internalOpacityFractions = null;
                }
                if (this.internalOpacityFractions == null)
                {
                    this.internalOpacityFractions = this.OpacityFractions ?? CarouselPanelHelperMethods.GetDefaultOpacityFractionsCollection();
                }
                return this.internalOpacityFractions;
            }
        }

        private void UpdateOpacityFractions(UIElement item)
        {
            if ((!this.OpacityEnabled || (this.InternalOpacityFractions == null)) || (this.InternalOpacityFractions.Count <= 0))
            {
                item.Opacity = 1.0;
            }
            else
            {
                double currentOpacity = GetCurrentEffectValue(item, this.InternalOpacityFractions);
                item.Opacity = currentOpacity;
            }

            if (CarouselPanel.GetPathFraction(item) == 0 || CarouselPanel.GetPathFraction(item) == 1)
            {
                item.Opacity = 0;
            }
        }
        #endregion

        #region Scaling

        public bool ScalingEnabled
        {
            get { return (bool)GetValue(ScalingEnabledProperty); }
            set { SetValue(ScalingEnabledProperty, value); }
        }

        public static readonly DependencyProperty ScalingEnabledProperty =
            DependencyProperty.Register("ScalingEnabled", typeof(bool), typeof(CarouselPanel), new PropertyMetadata(true));

        public PathFractionCollection ScaleFractions
        {
            get { return (PathFractionCollection)GetValue(ScaleFractionsProperty); }
            set { SetValue(ScaleFractionsProperty, value); }
        }

        public static readonly DependencyProperty ScaleFractionsProperty =
            DependencyProperty.Register("ScaleFractions", typeof(PathFractionCollection), typeof(CarouselPanel), new PropertyMetadata(null));

        internal PathFractionCollection internalScaleFractions;
        internal PathFractionCollection InternalScaleFractions
        {
            get
            {
                if (CanResetPathFractionCollection(this.internalScaleFractions, this.OpacityFractions))
                {
                    this.internalScaleFractions = null; 
                }
                if (this.internalScaleFractions == null)
                {
                    this.internalScaleFractions = this.ScaleFractions ?? CarouselPanelHelperMethods.GetDefaultScaleFractionsCollection();
                }
                return this.internalScaleFractions;
            }
        }

        private void UpdateScaleFractions(UIElement item, out ScaleTransform transform)
        {
            transform = new ScaleTransform(1.0, 1.0);
            if ((!this.ScalingEnabled || (this.InternalScaleFractions == null)) || (this.InternalScaleFractions.Count <= 0))
            {
                transform = new ScaleTransform();
            }
            else
            {
                double currentScale = GetCurrentEffectValue(item, this.InternalScaleFractions);
                transform = new ScaleTransform(currentScale, currentScale);
            }
        }
        
        #endregion

        #region SkewAngleX

        public bool SkewAngleXEnabled
        {
            get { return (bool)GetValue(SkewAngleXEnabledProperty); }
            set { SetValue(SkewAngleXEnabledProperty, value); }
        }

        public static readonly DependencyProperty SkewAngleXEnabledProperty =
            DependencyProperty.Register("SkewAngleXEnabled", typeof(bool), typeof(CarouselPanel), new PropertyMetadata(true));

        public PathFractionCollection SkewAngleXFractions
        {
            get { return (PathFractionCollection)GetValue(SkewAngleXFractionsProperty); }
            set { SetValue(SkewAngleXFractionsProperty, value); }
        }

        public static readonly DependencyProperty SkewAngleXFractionsProperty =
            DependencyProperty.Register("SkewAngleXFractions", typeof(PathFractionCollection), typeof(CarouselPanel), new PropertyMetadata(null));

        internal PathFractionCollection internalSkewAngleXFractions;
        internal PathFractionCollection InternalSkewAngleXFractions
        {
            get
            {
                if (CanResetPathFractionCollection(this.internalSkewAngleXFractions, this.SkewAngleXFractions))
                {
                    this.internalSkewAngleXFractions = null;
                }
                if (this.internalSkewAngleXFractions == null)
                {
                    this.internalSkewAngleXFractions = this.SkewAngleXFractions ?? CarouselPanelHelperMethods.GetDefaultSkewAngleXFractionsCollection();
                }
                return this.internalSkewAngleXFractions;
            }
        }

        private void UpdateSkewAngleXFractions(UIElement item, out SkewTransform transform)
        {
            if ((!this.SkewAngleXEnabled || (this.InternalSkewAngleXFractions == null)) || (this.InternalSkewAngleXFractions.Count <= 0))
            {
                transform = new SkewTransform();
            }
            else
            {
                double currentSkewAngleX = GetCurrentEffectValue(item, this.InternalSkewAngleXFractions);
                transform = new SkewTransform(currentSkewAngleX, 0.0);
            }
        }
        #endregion

        #region SkewAngleY

        public bool SkewAngleYEnabled
        {
            get { return (bool)GetValue(SkewAngleYEnabledProperty); }
            set { SetValue(SkewAngleYEnabledProperty, value); }
        }

        public static readonly DependencyProperty SkewAngleYEnabledProperty =
            DependencyProperty.Register("SkewAngleYEnabled", typeof(bool), typeof(CarouselPanel), new PropertyMetadata(true));

        public PathFractionCollection SkewAngleYFractions
        {
            get { return (PathFractionCollection)GetValue(SkewAngleYFractionsProperty); }
            set { SetValue(SkewAngleYFractionsProperty, value); }
        }

        public static readonly DependencyProperty SkewAngleYFractionsProperty =
            DependencyProperty.Register("SkewAngleYFractions", typeof(PathFractionCollection), typeof(CarouselPanel), new PropertyMetadata(null));

        internal PathFractionCollection internalSkewAngleYFractions;
        internal PathFractionCollection InternalSkewAngleYFractions
        {
            get
            {
                if (CanResetPathFractionCollection(this.internalSkewAngleYFractions, this.SkewAngleYFractions))
                {
                    this.internalSkewAngleYFractions = null;
                }
                if (this.internalSkewAngleYFractions == null)
                {
                    this.internalSkewAngleYFractions = this.SkewAngleYFractions ?? CarouselPanelHelperMethods.GetDefaultSkewAngleYFractionsCollection();
                }
                return this.internalSkewAngleYFractions;
            }
        }

        private void UpdateSkewAngleYFractions(UIElement item, out SkewTransform transform)
        {
            if ((!this.SkewAngleYEnabled || (this.InternalSkewAngleYFractions == null)) || (this.InternalSkewAngleYFractions.Count <= 0))
            {
                transform = new SkewTransform();
            }
            else
            {
                double currentSkewAngleY = GetCurrentEffectValue(item, this.InternalSkewAngleYFractions);
                transform = new SkewTransform(currentSkewAngleY, 0.0);
            }
        }
        #endregion

        internal static bool CanResetPathFractionCollection(PathFractionCollection internalStops, PathFractionCollection publicStops)
        {
            return (((internalStops != null) && (publicStops != null)) && (internalStops != publicStops));
        }

        private static double GetCurrentEffectValue(UIElement item, PathFractionCollection effectCollection)
        {
            double currentPathFraction = GetPathFraction(item);
            FractionValue leftPoint = null;
            FractionValue rightPoint = null;
            effectCollection.FindNearestPoints(currentPathFraction, out leftPoint, out rightPoint);
            if (leftPoint == null)
            {
                return rightPoint.Value;
            }
            if (rightPoint == null)
            {
                return leftPoint.Value;
            }
            return CalculateChange(currentPathFraction, leftPoint, rightPoint);
        }

        internal static double CalculateChange(double currentPathFraction, FractionValue stop1, FractionValue stop2)
        {
            FractionValue biggerPoint;
            FractionValue smallerPoint;
            if (stop1.Value > stop2.Value)
            {
                biggerPoint = stop1;
                smallerPoint = stop2;
            }
            else
            {
                biggerPoint = stop2;
                smallerPoint = stop1;
            }
            double bigSide = biggerPoint.Value - smallerPoint.Value;
            double smallSide = ((currentPathFraction - smallerPoint.Fraction) * bigSide) / (biggerPoint.Fraction - smallerPoint.Fraction);
            return (smallerPoint.Value + smallSide);
        }
        #endregion

        internal int MaxPanelOffset = 0;
        internal int ViewPanelOffset = 0;
        internal int PanelOffset;

        internal void UpdatePanelOffset(int displacement)
        {
            if (displacement != 0)
            {
                int minOffset = 1;
                int maxOffset = (this.MaxPanelOffset - this.ViewPanelOffset) - 1;

                int Offset = CarouselPanelHelperMethods.CoerceValueBetweenRange(this.PanelOffset + displacement, minOffset, maxOffset);
                this.PanelOffset = Offset;
            }
        }

        private void CheckPanelOffset()
        {
            this.SetMaximumandViewPanelOffset();
            int newOffset = GetOffsetFromCurrentArrangement(this.GetCurrentItemPathArrangement());
            this.PanelOffset = newOffset;
        }

        private void SetMaximumandViewPanelOffset()
        {
            int maxPanelOffset = this.CalculateMaximumOffset();

            if (this.MaxPanelOffset != maxPanelOffset)
            {
                this.MaxPanelOffset = maxPanelOffset;
            }
            if (this.ItemsPerPage != this.ViewPanelOffset)
            {
                this.ViewPanelOffset = this.ItemsPerPage;
            }
        }

        private int CalculateMaximumOffset()
        {
            int maxPanelOffset = 0;
            if (this.CarouselPanelHelper.ItemsCount > 0)
            {
                maxPanelOffset = (this.CarouselPanelHelper.ItemsCount + this.ItemsPerPage) + this.ItemsPerPage;
            }
            return maxPanelOffset;
        }

        private static int GetOffsetFromCurrentArrangement(VisibleItemsHandler arrangement)
        {
            int offset = 0;
            if (arrangement.GetUsedPositions() > 0)
            {
                int largestIndex = arrangement.GetLargestItemIndex();
                int freePositionsOfTheLeft = arrangement.GetFreePositionsLeft();
                offset = (largestIndex + freePositionsOfTheLeft) + 1;
            }
            return (int)offset;
        }

        private static readonly DependencyProperty ItemPathFractionManagerProperty;
        internal static PathFractionManager GetPathFractionManager(UIElement element)
        {
            return (PathFractionManager)element.GetValue(ItemPathFractionManagerProperty);
        }

        internal static void SetPathFractionManager(UIElement element, PathFractionManager value)
        {
            element.SetValue(ItemPathFractionManagerProperty, value);
        }

        internal static void SetPathFraction(UIElement element, double value)
        {
            element.SetValue(PathFractionProperty, value);
        }

        internal static double GetPathFraction(UIElement element)
        {
            if (element == null)
            {
                return -1.0;
            }
            return (double)element.GetValue(PathFractionProperty);
        }
        //public double PanelOffset
        //{
        //    get
        //    {

        //    }
        //}
        public void BringItemIntoView(UIElement item, bool isItemSelected)
        {
            if (base.Children.Contains(item))
            {
                //this.FinishRunningAnimation();
                int moveBy = this.GetMovementOffsetFromTopElement(item);
                if (isItemSelected)
                {
                    //this.BringIntoViewItem = item;
                }
                this.MoveItemInternallyBy(moveBy);
            }
        }

        private int GetMovementOffsetFromTopElement(UIElement element)
        {
            double pathFraction = GetPathFraction(element);
            if (pathFraction == -1.0)
            {
                return 0;
            }
            int itemPosition = this.carouselPathHelper.GetPathFractionIndex(pathFraction);
            return (this.carouselPathHelper.TopElementPathFractionIndex - itemPosition);
        }

        VirtualizingPanelHandler CurrentVirtualizingPanelHandler;
        VirtualizingPanelHandler OldVirtualizingPanelHandler;
        VirtualizingPanelHandler NewVirtualizingPanelHandler;

        internal void MoveItemInternallyBy(int displacement)
        {
            if (this.CurrentVirtualizingPanelHandler != null)
            {
                VirtualizingPanelItemMoveHandler _VirtualizingPanelHandler = this.CurrentVirtualizingPanelHandler as VirtualizingPanelItemMoveHandler;
                if ((_VirtualizingPanelHandler != null) && _VirtualizingPanelHandler.IsOpposite(displacement))
                {
                    _VirtualizingPanelHandler.Reverse();
                    return;
                }
            }
            else
            {
                this.FinishItemMovements();
                int newDisplacement = this.CoerceDisplacement(displacement);
                if (newDisplacement != 0)
                {
                    if ((this.NewVirtualizingPanelHandler == null) && base.IsInitialized)
                    {
                        this.NewVirtualizingPanelHandler = new VirtualizingPanelItemMoveHandler(newDisplacement, this.CarouselPanelHelper);
                        this.Invalidate(false);
                    }
                }
            }
        }

        internal void Invalidate(bool invalidateVisual)
        {
            //this.InvalidatePathOffset();
            if (invalidateVisual)
            {
                base.InvalidateVisual();
            }
            else
            {
                base.InvalidateMeasure();
                base.InvalidateArrange();
            }
        }

        private void Start_ItemMovement()
        {
            if (this.NewVirtualizingPanelHandler != null)
            {
                this.PrepareItemsToMove(this.NewVirtualizingPanelHandler);
                if (this.CurrentVirtualizingPanelHandler == null)
                {
                    CompositionTarget.Rendering += new EventHandler(this.CompositionTargetRendering);
                }
                this.OldVirtualizingPanelHandler = this.NewVirtualizingPanelHandler;
                this.NewVirtualizingPanelHandler = null;
            }
        }

        private void PrepareItemsToMove(VirtualizingPanelHandler _VirtualizingPanelHandler)
        {
            List<VisiblePanelItem> itemsToAnimate = this.m_PathFractionRangeHandler.ToList<VisiblePanelItem>();
            VirtualizingPanelItemMoveHandler _VirtualizingPanelItemMoveHandler = _VirtualizingPanelHandler as VirtualizingPanelItemMoveHandler;
            if ((_VirtualizingPanelItemMoveHandler != null) && (_VirtualizingPanelItemMoveHandler.PathDisplacement < 0))
            {
                itemsToAnimate.Reverse();
            }
            _VirtualizingPanelHandler.Duration = new TimeSpan(0, 0, 0, 0, 300);// this.ItemsMovementAnimationDuration;
            foreach (VisiblePanelItem currentElement in itemsToAnimate)
            {
                currentElement.Child.RenderTransformOrigin = new Point(0.5, 0.5);
                _VirtualizingPanelHandler.AddItemToMove(currentElement);
            }
        }

        private int CoerceDisplacement(int displacement)
        {
            int newDisplacement = displacement;
            VisibleItemsHandler arrangement = this.GetCurrentItemPathArrangement();
            int itemsAfter = GetItemCountAfter(this.m_PathFractionRangeHandler, this.CarouselPanelHelper.ItemsCount);
            int itemsBefore = GetItemCountBefore(this.m_PathFractionRangeHandler);
            if (arrangement == null)
            {
                return 0;
            }
            if (displacement < 0)
            {
                int used = arrangement.GetUsedPositions();
                int fromLeft = arrangement.GetFreePositionsLeft();
                int max = ((used + fromLeft) - 1) + itemsBefore;
                return -Math.Min(max, -displacement);
            }
            if (displacement > 0)
            {
                int used = arrangement.GetUsedPositions();
                int fromRight = arrangement.GetFreePositionsRight();
                int max = ((used + fromRight) - 1) + itemsAfter;
                newDisplacement = Math.Min(max, displacement);
            }
            return newDisplacement;
        }

        internal static int GetItemCountAfter(PathFractionRangeHandler range, int itemCount)
        {
            if (range.LastVisibleItemIndex >= itemCount)
            {
                return 0;
            }
            return ((itemCount - range.LastVisibleItemIndex) - 1);
        }

        internal static int GetItemCountBefore(PathFractionRangeHandler range)
        {
            if (range.FirstVisibleItemIndex < 0)
            {
                return 0;
            }
            return range.FirstVisibleItemIndex;
        }

        private void InitializeItemMovement()
        {
            if (this.NewVirtualizingPanelHandler != null)
            {
                VisibleRangeAction action;
                LinkedList<VisiblePanelItem> pairs;
                this.NewVirtualizingPanelHandler.Initialize(this.carouselPathHelper, this.GetCurrentItemPathArrangement());
                this.NewVirtualizingPanelHandler.CalculateItemsToAdd(out action, out pairs);
                this.m_PathFractionRangeHandler.UpdateVisibleRange(action, pairs);
            }
        }

        private void FinishItemMovements()
        {
            if (this.CurrentVirtualizingPanelHandler != null)
            {
                this.UpdateVisualization();
                CompositionTarget.Rendering -= new EventHandler(this.CompositionTargetRendering);
                this.CurrentVirtualizingPanelHandler.EndItemMovement();
                IList<VisiblePanelItem> itemsToRemove = this.CurrentVirtualizingPanelHandler.GetItemsToRemoveEndofArrangeOverride();
                if ((itemsToRemove != null) && (itemsToRemove.Count > 0))
                {
                    foreach (VisiblePanelItem currentElement in itemsToRemove)
                    {
                        SetPathFraction(currentElement.Child, -1.0);
                    }
                }
                this.m_PathFractionRangeHandler.ScheduleClean(itemsToRemove);
                this.CurrentVirtualizingPanelHandler = null;
                //this.HandleBringIntoViewItem();
                this.CheckPanelOffset();
                this.Invalidate(false);
            }
        }

        private void CompositionTargetRendering(object sender, EventArgs e)
        {
            RenderingEventArgs renderArgs = (RenderingEventArgs)e;

            if (this.OldVirtualizingPanelHandler != null)
            {
                this.CurrentVirtualizingPanelHandler = this.OldVirtualizingPanelHandler;
                this.OldVirtualizingPanelHandler = null;
                this.CurrentVirtualizingPanelHandler.BeginItemMovement(renderArgs.RenderingTime);
                //base.SetValue(IsAnimatingPropertyKey, true);
                //if (this.IsSelectedTopItem)
                //{
                //    this.IsSelectedTopItem = false;
                //}
            }

            if (this.CurrentVirtualizingPanelHandler != null)
            {
                this.CurrentVirtualizingPanelHandler.Update(renderArgs.RenderingTime);
                this.UpdateVisualization();

                if (this.CurrentVirtualizingPanelHandler.State == ItemMovementState.Finished)
                {
                    this.FinishItemMovements();
                }
            }
        }
    }
}
