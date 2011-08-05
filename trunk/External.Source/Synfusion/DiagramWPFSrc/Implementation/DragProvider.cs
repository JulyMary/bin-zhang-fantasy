// <copyright file="DragProvider.cs" company="Syncfusion">
// Copyright Syncfusion Inc. 2001 - 2011. All rights reserved.
// Use of this code is subject to the terms of our license.
// A copy of the current license can be obtained at any time by e-mailing
// licensing@syncfusion.com. Any infringement will be prosecuted under
// applicable laws. 
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Interop;
using System.Reflection;
using System.IO;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.ComponentModel;

namespace Syncfusion.Windows.Diagram
{
    /// <summary>
    /// Represents Node Drag class.
    /// </summary>
#if SyncfusionFramework4_0
    [DesignTimeVisible(false)]
#endif
    public class DragProvider : Thumb
    {
        #region Class variables

        /// <summary>
        /// Used to store the cursor used.
        /// </summary>
        private Cursor m_cursor;

        /// <summary>
        /// Used to store the drag delta.
        /// </summary>
        private Point dragdelta;

        /// <summary>
        /// Represents the rotate transform.
        /// </summary>
        private RotateTransform rotateTransform;

        /// <summary>
        /// Used to hold the boolean value for IsDragging property.
        /// </summary>
        private static bool d = false;

        /// <summary>
        /// Used to store the DiagramControl instance.
        /// </summary>
        private DiagramControl dc;

        /// <summary>
        /// Used to store the old position of the nodes.
        /// </summary>
        private ObservableCollection<Point> oldpositioncollection = new ObservableCollection<Point>();

        /// <summary>
        /// Used to store the selected items.
        /// </summary>
        private ObservableCollection<INodeGroup> itemcollection = new ObservableCollection<INodeGroup>();

        /// <summary>
        /// Used to store the horizontal offset.
        /// </summary>
        private double scrolloffsetx = 0;

        /// <summary>
        /// Used to store the vertical offset.
        /// </summary>
        private double scrolloffsety = 0;

        /// <summary>
        /// Checks if drag operation is performed.
        /// </summary>
        private bool isdragged = false;

        #endregion

        #region Initialization

        /// <summary>
        /// Initializes a new instance of the <see cref="DragProvider"/> class.
        /// </summary>
        public DragProvider()
        {
            DragDelta += new DragDeltaEventHandler(DragProvider_DragDelta);
            DragStarted += new DragStartedEventHandler(DragProvider_DragStarted);
            DragCompleted += new DragCompletedEventHandler(DragProvider_DragCompleted);
        }

