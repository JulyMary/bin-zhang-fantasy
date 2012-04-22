// <copyright file="LineConnector.cs" company="Syncfusion">
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
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Reflection;
using System.Windows.Controls.Primitives;
using System.Collections.ObjectModel;
using Syncfusion.Windows.Shared;
using System.Windows.Interop;
using System.Windows.Threading;

namespace Syncfusion.Windows.Diagram
{
    /// <summary>
    /// Represents the Connectors to be used for making connections between the nodes. 
    /// </summary>
    /// <remarks>
    /// Connectors are objects that are used to create a link between two nodes. The node where the connection starts is known as the head node. The node where the connection ends is known as the tail node.
    /// <para>Three types of connectors are provided :Orthogonal, Straight and Bezier.</para>
    /// </remarks>
    /// <example>
    /// <para/>The following example shows how to create a <see cref="DiagramModel"/> in C# and add nodes and connections.
    /// <code language="C#">
    /// using System;
    /// using System.Collections.Generic;
    /// using System.Linq;
    /// using System.Text;
    /// using System.Windows;
    /// using System.Windows.Controls;
    /// using System.Windows.Data;
    /// using System.Windows.Documents;
    /// using System.Windows.Input;
    /// using System.Windows.Media;
    /// using System.Windows.Media.Imaging;
    /// using System.Windows.Navigation;
    /// using System.Windows.Shapes;
    /// using System.ComponentModel;
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
    ///       Model = new DiagramModel ();
    ///       View = new DiagramView ();
    ///       Control.View = View;
    ///       Control.Model = Model;
    ///       View.Bounds = new Thickness(0, 0, 1000, 1000);
    ///       //Specifies the node
    ///        Node n = new Node(Guid.NewGuid(), "Start");
    ///        n.Shape = Shapes.FlowChart_Start;
    ///        n.IsLabelEditable = true;
    ///        n.Label = "Start";
    ///        n.Level = 1;
    ///        n.OffsetX = 150;
    ///        n.OffsetY = 25;
    ///        n.Width = 150;
    ///        n.Height = 75;
    ///        n.ToolTip="Start Node";
    ///        Model.Nodes.Add(n);
    ///        ConnectionPort port = new ConnectionPort();
    ///        port.Node=n;
    ///        port.Left=75;
    ///        port.Top=10;
    ///        port.PortShape = PortShapes.Arrow;
    ///        port.PortStyle.Fill = Brushes.Transparent;
    ///        port.Height = 11;
    ///        port.Width = 11;
    ///        n.Ports.Add(port);
    ///         Node n1 = new Node(Guid.NewGuid(), "Decision1");
    ///         n1.Shape = Shapes.FlowChart_Process;
    ///         n1.IsLabelEditable = true;
    ///         n1.Label = "Alarm Rings";
    ///         n1.Level = 2;
    ///         n1.OffsetX = 150;
    ///         n1.OffsetY = 125;
    ///         n1.Width = 150;
    ///         n1.Height = 75;
    ///        Model.Nodes.Add(n1);
    ///        ConnectionPort port1 = new ConnectionPort();
    ///        port1.Node=n;
    ///        port1.Left=75;
    ///        port1.Top=50;
    ///        port1.PortShape = PortShapes.Arrow;
    ///        port1.PortStyle.Fill = Brushes.Transparent;
    ///        port1.Height = 11;
    ///        port1.Width = 11;
    ///        n1.Ports.Add(port1);
    ///         LineConnector o = new LineConnector();
    ///         o.ConnectorType = ConnectorType.Straight;
    ///         o.TailNode = n1;
    ///         o.HeadNode = n;
    ///         o.LabelHorizontalAlignment = HorizontalAlignment.Center;
    ///         o.LabelVerticalAlignment = HorizontalAlignment.Center;
    ///         o.Label="Syncfusion";
    ///         o.ConnectionHeadPort = port;
    ///         o.ConnectionTailPort = port1;
    ///         o.HeadDecoratorShape=DecoratorShape.Arrow;
    ///         o.TailDecoratorShape=DecoratorShape.Arrow;
    ///         Model.Connections.Add(o);
    ///    }
    ///    }
    ///    }
    /// </code>
    /// </example>
#if SyncfusionFramework4_0
    [DesignTimeVisible(false)]
#endif
    public class LineConnector : ConnectorBase, INodeGroup
    {
        #region Class Fields

        internal Point m_TempStartPoint;
        internal Point m_TempEndPoint;
        internal List<Point> m_TempInerPts;
        //internal Point m_TempDragingPoint;

        /// <summary>
        /// Used to flag bridging
        /// </summary>
        internal bool bridged = false;

        /// <summary>
        /// Used to invalidate line bridging
        /// </summary>
        internal bool invalidateBridging = true;

        /// <summary>
        /// Used to store the smallest x value
        /// </summary>
        internal double minx;

        /// <summary>
        /// Used to store the largest x value
        /// </summary>
        internal double maxx;

        /// <summary>
        /// Used to store the smallest y value
        /// </summary>
        internal double miny;

        /// <summary>
        /// Used to store the largest y value
        /// </summary>
        internal double maxy;

        /// <summary>
        /// current Measurement unit selected
        /// </summary>
        //private MeasureUnits currentMUSelected;

        /// <summary>
        /// sets ts Widened Path Geomentry
        /// </summary>
        private Geometry WidenedPathGeometry;


        //internal bool isfired;

        /// <summary>
        /// Used to store the bend length.
        /// </summary>
        private double mBendLength = 10d;

        /// <summary>
        /// Used to store connection end space
        /// </summary>
        private double mConnectionEndSpace = 6d;

        /// <summary>
        /// Used to store the editor instance
        /// </summary>
        internal LabelEditor editor;

        /// <summary>
        /// Used to store the view instance.
        /// </summary>
        private DiagramView dview;

        /// <summary>
        /// Used to store the DiagramControl instance
        /// </summary>
        internal DiagramControl dc;

        /// <summary>
        /// Used to store boolean information if value is defaulted
        /// </summary>
        private bool m_isDefaulted = true;

        /// <summary>
        /// Used to store boolean information if node is hit.
        /// </summary>
        private bool m_isnodehit = false;

        /// <summary>
        /// Used to check if mouse is double clicked.
        /// </summary>
        private bool isdoubleclicked = false;

        /// <summary>
        /// Used to store the head decorator shape for internal use.
        /// </summary>
        private DecoratorShape headshape = DecoratorShape.None;

        /// <summary>
        /// Used to store the tail decorator shape for internal use.
        /// </summary>
        private DecoratorShape tailshape = DecoratorShape.None;

        /// <summary>
        /// Used to check if nodes are overlapped.
        /// </summary>
        private bool m_isoverlapped = false;

        /// <summary>
        /// Used to check if default value is used for label width.
        /// </summary>
        internal bool isdefaulted = false;

        internal bool isClicked;

        #endregion

        #region Initialization

