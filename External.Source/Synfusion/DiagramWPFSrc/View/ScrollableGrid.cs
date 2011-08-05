#region Copyright Syncfusion Inc. 2001 - 2011
// Copyright Syncfusion Inc. 2001 - 2011. All rights reserved.
// Use of this code is subject to the terms of our license.
// A copy of the current license can be obtained at any time by e-mailing
// licensing@syncfusion.com. Any infringement will be prosecuted under
// applicable laws. 
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls.Primitives;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Threading;
using System.Windows.Media;

namespace Syncfusion.Windows.Diagram
{
    internal partial class ScrollableGrid : Grid, IScrollInfo
    {
        DiagramView view = null;

        public ScrollableGrid()
            : base()
        {
            //_ScrollOwner.ScrollChanged += new ScrollChangedEventHandler(_ScrollOwner_ScrollChanged);
            this.Loaded += new RoutedEventHandler(ScrollableGrid_Loaded);
            if (dc != null && dc.View != null)
            {
                view = dc.View;
                view.ScrollGrid = this as ScrollableGrid;
            }
        }
        void HorizontalThumb_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            if (view != null)
            {
                if (view.EnableVirtualization)
                {
                    this.callCalculate();
                    ////if (disp != null)
                    ////{
                    ////    disp.Abort();
                    ////}

                    //disp = this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.SystemIdle,
                    //                  new Syncfusion.Windows.Diagram.DiagramModel.Initialize(this.callCalculate));
                }
                view.UpdateViewGridOrigin();
            }
            else
            {

            }
        }
        void VerticalThumb_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            if (view != null)
            {
                if (view.EnableVirtualization)
                {
                    this.callCalculate();
                    ////if (disp != null)
                    ////{
                    ////    disp.Abort();
                    ////}

                    //disp = this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.SystemIdle,
                    //                  new Syncfusion.Windows.Diagram.DiagramModel.Initialize(this.callCalculate));
                }
                view.UpdateViewGridOrigin();
            }
            else
            {

            }
        }

        void ScrollableGrid_Loaded(object sender, RoutedEventArgs e)
        {
            if (_ScrollOwner != null)
            {
                this.Loaded -= new RoutedEventHandler(ScrollableGrid_Loaded);
                _ScrollOwner.ScrollChanged += new ScrollChangedEventHandler(_ScrollOwner_ScrollChanged);
            }
            //_ScrollOwner.ScrollChanged += new ScrollChangedEventHandler(_ScrollOwner_ScrollChanged);
            view = this.TemplatedParent as DiagramView;
            view.ScrollGrid = this as ScrollableGrid;
            view.VerifyVirtualization();
            // _ScrollOwner.ScrollChanged += new ScrollChangedEventHandler(_ScrollOwner_ScrollChanged);


        }
        DispatcherOperation disp;
        bool evenctcheck = true;
        bool horizontalcheck = true;
        bool verticalcheck = true;
        double Viewheight;
        double ViewWidth;
        void _ScrollOwner_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (evenctcheck)
            {
                if (horizontalcheck)
                {
                    ScrollBar Horizontalthumb = DiagramPage.GetScrollBarControl(_ScrollOwner, 3);
                    if (Horizontalthumb.Track != null)
                    {
                        Horizontalthumb.Track.Thumb.DragCompleted += new DragCompletedEventHandler(HorizontalThumb_DragCompleted);
                        horizontalcheck = false;
                    }
                    VerifyScrollVirtual();
                }

                if (verticalcheck)
                {
                    ScrollBar VerticalThumb = DiagramPage.GetScrollBarControl(_ScrollOwner, 2);
                    if (VerticalThumb.Track != null)
                    {
                        VerticalThumb.Track.Thumb.DragCompleted += new DragCompletedEventHandler(VerticalThumb_DragCompleted);
                        verticalcheck = false;
                    }
                    VerifyScrollVirtual();
                }
                if (!horizontalcheck && !verticalcheck)
                { evenctcheck = false; }

            }
            if (ViewWidth != _ScrollOwner.ViewportWidth)
            {
                if (Viewheight != _ScrollOwner.ViewportHeight)
                {
                    if (dc.View.EnableVirtualization)
                    {
                        this.callCalculate();
                    }
                }
                else
                {
                    if (dc.View.EnableVirtualization)
                    {
                        this.callCalculate();
                    }
                }
            }
           else if (Viewheight != _ScrollOwner.ViewportHeight)
            {
                if (dc.View.EnableVirtualization)
                {
                    this.callCalculate();
                }
            }
            ViewWidth = _ScrollOwner.ViewportWidth;
            Viewheight = _ScrollOwner.ViewportHeight;
             
            //if (view != null)
            //{
            //    if (view.EnableVirtualization)
            //    {
            //        if (disp != null)
            //        {
            //            disp.Abort();
            //        }
            //        disp = this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.SystemIdle,
            //                          new Syncfusion.Windows.Diagram.DiagramModel.Initialize(this.callCalculate));
            //    }
            //}
            //else
            //{

            //}
        }
        internal void callCalculate()
        {
            this.calculatesize(_ScrollOwner);
        }
        internal void MakeVirtualization()
        {
            if (view != null)
            {
                if (view.EnableVirtualization)
                {
                    if (disp != null)
                    {
                        disp.Abort();
                    }
                    disp = this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.SystemIdle,
                                      new Syncfusion.Windows.Diagram.DiagramModel.Initialize(this.callCalculate));
                }
            }

        }
        internal void VerifyScrollVirtual()
        {
            if (view != null)
            {
                if (view.EnableVirtualization)
                {
                    if (disp != null)
                    {
                        disp.Abort();
                    }
                    disp = this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.SystemIdle,
                                      new Syncfusion.Windows.Diagram.DiagramModel.Initialize(this.callCalculate));
                }
                else
                {
                    if (disp != null)
                    {
                        disp.Abort();
                    }
                }
            }

        }

        internal void CallNormailzation()
        {
            this.CalculateNormalization();
            view.Page.InvalidateMeasure();
            view.Page.InvalidateArrange();
        }

        private void CalculateNormalization()
        {
            view = dc.View;
            foreach (UIElement node in (view.Page as DiagramPage).AllChildren)
            {
                if (node is Node)
                {
                    if (!(node as Node).IsInternallyLoaded)
                    {
                        if (!view.Page.Children.Contains(node))
                        {
                            (view.Page as DiagramPage).AddingChildren(node);
                            //view.Page.Children.Add(node);
                        }

                    }

                }
                else if (node is LineConnector)
                {

                    if (!(node as LineConnector).IsInternallyLoaded)
                    {
                        if (!view.Page.Children.Contains(node))
                        {
                            (view.Page as DiagramPage).AddingChildren(node);
                            //view.Page.Children.Add(node);
                        }

                    }

                }

            }

        }

        internal void VirtualzingNode(Node element)
        {
            double Negativeaxistop = -(view.Page as DiagramPage).Top;
            double NegativeaxisLeft = -(view.Page as DiagramPage).Left;
            Rect viewablesize = new Rect();
            if (dc != null && !dc.IsLoaded)
            {
                view = dc.View;
                viewablesize = new Rect(0, 0, Constraint.Width, Constraint.Height);
            }
            else
            {
                if (NegativeaxisLeft < 0)
                {
                    if (Negativeaxistop < 0)
                    {

                    viewablesize = new Rect(_ScrollOwner.HorizontalOffset + NegativeaxisLeft, _ScrollOwner.VerticalOffset + Negativeaxistop, _ScrollOwner.ViewportWidth, _ScrollOwner.ViewportHeight);
                    }
                    else
                    {
                        viewablesize = new Rect(_ScrollOwner.HorizontalOffset + NegativeaxisLeft, _ScrollOwner.VerticalOffset, _ScrollOwner.ViewportWidth, _ScrollOwner.ViewportHeight);
                    }

                }

                else if (Negativeaxistop < 0)
                {
                    viewablesize = new Rect(_ScrollOwner.HorizontalOffset, _ScrollOwner.VerticalOffset + Negativeaxistop, _ScrollOwner.ViewportWidth, _ScrollOwner.ViewportHeight);
                }
                else
                {
                    viewablesize = new Rect(_ScrollOwner.HorizontalOffset, _ScrollOwner.VerticalOffset, _ScrollOwner.ViewportWidth, _ScrollOwner.ViewportHeight);
                }

            }
           
            if (!(element as Node).AllowVirtualization)
            {
                if (!(element as Node).IsInternallyLoaded)
                {
                    (view.Page as DiagramPage).AddingChildren(element);
                    (element as Node).Measure(new Size((element as Node).Width, (element as Node).Height));
                    (element as Node).Arrange(new Rect((element as Node).OffsetX, (element as Node).OffsetY, (element as Node).Width, (element as Node).Height));
                }
            }
            if (element is UIElement)
            {
                Rect nodebounds = new Rect((element as Node).PxOffsetX, (element as Node).PxOffsetY, (element as Node)._Width, (element as Node)._Height);
                if (viewablesize.IntersectsWith(nodebounds))
                {
                    if (!(element as Node).IsInternallyLoaded)
                    {
                        (view.Page as DiagramPage).AddingChildren(element);
                        // view.Page.Children.Add(element);
                    }
                }
                else if ((element as Node).IsInternallyLoaded)
                {
                    if ((element as Node).AllowVirtualization)
                    {
                        if (!view.EnableCaching)
                        {
                            if (!(element as Node).currentdragging)
                            {
                                (view.Page as DiagramPage).RemovingChildren(element);
                            }
                        }

                    }
                }
            }

        }
        internal void VirtualizingLine(LineConnector element)
        {
            double Negativeaxistop = -(view.Page as DiagramPage).Top;
            double NegativeaxisLeft = -(view.Page as DiagramPage).Left;
            Rect viewablesize = new Rect();
            if (dc != null && !dc.IsLoaded)
            {
                view = dc.View;
                viewablesize = new Rect(0, 0, Constraint.Width, Constraint.Height);
            }
            else
            {
                if (NegativeaxisLeft < 0)
                {
                    if (Negativeaxistop < 0)
                    {

                        viewablesize = new Rect(_ScrollOwner.HorizontalOffset + NegativeaxisLeft, _ScrollOwner.VerticalOffset + Negativeaxistop, _ScrollOwner.ViewportWidth, _ScrollOwner.ViewportHeight);
                    }
                    else
                    {
                        viewablesize = new Rect(_ScrollOwner.HorizontalOffset + NegativeaxisLeft, _ScrollOwner.VerticalOffset, _ScrollOwner.ViewportWidth, _ScrollOwner.ViewportHeight);
                    }

                }

                else if (Negativeaxistop < 0)
                {
                    viewablesize = new Rect(_ScrollOwner.HorizontalOffset, _ScrollOwner.VerticalOffset + Negativeaxistop, _ScrollOwner.ViewportWidth, _ScrollOwner.ViewportHeight);
                }
                else
                {
                    viewablesize = new Rect(_ScrollOwner.HorizontalOffset, _ScrollOwner.VerticalOffset, _ScrollOwner.ViewportWidth, _ScrollOwner.ViewportHeight);
                }

            }
           
            Rect linebounds = new Rect();
            if (!(element as LineConnector).AllowVirtualization)
            {
                if (!(element as LineConnector).IsInternallyLoaded)
                {
                    (view.Page as DiagramPage).AddingChildren(element);
                }
                (element as LineConnector).UpdateConnectorPathGeometry();
            }
            if (element is ICommon)
            {
                if ((element as LineConnector).IsInternallyLoaded)
                {
                    if ((element as LineConnector).HeadNode == null && (element as LineConnector).TailNode == null)
                    {
                        linebounds = (element as LineConnector).GetBounds();
                        if (viewablesize.IntersectsWith(linebounds))
                        {
                            if (!(element as LineConnector).IsInternallyLoaded)
                            {
                                (view.Page as DiagramPage).AddingChildren(element);
                            }
                        }
                        else if (!(element as LineConnector).IsInternallyLoaded && !view.EnableCaching)
                        {
                            (view.Page as DiagramPage).RemovingChildren(element);
                        }
                        else if (!view.EnableCaching)
                        {
                            (view.Page as DiagramPage).RemovingChildren(element);
                        }
 
                    }
                }
                else if (!(element as LineConnector).IsInternallyLoaded && !view.EnableCaching)
                {
                    (view.Page as DiagramPage).RemovingChildren(element);
                }
                else if (!view.EnableCaching)
                {
                    (view.Page as DiagramPage).RemovingChildren(element);
                }
            }
            if ((element as LineConnector).HeadNode != null && (element as LineConnector).TailNode != null)
            {

                if (((element as LineConnector).HeadNode as Node).IsInternallyLoaded || ((element as LineConnector).TailNode as Node).IsInternallyLoaded)
                {
                    if (!(element as LineConnector).IsInternallyLoaded)
                    {
                        (view.Page as DiagramPage).AddingChildren(element);
                    }
                }
                else
                {
                    if (!(element as LineConnector).IsInternallyLoaded && !view.EnableCaching)
                    {

                        (view.Page as DiagramPage).RemovingChildren(element);
                    }
                    else if (!view.EnableCaching)
                    {
                        (view.Page as DiagramPage).RemovingChildren(element);
                    }
                }
            }
            else
            {
               
                    linebounds = LineBounds(element as LineConnector);
                    if (viewablesize.IntersectsWith(linebounds))
                    {
                        if (!(element as LineConnector).IsInternallyLoaded)
                        {
                            (view.Page as DiagramPage).AddingChildren(element);
                        }
                    }
               
            }
            (element as LineConnector).UpdateConnectorPathGeometry();
            (view.Page as DiagramPage).InvalidateMeasure();
            (view.Page as DiagramPage).InvalidateArrange();

        }
        private int CompareBandIndex(UIElement bar1, UIElement bar2)
        {
            if (bar1 is Node && bar2 is Node)
            {
                if ((bar1 as Node).OffsetX > (bar2 as Node).OffsetX)
                    return 1;
                else if ((bar1 as Node).OffsetX == (bar2 as Node).OffsetX)
                    return 0;
                else
                    return -1;
            }
            else
            {
                return 0;
            }
        }

        internal Rect GetViewableSize(ScrollViewer scroll)
        {
            Rect viewablesize = new Rect();
            
            if (dc != null && !dc.IsLoaded)
            {
                view = dc.View;
                viewablesize = new Rect(0, 0, Constraint.Width, Constraint.Height);
                return viewablesize;
            }
            else
            {
                double Negativeaxistop = -(view.Page as DiagramPage).Top;
                double NegativeaxisLeft = -(view.Page as DiagramPage).Left;
                if (NegativeaxisLeft < 0)
                {
                    if (Negativeaxistop < 0)
                    {
                        viewablesize = new Rect(scroll.HorizontalOffset + NegativeaxisLeft, scroll.VerticalOffset + Negativeaxistop, scroll.ViewportWidth, scroll.ViewportHeight);
                    }
                    else
                    {
                        viewablesize = new Rect(scroll.HorizontalOffset + NegativeaxisLeft, scroll.VerticalOffset, scroll.ViewportWidth, scroll.ViewportHeight);
                    }
                }
                else if (Negativeaxistop < 0)
                {
                    viewablesize = new Rect(scroll.HorizontalOffset, scroll.VerticalOffset + Negativeaxistop, scroll.ViewportWidth, scroll.ViewportHeight);
                }
                else
                {
                    viewablesize = new Rect(scroll.HorizontalOffset, scroll.VerticalOffset, scroll.ViewportWidth, scroll.ViewportHeight);
                }
                return viewablesize;
            }
        }
        bool onecheck=true;
        private void calculatesize(ScrollViewer scroll)
        {
           if(dc!=null&&!dc.IsLoaded&&dc.Model.LayoutType!=LayoutType.None&&onecheck)
            {
                if (dc.Model.LayoutType == LayoutType.HierarchicalTreeLayout)
                {
                    HierarchicalTreeLayout tree = new HierarchicalTreeLayout(dc.Model, dc.View);
                    tree.RefreshLayout();
                }
               else  if (dc.Model.LayoutType == LayoutType.DirectedTreeLayout)
                {
                    DirectedTreeLayout tree = new DirectedTreeLayout(dc.Model, dc.View);
                    tree.RefreshLayout();
                }
                else if (dc.Model.LayoutType == LayoutType.RadialTreeLayout)
                {
                    RadialTreeLayout tree = new RadialTreeLayout(dc.Model, dc.View);
                    tree.RefreshLayout();
                }
                else if (dc.Model.LayoutType == LayoutType.TableLayout)
                {
                    TableLayout tree = new TableLayout(dc.Model, dc.View);
                    tree.RefreshLayout();
                }
                onecheck = false;
            }

            Rect viewablesize = new Rect();
            viewablesize = GetViewableSize(scroll);
            foreach (UIElement element in dc.Model.Nodes)
            {
                VirtualzingNode(element as Node);
            }
          //  Rect linebounds = new Rect();
            foreach (UIElement element in dc.Model.Connections)
            {
                VirtualizingLine(element as LineConnector);
            }
        }
        private const double LineSize = 16;
        private const double WheelSize = 3 * LineSize;

        private bool _CanHorizontallyScroll;
        private bool _CanVerticallyScroll;
        private ScrollViewer _ScrollOwner;
        internal Vector _Offset;
        private Size _Extent;
        private Size _Viewport;


        internal void VirtualizationRelatedToLine()
        {
            Rect linebounds = new Rect();
            foreach (UIElement element in view.dc.Model.Connections)
            {


                if (!(element as LineConnector).AllowVirtualization)
                {
                    (view.Page as DiagramPage).AddingChildren(element);

                    //}
                    //if (element is ICommon)
                    //{
                    //    if ((element as LineConnector).IsLoaded)
                    //    {
                    //        linebounds = (element as LineConnector).GetBounds();
                    //    }
                    //    else
                    //    {
                    //        linebounds = LineBounds(element as LineConnector);

                    //    }
                    //    if (viewablesize.IntersectsWith(linebounds))
                    //    {


                    //        if (!view.Page.Children.Contains(element))
                    //        {
                    //            (view.Page as DiagramPage).AddingChildren(element);
                    //            // view.Page.Children.Add(element);
                    //        }

                    //    }
                    //    else
                    //    {

                    //        view.Page.Children.Remove(element);
                    //    }
                    //}
                    if (view.EnableCaching)
                    {
                        if (view.Page.Children.Contains(element))
                        {
                            if ((element as LineConnector).HeadNode != null)
                            {
                                if (!(view.Page as DiagramPage).Children.Contains(((element as LineConnector).HeadNode as Node)))
                                {

                                    (view.Page as DiagramPage).AddingChildren(((element as LineConnector).HeadNode as Node));
                                }
                            }
                            if ((element as LineConnector).TailNode != null)
                            {
                                if (!(view.Page as DiagramPage).Children.Contains(((element as LineConnector).TailNode as Node)))
                                { (view.Page as DiagramPage).AddingChildren(((element as LineConnector).TailNode as Node)); }
                            }
                        }
                    }
                    //if ((element as LineConnector).HeadNode != null && (element as LineConnector).TailNode != null)
                    //{
                    //    if (((element as LineConnector).HeadNode as Node).IsLoaded || ((element as LineConnector).TailNode as Node).IsLoaded)
                    //    {
                    //        if (!view.Page.Children.Contains(element))
                    //        {
                    //            (view.Page as DiagramPage).AddingChildren(element);
                    //            // view.Page.Children.Add(element);
                    //        }
                    //    }
                    //}
                }
            }

        }
        internal Rect LineBounds(LineConnector line)
        {
            Rect rect = new Rect();
            if (line.PxStartPointPosition.X < line.PxEndPointPosition.X)
            {
                rect = new Rect(line.PxStartPointPosition.X, line.PxStartPointPosition.Y, Math.Abs(line.PxEndPointPosition.X - line.PxStartPointPosition.X), Math.Abs(line.PxEndPointPosition.Y - line.PxStartPointPosition.Y));
                return rect;
            }
            else
            {
                rect = new Rect(line.PxEndPointPosition.X, line.PxEndPointPosition.Y, Math.Abs(line.PxStartPointPosition.X - line.PxEndPointPosition.X), Math.Abs(line.PxStartPointPosition.Y - line.PxEndPointPosition.Y));
                return rect;
            }
        }

        public void LineDown()
        {
            if (view != null)
            {
                view.InvalidateViewGrid();
            }
            SetVerticalOffset(VerticalOffset + LineSize);

            if (view != null)
            {
                view.InvalidateViewGrid();
            }
        }

        public void LineUp()
        {
            if (view != null)
            {
                view.InvalidateViewGrid();
            }
            SetVerticalOffset(VerticalOffset - LineSize);
            if (view != null)
            {
                view.InvalidateViewGrid();
            }
        }

        public void LineLeft()
        {
            if (view != null)
            {
                view.InvalidateViewGrid();
            }
            SetHorizontalOffset(HorizontalOffset - LineSize);
        }

        public void LineRight()
        {
            if (view != null)
            {
                view.InvalidateViewGrid();
            }
            SetHorizontalOffset(HorizontalOffset + LineSize);
        }

        public void MouseWheelDown()
        {
            if (view != null)
            {
                view.InvalidateViewGrid();
            }
            SetVerticalOffset(VerticalOffset + WheelSize);
        }

        public void MouseWheelUp()
        {
            if (view != null)
            {
                view.InvalidateViewGrid();
            }
            SetVerticalOffset(VerticalOffset - WheelSize);
        }

        public void MouseWheelLeft()
        {
            if (view != null)
            {
                view.InvalidateViewGrid();
            }
            SetHorizontalOffset(HorizontalOffset - WheelSize);
        }

        public void MouseWheelRight()
        {
            if (view != null)
            {
                view.InvalidateViewGrid();
            }
            SetHorizontalOffset(HorizontalOffset + WheelSize);
        }

        public void PageDown()
        {
            if (view != null)
            {
                view.InvalidateViewGrid();
            }
            SetVerticalOffset(VerticalOffset + ViewportHeight);
        }

        public void PageUp()
        {
            if (view != null)
            {
                view.InvalidateViewGrid();
            }
            SetVerticalOffset(VerticalOffset - ViewportHeight);
        }

        public void PageLeft()
        {
            if (view != null)
            {
                view.InvalidateViewGrid();
            } SetHorizontalOffset(HorizontalOffset - ViewportWidth);

        }

        public void PageRight()
        {
            if (view != null)
            {
                view.InvalidateViewGrid();
            } SetHorizontalOffset(HorizontalOffset + ViewportWidth);
        }

        public ScrollViewer ScrollOwner
        {
            get { return _ScrollOwner; }
            set { _ScrollOwner = value; }
        }

        public bool CanHorizontallyScroll
        {
            get { return _CanHorizontallyScroll; }
            set { _CanHorizontallyScroll = value; }
        }

        public bool CanVerticallyScroll
        {
            get { return _CanVerticallyScroll; }
            set { _CanVerticallyScroll = value; }
        }

        public double ExtentHeight
        {
            get
            {
                if (view != null && view.Page != null)
                {
                    if ((view.Page as DiagramPage).Top > 0)
                    {
                        return Math.Max(_Viewport.Height + _Offset.Y, _Extent.Height);
                    }
                } return _Extent.Height;
            }
        }

        public double ExtentWidth
        {
            get
            {
                if (view != null && view.Page != null)
                {
                    if ((view.Page as DiagramPage).Left > 0)
                    {
                        return Math.Max(_Viewport.Width + _Offset.X, _Extent.Width);
                    }
                }
                return _Extent.Width;
            }
        }

        public double HorizontalOffset
        {
            get
            {
                //if (page != null && page.isdragging)
                //{
                //    return page.Left + _Offset.X;
                //}
                return _Offset.X;
            }
        }

        public double VerticalOffset
        {
            get
            {
                //if (page != null && page.isdragging)
                //{
                //    return page.Top + _Offset.Y;
                //} 
                return _Offset.Y;
            }
        }

        public double ViewportHeight
        { get { return _Viewport.Height; } }

        public double ViewportWidth
        { get { return _Viewport.Width; } }

        //protected void VerifyScrollData()
        //{
        //    //if (double.IsInfinity(viewport.Width))
        //    //{ viewport.Width = extent.Width; }

        //    //if (double.IsInfinity(viewport.Height))
        //    //{ viewport.Height = extent.Height; }

        //    _Extent = this.DesiredSize;
        //    _Viewport = new Size(this.ActualWidth, this.ActualHeight);

        //    //_Offset.X = Math.Max(0,
        //    //  Math.Min(_Offset.X, ExtentWidth - ViewportWidth));
        //    //_Offset.Y = Math.Max(0,
        //    //  Math.Min(_Offset.Y, ExtentHeight - ViewportHeight));

        //    if (ScrollOwner != null)
        //    { ScrollOwner.InvalidateScrollInfo(); }
        //}

        protected void VerifyScrollData(Size viewport, Size extent)
        {
            if (double.IsInfinity(viewport.Width))
            { viewport.Width = extent.Width; }

            if (double.IsInfinity(viewport.Height))
            { viewport.Height = extent.Height; }

            _Extent = extent;
            _Viewport = viewport;

            _Offset.X = Math.Max(0,
              Math.Min(_Offset.X, ExtentWidth - ViewportWidth));
            _Offset.Y = Math.Max(0,
              Math.Min(_Offset.Y, ExtentHeight - ViewportHeight));

            if (ScrollOwner != null)
            { ScrollOwner.InvalidateScrollInfo(); }
        }

        public Rect MakeVisible(System.Windows.Media.Visual visual, Rect rectangle)
        {
            return rectangle;
        }

        public void SetHorizontalOffset(double offset)
        {
            offset = Math.Max(0,
              Math.Min(offset, ExtentWidth - ViewportWidth));
            if (offset != _Offset.X)
            {
                _Offset.X = offset;
                InvalidateArrange();
            }
            MakeVirtualization();
        }

        public void SetVerticalOffset(double offset)
        {
            offset = Math.Max(0,
              Math.Min(offset, ExtentHeight - ViewportHeight));
            if (offset != _Offset.Y)
            {
                _Offset.Y = offset;
                InvalidateArrange();
            }
            MakeVirtualization();
        }

        Size INeed;
        Size ButIHave;
        internal static DiagramControl dc;
        internal Size Constraint;
        protected override Size MeasureOverride(Size constraint)
        {

            if (dc != null && !dc.IsLoaded)
            {
                Constraint = constraint;
                if (dc.View.EnableVirtualization)
                {
                    this.callCalculate();
                }
                else
                {
                    this.CallNormailzation();
                }
            }
            ButIHave = constraint;
            INeed = base.MeasureOverride(constraint);
            VerifyScrollData(ButIHave, INeed);
            return INeed;
        }
        //internal void BeforeLoad(Rect viewablesize)
        //{
        //    view = dc.View;
        //    foreach (UIElement element in dc.Model.Nodes)
        //    {
        //        if (!(element as Node).AllowVirtualization)
        //        {
        //            if (!(view.Page as DiagramPage).Children.Contains(element))
        //            {

        //                (view.Page as DiagramPage).AddingChildren(element);
        //                (element as Node).Measure(new Size((element as Node).Width, (element as Node).Height));
        //                (element as Node).Arrange(new Rect((element as Node).OffsetX, (element as Node).OffsetY, (element as Node).Width, (element as Node).Height));
        //            }

        //        }
        //        if (element is UIElement)
        //        {
        //            Rect nodebounds = new Rect((element as Node).OffsetX, (element as Node).OffsetY, (element as Node).Width, (element as Node).Height);
        //            if (viewablesize.IntersectsWith(nodebounds))
        //            {
        //                if (!view.Page.Children.Contains(element))
        //                {
        //                    (view.Page as DiagramPage).AddingChildren(element);
        //                    // view.Page.Children.Add(element);
        //                }
        //            }
        //            else if ((view.Page as DiagramPage).Children.Contains(element))
        //            {
        //                if ((element as Node).AllowVirtualization)
        //                {
        //                    if (!view.EnableCaching)
        //                    {
        //                        if (!(element as Node).currentdragging)
        //                        {
        //                            (view.Page as DiagramPage).Children.Remove(element);
        //                        }
        //                    }

        //                }
        //            }

        //            //else if ((view.Page as DiagramPage).Children.Contains(element) && (element as Node).AllowVirtualization && !(element as Node).currentdragging && !view.EnableCaching)
        //            //{

        //            //    (view.Page as DiagramPage).Children.Remove(element);
        //            //    //(view.Page as DiagramPage).RemovingChildren(element);
        //            //}
        //            //else if ((element as Node).AllowVirtualization && !(element as Node).currentdragging && !view.EnableCaching)
        //            //{
        //            //    (view.Page as DiagramPage).Children.Remove(element);

        //            //}
        //        }
        //    }
        //    Rect linebounds = new Rect();
        //    foreach (UIElement element in dc.Model.Connections)
        //    {


        //        if (!(element as LineConnector).IsVirtualEnabled)
        //        {
        //            (view.Page as DiagramPage).AddingChildren(element);

        //            (element as LineConnector).UpdateConnectorPathGeometry();
        //        }
        //        if (element is ICommon)
        //        {
        //            if ((element as LineConnector).IsLoaded)
        //            {
        //                linebounds = (element as LineConnector).GetBounds();
        //            }
        //            else
        //            {
        //                linebounds = LineBounds(element as LineConnector);

        //            }
        //            if (viewablesize.IntersectsWith(linebounds))
        //            {


        //                if (!view.Page.Children.Contains(element))
        //                {

        //                    (view.Page as DiagramPage).AddingChildren(element);
        //                    // view.Page.Children.Add(element);
        //                    (element as LineConnector).UpdateConnectorPathGeometry();
        //                }

        //            }
        //            else if ((view.Page as DiagramPage).Children.Contains(element) && !view.EnableCaching)
        //            {

        //                view.Page.Children.Remove(element);
        //            }
        //            else if (!view.EnableCaching)
        //            {
        //                view.Page.Children.Remove(element);

        //            }
        //        }
        //        //if (view.VirtualizationRelatedToLine)
        //        //{
        //        //    if (view.Page.Children.Contains(element))
        //        //    {
        //        //        if ((element as LineConnector).HeadNode != null)
        //        //        {
        //        //            if (!(view.Page as DiagramPage).Children.Contains(((element as LineConnector).HeadNode as Node)))
        //        //            {

        //        //                (view.Page as DiagramPage).AddingChildren(((element as LineConnector).HeadNode as Node));
        //        //            }
        //        //        }
        //        //        if ((element as LineConnector).TailNode != null)
        //        //        {
        //        //            if (!(view.Page as DiagramPage).Children.Contains(((element as LineConnector).TailNode as Node)))
        //        //            { (view.Page as DiagramPage).AddingChildren(((element as LineConnector).TailNode as Node)); }
        //        //        }
        //        //    }
        //        //}

        //        if ((element as LineConnector).HeadNode != null && (element as LineConnector).TailNode != null)
        //        {
        //            if (((element as LineConnector).HeadNode as Node).IsLoaded || ((element as LineConnector).TailNode as Node).IsLoaded)
        //            {
        //                if (!view.Page.Children.Contains(element))
        //                {
        //                    (view.Page as DiagramPage).AddingChildren(element);
        //                }

        //                if (!((element as LineConnector).HeadNode as Node).IsLoaded)
        //                {
        //                    // (element as LineConnector).StartPointPosition = new Point(((element as LineConnector).HeadNode as Node).OffsetX, ((element as LineConnector).HeadNode as Node).OffsetY);
        //                    //    (element as LineConnector).GetTerminalPoints(element as LineConnector);
        //                    (element as LineConnector).UpdateConnectorPathGeometry();
        //                }
        //                else if (!((element as LineConnector).TailNode as Node).IsLoaded)
        //                {

        //                    // (element as LineConnector).PxEndPointPosition = new Point(((element as LineConnector).TailNode as Node).OffsetX, ((element as LineConnector).TailNode as Node).OffsetY);
        //                    // (element as LineConnector).GetTerminalPoints(element as LineConnector);
        //                    // (element as LineConnector).GetTerminalPoints(element as LineConnector);
        //                    (element as LineConnector).UpdateConnectorPathGeometry();
        //                }
        //                else
        //                {
        //                    (element as LineConnector).UpdateConnectorPathGeometry();
        //                }


        //                // view.Page.Children.Add(element);

        //            }

        //        }


        //    }
        //    (view.Page as DiagramPage).InvalidateMeasure();
        //    (view.Page as DiagramPage).InvalidateArrange();
        //}
        protected override Size ArrangeOverride(Size arrangeSize)
        {
            Size actualSize = base.ArrangeOverride(arrangeSize);
            foreach (FrameworkElement ele in this.Children)
            {
                //if (ele is ContentPresenter && page != null)
                //{
                //    ele.Arrange(new Rect(new Point(page.Left - HorizontalOffset, page.Top - VerticalOffset), ele.DesiredSize));
                //    //ele.Arrange(new Rect(new Point(-HorizontalOffset, - VerticalOffset), new Size(ele.DesiredSize.Width + page.Left, ele.DesiredSize.Height + page.Top)));
                //}
                //else
                if (view != null && view.Page != null)
                {
                    DiagramPage page = view.Page as DiagramPage;
                    if (!(ele is System.Windows.Shapes.Rectangle))
                    {
                        ele.Arrange(new Rect(new Point(page.Left - HorizontalOffset, page.Top - VerticalOffset), new Size(ele.ActualWidth, ele.ActualHeight)));
                        ele.InvalidateVisual();
                    }
                    else
                    {
                        ele.Arrange(new Rect(new Point(-HorizontalOffset, -VerticalOffset), new Size(ele.ActualWidth, ele.ActualHeight)));
                    }
                }
            }
            VerifyScrollData(ButIHave, INeed);
            return arrangeSize;
        }


        //protected override Size MeasureOverride(Size availableSize)
        //{
        //    Size visualDesiredSize = new Size(0, 0);
        //    Size negativeSize = new Size(0, 0);
        //    foreach (UIElement ele in InternalChildren)
        //    {
        //        ele.Measure(availableSize);
        //        Rect rect = ele.TransformToVisual(this).TransformBounds(new Rect(ele.RenderSize));
        //        //if (ele is ICommon)
        //        {
        //            visualDesiredSize.Width = Math.Max(rect.Right, visualDesiredSize.Width);
        //            visualDesiredSize.Height = Math.Max(rect.Bottom, visualDesiredSize.Height);
        //        }
        //    }

        //    return visualDesiredSize;
        //}

        //protected override Size ArrangeOverride(Size arrangeSize)
        //{
        //    Size actualSize = base.ArrangeOverride(arrangeSize);
        //    foreach (UIElement element in this.InternalChildren)
        //    {

        //        Size dSize;
        //        dSize = element.DesiredSize;
        //        if (element is DiagramViewGrid)
        //        {
        //            {
        //                //element.Arrange(new Rect(offsetX, offsetY, this.DesiredSize.Width, this.DesiredSize.Height));
        //            }
        //        }
        //        else
        //        {
        //            element.Arrange(new Rect(0, 0, dSize.Width, dSize.Height));
        //        }
        //    }


        //    VerifyScrollData(ButIHave, INeed);
        //    return arrangeSize;
        //    //return arrangeSize;
        //}

    }
}
