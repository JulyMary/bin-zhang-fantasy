﻿// <copyright file="LineConnectorAdorner.cs" company="Syncfusion">
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
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace Syncfusion.Windows.Diagram
{
    /// <summary>
    /// Represents the adorner used for Connectors.
    /// </summary>
    internal class LineConnectorAdorner : Adorner
    {

        #region Class fields
        private Style thumbStyle;
        private RotateTransform headAngle = new RotateTransform();
        private RotateTransform tailAngle = new RotateTransform();
        private RotateTransform vertexAngle = new RotateTransform();
        private bool dragged = false;
        private List<object> delayStack = new List<object>();
        private Point tempst = new Point();
        private Point tempend = new Point();

        private bool deletingMode = false;
        private Thumb Vertex = new Thumb();
        private List<Thumb> Vertexs = new List<Thumb>();
        private int vertexIndex = -1;
        private List<Point> InterPts = new List<Point>();
        /// <summary>
        /// Used to store the line start point position. 
        /// </summary>
        private Point startpos = new Point();

        /// <summary>
        /// Used to store the line end point position. 
        /// </summary>
        private Point endpos = new Point();

        /// <summary>
        /// Used to store the horizontal scroll offset. 
        /// </summary>
        private double scrolloffsetx = 0;

        /// <summary>
        /// Used to store the vertical scroll offset. 
        /// </summary>
        private double scrolloffsety = 0;

        /// <summary>
        /// Represents the port visibility.
        /// </summary>
        private bool portvisibilitycheck = false;

        /// <summary>
        /// To check if it is a head node.
        /// </summary>
        private bool isheadnode;

        /// <summary>
        /// Represents the IDiagramPage instance.
        /// </summary>
        private IDiagramPage diagramPanel;

        /// <summary>
        /// Represents the adorner.
        /// </summary>
        private Canvas adornerPanel;

        /// <summary>
        /// Represents the line connector.
        /// </summary>
        private IEdge lineconnector;

        /// <summary>
        /// Represents the path geometry.
        /// </summary>
        private PathGeometry pathGeometry;

        /// <summary>
        /// Represents the fixed node.
        /// </summary>
        private Node fixedNodeConnection;

        /// <summary>
        /// Represents the movable node.
        /// </summary>
        private Node movableNodeConnection;

        /// <summary>
        /// Represents the hit node.
        /// </summary>
        private Node hitNodeConnector;

        /// <summary>
        /// Represents the head thumb.
        /// </summary>
        private Thumb headThumb;

        /// <summary>
        /// Represents the tail thumb.
        /// </summary>
        private Thumb tailThumb;

        /// <summary>
        /// Represents the drawing pen.
        /// </summary>
        private Pen drawingPen;

        /// <summary>
        /// Represents the VisualCollection
        /// </summary>
        private VisualCollection visualCollection;

        /// <summary>
        /// Represents the previously hit node.
        /// </summary>
        private Node previousHitNode = null;

        /// <summary>
        /// Represents the <see cref="DiagramControl"/> instance.
        /// </summary>
        private DiagramControl dc;

        /// <summary>
        /// Represents the <see cref="ConnectionPort"/> which was hit.
        /// </summary>
        private ConnectionPort m_hitPort;

        /// <summary>
        /// Represents the <see cref="ConnectionPort"/> which was previously hit.
        /// </summary>
        private ConnectionPort previoushitport = null;

        /// <summary>
        /// Represents the center <see cref="ConnectionPort"/> .
        /// </summary>
        private ConnectionPort centerport;

        /// <summary>
        /// Specifies if the center port was hit.
        /// </summary>
        private bool centerhit = false;

        /// <summary>
        /// Specifies the ConnectorPoint value.
        /// </summary>
        private bool m_connpoint = false;

        /// <summary>
        /// Specifies if the thumb is head thumb.
        /// </summary>
        private bool m_head = false;

        /// <summary>
        /// Specifies if the thumb is tail thumb.
        /// </summary>
        private bool m_tail = false;
        /// <summary>
        /// Stores the drag distance.
        /// </summary>
        private Point dragdelta;

        internal bool isMouseup;

        #endregion

        #region Initialization

        /// <summary>
        /// Initializes static members of the <see cref="LineConnectorAdorner"/> class.
        /// </summary>
        static LineConnectorAdorner()
        {
            FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(LineConnectorAdorner), new FrameworkPropertyMetadata(typeof(LineConnectorAdorner)));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LineConnectorAdorner"/> class.
        /// </summary>
        /// <param name="dPanel">The <see cref="DiagramPage"/> instance.</param>
        /// <param name="edge">The edge connected to the node.</param>
        public LineConnectorAdorner(IDiagramPage dPanel, IEdge edge)
            : base(dPanel as Panel)
        {
            dc = DiagramPage.GetDiagramControl((FrameworkElement)dPanel);
            this.diagramPanel = dPanel;
            adornerPanel = new Canvas();
            this.visualCollection = new VisualCollection(this);
            this.visualCollection.Add(adornerPanel);
            this.lineconnector = edge;
            this.lineconnector.PropertyChanged += new PropertyChangedEventHandler(DecoratorPositionChanged);
            InvalidateThumbs();
            drawingPen = new Pen(lineconnector.LineStyle.Stroke, lineconnector.LineStyle.StrokeThickness);
            this.Unloaded += new RoutedEventHandler(ConnectionAdorner_Unloaded);
            this.Cursor = Cursors.Cross;
            drawingPen.LineJoin = PenLineJoin.Round;
            headThumb.SizeChanged += new SizeChangedEventHandler(headThumb_SizeChanged);
            tailThumb.SizeChanged += new SizeChangedEventHandler(tailThumb_SizeChanged);
        }


        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the node which is currently selected through HitTesting.
        /// </summary>
        /// <value>
        /// Type: <see cref="Node"/>
        /// The Node which was hit.
        /// </value>
        internal Node HitNodeConnector
        {
            get
            {
                return hitNodeConnector;
            }

            set
            {
                if (hitNodeConnector != value)
                {
                    hitNodeConnector = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the Hit port
        /// </summary>
        private ConnectionPort HitPort
        {
            get
            {
                return m_hitPort;
            }

            set
            {
                if (m_hitPort != value)
                {
                    m_hitPort = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to connect to another point on the page and indicates that no node was hit.
        /// </summary>
        internal bool ConnectorPoint
        {
            get { return m_connpoint; }
            set { m_connpoint = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to connect to another point on the page and indicates that no node was hit.
        /// </summary>
        internal bool IsHeadThumb
        {
            get { return m_head; }
            set { m_head = value; }
        }

        internal bool IsTailThumb
        {
            get { return m_tail; }
            set { m_tail = value; }
        }
        #endregion

        #region Overrides

        /// <summary>
        /// Gets the number of visual child elements within this element.
        /// </summary>
        /// <value>
        /// Count value.
        /// </value>
        protected override int VisualChildrenCount
        {
            get
            {
                return this.visualCollection.Count;
            }
        }

        /// <summary>
        /// Returns a child at the specified index from a collection of child elements.
        /// </summary>
        /// <param name="index">The index of the visual object in the
        /// VisualCollection.</param>
        /// <exception cref="ArgumentOutOfRangeException">Index must be 0 because only one child element is present.</exception>
        /// <returns>
        /// The child in the VisualCollection at the specified index
        /// value.
        /// </returns>
        protected override Visual GetVisualChild(int index)
        {
            return this.visualCollection[index];
        }

        /// <summary>
        /// Calls render of the LineConnectorAdorner.
        /// </summary>
        /// <param name="drawingcontext">The drawing instructions for a specific element. This context is provided to the layout system.</param>
        protected override void OnRender(DrawingContext drawingcontext)
        {
            base.OnRender(drawingcontext);
            drawingcontext.DrawGeometry(null, drawingPen, this.pathGeometry);
        }

        /// <summary>
        /// Positions child elements and determines a size for the control.
        /// </summary>
        /// <param name="finalSize">The final area within the parent
        /// that this element should use to arrange itself and its children.</param>
        /// <returns>
        /// The actual size used.
        /// </returns>
        protected override Size ArrangeOverride(Size finalSize)
        {
            ////if ((this.diagramPanel as DiagramPage).IsConnectorDropped)
            ////{
            ////    if (dc.View.Scrollviewer != null)
            ////    {
            ////        dc.View.Scrollviewer.ScrollToHorizontalOffset(Math.Abs((this.diagramPanel as DiagramPage).HorValue) * dc.View.CurrentZoom);
            ////        dc.View.Scrollviewer.ScrollToVerticalOffset(Math.Abs((this.diagramPanel as DiagramPage).VerValue) * dc.View.CurrentZoom);
            ////    }
            ////}

            adornerPanel.Arrange(new Rect(0, 0, this.diagramPanel.ActualWidth, this.diagramPanel.ActualHeight));
            return finalSize;
        }

        #endregion

        #region Events

        /// <summary>
        /// Calls DecoratorPositionChanged method of the instance, notifies of the dependency property value changes.
        /// </summary>
        /// <param name="sender"> object, the change occurs on.</param>
        /// <param name="e">Property change details, such as old value and new value.</param>
        private void DecoratorPositionChanged(object sender, PropertyChangedEventArgs e)
        {
            Canvas.SetLeft(headThumb, lineconnector.HeadDecoratorPosition.X);
            Canvas.SetTop(headThumb, lineconnector.HeadDecoratorPosition.Y);
            headAngle.Angle = (lineconnector as LineConnector).HeadDecoratorAngle;
            Canvas.SetLeft(tailThumb, lineconnector.TailDecoratorPosition.X);
            Canvas.SetTop(tailThumb, lineconnector.TailDecoratorPosition.Y);
            tailAngle.Angle = ((lineconnector as LineConnector).TailDecoratorAngle);
        }

        /// <summary>
        /// Handles the Unloaded event of the ConnectionAdorner control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void ConnectionAdorner_Unloaded(object sender, RoutedEventArgs e)
        {
            headThumb.DragDelta -= new DragDeltaEventHandler(Thumb_DragDelta);
            headThumb.DragStarted -= new DragStartedEventHandler(Thumb_DragStarted);
            headThumb.DragCompleted -= new DragCompletedEventHandler(Thumb_DragCompleted);
            tailThumb.DragDelta -= new DragDeltaEventHandler(Thumb_DragDelta);
            tailThumb.DragStarted -= new DragStartedEventHandler(Thumb_DragStarted);
            tailThumb.DragCompleted -= new DragCompletedEventHandler(Thumb_DragCompleted);
        }

        #endregion

        #region Methods

        private List<Thumb> temp = new List<Thumb>();
        internal void showPoints(List<Point> pts, LineConnector lc)
        {
            foreach (Thumb o in temp)
            {
                this.adornerPanel.Children.Remove(o);
            }
            temp = new List<Thumb>();
            foreach (Point v in pts)
            {
                Thumb Vertex = new Thumb();
                Vertex.Name = "test";
                Canvas.SetLeft(Vertex, v.X);
                Canvas.SetTop(Vertex, v.Y);

                this.adornerPanel.Children.Add(Vertex);

                Style thumbStyle;
                thumbStyle = (lc as LineConnector).VertexStyle;
                if (thumbStyle != null)
                {
                    Vertex.Style = thumbStyle;
                }

                temp.Add(Vertex);
            }
        }

        internal void UpdateVertexsPosition()
        {
            LineConnector lc = (LineConnector)lineconnector;
            InterPts = lc.IntermediatePoints;
            int i = 0;

            if (!deletingMode)
            {

                if (lc.IntermediatePoints != null && Vertexs.Count == lc.IntermediatePoints.Count)
                {
                    foreach (Thumb Vertex in Vertexs)
                    {
                        if (InterPts != null && InterPts.Count > i)
                        {
                            Canvas.SetLeft(Vertex, InterPts[i].X);//MeasureUnitsConverter.ToPixels(InterPts[i].X, lc.MeasurementUnit));
                            Canvas.SetTop(Vertex, InterPts[i].Y);//MeasureUnitsConverter.ToPixels(InterPts[i].Y, lc.MeasurementUnit));
                        }
                        i++;
                    }
                }
                else
                {
                    InvalidateVertexs();
                }

            }
        }

        internal void InvalidateVertexs()
        {
            LineConnector lc = (LineConnector)lineconnector;

            foreach (Thumb x in Vertexs)
            {
                this.adornerPanel.Children.Remove(x);
            }
            if (Vertexs.Count > 0)
            {
                Vertexs.Clear();
            }

            if (lc.IntermediatePoints != null)
            {
                InterPts = lc.IntermediatePoints;
                foreach (Point v in lc.IntermediatePoints)
                {
                    Vertex = new Thumb();
                    Vertex.Name = "Vertex";

                    Canvas.SetLeft(Vertex, v.X);// MeasureUnitsConverter.ToPixels(v.X, lc.MeasurementUnit));
                    Canvas.SetTop(Vertex, v.Y);// MeasureUnitsConverter.ToPixels(v.Y, lc.MeasurementUnit));
                    Style thumbStyle;
                    this.adornerPanel.Children.Add(Vertex);
                    thumbStyle = (lc as LineConnector).VertexStyle;
                    if (thumbStyle != null && lc.ConnectorType != ConnectorType.Bezier)
                    {
                        Vertex.Style = thumbStyle;
                    }
                    if (!(lc as LineConnector).IsVertexVisible || lc.ConnectorType == ConnectorType.Bezier)
                        Vertex.Visibility = Visibility.Collapsed;
                    if ((lc as LineConnector).IsVertexMovable)
                    {
                        Vertex.SizeChanged += new SizeChangedEventHandler(Vertex_SizeChanged);
                        Vertex.DragDelta += new DragDeltaEventHandler(Thumb_DragDelta);//(Vertex_DragDelta);
                        Vertex.DragStarted += new DragStartedEventHandler(Thumb_DragStarted);
                        Vertex.DragCompleted += new DragCompletedEventHandler(Thumb_DragCompleted);
                        Vertex.MouseDoubleClick += new MouseButtonEventHandler(Vertex_MouseDoubleClick);
                        Vertex.MouseRightButtonUp += new MouseButtonEventHandler(Vertex_MouseRightButtonUp);
                    }
                    Vertexs.Add(Vertex);
                }
            }
        }

        void Vertex_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Style thumbStyle = (lineconnector as LineConnector).FindResource("ConnectorAdornerVertexStyle") as Style;

            foreach (Thumb vertex in Vertexs)
            {
                if (lineconnector != null)
                {
                    vertexAngle = new RotateTransform(45, 0, 0);
                    TranslateTransform vtt = new TranslateTransform(-vertex.Width / 2, -vertex.Height / 2);
                    TransformGroup vtg = new TransformGroup();
                    vtg.Children.Add(vtt);
                    vtg.Children.Add(vertexAngle);
                    vertex.RenderTransform = vtg;
                }
            }
        }


        /// <summary>
        /// Updates the style.
        /// </summary>
        internal void updateThumbStyle()
        {
            LineConnector connector = lineconnector as LineConnector;

            if ((lineconnector as LineConnector).DecoratorAdornerStyle != null)
            {
                thumbStyle = (lineconnector as LineConnector).DecoratorAdornerStyle;
            }
            else
            {
                thumbStyle = (lineconnector as Control).FindResource("ConnectorAdornerThumbStyle") as Style;

            }

            if (thumbStyle != null)
            {
                headThumb.Style = thumbStyle;
                tailThumb.Style = thumbStyle;
            }

            headThumb.Visibility = connector.IsHeadMovable ? Visibility.Visible : System.Windows.Visibility.Collapsed;
            tailThumb.Visibility = connector.IsTailMovable ? Visibility.Visible : System.Windows.Visibility.Collapsed;

        }

        /// <summary>
        /// Invalidates the thumbs.
        /// </summary>
        private void InvalidateThumbs()
        {
            ////Initialize the thumbs
            headThumb = new Thumb();
            tailThumb = new Thumb();
            headThumb.Name = "HeadThumb";
            tailThumb.Name = "TailThumb";
            
            updateThumbStyle();
            
            Canvas.SetLeft(headThumb, lineconnector.HeadDecoratorPosition.X);
            Canvas.SetTop(headThumb, lineconnector.HeadDecoratorPosition.Y);

            headAngle.Angle = (lineconnector as LineConnector).HeadDecoratorAngle;

            this.adornerPanel.Children.Add(headThumb);

            Canvas.SetLeft(tailThumb, lineconnector.TailDecoratorPosition.X);
            Canvas.SetTop(tailThumb, lineconnector.TailDecoratorPosition.Y);
            tailAngle.Angle = (lineconnector as LineConnector).TailDecoratorAngle;

            this.adornerPanel.Children.Add(tailThumb);
            InvalidateVertexs();

            ////EventHandlers
            headThumb.DragDelta += new DragDeltaEventHandler(Thumb_DragDelta);
            headThumb.DragStarted += new DragStartedEventHandler(Thumb_DragStarted);
            headThumb.DragCompleted += new DragCompletedEventHandler(Thumb_DragCompleted);
            tailThumb.DragDelta += new DragDeltaEventHandler(Thumb_DragDelta);
            tailThumb.DragStarted += new DragStartedEventHandler(Thumb_DragStarted);
            tailThumb.DragCompleted += new DragCompletedEventHandler(Thumb_DragCompleted);
            headThumb.MouseDoubleClick += new MouseButtonEventHandler(HeadThumb_MouseDoubleClick);
            tailThumb.MouseDoubleClick += new MouseButtonEventHandler(TailThumb_MouseDoubleClick);
            headThumb.MouseRightButtonUp += new MouseButtonEventHandler(Thumb_MouseRightButtonUp);
            tailThumb.MouseRightButtonUp += new MouseButtonEventHandler(Thumb_MouseRightButtonUp);
        }

        /// <summary>
        /// Handles the MouseRightButtonUp event of the Thumb control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Input.MouseButtonEventArgs"/> instance containing the event data.</param>
        private void Thumb_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (dc != null && dc.View != null && (lineconnector as LineConnector) != null)
            {
                if (dc.View.Page != null && (dc.View.Page is DiagramPage))
                {
                    TimeSpan diff = DateTime.Now.Subtract(dc.View.timeRightClick);
                    int d = diff.CompareTo(new TimeSpan(0, 0, 0, 0, 500));
                    if (d == 1)
                    {

                        if (dc.View.IsPageEditable && dc.View.LineConnectorContextMenu == null && (lineconnector as LineConnector).ContextMenu == null)
                        {
                            MenuItem m = new MenuItem();
                            m.Header = "Order";
                            MenuItem m1 = new MenuItem();
                            m1.Header = "Bring To Front";
                            m1.Click += new RoutedEventHandler(M1_Click);
                            m.Items.Add(m1);
                            MenuItem m2 = new MenuItem();
                            m2.Header = "Bring Forward";
                            m2.Click += new RoutedEventHandler(M2_Click);
                            m.Items.Add(m2);
                            MenuItem m3 = new MenuItem();
                            m3.Header = "Send Backward";
                            m3.Click += new RoutedEventHandler(M3_Click);
                            m.Items.Add(m3);
                            MenuItem m4 = new MenuItem();
                            m4.Header = "Send To Back";
                            m4.Click += new RoutedEventHandler(M4_Click);
                            m.Items.Add(m4);

                            MenuItem group = new MenuItem();
                            group.Header = "Grouping";
                            MenuItem g1 = new MenuItem();
                            g1.Header = "Group";
                            g1.Click += new RoutedEventHandler(G1_Click);
                            group.Items.Add(g1);
                            MenuItem g2 = new MenuItem();
                            g2.Header = "Ungroup";
                            g2.Click += new RoutedEventHandler(G2_Click);
                            group.Items.Add(g2);

                            MenuItem del = new MenuItem();
                            del.Header = "Delete";
                            del.Click += new RoutedEventHandler(Del_Click);

                            this.ContextMenu = new ContextMenu();
                            this.ContextMenu.Items.Add(m);
                            this.ContextMenu.Items.Add(group);
                            this.ContextMenu.Items.Add(del);

                            if (lineconnector != null && lineconnector.IsSelected)
                            {
                                m1.IsEnabled = true;
                                m2.IsEnabled = true;
                                m3.IsEnabled = true;
                                m4.IsEnabled = true;
                                del.IsEnabled = true;
                            }
                            else
                            {
                                m1.IsEnabled = false;
                                m2.IsEnabled = false;
                                m3.IsEnabled = false;
                                m4.IsEnabled = false;
                                del.IsEnabled = false;
                            }

                            if (lineconnector != null && (lineconnector as LineConnector).IsGrouped)
                            {
                                foreach (Group g in (lineconnector as LineConnector).Groups)
                                {
                                    if (g.IsSelected)
                                    {
                                        del.IsEnabled = true;
                                    }
                                }
                            }

                            if (dc.View.SelectionList.Count > 1 && !(lineconnector as LineConnector).IsGrouped)
                            {
                                g1.IsEnabled = true;
                            }
                            else
                            {
                                foreach (INodeGroup item in dc.View.SelectionList)
                                {
                                    if (item is Group)
                                    {
                                        foreach (INodeGroup n in (item as Group).NodeChildren)
                                        {
                                            foreach (INodeGroup node in dc.View.SelectionList)
                                            {
                                                if (!(item as Group).NodeChildren.Contains(node) && !(node is Group))
                                                {
                                                    g1.IsEnabled = true;
                                                    break;
                                                }
                                                else
                                                {
                                                    g1.IsEnabled = false;
                                                }
                                            }
                                        }
                                    }
                                }
                            }

                            if (dc.View != null && dc.View.SelectionList.Count <= 1)
                            {
                                g1.IsEnabled = false;
                            }
                        }
                        else
                        {
                            if ((lineconnector as LineConnector).ContextMenu == null)
                            {
                                this.ContextMenu = dc.View.LineConnectorContextMenu;
                            }
                            else
                            {
                                this.ContextMenu = (lineconnector as LineConnector).ContextMenu;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Handles the Click event of the delete menu item.
        /// </summary>
        /// <param name="sender">The diagram view.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void Del_Click(object sender, RoutedEventArgs e)
        {
            DiagramCommandManager.Delete.Execute(dc.View.Page, dc.View);
        }

        /// <summary>
        /// Handles the Click event of the bring to front menu item.
        /// </summary>
        /// <param name="sender">The diagram view.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void M1_Click(object sender, RoutedEventArgs e)
        {
            DiagramCommandManager.BringToFront.Execute(dc.View.Page, dc.View);
        }

        /// <summary>
        /// Handles the Click event of the bring forward menu item.
        /// </summary>
        /// <param name="sender">The diagram view.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void M2_Click(object sender, RoutedEventArgs e)
        {
            DiagramCommandManager.MoveForward.Execute(dc.View.Page, dc.View);
        }

        /// <summary>
        /// Handles the Click event of the send backward menu item.
        /// </summary>
        /// <param name="sender">The diagram view.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void M3_Click(object sender, RoutedEventArgs e)
        {
            DiagramCommandManager.SendBackward.Execute(dc.View.Page, dc.View);
        }

        /// <summary>
        /// Handles the Click event of the send to back menu item.
        /// </summary>
        /// <param name="sender">The diagram view.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void M4_Click(object sender, RoutedEventArgs e)
        {
            DiagramCommandManager.SendToBack.Execute(dc.View.Page, dc.View);
        }

        /// <summary>
        /// Handles the Click event of the group menu item.
        /// </summary>
        /// <param name="sender">The diagram view.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void G1_Click(object sender, RoutedEventArgs e)
        {
            DiagramCommandManager.Group.Execute(dc.View.Page, dc.View);
        }

        /// <summary>
        /// Handles the Click event of the ungroup menu item.
        /// </summary>
        /// <param name="sender">The diagram view.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void G2_Click(object sender, RoutedEventArgs e)
        {
            DiagramCommandManager.Ungroup.Execute(dc.View.Page, dc.View);
        }

        /// <summary>
        /// Handles the MouseDoubleClick event of the tailThumb control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Input.MouseButtonEventArgs"/> instance containing the event data.</param>
        private void TailThumb_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (dc.View.IsPageEditable)
            {
                ConnRoutedEventArgs newEventArgs = new ConnRoutedEventArgs(lineconnector.HeadNode as Node, lineconnector.TailNode as Node, lineconnector as LineConnector);
                newEventArgs.RoutedEvent = DiagramView.ConnectorDoubleClickEvent;
                RaiseEvent(newEventArgs);
                if ((lineconnector as LineConnector).IsLabelEditable)
                {
                    (lineconnector as LineConnector).Labeledit();
                }
            }
        }

        /// <summary>
        /// Handles the MouseDoubleClick event of the headThumb control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Input.MouseButtonEventArgs"/> instance containing the event data.</param>
        private void HeadThumb_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (dc.View.IsPageEditable)
            {
                ConnRoutedEventArgs newEventArgs = new ConnRoutedEventArgs(lineconnector.HeadNode as Node, lineconnector.TailNode as Node, lineconnector as LineConnector);
                newEventArgs.RoutedEvent = DiagramView.ConnectorDoubleClickEvent;
                RaiseEvent(newEventArgs);
                if ((lineconnector as LineConnector).IsLabelEditable)
                {
                    (lineconnector as LineConnector).Labeledit();
                }
            }
        }

        /// <summary>
        /// Handles the DragStarted event of the Thumb control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Controls.Primitives.DragStartedEventArgs"/> instance containing the event data.</param>
        private void Thumb_DragStarted(object sender, DragStartedEventArgs e)
        {
            (lineconnector as LineConnector).isClicked = true;
            this.isMouseup = false;
            if (sender != null && sender is Thumb && ((Thumb)sender).Name == "Vertex" && lineconnector != null)
            {
                if (!(lineconnector as LineConnector).IsVertexMovable)
                {
                    return;
                }
            }
            dragged = false;
            dc.View.IsDragged = true;
            if (dc.View.IsPageEditable)
            {
                if (!dc.View.IsPanEnabled)
                {
                    scrolloffsetx = dc.View.Scrollviewer.HorizontalOffset;
                    scrolloffsety = dc.View.Scrollviewer.VerticalOffset;
                    this.HitPort = null;
                    this.HitNodeConnector = null;
                    this.pathGeometry = null;
                    this.Cursor = Cursors.Cross;
                    this.lineconnector.LineStyle.StrokeDashArray = new DoubleCollection(new double[] { 3, 3 });

                    if (sender == headThumb)
                    {
                        IsHeadThumb = true;
                        IsTailThumb = false;
                        if (lineconnector.TailNode != null)
                        {
                            fixedNodeConnection = lineconnector.TailNode as Node;
                        }

                        if (lineconnector.HeadNode != null)
                        {
                            movableNodeConnection = lineconnector.HeadNode as Node;
                        }

                        isheadnode = true;
                    }

                    if (sender == tailThumb)
                    {
                        IsHeadThumb = false;
                        IsTailThumb = true;
                        if (lineconnector.TailNode != null)
                        {
                            movableNodeConnection = lineconnector.TailNode as Node;
                        }
                        else
                        {
                            movableNodeConnection = null;
                        }

                        if (lineconnector.HeadNode != null)
                        {
                            fixedNodeConnection = lineconnector.HeadNode as Node;
                        }
                        else
                        {
                            fixedNodeConnection = null;
                        }

                        isheadnode = false;
                    }

                    if (((Thumb)sender).Name == "Vertex")
                    {
                        Vertex = (Thumb)sender;
                        for (int i = 0; i < InterPts.Count; i++)
                        {
                            double Left = InterPts[i].X;// MeasureUnitsConverter.ToPixels(InterPts[i].X, (lineconnector as LineConnector).MeasurementUnit);
                            double Top = InterPts[i].Y;// MeasureUnitsConverter.ToPixels(InterPts[i].Y, (lineconnector as LineConnector).MeasurementUnit);
                            if (Left == Canvas.GetLeft(Vertex) && Top == Canvas.GetTop(Vertex))
                            {
                                try
                                {
                                    if (i + 1 < InterPts.Count)
                                    {
                                        if (lineconnector.ConnectorType == ConnectorType.Orthogonal && (InterPts[i + 1].X == Left && InterPts[i + 1].Y == Top))
                                        {
                                            Vertex = Vertexs[i];
                                        }
                                    }
                                }
                                catch { }
                                vertexIndex = i;
                                IsHeadThumb = false;
                                IsTailThumb = false;
                                movableNodeConnection = null;
                                fixedNodeConnection = null;
                                if ((Keyboard.Modifiers & (ModifierKeys.Shift | ModifierKeys.Control)) == (ModifierKeys.Shift | ModifierKeys.Control))
                                {
                                    if (lineconnector.ConnectorType == ConnectorType.Straight)
                                    {
                                        DoStackOperationsForCSDelete(vertexIndex);
                                        (lineconnector as LineConnector).IntermediatePoints.Remove(InterPts[vertexIndex]);
                                    }
                                    else if (InterPts.Count >= 4)
                                    {
                                        if (vertexIndex == 0)
                                        {
                                            vertexIndex++;
                                            if (InterPts[vertexIndex].X == InterPts[vertexIndex + 1].X)
                                            {
                                                DoStackOperationsForCSDelete(vertexIndex - 1);
                                                InterPts[vertexIndex - 1] = new Point(InterPts[vertexIndex - 1].X, InterPts[vertexIndex + 1].Y);
                                                (lineconnector as LineConnector).IntermediatePoints.Remove(InterPts[vertexIndex]);
                                                (lineconnector as LineConnector).IntermediatePoints.Remove(InterPts[vertexIndex]);
                                            }
                                            else if (InterPts[vertexIndex].Y == InterPts[vertexIndex + 1].Y)
                                            {
                                                DoStackOperationsForCSDelete(vertexIndex);
                                                InterPts[vertexIndex + 2] = new Point(InterPts[vertexIndex].X, InterPts[vertexIndex + 2].Y);
                                                (lineconnector as LineConnector).IntermediatePoints.Remove(InterPts[vertexIndex]);
                                                (lineconnector as LineConnector).IntermediatePoints.Remove(InterPts[vertexIndex]);
                                            }
                                        }
                                        else if (vertexIndex == InterPts.Count - 1 || vertexIndex == InterPts.Count - 2)
                                        {
                                            if (vertexIndex == InterPts.Count - 1)
                                            {
                                                vertexIndex--;
                                            }
                                            if (InterPts[vertexIndex].X == InterPts[vertexIndex + 1].X)
                                            {
                                                DoStackOperationsForCSDelete(vertexIndex - 1);
                                                InterPts[vertexIndex - 1] = new Point(InterPts[vertexIndex - 1].X, InterPts[vertexIndex + 1].Y);
                                                (lineconnector as LineConnector).IntermediatePoints.Remove(InterPts[vertexIndex]);
                                                (lineconnector as LineConnector).IntermediatePoints.Remove(InterPts[vertexIndex]);
                                            }
                                            else if (InterPts[vertexIndex].Y == InterPts[vertexIndex + 1].Y)
                                            {
                                                DoStackOperationsForCSDelete(vertexIndex);
                                                if (InterPts.Count != vertexIndex + 2)
                                                    InterPts[vertexIndex + 2] = new Point(InterPts[vertexIndex].X, InterPts[vertexIndex + 2].Y);
                                                else
                                                    InterPts[vertexIndex - 1] = new Point(InterPts[vertexIndex + 1].X, InterPts[vertexIndex - 1].Y);
                                                (lineconnector as LineConnector).IntermediatePoints.Remove(InterPts[vertexIndex]);
                                                (lineconnector as LineConnector).IntermediatePoints.Remove(InterPts[vertexIndex]);
                                            }
                                        }
                                        else
                                        {
                                            if (InterPts[vertexIndex].X == InterPts[vertexIndex + 1].X)
                                            {
                                                DoStackOperationsForCSDelete(vertexIndex - 1);
                                                InterPts[vertexIndex - 1] = new Point(InterPts[vertexIndex - 1].X, InterPts[vertexIndex + 1].Y);
                                                (lineconnector as LineConnector).IntermediatePoints.Remove(InterPts[vertexIndex]);
                                                (lineconnector as LineConnector).IntermediatePoints.Remove(InterPts[vertexIndex]);
                                            }
                                            else if (InterPts[vertexIndex].Y == InterPts[vertexIndex + 1].Y)
                                            {
                                                DoStackOperationsForCSDelete(vertexIndex);
                                                InterPts[vertexIndex + 2] = new Point(InterPts[vertexIndex].X, InterPts[vertexIndex + 2].Y);
                                                (lineconnector as LineConnector).IntermediatePoints.Remove(InterPts[vertexIndex]);
                                                (lineconnector as LineConnector).IntermediatePoints.Remove(InterPts[vertexIndex]);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        (lineconnector as LineConnector).IntermediatePoints.Clear();
                                        (lineconnector as LineConnector).IntermediatePoints.Add(new Point(0, 0));
                                    }
                                    deletingMode = true;
                                    (lineconnector as LineConnector).UpdateConnectorPathGeometry();
                                }
                                break;
                            }
                        }
                    }
                    if ((Keyboard.Modifiers & (ModifierKeys.Shift | ModifierKeys.Control)) != (ModifierKeys.Shift | ModifierKeys.Control))
                    {
                        DoStackOperations(sender);
                    }
                }
            }
        }

        private void DoStackOperationsForCSDelete(int p)
        {
            if (dc != null && dc.View != null && dc.View.UndoRedoEnabled)
            {
                if (!(dc.View.undo || dc.View.redo))
                {
                    dc.View.RedoStack.Clear();
                }
                if (lineconnector.ConnectorType == ConnectorType.Orthogonal)
                {
                    if (p + 2 == InterPts.Count)
                        p--;
                    dc.View.UndoStack.Push(InterPts[p + 2]);//MeasureUnitsConverter.ToPixels(InterPts[p + 2], (this.lineconnector as LineConnector).MeasurementUnit));
                    dc.View.UndoStack.Push(InterPts[p + 1]);//MeasureUnitsConverter.ToPixels(InterPts[p + 1], (this.lineconnector as LineConnector).MeasurementUnit));
                    dc.View.UndoStack.Push(InterPts[p]);//MeasureUnitsConverter.ToPixels(InterPts[p], (this.lineconnector as LineConnector).MeasurementUnit));
                    dc.View.UndoStack.Push(p);
                    dc.View.UndoStack.Push(lineconnector as LineConnector);
                    dc.View.UndoStack.Push("CSDelete");
                }
                else if (lineconnector.ConnectorType == ConnectorType.Straight)
                {
                    dc.View.UndoStack.Push(InterPts[p]);//MeasureUnitsConverter.ToPixels(InterPts[p], (this.lineconnector as LineConnector).MeasurementUnit));
                    dc.View.UndoStack.Push(p);
                    dc.View.UndoStack.Push(lineconnector as LineConnector);
                    dc.View.UndoStack.Push("CSDelete");
                }
            }
        }

        /// <summary>
        /// Does the stack operations.
        /// </summary>
        /// <param name="sender">The sender.</param>
        private void DoStackOperations(object sender)
        {
            if (dc != null && dc.View != null && dc.View.UndoRedoEnabled)
            {
                if (!(dc.View.undo || dc.View.redo))
                {
                    dc.View.RedoStack.Clear();
                }
                if (sender == tailThumb)
                {
                    if (lineconnector.TailNode == null)
                    {
                        endpos = (lineconnector as LineConnector).PxEndPointPosition;
                        dc.View.UndoStack.Push(endpos);//MeasureUnitsConverter.ToPixels(endpos, (this.lineconnector as LineConnector).MeasurementUnit));
                        dc.View.UndoStack.Push("Tailend");
                        dc.View.UndoStack.Push(lineconnector as LineConnector);
                        dc.View.UndoStack.Push("Dragged");
                    }
                }
                else
                    if (sender == headThumb)
                    {
                        if (lineconnector.HeadNode == null)
                        {
                            startpos = (lineconnector as LineConnector).PxStartPointPosition;
                            dc.View.UndoStack.Push(startpos);//MeasureUnitsConverter.ToPixels(startpos, (this.lineconnector as LineConnector).MeasurementUnit));
                            dc.View.UndoStack.Push("Headend");
                            dc.View.UndoStack.Push(lineconnector as LineConnector);
                            dc.View.UndoStack.Push("Dragged");
                        }
                    }
                    else if (((Thumb)sender).Name == "Vertex")
                    {
                        delayStack = new List<object>();
                        if (lineconnector.ConnectorType == ConnectorType.Straight)
                        {
                            dc.View.UndoStack.Push(InterPts[vertexIndex]);//MeasureUnitsConverter.ToPixels(InterPts[vertexIndex], (this.lineconnector as LineConnector).MeasurementUnit));
                            dc.View.UndoStack.Push(vertexIndex);
                            dc.View.UndoStack.Push(1);
                            dc.View.UndoStack.Push("Vertex");
                            dc.View.UndoStack.Push(lineconnector as LineConnector);
                            dc.View.UndoStack.Push("Dragged");
                        }
                        if (lineconnector.ConnectorType == ConnectorType.Orthogonal)
                        {
                            if (vertexIndex == 0)
                            {
                                delayStack.Add(null);
                                delayStack.Add(1);

                                delayStack.Add(InterPts[0]);//MeasureUnitsConverter.ToPixels(InterPts[0], (this.lineconnector as LineConnector).MeasurementUnit));
                                delayStack.Add(2);

                                if (InterPts.Count > 1)
                                    delayStack.Add(InterPts[1]);//MeasureUnitsConverter.ToPixels(InterPts[1], (this.lineconnector as LineConnector).MeasurementUnit));
                                else
                                    delayStack.Add(null);
                                delayStack.Add(3);

                                delayStack.Add(3);
                                delayStack.Add("Vertex");
                                delayStack.Add(lineconnector as LineConnector);
                                delayStack.Add("Dragged");
                            }
                            else if (vertexIndex == InterPts.Count - 1 && vertexIndex != 0)
                            {
                                if (InterPts.Count >= 2)
                                    delayStack.Add(InterPts[vertexIndex - 1]);//MeasureUnitsConverter.ToPixels(InterPts[vertexIndex - 1], (this.lineconnector as LineConnector).MeasurementUnit));
                                else
                                    delayStack.Add(null);
                                delayStack.Add(InterPts.Count - 1 - 1);

                                delayStack.Add(InterPts[vertexIndex]);//MeasureUnitsConverter.ToPixels(InterPts[vertexIndex], (this.lineconnector as LineConnector).MeasurementUnit));
                                delayStack.Add(InterPts.Count - 1);

                                delayStack.Add(null);
                                delayStack.Add(InterPts.Count - 1 + 1);


                                delayStack.Add(3);
                                delayStack.Add("Vertex");
                                delayStack.Add(lineconnector as LineConnector);
                                delayStack.Add("Dragged");
                            }
                            else
                            {
                                dc.View.UndoStack.Push(InterPts[vertexIndex - 1]);//MeasureUnitsConverter.ToPixels(InterPts[vertexIndex - 1], (this.lineconnector as LineConnector).MeasurementUnit));
                                dc.View.UndoStack.Push(vertexIndex - 1);
                                dc.View.UndoStack.Push(InterPts[vertexIndex]);//MeasureUnitsConverter.ToPixels(InterPts[vertexIndex], (this.lineconnector as LineConnector).MeasurementUnit));
                                dc.View.UndoStack.Push(vertexIndex);
                                dc.View.UndoStack.Push(InterPts[vertexIndex + 1]);//MeasureUnitsConverter.ToPixels(InterPts[vertexIndex + 1], (this.lineconnector as LineConnector).MeasurementUnit));
                                dc.View.UndoStack.Push(vertexIndex + 1);
                                dc.View.UndoStack.Push(3);
                                dc.View.UndoStack.Push("Vertex");
                                dc.View.UndoStack.Push(lineconnector as LineConnector);
                                dc.View.UndoStack.Push("Dragged");
                            }
                        }
                    }
            }
        }

        /// <summary>
        /// Handles the DragDelta event of the Thumb control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Controls.Primitives.DragDeltaEventArgs"/> instance containing the event data.</param>
        private void Thumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            (lineconnector as LineConnector).isClicked = false;
            dragged = true;
            if (deletingMode == true)
            {
                return;
            }
            //dc.View.IsLinedragdelta = true;
            //dc.View.IsLinedragdeltay = true;
            dc.View.Isdragdelta = true;
            //dc.View.IsJustScrolled = false;
            //dc.View.IsJustWheeled = false;
            if (dc.View.IsPageEditable)
            {
                if (!dc.View.IsPanEnabled)
                {
                    Point pos = Mouse.GetPosition(this);
                    if (this.dc != null && this.dc.View != null)
                    {
                        if (dc.View.SnapToVerticalGrid)
                        {
                            pos.X = Node.Round(pos.X, dc.View.PxSnapOffsetX);
                        }
                        if (dc.View.SnapToHorizontalGrid)
                        {
                            pos.Y = Node.Round(pos.Y, dc.View.PxSnapOffsetY);
                        }
                    }
                    if (!this.isMouseup)
                    {
                        ConnDragRoutedEventArgs newEventArgs;
                        if (fixedNodeConnection != null && movableNodeConnection != null)
                        {
                            newEventArgs = new ConnDragRoutedEventArgs(fixedNodeConnection as Node, this.movableNodeConnection as Node, this.lineconnector as LineConnector);
                        }
                        else if (fixedNodeConnection == null && movableNodeConnection != null)
                        {
                            newEventArgs = new ConnDragRoutedEventArgs(this.movableNodeConnection as Node, this.lineconnector as LineConnector);
                        }
                        else if (fixedNodeConnection != null && movableNodeConnection == null)
                        {
                            newEventArgs = new ConnDragRoutedEventArgs(this.lineconnector as LineConnector, fixedNodeConnection as Node);
                        }
                        else
                        {
                            newEventArgs = new ConnDragRoutedEventArgs(this.lineconnector as LineConnector);
                        }

                        newEventArgs.RoutedEvent = DiagramView.ConnectorDragStartEvent;
                        RaiseEvent(newEventArgs);
                        this.isMouseup = true;
                    }

                    Point te = pos;// MeasureUnitsConverter.FromPixels(pos, (lineconnector as LineConnector).MeasurementUnit);

                    //Point te = MeasureUnitsConverter.FromPixels(pos, (lineconnector as LineConnector).MeasurementUnit);
                    bool foundNode = this.HitTesting(te);
                    Thumb temp = (Thumb)sender;
                    if (temp.Name == "Vertex")
                    {
                        foundNode = false;
                        centerhit = false;
                    }
                    if (foundNode && HitNodeConnector != null)
                    {
                        pos = Mouse.GetPosition(this);
                        if (this.dc != null && this.dc.View != null)
                        {
                            if (dc.View.SnapToVerticalGrid)
                            {
                                pos.X = Node.Round(pos.X, dc.View.PxSnapOffsetX);
                            }
                            if (dc.View.SnapToHorizontalGrid)
                            {
                                pos.Y = Node.Round(pos.Y, dc.View.PxSnapOffsetY);
                            }
                        }
                        Point t = pos;// MeasureUnitsConverter.FromPixels(pos, (this.lineconnector as LineConnector).MeasurementUnit);
                        this.pathGeometry = UpdateConnectorAdornerPathGeometry(t);
                    }
                    else
                    {
                        pos = Mouse.GetPosition(this);
                        if (this.dc != null && this.dc.View != null)
                        {
                            if (dc.View.SnapToVerticalGrid)
                            {
                                pos.X = Node.Round(pos.X, dc.View.PxSnapOffsetX);
                            }
                            if (dc.View.SnapToHorizontalGrid)
                            {
                                pos.Y = Node.Round(pos.Y, dc.View.PxSnapOffsetY);
                            }
                        }
                        Point t = pos;// MeasureUnitsConverter.FromPixels(pos, (this.lineconnector as LineConnector).MeasurementUnit);
                        this.pathGeometry = UpdateConnectorAdornerPathGeometry(t);
                        foreach (Node n in dc.Model.Nodes)
                        {
                            n.IsDragConnectionOver = false;
                        }
                    }

                    if (HitNodeConnector != null)
                    {
                        if (!centerhit)
                        {
                            foreach (ConnectionPort port in HitNodeConnector.Ports)
                            {
                                if (port != HitPort)
                                {
                                    port.IsDragOverPort = false;
                                }
                            }

                            if (centerport != null)
                            {
                                centerport.IsDragOverPort = false;
                            }
                        }
                        else
                        {
                            if (centerhit)
                            {
                                foreach (ConnectionPort port in HitNodeConnector.Ports)
                                {
                                    if (port.Name != null)
                                    {
                                        if (!port.Name.Equals("PART_Sync_CenterPort"))
                                        {
                                            port.IsDragOverPort = false;
                                        }
                                    }
                                }
                            }
                        }
                    }

                    dragdelta = new Point(e.HorizontalChange, e.VerticalChange);
                    double dx = dragdelta.X;// MeasureUnitsConverter.FromPixels(dragdelta.X, (lineconnector as LineConnector).MeasurementUnit);
                    double dy = dragdelta.Y;// MeasureUnitsConverter.FromPixels(dragdelta.Y, (lineconnector as LineConnector).MeasurementUnit);
                    double sX = (lineconnector as LineConnector).PxStartPointPosition.X;
                    double sY = (lineconnector as LineConnector).PxStartPointPosition.Y;
                    double eX = (lineconnector as LineConnector).PxEndPointPosition.X;
                    double eY = (lineconnector as LineConnector).PxEndPointPosition.Y;

                    if (double.IsNaN(sX))
                    {
                        sX = 0;
                    }

                    if (double.IsNaN(sY))
                    {
                        sY = 0;
                    }

                    if (double.IsNaN(eX))
                    {
                        eX = 0;
                    }

                    if (double.IsNaN(eY))
                    {
                        eY = 0;
                    }

                    Point stp = new Point((lineconnector as LineConnector).PxStartPointPosition.X + sX, (lineconnector as LineConnector).PxStartPointPosition.Y + sY);
                    Point enp = new Point((lineconnector as LineConnector).PxEndPointPosition.X + eX, (lineconnector as LineConnector).PxEndPointPosition.Y + eY);

                    //if (sX < stp.X && eX < enp.X)
                    //{
                    //    (dc.View.Page as DiagramPage).IsPositive = true;
                    //}
                    //else
                    //{
                    //    (dc.View.Page as DiagramPage).IsPositive = false;
                    //}

                    //if (sY < stp.Y && eY < enp.Y)
                    //{
                    //    (dc.View.Page as DiagramPage).IsPositiveY = true;
                    //}
                    //else
                    //{
                    //    (dc.View.Page as DiagramPage).IsPositiveY = false;
                    //}

                    //if ((lineconnector as LineConnector).StartPointPosition.X - dc.View.Scrollviewer.HorizontalOffset > 0 && (lineconnector as LineConnector).EndPointPosition.X - dc.View.Scrollviewer.HorizontalOffset > 0)
                    //{
                    //    (dc.View.Page as DiagramPage).GreaterThanZero = true;
                    //}
                    //else
                    //{
                    //    (dc.View.Page as DiagramPage).GreaterThanZero = false;
                    //}

                    //if ((lineconnector as LineConnector).StartPointPosition.Y - dc.View.Scrollviewer.VerticalOffset > 0 && (lineconnector as LineConnector).EndPointPosition.Y - dc.View.Scrollviewer.VerticalOffset > 0)
                    //{
                    //    (dc.View.Page as DiagramPage).GreaterThanZeroY = true;
                    //}
                    //else
                    //{
                    //    (dc.View.Page as DiagramPage).GreaterThanZeroY = false;
                    //}

                    //if (stp.X > 0 && enp.X > 0)
                    //{
                    //    dc.View.IsOffsetxpositive = true;
                    //}
                    //else
                    //{
                    //    dc.View.IsOffsetxpositive = false;
                    //}

                    //if (stp.Y > 0 && enp.Y > 0)
                    //{
                    //    dc.View.IsOffsetypositive = true;
                    //}
                    //else
                    //{
                    //    dc.View.IsOffsetypositive = false;
                    //}

                    //if (dc.View.CurrentZoom >= 1)
                    //{
                    //    (dc.View.Page as DiagramPage).Hor = -dc.View.Scrollviewer.HorizontalOffset / dc.View.CurrentZoom;
                    //    (dc.View.Page as DiagramPage).Ver = -dc.View.Scrollviewer.VerticalOffset / dc.View.CurrentZoom;
                    //}
                    //else
                    //{
                    //    (dc.View.Page as DiagramPage).Hor = -dc.View.Scrollviewer.HorizontalOffset * dc.View.CurrentZoom;
                    //    (dc.View.Page as DiagramPage).Ver = -dc.View.Scrollviewer.VerticalOffset * dc.View.CurrentZoom;
                    //}

                    this.InvalidateVisual();
                }
            }
        }
        /// <summary>
        /// Handles the SizeChanged event of the tailThumb control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.SizeChangedEventArgs"/> instance containing the event data.</param>
        void tailThumb_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            TranslateTransform ttt = new TranslateTransform(-tailThumb.ActualWidth + 2, -tailThumb.ActualHeight / 2);
            //   TranslateTransform ttt = new TranslateTransform(-12, -7);
            if (thumbStyle != null)
            {
                tailThumb.Style = thumbStyle;
            }
            if (lineconnector != null)
            {
                tailAngle = new RotateTransform(lineconnector.TailDecoratorAngle, 0, 0);
            }
            else
            {
                tailAngle = new RotateTransform(0, 0, 0);
            }
            TransformGroup ttg = new TransformGroup();
            ttg.Children.Add(ttt);
            ttg.Children.Add(tailAngle);
            tailThumb.RenderTransform = ttg;
        }

        /// <summary>
        /// Handles the SizeChanged event of the headThumb control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.SizeChangedEventArgs"/> instance containing the event data.</param>
        void headThumb_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            TranslateTransform htt = new TranslateTransform(-headThumb.ActualWidth + 2, -headThumb.ActualHeight / 2);

            // TranslateTransform htt = new TranslateTransform(-12, -7);
            if (lineconnector != null)
            {
                headAngle = new RotateTransform(lineconnector.HeadDecoratorAngle, 0, 0);

            }
            else
            {
                headAngle = new RotateTransform(0, 0, 0);
            }

            TransformGroup htg = new TransformGroup();
            htg.Children.Add(htt);
            htg.Children.Add(headAngle);
            headThumb.RenderTransform = htg;
        }
        /// <summary>
        /// Handles the DragCompleted event of the Thumb control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Controls.Primitives.DragCompletedEventArgs"/> instance containing the event data.</param>
        private void Thumb_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            if (dc.View.IsPageEditable && (lineconnector as LineConnector).isClicked)
            {
                ConnectorRoutedEventArgs NewEventArgs = new ConnectorRoutedEventArgs((lineconnector as LineConnector));
                NewEventArgs.RoutedEvent = DiagramView.ConnectorClickEvent;
                RaiseEvent(NewEventArgs);
            }
            Point pos = Mouse.GetPosition(this);
            (lineconnector as LineConnector).isClicked = false;
            if (this.dc != null && this.dc.View != null)
            {
                if (dc.View.SnapToVerticalGrid)
                {
                    pos.X = Node.Round(pos.X, dc.View.PxSnapOffsetX);
                }
                if (dc.View.SnapToHorizontalGrid)
                {
                    pos.Y = Node.Round(pos.Y, dc.View.PxSnapOffsetY);
                }
            }
            if (!dragged)
            {
                this.lineconnector.LineStyle.StrokeDashArray = null;
                if (deletingMode)
                {
                    InvalidateVertexs();
                }
                deletingMode = false;
                return;
            }
            ConnectorPoint = false;
            if (deletingMode)
            {
                (lineconnector as LineConnector).UpdateConnectorPathGeometry();
                if (lineconnector.ConnectorType == ConnectorType.Orthogonal)
                {
                    if (vertexIndex == 0)
                    {
                    }
                    else if (vertexIndex == Vertexs.Count - 1)
                    {
                    }
                    else
                    {
                    }
                }
                this.adornerPanel.Children.Remove(Vertex);
                deletingMode = false;
            }
            else
            {
                if (HitNodeConnector != null)
                {
                    if (lineconnector != null)
                    {
                        HitNodeConnector.IsDragConnectionOver = false;
                        if (portvisibilitycheck)
                        {
                            HitNodeConnector.PortVisibility = Visibility.Collapsed;
                            portvisibilitycheck = false;
                        }

                        if (centerport != null)
                        {
                            centerport.IsDragOverPort = false;
                        }

                        if (movableNodeConnection != null)
                        {
                            if (lineconnector.HeadNode != null && movableNodeConnection == lineconnector.HeadNode)
                            {
                                movableNodeConnection.OutEdges.Remove(lineconnector);
                            }
                            else if (lineconnector.TailNode != null && movableNodeConnection == lineconnector.TailNode)
                            {
                                movableNodeConnection.InEdges.Remove(lineconnector);
                            }

                            movableNodeConnection.Edges.Remove(lineconnector);
                        }

                        ////HitNodeConnector.Edges.Add(lineconnector);
                        if (IsTailThumb)
                        {
                            lineconnector.TailNode = this.HitNodeConnector;
                            (lineconnector as LineConnector).TailNodeReferenceNo = (lineconnector.TailNode as Node).ReferenceNo;
                            if (!centerhit)
                            {
                                foreach (ConnectionPort port in (lineconnector.TailNode as Node).Ports)
                                {
                                    port.IsDragOverPort = false;
                                    if (port == HitPort)
                                    {
                                        if (port.CenterPortReferenceNo != 0)
                                        {
                                            lineconnector.ConnectionTailPort = port;
                                            (lineconnector as LineConnector).TailPortReferenceNo = port.PortReferenceNo;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                lineconnector.ConnectionTailPort = null;
                            }
                        }
                        else if (IsHeadThumb)
                        {
                            lineconnector.HeadNode = this.HitNodeConnector;
                            (lineconnector as LineConnector).HeadNodeReferenceNo = (lineconnector.HeadNode as Node).ReferenceNo;
                            if (!centerhit)
                            {
                                foreach (ConnectionPort port in (lineconnector.HeadNode as Node).Ports)
                                {
                                    port.IsDragOverPort = false;
                                    if (port == HitPort)
                                    {
                                        if (port.CenterPortReferenceNo != 0)
                                        {
                                            lineconnector.ConnectionHeadPort = port;
                                            (lineconnector as LineConnector).HeadPortReferenceNo = port.PortReferenceNo;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                lineconnector.ConnectionHeadPort = null;
                            }
                        }
                    }

                    HitNodeConnector.IsDragConnectionOver = false;
                }

                if (HitNodeConnector == null)
                {
                    ConnectorPoint = true;
                    if (IsTailThumb)
                    {
                        if (lineconnector.ConnectorType == ConnectorType.Orthogonal && (lineconnector as LineConnector).IntermediatePoints.Count >= 2)
                            updateOrthogonalThumbs();
                        if (previousHitNode != null)
                        {
                            previousHitNode.InEdges.Remove(lineconnector);
                            previousHitNode.Edges.Remove(lineconnector);
                        }
                        (lineconnector as LineConnector).PxEndPointPosition = pos;// MeasureUnitsConverter.FromPixels(pos, (this.lineconnector as LineConnector).MeasurementUnit);
                    }
                    else if (IsHeadThumb)
                    {
                        if (lineconnector.ConnectorType == ConnectorType.Orthogonal && (lineconnector as LineConnector).IntermediatePoints.Count >= 2)
                            updateOrthogonalThumbs();
                        if (previousHitNode != null)
                        {
                            previousHitNode.OutEdges.Remove(lineconnector);
                            previousHitNode.Edges.Remove(lineconnector);
                        }
                        (lineconnector as LineConnector).PxStartPointPosition = pos;// MeasureUnitsConverter.FromPixels(pos, (this.lineconnector as LineConnector).MeasurementUnit);
                    }
                    else
                    {
                        if (lineconnector.ConnectorType == ConnectorType.Orthogonal)
                            updateOrthogonalThumbs();
                        InterPts[vertexIndex] = pos;// MeasureUnitsConverter.FromPixels(pos, (this.lineconnector as LineConnector).MeasurementUnit);
                        Canvas.SetLeft(Vertex, InterPts[vertexIndex].X);//MeasureUnitsConverter.ToPixels(InterPts[vertexIndex].X, (this.lineconnector as LineConnector).MeasurementUnit));
                        Canvas.SetTop(Vertex, InterPts[vertexIndex].Y);// MeasureUnitsConverter.ToPixels(InterPts[vertexIndex].Y, (this.lineconnector as LineConnector).MeasurementUnit));
                    }
                }
                (lineconnector as LineConnector).UpdateConnectorPathGeometry();
                ConnDragEndRoutedEventArgs newEventArgs;
                if (fixedNodeConnection != null && HitNodeConnector != null)
                {
                    newEventArgs = new ConnDragEndRoutedEventArgs(this.fixedNodeConnection as Node, this.HitNodeConnector as Node, this.lineconnector as LineConnector);
                }
                else if (fixedNodeConnection == null && HitNodeConnector != null)
                {
                    newEventArgs = new ConnDragEndRoutedEventArgs(this.HitNodeConnector as Node, this.lineconnector as LineConnector);
                }
                else if (fixedNodeConnection != null && HitNodeConnector == null)
                {
                    newEventArgs = new ConnDragEndRoutedEventArgs(this.lineconnector as LineConnector, this.fixedNodeConnection as Node);
                }
                else
                {
                    newEventArgs = new ConnDragEndRoutedEventArgs(this.lineconnector as LineConnector);
                }
                newEventArgs.RoutedEvent = DiagramView.ConnectorDragEndEvent;
                RaiseEvent(newEventArgs);
                //}
                //else
                //{
                //    ConnDragEndRoutedEventArgs newEventArgs;
                //    if (fixedNodeConnection != null && HitNodeConnector != null)
                //    {
                //        newEventArgs = new ConnDragEndRoutedEventArgs(this.fixedNodeConnection as Node, this.HitNodeConnector as Node, this.lineconnector as LineConnector);
                //    }
                //    else
                //        if (fixedNodeConnection == null && HitNodeConnector != null)
                //        {
                //            newEventArgs = new ConnDragEndRoutedEventArgs(this.HitNodeConnector as Node, this.lineconnector as LineConnector);
                //        }
                //        else if (fixedNodeConnection != null && HitNodeConnector == null)
                //        {
                //            newEventArgs = new ConnDragEndRoutedEventArgs(this.lineconnector as LineConnector, this.fixedNodeConnection as Node);
                //        }
                //        else
                //        {
                //            newEventArgs = new ConnDragEndRoutedEventArgs(this.lineconnector as LineConnector);
                //        }

                //    newEventArgs.RoutedEvent = DiagramView.ConnectorDragEndEvent;
                //    RaiseEvent(newEventArgs);
                //}
            }
            //this.InvalidateVertexs();
            if (lineconnector.ConnectorType != ConnectorType.Bezier)
            {
                this.UpdateVertexsPosition();
            }

            this.HitNodeConnector = null;
            this.HitPort = null;
            this.pathGeometry = null;
            this.lineconnector.LineStyle.StrokeDashArray = null;
            this.InvalidateVisual();
            (dc.View.Page as DiagramPage).InvalidateMeasure();
            //dc.View.Linehoffset = dc.View.Scrollviewer.HorizontalOffset;
            //dc.View.Linevoffset = dc.View.Scrollviewer.VerticalOffset;
            double scrollendoffsetx = dc.View.Scrollviewer.HorizontalOffset;
            double scrollendoffsety = dc.View.Scrollviewer.VerticalOffset;
            double diffx = scrollendoffsetx - scrolloffsetx;
            double diffy = scrollendoffsety - scrolloffsety;

            //(dc.View.Page as DiagramPage).LeastX -= diffx;
            //(dc.View.Page as DiagramPage).LeastY -= diffy;
        }

        private void updateOrthogonalThumbs()
        {
            Point pos = Mouse.GetPosition(this);
            if (this.dc != null && this.dc.View != null)
            {
                if (dc.View.SnapToVerticalGrid)
                {
                    pos.X = Node.Round(pos.X, dc.View.PxSnapOffsetX);
                }
                if (dc.View.SnapToHorizontalGrid)
                {
                    pos.Y = Node.Round(pos.Y, dc.View.PxSnapOffsetY);
                }
            }
            Point mousePt = pos;// MeasureUnitsConverter.FromPixels(pos, (this.lineconnector as LineConnector).MeasurementUnit);

            settempstPoints();
            settempendPoints();
            Point tempEnd = (lineconnector as LineConnector).PxEndPointPosition;
            if (tempEnd.Equals(new Point(0, 0)))
                tempEnd = tempend;
            Point tempStart = (lineconnector as LineConnector).PxStartPointPosition;
            if (tempStart.Equals(new Point(0, 0)))
                tempStart = tempst;
            if (IsHeadThumb)
            {
                if (Math.Abs(tempStart.X - InterPts[0].X)
                > Math.Abs(tempStart.Y - InterPts[0].Y))
                {
                    InterPts[0] = new Point(InterPts[0].X, mousePt.Y);
                }
                else
                {
                    InterPts[0] = new Point(mousePt.X, InterPts[0].Y);
                }
                Point t = InterPts[0];// MeasureUnitsConverter.ToPixels(InterPts[0], (this.lineconnector as LineConnector).MeasurementUnit);
                Canvas.SetLeft(Vertexs[0], t.X);
                Canvas.SetTop(Vertexs[0], t.Y);
            }
            else if (IsTailThumb)
            {
                if (Math.Abs(tempEnd.X - InterPts[InterPts.Count - 1].X)
                > Math.Abs(tempEnd.Y - InterPts[InterPts.Count - 1].Y))
                {
                    InterPts[InterPts.Count - 1] = new Point(InterPts[InterPts.Count - 1].X, mousePt.Y);
                }
                else
                {
                    InterPts[InterPts.Count - 1] = new Point(mousePt.X, InterPts[InterPts.Count - 1].Y);
                }
                Point t = InterPts[InterPts.Count - 1];// MeasureUnitsConverter.ToPixels(InterPts[InterPts.Count - 1], (this.lineconnector as LineConnector).MeasurementUnit);
                Canvas.SetLeft(Vertexs[InterPts.Count - 1], t.X);
                Canvas.SetTop(Vertexs[InterPts.Count - 1], t.Y);
            }
            else if (vertexIndex == 0)
            {
            }
            else if (vertexIndex == InterPts.Count - 1)
            {
            }
            else if (InterPts.Count >= 3)
            {
                if (InterPts[vertexIndex - 1].X == InterPts[vertexIndex].X)
                {
                    InterPts[vertexIndex - 1] = new Point(mousePt.X, InterPts[vertexIndex - 1].Y);
                    InterPts[vertexIndex + 1] = new Point(InterPts[vertexIndex + 1].X, mousePt.Y);

                    Point t = InterPts[vertexIndex - 1];// MeasureUnitsConverter.ToPixels(InterPts[vertexIndex - 1], (this.lineconnector as LineConnector).MeasurementUnit);
                    Canvas.SetLeft(Vertexs[vertexIndex - 1], t.X);
                    Canvas.SetTop(Vertexs[vertexIndex - 1], t.Y);

                    t = InterPts[vertexIndex + 1];// MeasureUnitsConverter.ToPixels(InterPts[vertexIndex + 1], (this.lineconnector as LineConnector).MeasurementUnit);
                    Canvas.SetLeft(Vertexs[vertexIndex + 1], t.X);
                    Canvas.SetTop(Vertexs[vertexIndex + 1], t.Y);
                }
                else
                {
                    InterPts[vertexIndex - 1] = new Point(InterPts[vertexIndex - 1].X, mousePt.Y);
                    InterPts[vertexIndex + 1] = new Point(mousePt.X, InterPts[vertexIndex + 1].Y);

                    Point t = InterPts[vertexIndex - 1];//  MeasureUnitsConverter.ToPixels(InterPts[vertexIndex - 1], (this.lineconnector as LineConnector).MeasurementUnit);
                    Canvas.SetLeft(Vertexs[vertexIndex - 1], t.X);
                    Canvas.SetTop(Vertexs[vertexIndex - 1], t.Y);

                    t = InterPts[vertexIndex + 1];// MeasureUnitsConverter.ToPixels(InterPts[vertexIndex + 1], (this.lineconnector as LineConnector).MeasurementUnit);
                    Canvas.SetLeft(Vertexs[vertexIndex + 1], t.X);
                    Canvas.SetTop(Vertexs[vertexIndex + 1], t.Y);
                }
                if (vertexIndex == 1)
                {
                }
                else
                {
                }
                if (vertexIndex == InterPts.Count - 2)
                {

                }
                else
                {

                }

            }

        }


        private void Vertex_DragStarted(object sender, DragStartedEventArgs e) { }
        private void Vertex_DragDelta(object sender, DragDeltaEventArgs e) { }
        private void Vertex_DragCompleted(object sender, DragCompletedEventArgs e) { }
        private void Vertex_MouseRightButtonUp(object sender, MouseButtonEventArgs e) { }
        private void Vertex_MouseDoubleClick(object sender, MouseButtonEventArgs e) { }


        /// <summary>
        /// Is called whenever the path geometry is to be updated.
        /// </summary>
        /// <param name="position">The endpoint of the connector</param>
        /// <returns>The updated PathGeometry.</returns>
        private PathGeometry UpdateConnectorAdornerPathGeometry(Point position)
        {
            Point pos = Mouse.GetPosition(this);
            if (this.dc != null && this.dc.View != null)
            {
                if (dc.View.SnapToVerticalGrid)
                {
                    pos.X = Node.Round(pos.X, dc.View.PxSnapOffsetX);
                }
                if (dc.View.SnapToHorizontalGrid)
                {
                    pos.Y = Node.Round(pos.Y, dc.View.PxSnapOffsetY);
                }
            }
            if (lineconnector.ConnectorType == ConnectorType.Bezier)
            {
                Rect sourceRect = new Rect();
                Rect rectTarget = new Rect();
                Point startPoint = new Point(0, 0);
                Point endPoint = new Point(0, 0);
                double x1 = 0;
                double y1 = 0;
                double x2 = 0;
                double y2 = 0;
                bool isLeft = false;
                bool isRight = false;
                bool isTop = false;
                bool isBottom = false;
                bool tisLeft = false;
                bool tisRight = false;
                bool tisTop = false;
                bool tisBottom = false;
                NodeInfo source = new NodeInfo();
                Point p = new Point(0, 0);
                PathGeometry pathgeometry = new PathGeometry();
                LineConnector lc = lineconnector as LineConnector;
                if (fixedNodeConnection != null)
                {
                    fixedNodeConnection.refreshBoundaries();
                    sourceRect = lc.getNodeRect(fixedNodeConnection);
                    source = lc.getRectAsNodeInfo(sourceRect);
                    if (sourceRect == Rect.Empty)
                    {
                        source = fixedNodeConnection.GetInfo();
                        sourceRect = new Rect(
                                              source.Left,
                                              source.Top,
                                              source.Size.Width,
                                              source.Size.Height);
                    }
                    if (sourceRect != Rect.Empty)
                    {
                        sourceRect = new Rect(source.Left, source.Top, source.Size.Width, source.Size.Height);
                    }
                    p = position;// MeasureUnitsConverter.FromPixels(position, source.MeasurementUnit);
                    if (HitTesting(position))
                    {
                        if (HitNodeConnector != null)
                        {
                            Rect r = lc.getNodeRect(HitNodeConnector);
                            NodeInfo target;
                            target = lc.getRectAsNodeInfo(r);
                            if (r != Rect.Empty)
                            {
                                r = new Rect(target.Left, target.Top, target.Size.Width, target.Size.Height);
                            }
                            double hitwidth = HitNodeConnector.Width;// MeasureUnitsConverter.FromPixels(HitNodeConnector.Width, target.MeasurementUnit);
                            double hitheight = HitNodeConnector.Height;// MeasureUnitsConverter.FromPixels(HitNodeConnector.Height, target.MeasurementUnit);
                            rectTarget = r;
                            if (IsHeadThumb)
                            {
                                ConnectorBase.GetOrthogonalLineIntersect(target, source, rectTarget, sourceRect, out isTop, out isBottom, out isLeft, out isRight, out tisTop, out tisBottom, out tisLeft, out tisRight, out startPoint, out endPoint);
                            }
                            else
                            {
                                ConnectorBase.GetOrthogonalLineIntersect(source, target, sourceRect, rectTarget, out isTop, out isBottom, out isLeft, out isRight, out tisTop, out tisBottom, out tisLeft, out tisRight, out startPoint, out endPoint);
                            }

                            if (previoushitport != null)
                            {
                                if (isheadnode)
                                {
                                    if ((lineconnector as LineConnector).TailNode != null)
                                    {
                                        NodeInfo fixednode = ((lineconnector as LineConnector).TailNode as Node).GetInfo();
                                        Rect temp = lc.getNodeRect(lc.TailNode as Node);
                                        fixednode = lc.getRectAsNodeInfo(temp);

                                        if (temp != Rect.Empty)
                                        {
                                            temp = new Rect(fixednode.Left, fixednode.Top, fixednode.Size.Width, fixednode.Size.Height);
                                        }

                                        double hitwidth1 = ((lineconnector as LineConnector).TailNode as Node).Width;// MeasureUnitsConverter.FromPixels(((lineconnector as LineConnector).TailNode as Node).Width, fixednode.MeasurementUnit);
                                        double hitheight1 = ((lineconnector as LineConnector).TailNode as Node).Height;// MeasureUnitsConverter.FromPixels(((lineconnector as LineConnector).TailNode as Node).Height, fixednode.MeasurementUnit);
                                        Rect rectFixed = temp;
                                        ConnectorBase.GetOrthogonalLineIntersect(target, fixednode, rectTarget, rectFixed, out isTop, out isBottom, out isLeft, out isRight, out tisTop, out tisBottom, out tisLeft, out tisRight, out startPoint, out endPoint);
                                    }
                                    else
                                    {
                                        endPoint = (lineconnector as LineConnector).PxEndPointPosition;
                                    }

                                    startPoint = new Point(previoushitport.Left, previoushitport.Top);
                                    startPoint = previoushitport.Node.TranslatePoint(startPoint, dc.View.Page);
                                    //startPoint = MeasureUnitsConverter.FromPixels(startPoint, (lineconnector as LineConnector).MeasurementUnit);
                                }
                                else
                                {
                                    if ((lineconnector as LineConnector).HeadNode != null)
                                    {
                                        NodeInfo headfixed = ((lineconnector as LineConnector).HeadNode as Node).GetInfo();
                                        Rect temp = lc.getNodeRect(lc.HeadNode as Node);
                                        headfixed = lc.getRectAsNodeInfo(temp);

                                        if (temp != Rect.Empty)
                                        {
                                            temp = new Rect(headfixed.Left, headfixed.Top, headfixed.Size.Width, headfixed.Size.Height);
                                        }

                                        double hitwidth2 = ((lineconnector as LineConnector).HeadNode as Node).Width;// MeasureUnitsConverter.FromPixels(((lineconnector as LineConnector).HeadNode as Node).Width, headfixed.MeasurementUnit);
                                        double hitheight2 = ((lineconnector as LineConnector).HeadNode as Node).Height;// MeasureUnitsConverter.FromPixels(((lineconnector as LineConnector).HeadNode as Node).Height, headfixed.MeasurementUnit);
                                        Rect rectheadfixed = temp;
                                        ConnectorBase.GetOrthogonalLineIntersect(headfixed, target, rectheadfixed, rectTarget, out isTop, out isBottom, out isLeft, out isRight, out tisTop, out tisBottom, out tisLeft, out tisRight, out startPoint, out endPoint);
                                    }
                                    else
                                    {
                                        startPoint = (lineconnector as LineConnector).PxStartPointPosition;
                                    }

                                    endPoint = new Point(previoushitport.Left, previoushitport.Top);
                                    endPoint = previoushitport.Node.TranslatePoint(endPoint, dc.View.Page);
                                    //endPoint = MeasureUnitsConverter.FromPixels(endPoint, (lineconnector as LineConnector).MeasurementUnit);
                                }
                            }
                            else
                            {
                                if (isheadnode)
                                {
                                    if ((lineconnector as LineConnector).TailNode != null)
                                    {
                                        NodeInfo fixednode = ((lineconnector as LineConnector).TailNode as Node).GetInfo();
                                        Rect temp = lc.getNodeRect(lc.TailNode as Node);
                                        fixednode = lc.getRectAsNodeInfo(temp);

                                        if (temp != Rect.Empty)
                                        {
                                            temp = new Rect(fixednode.Left, fixednode.Top, fixednode.Size.Width, fixednode.Size.Height);
                                        }

                                        double hitwidth1 = ((lineconnector as LineConnector).TailNode as Node).Width;//MeasureUnitsConverter.FromPixels(((lineconnector as LineConnector).TailNode as Node).Width, fixednode.MeasurementUnit);
                                        double hitheight1 = ((lineconnector as LineConnector).TailNode as Node).Height;//MeasureUnitsConverter.FromPixels(((lineconnector as LineConnector).TailNode as Node).Height, fixednode.MeasurementUnit);
                                        Rect rectFixed = temp;
                                        ConnectorBase.GetOrthogonalLineIntersect(target, fixednode, rectTarget, rectFixed, out isTop, out isBottom, out isLeft, out isRight, out tisTop, out tisBottom, out tisLeft, out tisRight, out startPoint, out endPoint);
                                    }
                                    else
                                    {
                                        endPoint = (lineconnector as LineConnector).PxEndPointPosition;
                                    }
                                }
                                else
                                {
                                    if ((lineconnector as LineConnector).HeadNode != null)
                                    {
                                        NodeInfo headfixed = ((lineconnector as LineConnector).HeadNode as Node).GetInfo();
                                        Rect temp = lc.getNodeRect(lc.HeadNode as Node);
                                        headfixed = lc.getRectAsNodeInfo(temp);

                                        if (temp != Rect.Empty)
                                        {
                                            temp = new Rect(headfixed.Left, headfixed.Top, headfixed.Size.Width, headfixed.Size.Height);
                                        }

                                        double hitwidth2 = ((lineconnector as LineConnector).HeadNode as Node).Width;// MeasureUnitsConverter.FromPixels(((lineconnector as LineConnector).HeadNode as Node).Width, headfixed.MeasurementUnit);
                                        double hitheight2 = ((lineconnector as LineConnector).HeadNode as Node).Height;// MeasureUnitsConverter.FromPixels(((lineconnector as LineConnector).HeadNode as Node).Height, headfixed.MeasurementUnit);
                                        Rect rectheadfixed = temp;
                                        ConnectorBase.GetOrthogonalLineIntersect(headfixed, target, rectheadfixed, rectTarget, out isTop, out isBottom, out isLeft, out isRight, out tisTop, out tisBottom, out tisLeft, out tisRight, out startPoint, out endPoint);
                                    }
                                    else
                                    {
                                        startPoint = (lineconnector as LineConnector).PxStartPointPosition;
                                    }
                                }
                            }

                            if (!IsHeadThumb)
                            {
                                if ((lineconnector as LineConnector).ConnectionHeadPort != null)
                                {
                                    startPoint = new Point((lineconnector as LineConnector).ConnectionHeadPort.Left, (lineconnector as LineConnector).ConnectionHeadPort.Top);
                                    startPoint = ((lineconnector as LineConnector).HeadNode as Node).TranslatePoint(startPoint, dc.View.Page);
                                    //startPoint = MeasureUnitsConverter.FromPixels(startPoint, (lineconnector as LineConnector).MeasurementUnit);
                                }
                            }
                            else
                            {
                                if ((lineconnector as LineConnector).ConnectionTailPort != null)
                                {
                                    endPoint = new Point((lineconnector as LineConnector).ConnectionTailPort.Left, (lineconnector as LineConnector).ConnectionTailPort.Top);
                                    endPoint = ((lineconnector as LineConnector).TailNode as Node).TranslatePoint(endPoint, dc.View.Page);
                                    //endPoint = MeasureUnitsConverter.FromPixels(endPoint, (lineconnector as LineConnector).MeasurementUnit);
                                }
                            }

                            x1 = startPoint.X;
                            y1 = startPoint.Y;
                            x2 = endPoint.X;
                            y2 = endPoint.Y;
                        }
                        else
                        {
                            if (!IsHeadThumb)
                            {
                                x1 = fixedNodeConnection.PxPosition.X;
                                y1 = fixedNodeConnection.PxPosition.Y;
                                x2 = pos.X;// MeasureUnitsConverter.FromPixels(pos, (this.lineconnector as LineConnector).MeasurementUnit).X;
                                y2 = pos.Y;// MeasureUnitsConverter.FromPixels(pos, (this.lineconnector as LineConnector).MeasurementUnit).Y;
                            }
                            else
                            {
                                x2 = fixedNodeConnection.PxPosition.X;
                                y2 = fixedNodeConnection.PxPosition.Y;
                                x1 = pos.X;// MeasureUnitsConverter.FromPixels(pos, (this.lineconnector as LineConnector).MeasurementUnit).X;
                                y2 = pos.Y;// MeasureUnitsConverter.FromPixels(pos, (this.lineconnector as LineConnector).MeasurementUnit).Y;
                            }
                        }
                    }
                    else
                    {
                        double twozero = 20;// MeasureUnitsConverter.FromPixels(20, DiagramPage.Munits);
                        double twofive = 25;// MeasureUnitsConverter.FromPixels(25, DiagramPage.Munits);
                        Point MousePt = pos;// MeasureUnitsConverter.FromPixels(pos, (this.lineconnector as LineConnector).MeasurementUnit);
                        double targetleft = MousePt.X;// MeasureUnitsConverter.FromPixels(MousePt.X, DiagramPage.Munits);
                        double targettop = MousePt.Y;// MeasureUnitsConverter.FromPixels(MousePt.Y, DiagramPage.Munits);
                        NodeInfo pointend = new NodeInfo();
                        Rect pointendRect = new Rect();
                        if (!IsHeadThumb)
                        {
                            lineconnector.ConnectionTailPort = null;
                            pointendRect = new Rect(
                                                  targetleft,
                                                  targettop,
                                                  twozero,
                                                  twozero);
                            pointend.Position = new Point(MousePt.X + twofive, MousePt.Y + twofive);

                            if ((lineconnector as LineConnector).HeadNode != null)
                            {
                                NodeInfo src;
                                Rect rectsrc;
                                rectsrc = lc.getNodeRect(lc.HeadNode as Node);
                                src = lc.getRectAsNodeInfo(rectsrc);
                                if (rectsrc != Rect.Empty)
                                {
                                    rectsrc = new Rect(src.Left, src.Top, src.Size.Width, src.Size.Height);
                                }
                                if (rectsrc == Rect.Empty)
                                {
                                    src = ((lineconnector as LineConnector).HeadNode as Node).GetInfo();

                                    rectsrc = new Rect(
                                        src.Left,
                                        src.Top,
                                        src.Size.Width,
                                        src.Size.Height);
                                }
                                Point sp1 = new Point(src.Position.X, src.Position.Y);

                                if (!IsHeadThumb)
                                {
                                    ConnectorBase.GetOrthogonalLineIntersect(src, pointend, rectsrc, pointendRect, out isTop, out isBottom, out isLeft, out isRight, out tisTop, out tisBottom, out tisLeft, out tisRight, out startPoint, out endPoint);
                                }
                                else
                                {
                                    ConnectorBase.GetOrthogonalLineIntersect(pointend, src, pointendRect, rectsrc, out isTop, out isBottom, out isLeft, out isRight, out tisTop, out tisBottom, out tisLeft, out tisRight, out startPoint, out endPoint);
                                }
                            }
                            else
                            {
                                startPoint = (lineconnector as LineConnector).PxStartPointPosition;
                            }

                            endPoint = pos;// MeasureUnitsConverter.FromPixels(pos, (this.lineconnector as LineConnector).MeasurementUnit);
                            if ((lineconnector as LineConnector).ConnectionHeadPort != null)
                            {
                                startPoint = new Point((lineconnector as LineConnector).ConnectionHeadPort.Left, (lineconnector as LineConnector).ConnectionHeadPort.Top);
                                startPoint = ((lineconnector as LineConnector).HeadNode as Node).TranslatePoint(startPoint, dc.View.Page);
                                //startPoint = MeasureUnitsConverter.FromPixels(startPoint, (lineconnector as LineConnector).MeasurementUnit);
                            }
                        }
                        else
                        {
                            lineconnector.ConnectionHeadPort = null;
                            pointendRect = new Rect(
                                                    targetleft,
                                                     targettop,
                                                     twozero,
                                                     twozero);
                            Point MousePt2 = pos;// MeasureUnitsConverter.FromPixels(pos, (this.lineconnector as LineConnector).MeasurementUnit);
                            pointend.Position = new Point(MousePt2.X, MousePt2.Y);
                            if ((lineconnector as LineConnector).TailNode != null)
                            {
                                NodeInfo target;
                                Rect recttarget;
                                recttarget = lc.getNodeRect(lc.TailNode as Node);
                                target = lc.getRectAsNodeInfo(recttarget);
                                if (recttarget != Rect.Empty)
                                {
                                    recttarget = new Rect(target.Left, target.Top, target.Size.Width, target.Size.Height);
                                }
                                if (recttarget == Rect.Empty)
                                {
                                    target = ((lineconnector as LineConnector).TailNode as Node).GetInfo();
                                    recttarget = new Rect(
                                                                target.Left,
                                                                target.Top,
                                                                target.Size.Width,
                                                                target.Size.Height);
                                }
                                Point ep1 = new Point(target.Position.X, target.Position.Y);
                                if (!IsHeadThumb)
                                {
                                    ConnectorBase.GetOrthogonalLineIntersect(target, pointend, recttarget, pointendRect, out isTop, out isBottom, out isLeft, out isRight, out tisTop, out tisBottom, out tisLeft, out tisRight, out startPoint, out endPoint);
                                }
                                else
                                {
                                    ConnectorBase.GetOrthogonalLineIntersect(pointend, target, pointendRect, recttarget, out isTop, out isBottom, out isLeft, out isRight, out tisTop, out tisBottom, out tisLeft, out tisRight, out startPoint, out endPoint);
                                }
                            }
                            else
                            {
                                endPoint = (lineconnector as LineConnector).PxEndPointPosition;
                            }

                            startPoint = pos;// MeasureUnitsConverter.FromPixels(pos, (this.lineconnector as LineConnector).MeasurementUnit);

                            if ((lineconnector as LineConnector).ConnectionTailPort != null)
                            {
                                endPoint = new Point((lineconnector as LineConnector).ConnectionTailPort.Left, (lineconnector as LineConnector).ConnectionTailPort.Top);
                                endPoint = ((lineconnector as LineConnector).TailNode as Node).TranslatePoint(endPoint, dc.View.Page);
                                //endPoint = MeasureUnitsConverter.FromPixels(endPoint, (lineconnector as LineConnector).MeasurementUnit);
                            }
                        }
                    }

                    x1 = startPoint.X;
                    y1 = startPoint.Y;
                    x2 = endPoint.X;
                    y2 = endPoint.Y;
                }
                else
                {
                    if (!IsHeadThumb)
                    {
                        x1 = (lineconnector as LineConnector).PxStartPointPosition.X;
                        y1 = (lineconnector as LineConnector).PxStartPointPosition.Y;
                        x2 = pos.X;// MeasureUnitsConverter.FromPixels(pos, (this.lineconnector as LineConnector).MeasurementUnit).X;
                        y2 = pos.Y;// MeasureUnitsConverter.FromPixels(pos, (this.lineconnector as LineConnector).MeasurementUnit).Y;
                    }
                    else
                    {
                        x2 = (lineconnector as LineConnector).PxEndPointPosition.X;
                        y2 = (lineconnector as LineConnector).PxEndPointPosition.Y;
                        x1 = pos.X;// MeasureUnitsConverter.FromPixels(pos, (this.lineconnector as LineConnector).MeasurementUnit).X;
                        y1 = pos.Y;// MeasureUnitsConverter.FromPixels(pos, (this.lineconnector as LineConnector).MeasurementUnit).Y;
                    }
                }

                PathFigure pathfigure = new PathFigure();
                //double two = 2.0;// MeasureUnitsConverter.FromPixels(2.0, DiagramPage.Munits);
                double twenty = 20;// MeasureUnitsConverter.FromPixels(20, DiagramPage.Munits);
                double num = Math.Max((double)(Math.Abs((double)(x2 - x1)) / 2.0), twenty);
                pathfigure.StartPoint = new Point(x1, y1);// MeasureUnitsConverter.ToPixels(new Point(x1, y1), (lineconnector as LineConnector).MeasurementUnit);

                BezierSegment segment = new BezierSegment();
                if (isBottom)
                {
                    segment = GetSegment(x1, y1 + num, x2, y2, x2, y2, num, tisTop, tisBottom, tisLeft, tisRight);
                }
                else if (isTop)
                {
                    segment = GetSegment(x1, y1 - num, x2, y2, x2, y2, num, tisTop, tisBottom, tisLeft, tisRight);
                }
                else if (isRight)
                {
                    segment = GetSegment(x1 + num, y1, x2, y2, x2, y2, num, tisTop, tisBottom, tisLeft, tisRight);
                }
                else
                {
                    segment = GetSegment(x1 - num, y1, x2, y2, x2, y2, num, tisTop, tisBottom, tisLeft, tisRight);
                }
                //segment.Point1 = MeasureUnitsConverter.ToPixels(segment.Point1, (lineconnector as LineConnector).MeasurementUnit);
                //segment.Point2 = MeasureUnitsConverter.ToPixels(segment.Point2, (lineconnector as LineConnector).MeasurementUnit);
                //segment.Point3 = MeasureUnitsConverter.ToPixels(segment.Point3, (lineconnector as LineConnector).MeasurementUnit);
                pathfigure.Segments.Add(segment);
                pathgeometry.Figures.Add(pathfigure);

                return pathgeometry;
            }
            else
            {
                if (fixedNodeConnection != null && lineconnector.ConnectorType != ConnectorType.Orthogonal)
                {
                    PathGeometry pathgeometry = new PathGeometry();
                    List<Point> connectionPoints = GetAdornerLinePoints(fixedNodeConnection.GetInfo(), position);

                    for (int i = 0; i < connectionPoints.Count; i++)
                    {
                        //connectionPoints[i] = MeasureUnitsConverter.ToPixels(connectionPoints[i], (lineconnector as LineConnector).MeasurementUnit);
                    }
                    if (connectionPoints.Count > 0)
                    {
                        PathFigure pathfigure = new PathFigure();
                        pathfigure.StartPoint = connectionPoints[0];
                        connectionPoints.Remove(connectionPoints[0]);
                        pathfigure.Segments.Add(new PolyLineSegment(connectionPoints, true));
                        pathgeometry.Figures.Add(pathfigure);
                    }

                    return pathgeometry;
                }
                else
                {
                    PathGeometry pathgeometry = new PathGeometry();
                    List<Point> connectionPoints = new List<Point>();
                    if (IsHeadThumb || IsTailThumb)
                    {
                        try
                        {
                            if (lineconnector.HeadNode != null)
                            {
                                bool t = IsTailThumb;
                                IsTailThumb = true;
                                connectionPoints = GetAdornerLinePoints((lineconnector.HeadNode as Node).GetInfo(), position);
                                IsTailThumb = t;
                            }
                            else if (lineconnector.TailNode != null)
                            {
                                bool t = IsHeadThumb;
                                IsHeadThumb = true;
                                connectionPoints = GetAdornerLinePoints((lineconnector.TailNode as Node).GetInfo(), position);
                                IsHeadThumb = t;
                            }
                            if (lineconnector.HeadNode != null || lineconnector.TailNode != null)
                            {
                                tempst = connectionPoints[0];
                                tempend = connectionPoints[1];
                            }
                        }
                        catch { }
                    }
                    if ((lineconnector as LineConnector).ConnectorType == ConnectorType.Orthogonal && (lineconnector as LineConnector).IntermediatePoints.Count < 2)
                    {
                        //connectionPoints = GetAdornerLinePoints();

                        //settempendPoints();
                        //settempstPoints();
                        connectionPoints.Clear();
                        if (IsHeadThumb)
                        {
                            //connectionPoints.Add(MeasureUnitsConverter.FromPixels(pos, (this.lineconnector as LineConnector).MeasurementUnit));
                            //if ((this.lineconnector as LineConnector).TailNode == null)
                            //{
                            //    connectionPoints.Add(new Point(connectionPoints[0].X, (lineconnector as LineConnector).EndPointPosition.Y));
                            //}
                            //else
                            //{
                            //    connectionPoints.Add(tempend);
                            //}
                            //connectionPoints.Add(new Point((lineconnector as LineConnector).EndPointPosition.X, (lineconnector as LineConnector).EndPointPosition.Y));
                            if ((this.lineconnector as LineConnector).TailNode == null)
                            {
                                connectionPoints.Add(new Point((lineconnector as LineConnector).PxEndPointPosition.X, (lineconnector as LineConnector).PxEndPointPosition.Y));
                            }
                            else
                            {
                                connectionPoints.Add(tempend);
                            }
                            connectionPoints.Add(pos);//MeasureUnitsConverter.FromPixels(pos, (this.lineconnector as LineConnector).MeasurementUnit));
                            connectionPoints.Insert(1, (new Point(connectionPoints[1].X, connectionPoints[0].Y)));
                            //connectionPoints.Add((this.lineconnector as LineConnector).getOrthognalLine());
                        }
                        else if (IsTailThumb)
                        {
                            if ((this.lineconnector as LineConnector).HeadNode == null)
                            {
                                connectionPoints.Add(new Point((lineconnector as LineConnector).PxStartPointPosition.X, (lineconnector as LineConnector).PxStartPointPosition.Y));
                            }
                            else
                            {
                                connectionPoints.Add(tempst);
                            }
                            //connectionPoints.Add((this.lineconnector as LineConnector).getOrthognalLine());
                            connectionPoints.Add(pos);//MeasureUnitsConverter.FromPixels(pos, (this.lineconnector as LineConnector).MeasurementUnit));
                            connectionPoints.Insert(1, (new Point(connectionPoints[0].X, connectionPoints[1].Y)));
                        }
                    }
                    else
                    {
                        connectionPoints = GetAdornerLinePoints();
                    }
                    settempstPoints();
                    settempendPoints();
                    for (int i = 0; i < connectionPoints.Count; i++)
                    {
                        //connectionPoints[i] = MeasureUnitsConverter.ToPixels(connectionPoints[i], (lineconnector as LineConnector).MeasurementUnit);
                    }
                    if (connectionPoints.Count > 0)
                    {
                        PathFigure pathfigure = new PathFigure();
                        pathfigure.StartPoint = connectionPoints[0];
                        connectionPoints.Remove(connectionPoints[0]);
                        pathfigure.Segments.Add(new PolyLineSegment(connectionPoints, true));
                        pathgeometry.Figures.Add(pathfigure);
                    }

                    return pathgeometry;
                }
            }
        }

        /// <summary>
        /// Returns the Bezier segment.
        /// </summary>
        /// <param name="x1">The x coordinate of the starting point(first control point) of the curve.</param>
        /// <param name="y1">The y coordinate of the starting point(first control point)of the curve.</param>
        /// <param name="x2">The x coordinate of the end point of the curve.</param>
        /// <param name="y2">The y coordinate of the end point of the curve.</param>
        /// <param name="temp1">The x coordinate of the second control point of the curve.</param>
        /// <param name="temp2">The y coordinate of the second control point of the curve.</param>
        /// <param name="num1">It specifies the amount of curve to be provided.Value is 150d.</param>
        /// <param name="isTop">Flag indicating the top side.</param>
        /// <param name="isBottom">Flag indicating the bottom side.</param>
        /// <param name="isLeft">Flag indicating the left side.</param>
        /// <param name="isRight">Flag indicating the right side.</param>
        /// <returns>The Bezier segment.</returns>
        private BezierSegment GetSegment(double x1, double y1, double x2, double y2, double temp1, double temp2, double num1, bool isTop, bool isBottom, bool isLeft, bool isRight)
        {
            if (isTop)
            {
                return Segment(x1, y1, x2, y2, temp1, temp2 - num1);
            }
            else if (isBottom)
            {
                return Segment(x1, y1, x2, y2, temp1, temp2 + num1);
            }
            else if (isLeft)
            {
                return Segment(x1, y1, x2, y2, temp1 - num1, temp2);
            }
            else
            {
                return Segment(x1, y1, x2, y2, temp1 + num1, temp2);
            }
        }

        /// <summary>
        /// Returns the segment.
        /// </summary>
        /// <param name="x1">The x coordinate of the starting point(first control point) of the curve.</param>
        /// <param name="y1">The y coordinate of the starting point(first control point) of the curve.</param>
        /// <param name="x2">The x coordinate of the end point of the curve.</param>
        /// <param name="y2">The y coordinate of the end point of the curve.</param>
        /// <param name="temp1">The x coordinate of the second control point of the curve.</param>
        /// <param name="temp2">The y coordinate of the second control point of the curve.</param>
        /// <returns>The Bezier segment</returns>
        private BezierSegment Segment(double x1, double y1, double x2, double y2, double temp1, double temp2)
        {
            BezierSegment segment = new BezierSegment
            {
                Point1 = new Point(x1, y1),
                Point2 = new Point(temp1, temp2),
                Point3 = new Point(x2, y2)
            };
            return segment;
        }

        internal void settempstPoints()
        {
            Point endPoint = Mouse.GetPosition(this);
            if (this.dc != null && this.dc.View != null)
            {
                if (dc.View.SnapToVerticalGrid)
                {
                    endPoint.X = Node.Round(endPoint.X, dc.View.PxSnapOffsetX);
                }
                if (dc.View.SnapToHorizontalGrid)
                {
                    endPoint.Y = Node.Round(endPoint.Y, dc.View.PxSnapOffsetY);
                }
            }
            //endPoint = MeasureUnitsConverter.FromPixels(endPoint, (this.lineconnector as LineConnector).MeasurementUnit);
            bool b1, b2, b3, b4;
            if ((lineconnector as LineConnector).HeadNode != null)
            {
                NodeInfo src = ((lineconnector as LineConnector).HeadNode as Node).GetInfo();
                Rect rectsrc = new Rect(
                    src.Left,
                    src.Top,
                    src.Size.Width,
                    src.Size.Height);
                Point sp1 = new Point(src.Position.X, src.Position.Y);
                tempst = ConnectorBase.GetLineIntersect(src, sp1, endPoint, rectsrc, out b1, out b2, out b3, out b4, lineconnector.ConnectorType);
                if (dc != null && dc.View != null && dc.View.Page != null)
                {
                    if ((lineconnector as LineConnector).ConnectionHeadPort != null)
                    {
                        tempst = new Point((lineconnector as LineConnector).ConnectionHeadPort.Left, (lineconnector as LineConnector).ConnectionHeadPort.Top);
                        tempst = ((lineconnector as LineConnector).HeadNode as Node).TranslatePoint(tempst, dc.View.Page);
                        //tempst = MeasureUnitsConverter.FromPixels(tempst, (lineconnector as LineConnector).MeasurementUnit);
                    }
                }
            }
        }
        internal void settempendPoints()
        {
            Point s = Mouse.GetPosition(this);
            if (this.dc != null && this.dc.View != null)
            {
                if (dc.View.SnapToVerticalGrid)
                {
                    s.X = Node.Round(s.X, dc.View.PxSnapOffsetX);
                }
                if (dc.View.SnapToHorizontalGrid)
                {
                    s.Y = Node.Round(s.Y, dc.View.PxSnapOffsetY);
                }
            }
            //s = MeasureUnitsConverter.FromPixels(s, (this.lineconnector as LineConnector).MeasurementUnit);
            bool b1, b2, b3, b4;
            if ((lineconnector as LineConnector).TailNode != null)
            {
                NodeInfo target = ((lineconnector as LineConnector).TailNode as Node).GetInfo();
                Rect recttarget = new Rect(
                    target.Left,
                    target.Top,
                    target.Size.Width,
                    target.Size.Height);
                Point ep1 = new Point(target.Position.X, target.Position.Y);
                tempend = ConnectorBase.GetLineIntersect(target, ep1, s, recttarget, out b1, out b2, out b3, out b4, lineconnector.ConnectorType);
                if (dc != null && dc.View != null && dc.View.Page != null)
                {
                    if ((lineconnector as LineConnector).ConnectionTailPort != null)
                    {
                        tempend = new Point((lineconnector as LineConnector).ConnectionTailPort.Left, (lineconnector as LineConnector).ConnectionTailPort.Top);
                        tempend = ((lineconnector as LineConnector).TailNode as Node).TranslatePoint(tempend, dc.View.Page);
                        //tempend = MeasureUnitsConverter.FromPixels(tempend, (lineconnector as LineConnector).MeasurementUnit);
                    }
                }
            }
        }
        /// <summary>
        /// Calculates the points which form the path geometry. 
        /// </summary>
        /// <param name="source">The head node</param>
        /// <param name="sinkPoint">The endpoint of the connector.</param>
        /// <returns>Collection of points.</returns>
        internal List<Point> GetAdornerLinePoints(NodeInfo source, Point sinkPoint)
        {
            Point pos = Mouse.GetPosition(this);
            if (this.dc != null && this.dc.View != null)
            {
                if (dc.View.SnapToVerticalGrid)
                {
                    pos.X = Node.Round(pos.X, dc.View.PxSnapOffsetX);
                }
                if (dc.View.SnapToHorizontalGrid)
                {
                    pos.Y = Node.Round(pos.Y, dc.View.PxSnapOffsetY);
                }
            }
            bool b1 = false;
            bool b2 = false;
            bool b3 = false;
            bool b4 = false;
            bool b5 = false;
            bool b6 = false;
            bool b7 = false;
            bool b8 = false;
            Point p = new Point(0, 0);
            List<Point> linePoints = new List<Point>();
            Rect rectSource = new Rect(
                                 source.Left,
                                 source.Top,
                                 source.Size.Width,
                                 source.Size.Height);
            Point st = new Point(source.Position.X, source.Position.Y);
            Point endPoint = sinkPoint;
            try
            {
                Point ep = endPoint;// MeasureUnitsConverter.FromPixels(endPoint, source.MeasurementUnit);
                if (!rectSource.Contains(ep))
                {
                    if (lineconnector.ConnectorType == ConnectorType.Orthogonal)
                    {
                        Point startPoint = new Point(0, 0);

                        if (HitTesting(sinkPoint))
                        {
                            if (HitNodeConnector != null)
                            {
                                NodeInfo target = HitNodeConnector.GetInfo();
                                double hitwidth = HitNodeConnector.Width;// MeasureUnitsConverter.FromPixels(HitNodeConnector.Width, target.MeasurementUnit);
                                double hitheight = HitNodeConnector.Height;// MeasureUnitsConverter.FromPixels(HitNodeConnector.Height, target.MeasurementUnit);
                                Rect rectTarget = new Rect(HitNodeConnector.PxOffsetX, HitNodeConnector.PxOffsetY, hitwidth, hitheight);
                                if (IsHeadThumb)
                                {
                                    ConnectorBase.GetOrthogonalLineIntersect(target, source, rectTarget, rectSource, out b1, out b2, out b3, out b4, out b5, out b6, out b7, out b8, out startPoint, out endPoint);
                                }
                                else if (IsTailThumb)
                                {
                                    ConnectorBase.GetOrthogonalLineIntersect(source, target, rectSource, rectTarget, out b1, out b2, out b3, out b4, out b5, out b6, out b7, out b8, out startPoint, out endPoint);
                                }

                                if (previoushitport != null)
                                {
                                    if (isheadnode)
                                    {
                                        if ((lineconnector as LineConnector).TailNode != null)
                                        {
                                            NodeInfo fixednode = ((lineconnector as LineConnector).TailNode as Node).GetInfo();
                                            double hitwidth1 = ((lineconnector as LineConnector).TailNode as Node).Width;// MeasureUnitsConverter.FromPixels(((lineconnector as LineConnector).TailNode as Node).Width, fixednode.MeasurementUnit);
                                            double hitheight1 = ((lineconnector as LineConnector).TailNode as Node).Height;// MeasureUnitsConverter.FromPixels(((lineconnector as LineConnector).TailNode as Node).Height, fixednode.MeasurementUnit);
                                            Rect rectFixed = new Rect(((lineconnector as LineConnector).TailNode as Node).PxOffsetX, ((lineconnector as LineConnector).TailNode as Node).PxOffsetY, hitwidth1, hitheight1);
                                            ConnectorBase.GetOrthogonalLineIntersect(target, fixednode, rectTarget, rectFixed, out b1, out b2, out b3, out b4, out b5, out b6, out b7, out b8, out startPoint, out endPoint);
                                        }
                                        else
                                        {
                                            endPoint = (lineconnector as LineConnector).PxEndPointPosition;
                                        }

                                        startPoint = new Point(previoushitport.Left, previoushitport.Top);
                                        startPoint = previoushitport.Node.TranslatePoint(startPoint, dc.View.Page);
                                    }
                                    else
                                    {
                                        if ((lineconnector as LineConnector).HeadNode != null)
                                        {
                                            NodeInfo headfixed = ((lineconnector as LineConnector).HeadNode as Node).GetInfo();
                                            double hitwidth2 = ((lineconnector as LineConnector).HeadNode as Node).Width;// MeasureUnitsConverter.FromPixels(((lineconnector as LineConnector).HeadNode as Node).Width, headfixed.MeasurementUnit);
                                            double hitheight2 = ((lineconnector as LineConnector).HeadNode as Node).Height;// MeasureUnitsConverter.FromPixels(((lineconnector as LineConnector).HeadNode as Node).Height, headfixed.MeasurementUnit);
                                            Rect rectheadfixed = new Rect(((lineconnector as LineConnector).HeadNode as Node).PxOffsetX, ((lineconnector as LineConnector).HeadNode as Node).PxOffsetY, hitwidth2, hitheight2);
                                            ConnectorBase.GetOrthogonalLineIntersect(headfixed, target, rectheadfixed, rectTarget, out b1, out b2, out b3, out b4, out b5, out b6, out b7, out b8, out startPoint, out endPoint);
                                        }
                                        else
                                        {
                                            startPoint = (lineconnector as LineConnector).PxStartPointPosition;
                                        }

                                        endPoint = new Point(previoushitport.Left, previoushitport.Top);
                                        endPoint = previoushitport.Node.TranslatePoint(endPoint, dc.View.Page);
                                    }
                                }
                                if (IsTailThumb)
                                {
                                    if ((lineconnector as LineConnector).ConnectionHeadPort != null)
                                    {
                                        startPoint = new Point((lineconnector as LineConnector).ConnectionHeadPort.Left, (lineconnector as LineConnector).ConnectionHeadPort.Top);
                                        startPoint = ((lineconnector as LineConnector).HeadNode as Node).TranslatePoint(startPoint, dc.View.Page);
                                        //startPoint = MeasureUnitsConverter.FromPixels(startPoint, (lineconnector as LineConnector).MeasurementUnit);
                                    }
                                }
                                else if (IsHeadThumb)
                                {
                                    if ((lineconnector as LineConnector).ConnectionTailPort != null)
                                    {
                                        endPoint = new Point((lineconnector as LineConnector).ConnectionTailPort.Left, (lineconnector as LineConnector).ConnectionTailPort.Top);
                                        endPoint = ((lineconnector as LineConnector).TailNode as Node).TranslatePoint(endPoint, dc.View.Page);
                                        //endPoint = MeasureUnitsConverter.FromPixels(endPoint, (lineconnector as LineConnector).MeasurementUnit);
                                    }
                                }

                                linePoints.Add(startPoint);
                                linePoints.Add(endPoint);
                            }
                        }
                        else
                        {
                            Point s = new Point(0, 0);
                            if (IsTailThumb)
                            {
                                lineconnector.ConnectionTailPort = null;
                                endPoint = pos;// MeasureUnitsConverter.FromPixels(pos, (this.lineconnector as LineConnector).MeasurementUnit);
                                if ((lineconnector as LineConnector).HeadNode != null)
                                {
                                    NodeInfo src = ((lineconnector as LineConnector).HeadNode as Node).GetInfo();
                                    Rect rectsrc = new Rect(
                                        src.Left,
                                        src.Top,
                                        src.Size.Width,
                                        src.Size.Height);
                                    Point sp1 = new Point(src.Position.X, src.Position.Y);
                                    s = ConnectorBase.GetLineIntersect(src, sp1, endPoint, rectsrc, out b1, out b2, out b3, out b4, lineconnector.ConnectorType);
                                }
                                else
                                {
                                    s = (lineconnector as LineConnector).PxStartPointPosition;
                                    fixedNodeConnection = null;
                                }

                                if ((lineconnector as LineConnector).ConnectionHeadPort != null)
                                {
                                    s = new Point((lineconnector as LineConnector).ConnectionHeadPort.Left, (lineconnector as LineConnector).ConnectionHeadPort.Top);
                                    s = ((lineconnector as LineConnector).HeadNode as Node).TranslatePoint(s, dc.View.Page);
                                    //s = MeasureUnitsConverter.FromPixels(s, (lineconnector as LineConnector).MeasurementUnit);
                                }
                            }
                            else if (IsHeadThumb)
                            {
                                lineconnector.ConnectionHeadPort = null;
                                s = pos;// MeasureUnitsConverter.FromPixels(pos, (this.lineconnector as LineConnector).MeasurementUnit);
                                if ((lineconnector as LineConnector).TailNode != null)
                                {
                                    NodeInfo target = ((lineconnector as LineConnector).TailNode as Node).GetInfo();
                                    Rect recttarget = new Rect(
                                        target.Left,
                                        target.Top,
                                        target.Size.Width,
                                        target.Size.Height);
                                    Point ep1 = new Point(target.Position.X, target.Position.Y);
                                    endPoint = ConnectorBase.GetLineIntersect(target, ep1, s, recttarget, out b1, out b2, out b3, out b4, lineconnector.ConnectorType);
                                }
                                else
                                {
                                    endPoint = (lineconnector as LineConnector).PxEndPointPosition;
                                    fixedNodeConnection = null;
                                }

                                if ((lineconnector as LineConnector).ConnectionTailPort != null)
                                {
                                    endPoint = new Point((lineconnector as LineConnector).ConnectionTailPort.Left, (lineconnector as LineConnector).ConnectionTailPort.Top);
                                    endPoint = ((lineconnector as LineConnector).TailNode as Node).TranslatePoint(endPoint, dc.View.Page);
                                    //endPoint = MeasureUnitsConverter.FromPixels(endPoint, (lineconnector as LineConnector).MeasurementUnit);
                                }
                            }

                            linePoints.Add(s);
                            linePoints.Add(endPoint);
                        }
                    }
                    else
                    {
                        Point s = new Point();
                        if (IsTailThumb)
                        {
                            if (!HitTesting(sinkPoint))
                            {
                                lineconnector.ConnectionTailPort = null;
                            }

                            endPoint = pos;// MeasureUnitsConverter.FromPixels(pos, (this.lineconnector as LineConnector).MeasurementUnit);
                            if ((lineconnector as LineConnector).HeadNode != null)
                            {
                                NodeInfo src = ((lineconnector as LineConnector).HeadNode as Node).GetInfo();
                                Rect rectsrc = new Rect(
                                    src.Left,
                                    src.Top,
                                    src.Size.Width,
                                    src.Size.Height);
                                Point sp1 = new Point(src.Position.X, src.Position.Y);
                                s = ConnectorBase.GetLineIntersect(src, sp1, endPoint, rectsrc, out b1, out b2, out b3, out b4, lineconnector.ConnectorType);
                            }
                            else
                            {
                                s = (lineconnector as LineConnector).PxStartPointPosition;
                            }

                            if ((lineconnector as LineConnector).ConnectionHeadPort != null)
                            {
                                s = new Point((lineconnector as LineConnector).ConnectionHeadPort.Left, (lineconnector as LineConnector).ConnectionHeadPort.Top);
                                s = ((lineconnector as LineConnector).HeadNode as Node).TranslatePoint(s, dc.View.Page);
                                //s = MeasureUnitsConverter.FromPixels(s, (lineconnector as LineConnector).MeasurementUnit);
                            }
                        }
                        else if (IsHeadThumb)
                        {
                            if (!HitTesting(sinkPoint))
                            {
                                lineconnector.ConnectionHeadPort = null;
                            }

                            s = pos;// MeasureUnitsConverter.FromPixels(pos, (this.lineconnector as LineConnector).MeasurementUnit);
                            if ((lineconnector as LineConnector).TailNode != null)
                            {
                                NodeInfo target = ((lineconnector as LineConnector).TailNode as Node).GetInfo();
                                Rect recttarget = new Rect(
                                                            target.Left,
                                                            target.Top,
                                                            target.Size.Width,
                                                            target.Size.Height);
                                Point ep1 = new Point(target.Position.X, target.Position.Y);
                                endPoint = ConnectorBase.GetLineIntersect(target, ep1, s, recttarget, out b1, out b2, out b3, out b4, lineconnector.ConnectorType);
                            }
                            else
                            {
                                endPoint = (lineconnector as LineConnector).PxEndPointPosition;
                            }

                            if ((lineconnector as LineConnector).ConnectionTailPort != null)
                            {
                                endPoint = new Point((lineconnector as LineConnector).ConnectionTailPort.Left, (lineconnector as LineConnector).ConnectionTailPort.Top);
                                endPoint = ((lineconnector as LineConnector).TailNode as Node).TranslatePoint(endPoint, dc.View.Page);
                                //endPoint = MeasureUnitsConverter.FromPixels(endPoint, (lineconnector as LineConnector).MeasurementUnit);
                            }
                        }
                        if (IsHeadThumb)
                        {
                            linePoints.Add(pos);//MeasureUnitsConverter.FromPixels(pos, (this.lineconnector as LineConnector).MeasurementUnit));
                            if (InterPts.Count != 0)
                                endPoint = InterPts[0];
                            else
                            {

                            }
                        }
                        else if (IsTailThumb)
                        {
                            if (InterPts.Count != 0)
                                s = InterPts[InterPts.Count - 1];
                            else
                            {

                            }
                            linePoints.Add(s);
                            endPoint = pos;// MeasureUnitsConverter.FromPixels(pos, (this.lineconnector as LineConnector).MeasurementUnit);
                        }
                        if (s != p)
                        {
                            linePoints.Add(s);
                        }

                        linePoints.Add(endPoint);
                    }
                }
            }
            catch
            {
            }

            return linePoints;
        }

        /// <summary>
        /// Gets the line points when no head node and tail node are specified.
        /// </summary>
        /// <returns>The List of points</returns>
        internal List<Point> GetAdornerLinePoints()
        {
            Point pos = Mouse.GetPosition(this);
            if (this.dc != null && this.dc.View != null)
            {
                if (dc.View.SnapToVerticalGrid)
                {
                    pos.X = Node.Round(pos.X, dc.View.PxSnapOffsetX);
                }
                if (dc.View.SnapToHorizontalGrid)
                {
                    pos.Y = Node.Round(pos.Y, dc.View.PxSnapOffsetY);
                }
            }
            bool delayStackFirst = false;
            bool delayStackLast = false;
            Point startPoint = new Point(0, 0);
            Point endPoint = new Point(0, 0);
            List<Point> connectionPoints = new List<Point>();
            if (lineconnector.ConnectorType == ConnectorType.Straight)
            {
                if (IsHeadThumb)
                {
                    startPoint = pos;// MeasureUnitsConverter.FromPixels(pos, (this.lineconnector as LineConnector).MeasurementUnit);
                    if (InterPts.Count != 0)
                        endPoint = InterPts[0];
                    else
                        endPoint = new Point((lineconnector as LineConnector).PxEndPointPosition.X, (lineconnector as LineConnector).PxEndPointPosition.Y);
                }
                else if (IsTailThumb)
                {
                    if (InterPts.Count != 0)
                        startPoint = InterPts[InterPts.Count - 1];
                    else
                        startPoint = new Point((lineconnector as LineConnector).PxStartPointPosition.X, (lineconnector as LineConnector).PxStartPointPosition.Y);
                    endPoint = pos;// MeasureUnitsConverter.FromPixels(pos, (this.lineconnector as LineConnector).MeasurementUnit);
                }
                else
                {
                    double hLeft = Canvas.GetLeft(headThumb);// MeasureUnitsConverter.FromPixels(Canvas.GetLeft(headThumb), (this.lineconnector as LineConnector).MeasurementUnit);
                    double hTop = Canvas.GetTop(headThumb);// MeasureUnitsConverter.FromPixels(Canvas.GetTop(headThumb), (this.lineconnector as LineConnector).MeasurementUnit);

                    double tLeft = Canvas.GetLeft(tailThumb);// MeasureUnitsConverter.FromPixels(Canvas.GetLeft(tailThumb), (this.lineconnector as LineConnector).MeasurementUnit);
                    double tTop = Canvas.GetTop(tailThumb);// MeasureUnitsConverter.FromPixels(Canvas.GetTop(tailThumb), (this.lineconnector as LineConnector).MeasurementUnit);
                    if (vertexIndex == 0)
                    {
                        startPoint = new Point(hLeft, hTop);
                        if (InterPts.Count == 1)
                            endPoint = new Point(tLeft, tTop);
                        else
                            endPoint = InterPts[vertexIndex + 1];
                    }
                    else if (vertexIndex == InterPts.Count - 1)
                    {
                        startPoint = InterPts[vertexIndex - 1];
                        endPoint = new Point(tLeft, tTop);
                    }
                    else
                    {
                        startPoint = InterPts[vertexIndex - 1];
                        endPoint = InterPts[vertexIndex + 1];
                    }
                }
                connectionPoints.Add(startPoint);
                if (!IsHeadThumb && !IsTailThumb)
                {
                    connectionPoints.Add(pos);//MeasureUnitsConverter.FromPixels(pos, (this.lineconnector as LineConnector).MeasurementUnit));
                }
                connectionPoints.Add(endPoint);
            }
            else if (lineconnector.ConnectorType == ConnectorType.Orthogonal)
            {
                settempendPoints();
                settempstPoints();
                Point tempEnd = (lineconnector as LineConnector).PxEndPointPosition;
                if (tempEnd.Equals(new Point(0, 0)))
                    tempEnd = tempend;
                Point tempStart = (lineconnector as LineConnector).PxStartPointPosition;
                if (tempStart.Equals(new Point(0, 0)))
                    tempStart = tempst;
                if (IsHeadThumb)
                {
                    Point mousePt = pos;// MeasureUnitsConverter.FromPixels(pos, (this.lineconnector as LineConnector).MeasurementUnit);
                    connectionPoints.Add(mousePt);
                    if (Math.Abs(InterPts[1].X - InterPts[0].X)
                    < Math.Abs(InterPts[1].Y - InterPts[0].Y))
                    {
                        connectionPoints.Add(new Point(InterPts[0].X, mousePt.Y));
                    }
                    else
                    {
                        connectionPoints.Add(new Point(mousePt.X, InterPts[0].Y));
                    }
                    if (InterPts.Count == 1)
                    {
                        connectionPoints.Add(tempEnd);
                    }
                    else { connectionPoints.Add(InterPts[1]); }
                }
                else if (IsTailThumb)
                {
                    Point mousePt = pos;// MeasureUnitsConverter.FromPixels(pos, (this.lineconnector as LineConnector).MeasurementUnit);
                    connectionPoints.Add(mousePt);
                    if (Math.Abs(InterPts[InterPts.Count - 2].X - InterPts[InterPts.Count - 1].X)
                    < Math.Abs(InterPts[InterPts.Count - 2].Y - InterPts[InterPts.Count - 1].Y))
                    {
                        connectionPoints.Add(new Point(InterPts[InterPts.Count - 1].X, mousePt.Y));
                    }
                    else
                    {
                        connectionPoints.Add(new Point(mousePt.X, InterPts[InterPts.Count - 1].Y));
                    }
                    if (InterPts.Count == 1)
                    {
                        connectionPoints.Add(tempStart);
                    }
                    else { connectionPoints.Add(InterPts[InterPts.Count - 2]); }

                }
                else if (vertexIndex == 0)
                {
                    //if ((lineconnector as LineConnector).DynamicIntermediatePoints)
                    //{
                    //}
                    //else
                    {
                        Point tempPoint = new Point();
                        if (delayStack.Count > 0)
                        {
                            delayStackFirst = true;
                            if (Math.Abs(InterPts[1].X - InterPts[0].X)
                                                  < Math.Abs(InterPts[1].Y - InterPts[0].Y))
                            {
                                tempPoint = new Point(Math.Min(InterPts[0].X, tempStart.X) + Math.Abs(InterPts[0].X - tempStart.X) / 2, InterPts[0].Y);
                            }
                            else
                            {
                                tempPoint = new Point(InterPts[0].X, Math.Min(InterPts[0].Y, tempStart.Y) + Math.Abs(InterPts[0].Y - tempStart.Y) / 2);
                            }
                            (lineconnector as LineConnector).InsertIntermediatePoint(tempPoint);
                            for (int i = 0; i < 2; i++)
                            {
                                Point v = InterPts[i];
                                v = MeasureUnitsConverter.ToPixels(v, (lineconnector as LineConnector).MeasurementUnit);
                                Vertex = new Thumb();
                                Vertex.Name = "Vertex";
                                Canvas.SetLeft(Vertex, v.X);
                                Canvas.SetTop(Vertex, v.Y);

                                this.adornerPanel.Children.Add(Vertex);
                                Style thumbStyle = (lineconnector as Control).FindResource("ConnectorAdornerVertexStyle") as Style;
                                if ((lineconnector as LineConnector).VertexStyle == thumbStyle)
                                {
                                    Vertex.Style = thumbStyle;
                                }
                                else
                                {
                                    Vertex.Style = (lineconnector as LineConnector).VertexStyle;
                                }
                                Vertex.SizeChanged += new SizeChangedEventHandler(Vertex_SizeChanged);
                                Vertex.DragDelta += new DragDeltaEventHandler(Thumb_DragDelta);
                                Vertex.DragStarted += new DragStartedEventHandler(Thumb_DragStarted);
                                Vertex.DragCompleted += new DragCompletedEventHandler(Thumb_DragCompleted);
                                Vertex.MouseDoubleClick += new MouseButtonEventHandler(Vertex_MouseDoubleClick);
                                Vertex.MouseRightButtonUp += new MouseButtonEventHandler(Vertex_MouseRightButtonUp);
                                Vertexs.Insert(i, Vertex);
                            }
                            vertexIndex = 2;
                            Vertex = Vertexs[2];
                        }
                    }
                }
                if (vertexIndex == InterPts.Count - 1)
                {
                    if (delayStack.Count > 4)
                    {
                        delayStackLast = true;
                        Point tempPoint = new Point();
                        int i = InterPts.Count - 1;
                        Point end = (lineconnector as LineConnector).GetTerminalPoints()[1];
                        if (Math.Abs(InterPts[i - 1].X - InterPts[i].X)
                        < Math.Abs(InterPts[i - 1].Y - InterPts[i].Y))
                        {
                            tempPoint = new Point(Math.Min(InterPts[i].X, end.X) + Math.Abs(InterPts[i].X - end.X) / 2, InterPts[i].Y);
                        }
                        else
                        {
                            tempPoint = new Point(InterPts[i].X, Math.Min(InterPts[i].Y, end.Y) + Math.Abs(InterPts[i].Y - end.Y) / 2);
                        }
                        (lineconnector as LineConnector).InsertIntermediatePoint(tempPoint);
                        for (int j = 0; j < 2; j++)
                        {
                            Point v = InterPts[InterPts.Count - 1 - j];
                            //v = MeasureUnitsConverter.ToPixels(v, (lineconnector as LineConnector).MeasurementUnit);
                            Vertex = new Thumb();
                            Vertex.Name = "Vertex";
                            Canvas.SetLeft(Vertex, v.X);
                            Canvas.SetTop(Vertex, v.Y);

                            this.adornerPanel.Children.Add(Vertex);
                            Style thumbStyle = (lineconnector as Control).FindResource("ConnectorAdornerVertexStyle") as Style;
                            if ((lineconnector as LineConnector).VertexStyle == thumbStyle)
                            {
                                Vertex.Style = thumbStyle;
                            }
                            else
                            {
                                Vertex.Style = (lineconnector as LineConnector).VertexStyle;
                            }
                            Vertex.SizeChanged += new SizeChangedEventHandler(Vertex_SizeChanged);
                            Vertex.DragDelta += new DragDeltaEventHandler(Thumb_DragDelta);
                            Vertex.DragStarted += new DragStartedEventHandler(Thumb_DragStarted);
                            Vertex.DragCompleted += new DragCompletedEventHandler(Thumb_DragCompleted);
                            Vertex.MouseDoubleClick += new MouseButtonEventHandler(Vertex_MouseDoubleClick);
                            Vertex.MouseRightButtonUp += new MouseButtonEventHandler(Vertex_MouseRightButtonUp);
                            Vertexs.Add(Vertex);
                        }
                        vertexIndex = InterPts.Count - 3;
                        Vertex = Vertexs[vertexIndex];
                    }
                }
                if (vertexIndex == InterPts.Count - 1)
                {
                    if (delayStack.Count != 0)
                    {
                        delayStackLast = true;
                    }
                    Point tempPoint = new Point();
                    int i = InterPts.Count - 1;
                    Point end = (lineconnector as LineConnector).GetTerminalPoints()[1];
                    if (Math.Abs(InterPts[i - 1].X - InterPts[i].X)
                    < Math.Abs(InterPts[i - 1].Y - InterPts[i].Y))
                    {
                        tempPoint = new Point(Math.Min(InterPts[i].X, end.X) + Math.Abs(InterPts[i].X - end.X) / 2, InterPts[i].Y);
                    }
                    else
                    {
                        tempPoint = new Point(InterPts[i].X, Math.Min(InterPts[i].Y, end.Y) + Math.Abs(InterPts[i].Y - end.Y) / 2);
                    }
                    (lineconnector as LineConnector).InsertIntermediatePoint(tempPoint);
                    for (int j = 0; j < 2; j++)
                    {
                        Point v = InterPts[InterPts.Count - 1 - j];
                        //v = MeasureUnitsConverter.ToPixels(v, (lineconnector as LineConnector).MeasurementUnit);
                        Vertex = new Thumb();
                        Vertex.Name = "Vertex";
                        Canvas.SetLeft(Vertex, v.X);
                        Canvas.SetTop(Vertex, v.Y);

                        this.adornerPanel.Children.Add(Vertex);
                        Style thumbStyle = (lineconnector as Control).FindResource("ConnectorAdornerVertexStyle") as Style;
                        if ((lineconnector as LineConnector).VertexStyle == thumbStyle)
                        {
                            Vertex.Style = thumbStyle;
                        }
                        else
                        {
                            Vertex.Style = (lineconnector as LineConnector).VertexStyle;
                        }
                        Vertex.SizeChanged += new SizeChangedEventHandler(Vertex_SizeChanged);
                        Vertex.DragDelta += new DragDeltaEventHandler(Thumb_DragDelta);
                        Vertex.DragStarted += new DragStartedEventHandler(Thumb_DragStarted);
                        Vertex.DragCompleted += new DragCompletedEventHandler(Thumb_DragCompleted);
                        Vertex.MouseDoubleClick += new MouseButtonEventHandler(Vertex_MouseDoubleClick);
                        Vertex.MouseRightButtonUp += new MouseButtonEventHandler(Vertex_MouseRightButtonUp);
                        Vertexs.Add(Vertex);
                    }
                    vertexIndex = InterPts.Count - 3;
                    Vertex = Vertexs[vertexIndex];
                }
                if (delayStackFirst)
                {
                    delayStack[0] = InterPts[0];// MeasureUnitsConverter.ToPixels(InterPts[0], (this.lineconnector as LineConnector).MeasurementUnit);
                }
                if (delayStackLast)
                {
                    delayStack[4] = InterPts[InterPts.Count - 1];// MeasureUnitsConverter.ToPixels(InterPts[InterPts.Count - 1], (this.lineconnector as LineConnector).MeasurementUnit);
                }

                if (dc != null && dc.View != null && dc.View.UndoRedoEnabled)
                {
                    if (delayStackFirst || delayStackLast)
                    {
                        foreach (object x in delayStack)
                        {
                            dc.View.UndoStack.Push(x);
                        }
                    }
                }
                if (InterPts.Count >= 3 && !(delayStackFirst || delayStackLast || IsTailThumb || IsHeadThumb))
                {
                    Point mousePt = pos;// MeasureUnitsConverter.FromPixels(pos, (this.lineconnector as LineConnector).MeasurementUnit);

                    double hLeft = Canvas.GetLeft(headThumb);// MeasureUnitsConverter.FromPixels(Canvas.GetLeft(headThumb), (this.lineconnector as LineConnector).MeasurementUnit);
                    double hTop = Canvas.GetTop(headThumb);// MeasureUnitsConverter.FromPixels(Canvas.GetTop(headThumb), (this.lineconnector as LineConnector).MeasurementUnit);

                    double tLeft = Canvas.GetLeft(tailThumb);//.FromPixels(Canvas.GetLeft(tailThumb), (this.lineconnector as LineConnector).MeasurementUnit);
                    double tTop = Canvas.GetTop(tailThumb);// MeasureUnitsConverter.FromPixels(Canvas.GetTop(tailThumb), (this.lineconnector as LineConnector).MeasurementUnit);

                    connectionPoints.Add(mousePt);
                    if (InterPts[vertexIndex - 1].X == InterPts[vertexIndex].X)
                    {
                        connectionPoints.Insert(0, new Point(mousePt.X, InterPts[vertexIndex - 1].Y));
                        connectionPoints.Add(new Point(InterPts[vertexIndex + 1].X, mousePt.Y));
                    }
                    else
                    {
                        connectionPoints.Insert(0, new Point(InterPts[vertexIndex - 1].X, mousePt.Y));
                        connectionPoints.Add(new Point(mousePt.X, InterPts[vertexIndex + 1].Y));
                    }
                    if (vertexIndex == 1)
                    {
                        connectionPoints.Insert(0, new Point(hLeft, hTop));
                    }
                    else
                    {
                        connectionPoints.Insert(0, InterPts[vertexIndex - 2]);
                    }
                    if (vertexIndex == InterPts.Count - 2)
                    {
                        connectionPoints.Add(new Point(tLeft, tTop));
                    }
                    else
                    {
                        connectionPoints.Add(InterPts[vertexIndex + 2]);
                    }
                }
            }

            return connectionPoints;
        }

        /// <summary>
        /// Adds the points.
        /// </summary>
        /// <param name="linePoints">The line points.</param>
        /// <returns>The connection points along with the added point.</returns>
        private List<Point> AddPoints(List<Point> linePoints)
        {
            List<Point> points = new List<Point>();
            for (int i = 0; i < linePoints.Count; i++)
            {
                points.Add(linePoints[i]);
            }

            points.Insert(1, new Point(points[1].X, points[0].Y));
            return points;
        }

        /// <summary>
        /// Identifies the hit object.
        /// </summary>
        /// <param name="hitPoint">The point to be tested.</param>
        /// <returns>True if hit object is Node ,false otherwise.</returns>
        private bool HitTesting(Point hitPoint)
        {
            //hitPoint = MeasureUnitsConverter.ToPixels(hitPoint, (lineconnector as LineConnector).MeasurementUnit);
            bool isporthit = false;
            DependencyObject hitObject = (diagramPanel as Panel).InputHitTest(hitPoint) as DependencyObject;
            while (hitObject != null)
            {
                if (hitObject is ConnectionPort)
                {
                    if ((hitObject as ConnectionPort).CenterPortReferenceNo == 0)
                    {
                        (hitObject as ConnectionPort).IsDragOverPort = true;
                        centerport = hitObject as ConnectionPort;
                        if (previoushitport != null)
                        {
                            previoushitport.IsDragOverPort = false;
                        }

                        HitPort = null;
                        previoushitport = null;
                        centerhit = true;
                    }
                    else
                    {
                        centerhit = false;
                        (lineconnector as LineConnector).Isnodehit = false;
                        foreach (Node n in dc.Model.Nodes)
                        {
                            foreach (ConnectionPort port in n.Ports)
                            {
                                port.IsDragOverPort = false;
                                if (port == hitObject as ConnectionPort)
                                {
                                    HitPort = hitObject as ConnectionPort;
                                    HitNodeConnector = (hitObject as ConnectionPort).Node;
                                    previoushitport = hitObject as ConnectionPort;
                                    previoushitport.IsDragOverPort = true;
                                    isporthit = true;
                                }
                            }
                        }
                    }

                    try
                    {
                        foreach (Group g in ((hitObject as ConnectionPort).Node as Node).Groups)
                        {
                            g.IsDragConnectionOver = false;
                        }
                    }
                    catch
                    {
                    }

                    HitNodeConnector = (hitObject as ConnectionPort).Node;
                    previousHitNode = (hitObject as ConnectionPort).Node;

                    if (HitNodeConnector != fixedNodeConnection)
                    {
                        if (!isporthit)
                        {
                            previousHitNode.IsDragConnectionOver = true;
                        }
                    }

                    return true;
                }

                if ((hitObject is Node) && !(hitObject is Layer))
                {
                    centerhit = false;
                    Node node = hitObject as Node;
                    HitNodeConnector = hitObject as Node;
                    previousHitNode = hitObject as Node;
                    if (node.PortVisibility != Visibility.Visible && node.IsPortEnabled)
                    {
                        portvisibilitycheck = true;
                        node.PortVisibility = Visibility.Visible;
                    }

                    if (node.Ports.Count == 1 && !node.IsGrouped)
                    {
                        HitNodeConnector = hitObject as Node;
                        previousHitNode = hitObject as Node;
                        previoushitport = null;
                        HitPort = null;
                        if (HitNodeConnector != fixedNodeConnection)
                        {
                            previousHitNode.IsDragConnectionOver = true;
                        }

                        foreach (Group g in HitNodeConnector.Groups)
                        {
                            g.IsDragConnectionOver = false;
                        }

                        return true;
                    }
                    else
                    {
                        if (node.IsGrouped)
                        {
                            node.IsDragConnectionOver = false;
                            HitNodeConnector = node.Groups[node.Groups.Count - 1] as Node;
                            previousHitNode = node.Groups[node.Groups.Count - 1] as Node;
                            if (previoushitport != null)
                            {
                                previoushitport.IsDragOverPort = false;
                            }

                            if (centerport != null)
                            {
                                centerport.IsDragOverPort = false;
                            }

                            previoushitport = null;
                            HitPort = null;
                            if (HitNodeConnector != fixedNodeConnection)
                            {
                                previousHitNode.IsDragConnectionOver = true;
                            }

                            return true;
                        }
                    }

                    if (centerhit)
                    {
                        return true;
                    }

                    return false;
                }

                hitObject = VisualTreeHelper.GetParent(hitObject);
            }

            if (previoushitport != null)
            {
                previoushitport.IsDragOverPort = false;
            }

            if (centerport != null)
            {
                centerport.IsDragOverPort = false;
            }

            if (previousHitNode != null)
            {
                previousHitNode.IsDragConnectionOver = false;
                if (portvisibilitycheck)
                {
                    previousHitNode.PortVisibility = Visibility.Collapsed;
                    portvisibilitycheck = false;
                }
            }

            HitNodeConnector = null;
            previoushitport = null;
            HitPort = null;
            return false;
        }

        /// <summary>
        /// Makes the end connection to the respective node by finding the correct direction of the node.
        /// </summary>
        /// <param name="pathPoints">Collection of points.</param>
        /// <param name="startPoint">The start point of the connector.</param>
        /// <param name="endPoint">The end point of the connector.</param>
        /// <param name="isTop">Flag indicating the top side of the source.</param>
        /// <param name="isBottom">Flag indicating the bottom side of the source.</param>
        /// <param name="isLeft">Flag indicating the left side of the source.</param>
        /// <param name="isRight">Flag indicating the right side of the source.</param>
        /// <param name="tisTop">Flag indicating the top side of the target.</param>
        /// <param name="tisBottom">Flag indicating the bottom side of the target.</param>
        /// <param name="tisLeft">Flag indicating the left side of the target.</param>
        /// <param name="tisRight">Flag indicating the right side of the target.</param>
        private void FindConnectionEnd(List<Point> pathPoints, Point startPoint, Point endPoint, bool isTop, bool isBottom, bool isLeft, bool isRight, bool tisTop, bool tisBottom, bool tisLeft, bool tisRight)
        {
            Point startpoint = new Point(0, 0);
            Point endpoint = new Point(0, 0);

            if (isRight)
            {
                startpoint = new Point(startPoint.X - 7, startPoint.Y);
            }
            else if (isBottom)
            {
                startpoint = new Point(startPoint.X, startPoint.Y - 7);
            }
            else if (isLeft)
            {
                startpoint = new Point(startPoint.X + 7, startPoint.Y);
            }
            else if (isTop)
            {
                startpoint = new Point(startPoint.X, startPoint.Y + 7);
            }

            if (tisRight)
            {
                endpoint = new Point(endPoint.X - 7, endPoint.Y);
            }
            else if (tisBottom)
            {
                endpoint = new Point(endPoint.X, endPoint.Y - 7);
            }
            else if (tisLeft)
            {
                endpoint = new Point(endPoint.X + 7, endPoint.Y);
            }
            else if (tisTop)
            {
                endpoint = new Point(endPoint.X, endPoint.Y + 7);
            }

            pathPoints.Insert(0, startpoint);
            pathPoints.Add(endpoint);
        }

        #endregion
    }
}