        /// <summary>
        /// Initializes static members of the <see cref="LineConnector"/> class.
        /// </summary>
        static LineConnector()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(LineConnector), new FrameworkPropertyMetadata(typeof(LineConnector)));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LineConnector"/> class.
        /// </summary>
        public LineConnector()
            : base()
        {
            this.ID = Guid.NewGuid();
            this.Loaded += new RoutedEventHandler(LineConnector_Loaded);
            this.AddHandler(Control.MouseLeftButtonUpEvent, new MouseButtonEventHandler(Lineconnector_MouseLeftButtonUp), true);
            this.Unloaded += new RoutedEventHandler(Connection_Unloaded);
            //currentMUSelected = this.MeasurementUnit;
            if (this.IntermediatePoints == null)
            {
                IntermediatePoints = new List<Point>();
                IntermediatePoints.Add(new Point(0, 0));
                IntermediatePoints.Add(new Point(0, 0));
            }
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="LineConnector"/> class.
        /// </summary>
        /// <param name="source">The source node.</param>
        /// <param name="sink">The sink node.</param>
        /// <param name="view">The view instance.</param>
        public LineConnector(Node source, Node sink, DiagramView view)
            : base()
        {
            dview = view;
            this.ID = Guid.NewGuid();
            if (this.IntermediatePoints == null)
            {
                IntermediatePoints = new List<Point>();
                IntermediatePoints.Add(new Point(0, 0));
                IntermediatePoints.Add(new Point(0, 0));
            }
            this.ConnectorType = (dview.Page as DiagramPage).ConnectorType;
            this.HeadNode = source;
            this.TailNode = sink;
            this.Loaded += new RoutedEventHandler(LineConnector_Loaded);
            this.Unloaded += new RoutedEventHandler(Connection_Unloaded);
            //m_startpointposition.X = DropPoint.X - 25;
            //m_startpointposition.Y = DropPoint.Y - 25;
            //m_endpointposition.X = DropPoint.X + 25;
            //m_endpointposition.Y = DropPoint.Y + 25;
            //currentMUSelected = this.MeasurementUnit;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LineConnector"/> class.
        /// </summary>
        /// <param name="view">The view instance.</param>
        public LineConnector(DiagramView view)
            : base()
        {
            if (view != null)
            {
                dview = view;
                this.Loaded += new RoutedEventHandler(LineConnector_Loaded);
                this.ID = Guid.NewGuid();
                this.ConnectorType = (dview.Page as DiagramPage).ConnectorType;
                this.Unloaded += new RoutedEventHandler(Connection_Unloaded);
                //currentMUSelected = this.MeasurementUnit;
            }
            else
            {
                this.Loaded += new RoutedEventHandler(DelayedConnectorType);
                this.ID = Guid.NewGuid();
                this.Loaded += new RoutedEventHandler(LineConnector_Loaded);
                this.AddHandler(Control.MouseLeftButtonUpEvent, new MouseButtonEventHandler(Lineconnector_MouseLeftButtonUp), true);
                this.Unloaded += new RoutedEventHandler(Connection_Unloaded);
                //currentMUSelected = this.MeasurementUnit;
            }
            if (this.IntermediatePoints == null)
            {
                IntermediatePoints = new List<Point>();
                IntermediatePoints.Add(new Point(0, 0));
                IntermediatePoints.Add(new Point(0, 0));
            }
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            dc =  DiagramPage.GetDiagramControl(this);
            //if (dc != null && dc.View != null && dc.View.Page != null)
            //{
            //    PxStartPointPosition = MeasureUnitsConverter.ToPixels(StartPointPosition, (dc.View.Page as DiagramPage).MeasurementUnits);
            //    PxEndPointPosition = MeasureUnitsConverter.ToPixels(EndPointPosition, (dc.View.Page as DiagramPage).MeasurementUnits);
            //    PxConnectionEndSpace = MeasureUnitsConverter.ToPixels(ConnectionEndSpace, (dc.View.Page as DiagramPage).MeasurementUnits);
            //}
            //else
            //{
            //    if (PxStartPointPosition.Equals(new Point(0,0)))
            //    {
            //        PxStartPointPosition = StartPointPosition;
            //    }
            //    if (PxEndPointPosition.Equals(new Point(0, 0)))
            //    {
            //        PxEndPointPosition = EndPointPosition;
            //    }
            //    if (PxConnectionEndSpace.Equals(new Point(0, 0)))
            //    {
            //        PxConnectionEndSpace = ConnectionEndSpace;
            //    }
            //}
        }

        #endregion

        #region Events

        /// <summary>
        /// Handles the Unloaded event of the Connection control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void Connection_Unloaded(object sender, RoutedEventArgs e)
        {
            //if (dc != null && dc.View != null && dc.View.Page != null)
            //{
            //    if (dc.View.Scrollviewer != null && !(dc.View.Page as DiagramPage).IsDiagrampageLoaded && dc.Model.LayoutType == LayoutType.None)
            //    {
            //        if (dview.IsMouseScrolled)
            //        {
            //            //if ((dview.Page as DiagramPage).Minleft >= 0)
            //            //{
            //            //    dview.Scrollviewer.ScrollToHorizontalOffset(Math.Abs((dview.Page as DiagramPage).HorValue));
            //            //}
            //            //else
            //            //{
            //            //    dview.Scrollviewer.ScrollToHorizontalOffset(Math.Abs((dview.Page as DiagramPage).HorValue * dview.CurrentZoom));
            //            //}

            //            //if ((dview.Page as DiagramPage).MinTop >= 0)
            //            //{
            //            //    dview.Scrollviewer.ScrollToVerticalOffset(Math.Abs((dview.Page as DiagramPage).VerValue));
            //            //}
            //            //else
            //            //{
            //            //    dview.Scrollviewer.ScrollToVerticalOffset(Math.Abs((dview.Page as DiagramPage).VerValue * dview.CurrentZoom));
            //            //}
            //        }
            //        else
            //        {
            //            //dc.View.Scrollviewer.ScrollToHorizontalOffset(Math.Abs((dc.View.Page as DiagramPage).HorValue) * dc.View.CurrentZoom);
            //            //dc.View.Scrollviewer.ScrollToVerticalOffset(Math.Abs((dc.View.Page as DiagramPage).VerValue) * dc.View.CurrentZoom);
            //        }
            //    }
            //}
            //// remove adorner
            if (this.LineAdorner != null)
            {
                IDiagramPage diagramPanel = VisualTreeHelper.GetParent(this) as IDiagramPage;

                AdornerLayer adorner = AdornerLayer.GetAdornerLayer(this);
                if (adorner != null)
                {
                    adorner.Remove(this.LineAdorner);
                    this.LineAdorner = null;
                }
            }

            if (this.HeadNode != null)
            {
                (this.HeadNode as Node).PropertyChanged -= Line_PropertyChanged;
            }
            if (this.TailNode != null)
            {
                (this.TailNode as Node).PropertyChanged -= Line_PropertyChanged;
            }
        }

        private void DelayedConnectorType(object sender, RoutedEventArgs e)
        {
            this.Loaded -= DelayedConnectorType;
            if (dc != null && dc.View != null && dc.View.Page != null)
            {
                this.ConnectorType = (dc.View.Page as DiagramPage).ConnectorType; ;
            }
        }


        /// <summary>
        /// Handles the Loaded event of the LineConnector control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void LineConnector_Loaded(object sender, RoutedEventArgs e)
        {
            //Node.SetUnitBinding("PxStartPointPosition", "MeasurementUnit", ConnectorBase.StartPointPositionProperty, this);
            //Node.SetUnitBinding("PxEndPointPosition", "MeasurementUnit", ConnectorBase.EndPointPositionProperty, this);
            //Node.SetUnitBinding("PxConnectionEndSpace", "MeasurementUnit", LineConnector.ConnectionEndSpaceProperty, this);

            dview = GetDiagramView(this);
            if (this.IsSelected)
            {
                ShowAdorner();
                dview.SelectionList.Add(this);
            }
            if (this.HeadNode != null)
            {
                (this.HeadNode as Node).PropertyChanged -= Line_PropertyChanged;
                (this.HeadNode as Node).PropertyChanged += Line_PropertyChanged;
            }
            if (this.TailNode != null)
            {
                (this.TailNode as Node).PropertyChanged -= Line_PropertyChanged;
                (this.TailNode as Node).PropertyChanged += Line_PropertyChanged;
            }

            if (this.LabelTemplate != null)
            {
                this.UpdateDecoratorPosition();
            }
            if (this.LabelWidth == 0)
            {
                this.LabelWidth = Distance;
                isdefaulted = true;
            }

            if (HeadNode != null)
            {
                this.HeadNodeReferenceNo = (this.HeadNode as Node).ReferenceNo;
            }
            if (TailNode != null)
            {
                this.TailNodeReferenceNo = (this.TailNode as Node).ReferenceNo;
            }
            if (ConnectionTailPort !=null)
            {
                this.TailPortReferenceNo = this.ConnectionTailPort.PortReferenceNo;
            }
            if (ConnectionHeadPort != null)
            {
                this.HeadPortReferenceNo = this.ConnectionHeadPort.PortReferenceNo;
            }

            try
            {
                dview = Node.GetDiagramView(this);
                //if (dview != null && this.minx > 0)
                //    (dview.Page as DiagramPage).Hor = 0;
                //if (dview != null && this.miny > 0)
                //    (dview.Page as DiagramPage).Ver = 0;
                //if (dc != null)
                //{
                //    if (dc.View.Scrollviewer != null && (dview.Page as DiagramPage).IsConnectorDropped && dc.Model.LayoutType != LayoutType.None)
                //    {
                //        if (dview.IsMouseScrolled)
                //        {
                //            if ((dview.Page as DiagramPage).Minleft >= 0)
                //            {
                //                dview.Scrollviewer.ScrollToHorizontalOffset(Math.Abs((dview.Page as DiagramPage).HorValue));
                //            }
                //            else
                //            {
                //                dview.Scrollviewer.ScrollToHorizontalOffset(Math.Abs((dview.Page as DiagramPage).HorValue * dview.CurrentZoom));
                //            }

                //            if ((dview.Page as DiagramPage).MinTop >= 0)
                //            {
                //                dview.Scrollviewer.ScrollToVerticalOffset(Math.Abs((dview.Page as DiagramPage).VerValue));
                //            }
                //            else
                //            {
                //                dview.Scrollviewer.ScrollToVerticalOffset(Math.Abs((dview.Page as DiagramPage).VerValue * dview.CurrentZoom));
                //            }
                //        }
                //        else
                //        {
                //            dc.View.Scrollviewer.ScrollToHorizontalOffset(Math.Abs((dc.View.Page as DiagramPage).HorValue) * dc.View.CurrentZoom);
                //            dc.View.Scrollviewer.ScrollToVerticalOffset(Math.Abs((dc.View.Page as DiagramPage).VerValue) * dc.View.CurrentZoom);
                //        }
                //    }
                //}

                //(dview.Page as DiagramPage).IsConnectorDropped = false;
                //(dview.Page as DiagramPage).IsDiagrampageLoaded = false;
                if (!dview.IsPageEditable)
                {
                    DiagramView.PageEdit = false;
                    IsLabelEditable = false;
                }
                Path HeadDecoratorPath = (this.GetTemplateChild("PART_HeadDecoratorAnchorPath") as Path);
                Path TailDecoratorPath = (this.GetTemplateChild("PART_SinkAnchorPath") as Path);
                (HeadDecoratorPath.Parent as Grid).SizeChanged += new SizeChangedEventHandler(LineConnectorDecorator_SizeChanged);
                (TailDecoratorPath.Parent as Grid).SizeChanged += new SizeChangedEventHandler(LineConnectorDecorator_SizeChanged);
                HeadDecoratorPath.SizeChanged += new SizeChangedEventHandler(LineConnectorDecorator_SizeChanged);
                TailDecoratorPath.SizeChanged += new SizeChangedEventHandler(LineConnectorDecorator_SizeChanged);
            }
            catch
            {
            }

            if (this.m_LineDrawing)
            {
                this.m_LineDrawing = false;
                dc.View.Page.InvalidateMeasure();
            }
        }

        void LineConnectorDecorator_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            TranslateTransform tth = new TranslateTransform(-(this.GetTemplateChild("PART_HeadDecoratorAnchorPath") as Path).ActualWidth + 2, -(this.GetTemplateChild("PART_HeadDecoratorAnchorPath") as Path).ActualHeight / 2);
            RotateTransform headAngle = new RotateTransform(this.HeadDecoratorAngle, 0, 0);
            TransformGroup tthg = new TransformGroup();
            tthg.Children.Add(tth);
            tthg.Children.Add(headAngle);
            (this.GetTemplateChild("PART_HeadDecoratorAnchorPath") as Path).RenderTransform = tthg;

            TranslateTransform ttt = new TranslateTransform(-(this.GetTemplateChild("PART_SinkAnchorPath") as Path).ActualWidth + 2, -(this.GetTemplateChild("PART_SinkAnchorPath") as Path).ActualHeight / 2);
            RotateTransform tailAngle = new RotateTransform(this.TailDecoratorAngle, 0, 0);
            TransformGroup tttg = new TransformGroup();
            tttg.Children.Add(ttt);
            tttg.Children.Add(tailAngle);
            (this.GetTemplateChild("PART_SinkAnchorPath") as Path).RenderTransform = tttg;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the internal tail decorator shape. Used for internal assignments in case of overlapped nodes.
        /// </summary>
        /// <value>The internal tail decorator shape.</value>
        internal DecoratorShape InternalTailShape
        {
            get { return tailshape; }
            set { tailshape = value; }
        }

        /// <summary>
        /// Gets or sets the internal head decorator shape. Used for internal assignments in case of overlapped nodes.
        /// </summary>
        /// <value>The internal head decorator shape.</value>
        internal DecoratorShape InternalHeadShape
        {
            get { return headshape; }
            set { headshape = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the nodes have overlapped.
        /// </summary>
        /// <value>
        /// <c>true</c> if overlapped; otherwise, <c>false</c>.
        /// </value>
        internal bool IsOverlapped
        {
            get
            {
                return m_isoverlapped;
            }

            set
            {
                m_isoverlapped = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="LineConnector"/> is node is hit.
        /// </summary>
        /// <value><c>true</c> if node is hit; otherwise, <c>false</c>.</value>
        internal bool Isnodehit
        {
            get
            {
                return m_isnodehit;
            }

            set
            {
                m_isnodehit = value;
            }
        }

        /// <summary>
        /// Gets or sets the bent line length which is used only for Orthogonal Line ConnectorType.
        /// </summary>
        /// <remarks>
        /// Default value is 10d.
        /// </remarks>
        internal double BendLength
        {
            get
            {
                return mBendLength;
            }

            set
            {
                mBendLength = value;
            }
        }

        /// <summary>
        /// Gets or sets the distance between the connector end position and the node.
        /// </summary>
        /// <value>
        /// Type: <see cref="double"/>
        /// Value indicating the distance.
        /// </value>
        /// <example>
        /// <code language="C#">
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
        ///       Model = new DiagramModel ();
        ///       View = new DiagramView ();
        ///       Control.View = View;
        ///       Control.Model = Model;
        ///       View.Bounds = new Thickness(0, 0, 1000, 1000);
        ///       //Specifies the node
        ///        Node n = new Node(Guid.NewGuid(), "Start");
        ///        n.Shape = Shapes.FlowChart_Start;
        ///        n.IsLabelEditable = true;
        ///        n.Label = "Start";
        ///        n.Level = 1;
        ///        n.OffsetX = 150;
        ///        n.OffsetY = 25;
        ///        n.Width = 150;
        ///        n.Height = 75;
        ///        n.ToolTip="Start Node";
        ///        Model.Nodes.Add(n);
        ///         Node n1 = new Node(Guid.NewGuid(), "Decision1");
        ///         n1.Shape = Shapes.FlowChart_Process;
        ///         n1.IsLabelEditable = true;
        ///         n1.Label = "Alarm Rings";
        ///         n1.Level = 2;
        ///         n1.OffsetX = 150;
        ///         n1.OffsetY = 125;
        ///         n1.Width = 150;
        ///         n1.Height = 75;
        ///        Model.Nodes.Add(n1);
        ///         LineConnector o = new LineConnector();
        ///         o.ConnectorType = ConnectorType.Straight;
        ///         o.TailNode = n1;
        ///         o.HeadNode = n;
        ///         o.Label="Syncfusion";
        ///         o.ConnectionEndSpace= 6d; 
        ///         Model.Connections.Add(o);
        ///    }
        ///    }
        ///    }
        /// </code>
        /// </example>
        /// <remarks>
        ///  Default value is 6. 
        ///  In case, if a decorator shape other than none is specified, 
        ///  a value >=6 should be given to make the connection start from the edge of the node,
        ///  or else the connector may cross the edge of the node .
        /// </remarks>
        public double ConnectionEndSpace
        {
            get { return (double)GetValue(ConnectionEndSpaceProperty); }
            set { SetValue(ConnectionEndSpaceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ConnectionEndSpace.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ConnectionEndSpaceProperty =
            DependencyProperty.Register("ConnectionEndSpace", typeof(double), typeof(LineConnector), new UIPropertyMetadata(6d));
        
        /// <summary>
        /// Gets or sets a value indicating whether this instance is defaulted.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is defaulted; otherwise, <c>false</c>.
        /// </value>
        internal bool IsDefaulted
        {
            get
            {
                return m_isDefaulted;
            }

            set
            {
                m_isDefaulted = value;
            }
        }

        #endregion

        #region Class Override


        internal void ConnectorSelection()
        {
            //dview = GetDiagramView(this);
            //if(this.IsSelected)
            //(dview as DiagramView).SelectionList.Add(this);

            LineConnector n = this as LineConnector;
            if (n.dc != null)
            {
                IDiagramPage m_diagramPage = VisualTreeHelper.GetParent(this) as IDiagramPage;
                if (n.IsSelected && dview != null && (n.dc.View.Page as DiagramPage).SelectionList != null)
                {
                    (dview as DiagramView).SelectionList.Add(this);

                    ShowAdorner();
                }
                else
                {
                    (dview as DiagramView).SelectionList.Remove(this);
                    HideAdorner();
                }
            }


            if (n.IsSelected)
            {
                ConnectorRoutedEventArgs newEventArgs = new ConnectorRoutedEventArgs(n as LineConnector);
                newEventArgs.RoutedEvent = DiagramView.ConnectorSelectedEvent;
                n.RaiseEvent(newEventArgs);
                if (this.IsLoaded)
                    ShowAdorner();
                //  RaiseEvent(newEventArgs);
            }
            else
            {
                ConnectorRoutedEventArgs newEventArgs = new ConnectorRoutedEventArgs(n as LineConnector);
                newEventArgs.RoutedEvent = DiagramView.ConnectorUnSelectedEvent;
                HideAdorner();
                n.RaiseEvent(newEventArgs);
            }

        }
        /// <summary>
        /// Invoked whenever application code or internal processes call
        /// <see cref="System.Windows.FrameworkElement.ApplyTemplate"/> method.
        /// </summary>
        public override void OnApplyTemplate()
        {
            dc = DiagramPage.GetDiagramControl((FrameworkElement)this);
            dview = GetDiagramView(this);
            editor = GetTemplateChild("PART_ConnectorLabelEditor") as LabelEditor;
            editor.Loaded += new RoutedEventHandler(editor_Loaded);
            editor.AfterLabelEdit += new LabelEditor.AfterLabelEditHandler(editor_AfterLabelEdit);
            if (dview != null)
            {
                if (!dview.SelectionList.Contains(this) && this.IsSelected)
                {
                    ConnectorSelection();
                }
            }
            if (dc != null)
            {
                //if (DiagramControl.IsPageLoaded)
                //{
                //    (dc.View.Page as DiagramPage).IsDiagrampageLoaded = true;
                //}
                //else
                //{
                //    (dc.View.Page as DiagramPage).IsDiagrampageLoaded = false;
                //}

                //if (dc.View.Scrollviewer != null && dc.Model.LayoutType == LayoutType.None && !(dc.View.Page as DiagramPage).IsDiagrampageLoaded)
                //{
                //    dc.View.Scrollviewer.ScrollToHorizontalOffset(Math.Abs((dc.View.Page as DiagramPage).HorValue) * dc.View.CurrentZoom);
                //    dc.View.Scrollviewer.ScrollToVerticalOffset(Math.Abs((dc.View.Page as DiagramPage).VerValue) * dc.View.CurrentZoom);
                //}
            }
            if (dc != null)
            {
                ContextMenu_Delete = dc.m_ResourceWrapper.ContextMenu_Delete;
                ContextMenu_Grouping = dc.m_ResourceWrapper.ContextMenu_Grouping;
                ContextMenu_Grouping_Group = dc.m_ResourceWrapper.ContextMenu_Grouping_Group;
                ContextMenu_Grouping_Ungroup = dc.m_ResourceWrapper.ContextMenu_Grouping_Ungroup;
                ContextMenu_Order = dc.m_ResourceWrapper.ContextMenu_Order;
                ContextMenu_Order_BringForward = dc.m_ResourceWrapper.ContextMenu_Order_BringForward;
                ContextMenu_Order_BringToFront = dc.m_ResourceWrapper.ContextMenu_Order_BringToFront;
                ContextMenu_Order_SendBackward = dc.m_ResourceWrapper.ContextMenu_Order_SendBackward;
                ContextMenu_Order_SendToBack = dc.m_ResourceWrapper.ContextMenu_Order_SendToBack;
            }
        }

        internal static DiagramView GetDiagramView(DependencyObject element)
        {
            while (element != null && !(element is DiagramView))
            {
                element = VisualTreeHelper.GetParent(element);
            }

            return element as DiagramView;
        }

        //private void ConnectorSelection()
        //{
        //    dview = GetDiagramView(this);
        //    //if(this.IsSelected)
        //    //(dview as DiagramView).SelectionList.Add(this);

        //    LineConnector n = this as LineConnector;

        //    if ((n.GetTemplateChild("PART_Rotator") as Rotator) != null)
        //    {
        //        if (n.AllowDrop && n.IsSelected)
        //        {
        //            (n.GetTemplateChild("PART_Rotator") as Rotator).Visibility = Visibility.Visible;
        //        }
        //        else
        //        {
        //            (n.GetTemplateChild("PART_Rotator") as Rotator).Visibility = Visibility.Collapsed;
        //        }
        //    }
        //    if (n.IsSelected)
        //    {
        //        ConnectorRoutedEventArgs newEventArgs = new ConnectorRoutedEventArgs(n as LineConnector);
        //        newEventArgs.RoutedEvent = DiagramView.ConnectorSelectedEvent;
        //        n.RaiseEvent(newEventArgs);
        //        //  RaiseEvent(newEventArgs);
        //    }
        //    else
        //    {
        //        ConnectorRoutedEventArgs newEventArgs = new ConnectorRoutedEventArgs(n as LineConnector);
        //        newEventArgs.RoutedEvent = DiagramView.ConnectorUnSelectedEvent;
        //        n.RaiseEvent(newEventArgs);
        //    }

        //}

        void editor_BeforeLabelEdit(object sender, BeforeLabelEditEventArgs e)
        {
        }

        private ContentPresenter presenter;
        void editor_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (Visual visual in VisualUtils.EnumChildrenOfType(this.editor, typeof(ContentPresenter)))
            {
                if ((visual as ContentPresenter).Name == "Content")
                {
                    presenter = visual as ContentPresenter;
                    if (editor.TemplatedParent != null && editor.TemplatedParent is LineConnector)
                    {
                        presenter.HorizontalAlignment = HorizontalAlignment.Left;
                    }
                }
            }

            this.UpdateConnectorPathGeometry();
            this.UpdateDecoratorPosition();
        }

        void editor_AfterLabelEdit(object sender, AfterLabelEditEventArgs e)
        {
            this.UpdateConnectorPathGeometry();
            this.UpdateDecoratorPosition();
        }

        void editor_KeyDown(object sender, KeyEventArgs e)
        {
        }

        void editor_LostFocus(object sender, RoutedEventArgs e)
        {
        }


        /// <summary>
        /// Invoked when Label editing is started.
        /// </summary>
        public void Labeledit()
        {
            if (IsLabelEditable)
            {
                editor.LabelEditStartInternal(editor);
            }
        }

        /// <summary>
        /// Invoked when Label editing is complete
        /// </summary>
        internal void CompleteConnEditing()
        {
            editor.CompleteHeaderEditInternal(editor, true);
        }

        internal string ContextMenu_Delete;
        internal string ContextMenu_Grouping;
        internal string ContextMenu_Grouping_Group;
        internal string ContextMenu_Grouping_Ungroup;
        internal string ContextMenu_Order;
        internal string ContextMenu_Order_BringForward;
        internal string ContextMenu_Order_BringToFront;
        internal string ContextMenu_Order_SendBackward;
        internal string ContextMenu_Order_SendToBack;

        /// <summary>
        /// Invoked when an unhandled <see cref="E:System.Windows.UIElement.MouseRightButtonUp"/> routed event reaches an element in its route that is derived from this class. Implement this method to add class handling for this event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.Windows.Input.MouseButtonEventArgs"/> that contains the event data. The event data reports that the right mouse button was released.</param>
        protected override void OnMouseRightButtonUp(MouseButtonEventArgs e)
        {
            if (dview != null)
            {
                if (dview.IsPageEditable && dview.LineConnectorContextMenu == null && this.ContextMenu == null)
                {
                    MenuItem m = new MenuItem();
                    m.Header = ContextMenu_Order;
                    MenuItem m1 = new MenuItem();
                    m1.Header = ContextMenu_Order_BringToFront;
                    m1.Click += new RoutedEventHandler(M1_Click);
                    m.Items.Add(m1);
                    MenuItem m2 = new MenuItem();
                    m2.Header = ContextMenu_Order_BringForward;
                    m2.Click += new RoutedEventHandler(M2_Click);
                    m.Items.Add(m2);
                    MenuItem m3 = new MenuItem();
                    m3.Header = ContextMenu_Order_SendBackward;
                    m3.Click += new RoutedEventHandler(M3_Click);
                    m.Items.Add(m3);
                    MenuItem m4 = new MenuItem();
                    m4.Header = ContextMenu_Order_SendToBack;
                    m4.Click += new RoutedEventHandler(M4_Click);
                    m.Items.Add(m4);

                    MenuItem group = new MenuItem();
                    group.Header = ContextMenu_Grouping;
                    MenuItem g1 = new MenuItem();
                    g1.Header = ContextMenu_Grouping_Group;
                    g1.Click += new RoutedEventHandler(G1_Click);
                    group.Items.Add(g1);
                    MenuItem g2 = new MenuItem();
                    g2.Header = ContextMenu_Grouping_Ungroup;
                    g2.Click += new RoutedEventHandler(G2_Click);
                    group.Items.Add(g2);

                    MenuItem del = new MenuItem();
                    del.Header = ContextMenu_Delete;
                    del.Click += new RoutedEventHandler(Del_Click);

                    this.ContextMenu = new ContextMenu();
                    this.ContextMenu.Items.Add(m);
                    this.ContextMenu.Items.Add(group);
                    this.ContextMenu.Items.Add(del);

                    if (this.IsSelected)
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

                    if (this.IsGrouped)
                    {
                        foreach (Group g in this.Groups)
                        {
                            if (g.IsSelected)
                            {
                                del.IsEnabled = true;
                            }
                        }
                    }

                    if (dview != null && dview.SelectionList.Count > 1 && !this.IsGrouped)
                    {
                        g1.IsEnabled = true;
                    }
                    else
                    {
                        foreach (INodeGroup item in dview.SelectionList)
                        {
                            if (item is Group)
                            {
                                foreach (INodeGroup n in (item as Group).NodeChildren)
                                {
                                    foreach (INodeGroup node in dview.SelectionList)
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

                    if (dview != null && dview.SelectionList.Count <= 1)
                    {
                        g1.IsEnabled = false;
                    }
                }
                else
                {
                    if (this.ContextMenu == null)
                    {
                        this.ContextMenu = dview.LineConnectorContextMenu;
                    }
                }
            }

            base.OnMouseRightButtonUp(e);
        }

        /// <summary>
        /// Handles the Click event of the delete menu item.
        /// </summary>
        /// <param name="sender">The diagram view.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void Del_Click(object sender, RoutedEventArgs e)
        {
            DiagramCommandManager.Delete.Execute(dview.Page, dview);
        }

        /// <summary>
        /// Handles the Click event of the bring to front menu item.
        /// </summary>
        /// <param name="sender">The diagram view.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void M1_Click(object sender, RoutedEventArgs e)
        {
            DiagramCommandManager.BringToFront.Execute(dview.Page, dview);
        }

        /// <summary>
        /// Handles the Click event of the bring forward menu item.
        /// </summary>
        /// <param name="sender">The diagram view.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void M2_Click(object sender, RoutedEventArgs e)
        {
            DiagramCommandManager.MoveForward.Execute(dview.Page, dview);
        }

        /// <summary>
        /// Handles the Click event of the send backward menu item.
        /// </summary>
        /// <param name="sender">The diagram view.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void M3_Click(object sender, RoutedEventArgs e)
        {
            DiagramCommandManager.SendBackward.Execute(dview.Page, dview);
        }

        /// <summary>
        /// Handles the Click event of the send to back menu item.
        /// </summary>
        /// <param name="sender">The diagram view.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void M4_Click(object sender, RoutedEventArgs e)
        {
            DiagramCommandManager.SendToBack.Execute(dview.Page, dview);
        }

        /// <summary>
        /// Handles the Click event of the group menu item.
        /// </summary>
        /// <param name="sender">The diagram view.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void G1_Click(object sender, RoutedEventArgs e)
        {
            DiagramCommandManager.Group.Execute(dview.Page, dview);
        }

        /// <summary>
        /// Handles the Click event of the ungroup menu item.
        /// </summary>
        /// <param name="sender">The diagram view.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void G2_Click(object sender, RoutedEventArgs e)
        {
            DiagramCommandManager.Ungroup.Execute(dview.Page, dview);
        }

        /// <summary>
        ///  Provides class handling for the MouseDoubleClick routed event that occurs when 
        ///  the mouse left button is clicked twice in succession.
        /// </summary>
        /// <param name="e">The MouseButtonEventArgs.</param>
        protected override void OnMouseDoubleClick(MouseButtonEventArgs e)
        {
            base.OnMouseDoubleClick(e);
            if (dview.IsPageEditable)
            {
                isdoubleclicked = true;

                ConnRoutedEventArgs newEventArgs;
                if (HeadNode != null && TailNode != null)
                {
                    newEventArgs = new ConnRoutedEventArgs(this.HeadNode as Node, this.TailNode as Node, this);
                }
                else if (HeadNode == null && TailNode != null)
                {
                    newEventArgs = new ConnRoutedEventArgs(this.TailNode as Node, this);
                }
                else if (HeadNode != null && TailNode == null)
                {
                    newEventArgs = new ConnRoutedEventArgs(this, this.HeadNode as Node);
                }
                else
                {
                    newEventArgs = new ConnRoutedEventArgs(this);
                }

                newEventArgs.RoutedEvent = DiagramView.ConnectorDoubleClickEvent;
                RaiseEvent(newEventArgs);
                if (IsLabelEditable)
                {
                    editor.LabelEditStartInternal(editor);
                }
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if ((Keyboard.Modifiers & (ModifierKeys.Shift | ModifierKeys.Control)) == (ModifierKeys.Shift | ModifierKeys.Control)
                        && ((this.ConnectorType == ConnectorType.Orthogonal) || (this.ConnectorType == ConnectorType.Straight))
                        )
            {
                if (BrowserInteropHelper.IsBrowserHosted)
                {
                    this.Cursor = System.Windows.Input.Cursors.Pen;
                }
                else
                {
                    Assembly ass = Assembly.GetExecutingAssembly();
                    System.IO.Stream stream = ass.GetManifestResourceStream("Syncfusion.Windows.Diagram.Icons.InsertVertex.cur");
                    this.Cursor = new Cursor(stream);
                }
            }
            else
                this.Cursor = Cursors.Arrow;
        }
        /// <summary>
        /// Provides class handling for the MouseLeftButtonUp routed event that occurs when
        /// the mouse left button is released over this control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The MouseButtonEventArgs.</param>
        private void Lineconnector_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

            if (dc.View.IsPageEditable && !this.isdoubleclicked && this.isClicked)
            {
                ConnectorRoutedEventArgs NewEventArgs = new ConnectorRoutedEventArgs(this);
                NewEventArgs.RoutedEvent = DiagramView.ConnectorClickEvent;
                RaiseEvent(NewEventArgs);
            }

            bool notselected = true;
            if (dc.View.IsPageEditable && this.IsGrouped)
            {
                if (!dc.View.IsPanEnabled && !isdoubleclicked)
                {
                    IDiagramPage m_diagramPage = VisualTreeHelper.GetParent(this) as IDiagramPage;
                    //// update selection
                    if (m_diagramPage != null)
                    {
                        if ((Keyboard.Modifiers & (ModifierKeys.Shift | ModifierKeys.Control)) != ModifierKeys.None)
                        {
                            if (this.IsSelected)
                            {
                                m_diagramPage.SelectionList.Remove(this);
                            }
                            else
                            {
                                if (!this.IsSelected)
                                {
                                    foreach (Node gnode in this.Groups)
                                    {
                                        if (!gnode.IsSelected)
                                        {
                                            notselected = true;
                                        }
                                        else
                                        {
                                            notselected = false;
                                            break;
                                        }
                                    }

                                    if (notselected)
                                    {
                                        if (!this.IsGrouped)
                                        {
                                            m_diagramPage.SelectionList.Add(this);
                                        }
                                        else
                                        {
                                            m_diagramPage.SelectionList.Add(this.Groups[this.Groups.Count - 1]);
                                        }
                                    }
                                    else if (this.IsGrouped)
                                    {
                                        CollectionExt groupednodes = new CollectionExt();
                                        foreach (Group g in this.Groups)
                                        {
                                            groupednodes.Add(g);
                                        }

                                        groupednodes.Insert(0, this);
                                        foreach (ICommon gnode in groupednodes)
                                        {
                                            if (gnode.IsSelected)
                                            {
                                                if (groupednodes.Count > 1)
                                                {
                                                    int index = groupednodes.IndexOf(gnode);
                                                    if (index == 0)
                                                    {
                                                        m_diagramPage.SelectionList.Add(groupednodes[groupednodes.Count - 1]);
                                                    }
                                                    else
                                                    {
                                                        m_diagramPage.SelectionList.Add(groupednodes[index - 1]);
                                                    }

                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }
                                else if (this.IsGrouped)
                                {
                                    m_diagramPage.SelectionList.Add(this.Groups[this.Groups.Count - 1]);
                                }
                            }
                        }
                        else
                        {
                            if (!this.IsSelected)
                            {
                                foreach (Node gnode in this.Groups)
                                {
                                    if (!gnode.IsSelected)
                                    {
                                        notselected = true;
                                    }
                                    else
                                    {
                                        notselected = false;
                                        break;
                                    }
                                }

                                if (notselected)
                                {
                                    if (!this.IsGrouped)
                                    {
                                        m_diagramPage.SelectionList.Select(this);
                                    }
                                    else
                                    {
                                        m_diagramPage.SelectionList.Select(this.Groups[this.Groups.Count - 1]);
                                    }
                                }
                                else if (this.IsGrouped)
                                {
                                    CollectionExt groupednodes = new CollectionExt();
                                    foreach (Group g in this.Groups)
                                    {
                                        groupednodes.Add(g);
                                    }

                                    groupednodes.Insert(0, this);

                                    foreach (INodeGroup gnode in groupednodes)
                                    {
                                        if ((gnode as ICommon).IsSelected)
                                        {
                                            if (groupednodes.Count > 1)
                                            {
                                                int index = groupednodes.IndexOf(gnode);
                                                if (index == 0)
                                                {
                                                    m_diagramPage.SelectionList.Select(groupednodes[groupednodes.Count - 1]);
                                                }
                                                else
                                                {
                                                    m_diagramPage.SelectionList.Select(groupednodes[index - 1]);
                                                }

                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                            else if (this.IsGrouped)
                            {
                                m_diagramPage.SelectionList.Select(this.Groups[this.Groups.Count - 1]);
                            }
                        }

                        (m_diagramPage as DiagramPage).Focus();
                    }
                }

                ////e.Handled = true;
            }



            isdoubleclicked = false;
            ////base.OnMouseUp(e);
        }

        /// <summary>
        /// Provides class handling for the MouseDown routed event that occurs when the mouse 
        /// button is pressed while the mouse pointer is over this control.
        /// </summary>
        /// <param name="e">The MouseButtonEventArgs</param>
        /// 

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            Visibility adornerVisibility;
            if (LineAdorner != null)
            {
                adornerVisibility = LineAdorner.Visibility;
            }
            else
            {
                adornerVisibility = Visibility.Collapsed;
            }

            if (dc.View.IsPageEditable && !this.IsGrouped)
            {
                ////base.OnPreviewMouseLeftButtonDown(e);

                IDiagramPage diagramPanel = VisualTreeHelper.GetParent(this) as IDiagramPage;

                ////diagramPanel.SelectionList.Clear();
                if (diagramPanel != null)
                {
                    if ((Keyboard.Modifiers & (ModifierKeys.Shift | ModifierKeys.Control)) == (ModifierKeys.Shift | ModifierKeys.Control)
                        && ((this.ConnectorType == ConnectorType.Orthogonal) || (this.ConnectorType == ConnectorType.Straight))
                        )
                    {
                        Point t = e.GetPosition(this);// MeasureUnitsConverter.FromPixels(e.GetPosition(this), MeasurementUnit);
                        InsertIntermediatePoint(t);
                        UpdateConnectorPathGeometry();
                        if (LineAdorner == null)
                        {
                            ShowAdorner();
                            HideAdorner();
                        }
                        diagramPanel.SelectionList.Clear();
                        diagramPanel.SelectionList.Add(this);
                        (LineAdorner as LineConnectorAdorner).InvalidateVertexs();
                    }

                    else if ((Keyboard.Modifiers & (ModifierKeys.Shift | ModifierKeys.Control)) != ModifierKeys.None)
                    {
                        if (this.IsSelected)
                        {
                            diagramPanel.SelectionList.Remove(this);
                        }
                        else
                        {
                            diagramPanel.SelectionList.Add(this);
                        }
                    }
                    else if (!this.IsSelected)
                    {
                        diagramPanel.SelectionList.Clear();
                        diagramPanel.SelectionList.Add(this);
                    }

                    (diagramPanel as DiagramPage).Focus();
                    if (dc.View.IsPageEditable && !this.isdoubleclicked && adornerVisibility.Equals(Visibility.Collapsed))
                    {
                        ConnectorRoutedEventArgs NewEventArgs = new ConnectorRoutedEventArgs(this);
                        NewEventArgs.RoutedEvent = DiagramView.ConnectorClickEvent;
                        RaiseEvent(NewEventArgs);
                        this.isClicked = false;
                    }
                    else if (dc.View.IsPageEditable && !this.isdoubleclicked && !adornerVisibility.Equals(Visibility.Collapsed))
                    {
                        this.isClicked = true;
                    }
                }
            }

            e.Handled = true;
        }

        internal void InsertIntermediatePoint(Point point)
        {
            double three = 3.0;// MeasureUnitsConverter.FromPixels(3.0, this.MeasurementUnit);
            if (IntermediatePoints == null)
            {
                IntermediatePoints = new List<Point>();
            }
            List<Point> points = new List<Point>();
            if (HeadNode != null)
            {
                NodeInfo ni;
                Rect sourceRect = getNodeRect(HeadNode as Node);
                ni = getRectAsNodeInfo(sourceRect);
                if (dc != null && dc.View != null && dc.View.Page != null)
                {
                    if (this.ConnectionHeadPort != null)
                    {
                        ni.Position = new Point(this.ConnectionHeadPort.CenterPosition.X, this.ConnectionHeadPort.CenterPosition.Y);
                        ni.Position = (HeadNode as Node).TranslatePoint(ni.Position, dc.View.Page);
                        ni.Position = ni.Position;// MeasureUnitsConverter.FromPixels(ni.Position, MeasurementUnit);
                    }
                }

                points.Add(ni.Position);
            }
            else
            {
                points.Add(this.PxStartPointPosition);
            }
            points.AddRange(IntermediatePoints);
            if (TailNode != null)
            {
                NodeInfo ni;
                Rect targetRect = getNodeRect(TailNode as Node);
                ni = getRectAsNodeInfo(targetRect);

                if (dc != null && dc.View != null && dc.View.Page != null)
                {
                    if (this.ConnectionTailPort != null)
                    {
                        ni.Position = new Point(this.ConnectionTailPort.CenterPosition.X, this.ConnectionTailPort.CenterPosition.Y);
                        ni.Position = (TailNode as Node).TranslatePoint(ni.Position, dc.View.Page);
                        ni.Position = ni.Position;// MeasureUnitsConverter.FromPixels(ni.Position, MeasurementUnit);
                    }
                    points.Add(ni.Position);
                }
            }
            else
            {
                points.Add(this.PxEndPointPosition);
            }

            List<double> Diff = new List<double>();
            for (int i = 0; i < points.Count - 1; i++)
            {
                Point st = points[i];
                Point en = points[i + 1];
                double delx = Math.Abs(st.X - en.X);
                double dely = Math.Abs(st.Y - en.Y);
                double lhs = ((point.Y - st.Y) / (en.Y - st.Y));
                double rhs = ((point.X - st.X) / (en.X - st.X));
                if (double.IsInfinity(lhs) || double.IsInfinity(rhs) || double.IsNaN(lhs) || double.IsNaN(rhs))
                {
                    if (st.X == en.X)
                    {
                        if (st.Y == en.Y)
                        {
                            Diff.Add(10000d);
                        }
                        else if (((st.Y > point.Y) && (point.Y > en.Y)) || ((st.Y < point.Y) && (point.Y < en.Y)))
                        {
                            Diff.Add(Math.Abs(st.X - point.X));
                        }
                        else
                        {
                            Diff.Add(10000d);
                        }
                    }
                    else if (st.Y == en.Y)
                    {
                        if (((st.X > point.X) && (point.X > en.X)) || ((st.X < point.X) && (point.X < en.X)))
                        {
                            Diff.Add(Math.Abs(st.Y - point.Y));
                        }
                        else
                        {
                            Diff.Add(10000d);
                        }
                    }
                    else
                    {
                        Diff.Add(10000d);
                    }
                }
                else if (ConnectorType != ConnectorType.Orthogonal)
                {
                    if ((st.X >= point.X && point.X >= en.X) || (st.X <= point.X && point.X <= en.X) || delx < three)
                    {
                        if ((st.Y >= point.Y && point.Y >= en.Y) || (st.Y <= point.Y && point.Y <= en.Y) || dely < three)
                        {
                            Diff.Add(Math.Abs(lhs - rhs));
                        }
                        else
                        {
                            Diff.Add(10000d);
                        }
                    }
                    else
                    {
                        Diff.Add(10000d);
                    }
                }
            }
            double selected = 10000;
            int selectedIndex = 0;
            for (int i = 0; i < Diff.Count - 1; i++)
            {
                double temp = Math.Min(Diff[i], Diff[i + 1]);
                if (temp < selected)
                {
                    selected = temp;
                    if (selected == Diff[i])
                        selectedIndex = i;
                    else
                        selectedIndex = i + 1;
                }
            }

            if (IntermediatePoints.Count < selectedIndex)
            {
                selectedIndex = IntermediatePoints.Count;
            }
            if (this.ConnectorType == ConnectorType.Orthogonal)
            {
                IntermediatePoints.Insert(selectedIndex, point);
            }
            IntermediatePoints.Insert(selectedIndex, point);
            if (ConnectorType == ConnectorType.Orthogonal)
            {
                if (IntermediatePoints.Count > 2)
                {
                    if (selectedIndex != 0)
                    {
                        if (Math.Abs(IntermediatePoints[selectedIndex].X - IntermediatePoints[selectedIndex - 1].X)
                            > Math.Abs(IntermediatePoints[selectedIndex].Y - IntermediatePoints[selectedIndex - 1].Y))
                        {
                            IntermediatePoints[selectedIndex] = new Point(IntermediatePoints[selectedIndex].X, IntermediatePoints[selectedIndex - 1].Y);
                            IntermediatePoints[selectedIndex + 1] = new Point(IntermediatePoints[selectedIndex + 1].X, IntermediatePoints[selectedIndex - 1].Y);
                        }
                        else
                        {
                            IntermediatePoints[selectedIndex] = new Point(IntermediatePoints[selectedIndex - 1].X, IntermediatePoints[selectedIndex].Y);
                            IntermediatePoints[selectedIndex + 1] = new Point(IntermediatePoints[selectedIndex - 1].X, IntermediatePoints[selectedIndex + 1].Y);
                        }
                    }
                    else
                    {
                        if (Math.Abs(IntermediatePoints[selectedIndex].X - IntermediatePoints[selectedIndex + 2].X)
                            > Math.Abs(IntermediatePoints[selectedIndex].Y - IntermediatePoints[selectedIndex + 2].Y))
                        {
                            IntermediatePoints[selectedIndex] = new Point(IntermediatePoints[selectedIndex].X, IntermediatePoints[selectedIndex + 2].Y);
                            IntermediatePoints[selectedIndex + 1] = new Point(IntermediatePoints[selectedIndex + 1].X, IntermediatePoints[selectedIndex + 2].Y);
                        }
                        else
                        {
                            IntermediatePoints[selectedIndex] = new Point(IntermediatePoints[selectedIndex + 2].X, IntermediatePoints[selectedIndex].Y);
                            IntermediatePoints[selectedIndex + 1] = new Point(IntermediatePoints[selectedIndex + 2].X, IntermediatePoints[selectedIndex + 1].Y);
                        }
                    }
                }
            }
            if (ConnectorType == ConnectorType.Orthogonal || ConnectorType == ConnectorType.Straight)
            {
                DoStackOperationForNewPoint(selectedIndex);
            }
        }

        private void DoStackOperationForNewPoint(int selectedIndex)
        {
            if (dc != null && dc.View != null && dc.View.UndoRedoEnabled)
            {
                if (!(dc.View.undo || dc.View.redo))
                {
                    dc.View.RedoStack.Clear();
                }
                dc.View.UndoStack.Push(selectedIndex);
                dc.View.UndoStack.Push(this);
                dc.View.UndoStack.Push(IntermediatePoints[selectedIndex]);//MeasureUnitsConverter.ToPixels(IntermediatePoints[selectedIndex], this.MeasurementUnit));
                dc.View.UndoStack.Push("Added");
            }
        }

        /// <summary>
        /// Shows the adorner
        /// </summary>
        protected override void ShowAdorner()
        {
            if (VertexStyle == null)
                VertexStyle = this.FindResource("ConnectorAdornerVertexStyle") as Style;
            if (this.LineAdorner == null)
            {
                IDiagramPage mPage = VisualTreeHelper.GetParent(this) as IDiagramPage;

                AdornerLayer adorner = AdornerLayer.GetAdornerLayer(this);
                if (adorner != null)
                {
                    this.LineAdorner = new LineConnectorAdorner(mPage, this);
                    adorner.Add(this.LineAdorner);
                }
            }
            else
            {
                this.LineAdorner.Visibility = Visibility.Visible;
            }
        }

        /// <summary>
        /// Hides the adorner.
        /// </summary>
        protected override void HideAdorner()
        {
            if (this.LineAdorner != null)
            {
                this.LineAdorner.Visibility = Visibility.Collapsed;
            }
        }

        /// <summary>
        /// Calls Line_PropertyChanged method of the instance, notifies of the dependency property value changes.
        /// </summary>
        /// <param name="sender">object, the change occurs on.</param>
        /// <param name="e">Property change details, such as old value and new value.</param>
        protected override void Line_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("Position"))
            {
                UpdateConnectorPathGeometry();
            }
        }


        protected virtual void OnConnectorPathGeometryUpdated(EventArgs e)
        {
            if (this.ConnectorPathGeometryUpdated != null)
            {
                this.ConnectorPathGeometryUpdated(this, e);
            }
        }

        public event EventHandler<EventArgs> ConnectorPathGeometryUpdated;

        /// <summary>
        /// Called whenever the head node, tail node or position of the node is changed. 
        /// </summary>
        /// 
        private bool _updateConnectorPathGeometry = false;
        public override void UpdateConnectorPathGeometry()
        {
            if (!this.IsLoaded || (this.dc != null && this.dc.IsLoadingFromFile))
            {
                return;
            }



            if(!_updateConnectorPathGeometry)
            {
                _updateConnectorPathGeometry = true;
                //if (this.MeasurementUnit != currentMUSelected)
                //{
                //    doRunTimeUnitChange();
                //}
                if (isdefaulted)
                {
                    this.LabelWidth = Distance;
                }

                if (dview == null)
                {
                    dview = Node.GetDiagramView(this);
                }
                Point startpos = this.PxStartPointPosition;
                Point endpos = this.PxEndPointPosition;
                bool isLeft = false;
                bool isRight = false;
                bool isTop = false;
                bool isBottom = false;
                bool tisLeft = false;
                bool tisRight = false;
                bool tisTop = false;
                bool tisBottom = false;
                Point s = new Point(0, 0);
                Point e = new Point(0, 0);
                Point sp = new Point(0, 0);
                Point ep = new Point(0, 0);
                double x1 = 0;
                double y1 = 0;
                double x2 = 0;
                double y2 = 0;
                Point startPoint = new Point(0, 0);
                Point endPoint = new Point(0, 0);
                Rect sourceRect = new Rect();
                Rect targetRect = new Rect();
                NodeInfo source = new NodeInfo();
                NodeInfo target = new NodeInfo();
                double cs = 0;
                double twozero = 20;// MeasureUnitsConverter.FromPixels(20, DiagramPage.Munits);
                double sourceleft = PxStartPointPosition.X;// MeasureUnitsConverter.FromPixels(StartPointPosition.X, DiagramPage.Munits);
                double sourcetop = PxStartPointPosition.Y;// MeasureUnitsConverter.FromPixels(StartPointPosition.Y, DiagramPage.Munits);
                double targetleft = PxEndPointPosition.X;// MeasureUnitsConverter.FromPixels(EndPointPosition.X, DiagramPage.Munits);
                double targettop = PxEndPointPosition.Y;// MeasureUnitsConverter.FromPixels(EndPointPosition.Y, DiagramPage.Munits);
                double dropcentreX = DropPoint.X;// MeasureUnitsConverter.FromPixels(DropPoint.X, DiagramPage.Munits);
                double dropcentreY = DropPoint.Y;// MeasureUnitsConverter.FromPixels(DropPoint.Y, DiagramPage.Munits);
                double twofive = 25;// MeasureUnitsConverter.FromPixels(25, DiagramPage.Munits);
                if (HeadNode != null)
                {
                    (HeadNode as Node).refreshBoundaries();
                    cs = PxConnectionEndSpace;// MeasureUnitsConverter.FromPixels(this.ConnectionEndSpace, this.MeasurementUnit);
                    sourceRect = getNodeRect(HeadNode as Node);
                    source = getRectAsNodeInfo(sourceRect);
                    if (sourceRect != Rect.Empty)
                    {
                        sourceRect = new Rect(source.Left, source.Top, source.Size.Width, source.Size.Height);
                    }
                    if (sourceRect == Rect.Empty)
                    {
                        source = (HeadNode as Node).GetInfo();
                        sourceRect = new Rect(
                                            source.Left,
                                            source.Top,
                                            source.Size.Width,
                                            source.Size.Height);
                        sourceRect.Inflate(cs, cs);
                    }

                    startPoint = new Point(source.Position.X, source.Position.Y);
                }
                else
                {
                    startPoint = new Point(PxStartPointPosition.X, PxStartPointPosition.Y);
                    s = new Point(PxStartPointPosition.X, PxStartPointPosition.Y);
                    sourceRect = new Rect(
                                        sourceleft,
                                        sourcetop,
                                        twozero,
                                        twozero);
                    source.Position = new Point(PxStartPointPosition.X + twofive, PxStartPointPosition.Y + twofive);
                }

                if (TailNode != null)
                {
                    cs = PxConnectionEndSpace;// //MeasureUnitsConverter.FromPixels(this.ConnectionEndSpace, this.MeasurementUnit);
                    (TailNode as Node).refreshBoundaries();
                    targetRect = getNodeRect(TailNode as Node);
                    target = getRectAsNodeInfo(targetRect);
                    if (targetRect != Rect.Empty)
                    {
                        targetRect = new Rect(target.Left, target.Top, target.Size.Width, target.Size.Height);
                    }
                    if (targetRect == Rect.Empty)
                    {
                        target = (TailNode as Node).GetInfo();
                        targetRect = new Rect(
                                              target.Left,
                                              target.Top,
                                              target.Size.Width,
                                              target.Size.Height);
                        targetRect.Inflate(cs, cs);
                    }
                    endPoint = new Point(target.Position.X, target.Position.Y);

                }
                else
                {
                    e = new Point(PxEndPointPosition.X, PxEndPointPosition.Y);
                    endPoint = new Point(PxEndPointPosition.X, PxEndPointPosition.Y);
                    targetRect = new Rect(
                                          targetleft,
                                          targettop,
                                          twozero,
                                          twozero);
                    target.Position = new Point(PxEndPointPosition.X - twofive, PxEndPointPosition.Y - twofive);
                }

                if (this.ConnectorType == ConnectorType.Bezier)
                {
                    #region Bezier
                    try
                    {
                        if (HeadNode != null || TailNode != null)
                        {
                            sp = startPoint;// MeasureUnitsConverter.FromPixels(startPoint, source.MeasurementUnit);
                            ep = endPoint;// MeasureUnitsConverter.FromPixels(endPoint, source.MeasurementUnit);

                            if (!(Orientation == TreeOrientation.LeftRight || Orientation == TreeOrientation.RightLeft))
                            {
                                ConnectorBase.GetOrthogonalLineIntersect(source, target, sourceRect, targetRect, out isTop, out isBottom, out isLeft, out isRight, out tisTop, out tisBottom, out tisLeft, out tisRight, out s, out e);
                                extendPoints(isTop, isBottom, isLeft, isRight, tisTop, tisBottom, tisLeft, tisRight, out s, out e, s, e);
                            }
                            else
                            {
                                ConnectorBase.GetTreeOrthogonalLineIntersect(source, target, sourceRect, targetRect, out isTop, out isBottom, out isLeft, out isRight, out tisTop, out tisBottom, out tisLeft, out tisRight, out s, out e);
                                extendPoints(isTop, isBottom, isLeft, isRight, tisTop, tisBottom, tisLeft, tisRight, out s, out e, s, e);
                            }

                        }

                        if (HeadNode != null)
                        {
                            sp = startPoint;// MeasureUnitsConverter.FromPixels(startPoint, source.MeasurementUnit);

                            if (ConnectionHeadPort != null)
                            {
                                s = new Point(ConnectionHeadPort.CenterPosition.X, ConnectionHeadPort.CenterPosition.Y);
                                s = (HeadNode as Node).TranslatePoint(s, dview.Page);
                                //s = MeasureUnitsConverter.FromPixels(s, MeasurementUnit);
                            }
                        }

                        if (TailNode != null)
                        {
                            ep = endPoint;// MeasureUnitsConverter.FromPixels(endPoint, source.MeasurementUnit);
                            if (ConnectionTailPort != null)
                            {
                                e = new Point(ConnectionTailPort.CenterPosition.X, ConnectionTailPort.CenterPosition.Y);
                                e = (TailNode as Node).TranslatePoint(e, dview.Page);
                                //e = MeasureUnitsConverter.FromPixels(e, MeasurementUnit);
                            }
                        }

                        if (HeadNode == null)
                        {
                            s = new Point(PxStartPointPosition.X, PxStartPointPosition.Y);
                        }

                        if (TailNode == null)
                        {
                            e = new Point(PxEndPointPosition.X, PxEndPointPosition.Y);
                        }

                        if (this.LineAdorner != null)
                        {
                            if ((this.LineAdorner as LineConnectorAdorner).ConnectorPoint)
                            {
                                (this.LineAdorner as LineConnectorAdorner).ConnectorPoint = false;
                                if ((this.LineAdorner as LineConnectorAdorner).IsHeadThumb)
                                {
                                    s = Mouse.GetPosition(this);// MeasureUnitsConverter.FromPixels(Mouse.GetPosition(this), this.MeasurementUnit);
                                    HeadNode = null;
                                }
                                else if ((this.LineAdorner as LineConnectorAdorner).IsTailThumb)
                                {
                                    e = Mouse.GetPosition(this);// MeasureUnitsConverter.FromPixels(Mouse.GetPosition(this), this.MeasurementUnit);
                                    TailNode = null;
                                }
                            }
                        }
                        if (this.HeadNode != null)
                        {
                            if (this.ConnectionHeadPort != null && HeadNode != null)
                            {
                                if (dview != null && dview.Page != null)
                                {
                                    s = new Point(ConnectionHeadPort.CenterPosition.X, ConnectionHeadPort.CenterPosition.Y);
                                    s = (HeadNode as Node).TranslatePoint(s, dview.Page);
                                    //s = s;// MeasureUnitsConverter.FromPixels(s, this.MeasurementUnit);
                                }
                            }
                            else
                            {
                                s = GetIntersectionPoint(s, (HeadNode as Node));
                            }
                        }
                        if (this.TailNode != null)
                        {

                            if (this.ConnectionTailPort != null && TailNode != null)
                            {
                                if (dview != null && dview.Page != null)
                                {
                                    e = new Point(ConnectionTailPort.CenterPosition.X, ConnectionTailPort.CenterPosition.Y);
                                    e = (TailNode as Node).TranslatePoint(e, dview.Page);
                                    //e = e;// MeasureUnitsConverter.FromPixels(e, this.MeasurementUnit);
                                }
                            }
                            else
                            {
                                e = GetIntersectionPoint(e, (TailNode as Node));
                            }
                        }
                        if ((this.HeadNode as Node) != null && (this.TailNode as Node) != null)
                        {
                            if(!dc.View.Page.Children.Contains(this.HeadNode as Node)&&dc.View.Page.Children.Contains(this.TailNode as Node))
                            {
                                s = new Point((this.HeadNode as Node).OffsetX, (this.HeadNode as Node).OffsetY); //GetIntersectionPoint(new Point((this.HeadNode as Node).OffsetX, (this.HeadNode as Node).OffsetY), (this.TailNode as Node));
                            }
                            if (dc.View.Page.Children.Contains(this.HeadNode as Node) && !dc.View.Page.Children.Contains(this.TailNode as Node))
                            {
                                e =  (new Point((this.TailNode as Node).OffsetX, (this.TailNode as Node).OffsetY)); //GetIntersectionPoint(new Point((this.TailNode as Node).OffsetX, (this.TailNode as Node).OffsetY), (this.HeadNode as Node));

                            }

                        }

                        x1 = s.X;// MeasureUnitsConverter.ToPixels(s.X, this.MeasurementUnit);
                        y1 = s.Y;// MeasureUnitsConverter.ToPixels(s.Y, this.MeasurementUnit);
                        x2 = e.X;// MeasureUnitsConverter.ToPixels(e.X, this.MeasurementUnit);
                        y2 = e.Y;// MeasureUnitsConverter.ToPixels(e.Y, this.MeasurementUnit);
                        PathGeometry pathgeometry = new PathGeometry();
                        PathFigure pathfigure = new PathFigure();
                        double num = 150.0;
                        Point t = new Point(x1, y1);
                        pathfigure.StartPoint = t;
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

                        double x = Math.Pow((x1 - x2), 2);
                        double y = Math.Pow((y1 - y2), 2);
                        Distance = Math.Sqrt(x + y);
                        pathfigure.Segments.Add(segment);
                        pathgeometry.Figures.Add(pathfigure);

                        if (HeadNode != null && TailNode != null)
                        {
                            m_isoverlapped = false;
                            this.TailDecoratorShape = this.InternalTailShape;
                            this.HeadDecoratorShape = this.InternalHeadShape;
                        }
                        if (this.ConnectorPathGeometry != null)
                        {
                            this.ConnectorPathGeometry.Figures.Clear();
                            this.ConnectorPathGeometry.Figures.Add(pathfigure);
                            this.UpdateDecoratorPosition();
                        }
                        else
                        {
                            this.ConnectorPathGeometry = pathgeometry;
                        }
                        this.VirtualConnectorPathGeometry = pathgeometry.Clone();
                        this.WidenedPathGeometry = pathgeometry.GetWidenedPathGeometry(new Pen(Brushes.Black, 1));
                        invalidateBridging = true;
                    }
                    catch
                    {
                    }
                    #endregion Bezier
                }

                else
                {
                    {
                        List<Point> connectionPoints = new List<Point>();
                        PathGeometry pathgeometry = new PathGeometry();
                        //if (ConnectorType != ConnectorType.Orthogonal && IntermediatePoints != null && IntermediatePoints.Count != 2)
                        {
                            connectionPoints = GetTerminalPoints();
                        }
                        if (IntermediatePoints == null)
                        {
                            IntermediatePoints = new List<Point>();
                        }
                        if (connectionPoints.Count > 0)
                        {
                            if (this.ConnectorType == ConnectorType.Orthogonal)
                            {
                                connectionPoints = meetOrhogonalConstraints(connectionPoints, cs);
                            }
                            m_isoverlapped = false;
                            PathFigure pathfigure = new PathFigure();
                            if (IntermediatePoints.Count >= 1)
                            {
                                for (int i = IntermediatePoints.Count - 1; i >= 0; i--)
                                {
                                    connectionPoints.Insert(1, IntermediatePoints[i]);
                                }
                            }
                            for (int i = 0; i < connectionPoints.Count; i++)
                            {
                                //connectionPoints[i] = MeasureUnitsConverter.ToPixels(connectionPoints[i], this.MeasurementUnit);
                            }
                            if (this.ConnectorType == ConnectorType.Orthogonal)
                                adjustIntermediatePoints(connectionPoints);
                            //findDistance(connectionPoints);
                            pathfigure.StartPoint = connectionPoints[0];
                            connectionPoints.Remove(connectionPoints[0]);
                            foreach (Point p in connectionPoints)
                            {
                                pathfigure.Segments.Add(new LineSegment(p, true));
                            }
                            //pathfigure.Segments.Add(new PolyLineSegment(connectionPoints, true));
                            pathgeometry.Figures.Add(pathfigure);
                            //this.ConnectorPathGeometry = pathgeometry;
                            //this.VirtualConnectorPathGeometry = pathgeometry.Clone();
                            if (this.ConnectorPathGeometry != null)
                            {
                                this.ConnectorPathGeometry.Figures.Clear();
                                this.ConnectorPathGeometry.Figures.Add(pathfigure);
                                //this.UpdateDecoratorPosition();
                            }
                            else
                            {
                                this.ConnectorPathGeometry = pathgeometry;
                            }
                            this.VirtualConnectorPathGeometry = pathgeometry.Clone();
                            this.UpdateDecoratorPosition();
                            this.WidenedPathGeometry = pathgeometry.GetWidenedPathGeometry(new Pen(Brushes.Black, 1));

                            setMinMax();
                            invalidateBridging = true;
                            //{
                            //    //if (disp != null)
                            //    //{
                            //    //    disp.Abort();
                            //    //}
                            //    //object o = this.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.SystemIdle,
                            //    //    new Initialize(this.SetLineBridging));
                            //    disp = this.Dispatcher.BeginInvoke(
                            //        System.Windows.Threading.DispatcherPriority.SystemIdle,
                            //        new Initialize(this.Thread));
                            //}
                            //if (ConnectorType != ConnectorType.Bezier)
                            //{
                            //    SetLineBridging();
                            //}
                        }
                    }

                    #region dump
                    //if (HeadNode != null || TailNode != null)
                    //{
                    //    List<Point> connectionPoints = new List<Point>();

                    //    PathGeometry pathgeometry = new PathGeometry();
                    //    connectionPoints = GetTerminalPoints();
                    //    if (connectionPoints.Count > 0)
                    //    {
                    //        if (this.ConnectorType == ConnectorType.Orthogonal)
                    //        {
                    //            connectionPoints = meetOrhogonalConstraints(connectionPoints, cs);
                    //        }
                    //        m_isoverlapped = false;
                    //        PathFigure pathfigure = new PathFigure();

                    //        if (ConnectorType == ConnectorType.Orthogonal && (IntermediatePoints == null || IntermediatePoints.Count == 0))
                    //        {
                    //            IntermediatePoints = new List<Point>();

                    //            IntermediatePoints.Add(new Point(connectionPoints[0].X + 10, connectionPoints[0].Y));
                    //            IntermediatePoints.Add(new Point(IntermediatePoints[0].X, connectionPoints[1].Y));
                    //        }
                    //        else if (IntermediatePoints != null)
                    //        {
                    //            for (int i = IntermediatePoints.Count - 1; i >= 0; i--)
                    //            {
                    //                connectionPoints.Insert(1, IntermediatePoints[i]);
                    //            }
                    //        }
                    //        for (int i = 0; i < connectionPoints.Count; i++)
                    //        {
                    //            connectionPoints[i] = MeasureUnitsConverter.ToPixels(connectionPoints[i], this.MeasurementUnit);
                    //        }
                    //        if (this.ConnectorType == ConnectorType.Orthogonal)
                    //            adjustIntermediatePoints(connectionPoints);
                    //        findDistance(connectionPoints);
                    //        pathfigure.StartPoint = connectionPoints[0];
                    //        connectionPoints.Remove(connectionPoints[0]);
                    //        pathfigure.Segments.Add(new PolyLineSegment(connectionPoints, true));
                    //        pathgeometry.Figures.Add(pathfigure);
                    //        this.ConnectorPathGeometry = pathgeometry;
                    //    }

                    //}
                    //else
                    //{
                    //    PathGeometry pathgeometry = new PathGeometry();
                    //    List<Point> connectionPoints = GetTerminalPoints();
                    //    if (connectionPoints.Count > 0)
                    //    {
                    //        PathFigure pathfigure = new PathFigure();
                    //        if (IntermediatePoints == null || IntermediatePoints.Count == 0)
                    //        {
                    //            IntermediatePoints = new List<Point>();
                    //            if (this.ConnectorType == ConnectorType.Orthogonal)
                    //            {
                    //                IntermediatePoints.Add(new Point(connectionPoints[0].X, connectionPoints[0].Y + MeasureUnitsConverter.FromPixels(20, this.MeasurementUnit)));
                    //                IntermediatePoints.Add(new Point(connectionPoints[1].X, IntermediatePoints[0].Y));
                    //                for (int i = IntermediatePoints.Count - 1; i >= 0; i--)
                    //                {
                    //                    connectionPoints.Insert(1, IntermediatePoints[i]);
                    //                }
                    //            }
                    //        }
                    //        else
                    //        {
                    //            for (int i = IntermediatePoints.Count - 1; i >= 0; i--)
                    //            {
                    //                connectionPoints.Insert(1, IntermediatePoints[i]);
                    //            }
                    //            if (ConnectorType == ConnectorType.Orthogonal)
                    //            {
                    //                if (LineAdorner != null)
                    //                    (this.LineAdorner as LineConnectorAdorner).InvalidateVertexs();
                    //            }
                    //        }
                    //        if (this.ConnectorType == ConnectorType.Orthogonal && IntermediatePoints.Count == 2)
                    //        {
                    //            defaultStyle();
                    //        }
                    //        for (int i = 0; i < connectionPoints.Count; i++)
                    //        {
                    //            connectionPoints[i] = MeasureUnitsConverter.ToPixels(connectionPoints[i], this.MeasurementUnit);
                    //        }

                    //        if (this.ConnectorType == ConnectorType.Orthogonal)
                    //            adjustIntermediatePoints(connectionPoints);
                    //        findDistance(connectionPoints);
                    //        pathfigure.StartPoint = connectionPoints[0];
                    //        connectionPoints.Remove(connectionPoints[0]);
                    //        pathfigure.Segments.Add(new PolyLineSegment(connectionPoints, true));
                    //        pathgeometry.Figures.Add(pathfigure);
                    //        this.ConnectorPathGeometry = pathgeometry;
                    //    }
                    //}
                    #endregion dump
                }
                if (LineAdorner != null && ConnectorType != ConnectorType.Bezier)
                {
                    //(this.LineAdorner as LineConnectorAdorner).InvalidateVertexs();
                    (this.LineAdorner as LineConnectorAdorner).UpdateVertexsPosition();
                }


                this.bridged = true;

                this.OnConnectorPathGeometryUpdated(EventArgs.Empty);

                this._updateConnectorPathGeometry = false;
            }

            
        }
        //public  void UpdateConnectorPath(LineConnector line)
        //{
        //    if (!this.IsLoaded || (this.dc != null && this.dc.IsLoadingFromFile))
        //    {
        //        return;
        //    }
        //    {
        //        if (this.MeasurementUnit != currentMUSelected)
        //        {
        //            doRunTimeUnitChange();
        //        }
        //        if (isdefaulted)
        //        {
        //            this.LabelWidth = Distance;
        //        }

        //        if (dview == null)
        //        {
        //            dview = Node.GetDiagramView(this);
        //        }
        //        Point startpos = this.PxStartPointPosition;
        //        Point endpos = this.PxEndPointPosition;
        //        bool isLeft = false;
        //        bool isRight = false;
        //        bool isTop = false;
        //        bool isBottom = false;
        //        bool tisLeft = false;
        //        bool tisRight = false;
        //        bool tisTop = false;
        //        bool tisBottom = false;
        //        Point s = new Point(0, 0);
        //        Point e = new Point(0, 0);
        //        Point sp = new Point(0, 0);
        //        Point ep = new Point(0, 0);
        //        double x1 = 0;
        //        double y1 = 0;
        //        double x2 = 0;
        //        double y2 = 0;
        //        Point startPoint = new Point(0, 0);
        //        Point endPoint = new Point(0, 0);
        //        Rect sourceRect = new Rect();
        //        Rect targetRect = new Rect();
        //        NodeInfo source = new NodeInfo();
        //        NodeInfo target = new NodeInfo();
        //        double cs = 0;
        //        double twozero = 20;// MeasureUnitsConverter.FromPixels(20, DiagramPage.Munits);
        //        double sourceleft = PxStartPointPosition.X;// MeasureUnitsConverter.FromPixels(StartPointPosition.X, DiagramPage.Munits);
        //        double sourcetop = PxStartPointPosition.Y;// MeasureUnitsConverter.FromPixels(StartPointPosition.Y, DiagramPage.Munits);
        //        double targetleft = PxEndPointPosition.X;// MeasureUnitsConverter.FromPixels(EndPointPosition.X, DiagramPage.Munits);
        //        double targettop = PxEndPointPosition.Y;// MeasureUnitsConverter.FromPixels(EndPointPosition.Y, DiagramPage.Munits);
        //        double dropcentreX = DropPoint.X;// MeasureUnitsConverter.FromPixels(DropPoint.X, DiagramPage.Munits);
        //        double dropcentreY = DropPoint.Y;// MeasureUnitsConverter.FromPixels(DropPoint.Y, DiagramPage.Munits);
        //        double twofive = 25;// MeasureUnitsConverter.FromPixels(25, DiagramPage.Munits);
        //        if ((HeadNode as Node).IsLoaded)
        //        {
        //            (HeadNode as Node).refreshBoundaries();
        //            cs = PxConnectionEndSpace;// MeasureUnitsConverter.FromPixels(this.ConnectionEndSpace, this.MeasurementUnit);
        //            sourceRect = getNodeRect(HeadNode as Node);
        //            source = getRectAsNodeInfo(sourceRect);
        //            if (sourceRect != Rect.Empty)
        //            {
        //                sourceRect = new Rect(source.Left, source.Top, source.Size.Width, source.Size.Height);
        //            }
        //            if (sourceRect == Rect.Empty)
        //            {
        //                source = (HeadNode as Node).GetInfo();
        //                sourceRect = new Rect(
        //                                    source.Left,
        //                                    source.Top,
        //                                    source.Size.Width,
        //                                    source.Size.Height);
        //                sourceRect.Inflate(cs, cs);
        //            }

        //            startPoint = new Point(source.Position.X, source.Position.Y);
        //        }
        //        else
        //        {
        //            startPoint = new Point((HeadNode as Node).OffsetX, (HeadNode as Node).OffsetY);
        //            s = startPoint;//new Point(PxStartPointPosition.X, PxStartPointPosition.Y);
        //            sourceRect = new Rect(
        //                                (HeadNode as Node).OffsetX,
        //                                (HeadNode as Node).OffsetY,
        //                                100,
        //                                100);
        //            source.Position = new Point(startPoint.X + twofive, startPoint.Y + twofive);
        //        }

        //        if ((TailNode as Node).IsLoaded)
        //        {
        //            //cs = PxConnectionEndSpace;// //MeasureUnitsConverter.FromPixels(this.ConnectionEndSpace, this.MeasurementUnit);
        //            //(TailNode as Node).refreshBoundaries();
        //            //targetRect = getNodeRect(TailNode as Node);
        //            //target = getRectAsNodeInfo(targetRect);
        //            //if (targetRect != Rect.Empty)
        //            //{
        //            //    targetRect = new Rect(400,400,500,5000);
        //            //}
        //            //if (targetRect == Rect.Empty)
        //            //{
        //            //    target = (TailNode as Node).GetInfo();
        //            //    targetRect = new Rect(400, 400, 500, 5000);
        //            //    targetRect.Inflate(cs, cs);
        //            //}
        //            //endPoint = new Point(500,500);

        //        }
        //        else
        //        {
        //            e = new Point((TailNode as Node).OffsetX, (TailNode as Node).OffsetY);//new Point(300,300);
        //            endPoint = e;//new Point(300,300);
        //            targetRect = new Rect(
        //                                  (TailNode as Node).OffsetX,
        //                                  (TailNode as Node).OffsetY,
        //                                  100,
        //                                  100);
        //            target.Position = new Point(endPoint.X- twofive, endPoint.Y - twofive);
        //        }

        //        if (this.ConnectorType == ConnectorType.Bezier)
        //        {
        //            #region Bezier
        //            try
        //            {
        //                if (HeadNode != null || TailNode != null)
        //                {
        //                    sp = startPoint;// MeasureUnitsConverter.FromPixels(startPoint, source.MeasurementUnit);
        //                    ep = endPoint;// MeasureUnitsConverter.FromPixels(endPoint, source.MeasurementUnit);

        //                    if (!(DiagramModel.TreeOrientation == TreeOrientation.LeftRight || DiagramModel.TreeOrientation == TreeOrientation.RightLeft))
        //                    {
        //                        ConnectorBase.GetOrthogonalLineIntersect(source, target, sourceRect, targetRect, out isTop, out isBottom, out isLeft, out isRight, out tisTop, out tisBottom, out tisLeft, out tisRight, out s, out e);
        //                        extendPoints(isTop, isBottom, isLeft, isRight, tisTop, tisBottom, tisLeft, tisRight, out s, out e, s, e);
        //                    }
        //                    else
        //                    {
        //                        ConnectorBase.GetTreeOrthogonalLineIntersect(source, target, sourceRect, targetRect, out isTop, out isBottom, out isLeft, out isRight, out tisTop, out tisBottom, out tisLeft, out tisRight, out s, out e);
        //                        extendPoints(isTop, isBottom, isLeft, isRight, tisTop, tisBottom, tisLeft, tisRight, out s, out e, s, e);
        //                    }

        //                }

                        

        //                if (this.LineAdorner != null)
        //                {
        //                    if ((this.LineAdorner as LineConnectorAdorner).ConnectorPoint)
        //                    {
        //                        (this.LineAdorner as LineConnectorAdorner).ConnectorPoint = false;
        //                        if ((this.LineAdorner as LineConnectorAdorner).IsHeadThumb)
        //                        {
        //                            s = Mouse.GetPosition(this);// MeasureUnitsConverter.FromPixels(Mouse.GetPosition(this), this.MeasurementUnit);
        //                            HeadNode = null;
        //                        }
        //                        else if ((this.LineAdorner as LineConnectorAdorner).IsTailThumb)
        //                        {
        //                            e = Mouse.GetPosition(this);// MeasureUnitsConverter.FromPixels(Mouse.GetPosition(this), this.MeasurementUnit);
        //                            TailNode = null;
        //                        }
        //                    }
        //                }
        //                x1 = s.X;// MeasureUnitsConverter.ToPixels(s.X, this.MeasurementUnit);
        //                y1 = s.Y;// MeasureUnitsConverter.ToPixels(s.Y, this.MeasurementUnit);
        //                x2 = e.X;// MeasureUnitsConverter.ToPixels(e.X, this.MeasurementUnit);
        //                y2 = e.Y;// MeasureUnitsConverter.ToPixels(e.Y, this.MeasurementUnit);
        //                PathGeometry pathgeometry = new PathGeometry();
        //                PathFigure pathfigure = new PathFigure();
        //                double num = 150.0;
        //                Point t = new Point(x1, y1);
        //                pathfigure.StartPoint = t;
        //                BezierSegment segment = new BezierSegment();

        //                if (isBottom)
        //                {
        //                    segment = GetSegment(x1, y1 + num, x2, y2, x2, y2, num, tisTop, tisBottom, tisLeft, tisRight);
        //                }
        //                else if (isTop)
        //                {
        //                    segment = GetSegment(x1, y1 - num, x2, y2, x2, y2, num, tisTop, tisBottom, tisLeft, tisRight);
        //                }
        //                else if (isRight)
        //                {
        //                    segment = GetSegment(x1 + num, y1, x2, y2, x2, y2, num, tisTop, tisBottom, tisLeft, tisRight);
        //                }
        //                else
        //                {
        //                    segment = GetSegment(x1 - num, y1, x2, y2, x2, y2, num, tisTop, tisBottom, tisLeft, tisRight);
        //                }

        //                double x = Math.Pow((x1 - x2), 2);
        //                double y = Math.Pow((y1 - y2), 2);
        //                Distance = Math.Sqrt(x + y);
        //                pathfigure.Segments.Add(segment);
        //                pathgeometry.Figures.Add(pathfigure);
        //                this.VirtualConnectorPathGeometry = pathgeometry.Clone();
        //                this.WidenedPathGeometry = pathgeometry.GetWidenedPathGeometry(new Pen(Brushes.Black, 1));
        //                invalidateBridging = true;
        //            }
        //            catch
        //            {
        //            }
        //            #endregion Bezier
        //        }

        //        else
        //        {
        //            {
        //                List<Point> connectionPoints = new List<Point>();
        //                PathGeometry pathgeometry = new PathGeometry();
        //                //if (ConnectorType != ConnectorType.Orthogonal && IntermediatePoints != null && IntermediatePoints.Count != 2)
        //                {
        //                    connectionPoints = GetTerminalPoints(this as LineConnector);
        //                }
        //                if (IntermediatePoints == null)
        //                {
        //                    IntermediatePoints = new List<Point>();
        //                }
        //                if (connectionPoints.Count > 0)
        //                {
        //                    if (this.ConnectorType == ConnectorType.Orthogonal)
        //                    {
        //                        connectionPoints = meetOrhogonalConstraints(connectionPoints, cs);
        //                    }
        //                    m_isoverlapped = false;
        //                    PathFigure pathfigure = new PathFigure();
        //                    if (IntermediatePoints.Count >= 1)
        //                    {
        //                        for (int i = IntermediatePoints.Count - 1; i >= 0; i--)
        //                        {
        //                            connectionPoints.Insert(1, IntermediatePoints[i]);
        //                        }
        //                    }
        //                    for (int i = 0; i < connectionPoints.Count; i++)
        //                    {
        //                        //connectionPoints[i] = MeasureUnitsConverter.ToPixels(connectionPoints[i], this.MeasurementUnit);
        //                    }
        //                    if (this.ConnectorType == ConnectorType.Orthogonal)
        //                        adjustIntermediatePoints(connectionPoints);
        //                    //findDistance(connectionPoints);
        //                    pathfigure.StartPoint = connectionPoints[0];
        //                    connectionPoints.Remove(connectionPoints[0]);
        //                    foreach (Point p in connectionPoints)
        //                    {
        //                        pathfigure.Segments.Add(new LineSegment(p, true));
        //                    }
        //                    //pathfigure.Segments.Add(new PolyLineSegment(connectionPoints, true));
        //                    pathgeometry.Figures.Add(pathfigure);
        //                    //this.ConnectorPathGeometry = pathgeometry;
        //                    //this.VirtualConnectorPathGeometry = pathgeometry.Clone();
        //                    if (this.ConnectorPathGeometry != null)
        //                    {
        //                        this.ConnectorPathGeometry.Figures.Clear();
        //                        this.ConnectorPathGeometry.Figures.Add(pathfigure);
        //                        //this.UpdateDecoratorPosition();
        //                    }
        //                    else
        //                    {
        //                        this.ConnectorPathGeometry = pathgeometry;
        //                    }
        //                    this.VirtualConnectorPathGeometry = pathgeometry.Clone();
        //                    this.UpdateDecoratorPosition();
        //                    this.WidenedPathGeometry = pathgeometry.GetWidenedPathGeometry(new Pen(Brushes.Black, 1));

        //                    setMinMax();
        //                    invalidateBridging = true;
        //                    //{
        //                    //    //if (disp != null)
        //                    //    //{
        //                    //    //    disp.Abort();
        //                    //    //}
        //                    //    //object o = this.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.SystemIdle,
        //                    //    //    new Initialize(this.SetLineBridging));
        //                    //    disp = this.Dispatcher.BeginInvoke(
        //                    //        System.Windows.Threading.DispatcherPriority.SystemIdle,
        //                    //        new Initialize(this.Thread));
        //                    //}
        //                    //if (ConnectorType != ConnectorType.Bezier)
        //                    //{
        //                    //    SetLineBridging();
        //                    //}
        //                }
        //            }

        //            #region dump
        //            //if (HeadNode != null || TailNode != null)
        //            //{
        //            //    List<Point> connectionPoints = new List<Point>();

        //            //    PathGeometry pathgeometry = new PathGeometry();
        //            //    connectionPoints = GetTerminalPoints();
        //            //    if (connectionPoints.Count > 0)
        //            //    {
        //            //        if (this.ConnectorType == ConnectorType.Orthogonal)
        //            //        {
        //            //            connectionPoints = meetOrhogonalConstraints(connectionPoints, cs);
        //            //        }
        //            //        m_isoverlapped = false;
        //            //        PathFigure pathfigure = new PathFigure();

        //            //        if (ConnectorType == ConnectorType.Orthogonal && (IntermediatePoints == null || IntermediatePoints.Count == 0))
        //            //        {
        //            //            IntermediatePoints = new List<Point>();

        //            //            IntermediatePoints.Add(new Point(connectionPoints[0].X + 10, connectionPoints[0].Y));
        //            //            IntermediatePoints.Add(new Point(IntermediatePoints[0].X, connectionPoints[1].Y));
        //            //        }
        //            //        else if (IntermediatePoints != null)
        //            //        {
        //            //            for (int i = IntermediatePoints.Count - 1; i >= 0; i--)
        //            //            {
        //            //                connectionPoints.Insert(1, IntermediatePoints[i]);
        //            //            }
        //            //        }
        //            //        for (int i = 0; i < connectionPoints.Count; i++)
        //            //        {
        //            //            connectionPoints[i] = MeasureUnitsConverter.ToPixels(connectionPoints[i], this.MeasurementUnit);
        //            //        }
        //            //        if (this.ConnectorType == ConnectorType.Orthogonal)
        //            //            adjustIntermediatePoints(connectionPoints);
        //            //        findDistance(connectionPoints);
        //            //        pathfigure.StartPoint = connectionPoints[0];
        //            //        connectionPoints.Remove(connectionPoints[0]);
        //            //        pathfigure.Segments.Add(new PolyLineSegment(connectionPoints, true));
        //            //        pathgeometry.Figures.Add(pathfigure);
        //            //        this.ConnectorPathGeometry = pathgeometry;
        //            //    }

        //            //}
        //            //else
        //            //{
        //            //    PathGeometry pathgeometry = new PathGeometry();
        //            //    List<Point> connectionPoints = GetTerminalPoints();
        //            //    if (connectionPoints.Count > 0)
        //            //    {
        //            //        PathFigure pathfigure = new PathFigure();
        //            //        if (IntermediatePoints == null || IntermediatePoints.Count == 0)
        //            //        {
        //            //            IntermediatePoints = new List<Point>();
        //            //            if (this.ConnectorType == ConnectorType.Orthogonal)
        //            //            {
        //            //                IntermediatePoints.Add(new Point(connectionPoints[0].X, connectionPoints[0].Y + MeasureUnitsConverter.FromPixels(20, this.MeasurementUnit)));
        //            //                IntermediatePoints.Add(new Point(connectionPoints[1].X, IntermediatePoints[0].Y));
        //            //                for (int i = IntermediatePoints.Count - 1; i >= 0; i--)
        //            //                {
        //            //                    connectionPoints.Insert(1, IntermediatePoints[i]);
        //            //                }
        //            //            }
        //            //        }
        //            //        else
        //            //        {
        //            //            for (int i = IntermediatePoints.Count - 1; i >= 0; i--)
        //            //            {
        //            //                connectionPoints.Insert(1, IntermediatePoints[i]);
        //            //            }
        //            //            if (ConnectorType == ConnectorType.Orthogonal)
        //            //            {
        //            //                if (LineAdorner != null)
        //            //                    (this.LineAdorner as LineConnectorAdorner).InvalidateVertexs();
        //            //            }
        //            //        }
        //            //        if (this.ConnectorType == ConnectorType.Orthogonal && IntermediatePoints.Count == 2)
        //            //        {
        //            //            defaultStyle();
        //            //        }
        //            //        for (int i = 0; i < connectionPoints.Count; i++)
        //            //        {
        //            //            connectionPoints[i] = MeasureUnitsConverter.ToPixels(connectionPoints[i], this.MeasurementUnit);
        //            //        }

        //            //        if (this.ConnectorType == ConnectorType.Orthogonal)
        //            //            adjustIntermediatePoints(connectionPoints);
        //            //        findDistance(connectionPoints);
        //            //        pathfigure.StartPoint = connectionPoints[0];
        //            //        connectionPoints.Remove(connectionPoints[0]);
        //            //        pathfigure.Segments.Add(new PolyLineSegment(connectionPoints, true));
        //            //        pathgeometry.Figures.Add(pathfigure);
        //            //        this.ConnectorPathGeometry = pathgeometry;
        //            //    }
        //            //}
        //            #endregion dump
        //        }
        //        if (LineAdorner != null && ConnectorType != ConnectorType.Bezier)
        //        {
        //            //(this.LineAdorner as LineConnectorAdorner).InvalidateVertexs();
        //            (this.LineAdorner as LineConnectorAdorner).UpdateVertexsPosition();
        //        }
        //    }

        //    this.bridged = true;
            
        //}

        internal void Thread()
        {
            if (dc != null && dc.Model != null)
            {
                List<UIElement> ordered = (from UIElement item in dc.Model.Connections
                                           orderby Panel.GetZIndex(item as UIElement)
                                           select item as UIElement).ToList();
                foreach (UIElement element in ordered)
                {
                    if (element is LineConnector)
                    {
                        (element as LineConnector).SetLineBridging();
                    }
                }
            }
        }
        internal void SetLineBridging()
        {

            if (this.dc != null /*&& !dc.View.Isdragdelta*/ && dc.Model.LineBridgingEnabled && this.LineBridgingEnabled)
            {
                Dictionary<int, List<ArcSegment>> arcD = new Dictionary<int, List<ArcSegment>>();
                Dictionary<int, List<Point>> startBD = new Dictionary<int, List<Point>>();
                int count = -1;
                foreach (LineConnector lc in dc.Model.Connections)
                {
                    if (/*lc != this &&*/ (lc.zOrder < this.zOrder || lc.ConnectorType == ConnectorType.Bezier || !lc.LineBridgingEnabled)
                        && (lc.VirtualConnectorPathGeometry != null && this.VirtualConnectorPathGeometry != null)
                        && (lc.invalidateBridging || this.invalidateBridging)
                        )
                    {
                        if (IsCPGOverlapped(this.VirtualConnectorPathGeometry.Bounds, lc.VirtualConnectorPathGeometry.Bounds))
                        {
                            Point[] pts;
                            if (!lc.Equals(this))
                            {
                                pts = GetIntersectionPointsFromWidened(this.WidenedPathGeometry, lc.WidenedPathGeometry);
                                //pts = LineConnector.FindPOIBetweenTwoPolyLine(this.VirtualConnectorPathGeometry.Figures[0], lc.VirtualConnectorPathGeometry.Figures[0]).ToArray<Point>();
                            }
                            else
                            {
                                List<Point> pts1;

                                List<Point> line1 = new List<Point>();
                                //line1 = base.getLinePtsOnlyFromCPG();
                                line1.Add(this.VirtualConnectorPathGeometry.Figures[0].StartPoint);
                                line1.AddRange(IntermediatePoints);
                                line1.Add((this.VirtualConnectorPathGeometry.Figures[0].Segments.Last<PathSegment>() as LineSegment).Point);

                                List<Point> line2 = new List<Point>();
                                line2.Add(lc.VirtualConnectorPathGeometry.Figures[0].StartPoint);
                                line2.AddRange(lc.IntermediatePoints);
                                line2.Add((lc.VirtualConnectorPathGeometry.Figures[0].Segments.Last<PathSegment>() as LineSegment).Point);

                                pts1 = FindPOIBetweenTwoPolyLine(line1, line2, true);
                                //pts1 = LineConnector.FindPOIBetweenTwoPolyLine(this.VirtualConnectorPathGeometry.Figures[0], lc.VirtualConnectorPathGeometry.Figures[0]);
                                pts = (pts1.ToArray<Point>()).ToArray<Point>();
                            }


                            //KeyValuePair<int, ArcSegment> kv;
                            //kv.
                            foreach (Point p in pts)
                            {
                                //count++;
                                double fullLength;
                                Point dummy;
                                int segmentIndex;
                                double length = GetLengthAtFractionPoint(this.VirtualConnectorPathGeometry.Figures[0], p, out fullLength, out segmentIndex);
                                if (segmentIndex < 0)
                                {
                                    continue;
                                }
                                Point startBridge, endBridge;
                                double fractLength = (length - (this.BridgeSpacing / 2)) / fullLength;
                                this.VirtualConnectorPathGeometry.GetPointAtFractionLength(fractLength, out startBridge, out dummy);
                                fractLength = (length + (this.BridgeSpacing / 2)) / fullLength;
                                this.VirtualConnectorPathGeometry.GetPointAtFractionLength(fractLength, out endBridge, out dummy);



                                //PathFigure pf = new PathFigure();
                                //pf.StartPoint = startBridge;
                                //pf.StartPoint = p;
                                //pf.Segments.Add(new ArcSegment(endBridge, new Size(8, 8), 0, true, SweepDirection.Clockwise, true));
                                //pf.Segments.Add(new ArcSegment(new Point(p.X+10,p.Y+10), new Size(5, 5), 0, true, SweepDirection.Clockwise, true));

                                //this.ConnectorPathGeometry.Figures.Add(pf);
                                //int index = this.ConnectorPathGeometry.Figures[0].Segments.IndexOf(selectedSegment);
                                SweepDirection sd;

                                Point start, end;
                                if (segmentIndex == 0)
                                {
                                    start = (this.VirtualConnectorPathGeometry.Figures[0].StartPoint);
                                }
                                else
                                {
                                    start = ((this.VirtualConnectorPathGeometry.Figures[0].Segments[segmentIndex - 1] as LineSegment).Point);
                                }
                                end = (this.VirtualConnectorPathGeometry.Figures[0].Segments[segmentIndex] as LineSegment).Point;
                                double angle = findAngle(start, end);
                                if (angle > 0 && angle < 180)
                                {
                                    sd = SweepDirection.Clockwise;
                                }
                                else
                                {
                                    sd = SweepDirection.Counterclockwise;
                                }

                                if (arcD.ContainsKey(segmentIndex))
                                {
                                    Point fixedpoint;
                                    if (segmentIndex == 0)
                                    {
                                        fixedpoint = (this.VirtualConnectorPathGeometry.Figures[0].StartPoint);
                                    }
                                    else
                                    {
                                        fixedpoint = ((this.VirtualConnectorPathGeometry.Figures[0].Segments[segmentIndex - 1] as LineSegment).Point);
                                    }

                                    double fix = Math.Abs((fixedpoint - endBridge).Length);
                                    double var;

                                    int insertAt = -1;
                                    count = -1;
                                    foreach (ArcSegment arc in arcD[segmentIndex])
                                    {
                                        count++;
                                        var = Math.Abs((fixedpoint - arc.Point).Length);
                                        //if (Math.Abs(fix - var) < BridgeSpacing)
                                        //{
                                        //    if (fix < var)
                                        //    {
                                        //        startBD[segmentIndex][count] = startBridge;
                                        //        insertAt = -2;
                                        //    }
                                        //    else
                                        //    {
                                        //        arcD[segmentIndex][count].Point = endBridge;
                                        //        insertAt = -2;
                                        //    }
                                        //}
                                        if (fix < var)
                                        {
                                            insertAt = count;
                                            break;
                                        }
                                    }
                                    if (insertAt >= 0)
                                    {
                                        arcD[segmentIndex].Insert(insertAt, new ArcSegment(endBridge, new Size(1, 1), 0, true, sd, true));
                                        startBD[segmentIndex].Insert(insertAt, startBridge);
                                        //arcD[segmentIndex].Add(new ArcSegment(endBridge, new Size(1, 1), 0, true, SweepDirection.Clockwise, true));
                                        //startBD[segmentIndex].Add(startBridge);
                                    }
                                    else //if (insertAt == -1)
                                    {
                                        arcD[segmentIndex].Add(new ArcSegment(endBridge, new Size(1, 1), 0, true, sd, true));
                                        startBD[segmentIndex].Add(startBridge);
                                    }
                                }
                                else
                                {
                                    List<ArcSegment> arcs = new List<ArcSegment>();
                                    arcs.Add(new ArcSegment(endBridge, new Size(1, 1), 0, true, sd, true));
                                    List<Point> points = new List<Point>();
                                    points.Add(startBridge);
                                    arcD.Add(segmentIndex, arcs);
                                    startBD.Add(segmentIndex, points);
                                }
                                /*segmentIndex += (2 * count);
                                if (segmentIndex >= 0)
                                {
                                    //if (selectedSegment is LineSegment)
                                    {
                                        Point end = (ConnectorPathGeometry.Figures[0].Segments[segmentIndex] as LineSegment).Point;
                                        ConnectorPathGeometry.Figures[0].Segments.Insert(segmentIndex + 1, new ArcSegment(endBridge, new Size(8, 8), 0, true, SweepDirection.Clockwise, true));
                                        ConnectorPathGeometry.Figures[0].Segments.Insert(segmentIndex + 2, new LineSegment(end, true));
                                        (ConnectorPathGeometry.Figures[0].Segments[segmentIndex] as LineSegment).Point = startBridge;
                                    }
                                }*/
                            }
                        }
                    }
                }
                var v = (from k in arcD.Keys orderby k ascending select k);//.ToDictionary<ArcSegment,int>(new Func<ArcSegment,int>(ArcSegmentTarget));
                count = -1;
                if (arcD.Count > 0)
                {
                    this.ConnectorPathGeometry = this.VirtualConnectorPathGeometry.Clone();
                }
                foreach (int Index in v)
                {
                    this.bridged = true;
                    for (int i = 1; i < arcD[Index].Count; i++)
                    {
                        if ((arcD[Index][i].Point - arcD[Index][i - 1].Point).Length < BridgeSpacing)
                        {
                            arcD[Index][i - 1].Point = arcD[Index][i].Point;
                            arcD[Index].RemoveAt(i);
                            startBD[Index].RemoveAt(i);
                            i--;
                        }
                    }
                    //count++;
                    int segmentIndex;//= Index + (2 * count);
                    var item = arcD[Index];
                    foreach (ArcSegment arc in arcD[Index])
                    {
                        count++;
                        segmentIndex = Index + (2 * count);
                        Point end = (ConnectorPathGeometry.Figures[0].Segments[segmentIndex] as LineSegment).Point;
                        //ConnectorPathGeometry.Figures[0].Segments.Insert(segmentIndex + 1, new ArcSegment(endBridge, new Size(8, 8), 0, true, SweepDirection.Clockwise, true));
                        ConnectorPathGeometry.Figures[0].Segments.Insert(segmentIndex + 1, arc);
                        ConnectorPathGeometry.Figures[0].Segments.Insert(segmentIndex + 2, new LineSegment(end, true));
                        (ConnectorPathGeometry.Figures[0].Segments[segmentIndex] as LineSegment).Point = startBD[Index][arcD[Index].IndexOf(arc)];
                    }
                }
            }

        }

        private bool IsCPGOverlapped(Rect rect, Rect rect_2)
        {
            return rect.IntersectsWith(rect_2);
        }

        private void extendPoints(bool isTop, bool isBottom, bool isLeft, bool isRight, bool tisTop, bool tisBottom, bool tisLeft, bool tisRight, out Point s, out Point e, Point xs, Point xe)
        {
            s = xs;
            e = xe;
            double fivezero = 50;// MeasureUnitsConverter.FromPixels(50, this.MeasurementUnit);
            if (isTop)
            {
                s = new Point(s.X, s.Y - fivezero);
            }
            else if (isBottom)
            {
                s = new Point(s.X, s.Y + fivezero);
            }
            else if (isRight)
            {
                s = new Point(s.X + fivezero, s.Y);
            }
            else if (isRight)
            {
                s = new Point(s.X - fivezero, s.Y);
            }

            if (tisTop)
            {
                e = new Point(e.X, e.Y - fivezero);
            }
            else if (tisBottom)
            {
                e = new Point(e.X, e.Y + fivezero);
            }
            else if (tisLeft)
            {
                e = new Point(e.X - fivezero, e.Y);
            }
            else if (tisRight)
            {
                e = new Point(e.X + fivezero, e.Y);
            }

        }

        internal Rect getNodeRect(Node n)
        {
            Rect rect = new Rect();
            if (n != null && n.Boundaries != null && n.Boundaries.RenderedGeometry != null)
            {
                rect = n.Boundaries.RenderedGeometry.Bounds;
            }
            if (n != null && n.Boundaries != null && (n.Boundaries is Path) && (n.Boundaries as Path).Data != null && (n.Boundaries as Path).Data != Geometry.Empty)
            {
                rect = (n.Boundaries as Path).Data.Bounds;
            }
            if (dc.View != null && !dc.View.Page.Children.Contains(n))
            {
                  rect= new Rect(n.PxOffsetX,n.PxOffsetY,n.ActualWidth,n.ActualHeight);
            }
            return rect;
        }
        internal double verifyNan(double value)
        {
            if (Double.IsNaN(value))
            {
                return 0;
            }
            else
            {
                return value;
            }
        }

        internal List<Point> GetTerminalPoints()
        {
            List<Point> ret = new List<Point>();
            if (this.LineAdorner != null)
            {
                if ((this.LineAdorner as LineConnectorAdorner).ConnectorPoint)
                {
                    (this.LineAdorner as LineConnectorAdorner).ConnectorPoint = false;
                    if ((this.LineAdorner as LineConnectorAdorner).IsHeadThumb)
                    {
                        HeadNode = null;
                    }
                    else if ((this.LineAdorner as LineConnectorAdorner).IsTailThumb)
                    {
                        TailNode = null;
                    }

                }
            }
            if (this.HeadNode != null && this.TailNode != null)
            {
                if ((this.HeadNode as Node).IsInternallyLoaded&&(this.TailNode as Node).IsInternallyLoaded)
                {
                    if (IntermediatePoints != null && IntermediatePoints.Count >= 1)
                    {
                        ret.Add(GetIntersectionPoint(IntermediatePoints[0], HeadNode as Node));
                        ret.Add(GetIntersectionPoint(IntermediatePoints[IntermediatePoints.Count - 1], TailNode as Node));
                    }
                    else
                    {
                        ret.Add(GetIntersectionPoint((TailNode as Node).PxPosition/*MeasureUnitsConverter.FromPixels((TailNode as Node).Position, MeasurementUnit)*/, HeadNode as Node));
                        ret.Add(GetIntersectionPoint((HeadNode as Node).PxPosition/*MeasureUnitsConverter.FromPixels((HeadNode as Node).Position, MeasurementUnit)*/, TailNode as Node));
                    }
                }
                else  if (!(this.HeadNode as Node).IsInternallyLoaded && (this.TailNode as Node).IsInternallyLoaded)
                {
                    ret.Add(new Point((this.HeadNode as Node).OffsetX + (verifyNan((this.HeadNode as Node)._Width) / 2), (this.HeadNode as Node).OffsetY + (verifyNan((this.HeadNode as Node)._Height) / 2)));
                    if (IntermediatePoints != null && IntermediatePoints.Count >= 1)
                    {
                        ret.Add(GetIntersectionPoint(IntermediatePoints[IntermediatePoints.Count - 1], TailNode as Node));
                    }
                    else
                    {
                        ret.Add(GetIntersectionPoint(new Point((this.HeadNode as Node).OffsetX, (this.HeadNode as Node).OffsetY), TailNode as Node));
                        
                    }
                }
                else if ((this.HeadNode as Node).IsInternallyLoaded && !(this.TailNode as Node).IsInternallyLoaded)
                {
                    if (IntermediatePoints != null && IntermediatePoints.Count >= 1)
                    {
                        ret.Add(GetIntersectionPoint(IntermediatePoints[0], HeadNode as Node));
                    }
                    else
                    {
                        ret.Add(GetIntersectionPoint(new Point((this.TailNode as Node).OffsetX, (this.TailNode as Node).OffsetY), HeadNode as Node));
                    }
                    ret.Add(new Point((this.TailNode as Node).OffsetX + (verifyNan((this.TailNode as Node)._Width) / 2), (this.TailNode as Node).OffsetY + (verifyNan((this.TailNode as Node)._Height) / 2)));
                }
            }
            else if (this.HeadNode == null && this.TailNode != null)
            {
                ret.Add(this.PxStartPointPosition);
                if (IntermediatePoints != null && IntermediatePoints.Count >= 1)
                {
                    ret.Add(GetIntersectionPoint(IntermediatePoints[IntermediatePoints.Count - 1], TailNode as Node));
                }
                else
                {
                    ret.Add(GetIntersectionPoint(this.PxStartPointPosition, TailNode as Node));
                }
            }
            else if (this.HeadNode != null && this.TailNode == null)
            {
                if (IntermediatePoints != null && IntermediatePoints.Count >= 1)
                {
                    ret.Add(GetIntersectionPoint(IntermediatePoints[0], HeadNode as Node));
                }
                else
                {
                    ret.Add(GetIntersectionPoint(this.PxEndPointPosition, HeadNode as Node));
                }
                ret.Add(this.PxEndPointPosition);
            }
            else if (this.HeadNode == null && this.TailNode == null)
            {
                ret.Add(PxStartPointPosition);
                ret.Add(PxEndPointPosition);
            }
            if (dview != null && dview.Page != null && ret != null && ret.Count == 2)
            {
                if (this.ConnectionHeadPort != null && HeadNode != null)
                {
                    ret[0] = new Point(ConnectionHeadPort.CenterPosition.X, ConnectionHeadPort.CenterPosition.Y);
                    ret[0] = (HeadNode as Node).TranslatePoint(ret[0], dview.Page);
                    //ret[0] = MeasureUnitsConverter.FromPixels(ret[0], this.MeasurementUnit);
                }
                if (this.ConnectionTailPort != null && TailNode != null)
                {
                    ret[1] = new Point(ConnectionTailPort.CenterPosition.X, ConnectionTailPort.CenterPosition.Y);
                    ret[1] = (TailNode as Node).TranslatePoint(ret[1], dview.Page);
                    //ret[1] = MeasureUnitsConverter.FromPixels(ret[1], this.MeasurementUnit);
                }
            }
            return ret;
        }

        private List<Point> meetOrhogonalConstraints(List<Point> connectionPoints, double cs)
        {
            if (this.AutoAdjustPoints && this.HeadNode != null && this.TailNode != null && this.ConnectionHeadPort != null && this.ConnectionTailPort != null)
            {
                Dock HeadDirection = Dock.Top;
                Dock TailDirection = Dock.Top;

                double headDist = this.FirstSegmentLength;
                double tailDist = this.LastSegmentLength;
                bool isHeadRecal = false, isTailRecal = false;
                if (!double.IsNaN(headDist))
                {
                    isHeadRecal = true;
                }
                else
                {
                    headDist = 50;
                }

                if (!double.IsNaN(tailDist))
                {
                    isTailRecal = true;
                }
                else
                {
                    tailDist = 50;
                }

                Node HeadNode = (this.HeadNode as Node);
                Node TailNode = (this.TailNode as Node);
                //connectionPoints.Clear();
                List<Point> PxInterPts = new List<Point>();
                foreach (Point pt in this.IntermediatePoints)
                {
                    PxInterPts.Add(pt);//MeasureUnitsConverter.ToPixels(pt, this.MeasurementUnit));
                }
                if (this.ConnectionHeadPort != null)
                {
                    HeadDirection = this.ConnectionHeadPort.Direction();
                }
                if (this.ConnectionTailPort != null)
                {
                    TailDirection = this.ConnectionTailPort.Direction();
                }
                if (this.HeadNode != null && this.TailNode != null && this.ConnectionHeadPort != null && this.ConnectionTailPort != null)
                {
                    // Horizontal - Horizontal
                    if ((HeadDirection == Dock.Left || HeadDirection == Dock.Right) &&
                        (TailDirection == Dock.Left || TailDirection == Dock.Right))
                    {
                        #region init
                        bool is4Pts = false;
                        if (HeadDirection == Dock.Right && TailDirection == Dock.Right)
                        {
                            if ((ConnectionTailPort.PagePosition.Y > HeadNode.Top) && (ConnectionTailPort.PagePosition.Y < HeadNode.Bottom))
                            {
                                is4Pts = true;
                            }
                        }
                        else if (HeadDirection == Dock.Left && TailDirection == Dock.Left)
                        {
                            if ((ConnectionTailPort.PagePosition.Y > HeadNode.Top) && (ConnectionTailPort.PagePosition.Y < HeadNode.Bottom))
                            {
                                is4Pts = true;
                            }
                        }
                        else if (HeadDirection == Dock.Left && TailDirection == Dock.Right)
                        {
                            if (HeadNode.Left < TailNode.Right)
                            {
                                is4Pts = true;
                            }
                        }
                        else if (HeadDirection == Dock.Right && TailDirection == Dock.Left)
                        {
                            if (HeadNode.Right > TailNode.Left)
                            {
                                is4Pts = true;
                            }
                        }
                        #endregion

                        #region 4 Points Required
                        //Required 4 inter pts.
                        if (is4Pts)
                        {
                            bool isNewPtsAdded = false;
                            if (PxInterPts.Count % 2 != 0)
                            {
                                isNewPtsAdded = true;
                                PxInterPts.Add(new Point(0, 0));
                            }
                            //Update Head (Right)
                            if (HeadDirection == Dock.Right)
                            {
                                if (PxInterPts.Count >= 4)
                                {
                                    if (isHeadRecal || PxInterPts[0].X < HeadNode.Right)
                                    {
                                        PxInterPts[0] = new Point(ConnectionHeadPort.PagePosition.X + headDist, ConnectionHeadPort.PagePosition.Y);
                                    }
                                    else
                                    {
                                        PxInterPts[0] = new Point(PxInterPts[0].X, ConnectionHeadPort.PagePosition.Y);
                                    }
                                    PxInterPts[1] = new Point(PxInterPts[0].X, PxInterPts[1].Y);
                                }
                                else
                                {
                                    while (PxInterPts.Count < 4)
                                    {
                                        isNewPtsAdded = true;
                                        PxInterPts.Add(new Point(0, 0));
                                    }
                                    PxInterPts[0] = new Point(ConnectionHeadPort.PagePosition.X + headDist, ConnectionHeadPort.PagePosition.Y);
                                    PxInterPts[1] = new Point(PxInterPts[0].X, ConnectionHeadPort.PagePosition.X - headDist);
                                }
                            }
                            //Update Head (Left)
                            else
                            {
                                if (PxInterPts.Count >= 4)
                                {
                                    if (isHeadRecal || PxInterPts[0].X > HeadNode.Left)
                                    {
                                        PxInterPts[0] = new Point(ConnectionHeadPort.PagePosition.X - headDist, ConnectionHeadPort.PagePosition.Y);
                                    }
                                    else
                                    {
                                        PxInterPts[0] = new Point(PxInterPts[0].X, ConnectionHeadPort.PagePosition.Y);
                                    }
                                    PxInterPts[1] = new Point(PxInterPts[0].X, PxInterPts[1].Y);
                                }
                                else
                                {
                                    while (PxInterPts.Count < 4)
                                    {
                                        isNewPtsAdded = true;
                                        PxInterPts.Add(new Point(0, 0));
                                    }
                                    PxInterPts[0] = new Point(ConnectionHeadPort.PagePosition.X - headDist, ConnectionHeadPort.PagePosition.Y);
                                    PxInterPts[1] = new Point(PxInterPts[0].X, ConnectionHeadPort.PagePosition.Y - headDist);
                                }
                            }

                            //Update Tail (Right)
                            if (TailDirection == Dock.Right)
                            {
                                if (PxInterPts.Count >= 4 && !isNewPtsAdded)
                                {
                                    int last = PxInterPts.Count;
                                    if (isTailRecal || PxInterPts[last - 1].X < TailNode.Right)
                                    {
                                        PxInterPts[last - 1] = new Point(ConnectionTailPort.PagePosition.X + tailDist, ConnectionTailPort.PagePosition.Y);
                                    }
                                    else
                                    {
                                        PxInterPts[last - 1] = new Point(PxInterPts[last - 1].X, ConnectionTailPort.PagePosition.Y);
                                    }
                                    PxInterPts[last - 2] = new Point(PxInterPts[last - 1].X, PxInterPts[last - 2].Y);
                                    PxInterPts[last - 3] = new Point(PxInterPts[last - 3].X, PxInterPts[last - 2].Y);
                                }
                                else
                                {
                                    while (PxInterPts.Count < 4)
                                    {
                                        isNewPtsAdded = true;
                                        PxInterPts.Add(new Point(0, 0));
                                    }
                                    int last = PxInterPts.Count;
                                    PxInterPts[last - 1] = new Point(ConnectionTailPort.PagePosition.X + tailDist, ConnectionTailPort.PagePosition.Y);
                                    PxInterPts[last - 2] = new Point(PxInterPts[last - 1].X, ConnectionTailPort.PagePosition.Y - tailDist);
                                    PxInterPts[last - 3] = new Point(PxInterPts[last - 3].X, PxInterPts[last - 2].Y);
                                }
                            }
                            //Update Tail (Left)
                            else
                            {
                                if (PxInterPts.Count >= 4 && !isNewPtsAdded)
                                {
                                    int last = PxInterPts.Count;
                                    if (isTailRecal || PxInterPts[last - 1].X > TailNode.Left)
                                    {
                                        PxInterPts[last - 1] = new Point(ConnectionTailPort.PagePosition.X - tailDist, ConnectionTailPort.PagePosition.Y);
                                    }
                                    else
                                    {
                                        PxInterPts[last - 1] = new Point(PxInterPts[last - 1].X, ConnectionTailPort.PagePosition.Y);
                                    }
                                    PxInterPts[last - 2] = new Point(PxInterPts[last - 1].X, PxInterPts[last - 2].Y);
                                    PxInterPts[last - 3] = new Point(PxInterPts[last - 3].X, PxInterPts[last - 2].Y);
                                }
                                else
                                {
                                    while (PxInterPts.Count < 4)
                                    {
                                        isNewPtsAdded = true;
                                        PxInterPts.Add(new Point(0, 0));
                                    }
                                    int last = PxInterPts.Count;
                                    PxInterPts[last - 1] = new Point(ConnectionTailPort.PagePosition.X + tailDist, ConnectionTailPort.PagePosition.Y);
                                    PxInterPts[last - 2] = new Point(PxInterPts[last - 1].X, ConnectionTailPort.PagePosition.Y - tailDist);
                                    PxInterPts[last - 3] = new Point(PxInterPts[last - 3].X, PxInterPts[last - 2].Y);
                                }
                            }
                        }
                        #endregion

                        #region 2 Points Required
                        //required 2 inter pts.
                        else
                        {
                            bool isNewPtsAdded = false;
                            if (PxInterPts.Count % 2 != 0)
                            {
                                isNewPtsAdded = true;
                                PxInterPts.Add(new Point(0, 0));
                            }
                            if (HeadDirection == Dock.Right)
                            {
                                if (PxInterPts.Count >= 2)
                                {
                                    int last = PxInterPts.Count;
                                    if (isHeadRecal || PxInterPts[0].X < HeadNode.Right)
                                    {
                                        PxInterPts[0] = new Point(ConnectionHeadPort.PagePosition.X + headDist, ConnectionHeadPort.PagePosition.Y);
                                    }
                                    else
                                    {
                                        PxInterPts[0] = new Point(PxInterPts[0].X, ConnectionHeadPort.PagePosition.Y);
                                    }
                                    PxInterPts[1] = new Point(PxInterPts[0].X, PxInterPts[1].Y);
                                }
                                else
                                {
                                    while (PxInterPts.Count < 2)
                                    {
                                        isNewPtsAdded = true;
                                        PxInterPts.Add(new Point(0, 0));
                                    }
                                    PxInterPts[0] = new Point(ConnectionHeadPort.PagePosition.X + headDist, ConnectionHeadPort.PagePosition.Y);
                                    PxInterPts[1] = new Point(PxInterPts[0].X, PxInterPts[1].Y);
                                }
                            }
                            else
                            {
                                if (PxInterPts.Count >= 2)
                                {
                                    if (isHeadRecal || PxInterPts[0].X > HeadNode.Left)
                                    {
                                        PxInterPts[0] = new Point(ConnectionHeadPort.PagePosition.X - headDist, ConnectionHeadPort.PagePosition.Y);
                                    }
                                    else
                                    {
                                        PxInterPts[0] = new Point(PxInterPts[0].X, ConnectionHeadPort.PagePosition.Y);
                                    }
                                    PxInterPts[1] = new Point(PxInterPts[0].X, PxInterPts[1].Y);
                                }

                                else
                                {
                                    while (PxInterPts.Count < 2)
                                    {
                                        isNewPtsAdded = true;
                                        PxInterPts.Add(new Point(0, 0));
                                    }
                                    PxInterPts[0] = new Point(ConnectionHeadPort.PagePosition.X - headDist, ConnectionHeadPort.PagePosition.Y);
                                    PxInterPts[1] = new Point(PxInterPts[0].X, PxInterPts[1].Y);
                                }
                            }

                            if (TailDirection == Dock.Right)
                            {
                                if ((PxInterPts.Count >= 2) && !isNewPtsAdded)
                                {
                                    int last = PxInterPts.Count;
                                    if ((isTailRecal && (PxInterPts.Count > 2 || (ConnectionHeadPort.PagePosition.X + headDist < ConnectionTailPort.PagePosition.X + tailDist))) || PxInterPts[last - 1].X < TailNode.Right)
                                    {
                                        PxInterPts[last - 1] = new Point(ConnectionTailPort.PagePosition.X + tailDist, ConnectionTailPort.PagePosition.Y);
                                    }
                                    else
                                    {
                                        PxInterPts[last - 1] = new Point(PxInterPts[last - 1].X, ConnectionTailPort.PagePosition.Y);
                                    }
                                    PxInterPts[last - 2] = new Point(PxInterPts[last - 1].X, PxInterPts[last - 2].Y);
                                }

                                else
                                {
                                    while (PxInterPts.Count < 2)
                                    {
                                        PxInterPts.Add(new Point(0, 0));
                                    }
                                    int last = PxInterPts.Count;
                                    PxInterPts[last - 1] = new Point(ConnectionTailPort.PagePosition.X + tailDist, ConnectionTailPort.PagePosition.Y);
                                    PxInterPts[last - 2] = new Point(PxInterPts[last - 1].X, PxInterPts[last - 2].Y);
                                }
                            }
                            else
                            {
                                if ((PxInterPts.Count >= 2) && !isNewPtsAdded)
                                {
                                    int last = PxInterPts.Count;
                                    if ((isTailRecal && (PxInterPts.Count > 2 || (ConnectionHeadPort.PagePosition.X - headDist > ConnectionTailPort.PagePosition.X - tailDist))) || PxInterPts[last - 1].X > TailNode.Left)
                                    {
                                        PxInterPts[last - 1] = new Point(ConnectionTailPort.PagePosition.X - tailDist, ConnectionTailPort.PagePosition.Y);
                                    }
                                    else
                                    {
                                        PxInterPts[last - 1] = new Point(PxInterPts[last - 1].X, ConnectionTailPort.PagePosition.Y);
                                    }
                                    PxInterPts[last - 2] = new Point(PxInterPts[last - 1].X, PxInterPts[last - 2].Y);
                                }

                                else
                                {
                                    while (PxInterPts.Count < 2)
                                    {
                                        PxInterPts.Add(new Point(0, 0));
                                    }
                                    int last = PxInterPts.Count;
                                    PxInterPts[last - 1] = new Point(ConnectionTailPort.PagePosition.X - tailDist, ConnectionTailPort.PagePosition.Y);
                                    PxInterPts[last - 2] = new Point(PxInterPts[last - 1].X, PxInterPts[last - 2].Y);
                                }
                            }
                        }

                        #endregion

                    }
                    // Vertical - Vertical
                    else if ((HeadDirection == Dock.Top || HeadDirection == Dock.Bottom) &&
                        (TailDirection == Dock.Top || TailDirection == Dock.Bottom))
                    {
                        #region init

                        bool is4Pts = false;
                        if (HeadDirection == Dock.Bottom && TailDirection == Dock.Bottom)
                        {
                            if ((ConnectionTailPort.PagePosition.X > HeadNode.Left) && (ConnectionTailPort.PagePosition.X < HeadNode.Right))
                            {
                                is4Pts = true;
                            }
                        }
                        else if (HeadDirection == Dock.Top && TailDirection == Dock.Top)
                        {
                            if ((ConnectionTailPort.PagePosition.X > HeadNode.Left) && (ConnectionTailPort.PagePosition.X < HeadNode.Right))
                            {
                                is4Pts = true;
                            }
                        }
                        else if (HeadDirection == Dock.Top && TailDirection == Dock.Bottom)
                        {
                            if (HeadNode.Top < TailNode.Bottom)
                            {
                                is4Pts = true;
                            }
                        }
                        else if (HeadDirection == Dock.Bottom && TailDirection == Dock.Top)
                        {
                            if (HeadNode.Bottom > TailNode.Top)
                            {
                                is4Pts = true;
                            }
                        }
                        #endregion

                        #region 4 Points Required
                        //Required 4 inter pts.
                        if (is4Pts)
                        {
                            bool isNewPtsAdded = false;
                            //Update Head (Bottom)
                            if (HeadDirection == Dock.Bottom)
                            {
                                if (PxInterPts.Count >= 4)
                                {
                                    if (isHeadRecal || PxInterPts[0].Y < HeadNode.Bottom)
                                    {
                                        PxInterPts[0] = new Point(ConnectionHeadPort.PagePosition.X, ConnectionHeadPort.PagePosition.Y + headDist);
                                    }
                                    else
                                    {
                                        PxInterPts[0] = new Point(ConnectionHeadPort.PagePosition.X, PxInterPts[0].Y);
                                    }
                                    PxInterPts[1] = new Point(PxInterPts[1].X, PxInterPts[0].Y);
                                }
                                else
                                {
                                    while (PxInterPts.Count < 4)
                                    {
                                        isNewPtsAdded = true;
                                        PxInterPts.Add(new Point(0, 0));
                                    }
                                    PxInterPts[0] = new Point(ConnectionHeadPort.PagePosition.X, ConnectionHeadPort.PagePosition.Y + headDist);
                                    PxInterPts[1] = new Point(ConnectionHeadPort.PagePosition.X - headDist, PxInterPts[0].Y);
                                }
                            }
                            //Update Head (Top)
                            else
                            {
                                if (PxInterPts.Count >= 4)
                                {
                                    if (isHeadRecal || PxInterPts[0].Y > HeadNode.Top)
                                    {
                                        PxInterPts[0] = new Point(ConnectionHeadPort.PagePosition.X, ConnectionHeadPort.PagePosition.Y - headDist);
                                    }
                                    else
                                    {
                                        PxInterPts[0] = new Point(ConnectionHeadPort.PagePosition.X, PxInterPts[0].Y);
                                    }
                                    PxInterPts[1] = new Point(PxInterPts[1].X, PxInterPts[0].Y);
                                }
                                else
                                {
                                    while (PxInterPts.Count < 4)
                                    {
                                        isNewPtsAdded = true;
                                        PxInterPts.Add(new Point(0, 0));
                                    }
                                    PxInterPts[0] = new Point(ConnectionHeadPort.PagePosition.X, ConnectionHeadPort.PagePosition.Y - headDist);
                                    PxInterPts[1] = new Point(ConnectionHeadPort.PagePosition.X - headDist, PxInterPts[0].Y);
                                }
                            }

                            //Update Tail (Bottom)
                            if (TailDirection == Dock.Bottom)
                            {
                                if (PxInterPts.Count >= 4 && !isNewPtsAdded)
                                {
                                    int last = PxInterPts.Count;
                                    if (isTailRecal || PxInterPts[last - 1].Y < TailNode.Bottom)
                                    {
                                        PxInterPts[last - 1] = new Point(ConnectionTailPort.PagePosition.X, ConnectionTailPort.PagePosition.Y + tailDist);
                                    }
                                    else
                                    {
                                        PxInterPts[last - 1] = new Point(ConnectionTailPort.PagePosition.X, PxInterPts[last - 1].Y);
                                    }
                                    PxInterPts[last - 2] = new Point(PxInterPts[last - 2].X, PxInterPts[last - 1].Y);
                                    PxInterPts[last - 3] = new Point(PxInterPts[last - 2].X, PxInterPts[last - 3].Y);
                                }
                                else
                                {
                                    while (PxInterPts.Count < 4)
                                    {
                                        isNewPtsAdded = true;
                                        PxInterPts.Add(new Point(0, 0));
                                    }
                                    int last = PxInterPts.Count;
                                    PxInterPts[last - 1] = new Point(ConnectionTailPort.PagePosition.X, ConnectionTailPort.PagePosition.Y + tailDist);
                                    PxInterPts[last - 2] = new Point(ConnectionTailPort.PagePosition.X - tailDist, PxInterPts[last - 1].Y);
                                    PxInterPts[last - 3] = new Point(PxInterPts[last - 2].X, PxInterPts[last - 3].Y);
                                }
                            }
                            //Update Tail (Top)
                            else
                            {
                                if (PxInterPts.Count >= 4 && !isNewPtsAdded)
                                {
                                    int last = PxInterPts.Count;
                                    if (isTailRecal || PxInterPts[last - 1].Y > TailNode.Top)
                                    {
                                        PxInterPts[last - 1] = new Point(ConnectionTailPort.PagePosition.X, ConnectionTailPort.PagePosition.Y - tailDist);
                                    }
                                    else
                                    {
                                        PxInterPts[last - 1] = new Point(ConnectionTailPort.PagePosition.X, PxInterPts[last - 1].Y);
                                    }
                                    PxInterPts[last - 2] = new Point(PxInterPts[last - 2].X, PxInterPts[last - 1].Y);
                                    PxInterPts[last - 3] = new Point(PxInterPts[last - 2].X, PxInterPts[last - 3].Y);
                                }
                                else
                                {
                                    while (PxInterPts.Count < 4)
                                    {
                                        isNewPtsAdded = true;
                                        PxInterPts.Add(new Point(0, 0));
                                    }
                                    int last = PxInterPts.Count;
                                    PxInterPts[last - 1] = new Point(ConnectionTailPort.PagePosition.X, ConnectionTailPort.PagePosition.Y - tailDist);
                                    PxInterPts[last - 2] = new Point(ConnectionTailPort.PagePosition.X - tailDist, PxInterPts[last - 1].Y);
                                    PxInterPts[last - 3] = new Point(PxInterPts[last - 2].X, PxInterPts[last - 3].Y);
                                }
                            }
                        }
                        #endregion

                        #region 2 Points Required
                        //required 2 inter pts.
                        else
                        {
                            bool isNewPtsAdded = false;
                            if (HeadDirection == Dock.Bottom)
                            {
                                if (PxInterPts.Count >= 2)
                                {
                                    int last = PxInterPts.Count;
                                    if (isHeadRecal || PxInterPts[0].Y < HeadNode.Bottom)
                                    {
                                        PxInterPts[0] = new Point(ConnectionHeadPort.PagePosition.X, ConnectionHeadPort.PagePosition.Y + headDist);
                                    }
                                    else
                                    {
                                        PxInterPts[0] = new Point(ConnectionHeadPort.PagePosition.X, PxInterPts[0].Y);
                                    }
                                    PxInterPts[1] = new Point(PxInterPts[1].X, PxInterPts[0].Y);
                                }
                                else
                                {
                                    while (PxInterPts.Count < 2)
                                    {
                                        isNewPtsAdded = true;
                                        PxInterPts.Add(new Point(0, 0));
                                    }
                                    PxInterPts[0] = new Point(ConnectionHeadPort.PagePosition.X, ConnectionHeadPort.PagePosition.Y + headDist);
                                    PxInterPts[1] = new Point(PxInterPts[1].X, PxInterPts[0].Y);
                                }
                            }
                            else
                            {
                                if (PxInterPts.Count >= 2)
                                {
                                    if (isHeadRecal || PxInterPts[0].Y > HeadNode.Top)
                                    {
                                        PxInterPts[0] = new Point(ConnectionHeadPort.PagePosition.X, ConnectionHeadPort.PagePosition.Y - headDist);
                                    }
                                    else
                                    {
                                        PxInterPts[0] = new Point(ConnectionHeadPort.PagePosition.X, PxInterPts[0].Y);
                                    }
                                    PxInterPts[1] = new Point(PxInterPts[1].X, PxInterPts[0].Y);
                                }

                                else
                                {
                                    while (PxInterPts.Count < 2)
                                    {
                                        isNewPtsAdded = true;
                                        PxInterPts.Add(new Point(0, 0));
                                    }
                                    PxInterPts[0] = new Point(ConnectionHeadPort.PagePosition.X, ConnectionHeadPort.PagePosition.Y - headDist);
                                    PxInterPts[1] = new Point(PxInterPts[1].X, PxInterPts[0].Y);
                                }
                            }

                            if (TailDirection == Dock.Bottom)
                            {
                                if ((PxInterPts.Count >= 2) && !isNewPtsAdded)
                                {
                                    int last = PxInterPts.Count;
                                    if ((isTailRecal && (PxInterPts.Count > 2 || (ConnectionHeadPort.PagePosition.Y + headDist < ConnectionTailPort.PagePosition.Y + tailDist))) || PxInterPts[last - 1].Y < TailNode.Bottom)
                                    {
                                        PxInterPts[last - 1] = new Point(ConnectionTailPort.PagePosition.X, ConnectionTailPort.PagePosition.Y + tailDist);
                                    }
                                    else
                                    {
                                        PxInterPts[last - 1] = new Point(ConnectionTailPort.PagePosition.X, PxInterPts[last - 1].Y);
                                    }
                                    PxInterPts[last - 2] = new Point(PxInterPts[last - 2].X, PxInterPts[last - 1].Y);
                                }

                                else
                                {
                                    while (PxInterPts.Count < 2)
                                    {
                                        PxInterPts.Add(new Point(0, 0));
                                    }
                                    int last = PxInterPts.Count;
                                    PxInterPts[last - 1] = new Point(ConnectionTailPort.PagePosition.X, ConnectionTailPort.PagePosition.Y + tailDist);
                                    PxInterPts[last - 2] = new Point(PxInterPts[last - 2].X, PxInterPts[last - 1].Y);
                                }
                            }
                            else
                            {
                                if ((PxInterPts.Count >= 2) && !isNewPtsAdded)
                                {
                                    int last = PxInterPts.Count;
                                    if ((isTailRecal && (PxInterPts.Count > 2 || (ConnectionHeadPort.PagePosition.Y + headDist > ConnectionTailPort.PagePosition.Y + tailDist))) || PxInterPts[last - 1].Y > TailNode.Top)
                                    {
                                        PxInterPts[last - 1] = new Point(ConnectionTailPort.PagePosition.X, ConnectionTailPort.PagePosition.Y - tailDist);
                                    }
                                    else
                                    {
                                        PxInterPts[last - 1] = new Point(ConnectionTailPort.PagePosition.X, PxInterPts[last - 1].Y);
                                    }
                                    PxInterPts[last - 2] = new Point(PxInterPts[last - 2].X, PxInterPts[last - 1].Y);
                                }

                                else
                                {
                                    while (PxInterPts.Count < 2)
                                    {
                                        PxInterPts.Add(new Point(0, 0));
                                    }
                                    int last = PxInterPts.Count;
                                    PxInterPts[last - 1] = new Point(ConnectionTailPort.PagePosition.X, ConnectionTailPort.PagePosition.Y - tailDist);
                                    PxInterPts[last - 2] = new Point(PxInterPts[last - 2].X, PxInterPts[last - 1].Y);
                                }
                            }
                        }

                        #endregion
                    }
                    // Horizontal - Vertical
                    else if (((HeadDirection == Dock.Top || HeadDirection == Dock.Bottom) && ((TailDirection == Dock.Left) || (TailDirection == Dock.Right))) ||
                       ((TailDirection == Dock.Top || TailDirection == Dock.Bottom) && ((HeadDirection == Dock.Left) || (HeadDirection == Dock.Right))))
                    {
                        #region init

                        Point HeadPortPos = ConnectionHeadPort.PagePosition;
                        Point TailPortPos = ConnectionTailPort.PagePosition;
                        if ((PxInterPts.Count == 1) && ((HeadDirection == Dock.Right && TailDirection == Dock.Bottom && HeadPortPos.X < TailPortPos.X && HeadPortPos.Y > TailPortPos.Y) ||
                            (HeadDirection == Dock.Right && TailDirection == Dock.Top && HeadPortPos.X < TailPortPos.X && HeadPortPos.Y < TailPortPos.Y) ||
                            (HeadDirection == Dock.Left && TailDirection == Dock.Top && HeadPortPos.X > TailPortPos.X && HeadPortPos.Y < TailPortPos.Y) ||
                            (HeadDirection == Dock.Left && TailDirection == Dock.Bottom && HeadPortPos.X > TailPortPos.X && HeadPortPos.Y > TailPortPos.Y)
                            ))
                        {
                            PxInterPts[0] = new Point(TailPortPos.X, HeadPortPos.Y);
                        }
                        else if ((PxInterPts.Count == 1) && ((TailDirection == Dock.Right && HeadDirection == Dock.Bottom && HeadPortPos.X > TailPortPos.X && HeadPortPos.Y < TailPortPos.Y) ||
                            (TailDirection == Dock.Right && HeadDirection == Dock.Top && HeadPortPos.X > TailPortPos.X && HeadPortPos.Y > TailPortPos.Y) ||
                            (TailDirection == Dock.Left && HeadDirection == Dock.Top && HeadPortPos.X < TailPortPos.X && HeadPortPos.Y > TailPortPos.Y) ||
                            (TailDirection == Dock.Left && HeadDirection == Dock.Bottom && HeadPortPos.X < TailPortPos.X && HeadPortPos.Y < TailPortPos.Y)
                            ))
                        {
                            PxInterPts[0] = new Point(HeadPortPos.X, TailPortPos.Y);
                        }
                        else
                        {
                            bool isReset = false;

                            if (!(PxInterPts.Count >= 3))
                            {
                                isReset = true;
                                while (PxInterPts.Count < 3)
                                {
                                    PxInterPts.Add(new Point(0, 0));
                                }
                            }
                            bool isNewPtsAdded = false;
                            bool isAddedAtHead = false;
                            if (PxInterPts.Count % 2 == 0)
                            {
                                isNewPtsAdded = true;
                                //If Vertical start
                                if (PxInterPts[0].Y == PxInterPts[1].Y)
                                {
                                    //If Horizontal port connection
                                    if (HeadDirection == Dock.Left || HeadDirection == Dock.Right)
                                    {
                                        PxInterPts.Insert(0, new Point(0, 0));
                                        isAddedAtHead = true;
                                    }
                                    else
                                    {
                                        PxInterPts.Insert(PxInterPts.Count - 1, new Point(0, 0));
                                    }
                                }
                                else
                                {
                                    //If Horizontal port connection
                                    if (HeadDirection == Dock.Top || HeadDirection == Dock.Bottom)
                                    {
                                        PxInterPts.Insert(0, new Point(0, 0));
                                        isAddedAtHead = true;
                                    }
                                    else
                                    {
                                        PxInterPts.Insert(PxInterPts.Count - 1, new Point(0, 0));
                                    }
                                }
                            }

                        #endregion

                            #region Head region
                            if (HeadDirection == Dock.Left)
                            {
                                if (isHeadRecal || (PxInterPts[0].X > HeadNode.Left) || (isNewPtsAdded && isAddedAtHead) || isReset)
                                {
                                    PxInterPts[0] = new Point(ConnectionHeadPort.PagePosition.X - headDist, ConnectionHeadPort.PagePosition.Y);
                                    PxInterPts[1] = new Point(PxInterPts[0].X, PxInterPts[1].Y);
                                }
                                else
                                {
                                    PxInterPts[0] = new Point(PxInterPts[0].X, ConnectionHeadPort.PagePosition.Y);
                                    PxInterPts[1] = new Point(PxInterPts[0].X, PxInterPts[1].Y);
                                }
                            }
                            else if (HeadDirection == Dock.Right)
                            {
                                if (isHeadRecal || (PxInterPts[0].X < HeadNode.Right) || (isNewPtsAdded && isAddedAtHead) || isReset)
                                {
                                    PxInterPts[0] = new Point(ConnectionHeadPort.PagePosition.X + headDist, ConnectionHeadPort.PagePosition.Y);
                                    PxInterPts[1] = new Point(PxInterPts[0].X, PxInterPts[1].Y);
                                }
                                else
                                {
                                    PxInterPts[0] = new Point(PxInterPts[0].X, ConnectionHeadPort.PagePosition.Y);
                                    PxInterPts[1] = new Point(PxInterPts[0].X, PxInterPts[1].Y);
                                }
                            }
                            else if (HeadDirection == Dock.Top)
                            {
                                if (isHeadRecal || (PxInterPts[0].Y > HeadNode.Top) || (isNewPtsAdded && isAddedAtHead) || isReset)
                                {
                                    PxInterPts[0] = new Point(ConnectionHeadPort.PagePosition.X, ConnectionHeadPort.PagePosition.Y - headDist);
                                    PxInterPts[1] = new Point(PxInterPts[1].X, PxInterPts[0].Y);
                                }
                                else
                                {
                                    PxInterPts[0] = new Point(ConnectionHeadPort.PagePosition.X, PxInterPts[0].Y);
                                    PxInterPts[1] = new Point(PxInterPts[1].X, PxInterPts[0].Y);
                                }
                            }
                            else if (HeadDirection == Dock.Bottom)
                            {
                                if (isHeadRecal || (PxInterPts[0].Y < HeadNode.Bottom) || (isNewPtsAdded && isAddedAtHead) || isReset)
                                {
                                    PxInterPts[0] = new Point(ConnectionHeadPort.PagePosition.X, ConnectionHeadPort.PagePosition.Y + headDist);
                                    PxInterPts[1] = new Point(PxInterPts[1].X, PxInterPts[0].Y);
                                }
                                else
                                {
                                    PxInterPts[0] = new Point(ConnectionHeadPort.PagePosition.X, PxInterPts[0].Y);
                                    PxInterPts[1] = new Point(PxInterPts[1].X, PxInterPts[0].Y);
                                }
                            }
                            #endregion

                            #region Tail region
                            int last = PxInterPts.Count - 1;

                            if (TailDirection == Dock.Left)
                            {
                                if (isTailRecal || (PxInterPts[last].X > TailNode.Left) || (isNewPtsAdded && !isAddedAtHead) || isReset)
                                {
                                    PxInterPts[last] = new Point(ConnectionTailPort.PagePosition.X - tailDist, ConnectionTailPort.PagePosition.Y);
                                    PxInterPts[last - 1] = new Point(PxInterPts[last].X, PxInterPts[last - 1].Y);
                                }
                                else
                                {
                                    PxInterPts[last] = new Point(PxInterPts[last].X, ConnectionTailPort.PagePosition.Y);
                                    PxInterPts[last - 1] = new Point(PxInterPts[last].X, PxInterPts[last - 1].Y);
                                }
                            }
                            else if (TailDirection == Dock.Right)
                            {
                                if (isTailRecal || (PxInterPts[last].X < TailNode.Right) || (isNewPtsAdded && !isAddedAtHead) || isReset)
                                {
                                    PxInterPts[last] = new Point(ConnectionTailPort.PagePosition.X + tailDist, ConnectionTailPort.PagePosition.Y);
                                    PxInterPts[last - 1] = new Point(PxInterPts[last].X, PxInterPts[last - 1].Y);
                                }
                                else
                                {
                                    PxInterPts[last] = new Point(PxInterPts[last].X, ConnectionTailPort.PagePosition.Y);
                                    PxInterPts[last - 1] = new Point(PxInterPts[last].X, PxInterPts[last - 1].Y);
                                }
                            }
                            else if ((TailDirection == Dock.Top))
                            {
                                if (isTailRecal || (PxInterPts[last].Y > TailNode.Top) || (isNewPtsAdded && !isAddedAtHead) || isReset)
                                {
                                    PxInterPts[last] = new Point(ConnectionTailPort.PagePosition.X, ConnectionTailPort.PagePosition.Y - tailDist);
                                    PxInterPts[last - 1] = new Point(PxInterPts[last - 2].X, PxInterPts[last].Y);
                                }
                                else
                                {
                                    PxInterPts[last] = new Point(ConnectionTailPort.PagePosition.X, PxInterPts[last].Y);
                                    PxInterPts[last - 1] = new Point(PxInterPts[last - 1].X, PxInterPts[last].Y);
                                }
                            }
                            else if ((TailDirection == Dock.Bottom))
                            {
                                if (isTailRecal || PxInterPts[last].Y < TailNode.Bottom || (isNewPtsAdded && !isAddedAtHead) || isReset)
                                {
                                    PxInterPts[last] = new Point(ConnectionTailPort.PagePosition.X, ConnectionTailPort.PagePosition.Y + tailDist);
                                    PxInterPts[last - 1] = new Point(PxInterPts[last - 2].X, PxInterPts[last].Y);
                                }
                                else
                                {
                                    PxInterPts[last] = new Point(ConnectionTailPort.PagePosition.X, PxInterPts[last].Y);
                                    PxInterPts[last - 1] = new Point(PxInterPts[last - 1].X, PxInterPts[last].Y);
                                }
                            }

                            #endregion
                        }
                    }
                }
                IntermediatePoints.Clear();
                foreach (Point pt in PxInterPts)
                {
                    IntermediatePoints.Add(pt);//MeasureUnitsConverter.FromPixels(pt, this.MeasurementUnit));
                }
                connectionPoints.Clear();
                connectionPoints.Add(ConnectionHeadPort.PagePosition);//MeasureUnitsConverter.FromPixels(ConnectionHeadPort.PagePosition, this.MeasurementUnit));
                connectionPoints.Add(ConnectionTailPort.PagePosition);//MeasureUnitsConverter.FromPixels(ConnectionTailPort.PagePosition, this.MeasurementUnit));
                return connectionPoints;
            }

            //if (((HeadNode != null) || (TailNode != null)))
            //{
            //    List<Point> tmp = GetTerminalPoints();
            //    if ((HeadNode != null) && (TailNode == null))
            //        connectionPoints[0] = tmp[0];
            //    else if ((HeadNode == null) && (TailNode != null))
            //        connectionPoints[1] = tmp[1];
            //}

            /*
            if (IntermediatePoints == null || IntermediatePoints.Count == 0)
            {
                IntermediatePoints = new List<Point>();
                if (this.ConnectorType == ConnectorType.Orthogonal)
                {
                    double ten = MeasureUnitsConverter.FromPixels(10, this.MeasurementUnit);
                    IntermediatePoints.Add(new Point(connectionPoints[0].X + ten, connectionPoints[0].Y));
                    IntermediatePoints.Add(new Point(IntermediatePoints[0].X, connectionPoints[1].Y));
                    //for (int i = IntermediatePoints.Count - 1; i >= 0; i--)
                    //{
                    //    connectionPoints.Insert(1, IntermediatePoints[i]);
                    //}
                }
            }*/

            if (IntermediatePoints.Count <= 2)
            {
                if (IntermediatePoints.Count == 1)
                {
                    divideOrthognalLine();
                }
                else if (IntermediatePoints.Count == 2)
                {
                    defaultStyle();
                }
                connectionPoints = GetTerminalPoints();
            }
            if (connectionPoints.Count == 2 && IntermediatePoints.Count >= 2)
            {
                Point tempEnd = connectionPoints[1];
                Point tempStart = connectionPoints[0];
                if (IntermediatePoints.Count > 2)
                {
                    if (Math.Abs(IntermediatePoints[1].X - IntermediatePoints[0].X)
                        < Math.Abs(IntermediatePoints[1].Y - IntermediatePoints[0].Y))
                    {
                        if (tempStart.Y != 0)
                            IntermediatePoints[0] = new Point(IntermediatePoints[0].X, tempStart.Y);
                        if (HeadNode != null)
                        {
                            NodeInfo head;
                            Rect sourceRect = getNodeRect(HeadNode as Node);
                            head = getRectAsNodeInfo(sourceRect);

                            if (head.Position.X - (cs * 2 + head.Size.Width / 2) < IntermediatePoints[0].X
                                && IntermediatePoints[0].X < head.Position.X + (cs * 2 + head.Size.Width / 2))
                            {
                                if (IntermediatePoints[1].Y > head.Position.Y)
                                {
                                    /*connectionPoints[0] = */
                                    moveAway(connectionPoints[0], head.Position, true, true);
                                }
                                else
                                {
                                    /*connectionPoints[0] = */
                                    moveAway(connectionPoints[0], head.Position, true, true);
                                }
                            }
                        }
                    }
                    else if (Math.Abs(IntermediatePoints[1].X - IntermediatePoints[0].X)
                        == Math.Abs(IntermediatePoints[1].Y - IntermediatePoints[0].Y))
                    {
                        if (Math.Abs(tempStart.X - IntermediatePoints[0].X)
                        < Math.Abs(tempStart.Y - IntermediatePoints[0].Y))
                        {
                            IntermediatePoints[0] = new Point(tempStart.X, IntermediatePoints[0].Y);
                        }
                        else
                        {
                            IntermediatePoints[0] = new Point(IntermediatePoints[0].X, tempStart.Y);
                        }

                    }
                    else
                    {
                        if (tempStart.X != 0)
                            IntermediatePoints[0] = new Point(tempStart.X, IntermediatePoints[0].Y);
                        if (HeadNode != null)
                        {
                            NodeInfo head;
                            Rect sourceRect = getNodeRect(HeadNode as Node);
                            head = getRectAsNodeInfo(sourceRect);

                            if (sourceRect != Rect.Empty)
                            {
                                sourceRect = new Rect(head.Left, head.Top, head.Size.Width, head.Size.Height);
                            }
                            if (head.Position.Y - (cs * 2 + head.Size.Height / 2) < IntermediatePoints[0].Y
                                && IntermediatePoints[0].Y < head.Position.Y + (cs * 2 + head.Size.Height / 2))
                            {
                                if (IntermediatePoints[1].X > head.Position.X)
                                {
                                    /*connectionPoints[0] = */
                                    moveAway(connectionPoints[0], head.Position, false, true);
                                }
                                else
                                {
                                    /*connectionPoints[0] = */
                                    moveAway(connectionPoints[0], head.Position, false, true);
                                }
                            }
                        }
                    }
                    if (Math.Abs(IntermediatePoints[IntermediatePoints.Count - 2].X - IntermediatePoints[IntermediatePoints.Count - 1].X)
                        < Math.Abs(IntermediatePoints[IntermediatePoints.Count - 2].Y - IntermediatePoints[IntermediatePoints.Count - 1].Y))
                    {

                        if (tempEnd.Y != 0)
                            IntermediatePoints[IntermediatePoints.Count - 1] = new Point(IntermediatePoints[IntermediatePoints.Count - 1].X, tempEnd.Y);

                        if (TailNode != null)
                        {
                            NodeInfo tail;
                            Rect sourceRect = getNodeRect(TailNode as Node);
                            tail = getRectAsNodeInfo(sourceRect);

                            if (sourceRect != Rect.Empty)
                            {
                                sourceRect = new Rect(tail.Left, tail.Top, tail.Size.Width, tail.Size.Height);
                            }

                            if (tail.Position.X - (cs * 2 + tail.Size.Width / 2) < IntermediatePoints[IntermediatePoints.Count - 1].X
                                && IntermediatePoints[IntermediatePoints.Count - 1].X < tail.Position.X + (cs * 2 + tail.Size.Width / 2))
                            {
                                if (IntermediatePoints[IntermediatePoints.Count - 2].Y > tail.Position.Y)
                                {
                                    /*connectionPoints[1] = */
                                    moveAway(connectionPoints[1], tail.Position, true, false);
                                }
                                else
                                {
                                    /*connectionPoints[1] = */
                                    moveAway(connectionPoints[1], tail.Position, true, false);
                                }
                            }
                        }
                    }
                    else if (Math.Abs(IntermediatePoints[IntermediatePoints.Count - 2].X - IntermediatePoints[IntermediatePoints.Count - 1].X)
                        == Math.Abs(IntermediatePoints[IntermediatePoints.Count - 2].Y - IntermediatePoints[IntermediatePoints.Count - 1].Y))
                    {
                        if (Math.Abs(tempEnd.X - IntermediatePoints[IntermediatePoints.Count - 1].X)
                        < Math.Abs(tempEnd.Y - IntermediatePoints[IntermediatePoints.Count - 1].Y))
                        {
                            IntermediatePoints[IntermediatePoints.Count - 1] = new Point(tempEnd.X, IntermediatePoints[IntermediatePoints.Count - 1].Y);
                        }
                        else
                        {
                            IntermediatePoints[IntermediatePoints.Count - 1] = new Point(IntermediatePoints[IntermediatePoints.Count - 1].X, tempEnd.Y);
                        }
                    }
                    else
                    {
                        if (tempEnd.X != 0)
                            IntermediatePoints[IntermediatePoints.Count - 1] = new Point(tempEnd.X, IntermediatePoints[IntermediatePoints.Count - 1].Y);

                        if (TailNode != null)
                        {
                            NodeInfo tail;
                            Rect sourceRect = getNodeRect(TailNode as Node);
                            tail = getRectAsNodeInfo(sourceRect);

                            if (sourceRect != Rect.Empty)
                            {
                                sourceRect = new Rect(tail.Left, tail.Top, tail.Size.Width, tail.Size.Height);
                            }
                            if (tail.Position.Y - (cs * 2 + tail.Size.Height / 2) < IntermediatePoints[IntermediatePoints.Count - 1].Y
                                && IntermediatePoints[IntermediatePoints.Count - 1].Y < tail.Position.Y + (cs * 2 + tail.Size.Height / 2))
                            {
                                if (IntermediatePoints[IntermediatePoints.Count - 2].X > tail.Position.X)
                                {
                                    /*connectionPoints[1] = */
                                    moveAway(connectionPoints[1], tail.Position, false, false);
                                }
                                else
                                {
                                    /*connectionPoints[1] = */
                                    moveAway(connectionPoints[1], tail.Position, false, false);
                                }
                            }
                        }
                    }
                }

                else if (IntermediatePoints.Count == 2)
                {
                    //List<Point> sten = defaultStyle();
                    //connectionPoints = GetTerminalPoints();
                }
                ////only one intermediate point
                else
                {
                    if (Math.Abs(tempEnd.X - IntermediatePoints[0].X)
                        < Math.Abs(tempEnd.Y - IntermediatePoints[0].Y))
                    {
                        if (tempStart.Y != 0)
                            IntermediatePoints[0] = new Point(IntermediatePoints[0].X, tempStart.Y);
                    }
                    else if (Math.Abs(tempEnd.X - IntermediatePoints[0].X)
                        == Math.Abs(tempEnd.Y - IntermediatePoints[0].Y))
                    {
                        if (Math.Abs(tempStart.X - IntermediatePoints[IntermediatePoints.Count - 1].X)
                        < Math.Abs(tempStart.Y - IntermediatePoints[IntermediatePoints.Count - 1].Y))
                        {
                            tempStart.X++;
                            IntermediatePoints[IntermediatePoints.Count - 1] = new Point(IntermediatePoints[IntermediatePoints.Count - 1].X + 1, IntermediatePoints[IntermediatePoints.Count - 1].Y);
                        }
                        else if (Math.Abs(tempStart.X - IntermediatePoints[IntermediatePoints.Count - 1].X)
                        == Math.Abs(tempStart.Y - IntermediatePoints[IntermediatePoints.Count - 1].Y))
                        {
                        }
                        else
                        {
                            tempStart.X--;
                            IntermediatePoints[IntermediatePoints.Count - 1] = new Point(IntermediatePoints[IntermediatePoints.Count - 1].X, IntermediatePoints[IntermediatePoints.Count - 1].Y - 1);
                        }
                    }
                    else
                    {
                        if (tempStart.X != 0)
                            IntermediatePoints[0] = new Point(tempStart.X, IntermediatePoints[0].Y);
                    }

                    if (Math.Abs(tempStart.X - IntermediatePoints[IntermediatePoints.Count - 1].X)
                        < Math.Abs(tempStart.Y - IntermediatePoints[IntermediatePoints.Count - 1].Y))
                    {

                        if (tempEnd.Y != 0)
                            IntermediatePoints[IntermediatePoints.Count - 1] = new Point(IntermediatePoints[IntermediatePoints.Count - 1].X, tempEnd.Y);
                    }
                    else if (Math.Abs(tempStart.X - IntermediatePoints[IntermediatePoints.Count - 1].X)
                        == Math.Abs(tempStart.Y - IntermediatePoints[IntermediatePoints.Count - 1].Y))
                    {
                        if (Math.Abs(tempEnd.X - IntermediatePoints[0].X)
                            < Math.Abs(tempEnd.Y - IntermediatePoints[0].Y))
                        {
                            tempEnd.X++;
                            IntermediatePoints[IntermediatePoints.Count - 1] = new Point(IntermediatePoints[IntermediatePoints.Count - 1].X + 1, IntermediatePoints[IntermediatePoints.Count - 1].Y);
                        }
                        else if (Math.Abs(tempEnd.X - IntermediatePoints[0].X)
                            == Math.Abs(tempEnd.Y - IntermediatePoints[0].Y))
                        {
                        }
                        else
                        {
                            tempEnd.X--;
                            IntermediatePoints[IntermediatePoints.Count - 1] = new Point(IntermediatePoints[IntermediatePoints.Count - 1].X, IntermediatePoints[IntermediatePoints.Count - 1].Y - 1);
                        }
                    }
                    else
                    {
                        if (tempEnd.X != 0)
                            IntermediatePoints[IntermediatePoints.Count - 1] = new Point(tempEnd.X, IntermediatePoints[IntermediatePoints.Count - 1].Y);
                    }
                }
            }
            else
            {
            }
            return connectionPoints;
        }

        private void fixAdjacent()
        {
            if (IntermediatePoints != null && IntermediatePoints.Count > 2)
            {
                if (HeadNode != null)
                {
                    NodeInfo h;
                    Rect sourceRect = getNodeRect(HeadNode as Node);
                    h = getRectAsNodeInfo(sourceRect);
                    if (IntermediatePoints[1].X == IntermediatePoints[2].X)
                    {
                        IntermediatePoints[0] = new Point(h.Position.X, IntermediatePoints[1].Y);
                    }
                    else if (IntermediatePoints[1].Y == IntermediatePoints[2].Y)
                    {
                        IntermediatePoints[0] = new Point(IntermediatePoints[1].X, h.Position.Y);
                    }
                }
                if (TailNode != null)
                {
                    NodeInfo t;
                    Rect sourceRect = getNodeRect(HeadNode as Node);
                    t = getRectAsNodeInfo(sourceRect);
                    int c = IntermediatePoints.Count - 2;
                    if (IntermediatePoints[c].X == IntermediatePoints[c - 1].X)
                    {
                        IntermediatePoints[c + 1] = new Point(t.Position.X, IntermediatePoints[c].Y);
                    }
                    else if (IntermediatePoints[c].Y == IntermediatePoints[c - 1].Y)
                    {
                        IntermediatePoints[c + 1] = new Point(IntermediatePoints[c].X, t.Position.Y);
                    }
                }
            }
        }

        private Point moveAway(Point mov, Point fix, bool x, bool head)
        {
            double del = 20;// MeasureUnitsConverter.FromPixels(20, this.MeasurementUnit);
            if (x)
            {
                if (head)
                {
                    if ((fix.X - IntermediatePoints[0].X) < 0)
                    {
                        mov = new Point(mov.X + 2 * (fix.X - mov.X), mov.Y);
                        IntermediatePoints[0] = new Point(mov.X - del, IntermediatePoints[0].Y);
                        IntermediatePoints[1] = new Point(mov.X - del, IntermediatePoints[1].Y);
                    }
                    else
                    {
                        mov = new Point(mov.X + 2 * (fix.X - mov.X), mov.Y);
                        IntermediatePoints[0] = new Point(mov.X + del, IntermediatePoints[0].Y);
                        IntermediatePoints[1] = new Point(mov.X + del, IntermediatePoints[1].Y);
                    }
                }
                else
                {
                    int f = IntermediatePoints.Count - 1;
                    if ((fix.X - IntermediatePoints[f].X) < 0)
                    {
                        mov = new Point(mov.X + 2 * (fix.X - mov.X), mov.Y);
                        IntermediatePoints[f] = new Point(mov.X - del, IntermediatePoints[f].Y);
                        IntermediatePoints[f - 1] = new Point(mov.X - del, IntermediatePoints[f - 1].Y);
                    }
                    else
                    {
                        mov = new Point(mov.X + 2 * (fix.X - mov.X), mov.Y);
                        IntermediatePoints[f] = new Point(mov.X + del, IntermediatePoints[f].Y);
                        IntermediatePoints[f - 1] = new Point(mov.X + del, IntermediatePoints[f - 1].Y);
                    }
                }
            }
            else
            {
                if (head)
                {
                    if ((fix.Y - IntermediatePoints[0].Y) < 0)
                    {
                        mov = new Point(mov.X, mov.Y + 2 * (fix.Y - mov.Y));
                        IntermediatePoints[0] = new Point(IntermediatePoints[0].X, mov.Y - del);
                        IntermediatePoints[1] = new Point(IntermediatePoints[1].X, mov.Y - del);
                    }
                    else
                    {
                        mov = new Point(mov.X, mov.Y + 2 * (fix.Y - mov.Y));
                        IntermediatePoints[0] = new Point(IntermediatePoints[0].X, mov.Y + del);
                        IntermediatePoints[1] = new Point(IntermediatePoints[1].X, mov.Y + del);
                    }
                }
                else
                {
                    int f = IntermediatePoints.Count - 1;
                    if ((fix.Y - IntermediatePoints[f].Y) < 0)
                    {
                        mov = new Point(mov.X, mov.Y + 2 * (fix.Y - mov.Y));
                        IntermediatePoints[f] = new Point(IntermediatePoints[f].X, mov.Y - del);
                        IntermediatePoints[f - 1] = new Point(IntermediatePoints[f - 1].X, mov.Y - del);
                    }
                    else
                    {
                        mov = new Point(mov.X, mov.Y + 2 * (fix.Y - mov.Y));
                        IntermediatePoints[f] = new Point(IntermediatePoints[f].X, mov.Y + del);
                        IntermediatePoints[f - 1] = new Point(IntermediatePoints[f - 1].X, mov.Y + del);
                    }
                }
            }
            return mov;
        }

        //internal void doRunTimeUnitChange()
        //{
        //    if (HeadNode == null)
        //    {
        //        //StartPointPosition = MeasureUnitsConverter.ToPixels(StartPointPosition, currentMUSelected);
        //        //StartPointPosition = MeasureUnitsConverter.FromPixels(StartPointPosition, this.MeasurementUnit);
        //    }
        //    if (TailNode == null)
        //    {
        //        //EndPointPosition = MeasureUnitsConverter.ToPixels(EndPointPosition, currentMUSelected);
        //        //EndPointPosition = MeasureUnitsConverter.FromPixels(EndPointPosition, this.MeasurementUnit);
        //    }
        //    if (IntermediatePoints != null)
        //    {
        //        for (int i = 0; i < IntermediatePoints.Count; i++)
        //        {
        //            //IntermediatePoints[i] = MeasureUnitsConverter.ToPixels(IntermediatePoints[i], currentMUSelected);
        //            //IntermediatePoints[i] = MeasureUnitsConverter.FromPixels(IntermediatePoints[i], this.MeasurementUnit);
        //        }
        //    }
        //    //currentMUSelected = this.MeasurementUnit;
        //}

        private void defaultStyle()
        {
            double fifty = 50;// MeasureUnitsConverter.FromPixels(50, this.MeasurementUnit);
            double thirty = 30;// MeasureUnitsConverter.FromPixels(30, this.MeasurementUnit);
            List<Point> terms = new List<Point>();

            if (HeadNode != null && TailNode != null)
            {
                NodeInfo head;
                NodeInfo tail;

                //(HeadNode as Node).refreshBoundaries();
                //(TailNode as Node).refreshBoundaries();

                Rect h = getNodeRect(HeadNode as Node);
                Rect t = getNodeRect(TailNode as Node);

                head = getRectAsNodeInfo(h);
                tail = getRectAsNodeInfo(t);

                Point actualHeadPos = head.Position;
                Point actualTailPos = tail.Position;

                if (dview != null && dview.Page != null)
                {
                    if (this.ConnectionHeadPort != null && HeadNode != null)
                    {
                        head.Position = new Point(ConnectionHeadPort.CenterPosition.X, ConnectionHeadPort.CenterPosition.Y);
                        head.Position = (HeadNode as Node).TranslatePoint(head.Position, dview.Page);
                        head.Position = head.Position;// MeasureUnitsConverter.FromPixels(head.Position, this.MeasurementUnit);
                    }
                    if (this.ConnectionTailPort != null && TailNode != null)
                    {
                        tail.Position = new Point(ConnectionTailPort.CenterPosition.X, ConnectionTailPort.CenterPosition.Y);
                        tail.Position = (TailNode as Node).TranslatePoint(tail.Position, dview.Page);
                        tail.Position = tail.Position;// MeasureUnitsConverter.FromPixels(tail.Position, this.MeasurementUnit);
                    }
                }
                ////Top-Bottom
                if ((Math.Abs(head.Position.Y - tail.Position.Y) > head.Size.Height / 2 + tail.Size.Height / 2 + fifty)
                    && (
                    Orientation == TreeOrientation.TopBottom || Orientation == TreeOrientation.BottomTop))
                {
                    ////Top-to-Bottom
                    if (head.Position.Y - tail.Position.Y < 0)
                    {
                        IntermediatePoints[0] = new Point(head.Position.X, actualHeadPos.Y + head.Size.Height / 2 + thirty);
                        IntermediatePoints[1] = new Point(tail.Position.X, IntermediatePoints[0].Y);
                        //StartPointPosition = new Point(head.Position.X, head.Position.Y + head.Size.Height / 2);
                        //EndPointPosition = new Point(tail.Position.X, tail.Position.Y - tail.Size.Height / 2);
                    }
                    ////Bottom-to-Top
                    else
                    {
                        IntermediatePoints[0] = new Point(head.Position.X, actualHeadPos.Y - head.Size.Height / 2 - thirty);
                        IntermediatePoints[1] = new Point(tail.Position.X, IntermediatePoints[0].Y);
                        //StartPointPosition = new Point(head.Position.X, head.Position.Y - head.Size.Height / 2);
                        //EndPointPosition = new Point(tail.Position.X, tail.Position.Y + tail.Size.Height / 2);
                    }

                }
                ////Right-Left
                else
                {
                    ////Left-to-Right
                    if (head.Position.X - tail.Position.X < 0)
                    {
                        IntermediatePoints[0] = new Point(actualHeadPos.X + head.Size.Width / 2 + thirty, head.Position.Y);
                        IntermediatePoints[1] = new Point(IntermediatePoints[0].X, tail.Position.Y);
                        if (Math.Abs((head.Position.X + head.Size.Width / 2) - (tail.Position.X - tail.Size.Width / 2)) < fifty
                            || ((head.Position.X + head.Size.Width / 2) - (tail.Position.X - tail.Size.Width / 2) > 0)
                            )
                        {
                            IntermediatePoints[0] = new Point(
                                                                actualTailPos.X + tail.Size.Width / 2 + fifty, head.Position.Y);
                            IntermediatePoints[1] = new Point(IntermediatePoints[0].X, tail.Position.Y);
                        }
                        //StartPointPosition = new Point(head.Position.X + head.Size.Width / 2, head.Position.Y);
                        //EndPointPosition = new Point(tail.Position.X - tail.Size.Width / 2, tail.Position.Y);
                    }
                    ////Right-to-Left
                    else
                    {
                        IntermediatePoints[0] = new Point(actualHeadPos.X - head.Size.Width / 2 - thirty, head.Position.Y);
                        IntermediatePoints[1] = new Point(IntermediatePoints[0].X, tail.Position.Y);

                        if (Math.Abs((head.Position.X - head.Size.Width / 2) - (tail.Position.X + tail.Size.Width / 2)) < fifty
                            || ((head.Position.X - head.Size.Width / 2) - (tail.Position.X + tail.Size.Width / 2) < 0)
                            )
                        {
                            IntermediatePoints[0] = new Point(
                                                                actualTailPos.X - tail.Size.Width / 2 - fifty, head.Position.Y);
                            IntermediatePoints[1] = new Point(IntermediatePoints[0].X, tail.Position.Y);
                        }
                        //StartPointPosition = new Point(head.Position.X - head.Size.Width / 2, head.Position.Y);
                        //EndPointPosition = new Point(tail.Position.X + tail.Size.Width / 2, tail.Position.Y);
                    }
                }
            }
            else if (HeadNode != null && TailNode == null)
            {
                NodeInfo head;
                Rect sourceRect = getNodeRect(HeadNode as Node);
                head = getRectAsNodeInfo(sourceRect);
                Point actualHeadPos = head.Position;
                if (dview != null && dview.Page != null)
                {
                    if (this.ConnectionHeadPort != null && HeadNode != null)
                    {
                        head.Position = new Point(ConnectionHeadPort.CenterPosition.X, ConnectionHeadPort.CenterPosition.Y);
                        head.Position = (HeadNode as Node).TranslatePoint(head.Position, dview.Page);
                        //head.Position = MeasureUnitsConverter.FromPixels(head.Position, this.MeasurementUnit);
                    }
                }
                Point tail = PxEndPointPosition;
                ////Top-Bottom
                if (Math.Abs(head.Position.Y - tail.Y) > head.Size.Height / 2 + fifty)
                {
                    ////Top-to-Bottom
                    if (head.Position.Y - tail.Y < 0)
                    {
                        IntermediatePoints[0] = new Point(head.Position.X, actualHeadPos.Y + head.Size.Height / 2 + thirty);
                        IntermediatePoints[1] = new Point(tail.X, IntermediatePoints[0].Y);
                    }
                    ////Bottom-to-Top
                    else
                    {
                        IntermediatePoints[0] = new Point(head.Position.X, actualHeadPos.Y - head.Size.Height / 2 - thirty);
                        IntermediatePoints[1] = new Point(tail.X, IntermediatePoints[0].Y);
                    }
                }
                ////Right-Left
                else
                {
                    ////Left-to-Right
                    if (head.Position.X - tail.X < 0)
                    {
                        IntermediatePoints[0] = new Point(actualHeadPos.X + head.Size.Width / 2 + thirty, head.Position.Y);
                        IntermediatePoints[1] = new Point(IntermediatePoints[0].X, tail.Y);
                    }
                    ////Right-to-Left
                    else
                    {
                        IntermediatePoints[0] = new Point(actualHeadPos.X - head.Size.Width / 2 - thirty, head.Position.Y);
                        IntermediatePoints[1] = new Point(IntermediatePoints[0].X, tail.Y);
                    }
                }
            }
            else if (HeadNode == null && TailNode != null)
            {

                NodeInfo tail;
                Rect sourceRect = getNodeRect(TailNode as Node);
                tail = getRectAsNodeInfo(sourceRect);
                Point actualTailPos = tail.Position;
                if (dview != null && dview.Page != null)
                {
                    if (this.ConnectionTailPort != null && TailNode != null)
                    {
                        tail.Position = new Point(ConnectionTailPort.CenterPosition.X, ConnectionTailPort.CenterPosition.Y);
                        //tail.Position = (TailNode as Node).TranslatePoint(tail.Position, dview.Page);
                        //tail.Position = MeasureUnitsConverter.FromPixels(tail.Position, this.MeasurementUnit);
                    }
                }
                Point head = PxStartPointPosition;
                ////Top-Bottom
                if (Math.Abs(head.Y - tail.Position.Y) > tail.Size.Height / 2 + fifty)
                {
                    ////Top-to-Bottom
                    if (head.Y - tail.Position.Y < 0)
                    {
                        IntermediatePoints[0] = new Point(head.X, head.Y + thirty);
                        IntermediatePoints[1] = new Point(tail.Position.X, IntermediatePoints[0].Y);
                    }
                    ////Bottom-to-Top
                    else
                    {
                        IntermediatePoints[0] = new Point(head.X, head.Y - thirty);
                        IntermediatePoints[1] = new Point(tail.Position.X, IntermediatePoints[0].Y);
                    }
                }
                ////Right-Left
                else
                {
                    ////Left-to-Right
                    if (head.X - tail.Position.X < 0)
                    {
                        IntermediatePoints[0] = new Point(head.X + thirty, head.Y);
                        IntermediatePoints[1] = new Point(IntermediatePoints[0].X, tail.Position.Y);
                    }
                    ////Right-to-Left
                    else
                    {
                        IntermediatePoints[0] = new Point(head.X - thirty, head.Y);
                        IntermediatePoints[1] = new Point(IntermediatePoints[0].X, tail.Position.Y);
                    }
                }
            }
            else if (HeadNode == null && TailNode == null)
            {

                Point tail = PxEndPointPosition;
                Point head = PxStartPointPosition;
                ////Top-Bottom
                if (Math.Abs(head.Y - tail.Y) > fifty)
                {
                    ////Top-to-Bottom
                    if (head.Y - tail.Y < 0)
                    {
                        IntermediatePoints[0] = new Point(head.X, head.Y + thirty);
                        IntermediatePoints[1] = new Point(tail.X, IntermediatePoints[0].Y);
                    }
                    ////Bottom-to-Top
                    else
                    {
                        IntermediatePoints[0] = new Point(head.X, head.Y - thirty);
                        IntermediatePoints[1] = new Point(tail.X, IntermediatePoints[0].Y);
                    }
                }
                ////Right-Left
                else
                {
                    ////Left-to-Right
                    if (head.X - tail.X < 0)
                    {
                        IntermediatePoints[0] = new Point(head.X + thirty, head.Y);
                        IntermediatePoints[1] = new Point(IntermediatePoints[0].X, tail.Y);
                    }
                    ////Right-to-Left
                    else
                    {
                        IntermediatePoints[0] = new Point(head.X - thirty, head.Y);
                        IntermediatePoints[1] = new Point(IntermediatePoints[0].X, tail.Y);
                    }
                }
            }
        }

        private void divideOrthognalLine()
        {
            IntermediatePoints[0] = getOrthognalLine();
        }

        internal Point getOrthognalLine()
        {
            //double fifty = 50;// MeasureUnitsConverter.FromPixels(50, this.MeasurementUnit);
            //double thirty = 30;// MeasureUnitsConverter.FromPixels(30, this.MeasurementUnit);
            List<Point> terms = new List<Point>();

            Point ret = new Point(0, 0);

            if (HeadNode == null && TailNode == null)
            {
                if (Orientation == TreeOrientation.TopBottom)
                {
                    //IntermediatePoints[0] = new Point(StartPointPosition.X, EndPointPosition.Y);
                    ret = new Point(PxStartPointPosition.X, PxEndPointPosition.Y);
                }
                else
                {
                    //IntermediatePoints[0] = new Point(EndPointPosition.X, StartPointPosition.Y);
                    ret = new Point(PxEndPointPosition.X, PxStartPointPosition.Y);
                }
            }
            NodeInfo head = new NodeInfo();
            NodeInfo tail = new NodeInfo();

            Rect h = getNodeRect(HeadNode as Node);
            Rect t = getNodeRect(TailNode as Node);

            if (HeadNode == null)
            {
                head.Position = PxStartPointPosition;
            }
            else
            {
                head = getRectAsNodeInfo(h);
            }

            if (TailNode == null)
            {
                tail.Position = PxEndPointPosition;
            }
            else
            {
                tail = getRectAsNodeInfo(t);
            }

            Point actualHeadPos = head.Position;
            Point actualTailPos = tail.Position;

            if (dview != null && dview.Page != null)
            {
                if (this.ConnectionHeadPort != null && HeadNode != null)
                {
                    head.Position = new Point(ConnectionHeadPort.CenterPosition.X, ConnectionHeadPort.CenterPosition.Y);
                    head.Position = (HeadNode as Node).TranslatePoint(head.Position, dview.Page);
                    head.Position = head.Position;// MeasureUnitsConverter.FromPixels(head.Position, this.MeasurementUnit);
                }
                if (this.ConnectionTailPort != null && TailNode != null)
                {
                    tail.Position = new Point(ConnectionTailPort.CenterPosition.X, ConnectionTailPort.CenterPosition.Y);
                    tail.Position = (TailNode as Node).TranslatePoint(tail.Position, dview.Page);
                    tail.Position = tail.Position;// MeasureUnitsConverter.FromPixels(tail.Position, this.MeasurementUnit);
                }
            }

            Point hor = new Point(tail.Position.X, head.Position.Y);
            Point ver = new Point(head.Position.X, tail.Position.Y);

            if (HeadNode != null)
            {
                if (divideOrthoganalLine(HeadNode, TailNode, ConnectionHeadPort, ConnectionTailPort, head, tail, hor, ver))
                {
                    //IntermediatePoints[0] = hor;
                    ret = hor;
                }
                else
                {
                    //IntermediatePoints[0] = ver;
                    ret = ver;
                }
                //if (Math.Min(Math.Abs(hor.X - head.Position.X), Math.Abs(head.Position.X - head.Left)) >
                //    Math.Min(Math.Abs(ver.Y - head.Position.Y), Math.Abs(head.Position.Y - head.Top)))
                //{
                //    IntermediatePoints[0] = ver;
                //}
                //else
                //{
                //    IntermediatePoints[0] = hor;
                //}
            }
            else if (TailNode != null)
            {
                if (divideOrthoganalLine(TailNode, HeadNode, ConnectionTailPort, ConnectionHeadPort, tail, head, hor, ver))
                {
                    ret = hor;
                }
                else
                {
                    ret = ver;
                }
                ////Hor
                //if (Math.Min(Math.Abs(tail.Position.Y - tail.Top), Math.Abs(tail.Position.Y - (tail.Top + tail.Size.Height))) >
                //    Math.Min(Math.Abs(tail.Position.X - tail.Left), Math.Abs(tail.Position.X - (tail.Left + tail.Size.Width))))
                //{

                //}
                ////Vertical
                //else
                //{

                //}
            }
            return ret;
        }

        private bool divideOrthoganalLine(IShape HeadNode, IShape TailNode, ConnectionPort ConnectionHeadPort, ConnectionPort ConnectionTailPort, NodeInfo head, NodeInfo tail, Point hor, Point ver)
        {
            bool Horizontal = true;
            if (HeadNode != null && ConnectionHeadPort != null)
            {
                //Hor
                if (Math.Min(Math.Abs(head.Position.Y - head.Top), Math.Abs(head.Position.Y - (head.Top + head.Size.Height))) >
                    Math.Min(Math.Abs(head.Position.X - head.Left), Math.Abs(head.Position.X - (head.Left + head.Size.Width))))
                {
                    //Right
                    if (Math.Abs(head.Position.X - head.Left) > Math.Abs(head.Position.X - (head.Left + head.Size.Width)))
                    {
                        if (hor.X > head.Position.X)
                        {
                            //IntermediatePoints[0] = hor;
                            Horizontal = true;
                        }
                        else
                        {
                            //IntermediatePoints[0] = ver; 
                            Horizontal = false;
                        }
                    }
                    //Left
                    else
                    {
                        if (hor.X < head.Position.X)
                        {
                            //IntermediatePoints[0] = hor
                            Horizontal = true;
                        }
                        else
                        {
                            //IntermediatePoints[0] = ver;
                            Horizontal = false;
                        }
                    }
                }
                //Vertical
                else
                {
                    //Down
                    if (Math.Abs(head.Position.Y - head.Top) > Math.Abs(head.Position.Y - (head.Top + head.Size.Height)))
                    {
                        if (ver.Y > head.Position.Y)
                        {
                            //IntermediatePoints[0] = ver;
                            Horizontal = false;
                        }
                        else
                        {
                            //IntermediatePoints[0] = hor;
                            Horizontal = true;
                        }
                    }
                    //Up
                    else
                    {
                        if (ver.Y < head.Position.Y)
                        {
                            //IntermediatePoints[0] = ver;
                            Horizontal = false;
                        }
                        else
                        {
                            //IntermediatePoints[0] = hor;
                            Horizontal = true;
                        }
                    }
                }
            }
            else if (HeadNode != null)
            {
                if (TailNode != null && ConnectionTailPort != null)
                {
                    if ((head.BottomPoint.Y < tail.Position.Y) || (head.TopPoint.Y > tail.Position.Y))
                    {
                        //IntermediatePoints[0] = ver;
                        Horizontal = false;
                    }
                    else
                    {
                        //IntermediatePoints[0] = hor;
                        Horizontal = true;
                    }
                }
                else if (TailNode != null)
                {
                    //Bottom
                    if (head.BottomPoint.Y < tail.TopPoint.Y)
                    {
                        //IntermediatePoints[0] = ver;
                        Horizontal = false;
                    }
                    //Top
                    else if (head.TopPoint.Y > tail.BottomPoint.Y)
                    {
                        //IntermediatePoints[0] = ver;
                        Horizontal = false;
                    }
                    //Right Paralled
                    else if (head.RightPoint.X < tail.LeftPoint.X)
                    {
                        //IntermediatePoints[0] = hor;
                        Horizontal = true;
                    }
                    //Left Parallel
                    else if (head.LeftPoint.X > tail.RightPoint.X)
                    {
                        //IntermediatePoints[0] = hor;
                        Horizontal = true;
                    }
                    //Overlap
                    else
                    {
                        //IntermediatePoints[0] = hor;
                        Horizontal = true;
                    }
                }
                else
                {
                    if ((head.BottomPoint.Y < tail.Position.Y) || (head.TopPoint.Y > tail.Position.Y))
                    {
                        //IntermediatePoints[0] = ver;
                        Horizontal = false;
                    }
                    else
                    {
                        //IntermediatePoints[0] = hor;
                        Horizontal = true;
                    }
                }
            }
            return Horizontal;
        }

        internal NodeInfo getRectAsNodeInfo(Rect h)
        {
            NodeInfo head = new NodeInfo();
            if (Rect.Empty != h)
            {
                head.Left = h.Left;// MeasureUnitsConverter.FromPixels(h.Left, this.MeasurementUnit);
                head.Top = h.Top;// MeasureUnitsConverter.FromPixels(h.Top, this.MeasurementUnit);
                head.Size = h.Size;// MeasureUnitsConverter.FromPixels(h.Size, this.MeasurementUnit);
                head.Position = new Point(h.X + h.Width / 2, h.Y + h.Height / 2);// MeasureUnitsConverter.FromPixels(new Point(h.X + h.Width / 2, h.Y + h.Height / 2), this.MeasurementUnit);
                head.MeasurementUnit = this.MeasurementUnit;
            }
            return head;
        }

        private void adjustIntermediatePoints(List<Point> connectionPoints)
        {
            if (ConnectorType == ConnectorType.Orthogonal && IntermediatePoints.Count > 2)
            {
                bool foundDefect = false;
                for (int i = 0; i < connectionPoints.Count - 1; i++)
                {
                    if (!((connectionPoints[i].X == connectionPoints[i + 1].X) ||
                        (connectionPoints[i].Y == connectionPoints[i + 1].Y)))
                    {
                        double del = Math.Min(Math.Abs(connectionPoints[i].X - connectionPoints[i + 1].X)
                            , Math.Abs(connectionPoints[i].Y - connectionPoints[i + 1].Y));
                        if (del > 2)
                        {
                            foundDefect = true;
                            break;
                        }
                    }
                }
                if (foundDefect)
                {
                    for (int i = 0; i < connectionPoints.Count - 1; i++)
                    {
                        Point p1 = connectionPoints[i];
                        Point p2 = connectionPoints[i + 1];
                        if(Math.Abs(p1.X - p2.X ) >= Math.Abs(p1.Y - p2.Y))

                        {
                            connectionPoints[i + 1] = new Point(connectionPoints[i + 1].X, connectionPoints[i].Y);
                        }
                        else
                        {
                            connectionPoints[i + 1] = new Point(connectionPoints[i].X, connectionPoints[i + 1].Y);
                        }
                    }

                    
                }

                this.IntermediatePoints.Clear();
                for (int i = 0; i < connectionPoints.Count - 1; i++)
                {

                    bool remove = false;
                    if (i > 0 && i < connectionPoints.Count - 1)
                    {
                        Point p1 = connectionPoints[i - 1];
                        Point p2 = connectionPoints[i];
                        Point p3 = connectionPoints[i + 1];

                        if (AlmostEquals(p1.X, p2.X, p3.X))
                        {
                            connectionPoints[i + 1] = new Point(p1.X, p3.Y);
                            remove = true;
                        }
                        if(AlmostEquals(p1.Y, p2.Y, p3.Y))
                        {
                            connectionPoints[i + 1] = new Point(connectionPoints[i + 1].X, p1.Y);
                            remove = true;
                        }
                    }
                    if (!remove && i != 0)
                    {
                        this.IntermediatePoints.Add(connectionPoints[i]);
                    }


                }

                this.PxEndPointPosition = connectionPoints[connectionPoints.Count - 1];// MeasureUnitsConverter.FromPixels(connectionPoints[connectionPoints.Count - 1], MeasurementUnit);



            }
        }


        bool AlmostEquals(double x, double y, double z)
        {
            double sub1 = Math.Abs(x - y);
            double sub2 = Math.Abs(x - z);
            return sub1 <= 2 && sub2 <= 2;
        }

        #endregion

        #region Methods
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
        /// <returns>The Bezier segment</returns>
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
        /// <returns>The segment.</returns>
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


        internal void setMinMax()
        {
            if (this.ConnectorPathGeometry != null && this.ConnectorPathGeometry.Bounds != Rect.Empty)
            {
                minx = ConnectorPathGeometry.Bounds.Left;
                miny = ConnectorPathGeometry.Bounds.Top;
                maxx = ConnectorPathGeometry.Bounds.Right;
                maxy = ConnectorPathGeometry.Bounds.Bottom;
                double x = Math.Pow((minx - maxx), 2);
                double y = Math.Pow((miny - maxy), 2);
                Distance = Math.Sqrt(x + y);
            }
            else
            {
                minx = miny = maxx = maxy = 0;
            }
        }

        private void findDistance(List<Point> cp)
        {
            minx = cp[0].X;
            miny = cp[0].Y;
            maxx = cp[0].X;
            maxy = cp[0].Y;

            foreach (Point pt in cp)
            {
                if (minx > pt.X)
                    minx = pt.X;
                if (miny > pt.Y)
                    miny = pt.Y;
                if (maxx < pt.X)
                    maxx = pt.X;
                if (maxy < pt.Y)
                    maxy = pt.Y;
            }
            double x = Math.Pow((minx - maxx), 2);
            double y = Math.Pow((miny - maxy), 2);
            Distance = Math.Sqrt(x + y);
        }

        #endregion

        private Point GetIntersectionPoint(Point pt, Node n)
        {
            bool top = false;
            bool right = false;
            bool bottom = false;
            bool left = false;
            if (dview != null && dview.Page != null)
            {

                //n.refreshBoundaries();
                //pt = MeasureUnitsConverter.ToPixels(pt, this.MeasurementUnit);
                //Node tempx = (dview.Page as DiagramPage).tempx;
                //Path q = new Path();
                //q.Stroke = Brushes.Black;
                //tempx.Content = q;


                List<Point> p = new List<Point>();
                Rect rect = new Rect();
                if (n != null && n.Boundaries != null && n.Boundaries.RenderedGeometry != null)
                {
                    rect = n.Boundaries.RenderedGeometry.Bounds;
                }
                if (rect != Rect.Empty)
                {
                    p.Add(new Point(rect.Left + rect.Width / 2, rect.Top + rect.Height / 2));
                }
                else if (n != null && n.Boundaries != null && (n.Boundaries is Path) && (n.Boundaries as Path).Data != null && (n.Boundaries as Path).Data != Geometry.Empty)
                {
                    rect = (n.Boundaries as Path).Data.Bounds;
                    p.Add(new Point(rect.Left + rect.Width / 2, rect.Top + rect.Height / 2));
                }
                else
                {
                    p.Add(n.PxPosition);
                }

                if (this.ConnectorType == ConnectorType.Orthogonal)
                {
                    NodeInfo ni = getRectAsNodeInfo(rect);
                    FindEndAdjacent(out top, out right, out bottom, out left, ni, pt);
                    if (rect != null && rect != Rect.Empty)
                    {
                        if (top)
                        {
                            pt = new Point(rect.Left + rect.Width / 2, rect.Top - 10);
                        }
                        else if (right)
                        {
                            pt = new Point(rect.Right + 10, rect.Top + rect.Height / 2);
                        }
                        else if (bottom)
                        {
                            pt = new Point(rect.Left + rect.Width / 2, rect.Bottom + 10);
                        }
                        else if (left)
                        {
                            pt = new Point(rect.Left - 10, rect.Top + rect.Height / 2);
                        }
                    }
                }

                PathGeometry pathgeometry = new PathGeometry();
                PathFigure pathfigure = new PathFigure();
                pathfigure.StartPoint = pt;
                pathfigure.Segments.Add(new PolyLineSegment(p, true));
                pathgeometry.Figures.Add(pathfigure);
                Path path1 = new Path();
                path1.Data = pathgeometry;
                Path path2 = new Path();
                Point[] pts = new Point[0];
                if (n.Boundaries is Path)
                {
                    path2 = (n.Boundaries as Path);
                    pts = GetIntersectionPoints(path1.Data, path2.Data);
                }
                else if (n.Boundaries != null && n.Boundaries.RenderedGeometry != null)
                {
                    pts = GetIntersectionPoints(path1.Data, n.Boundaries.RenderedGeometry);
                }
                if (pts != null)
                {
                    Point r = findNearest(pt, pts);
                    if (r.X != 0 && r.Y != 0)
                    {
                        return r;// MeasureUnitsConverter.FromPixels(r, this.MeasurementUnit);
                    }
                    else
                    {
                        return n.PxPosition;// MeasureUnitsConverter.FromPixels(n.Position, this.MeasurementUnit);
                    }
                }
                else
                {
                    return n.PxPosition;// MeasureUnitsConverter.FromPixels(n.Position, this.MeasurementUnit);
                }
            }
            else
                return new Point(0, 0);
        }

        private void FindEndAdjacent(out bool top, out bool right, out bool bottom, out bool left, NodeInfo n, Point pt)
        {
            top = false;
            right = false;
            bottom = false;
            left = false;
            //pt = MeasureUnitsConverter.FromPixels(pt, this.MeasurementUnit);
            if (IntermediatePoints != null && IntermediatePoints.Count > 0)
            {
                int i = IntermediatePoints.IndexOf(pt);
                if (i == -1)
                {
                    Point[] pts = new Point[2];
                    pts[0] = (IntermediatePoints[0]);
                    pts[1] = (IntermediatePoints[IntermediatePoints.Count - 1]);
                    pt = findNearest(pt, pts);
                    i = IntermediatePoints.IndexOf(pt);
                    if (i == -1)
                    {
                        if (Math.Abs(IntermediatePoints[0].Y - pt.Y) < Math.Abs(IntermediatePoints[IntermediatePoints.Count - 1].Y - pt.Y))
                        {
                            i = 0;
                        }
                        else
                        {
                            i = IntermediatePoints.Count - 1;
                        }
                    }
                }

                if (i == 0 && IntermediatePoints.Count >= 3)
                {
                    if (Math.Abs(IntermediatePoints[0].X - IntermediatePoints[1].X)
                    > Math.Abs(IntermediatePoints[0].Y - IntermediatePoints[1].Y))
                    {
                        if (n.Position.Y > IntermediatePoints[1].Y)
                        {
                            top = true;
                        }
                        else
                        {
                            bottom = true;
                        }
                    }
                    else if (Math.Abs(IntermediatePoints[0].X - IntermediatePoints[1].X)
                    == Math.Abs(IntermediatePoints[0].Y - IntermediatePoints[1].Y))
                    {
                        if (Math.Abs(IntermediatePoints[1].X - IntermediatePoints[2].X)
                    > Math.Abs(IntermediatePoints[1].Y - IntermediatePoints[2].Y))
                        {
                            if (n.Position.X > IntermediatePoints[1].X)
                            {
                                left = true;
                            }
                            else
                            {
                                right = true;
                            }
                        }
                        else
                        {
                            if (n.Position.Y > IntermediatePoints[1].Y)
                            {
                                top = true;
                            }
                            else
                            {
                                bottom = true;
                            }
                        }
                    }
                    else
                    {
                        if (n.Position.X > IntermediatePoints[1].X)
                        {
                            left = true;
                        }
                        else
                        {
                            right = true;
                        }
                    }
                }
                else if (i == IntermediatePoints.Count - 1 && IntermediatePoints.Count >= 3)
                {
                    int last = IntermediatePoints.Count - 1;
                    if (Math.Abs(IntermediatePoints[last].X - IntermediatePoints[last - 1].X)
                    > Math.Abs(IntermediatePoints[last].Y - IntermediatePoints[last - 1].Y))
                    {
                        if (n.Position.Y > IntermediatePoints[last - 1].Y)
                        {
                            top = true;
                        }
                        else
                        {
                            bottom = true;
                        }
                    }
                    else if (Math.Abs(IntermediatePoints[last].X - IntermediatePoints[last - 1].X)
                    == Math.Abs(IntermediatePoints[last].Y - IntermediatePoints[last - 1].Y))
                    {
                        if (Math.Abs(IntermediatePoints[last - 1].X - IntermediatePoints[last - 2].X)
                    > Math.Abs(IntermediatePoints[last - 1].Y - IntermediatePoints[last - 2].Y))
                        {
                            if (n.Position.X > IntermediatePoints[last - 1].X)
                            {
                                left = true;
                            }
                            else
                            {
                                right = true;
                            }
                        }
                        else
                        {
                            if (n.Position.Y > IntermediatePoints[last - 1].Y)
                            {
                                top = true;
                            }
                            else
                            {
                                bottom = true;
                            }
                        }
                    }
                    else
                    {
                        if (n.Position.X > IntermediatePoints[last - 1].X)
                        {
                            left = true;
                        }
                        else
                        {
                            right = true;
                        }
                    }
                }
            }
        }

        private Point findNearest(Point pt, Point[] pts)
        {
            double dist = 1e100d;
            Point ret = new Point();
            foreach (Point p in pts)
            {
                //double d = Math.Sqrt(((pt.X - p.X) * (pt.X - p.X) + ((pt.Y - p.Y) * (pt.Y - p.Y))));
                double d = (pt - p).Length;
                if (dist > d)
                {
                    dist = d;
                    ret = p;
                }
            }
            return ret;
        }

        private Point[] GetIntersectionPointsFromWidened(Geometry og1, Geometry og2)
        {
            if (og1 != null && og2 != null)
            {
                //Geometry og1 = g1.GetWidenedPathGeometry(new Pen(Brushes.Black, 1.0));
                //Geometry og2 = g2.GetWidenedPathGeometry(new Pen(Brushes.Black, 1.0));
                CombinedGeometry cg = new CombinedGeometry(GeometryCombineMode.Intersect, og1, og2);

                PathGeometry pg = cg.GetFlattenedPathGeometry();//1000,ToleranceType.Absolute);
                //PathGeometry pg = cg.GetOutlinedPathGeometry();
                //Point[] result = new Point[pg.Figures.Count];
                //for (int i = 0; i < pg.Figures.Count; i++)
                //{
                //    Rect fig = new PathGeometry(new PathFigure[] { pg.Figures[i] }).Bounds;
                //    result[i] = new Point(fig.Left + fig.Width / 2.0, fig.Top + fig.Height / 2.0);
                //}
                //return result;
                return pg.Figures.Select(f => f.StartPoint).ToArray<Point>();
            }
            else
                return null;
        }

        private Point[] GetIntersectionPoints(Geometry g1, Geometry g2)
        {
            if (g2 != null)
            {
                double ces = PxConnectionEndSpace;
                ScaleTransform t = new ScaleTransform((g2.Bounds.Width + ces) / g2.Bounds.Width,
                                       (g2.Bounds.Height + ces) / g2.Bounds.Height, g2.Bounds.Left + g2.Bounds.Width / 2, g2.Bounds.Top + g2.Bounds.Height / 2);
                PathGeometry pg = Geometry.Combine(g2, g2, GeometryCombineMode.Intersect, t);
                //path2 = new Path();
                g2 = pg;
            }
            if (g1 != null && g2 != null)
            {
                Geometry og1 = g1.GetWidenedPathGeometry(new Pen(Brushes.Black, 1.0));
                Geometry og2 = g2.GetWidenedPathGeometry(new Pen(Brushes.Black, 1.0));
                CombinedGeometry cg = new CombinedGeometry(GeometryCombineMode.Intersect, og1, og2);

                PathGeometry pg = cg.GetFlattenedPathGeometry();//1000,ToleranceType.Absolute);
                //PathGeometry pg = cg.GetOutlinedPathGeometry();
                Point[] result = new Point[pg.Figures.Count];
                for (int i = 0; i < pg.Figures.Count; i++)
                {
                    Rect fig = new PathGeometry(new PathFigure[] { pg.Figures[i] }).Bounds;
                    result[i] = new Point(fig.Left + fig.Width / 2.0, fig.Top + fig.Height / 2.0);
                }
                return result;
            }
            else
                return null;
        }

        internal void setVirtualMin()
        {
            minx = miny = 0;
            minx = Math.Min(minx, PxStartPointPosition.X);
            minx = Math.Min(minx, PxEndPointPosition.X);
            miny = Math.Min(miny, PxStartPointPosition.Y);
            miny = Math.Min(miny, PxEndPointPosition.Y);
            if (IntermediatePoints != null)
            {
                for (int i = 0; i < IntermediatePoints.Count; i++)
                {
                    Point pt = IntermediatePoints[i];
                    minx = Math.Min(minx, IntermediatePoints[i].X);
                    miny = Math.Min(miny, IntermediatePoints[i].Y);
                }
            }
        }

        //internal double PxConnectionEndSpace
        //{
        //    get { return (double)GetValue(PxConnectionEndSpaceProperty); }
        //    set { SetValue(PxConnectionEndSpaceProperty, value); }
        //}

        //// Using a DependencyProperty as the backing store for PxConnectionEndSpace.  This enables animation, styling, binding, etc...
        //internal static readonly DependencyProperty PxConnectionEndSpaceProperty =
        //    DependencyProperty.Register("PxConnectionEndSpace", typeof(double), typeof(LineConnector));

        bool load = false;
        internal bool IsInternallyLoaded
        {
            get { return load; }
            set { load = value; }
        }
        internal double PxConnectionEndSpace
        {
            get
            {
                return MeasureUnitsConverter.ToPixels(ConnectionEndSpace, this.MeasurementUnit);
            }
            set
            {
                ConnectionEndSpace = MeasureUnitsConverter.FromPixels(value, this.MeasurementUnit);
            }
        }

        internal Rect GetBounds()
        {
            if (this.ConnectorPathGeometry != null && this.ConnectorPathGeometry.Bounds != Rect.Empty)
            {
                return this.ConnectorPathGeometry.Bounds;
            }
            else
                return Rect.Empty;
        }
    }
}