        /// <summary>
        /// Handles the DragStarted event of the DragProvider control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Controls.Primitives.DragStartedEventArgs"/> instance containing the event data.</param>
        private void DragProvider_DragStarted(object sender, DragStartedEventArgs e)
        {
            scrolloffsetx = dc.View.Scrollviewer.HorizontalOffset;
            scrolloffsety = dc.View.Scrollviewer.VerticalOffset;
            //dc.View.IsExeOnce = false;
            IShape node = this.DataContext as Node;
            if (node != null)
            {
                (node as Node).m_MouseMoving = true;
            }
            //if ((node as Node).LogicalOffsetX > 0 && !dc.View.IsJustScrolled)
            //    dc.View.IsMouseUponly = false;
            if (node != null && dc != null && dc.Model != null)
            {                
                pre = Mouse.GetPosition(dc.View.Page);
                foreach (Node n in dc.Model.Nodes)
                {
                    n.m_TempPosition = new Point(n.PxOffsetX, n.PxOffsetY);// MeasureUnitsConverter.ToPixels(new Point(n.LogicalOffsetX, n.LogicalOffsetY), n.MeasurementUnits);
                    n.m_TempSize = new Size(n.Width, n.Height);
                }
                foreach (LineConnector lc in dc.Model.Connections)
                {
                    lc.m_TempStartPoint = lc.PxStartPointPosition;// MeasureUnitsConverter.ToPixels(lc.StartPointPosition, lc.MeasurementUnit);
                    lc.m_TempEndPoint = lc.PxEndPointPosition;// MeasureUnitsConverter.ToPixels(lc.EndPointPosition, lc.MeasurementUnit);
                    lc.m_TempInerPts = new List<Point>();
                    if (lc.IntermediatePoints != null)
                    {
                        foreach (Point pt in lc.IntermediatePoints)
                        {
                            lc.m_TempInerPts.Add(pt);//MeasureUnitsConverter.ToPixels(pt, lc.MeasurementUnit));                            
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Handles the DragCompleted event of the DragProvider control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Controls.Primitives.DragCompletedEventArgs"/> instance containing the event data.</param>
        private void DragProvider_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            IShape node = this.DataContext as Node;
            if (DragProvider.Isdragging)
            {
                (node as Node).currentdragging = false;
                (node as Node).RaiseNodeDragEndEvent();
                DiagramView.IsOtherEvent = true;
            }
            if (node != null)
            {
                (node as Node).m_MouseMoving = true;
            }
            dc.View.IsDragged = false;
            if ((!(dc.View.undo || dc.View.redo)) && e.HorizontalChange > 0 && e.VerticalChange > 0)
            {
                dc.View.RedoStack.Clear();//
            }

            if (dc.View.Isdragdelta)
            {
                ////var nodes = dc.View.SelectionList.OfType<IShape>();
                isdragged = false;
                int i = 0;
                if (dc != null && dc.View != null && dc.View.UndoRedoEnabled)
                {
                    foreach (INodeGroup n in itemcollection)
                    {
                        ////dc.View.UndoStack.Push(oldpositioncollection[i]);
                        dc.View.UndoStack.Push((n as Node).OldOffset);//MeasureUnitsConverter.ToPixels((n as Node).OldOffset, (dc.View.Page as DiagramPage).MeasurementUnits));
                        dc.View.UndoStack.Push(n as Node);
                        dc.View.UndoStack.Push(itemcollection.Count);
                        dc.View.UndoStack.Push("Dragged");
                        i++;
                    }
                }
                itemcollection.Clear();
                //dc.View.IsScrollCheck = false;
                //dc.View.IsDupMouseWheeled = false;
                //dc.View.IsDupMousePressed = false;

                //DiagramView dview = dc.View;
                //double oldhoroff = 0;
                //if (dview != null)
                //{
                //    double d = dview.OldHoroffset - dview.Scrollviewer.HorizontalOffset;
                //    if (d == 0)
                //    {
                //        dview.HorThumbDragOffset = 0;
                //    }

                //    if (dview.IsScrollThumb || dview.IsMouseWheeled)
                //    {
                //        //(dview.Page as DiagramPage).Minleft = 0;
                //        if (dview.CurrentZoom >= 1)
                //        {
                //            //(dview.Page as DiagramPage).Dragleft += dview.HorThumbDragOffset / dview.CurrentZoom;
                //        }
                //        else
                //        {
                //            //(dview.Page as DiagramPage).Dragleft += dview.HorThumbDragOffset * dview.CurrentZoom;
                //        }

                //        oldhoroff = 0;
                //        if (!dview.IsExeOnce)
                //        {
                //            oldhoroff = dview.HorThumbDragOffset;
                //        }

                //        dview.IsExeOnce = true;
                //        dview.HorThumbDragOffset = 0;
                //    }
                //    else
                //    {
                //        //if (dview.IsScrolledRight && !(dview.Page as DiagramPage).IsPositive && (dview.Page as DiagramPage).Minleft == 0)
                //        //{
                //        //    if (dview.CurrentZoom >= 1)
                //        //    {
                //        //        //(dview.Page as DiagramPage).Dragleft += dview.HorThumbDragOffset / dview.CurrentZoom;
                //        //    }
                //        //    else
                //        //    {
                //        //        //(dview.Page as DiagramPage).Dragleft += dview.HorThumbDragOffset * dview.CurrentZoom;
                //        //    }

                //        //    dview.HorThumbDragOffset = 0;
                //        //    oldhoroff = 0;
                //        //}
                //        //else
                //        //{
                //        //    if ((dview.Page as DiagramPage).GreaterThanZero && node.LogicalOffsetX >= 0)
                //        //    {
                //        //        //(dview.Page as DiagramPage).Minleft = 0;
                //        //    }

                //        //    (dview.Page as DiagramPage).GreaterThanZero = false;

                //        //    if ((dview.Page as DiagramPage).Minleft != 0)
                //        //    {
                //        //        //(dview.Page as DiagramPage).Dragleft -= oldhoroff;
                //        //    }
                //        //    else
                //        //        if (!dview.IsOffsetxpositive)
                //        //        {
                //        //            if (dview.CurrentZoom >= 1)
                //        //            {
                //        //                //(dview.Page as DiagramPage).Dragleft += dview.HorThumbDragOffset / dview.CurrentZoom;
                //        //            }
                //        //            else
                //        //            {
                //        //                //(dview.Page as DiagramPage).Dragleft += dview.HorThumbDragOffset * dview.CurrentZoom;
                //        //            }
                //        //        }

                //        //    oldhoroff = 0;
                //        //    dview.HorThumbDragOffset = 0;
                //        //}
                //    }

                //    double oldveroff = 0;
                //    if (dview.IsScrollThumb || dview.IsMouseWheeled)
                //    {
                //        //(dview.Page as DiagramPage).MinTop = 0;
                //        //if (!dview.IsOffsetypositive)
                //        {
                //            if (dview.CurrentZoom >= 1)
                //            {
                //                //(dview.Page as DiagramPage).Dragtop += dview.VerThumbDragOffset / dview.CurrentZoom;
                //            }
                //            else
                //            {
                //                //(dview.Page as DiagramPage).Dragtop += dview.VerThumbDragOffset * dview.CurrentZoom;
                //            }
                //        }

                //        oldveroff = 0;
                //        if (!dview.IsExeOnceY)
                //        {
                //            oldveroff = dview.VerThumbDragOffset;
                //        }

                //        dview.IsExeOnceY = true;
                //        dview.VerThumbDragOffset = 0;
                //    }
                //    else
                //    {
                //        //if (dview.IsScrolledBottom && !(dview.Page as DiagramPage).IsPositiveY && (dview.Page as DiagramPage).MinTop == 0)
                //        //{
                //        //    if (dview.CurrentZoom >= 1)
                //        //    {
                //        //        //(dview.Page as DiagramPage).Dragtop += dview.VerThumbDragOffset / dview.CurrentZoom;
                //        //    }
                //        //    else
                //        //    {
                //        //        //(dview.Page as DiagramPage).Dragtop += dview.VerThumbDragOffset * dview.CurrentZoom;
                //        //    }

                //        //    dview.VerThumbDragOffset = 0;
                //        //    oldveroff = 0;
                //        //}
                //        //else
                //        //{
                //        //    double b = dview.VerThumbDragOffset;
                //        //    if ((dview.Page as DiagramPage).GreaterThanZeroY && node.LogicalOffsetY >= 0)
                //        //    {
                //        //        //(dview.Page as DiagramPage).MinTop = 0;
                //        //    }

                //        //    (dview.Page as DiagramPage).GreaterThanZeroY = false;

                //        //    //if ((dview.Page as DiagramPage).MinTop != 0)
                //        //    //{
                //        //    //    (dview.Page as DiagramPage).Dragtop -= oldveroff;
                //        //    //}
                //        //    //else
                //        //    //    if (!dview.IsOffsetypositive)
                //        //    //    {
                //        //    //        if (dview.CurrentZoom >= 1)
                //        //    //        {
                //        //    //            (dview.Page as DiagramPage).Dragtop += dview.VerThumbDragOffset / dview.CurrentZoom;
                //        //    //        }
                //        //    //        else
                //        //    //        {
                //        //    //            (dview.Page as DiagramPage).Dragtop += dview.VerThumbDragOffset * dview.CurrentZoom;
                //        //    //        }
                //        //    //    }

                //        //    oldveroff = 0;
                //        //    dview.VerThumbDragOffset = 0;
                //        //}
                //    }
                //}

                //double scrollendoffsetx = dc.View.Scrollviewer.HorizontalOffset;
                //double scrollendoffsety = dc.View.Scrollviewer.VerticalOffset;
                //double diffx = scrollendoffsetx - scrolloffsetx;
                //double diffy = scrollendoffsety - scrolloffsety;
                //(dc.View.Page as DiagramPage).LeastX -= diffx;
                //(dc.View.Page as DiagramPage).LeastY -= diffy;
                //dc.View.VerifyVirtualization();
                if (dc.View.EnableVirtualization)
                {
                    dc.View.ScrollGrid.callCalculate();
                }
                dc.View.Isdragdelta = false;
                //(dc.View.Page as DiagramPage).InvalidateMeasure();
                //(dc.View.Page as DiagramPage).InvalidateArrange();
                
            }
        }
        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the Measurement unit property.
        /// <value>
        /// Type: <see cref="MeasureUnits"/>
        /// Enum specifying the unit to be used.
        /// </value>
        /// </summary>
        internal MeasureUnits MeasurementUnits
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
        /// Gets or sets a value indicating whether the node is dragged.
        /// </summary>
        /// <value><c>true</c> if node is been dragged; otherwise, <c>false</c>.</value>
        internal static bool Isdragging
        {
            get
            {
                return d;
            }

            set
            {
                d = value;
            }
        }

        #endregion

        #region DPs

        /// <summary>
        /// Specifies the MeasurementUnits dependency property.
        /// </summary>
        public static readonly DependencyProperty MeasurementUnitsProperty = DependencyProperty.Register("MeasurementUnits", typeof(MeasureUnits), typeof(DragProvider), new PropertyMetadata(MeasureUnits.Pixel, new PropertyChangedCallback(OnMeasurementUnitChanged)));

        #endregion

        #region Implementation

        /// <summary>
        /// Called when [measurement unit changed].
        /// </summary>
        /// <param name="d">The DependencyObject.</param>
        /// <param name="e">The <see cref="System.Windows.DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void OnMeasurementUnitChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
        }

        /// <summary>
        /// Provides class handling for the <see cref="E:System.Windows.UIElement.MouseMove"/> event.
        /// </summary>
        /// <param name="e">The event data.</param>
        protected override void OnMouseMove(System.Windows.Input.MouseEventArgs e)
        {
            base.OnMouseMove(e);
            IShape node = this.DataContext as Node;
            IDiagramPage diagramPanel = VisualTreeHelper.GetParent(node as Node) as IDiagramPage;            
            dc = (node as Node).Nodediagramcontrol;
            if (dc == null)
            {
              dc=DiagramPage.GetDiagramControl((FrameworkElement)diagramPanel);
            }
            if (dc.View.IsPageEditable && (node as Node).AllowSelect && (node as Node).AllowMove)
            {
                if (dc.View.IsPanEnabled)
                {
                    if (BrowserInteropHelper.IsBrowserHosted)
                    {
                        this.Cursor = System.Windows.Input.Cursors.Hand;
                    }
                    else
                    {
                        Assembly ass = Assembly.GetExecutingAssembly();
                        Stream stream = ass.GetManifestResourceStream("Syncfusion.Windows.Diagram.Icons.Released.cur");
                        m_cursor = new Cursor(stream);
                        this.Cursor = m_cursor;
                    }
                }
                else
                {
                    this.Cursor = System.Windows.Input.Cursors.SizeAll;
                }
            }
            else
            {
                this.Cursor = System.Windows.Input.Cursors.Arrow;
            }
        }

        Point current;
        Point pre;
        double horChanage;
        double verChanage;

        /// <summary>
        /// Handles the DragDelta event of the DragProvider control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Controls.Primitives.DragDeltaEventArgs"/> instance containing the event data.</param>
        private void DragProvider_DragDelta(object sender, DragDeltaEventArgs e)
        {
            Node node = this.DataContext as Node;
            dc.View.IsDragged = true;
            //dc.View.IsMouseUponly = false;
            dc.View.Isdragdelta = true;
            //dc.View.IsJustScrolled = false;
            //dc.View.IsJustWheeled = false;
            VirtualizingPanel diagramPanel1 = (node as Node).Page as VirtualizingPanel; //VisualTreeHelper.GetParent(node as Node) as VirtualizingPanel;            
            if ((node as Node).AllowMove)
            {
                ////DragProvider.Isdragging = false;
                (node as Node).currentdragging = true;
                (node as Node).RaiseNodeDragStartEvent();
                DiagramView.IsOtherEvent = true;
                IDiagramPage diagramPanel = VisualTreeHelper.GetParent(node as Node) as IDiagramPage;
                DragProvider.Isdragging = true;
                dc = node.Nodediagramcontrol as DiagramControl;
                if (dc == null)
                {
                    dc = DiagramPage.GetDiagramControl((FrameworkElement)diagramPanel);
                }
                if (dc.View.IsPageEditable)
                {
                    current = Mouse.GetPosition(dc.View.Page as DiagramPage);
                    horChanage = current.X - pre.X;
                    verChanage = current.Y - pre.Y;
                    pre = current;
                    if (node is Layer && (node != null && diagramPanel != null))
                    {
                        Node n = this.DataContext as Node;
                        this.rotateTransform = n.RenderTransform as RotateTransform;
                        var nodes = diagramPanel.SelectionList.OfType<IShape>();
                        Node item = node;
                        ////foreach (IShape item in nodes)
                        {
                            if (!isdragged)
                            {
                                foreach (Node nod in (node as Layer).Nodes)
                                {
                                    (nod as Node).OldOffset = new Point((nod as Node).PxOffsetX, (nod as Node).PxOffsetY);
                                    itemcollection.Add(nod as IShape);
                                }
                            }

                            if (item is Layer)
                            {
                                DragGroup(dc, e, (item as Group), diagramPanel, new Point(0,0));
                            }

                            dragdelta = new Point(e.HorizontalChange, e.VerticalChange);
                            if (dc.View.SnapToVerticalGrid)
                            {
                                //dragdelta.X;
                            }
                            if (this.rotateTransform != null)
                            {
                                dragdelta = this.rotateTransform.Transform(dragdelta);
                            }

                            double offsetX = item.PxOffsetX;
                            double offsetY = item.PxOffsetY;
                            if (double.IsNaN(offsetX))
                            {
                                offsetX = 0;
                            }

                            if (double.IsNaN(offsetY))
                            {
                                offsetY = 0;
                            }

                            double dx = dragdelta.X;// MeasureUnitsConverter.FromPixels(dragdelta.X, this.MeasurementUnits);
                            double dy = dragdelta.Y;// MeasureUnitsConverter.FromPixels(dragdelta.Y, this.MeasurementUnits);
                            item.PxOffsetX = offsetX + dx;
                            item.PxOffsetY = offsetY + dy;

                            //if (dc.View.IsScrollCheck)
                            //{
                            //    //dc.View.IsScrollThumb = true;
                            //    dc.View.IsScrolled = true;
                            //}

                            //if (offsetX < item.LogicalOffsetX)
                            //{
                            //    (diagramPanel as DiagramPage).IsPositive = true;
                            //}
                            //else
                            //{
                            //    (diagramPanel as DiagramPage).IsPositive = false;
                            //}

                            //if (item.LogicalOffsetX - dc.View.Scrollviewer.HorizontalOffset > 0)
                            //{
                            //    (diagramPanel as DiagramPage).GreaterThanZero = true;
                            //}
                            //else
                            //{
                            //    (diagramPanel as DiagramPage).GreaterThanZero = false;
                            //}

                            //if (offsetY < item.LogicalOffsetY)
                            //{
                            //    (diagramPanel as DiagramPage).IsPositiveY = true;
                            //}
                            //else
                            //{
                            //    (diagramPanel as DiagramPage).IsPositiveY = false;
                            //}

                            //if (item.LogicalOffsetY - dc.View.Scrollviewer.VerticalOffset > 0)
                            //{
                            //    (diagramPanel as DiagramPage).GreaterThanZeroY = true;
                            //}
                            //else
                            //{
                            //    (diagramPanel as DiagramPage).GreaterThanZeroY = false;
                            //}

                            //if (item.LogicalOffsetX > 0)
                            //{
                            //    dc.View.IsOffsetxpositive = true;
                            //}
                            //else
                            //{
                            //    dc.View.IsOffsetxpositive = false;
                            //}

                            //if (item.LogicalOffsetY > 0)
                            //{
                            //    dc.View.IsOffsetypositive = true;
                            //}
                            //else
                            //{
                            //    dc.View.IsOffsetypositive = false;
                            //}
                        }

                        isdragged = true;
                    }
                    else if (node != null && diagramPanel != null && node.IsGrouped && !node.IsSelected)
                    {
                        foreach (Group g in (node as Node).Groups)
                        {
                            if (g.IsSelected && g.AllowMove)
                            {
                                Point delta;
                                delta = getDelta(node as Node, e);
                                CollectionExt gnodechildren = new CollectionExt();
                                //for (int i = 0; i < g.NodeChildren.Count; i++)
                                //{
                                //    gnodechildren.Add(g.NodeChildren[i] as INodeGroup);
                                //}

                                foreach (Group gnode in (node as Node).Groups)
                                {
                                    gnodechildren.Add(gnode);
                                }

                                Node n = this.DataContext as Node;
                                this.rotateTransform = n.RenderTransform as RotateTransform;                                
                                foreach (INodeGroup item in dc.View.SelectionList)
                                {
                                    if (!gnodechildren.Contains(item))
                                    {
                                        gnodechildren.Add(item);
                                    }
                                }

                                foreach (INodeGroup item in gnodechildren)
                                {                                    
                                    if (item is Group)
                                    {
                                        if (!(item is LineConnector))
                                        {
                                            if (!isdragged)
                                            {
                                                (item as Group).OldOffset = new Point((item as Group).PxOffsetX, (item as Group).PxOffsetY);
                                                itemcollection.Add(item as INodeGroup);
                                            }

                                            if (item is Group)
                                            {
                                                DragGroup(dc, e, (item as Group), diagramPanel, delta);
                                            }
                                            //setDragDelta(e);                                            
                                            double offsetX = (item as Group).m_TempPosition.X;
                                            double offsetY = (item as Group).m_TempPosition.Y;
                                            if (double.IsNaN(offsetX))
                                            {
                                                offsetX = 0;
                                            }

                                            if (double.IsNaN(offsetY))
                                            {
                                                offsetY = 0;
                                            }

                                            if (!(item as Group).m_MouseMoving)
                                            {
                                                (item as Group).m_TempPosition.X += dragdelta.X;
                                                (item as Group).m_TempPosition.Y += dragdelta.Y;
                                                (item as Group).PxOffsetX = (item as Group).m_TempPosition.X - delta.X;
                                                (item as Group).PxOffsetY = (item as Group).m_TempPosition.Y - delta.Y;
                                            }
                                            else
                                            {
                                                (item as Group).m_MouseMoving = false;
                                            }
                                        }
                                        else
                                            if (item is LineConnector)
                                            {
                                                LineConnector lc = (item as LineConnector);
                                                lc.m_TempStartPoint.X += dragdelta.X;
                                                lc.m_TempEndPoint.X += dragdelta.X;
                                                lc.m_TempStartPoint.Y += dragdelta.Y;
                                                lc.m_TempEndPoint.Y += dragdelta.Y;
                                                lc.PxStartPointPosition = new Point(lc.m_TempStartPoint.X - delta.X, lc.m_TempStartPoint.Y - delta.Y);
                                                lc.PxEndPointPosition = new Point(lc.m_TempEndPoint.X - delta.X, lc.m_TempEndPoint.Y - delta.Y);

                                                if (lc.IntermediatePoints != null)
                                                    for (int i = 0; i < lc.IntermediatePoints.Count; i++)
                                                    {
                                                        lc.m_TempInerPts[i] = new Point(lc.m_TempInerPts[i].X + dragdelta.X, lc.m_TempInerPts[i].Y + dragdelta.Y);
                                                        lc.IntermediatePoints[i] = new Point(lc.m_TempInerPts[i].X - delta.X, lc.m_TempInerPts[i].Y - delta.Y);
                                                    }
                                                lc.UpdateConnectorPathGeometry();
                                            }
                                    }

                                    else
                                        if(item is Node)
                                        {
                                            if (!isdragged)
                                            {
                                                (item as Node).OldOffset = new Point((item as Node).PxOffsetX, (item as Node).PxOffsetY);
                                                itemcollection.Add(item as INodeGroup);
                                            }                                            
                                            //setDragDelta(e);
                                            double offsetX = (item as Node).m_TempPosition.X;
                                            double offsetY = (item as Node).m_TempPosition.Y;
                                            if (double.IsNaN(offsetX))
                                            {
                                                offsetX = 0;
                                            }

                                            if (double.IsNaN(offsetY))
                                            {
                                                offsetY = 0;
                                            }

                                            if (!(item as Node).m_MouseMoving)
                                            {
                                                (item as Node).m_TempPosition.X += dragdelta.X;
                                                (item as Node).m_TempPosition.Y += dragdelta.Y;
                                                (item as Node).PxOffsetX = (item as Node).m_TempPosition.X - delta.X;
                                                (item as Node).PxOffsetY = (item as Node).m_TempPosition.Y - delta.Y;
                                            }
                                            else
                                            {
                                                (item as Node).m_MouseMoving = false;
                                            }
                                        }

                                    else
                                        if (item is LineConnector )
                                        {
                                            LineConnector lc = (item as LineConnector);
                                            lc.m_TempStartPoint.X += dragdelta.X;
                                            lc.m_TempEndPoint.X += dragdelta.X;
                                            lc.m_TempStartPoint.Y += dragdelta.Y;
                                            lc.m_TempEndPoint.Y += dragdelta.Y;
                                            lc.PxStartPointPosition = new Point(lc.m_TempStartPoint.X - delta.X, lc.m_TempStartPoint.Y - delta.Y);
                                            lc.PxEndPointPosition = new Point(lc.m_TempEndPoint.X - delta.X, lc.m_TempEndPoint.Y - delta.Y);

                                            if (lc.IntermediatePoints != null)
                                                for (int i = 0; i < lc.IntermediatePoints.Count; i++)
                                                {
                                                    lc.m_TempInerPts[i] = new Point(lc.m_TempInerPts[i].X + dragdelta.X, lc.m_TempInerPts[i].Y + dragdelta.Y);
                                                    lc.IntermediatePoints[i] = new Point(lc.m_TempInerPts[i].X - delta.X, lc.m_TempInerPts[i].Y - delta.Y);
                                                }
                                            lc.UpdateConnectorPathGeometry();
                                        }
                                }
                            }
                        }

                        isdragged = true;
                    }
                    else if (node != null && diagramPanel != null && node.IsSelected)
                    {                        
                        Node n = this.DataContext as Node;                        
                        this.rotateTransform = n.RenderTransform as RotateTransform;
                        var nodes = diagramPanel.SelectionList.OfType<IShape>().ToList();
                        var lines = diagramPanel.SelectionList.OfType<IEdge>().ToList();
                        //var tempnodes = diagramPanel.SelectionList.OfType<IShape>();

                        //foreach (Node ele in tempnodes)
                        //{
                        //    if (ele is Group)
                        //    {
                        //        nodes.AddRange((ele as Group).NodeChildren.OfType<IShape>());
                        //        nodes.Remove(ele);
                        //    }
                        //}

                        Point delta;
                        delta = getDelta(node as Node, e);
                        foreach (IShape item in nodes)
                        {
                            if (!isdragged)
                            {
                                (item as Node).OldOffset = new Point((item as Node).PxOffsetX, (item as Node).PxOffsetY);
                                itemcollection.Add(item as IShape);
                            }

                            if (item is Group)
                            {
                                DragGroup(dc, e, (item as Group), diagramPanel, delta);
                            }

                            //dragdelta = new Point(e.HorizontalChange, e.VerticalChange);
                            //if (this.rotateTransform != null)
                            //{
                            //    dragdelta = this.rotateTransform.Transform(dragdelta);
                            //}
                            //setDragDelta(e);
                            //double offsetX = item.LogicalOffsetX;
                            //double offsetY = item.LogicalOffsetY;
                            double offsetX = (item as Node).m_TempPosition.X;// MeasureUnitsConverter.FromPixels((item as Node).m_TempPosition.X, (item as Node).MeasurementUnits);
                            double offsetY = (item as Node).m_TempPosition.Y;// MeasureUnitsConverter.FromPixels((item as Node).m_TempPosition.Y, (item as Node).MeasurementUnits);
                            if (double.IsNaN(offsetX))
                            {
                                offsetX = 0;
                            }

                            if (double.IsNaN(offsetY))
                            {
                                offsetY = 0;
                            }

                            //double dx = MeasureUnitsConverter.FromPixels(dragdelta.X, this.MeasurementUnits);
                            //double dy = MeasureUnitsConverter.FromPixels(dragdelta.Y, this.MeasurementUnits);
                            //item.LogicalOffsetX = offsetX + dx;
                            //item.LogicalOffsetY = offsetY + dy;

                            if (!(item as Node).m_MouseMoving)
                            {
                                (item as Node).m_TempPosition.X += dragdelta.X;
                                (item as Node).m_TempPosition.Y += dragdelta.Y;
                                (item as Node).PxOffsetX = (item as Node).m_TempPosition.X - delta.X;//*/MeasureUnitsConverter.FromPixels((item as Node).m_TempPosition.X - delta.X, (item as Node).MeasurementUnits);*/
                                (item as Node).PxOffsetY = (item as Node).m_TempPosition.Y - delta.Y;//*/MeasureUnitsConverter.FromPixels((item as Node).m_TempPosition.Y - delta.Y, (item as Node).MeasurementUnits);*/
                            }
                            else
                            {
                                (item as Node).m_MouseMoving = false;
                            }

                            //if (dc.View.IsScrollCheck)
                            //{
                            //    //dc.View.IsScrollThumb = true;
                            //    dc.View.IsScrolled = true;
                            //}

                            //if (offsetX < item.LogicalOffsetX)
                            //{
                            //    (diagramPanel as DiagramPage).IsPositive = true;
                            //}
                            //else
                            //{
                            //    (diagramPanel as DiagramPage).IsPositive = false;
                            //}

                            //if (item.LogicalOffsetX - dc.View.Scrollviewer.HorizontalOffset > 0)
                            //{
                            //    (diagramPanel as DiagramPage).GreaterThanZero = true;
                            //}
                            //else
                            //{
                            //    (diagramPanel as DiagramPage).GreaterThanZero = false;
                            //}

                            //if (offsetY < item.LogicalOffsetY)
                            //{
                            //    (diagramPanel as DiagramPage).IsPositiveY = true;
                            //}
                            //else
                            //{
                            //    (diagramPanel as DiagramPage).IsPositiveY = false;
                            //}

                            //if (item.LogicalOffsetY - dc.View.Scrollviewer.VerticalOffset > 0)
                            //{
                            //    (diagramPanel as DiagramPage).GreaterThanZeroY = true;
                            //}
                            //else
                            //{
                            //    (diagramPanel as DiagramPage).GreaterThanZeroY = false;
                            //}

                            //if (item.LogicalOffsetX > 0)
                            //{
                            //    dc.View.IsOffsetxpositive = true;
                            //}
                            //else
                            //{
                            //    dc.View.IsOffsetxpositive = false;
                            //}

                            //if (item.LogicalOffsetY > 0)
                            //{
                            //    dc.View.IsOffsetypositive = true;
                            //}
                            //else
                            //{
                            //    dc.View.IsOffsetypositive = false;
                            //}
                        }

                        foreach (IEdge item in lines)
                        {                            
                            if (item is LineConnector)
                            {                                
                                LineConnector lc = (item as LineConnector);
                                lc.m_TempStartPoint.X += dragdelta.X;
                                lc.m_TempEndPoint.X += dragdelta.X;
                                lc.m_TempStartPoint.Y += dragdelta.Y;
                                lc.m_TempEndPoint.Y += dragdelta.Y;                               
                                lc.PxStartPointPosition = new Point(lc.m_TempStartPoint.X - delta.X, lc.m_TempStartPoint.Y - delta.Y);// MeasureUnitsConverter.FromPixels(new Point(lc.m_TempStartPoint.X - delta.X, lc.m_TempStartPoint.Y - delta.Y), lc.MeasurementUnit);
                                lc.PxEndPointPosition = new Point(lc.m_TempEndPoint.X - delta.X, lc.m_TempEndPoint.Y - delta.Y);// MeasureUnitsConverter.FromPixels(new Point(lc.m_TempEndPoint.X - delta.X, lc.m_TempEndPoint.Y - delta.Y), lc.MeasurementUnit);

                                if (lc.IntermediatePoints != null)
                                    for (int i = 0; i < lc.IntermediatePoints.Count; i++)
                                    {
                                        lc.m_TempInerPts[i] = new Point(lc.m_TempInerPts[i].X + dragdelta.X, lc.m_TempInerPts[i].Y + dragdelta.Y);
                                        lc.IntermediatePoints[i] = new Point(lc.m_TempInerPts[i].X - delta.X, lc.m_TempInerPts[i].Y - delta.Y);// MeasureUnitsConverter.FromPixels(new Point(lc.m_TempInerPts[i].X - delta.X, lc.m_TempInerPts[i].Y - delta.Y), lc.MeasurementUnit);
                                    }
                                lc.UpdateConnectorPathGeometry();
                            }
                        }
                        isdragged = true;
                    }

                    //(diagramPanel as DiagramPage).Hor = -dc.View.Scrollviewer.HorizontalOffset / dc.View.CurrentZoom;
                    //(diagramPanel as DiagramPage).Ver = -dc.View.Scrollviewer.VerticalOffset / dc.View.CurrentZoom;
                    dc.View.Ispositionchanged = true;
                    //diagramPanel.InvalidateMeasure();
                    //(node as Node).Arrange(new Rect(node.PxOffsetX, node.PxOffsetY, node.ActualWidth, node.ActualHeight));
                    (this.dc.View.Page as DiagramPage).InvalidateMeasure();
                    //(this.dc.View.Page as DiagramPage).InvalidateArrange();
                    //(diagramPanel1 as DiagramPage).InvalidateArrange();
                    //(diagramPanel1 as DiagramPage).InvalidateMeasure();
                    e.Handled = true;
                }
            }
        }
        private void setDragDelta(DragDeltaEventArgs e)
        {
            dragdelta = new Point(e.HorizontalChange, e.VerticalChange);
            if (dc.View.SnapToHorizontalGrid)
            {
                dragdelta.Y = verChanage;
            }
            if (dc.View.SnapToVerticalGrid)
            {
                dragdelta.X = horChanage;
            }
            if (this.rotateTransform != null)
            {
                Point pt = this.rotateTransform.Transform(new Point(e.HorizontalChange, e.VerticalChange));                
                if (!dc.View.SnapToHorizontalGrid)
                {
                    dragdelta.Y = pt.Y;
                }
                if (!dc.View.SnapToVerticalGrid)
                {
                    dragdelta.X = pt.X;
                }
            }
        }

        private Point getDelta(Node node, DragDeltaEventArgs e)
        {
            node.m_MouseMoving = true;
            setDragDelta(e);
            node.m_TempPosition.X += dragdelta.X;
            node.m_TempPosition.Y += dragdelta.Y;
            Point delta = new Point(node.m_TempPosition.X, node.m_TempPosition.Y);
            double offsetX = node.PxOffsetX;
            double offsetY = node.PxOffsetY;
            double dx = dragdelta.X;// MeasureUnitsConverter.FromPixels(dragdelta.X, this.MeasurementUnits);
            double dy = dragdelta.Y;// MeasureUnitsConverter.FromPixels(dragdelta.Y, this.MeasurementUnits);

            if (dc.View.SnapToHorizontalGrid)
            {
                node.PxOffsetY = Node.Round(node.m_TempPosition.Y, dc.View.PxSnapOffsetY);// MeasureUnitsConverter.FromPixels(Node.Round(node.m_TempPosition.Y, dc.View.PxSnapOffsetY), node.MeasurementUnits);
            }
            else
            {
                node.PxOffsetY = offsetY + dy;
                //node.LogicalOffsetY = MeasureUnitsConverter.FromPixels(node.m_TempPosition.Y, node.MeasurementUnits);
            }
            if (dc.View.SnapToVerticalGrid)
            {
                node.PxOffsetX = Node.Round(node.m_TempPosition.X, dc.View.PxSnapOffsetX);// MeasureUnitsConverter.FromPixels(Node.Round(node.m_TempPosition.X, dc.View.PxSnapOffsetX), node.MeasurementUnits);
            }
            else
            {
                node.PxOffsetX = offsetX + dx;
                //node.LogicalOffsetX = MeasureUnitsConverter.FromPixels(node.m_TempPosition.X, node.MeasurementUnits);
            }
            delta.X = delta.X - node.PxOffsetX;// MeasureUnitsConverter.ToPixels(node.LogicalOffsetX, node.MeasurementUnits);
            delta.Y = delta.Y - node.PxOffsetY;// MeasureUnitsConverter.ToPixels(node.LogicalOffsetY, node.MeasurementUnits);
            if (!dc.View.SnapToHorizontalGrid)
            {
                delta.Y = 0;
            }
            if (!dc.View.SnapToVerticalGrid)
            {
                delta.X = 0;
            }
            return delta;
        }

        /// <summary>
        /// Drags the group.
        /// </summary>
        /// <param name="dc">The DiagramControl object.</param>
        /// <param name="e">The <see cref="System.Windows.Controls.Primitives.DragDeltaEventArgs"/> instance containing the event data.</param>
        /// <param name="g">The group.</param>
        /// <param name="diagramPanel">The diagram panel.</param>
        private void DragGroup(DiagramControl dc, DragDeltaEventArgs e, Group g, IDiagramPage diagramPanel, Point delta)
        {
            CollectionExt gnodechildren = new CollectionExt();
            if (g is Layer)
            {
                foreach (INodeGroup item in (g as Layer).Nodes)
                {
                    if (!gnodechildren.Contains(item))
                    {
                        gnodechildren.Add(item);
                    }
                }
                foreach (INodeGroup item in (g as Layer).Lines)
                {
                    if (!gnodechildren.Contains(item))
                    {
                        gnodechildren.Add(item);
                    }
                }
            }
            else
            {
                gnodechildren = g.NodeChildren;
            }
            foreach (INodeGroup item in gnodechildren)
            {
                //dragdelta = new Point(e.HorizontalChange, e.VerticalChange);
                //if (this.rotateTransform != null)
                //{
                //    dragdelta = this.rotateTransform.Transform(dragdelta);
                //}
                setDragDelta(e);

                if (!(item is LineConnector))
                {
                    //double offsetX = (item as Node).LogicalOffsetX;
                    //double offsetY = (item as Node).LogicalOffsetY;
                    double offsetX = (item as Node).m_TempPosition.X;// MeasureUnitsConverter.FromPixels((item as Node).m_TempPosition.X, (item as Node).MeasurementUnits);
                    double offsetY = (item as Node).m_TempPosition.Y;// MeasureUnitsConverter.FromPixels((item as Node).m_TempPosition.Y, (item as Node).MeasurementUnits);

                    if (double.IsNaN(offsetX))
                    {
                        offsetX = 0;
                    }

                    if (double.IsNaN(offsetY))
                    {
                        offsetY = 0;
                    }

                    //double dx = MeasureUnitsConverter.FromPixels(dragdelta.X, this.MeasurementUnits);
                    //double dy = MeasureUnitsConverter.FromPixels(dragdelta.Y, this.MeasurementUnits);
                    //(item as Node).LogicalOffsetX = offsetX + dx;
                    //(item as Node).LogicalOffsetY = offsetY + dy;

                    if (!(item as Node).m_MouseMoving)
                    {
                        (item as Node).m_TempPosition.X += dragdelta.X;
                        (item as Node).m_TempPosition.Y += dragdelta.Y;
                        (item as Node).PxOffsetX = (item as Node).m_TempPosition.X - delta.X;//*/MeasureUnitsConverter.FromPixels((item as Node).m_TempPosition.X - delta.X, (item as Node).MeasurementUnits);*/
                        (item as Node).PxOffsetY = (item as Node).m_TempPosition.Y - delta.Y;//*/MeasureUnitsConverter.FromPixels((item as Node).m_TempPosition.Y - delta.Y, (item as Node).MeasurementUnits);*/
                    }
                    else
                    {
                        (item as Node).m_MouseMoving = false;
                    }

                    //if (dc.View.IsScrollCheck)
                    //{
                    //    //dc.View.IsScrollThumb = true;
                    //    dc.View.IsScrolled = true;
                    //}

                    //if (offsetX < (item as Node).LogicalOffsetX)
                    //{
                    //    (diagramPanel as DiagramPage).IsPositive = true;
                    //}
                    //else
                    //{
                    //    (diagramPanel as DiagramPage).IsPositive = false;
                    //}

                    //if ((item as Node).LogicalOffsetX - dc.View.Scrollviewer.HorizontalOffset > 0)
                    //{
                    //    (diagramPanel as DiagramPage).GreaterThanZero = true;
                    //}
                    //else
                    //{
                    //    (diagramPanel as DiagramPage).GreaterThanZero = false;
                    //}

                    //if (offsetY < (item as Node).LogicalOffsetY)
                    //{
                    //    (diagramPanel as DiagramPage).IsPositiveY = true;
                    //}
                    //else
                    //{
                    //    (diagramPanel as DiagramPage).IsPositiveY = false;
                    //}

                    //if ((item as Node).LogicalOffsetY - dc.View.Scrollviewer.VerticalOffset > 0)
                    //{
                    //    (diagramPanel as DiagramPage).GreaterThanZeroY = true;
                    //}
                    //else
                    //{
                    //    (diagramPanel as DiagramPage).GreaterThanZeroY = false;
                    //}

                    //if ((item as Node).LogicalOffsetX > 0)
                    //{
                    //    dc.View.IsOffsetxpositive = true;
                    //}
                    //else
                    //{
                    //    dc.View.IsOffsetxpositive = false;
                    //}

                    //if ((item as Node).LogicalOffsetY > 0)
                    //{
                    //    dc.View.IsOffsetypositive = true;
                    //}
                    //else
                    //{
                    //    dc.View.IsOffsetypositive = false;
                    //}
                }
                else
                    if (item is LineConnector)
                    {
                        //Point del = MeasureUnitsConverter.FromPixels(dragdelta, this.MeasurementUnits);

                        //(item as LineConnector).StartPointPosition = new Point((item as LineConnector).StartPointPosition.X + del.X, (item as LineConnector).StartPointPosition.Y + del.Y);
                        //(item as LineConnector).EndPointPosition = new Point((item as LineConnector).EndPointPosition.X + del.X, (item as LineConnector).EndPointPosition.Y + del.Y);
                        //if ((item as LineConnector).IntermediatePoints != null)
                        //{
                        //    for (int i = 0; i < (item as LineConnector).IntermediatePoints.Count; i++)
                        //    {
                        //        (item as LineConnector).IntermediatePoints[i] = new Point((item as LineConnector).IntermediatePoints[i].X + del.X, (item as LineConnector).IntermediatePoints[i].Y + del.Y);
                        //    }
                        //}
                        LineConnector lc = (item as LineConnector);
                        lc.m_TempStartPoint.X += dragdelta.X;
                        lc.m_TempEndPoint.X += dragdelta.X;
                        lc.m_TempStartPoint.Y += dragdelta.Y;
                        lc.m_TempEndPoint.Y += dragdelta.Y;

                        //double x = dragdelta.X - delta.X;
                        //double y = dragdelta.Y - delta.Y; Debug.WriteLine(delta);
                        lc.PxStartPointPosition = new Point(lc.m_TempStartPoint.X - delta.X, lc.m_TempStartPoint.Y - delta.Y);// MeasureUnitsConverter.FromPixels(new Point(lc.m_TempStartPoint.X - delta.X, lc.m_TempStartPoint.Y - delta.Y), lc.MeasurementUnit);
                        lc.PxEndPointPosition = new Point(lc.m_TempEndPoint.X - delta.X, lc.m_TempEndPoint.Y - delta.Y);//  MeasureUnitsConverter.FromPixels(new Point(lc.m_TempEndPoint.X - delta.X, lc.m_TempEndPoint.Y - delta.Y), lc.MeasurementUnit);

                        if (lc.IntermediatePoints != null)
                            for (int i = 0; i < lc.IntermediatePoints.Count; i++)
                            {
                                lc.m_TempInerPts[i] = new Point(lc.m_TempInerPts[i].X + dragdelta.X, lc.m_TempInerPts[i].Y + dragdelta.Y);
                                lc.IntermediatePoints[i] = new Point(lc.m_TempInerPts[i].X - delta.X, lc.m_TempInerPts[i].Y - delta.Y);// MeasureUnitsConverter.FromPixels(new Point(lc.m_TempInerPts[i].X - delta.X, lc.m_TempInerPts[i].Y - delta.Y), lc.MeasurementUnit);
                            }
                        lc.UpdateConnectorPathGeometry();
                    }
            }
        }
        #endregion
    }
}
