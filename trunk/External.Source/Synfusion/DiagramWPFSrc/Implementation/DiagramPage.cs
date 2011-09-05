// <copyright file="DiagramPage.cs" company="Syncfusion">
// Copyright Syncfusion Inc. 2001 - 2011. All rights reserved.
// Use of this code is subject to the terms of our license.
// A copy of the current license can be obtained at any time by e-mailing
// licensing@syncfusion.com. Any infringement will be prosecuted under
// applicable laws. 
// </copyright>

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Xml;
using System.Collections.ObjectModel;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Threading;

namespace Syncfusion.Windows.Diagram
{
    /// <summary>
    /// Represents the diagram page .
    /// <para> The DiagramPage is just a container to hold the objects(nodes and connectors) added through model.
    /// The DiagramView uses the page to display the diagram objects.
    /// </para>
    /// </summary>
#if SyncfusionFramework4_0
    [DesignTimeVisible(false)]
#endif
    public partial class DiagramPage : VirtualizingPanel, IDiagramPage, INotifyPropertyChanged
    {
        #region Class variables

        internal double Top
        {
            get { return (double)GetValue(TopProperty); }
            set { SetValue(TopProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Top.  This enables animation, styling, binding, etc...
        internal static readonly DependencyProperty TopProperty =
            DependencyProperty.Register("Top", typeof(double), typeof(DiagramPage), new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.AffectsParentArrange | FrameworkPropertyMetadataOptions.AffectsParentMeasure, OnTopChanged));

        internal double Bottom
        {
            get { return (double)GetValue(BottomProperty); }
            set { SetValue(BottomProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Bottom.  This enables animation, styling, binding, etc...
        internal static readonly DependencyProperty BottomProperty =
            DependencyProperty.Register("Bottom", typeof(double), typeof(DiagramPage), new UIPropertyMetadata(0d));

        internal double Right
        {
            get { return (double)GetValue(RightProperty); }
            set { SetValue(RightProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Right.  This enables animation, styling, binding, etc...
        internal static readonly DependencyProperty RightProperty =
            DependencyProperty.Register("Right", typeof(double), typeof(DiagramPage), new UIPropertyMetadata(0d));

        internal double Left
        {
            get { return (double)GetValue(LeftProperty); }
            set { SetValue(LeftProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Left.  This enables animation, styling, binding, etc...
        internal static readonly DependencyProperty LeftProperty =
            DependencyProperty.Register("Left", typeof(double), typeof(DiagramPage), new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.AffectsParentArrange | FrameworkPropertyMetadataOptions.AffectsParentMeasure, OnLeftChanged));

        private static void OnTopChanged(DependencyObject d, DependencyPropertyChangedEventArgs evtArgs)
        {
            DiagramPage page = d as DiagramPage;
            if (page.dview != null)
            {
                //page.dview.UpdateLayout();
                ((page.dview.Scrollviewer as ScrollViewer).Content as ScrollableGrid)._Offset.Y += (double)evtArgs.NewValue - (double)evtArgs.OldValue;
                //page.dview.Scrollviewer.ScrollToVerticalOffset(page.dview.Scrollviewer.VerticalOffset + (double)evtArgs.NewValue - (double)evtArgs.OldValue);
            }
        }

        private static void OnLeftChanged(DependencyObject d, DependencyPropertyChangedEventArgs evtArgs)
        {
            DiagramPage page = d as DiagramPage;
            if (page.dview != null)
            {
                //page.dview.UpdateLayout();
                ((page.dview.Scrollviewer as ScrollViewer).Content as ScrollableGrid)._Offset.X += (double)evtArgs.NewValue - (double)evtArgs.OldValue;
                //page.dview.Scrollviewer.ScrollToHorizontalOffset(page.dview.Scrollviewer.HorizontalOffset + (double)evtArgs.NewValue - (double)evtArgs.OldValue);
            }
        }

        //private DiagramViewGrid dvg;

        ///// <summary>
        ///// Used to check if the page is loaded.
        ///// </summary>
        //private bool loaded = false;


        /// <summary>
        /// Used to refer to the diagram control instance.
        /// </summary>
        //private DiagramControl diagctrl;

        ///// <summary>
        ///// Used to store the int value.
        ///// </summary>
        //private int i = 0;

        ///// <summary>
        ///// Used to refer to the vertical offset. Default value is 25d.
        ///// </summary>
        //private double vertoffset = 25d;

        ///// <summary>
        ///// Used to refer to the horizontal offset
        ///// </summary>
        //private double horoffset = 25d;

        ///// <summary>
        ///// Used to check if node is dropped.
        ///// </summary>
        //internal bool nodedropped = false;

        ///// <summary>
        ///// Used to refer to the execution instance.
        ///// </summary>
        //private bool exeonce = false;

        /// <summary>
        /// Used to check if the node transformation is done.
        /// </summary>
        //private bool transformed = false;

        ///// <summary>
        ///// Used to refer to the not first exe value.
        ///// </summary>
        //private bool notfirstexe = false;

        /// <summary>
        /// Used to store a static object.
        /// </summary>
        internal static object o;

        /// <summary>
        /// Used to store the connector type.
        /// </summary>
        private ConnectorType m_connectionType = ConnectorType.Orthogonal;

        /// <summary>
        /// Used to check if executed.
        /// </summary>
        //private bool isexe = false;

        ///// <summary>
        ///// Used to refer to the unit changed event.
        ///// </summary>
        //private bool m_unitchanged = false;

        ///// <summary>
        ///// Used to refer to the not pixel offset.
        ///// </summary>
        //private bool isnotpixeloffset = false;

        /// <summary>
        /// Used to check if unit is changed.
        /// </summary>
        //private bool unitchanged = false;

        /// <summary>
        /// Used to store the page.
        /// </summary>
        //private static DiagramPage page;

        /// <summary>
        /// Used to store selection list
        /// </summary>
        private NodeCollection m_selectionList;

        /// <summary>
        /// Used to store the View instance.
        /// </summary>
        private DiagramView dview;

        ///// <summary>
        ///// Used to store the units
        ///// </summary>
        //private MeasureUnits o_unit;

        ///// <summary>
        ///// Used to store boolean value on executing once.
        ///// </summary>
        //private bool once = false;

        ///// <summary>
        ///// Used to store boolean value on reaching zero once.
        ///// </summary>
        //private bool oncezero = false;

        ///// <summary>
        ///// Used to store the units temporarily.
        ///// </summary>
        //private MeasureUnits temp;

        ///// <summary>
        ///// Used to store the unit details 
        ///// </summary>
        //private bool isnotpixelwh = false;

        /// <summary>
        /// Used to store the diagram control.
        /// </summary>
        private DiagramControl dc;

        ///// <summary>
        ///// Used to store the changed unit.
        ///// </summary>
        //private bool isunitchanged = false;

        ///// <summary>
        ///// Used to store the measure units.
        ///// </summary>
        //private static MeasureUnits m_units;

        /// <summary>
        /// Used to store the  tree orientation
        /// </summary>
        private TreeOrientation oref;

        /// <summary>
        /// Used to store the horizontal spacing reference.
        /// </summary>
        private double href;

        /// <summary>
        /// Used to store the vertical spacing reference.
        /// </summary>
        private double vref;

        /// <summary>
        /// Used to store the space between the sub-trees.
        /// </summary>
        private double sref;

        /// <summary>
        /// Used to store the layout type.
        /// </summary>
        private LayoutType ltref;

        /// <summary>
        /// Used to store the resize bool value.
        /// </summary>
        private Style styleref;

        /// <summary>
        /// Used to store the line style reference value.
        /// </summary>
        private Style lstyleref;

        ///// <summary>
        ///// Used to store the minimumx.
        ///// </summary>
        //private double minimumX = 0;

        ///// <summary>
        ///// Used to store the minimumy.
        ///// </summary>
        //private double minimumY = 0;

        ///// <summary>
        ///// Used to store the minleftx value.
        ///// </summary>
        //private double minleftX = 0;

        ///// <summary>
        ///// Used to store the mintopY value.
        ///// </summary>
        //private double mintopY = 0;

        ///// <summary>
        ///// Used to store the drag left value.
        ///// </summary>
        //private double dleft = 0;

        ///// <summary>
        ///// Used to store the drag top value.
        ///// </summary>
        //private double dtop = 0;

        ///// <summary>
        ///// Used to store the IsPositive value.
        ///// </summary>
        //private bool pos = false;

        ///// <summary>
        /////  Used to store the GreaterThanZero value.
        ///// </summary>
        //private bool gtz = false;

        ///// <summary>
        /////  Used to store the IsPositiveY bool value.
        ///// </summary>
        //private bool posy = false;

        ///// <summary>
        /////  Used to store the GreaterThanZeroY bool value.
        ///// </summary>
        //private bool gtzy = false;

        ///// <summary>
        /////  Used to store the minx.
        ///// </summary>
        //private double cminx = 0;

        ///// <summary>
        /////  Used to store the miny
        ///// </summary>
        //private double cminy = 0;

        ///// <summary>
        /////  Used to store the  current minx
        ///// </summary>
        //private double curminx = 0;

        ///// <summary>
        /////  Used to store the current miny
        ///// </summary>
        //private double curminy = 0;

        ///// <summary>
        /////  Used to store the constant miny
        ///// </summary>
        //private double cmy = 0;

        ///// <summary>
        ///// Used to store the constant minx
        ///// </summary>
        //private double cmx = 0;

        ///// <summary>
        ///// Used to store the horizontal offset on adding connectors.
        ///// </summary>
        //private double hval = 0;

        ///// <summary>
        ///// Used to store the vertical offset on adding connectors.
        ///// </summary>
        //private double vval = 0;

        ///// <summary>
        ///// Used to check if connector is dropped.
        ///// </summary>
        //private bool linedrop = false;

        ///// <summary>
        ///// Used to store mouse scroll count
        ///// </summary>
        //private int scrollcount = 0;

        ///// <summary>
        ///// Used to store max value.
        ///// </summary>
        //private double max = 0;

        ///// <summary>
        ///// Used to store double value.
        ///// </summary>
        //private double s = 0;

        ///// <summary>
        ///// Used to store the least negative offsetx.
        ///// </summary>
        //private double leastx = 0;

        ///// <summary>
        ///// Used to store the least negative offsety.
        ///// </summary>
        //private double leasty = 0;

        ///// <summary>
        ///// Checks if the least values are calculated once.
        ///// </summary>
        //private bool isexeonce = false;

        ///// <summary>
        ///// Checks if the unit got changed.
        ///// </summary>
        //private bool unitchan = false;

        ///// <summary>
        ///// Used to store the horizontal scollbar width.
        ///// </summary>
        //private double sx = 0;

        ///// <summary>
        ///// Used to store the vertical scollbar width.
        ///// </summary>
        //private double sy = 0;

        #endregion

        #region Initialization

        static DiagramPage()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DiagramPage), new FrameworkPropertyMetadata(typeof(DiagramPage)));
        }

        internal List<UIElement> AllChildren;
        internal ObservableCollection<UIElement> RealizedChildren;
        /// <summary>
        /// Initializes a new instance of the <see cref="DiagramPage"/> class.
        /// </summary>
        public DiagramPage()
        {

            this.AllChildren = new List<UIElement>();
            this.RealizedChildren = new ObservableCollection<UIElement>();
            //DiagramViewGrid grid = new DiagramViewGrid();
            this.AllowDrop = true;
            this.Focus();
            this.Focusable = true;
            FocusManager.SetIsFocusScope(this, false);
            //DiagramViewGrid dvg = new DiagramViewGrid();
            //this.Children.Add(dvg);
            this.Loaded += new RoutedEventHandler(DiagramPage_Loaded);
            this.Unloaded += new RoutedEventHandler(DiagramPage_Unloaded);
            if (!this.CaptureMouse())
            {
                this.Background = Brushes.Transparent;
            }
            //this.SizeChanged += new SizeChangedEventHandler(DiagramPage_SizeChanged);

            //Style sty = new Style(typeof(System.Windows.Shapes.Path));
            //Setter bor = new Setter(System.Windows.Shapes.Path.StrokeProperty, Brushes.Red);
            //Setter borT = new Setter(System.Windows.Shapes.Path.StrokeThicknessProperty, 4d);
            //sty.Setters.Add(bor);
            //sty.Setters.Add(borT);
            //this.CustomPathStyle = sty;
            //ResourceDictionary rs = new ResourceDictionary();
            //rs.Source = new Uri("/Syncfusion.Diagram.WPF;component/Themes/DiagramPageStyle.xaml", UriKind.RelativeOrAbsolute);
            //this.Style = rs["DiagramPageStyle"] as Style;
        }

        void DiagramPage_Unloaded(object sender, RoutedEventArgs e)
        {
            if (dview != null && dview.Scrollviewer != null)
            {
                //dview.Scrollviewer.ScrollChanged -= Scrollviewer_ScrollChanged;
            }
        }

        //void DiagramPage_SizeChanged(object sender, SizeChangedEventArgs e)
        //{
        //    if (dvg != null && dview!=null && dview.Scrollviewer!=null)
        //    {
        //        //double width =  Math.Max(dview.Scrollviewer.ActualWidth, dview.Scrollviewer.ExtentWidth);
        //        //double height =  Math.Max(dview.Scrollviewer.ActualHeight, dview.Scrollviewer.ExtentHeight);
        //        //dvg.Arrange(new Rect(0, 0, width, height));
        //        if (dview.IsPageEditable)
        //        {
        //            dvg.InvalidateVisual();
        //        }
        //    }
        //}

        /// <summary>
        /// Is called when the diagram page gets loaded.
        /// </summary>
        /// <param name="sender">Diagram page</param>
        /// <param name="e">Event args</param>
        private void DiagramPage_Loaded(object sender, RoutedEventArgs e)
        {
            //DiagramViewGrid grid = new DiagramViewGrid();
            //diagctrl = GetDiagramControl(this);
            dc = GetDiagramControl(this);
            dview = dc.View;
            //temp = this.MeasurementUnits;
            //dvg = (sender as DiagramPage).Children[0] as DiagramViewGrid;
            //dview.mViewGrid = dvg;
            //dview.Scrollviewer.ScrollChanged -= Scrollviewer_ScrollChanged;
            //dview.Scrollviewer.ScrollChanged += new ScrollChangedEventHandler(Scrollviewer_ScrollChanged);

            double width = Math.Max(dview.Scrollviewer.ActualWidth, dview.Scrollviewer.ExtentWidth);
            double height = Math.Max(dview.Scrollviewer.ActualHeight, dview.Scrollviewer.ExtentHeight);
            //dvg.Arrange(new Rect(0, 0, width, height));
            //page = dview.Page as DiagramPage;
            // dview.VerifyVirtualization();
            this.InvalidateMeasure();

        }



        //void Scrollviewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        //{
        //    if (dvg != null && dview != null && dview.Scrollviewer != null)
        //    {
        //       // double width = /*this.RenderSize.Width;*/ Math.Max(dview.Scrollviewer.ActualWidth, dview.Scrollviewer.ExtentWidth);
        //       // double height = /*this.RenderSize.Height;*/ Math.Max(dview.Scrollviewer.ActualHeight, dview.Scrollviewer.ExtentHeight);
        //       //dvg.Arrange(new Rect(0, 0, width, height));
        //        dvg.InvalidateVisual();

        //    }
        //}

        #endregion

        #region Class Override

        //protected override Visual GetVisualChild(int index)
        //{
        //    return base.GetVisualChild(index);
        //}

        //protected override 
        /// <summary>
        /// Gets the Diagram Control object.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns>The Diagram Control object</returns>
        public static DiagramControl GetDiagramControl(FrameworkElement element)
        {
            DependencyObject parent = VisualTreeHelper.GetParent(element);
            while (parent != null)
            {
                if (parent is DiagramControl)
                {
                    return parent as DiagramControl;
                }

                DependencyObject temp = VisualTreeHelper.GetParent(parent);
                if (temp == null && parent is FrameworkElement)
                {
                    parent = (parent as FrameworkElement).Parent;
                }
                else
                {
                    parent = temp;
                }
            }

            return null;
        }

        internal static ScrollBar GetScrollBarControl(FrameworkElement element, int whichone)
        {
            DependencyObject parent = VisualTreeHelper.GetChild(element, 0);
            while (parent != null)
            {
                if (parent is ScrollBar)
                {
                    return parent as ScrollBar;
                }
                DependencyObject temp = VisualTreeHelper.GetChild(parent, whichone);
                if (temp == null && parent is FrameworkElement)
                {
                    parent = (parent as FrameworkElement).Parent;
                }
                else
                {
                    parent = temp;
                }
            }

            return null;
        }

        /// <summary>
        /// Measures elements.
        /// </summary>
        /// <param name="availableSize">The available size</param>
        /// <returns>The available size.</returns>
        //bool preScrollX = true;
        //bool preScrollY = true;

        //bool isPreLineNegative = true;
        protected override Size MeasureOverride(Size availableSize)
        {
            Size visualDesiredSize = new Size(0, 0);
            Size dataDesiredSize = new Size(0, 0);
            Size negativeSize = new Size(0, 0);
            Rect rect = new Rect();

            foreach (UIElement ele in this.AllChildren)
            {
                if (ele is INodeGroup)
                {
                    if ((ele as INodeGroup).ReferenceNo < 0)
                    {
                        (ele as INodeGroup).ReferenceNo = ReferenceCount++;
                    }
                }
                if (this.InternalChildren.Contains(ele))
                {
                    ele.Measure(availableSize);

                }

                //double right = ((Matrix)ele.TransformToVisual(this).GetValue(MatrixTransform.MatrixProperty)).OffsetX;
                //double bottom = ((Matrix)ele.TransformToVisual(this).GetValue(MatrixTransform.MatrixProperty)).OffsetY;
                if (ele is Node)
                {


                    if ((ele as Node).RectBounds == Rect.Empty)
                    {
                        rect = new Rect((ele as Node).PxOffsetX, (ele as Node).PxOffsetY, (ele as Node).ActualWidth, (ele as Node).ActualHeight);

                    }
                    else
                    {
                        rect = new Rect((ele as Node).PxOffsetX, (ele as Node).PxOffsetY, (ele as Node).ActualWidth, (ele as Node).ActualHeight);
                        (ele as Node).RectBounds = rect;
                    }
                }

                if (ele is DiagramViewGrid)
                {
                    rect = Rect.Empty;
                }
                else if (ele is LineConnector)
                {
                    rect = (ele as LineConnector).GetBounds();
                }
                //double right = rect.Right;
                //double bottom = rect.Bottom;

                if (ele is Node)
                {
                    dataDesiredSize.Width = Math.Max(dataDesiredSize.Width, (ele as Node).PxOffsetX + (ele as IShape).ActualWidth);
                    dataDesiredSize.Height = Math.Max(dataDesiredSize.Height, (ele as Node).PxOffsetY + (ele as IShape).ActualHeight);
                    //rect = new Rect(new Point((ele as IShape).LogicalOffsetX, (ele as IShape).LogicalOffsetY), ele.RenderSize);
                    //right = (ele as IShape).LogicalOffsetX;
                    ////Point pt = (ele as Node).TransformToAncestor(this).Transform(
                    //bottom = (ele as IShape).LogicalOffsetY;
                }
                if (ele is ICommon)
                {
                    visualDesiredSize.Width = Math.Max(rect.Right, visualDesiredSize.Width);
                    visualDesiredSize.Height = Math.Max(rect.Bottom, visualDesiredSize.Height);

                    negativeSize.Width = Math.Max(-rect.Left, negativeSize.Width);
                    negativeSize.Height = Math.Max(-rect.Top, negativeSize.Height);
                }
            }
            if (dview != null)
            {
                //dview.ViewGridOrigin = new Point(((Matrix)this.TransformToVisual(dview.ViewGrid).GetValue(MatrixTransform.MatrixProperty)).OffsetX,
                //    ((Matrix)this.TransformToVisual(dview.ViewGrid).GetValue(MatrixTransform.MatrixProperty)).OffsetY);
                dview.InvalidateViewGrid();
                //dview.UpdateViewGridOrigin();
                //dview.ViewGridOrigin = new Point(Left - dview.Scrollviewer.HorizontalOffset, Top - dview.Scrollviewer.VerticalOffset);
                //NegativeScrollViewer.SetOrigenPoint(dview.ViewGrid, new Point(negativeSize.Width, negativeSize.Height));
            }

            Top = negativeSize.Height;
            Left = negativeSize.Width;

            if (dview != null)
            {
                Top = Math.Max(Top, dview.panPoint.Y);
                Left = Math.Max(Left, dview.panPoint.X);
            }

            ////this.Margin = new Thickness(negativeSize.Width, negativeSize.Height, 0, 0);
            //if (this.dview != null)
            //{
            //    if (Top > 0)
            //    {
            //        Bottom = Math.Max(0, dview.Scrollviewer.ViewportHeight - desiredSize.Height - dview.ViewGridOrigin.Y);// - (Top - dview.Scrollviewer.VerticalOffset));
            //    }
            //    if (Left > 0)
            //    {
            //        Right = Math.Max(0, dview.Scrollviewer.ViewportWidth - desiredSize.Width - dview.ViewGridOrigin.X);// - (Left - dview.Scrollviewer.HorizontalOffset));
            //    }
            //}

            //this.RenderTransform = new TranslateTransform() { X = negativeSize.Width, Y = negativeSize.Height };
            if (visualDesiredSize.Equals(new Size(0, 0)))
            {
                return dataDesiredSize;
            }
            return visualDesiredSize;
        }
        static bool fullload = true;
        internal static void SetBridging(DiagramControl dc)
        {
            if (dc != null && dc.Model != null)
            {
                List<UIElement> ordered = (from UIElement item in dc.Model.Connections
                                           orderby Panel.GetZIndex(item as UIElement)
                                           select item as UIElement).ToList();
                bool atleastone = false;
                foreach (UIElement element in ordered)
                {
                    if (element is LineConnector)
                    {
                        if ((element as LineConnector).invalidateBridging)
                        {
                            atleastone = true;
                        }
                        (element as LineConnector).invalidateBridging = true;
                    }
                }
                if (atleastone)
                {
                    foreach (UIElement element in ordered)
                    {
                        if (element is LineConnector && (element as LineConnector).ConnectorType != ConnectorType.Bezier)
                        {
                            (element as LineConnector).SetLineBridging();
                        }
                    }
                }
                foreach (UIElement element in ordered)
                {
                    if (element is LineConnector)
                    {
                        (element as LineConnector).invalidateBridging = false;
                    }
                }
            }
            if (dc != null && fullload)
            {
                (dc.View.Page as DiagramPage).InvalidateMeasure();
                fullload = false;
            }
        }

        /// <summary>
        /// Positions child elements and determines a size for the control.
        /// </summary>
        /// <param name="finalSize">The final area within the parent
        /// that this element should use to arrange itself and its children.</param>
        /// <returns>The actual size used.</returns>
        protected override Size ArrangeOverride(Size finalSize)
        {
            foreach (UIElement element in this.InternalChildren)
            {
                double offsetX = 0;
                double offsetY = 0;

                if (element is Node)
                {
                    offsetX = (element as Node).PxOffsetX;// MeasureUnitsConverter.ToPixels((element as IShape).LogicalOffsetX, this.MeasurementUnits);
                    offsetY = (element as Node).PxOffsetY;// MeasureUnitsConverter.ToPixels((element as IShape).LogicalOffsetY, this.MeasurementUnits);
                }

                Size dSize;
                dSize = element.DesiredSize;
                if (element is DiagramViewGrid)
                {
                    {
                        //element.Arrange(new Rect(offsetX, offsetY, this.DesiredSize.Width, this.DesiredSize.Height));
                    }
                }
                else
                {
                    element.Arrange(new Rect(offsetX, offsetY, dSize.Width, dSize.Height));
                }
            }

            if (disp != null)
            {
                disp.Abort();
            }
            //object o = this.Dispatcher.Invoke(new Initialize(DiagramPage.SetBridging),System.Windows.Threading.DispatcherPriority.SystemIdle,
            //                             this.dc);       
            disp = this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.SystemIdle,
                                    new Initialize(DiagramPage.SetBridging), this.dc);
            //DiagramPage.SetBridging(this.dc);
            DiagramControl.IsPageLoaded = false;
            //nodedropped = false;
            //if (dc != null && dc.View != null)
            //{
            //    //dc.View.Deleted = false;
            //}
            return finalSize;
            //return this.DesiredSize;
        }

        DispatcherOperation disp;
        internal delegate void Initialize(DiagramControl dc);
        /// <summary>
        /// Creates a clone of the object.
        /// </summary>
        /// <param name="obj">The object.</param>
        public static void Copyitem(object obj)
        {
            o = obj;
        }

        /// <summary>
        /// Invoked when an unhandled <see cref="E:System.Windows.Input.Mouse.PreviewMouseWheel"/> attached event reaches an element in its route that is derived from this class. Implement this method to add class handling for this event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.Windows.Input.MouseWheelEventArgs"/> that contains the event data.</param>
        protected override void OnPreviewMouseWheel(MouseWheelEventArgs e)
        {
            //bool wheeldown = false;

            //if (this.CurrentMinY < 0)
            //{
            //    if (!dc.View.IsJustWheeled)
            //    {
            //        dc.View.IsJustWheeled = true;
            //        //Dragtop += MinTop;
            //    }
            //}

            //if (Keyboard.Modifiers != ModifierKeys.Control)
            //{
            //    //s = Math.Min(48, Math.Abs(dc.View.Scrollviewer.VerticalOffset));

            //    //double minus = Math.Abs(Dragtop) - (2 * s);

            //    if (e.Delta > 0)
            //    {
            //        oncezero = false;

            //        if (dc.View.Scrollviewer.VerticalOffset != 0)
            //        {
            //            //scrollcount--;
            //            //if (CurrentMinY < 0 && !wheeldown)
            //            //{
            //            //    //dc.View.Y -= (s + Dragtop + minus + dc.View.PanConstant) / dc.View.CurrentZoom;
            //            //}
            //            //else
            //            //{
            //            //    //dc.View.Y += (s + dc.View.PanConstant) / dc.View.CurrentZoom;
            //            //}
            //        }
            //    }
            //    else
            //    {
            //        once = false;
            //        if (dc.View.Scrollviewer.VerticalOffset < dc.View.Scrollviewer.ScrollableHeight)
            //        {
            //            //scrollcount++;
            //            //max = Math.Min(Math.Abs(dc.View.Scrollviewer.ScrollableHeight - dc.View.Scrollviewer.VerticalOffset), 48);
            //            //wheeldown = true;
            //            //dc.View.Y -= (max + dc.View.PanConstant) / dc.View.CurrentZoom;
            //        }
            //    }

            //    if (dc.View.Scrollviewer.VerticalOffset == dc.View.Scrollviewer.ScrollableHeight && e.Delta > 0)
            //    {
            //        if (!once)
            //        {
            //            once = true;
            //            //dc.View.ViewGridOrigin = new Point(dc.View.ViewGridOrigin.X, dc.View.Y * dc.View.CurrentZoom);
            //        }
            //    }

            //    if (dc.View.Scrollviewer.VerticalOffset == 0 && e.Delta < 0)
            //    {
            //        if (!oncezero)
            //        {
            //            oncezero = true;
            //            //dc.View.ViewGridOrigin = new Point(dc.View.ViewGridOrigin.X, dc.View.Y * dc.View.CurrentZoom);
            //        }
            //    }

            //    if (dc.View.Scrollviewer.VerticalOffset != dc.View.Scrollviewer.ScrollableHeight && dc.View.Scrollviewer.VerticalOffset != 0)
            //    {
            //        //dc.View.ViewGridOrigin = new Point(dc.View.ViewGridOrigin.X, dc.View.Y * dc.View.CurrentZoom);
            //    }
            //}

            base.OnPreviewMouseWheel(e);
        }

        #endregion

        #region  Properties

        internal Style CustomPathStyle
        {
            get { return (Style)GetValue(CustomPathStyleProperty); }
            set { SetValue(CustomPathStyleProperty, value); }
        }

        public Style CustomTheme
        {
            get { return (Style)GetValue(CustomThemeProperty); }
            set { SetValue(CustomThemeProperty, value); }
        }

        public Themes Theme
        {
            get { return (Themes)GetValue(ThemePathStyleProperty); }
            set { SetValue(ThemePathStyleProperty, value); }
        }

        #region drawing tools

        private bool m_IsPolyLineEnabled = false;

        [Obsolete("Please set EnableDrawingTools to true and set PolyLine as DrawingTools")]
        ///<summary>
        /// Use EnableDrawingTools as true and DrawingTools as PolyLine it is similar to IsPolyLineEnabled
        ///(diagramView.Page as DiagramPage).EnableDrawingTools = true;
        /// (diagramView.Page as DiagramPage).DrawingTool = DrawingTools.PolyLine;
        /// </summary>
        public bool IsPolyLineEnabled
        {
            get
            {
                return m_IsPolyLineEnabled;
            }
            set
            {
                m_IsPolyLineEnabled = value;
                EnablePolyLine();
            }
        }

        private void EnablePolyLine()
        {
            DiagramView dv = Node.GetDiagramView(this);
            if (dv != null)
            {
                if (IsPolyLineEnabled == true)
                {
                    dv.EnableDrawingTools = false;
                    dv.EnableConnection = false;
                }
                
            }
        }



        #endregion


        //bool mcall = false;
        //internal bool ManualMeasureCall
        //{
        //    get { return mcall; }
        //    set { mcall = value; }
        //}
        /// <summary>
        /// Gets or sets the style reference. Used for Serialization purpose.
        /// </summary>
        /// <value>The style ref.</value>
        public Style StyleRef
        {
            get { return styleref; }
            set { styleref = value; }
        }

        /// <summary>
        /// Gets or sets the LineStyleRef reference. Used for Serialization purpose.
        /// </summary>
        /// <value>The line style ref.</value>
        public Style LineStyleRef
        {
            get { return lstyleref; }
            set { lstyleref = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [enable resizing current node on multiple selection].
        /// </summary>
        /// <value>
        /// <c>true</c> if [enable resizing current node on multiple selection]; otherwise, <c>false</c>.
        /// </value>
        public bool EnableResizingCurrentNodeOnMultipleSelection
        {
            get
            {
                return (bool)GetValue(EnableResizingCurrentNodeOnMultipleSelectionProperty);
            }

            set
            {
                SetValue(EnableResizingCurrentNodeOnMultipleSelectionProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the orientation reference. Used for Serialization purpose.
        /// </summary>
        /// <value>The orientation ref.</value>
        public TreeOrientation OrientationRef
        {
            get { return oref; }
            set { oref = value; }
        }

        /// <summary>
        /// Gets or sets the Horizontal spacing reference.Used for Serialization purpose.
        /// </summary>
        /// <value>The horizontal spacing reference.</value>
        public double HorizontalSpacingref
        {
            get { return href; }
            set { href = value; }
        }

        /// <summary>
        /// Gets or sets the vertical spacing reference .Used for Serialization purpose.
        /// </summary>
        /// <value>The vertical spacing reference.</value>
        public double VerticalSpacingref
        {
            get { return vref; }
            set { vref = value; }
        }

        /// <summary>
        ///  Gets or sets the SpaceBetweenSubTreeSpacing reference .Used for Serialization purpose.
        /// </summary>
        /// <value>The sub tree spacing reference.</value>
        public double SubTreeSpacingref
        {
            get { return sref; }
            set { sref = value; }
        }

        /// <summary>
        ///  Gets or sets the SpaceBetweenSubTreeSpacing reference .Used for Serialization purpose.
        /// </summary>
        /// <value>The sub tree spacing reference.</value>
        public LayoutType LayoutTyperef
        {
            get { return ltref; }
            set { ltref = value; }
        }

        ///// <summary>
        ///// Gets or sets a value indicating whether diagram page is loaded.
        ///// </summary>
        ///// <value>
        ///// <c>true</c> if diagram page is loaded; otherwise, <c>false</c>.
        ///// </value>
        //internal bool IsDiagrampageLoaded
        //{
        //    get { return loaded; }
        //    set { loaded = value; }
        //}

        /// <summary>
        /// Gets or sets the HorizontalOffset value of the grid .
        /// </summary>
        /// <value>
        /// Type: <see cref="double"/>
        /// Offset value.
        /// </value>
        /// <remarks>
        /// Default value is 25d.</remarks>
        /// <example>
        /// <code language="C#">
        /// using Syncfusion.Core;
        /// using Syncfusion.Windows.Diagram;
        /// namespace WpfApplication1
        /// {
        /// public partial class Window1 : Window
        /// {
        ///    public DiagramControl Control;
        ///    public DiagramModel Model;
        ///    public DiagramView View;
        ///    public Window1 ()
        ///    {
        ///       InitializeComponent ();
        ///       Control = new DiagramControl ();
        ///       View = new DiagramView ();
        ///       Control.View = View;
        ///       HorizontalRuler hruler = new HorizontalRuler();
        ///       View.HorizontalRuler = hruler;
        ///       View.ShowHorizontalGridLine = false;
        ///       View.ShowVerticalGridLine = false;
        ///       VerticalRuler vruler = new VerticalRuler();
        ///       View.VerticalRuler = vruler;
        ///       View.Bounds = new Thickness (0, 0, 1000, 1000);
        ///       View.IsPageEditable = true;
        ///       (View.Page as DiagramPage).GridHorizontalOffset=100;
        ///    }
        ///    }
        ///    }
        /// </code>
        /// </example>
        public double GridHorizontalOffset
        {
            get
            {
                //return horoffset;
                return (double)GetValue(GridHorizontalOffsetProperty);
            }

            set
            {
                //value = MeasureUnitsConverter.ToPixels(value, this.MeasurementUnits);
                SetValue(GridHorizontalOffsetProperty, value);
                //horoffset = value;
            }
        }

        internal double PxGridHorizontalOffset
        {
            get
            {
                return MeasureUnitsConverter.ToPixels(GridHorizontalOffset, (this as DiagramPage).MeasurementUnits);
            }

            set
            {
                GridHorizontalOffset = MeasureUnitsConverter.FromPixels(value, (this as DiagramPage).MeasurementUnits);
            }
        }

        internal double PxGridVerticalOffset
        {
            get
            {
                return MeasureUnitsConverter.ToPixels((this as DiagramPage).GridVerticalOffset, (this as DiagramPage).MeasurementUnits);
            }

            set
            {
                GridVerticalOffset = MeasureUnitsConverter.FromPixels(value, (this as DiagramPage).MeasurementUnits);
            }
        }


        public static readonly DependencyProperty GridHorizontalOffsetProperty =
            DependencyProperty.Register("GridHorizontalOffset", typeof(double), typeof(DiagramPage), new PropertyMetadata(25d, new PropertyChangedCallback(OnGridHorizontalOffsetChanged)));

        /// <summary>
        /// Gets or sets the VerticalOffset value of the grid .
        /// </summary>
        /// <value>
        /// Type: <see cref="double"/>
        /// Offset value.
        /// </value>
        /// <remarks>
        /// Default value is 25d.</remarks>
        /// <example>
        /// <code language="C#">
        /// using Syncfusion.Core;
        /// using Syncfusion.Windows.Diagram;
        /// namespace WpfApplication1
        /// {
        /// public partial class Window1 : Window
        /// {
        ///    public DiagramControl Control;
        ///    public DiagramModel Model;
        ///    public DiagramView View;
        ///    public Window1 ()
        ///    {
        ///       InitializeComponent ();
        ///       Control = new DiagramControl ();
        ///       View = new DiagramView ();
        ///       Control.View = View;
        ///       HorizontalRuler hruler = new HorizontalRuler();
        ///       View.HorizontalRuler = hruler;
        ///       View.ShowHorizontalGridLine = false;
        ///       View.ShowVerticalGridLine = false;
        ///       VerticalRuler vruler = new VerticalRuler();
        ///       View.VerticalRuler = vruler;
        ///       View.Bounds = new Thickness (0, 0, 1000, 1000);
        ///       View.IsPageEditable = true;
        ///       (View.Page as DiagramPage).GridVerticalOffset=100;
        ///    }
        ///    }
        ///    }
        /// </code>
        /// </example>
        public double GridVerticalOffset
        {
            get
            {
                //return vertoffset;
                return (double)GetValue(GridVerticalOffsetProperty);
            }

            set
            {
                //value = MeasureUnitsConverter.ToPixels(value, this.MeasurementUnits);
                //vertoffset = value;
                SetValue(GridVerticalOffsetProperty, value);
            }
        }

        public static readonly DependencyProperty GridVerticalOffsetProperty =
            DependencyProperty.Register("GridVerticalOffset", typeof(double), typeof(DiagramPage), new PropertyMetadata(25d, new PropertyChangedCallback(OnGridVerticalOffsetChanged)));

        /// <summary>
        /// Gets the Selection List of the items.
        /// </summary>
        /// <value>
        /// Type: <see cref="NodeCollection"/>
        /// The list containing the selected items.
        /// </value>
        public NodeCollection SelectionList
        {
            get
            {
                if (m_selectionList == null)
                {
                    m_selectionList = new NodeCollection(this);
                }

                return m_selectionList;
            }
        }

        /// <summary>
        /// Gets or sets the Measurement unit property.
        /// </summary>
        /// <value>
        /// Type: <see cref="MeasureUnits"/>
        /// Enum specifying the unit to be used.
        /// </value>
        /// <example>
        /// <code language="C#">
        /// using Syncfusion.Core;
        /// using Syncfusion.Windows.Diagram;
        /// namespace WpfApplication1
        /// {
        /// public partial class Window1 : Window
        /// {
        ///    public DiagramControl Control;
        ///    public DiagramModel Model;
        ///    public DiagramView View;
        ///    public Window1 ()
        ///    {
        ///       InitializeComponent ();
        ///       Control = new DiagramControl ();
        ///       View = new DiagramView ();
        ///       Control.View = View;
        ///       HorizontalRuler hruler = new HorizontalRuler();
        ///       View.HorizontalRuler = hruler;
        ///       View.ShowHorizontalGridLine = false;
        ///       View.ShowVerticalGridLine = false;
        ///       VerticalRuler vruler = new VerticalRuler();
        ///       View.VerticalRuler = vruler;
        ///       View.Bounds = new Thickness (0, 0, 1000, 1000);
        ///       View.IsPageEditable = true;
        ///       (View.Page as DiagramPage).MeasurementUnits = MeasureUnits.Inch;
        ///       Node n = new Node(Guid.NewGuid(), "Start");
        ///        n.Shape = Shapes.FlowChart_Start;
        ///        n.IsLabelEditable = true;
        ///        n.Label = "Start";
        ///        n.OffsetX = 1.5;
        ///        n.OffsetY = 2.5;
        ///        n.Width = 1.5;
        ///        n.Height = 0.75;
        ///        n.ToolTip="Start Node";
        ///        Model.Nodes.Add(n);
        ///    }
        ///    }
        ///    }
        /// </code>
        /// </example>
        public MeasureUnits MeasurementUnits
        {
            get
            {
                return (MeasureUnits)GetValue(MeasurementUnitsProperty);
            }

            set
            {
                SetValue(MeasurementUnitsProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the the type of connection to be used.
        /// </summary>
        /// <value>
        /// Type: <see cref="ConnectorType"/>
        /// Enum specifying the type of the connector to be used.
        /// </value>
        /// <example>
        /// <para/>This example shows how to set ConnectorType in C#.
        /// <code language="C#">
        /// connObject.ConnectorType = ConnectorType.Orthogonal;
        /// </code>
        /// </example>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]

        public ConnectorType ConnectorType
        {
            get
            {
                return m_connectionType;
            }

            set
            {
                if (value != m_connectionType)
                {
                    m_connectionType = value;
                }
            }
        }



        #endregion

        #region Internal Properties

        /// <summary>
        /// Gets or sets the old unit.
        /// </summary>
        /// <value>The old unit.</value>
        //[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        //internal MeasureUnits OldUnit
        //{
        //    get
        //    {
        //        return o_unit;
        //    }

        //    set
        //    {
        //        o_unit = value;
        //    }
        //}

        /// <summary>
        /// Gets or sets a value indicating whether the unit is changed.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is unit changed; otherwise, <c>false</c>.
        /// </value>
        //[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        //internal bool IsUnitChanged
        //{
        //    get
        //    {
        //        return isunitchanged;
        //    }

        //    set
        //    {
        //        isunitchanged = value;
        //    }
        //}

        /// <summary>
        /// Gets or sets the reference count. Used for serialization purposes
        /// </summary>
        /// <value>The reference count.</value>
        public int ReferenceCount
        {
            get;
            set;
        }

        ///// <summary>
        ///// Gets or sets a value indicating whether [greater than zero].
        ///// </summary>
        ///// <value><c>true</c> if [greater than zero]; otherwise, <c>false</c>.</value>
        //internal bool GreaterThanZero
        //{
        //    get { return gtz; }
        //    set { gtz = value; }
        //}

        ///// <summary>
        ///// Gets or sets a value indicating whether this instance is positive.
        ///// </summary>
        ///// <value>
        ///// <c>true</c> if this instance is positive; otherwise, <c>false</c>.
        ///// </value>
        //internal bool IsPositive
        //{
        //    get { return pos; }
        //    set { pos = value; }
        //}

        ///// <summary>
        ///// Gets or sets a value indicating whether  <see cref="Node"/> is rotated or resized.
        ///// </summary>
        ///// <value><c>true</c> if transformed; otherwise, <c>false</c>.</value>
        //internal bool Istransformed
        //{
        //    get { return transformed; }
        //    set { transformed = value; }
        //}

        ///// <summary>
        ///// Gets or sets a value indicating whether [greater than zero Y].
        ///// </summary>
        ///// <value><c>true</c> if [greater than zero Y]; otherwise, <c>false</c>.</value>
        //internal bool GreaterThanZeroY
        //{
        //    get { return gtzy; }
        //    set { gtzy = value; }
        //}

        ///// <summary>
        ///// Gets or sets a value indicating whether this instance is positive Y.
        ///// </summary>
        ///// <value>
        ///// <c>true</c> if this instance is positive Y; otherwise, <c>false</c>.
        ///// </value>
        //internal bool IsPositiveY
        //{
        //    get { return posy; }
        //    set { posy = value; }
        //}

        ///// <summary>
        ///// Gets or sets the min X.
        ///// </summary>
        ///// <value>The min X.</value>
        //internal double MinX
        //{
        //    get { return minimumX; }
        //    set { minimumX = value; }
        //}

        ///// <summary>
        ///// Gets or sets the min Y.
        ///// </summary>
        ///// <value>The min Y.</value>
        //internal double MinY
        //{
        //    get { return minimumY; }
        //    set { minimumY = value; }
        //}

        ///// <summary>
        ///// Gets or sets the least X.
        ///// </summary>
        ///// <value>The least X.</value>
        //internal double LeastX
        //{
        //    get { return leastx; }
        //    set { leastx = value; }
        //}

        ///// <summary>
        ///// Gets or sets the least Y.
        ///// </summary>
        ///// <value>The least Y.</value>
        //internal double LeastY
        //{
        //    get { return leasty; }
        //    set { leasty = value; }
        //}

        ///// <summary>
        ///// Gets or sets the minleft.
        ///// </summary>
        ///// <value>The minleft.</value>
        //internal double Minleft
        //{
        //    get { return minleftX; }
        //    set { minleftX = value; }
        //}

        ///// <summary>
        ///// Gets or sets the min top.
        ///// </summary>
        ///// <value>The min top.</value>
        //internal double MinTop
        //{
        //    get { return mintopY; }
        //    set { mintopY = value; }
        //}

        ///// <summary>
        ///// Gets or sets the Dragleft. Used for serialization purpose
        ///// </summary>
        ///// <value>The Dragged value.</value>
        //internal double Dragleft
        //{
        //    get { return dleft; }
        //    set { dleft = value; }
        //}

        ///// <summary>
        ///// Gets or sets the Dragtop. Used for serialization purpose
        ///// </summary>
        ///// <value>The dragged value.</value>
        //internal double Dragtop
        //{
        //    get { return dtop; }
        //    set { dtop = value; }
        //}

        ///// <summary>
        ///// Gets or sets the const min X.
        ///// </summary>
        ///// <value>The const min X.</value>
        //internal double ConstMinX
        //{
        //    get { return cminx; }
        //    set { cminx = value; }
        //}

        ///// <summary>
        ///// Gets or sets the const min Y.
        ///// </summary>
        ///// <value>The const min Y.</value>
        //internal double ConstMinY
        //{
        //    get { return cminy; }
        //    set { cminy = value; }
        //}

        ///// <summary>
        ///// Gets or sets the old min X.
        ///// </summary>
        ///// <value>The old min X.</value>
        //internal double OldMinX
        //{
        //    get { return curminx; }
        //    set { curminx = value; }
        //}

        ///// <summary>
        ///// Gets or sets the old min Y.
        ///// </summary>
        ///// <value>The old min Y.</value>
        //internal double OldMinY
        //{
        //    get { return curminy; }
        //    set { curminy = value; }
        //}

        ///// <summary>
        ///// Gets or sets the current min Y.
        ///// </summary>
        ///// <value>The current min Y.</value>
        //internal double CurrentMinY
        //{
        //    get { return cmy; }
        //    set { cmy = value; }
        //}

        ///// <summary>
        ///// Gets or sets the current min X.
        ///// </summary>
        ///// <value>The current min X.</value>
        //internal double CurrentMinX
        //{
        //    get { return cmx; }
        //    set { cmx = value; }
        //}

        ///// <summary>
        ///// Gets or sets the measure units.
        ///// </summary>
        ///// <value>The measure unit.</value>
        //internal static MeasureUnits Munits
        //{
        //    get { return m_units; }
        //    set { m_units = value; }
        //}

        ///// <summary>
        ///// Gets or sets the horizontal offset value.
        ///// </summary>
        ///// <value>The horizontal offset value.</value>
        //internal double HorValue
        //{
        //    get { return hval; }
        //    set { hval = value; }
        //}

        ///// <summary>
        ///// Gets or sets the vertical offset value.
        ///// </summary>
        ///// <value>The vertical offset value.</value>
        //internal double VerValue
        //{
        //    get { return vval; }
        //    set { vval = value; }
        //}

        ///// <summary>
        ///// Gets or sets a value indicating whether connector is dropped.
        ///// </summary>
        ///// <value>
        ///// <c>true</c> if connector is dropped; otherwise, <c>false</c>.
        ///// </value>
        //internal bool IsConnectorDropped
        //{
        //    get { return linedrop; }
        //    set { linedrop = value; }
        //}

        ///// <summary>
        ///// Gets or sets a value indicating whether the unit changed.
        ///// </summary>
        ///// <value><c>true</c> if unit converted; otherwise, <c>false</c>.</value>
        //internal bool Unitconverted
        //{
        //    get { return unitchan; }
        //    set { unitchan = value; }
        //}

        ///// <summary>
        ///// Gets or sets the scroll X.
        ///// </summary>
        ///// <value>The scroll X.</value>
        //internal double ScrollX
        //{
        //    get
        //    {
        //        return sx;
        //    }

        //    set
        //    {
        //        sx = value;
        //    }
        //}

        ///// <summary>
        ///// Gets or sets the scroll Y.
        ///// </summary>
        ///// <value>The scroll Y.</value>
        //internal double ScrollY
        //{
        //    get
        //    {
        //        return sy;
        //    }

        //    set
        //    {
        //        sy = value;
        //    }
        //}

        #endregion

        #region DPs

        /// <summary>
        /// Identifies the CustomPathStyle dependency property.
        /// </summary>
        internal static readonly DependencyProperty CustomPathStyleProperty =
            DependencyProperty.Register("CustomPathStyle", typeof(Style), typeof(DiagramPage), new UIPropertyMetadata(null));

        /// <summary>
        /// Identifies the CustomTheme dependency property.
        /// </summary>
        public static readonly DependencyProperty CustomThemeProperty =
            DependencyProperty.Register("CustomTheme", typeof(Style), typeof(DiagramPage), new UIPropertyMetadata(null));

        /// <summary>
        /// Identifies the Theme dependency property.
        /// </summary>
        public static readonly DependencyProperty ThemePathStyleProperty =
            DependencyProperty.Register("Theme", typeof(Themes), typeof(DiagramPage), new UIPropertyMetadata(Themes.Default));

        /// <summary>
        /// Identifies the AllowSelect dependency property.
        /// </summary>
        public static readonly DependencyProperty EnableResizingCurrentNodeOnMultipleSelectionProperty = DependencyProperty.Register("EnableResizingCurrentNodeOnMultipleSelection", typeof(bool), typeof(DiagramPage), new UIPropertyMetadata(false));

        /// <summary>
        /// Defines the LayoutType property.  This is a dependency property.
        /// </summary>
        public static readonly DependencyProperty LayoutTypeProperty = DependencyProperty.Register("LayoutType", typeof(LayoutType), typeof(DiagramPage), new UIPropertyMetadata(LayoutType.None));

        /// <summary>
        /// Defines the MeasurementUnits property.  This is a dependency property.
        /// </summary>
        public static readonly DependencyProperty MeasurementUnitsProperty = DependencyProperty.Register("MeasurementUnits", typeof(MeasureUnits), typeof(DiagramPage), new PropertyMetadata(MeasureUnits.Pixel, new PropertyChangedCallback(OnUnitsChanged)));

        #endregion

        #region Events

        /// <summary>
        /// Called when [units changed].
        /// </summary>
        /// <param name="d">The DependencyObject.</param>
        /// <param name="e">The <see cref="System.Windows.DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void OnUnitsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DiagramPage page = (DiagramPage)d;
            if (page.dc != null && page.dc.View != null)
            {
                Thickness px = MeasureUnitsConverter.ToPixels(page.dc.View.Bounds, (MeasureUnits)e.OldValue);
                page.dc.View.Bounds = MeasureUnitsConverter.FromPixels(px, (MeasureUnits)e.NewValue);
                page.GridHorizontalOffset = MeasureUnitsConverter.Convert(page.GridHorizontalOffset, (MeasureUnits)e.OldValue, (MeasureUnits)e.NewValue);
                page.GridVerticalOffset = MeasureUnitsConverter.Convert(page.GridVerticalOffset, (MeasureUnits)e.OldValue, (MeasureUnits)e.NewValue);
                page.dc.View.SnapOffsetX = MeasureUnitsConverter.Convert(page.dc.View.SnapOffsetX, (MeasureUnits)e.OldValue, (MeasureUnits)e.NewValue);
                page.dc.View.SnapOffsetY = MeasureUnitsConverter.Convert(page.dc.View.SnapOffsetY, (MeasureUnits)e.OldValue, (MeasureUnits)e.NewValue);
            }
            //page.IsUnitChanged = true;
            //page.OldUnit = page.temp;
            //if (page.isexe)
            //{
            //    page.isnotpixelwh = true;
            //    page.isnotpixeloffset = true;
            //    page.Unitconverted = true;

            //    if (page.OldUnit != page.MeasurementUnits)
            //    {
            //        page.m_unitchanged = true;
            //    }
            //}

            //if (page.dc != null && page.dc.View!=null)
            //{
            //    page.dc.View.SnapOffsetX = MeasureUnitsConverter.Convert(page.dc.View.SnapOffsetX, (MeasureUnits)e.OldValue, (MeasureUnits)e.NewValue);
            //    page.dc.View.SnapOffsetY = MeasureUnitsConverter.Convert(page.dc.View.SnapOffsetY, (MeasureUnits)e.OldValue, (MeasureUnits)e.NewValue);
            //}

            //page.isexe = true;
            //page.temp = page.MeasurementUnits;
            //page.UpdateNodeLayout();

            //foreach (UIElement element in page.InternalChildren)
            //{
            //    if (element is LineConnector)
            //    {
            //        (element as LineConnector).doRunTimeUnitChange();
            //    }
            //}
        }
        private static void OnGridHorizontalOffsetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DiagramPage page = (DiagramPage)d;
            if (page.dc != null && page.dc.View != null)
            {
                page.dc.View.mViewGrid.InvalidateVisual();
            }
        }
        private static void OnGridVerticalOffsetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DiagramPage page = (DiagramPage)d;
            if (page.dc != null && page.dc.View != null)
            {
                page.dc.View.mViewGrid.InvalidateVisual();
            }
        }
        #endregion

        #region Implementation

        /// <summary>
        /// Updates the node offset positions and size with respect to the current unit.
        /// </summary>
        private void UpdateNodeLayout()
        {
            //DiagramPage.Munits = this.MeasurementUnits;

            foreach (UIElement node in this.InternalChildren)
            {
                //if (node is LineConnector)
                //{
                //    (node as ConnectorBase).MeasurementUnit = this.MeasurementUnits;
                //}
                //if (isnotpixeloffset)
                //{
                //    if (node is Node)
                //    {
                //        //double pOffsetX = MeasureUnitsConverter.ToPixels((node as Node).LogicalOffsetX, this.OldUnit);
                //        //double cOffsetX = MeasureUnitsConverter.Convert(pOffsetX, MeasureUnits.Pixel, this.MeasurementUnits);
                //        //(node as Node).LogicalOffsetX = cOffsetX;
                //        //double pOffsetY = MeasureUnitsConverter.ToPixels((node as Node).LogicalOffsetY, this.OldUnit);
                //        //double cOffsetY = MeasureUnitsConverter.Convert(pOffsetY, MeasureUnits.Pixel, this.MeasurementUnits);
                //        //(node as Node).LogicalOffsetY = cOffsetY;
                //    }

                //    if (notfirstexe)
                //    {
                //        if (!exeonce)
                //        {
                //            if (node is Node)
                //            {
                //                //dc = GetDiagramControl((FrameworkElement)(node as Node));
                //                //Thickness rect = new Thickness();
                //                //rect = MeasureUnitsConverter.ToPixels(dc.View.Bounds, this.OldUnit);
                //                //dc.View.Bounds = MeasureUnitsConverter.FromPixels(rect, this.MeasurementUnits);

                //                //double vspace = MeasureUnitsConverter.ToPixels(dc.Model.VerticalSpacing, this.OldUnit);
                //                //dc.Model.VerticalSpacing = MeasureUnitsConverter.FromPixels(vspace, this.MeasurementUnits);
                //                //double hspace = MeasureUnitsConverter.ToPixels(dc.Model.HorizontalSpacing, this.OldUnit);
                //                //dc.Model.HorizontalSpacing = MeasureUnitsConverter.FromPixels(hspace, this.MeasurementUnits);
                //                //double subspace = MeasureUnitsConverter.ToPixels(dc.Model.SpaceBetweenSubTrees, this.OldUnit);
                //                //dc.Model.SpaceBetweenSubTrees = MeasureUnitsConverter.FromPixels(subspace, this.MeasurementUnits);

                //                exeonce = true;
                //            }
                //        }
                //    }

                //    notfirstexe = true;
                //}

                //if (!isnotpixelwh || !m_unitchanged)
                //{
                //    if (node is Node)
                //    {
                //        //double oldwidth = (node as Node).Width;// MeasureUnitsConverter.FromPixels((node as Node).Width, this.OldUnit);
                //        //double a = oldwidth;// MeasureUnitsConverter.Convert(oldwidth, this.MeasurementUnits, MeasureUnits.Pixel);
                //        //(node as Node).Width = a;

                //        //double oldheight = (node as Node).Height;// MeasureUnitsConverter.FromPixels((node as Node).Height, this.OldUnit);
                //        //double b = oldheight;// MeasureUnitsConverter.Convert(oldheight, this.MeasurementUnits, MeasureUnits.Pixel);
                //        //(node as Node).Height = b;
                //        foreach (ConnectionPort port in (node as Node).Ports)
                //        {
                //            //double portleft = port.Left;// MeasureUnitsConverter.FromPixels(port.Left, this.OldUnit);
                //            //double c = portleft;// MeasureUnitsConverter.Convert(portleft, this.MeasurementUnits, MeasureUnits.Pixel);
                //            //port.Left = c;
                //            //double porttop = port.Top;// MeasureUnitsConverter.FromPixels(port.Top, this.OldUnit);
                //            //double d = porttop;// MeasureUnitsConverter.Convert(porttop, this.MeasurementUnits, MeasureUnits.Pixel);
                //            //port.Top = d;
                //        }
                //    }
                //    else if (node is LineConnector)
                //    {
                //        if (!DiagramControl.IsPageLoaded && !(node as LineConnector).IsDefaulted)
                //        {
                //            //double oldcs = (node as LineConnector).ConnectionEndSpace;// MeasureUnitsConverter.ToPixels((node as LineConnector).ConnectionEndSpace, this.MeasurementUnits);
                //            //(node as LineConnector).ConnectionEndSpace = oldcs;
                //        }

                //        m_unitchanged = false;
                //    }
                //}
            }

            //exeonce = false;
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            switch (e.Key)
            {
                case Key.Up:
                    dc.View.IsKeyDragged = true;
                    DiagramView.MoveUp(dc.View);
                    //DiagramCommandManager.MoveUp.Execute((e.Source as DiagramControl).View.Page, (e.Source as DiagramControl).View);
                    break;
                case Key.Down:
                    dc.View.IsKeyDragged = true;
                    DiagramView.MoveDown(dc.View);
                    //DiagramCommandManager.MoveDown.Execute((e.Source as DiagramControl).View.Page, (e.Source as DiagramControl).View);
                    break;
                case Key.Left:
                    dc.View.IsKeyDragged = true;
                    DiagramView.MoveLeft((dc.View));
                    //DiagramCommandManager.MoveLeft.Execute((e.Source as DiagramControl).View.Page, (e.Source as DiagramControl).View);
                    break;
                case Key.Right:
                    dc.View.IsKeyDragged = true;
                    DiagramView.MoveRight(dc.View);
                    //DiagramCommandManager.MoveRight.Execute((e.Source as DiagramControl).View.Page, (e.Source as DiagramControl).View);
                    break;
                default:
                    break;
            }
        }

        #endregion

        #region IDiagramPanel Members

        ///// <summary>
        ///// Invalidates the measures.
        ///// </summary>
        //public new void InvalidateMeasure()
        //{
        //    base.InvalidateMeasure();
        //}

        ///// <summary>
        ///// Gets the Actual Width of the DiagramPage.
        ///// </summary>
        ///// <value>
        ///// Type: <see cref="double"/>
        ///// Actual Width of the DiagramPage in pixels.
        ///// </value>
        //public new double ActualWidth
        //{
        //    get
        //    {
        //        return base.ActualWidth;
        //    }
        //}

        ///// <summary>
        ///// Gets the Actual Height of the DiagramPage.
        ///// </summary>
        ///// <value>
        ///// Type: <see cref="double"/>
        ///// Actual Height of the DiagramPage in pixels.
        ///// </value>
        //public new double ActualHeight
        //{
        //    get
        //    {
        //        return base.ActualHeight;
        //    }
        //}

        #endregion

        #region INotifyPropertyChanged Members

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raised when the appropriate property changes.
        /// </summary>
        /// <param name="name">The property name.</param>
        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
        #endregion

        #region Virtualization Members

        internal void AddingChildren(UIElement Child)
        {
            this.AddInternalChild(Child);
        }
        internal void RemovingChildren(UIElement Child)
        {
            if (Child is Node && (Child as Node).IsInternallyLoaded)
            {
                (Child as Node).IsInternallyLoaded = false;
                this.Children.Remove(Child);
            }
            else if (Child is LineConnector && (Child as LineConnector).IsInternallyLoaded)
            {
                (Child as LineConnector).IsInternallyLoaded = false;
                this.Children.Remove(Child);
            }
        }
        protected void AddInternalChild(UIElement child)
        {

            if (child is Node && !(child as Node).IsInternallyLoaded)
            {
                (child as Node).IsInternallyLoaded = true;
                base.AddInternalChild(child);
            }
            else if (child is LineConnector && !(child as LineConnector).IsInternallyLoaded)
            {
                (child as LineConnector).IsInternallyLoaded = true;
                base.AddInternalChild(child);
            }
        }
        protected void RemoveInternalChild(UIElement child)
        {


        }
        #endregion
    }
}
