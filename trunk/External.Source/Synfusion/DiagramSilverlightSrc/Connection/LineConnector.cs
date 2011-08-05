#region Copyright
// Copyright Syncfusion Inc. 2001 - 2011. All rights reserved.
// Use of this code is subject to the terms of our license.
// A copy of the current license can be obtained at any time by e-mailing
// licensing@syncfusion.com. Any infringement will be prosecuted under
// applicable laws. 
#endregion

namespace Syncfusion.Windows.Diagram
{
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
    ///namespace SilverlightApplication1
    /// {
    /// public partial class MainPage : UserControl
    /// {
    ///    public DiagramControl Control;
    ///    public DiagramModel Model;
    ///    public DiagramView View;
    ///    public MainPage()
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
    public class LineConnector : ConnectorBase, INodeGroup, ICommon
    {
        internal Point m_TempStartPoint;
        internal Point m_TempEndPoint;
        /// <summary>
        /// Identifies the ConnectorPathGeometry dependency property.
        /// </summary>
        public static readonly DependencyProperty ConnectorPathGeometryProperty = DependencyProperty.Register("ConnectorPathGeometry", typeof(PathGeometry), typeof(LineConnector), new PropertyMetadata(defaultCPG, new PropertyChangedCallback(OnConnectorPathGeometryChanged)));

        private static PathGeometry defaultCPG
        {
            get
            {
                PathGeometry pg = new PathGeometry();
                pg.Figures = new PathFigureCollection();
                PathFigure pf = new PathFigure();
                pf.StartPoint = new Point(0, 0);
                pf.Segments.Add(new PolyLineSegment());
                pg.Figures.Add(pf);
                return pg;
            }
        }

        /// <summary>
        /// Identifies the HeadDecoratorAngle dependency property.
        /// </summary>
        public static readonly DependencyProperty HeadDecoratorAngleProperty = DependencyProperty.Register("HeadDecoratorAngle", typeof(double), typeof(LineConnector), new PropertyMetadata(0d, new PropertyChangedCallback(OnHeadDecoratorAngleChanged)));

        /// <summary>
        /// Identifies the HeadDecoratorGrid dependency property.
        /// </summary>
        public static readonly DependencyProperty HeadDecoratorGridProperty = DependencyProperty.Register("HeadDecoratorGrid", typeof(Grid), typeof(LineConnector), new PropertyMetadata(null));

        /// <summary>
        /// Identifies the IsSelected dependency property.
        /// </summary>
        public static readonly DependencyProperty IsSelectedProperty = DependencyProperty.Register("IsSelected", typeof(bool), typeof(LineConnector), new PropertyMetadata(false, new PropertyChangedCallback(OnIsSelectedChanged)));

        /// <summary>
        /// Identifies the TailDecoratorAngle dependency property.
        /// </summary>
        public static readonly DependencyProperty TailDecoratorAngleProperty = DependencyProperty.Register("TailDecoratorAngle", typeof(double), typeof(LineConnector), new PropertyMetadata(0d, new PropertyChangedCallback(OnTailDecoratorAngleChanged)));

        /// <summary>
        /// Identifies the TailDecoratorGrid dependency property.
        /// </summary>
        public static readonly DependencyProperty TailDecoratorGridProperty = DependencyProperty.Register("TailDecoratorGrid", typeof(Grid), typeof(LineConnector), new PropertyMetadata(null));

        /// <summary>
        /// Identifies the ContextMenu dependency property
        /// </summary>
        public static readonly DependencyProperty ContextMenuProperty = DependencyProperty.Register("ContextMenu", typeof(ContextMenuControl), typeof(LineConnector), new PropertyMetadata(null));




        internal static readonly DependencyProperty LineAngleProperty = DependencyProperty.Register("LineAngle", typeof(double), typeof(LineConnector), new PropertyMetadata(0d, new PropertyChangedCallback(OnLineAngleChanged)));

        /// <summary>
        /// Used to store path head line
        /// </summary>
        private Path headpath;

        /// <summary>
        /// Used to store the DiagramControl instance
        /// </summary>
        private DiagramControl dc;

        /// <summary>
        /// Used to store the view instance.
        /// </summary>
        private DiagramView dview;

        /// <summary>
        /// Represents the fixed node.
        /// </summary>
        private Node fixedNodeConnection;


        /// <summary>
        /// Represents the movable node.
        /// </summary>
        private Node movableNodeConnection;

        /// <summary>
        /// Used to store the head decorator shape for internal use.
        /// </summary>
        private DecoratorShape headshape = DecoratorShape.None;

        /// <summary>
        /// Used to store the head thumb boolean
        /// </summary>
        private bool headthumb = false;

        /// <summary>
        /// Used to store node that is connected
        /// </summary>
        private Node hitNodeConnector = null;

        /// <summary>
        /// Used to check if default value is used for label width.
        /// </summary>
        private bool isdefaulted = false;

        /// <summary>
        /// Used to check if mouse is double clicked.
        /// </summary>
        private bool isdoubleclicked = false;

        /// <summary>
        /// Used to get the line canvas
        /// </summary>
        private Canvas linecanvas;
        private Grid labelGrid;
        private TextBox linetext;
        private TextBlock linelabel;
        private bool IsLinedrag = false;
        internal static bool linepopup = false;
        /// <summary>
        /// Used to store whether the line is dragging
        /// </summary>
        internal bool linedragging = false;

        /// <summary>
        /// Used to store boolean information if value is defaulted
        /// </summary>
        private bool misDefaulted = true;

        /// <summary>
        /// Used to store boolean information if node is hit.
        /// </summary>
        private bool misnodehit = false;

        /// <summary>
        /// Used to check if nodes are overlapped.
        /// </summary>
        private bool misoverlapped = false;

        /// <summary>
        /// Used to store the bend length.
        /// </summary>
        private double mbendLength = 10d;

        /// <summary>
        /// Used to store connection end space
        /// </summary>
        private double mconnectionEndSpace = 6d;

        /// <summary>
        /// Used to store previous hit node
        /// </summary>
        private Node previousHitNode = null;

        Random random = new Random();

        /// <summary>
        /// Used to store tail path
        /// </summary>
        private Path tailpath;

        /// <summary>
        /// Used to store the tail decorator shape for internal use.
        /// </summary>
        private DecoratorShape tailshape = DecoratorShape.None;

        /// <summary>
        /// Used to store tail thumb
        /// </summary>
        private bool tailthumb = false;


        /// <summary>
        /// Used to store the last node click instance
        /// </summary>
        private DateTime lastLineClick;

        /// <summary>
        /// Used to store the last node click point
        /// </summary>
        private Point lastLinePoint;

        private static bool mouseup;

        private static int n = 0;
        /// <summary>
        /// Initializes a new instance of the <see cref="LineConnector"/> class.
        /// </summary>
        /// <param name="source">The source node.</param>
        /// <param name="sink">The sink node.</param>
        /// <param name="view">The view instance.</param>
        public LineConnector(Node source, Node sink, DiagramView view)
            : base()
        {
            this.dview = view;
            this.ID = Guid.NewGuid();
            this.ConnectorType = (this.dview.Page as DiagramPage).ConnectorType;
            this.HeadNode = source;
            this.TailNode = sink;
            this.Loaded += new RoutedEventHandler(this.LineConnector_Loaded);
            mstartpointposition.X = DropPoint.X - 25;
            mstartpointposition.Y = DropPoint.Y - 25;
            mendpointposition.X = DropPoint.X + 25;
            mendpointposition.Y = DropPoint.Y + 25;
            PathGeometry geom = new PathGeometry();
            geom.Figures = new PathFigureCollection();
            PathFigure pf = new PathFigure();
            pf.Segments.Add(new PolyLineSegment());
            geom.Figures.Add(pf);
            ConnectorPathGeometry = geom;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LineConnector"/> class.
        /// </summary>
        public LineConnector()
            : base()
        {
            this.DefaultStyleKey = typeof(LineConnector);
            this.ID = Guid.NewGuid();
            this.Loaded += new RoutedEventHandler(this.LineConnector_Loaded);
            this.AddHandler(Control.MouseLeftButtonUpEvent, new MouseButtonEventHandler(this.Lineconnector_MouseLeftButtonUp), true);
            this.MouseLeftButtonDown += new MouseButtonEventHandler(this.LineConnector_MouseLeftButtonDown);
            this.MouseMove += new MouseEventHandler(this.LineConnector_MouseMove);
            PathGeometry geom = new PathGeometry();
            geom.Figures = new PathFigureCollection();
            PathFigure pf = new PathFigure();
            pf.Segments.Add(new PolyLineSegment());
            geom.Figures.Add(pf);
            ConnectorPathGeometry = geom;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LineConnector"/> class.
        /// </summary>
        /// <param name="view">The view instance.</param>
        public LineConnector(DiagramView view)
            : base()
        {
            this.DefaultStyleKey = typeof(LineConnector);
            this.dview = view;
            this.Loaded += new RoutedEventHandler(this.LineConnector_Loaded);
            this.ID = Guid.NewGuid();
            this.ConnectorType = (this.dview.Page as DiagramPage).ConnectorType;
            PathGeometry geom = new PathGeometry();
            geom.Figures = new PathFigureCollection();
            PathFigure pf = new PathFigure();
            pf.Segments.Add(new PolyLineSegment());
            geom.Figures.Add(pf);
            ConnectorPathGeometry = geom;
        }

        internal double PxConnectionEndSpace
        {
            get
            {
                return MeasureUnitsConverter.ToPixels(ConnectionEndSpace, MeasurementUnit);
            }
            set
            {
                ConnectionEndSpace = MeasureUnitsConverter.FromPixels(value, MeasurementUnit);
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
        ///namespace SilverlightApplication1
        /// {
        /// public partial class MainPage : UserControl
        /// {
        ///    public DiagramControl Control;
        ///    public DiagramModel Model;
        ///    public DiagramView View;
        ///    public MainPage()
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
            get
            {
                return this.mconnectionEndSpace;
            }

            set
            {
                this.mconnectionEndSpace = value;
                this.IsDefaulted = false;
            }
        }


        /// <summary>
        /// Gets or sets the PathGeometry of te connector.
        /// </summary>
        /// <value>
        /// Type: <see cref="Geometry"/>
        /// PathGeometry of the connector.
        /// </value>
        public PathGeometry ConnectorPathGeometry
        {
            get { return (PathGeometry)GetValue(ConnectorPathGeometryProperty); }
            set { SetValue(ConnectorPathGeometryProperty, value); }
        }

        /// <summary>
        /// Gets or sets the angle at which the head decorator is to be positioned.
        /// </summary>
        /// <value>
        /// Type: <see cref="double"/>
        /// Angle of the head decorator.
        /// </value>
        public double HeadDecoratorAngle
        {
            get
            {
                return (double)GetValue(HeadDecoratorAngleProperty);
            }

            set
            {
                SetValue(HeadDecoratorAngleProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets whether the line connector is selected.
        /// </summary>
        /// <value>
        /// Type: <see cref="Boolean"/>
        /// </value>
        public new bool IsSelected
        {
            get { return (bool)GetValue(IsSelectedProperty); }
            set { SetValue(IsSelectedProperty, value); }
        }

        /// <summary>
        /// Gets or sets the angle at which the tail decorator is to be positioned.
        /// </summary>
        /// <value>
        /// Type: <see cref="double"/>
        /// Angle of the tail decorator.
        /// </value>
        public double TailDecoratorAngle
        {
            get
            {
                return (double)GetValue(TailDecoratorAngleProperty);
            }

            set
            {
                SetValue(TailDecoratorAngleProperty, value);
            }
        }


        /// <summary>
        /// Gets or sets the line connector context menu.
        /// </summary>
        /// <value>The line connector context menu.</value>
        public ContextMenuControl ContextMenu
        {
            get
            {
                return (ContextMenuControl)GetValue(ContextMenuProperty);
            }
            set
            {
                SetValue(ContextMenuProperty, value);
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
                return this.mbendLength;
            }

            set
            {
                this.mbendLength = value;
            }
        }

        /// <summary>
        /// Gets or sets the head decorator grid.Used for internal assignments.
        /// </summary>
        /// <remarks>
        /// Default value is 10d.
        /// </remarks>
        internal Grid HeadDecoratorGrid
        {
            get
            {
                return (Grid)GetValue(HeadDecoratorGridProperty);
            }

            set
            {
                SetValue(HeadDecoratorGridProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the head path of the line.
        /// </summary>
        /// <remarks>
        /// Default path of the head.
        /// </remarks>
        internal Path HeadShape
        {
            get
            {
                return this.headpath;
            }

            set
            {
                this.headpath = value;
            }
        }

        /// <summary>
        /// Gets or sets the internal head decorator shape. Used for internal assignments in case of overlapped nodes.
        /// </summary>
        /// <value>The internal head decorator shape.</value>
        internal DecoratorShape InternalHeadShape
        {
            get { return this.headshape; }
            set { this.headshape = value; }
        }

        /// <summary>
        /// Gets or sets the internal tail decorator shape. Used for internal assignments in case of overlapped nodes.
        /// </summary>
        /// <value>The internal tail decorator shape.</value>
        internal DecoratorShape InternalTailShape
        {
            get { return this.tailshape; }
            set { this.tailshape = value; }
        }

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
                return this.misDefaulted;
            }

            set
            {
                this.misDefaulted = value;
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
                return this.misnodehit;
            }

            set
            {
                this.misnodehit = value;
            }
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
                return this.misoverlapped;
            }

            set
            {
                this.misoverlapped = value;
            }
        }

        /// <summary>
        /// Gets or sets the line canvas.Used for internal assignments.
        /// </summary>
        /// <remarks>
        /// Default value is 10d.
        /// </remarks>
        internal Canvas LineCanvas
        {
            get { return this.linecanvas; }
            set { this.linecanvas = value; }
        }

        internal double LineAngle
        {
            get
            {
                return (double)GetValue(LineAngleProperty);
            }

            set
            {
                SetValue(LineAngleProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the Tail decorator grid.Used for internal assignments.
        /// </summary>
        /// <remarks>
        /// Default value is 10d.
        /// </remarks>
        internal Grid TailDecoratorGrid
        {
            get
            {
                return (Grid)GetValue(TailDecoratorGridProperty);
            }

            set
            {
                SetValue(TailDecoratorGridProperty, value);
            }
        }


        /// <summary>
        /// Gets or sets the tail path of the line.
        /// </summary>
        /// <remarks>
        /// Default path of the tail.
        /// </remarks>
        internal Path TailShape
        {
            get
            {
                return this.tailpath;
            }

            set
            {
                this.tailpath = value;
            }
        }

        /// <summary>
        /// Adds points to the collection in case of orthogonal line .
        /// </summary>
        /// <param name="linePoints">Collection of points.</param>
        /// <returns>The modified collection of points</returns>
        private List<Point> AddLinePoints(List<Point> linePoints)
        {
            List<Point> points = new List<Point>();
            for (int i = 0; i < linePoints.Count; i++)
            {
                points.Add(linePoints[i]);
            }

            if (!(Orientation == TreeOrientation.LeftRight || Orientation == TreeOrientation.RightLeft))
            {
                points.Insert(1, new Point(points[1].X, points[0].Y));
                return points;
            }
            else
            {
                points.Insert(1, new Point(points[0].X, points[1].Y));
                return points;
            }
        }

        /// <summary>
        /// Adds points to the collection in case of orthogonal line .
        /// </summary>
        /// <param name="linePoints">Collection of points.</param>
        /// <returns>The modified collection of points</returns>
        internal List<Point> AddPoints(List<Point> linePoints)
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
        /// Invoked when Label editing is complete
        /// </summary>
        internal void CompleteConnEditing()
        {
        }

        /// <summary>
        /// Makes the end connection to the respective node by finding the correct direction of the node.
        /// </summary>
        /// <param name="connectionPoints">Collection of points.</param>
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
        private void FindConnectionEnd(List<Point> connectionPoints, Point startPoint, Point endPoint, bool isTop, bool isBottom, bool isLeft, bool isRight, bool tisTop, bool tisBottom, bool tisLeft, bool tisRight)
        {
            Point startpoint = new Point(0, 0);
            Point endpoint = new Point(0, 0);
            if (HeadNode != null)
            {
                if (isRight)
                {
                    startpoint = new Point(startPoint.X - this.BendLength, startPoint.Y);
                }
                else if (isBottom)
                {
                    startpoint = new Point(startPoint.X, startPoint.Y - this.BendLength);
                }
                else if (isLeft)
                {
                    startpoint = new Point(startPoint.X + this.BendLength, startPoint.Y);
                }
                else if (isTop)
                {
                    startpoint = new Point(startPoint.X, startPoint.Y + this.BendLength);
                }
            }
            else
            {
                startpoint = startPoint;
            }

            if (TailNode != null)
            {
                if (tisRight)
                {
                    endpoint = new Point(endPoint.X - this.BendLength, endPoint.Y);
                }
                else if (tisBottom)
                {
                    endpoint = new Point(endPoint.X, endPoint.Y - this.BendLength);
                }
                else if (tisLeft)
                {
                    endpoint = new Point(endPoint.X + this.BendLength, endPoint.Y);
                }
                else if (tisTop)
                {
                    endpoint = new Point(endPoint.X, endPoint.Y + this.BendLength);
                }
            }
            else
            {
                endpoint = endPoint;
            }

            if (ConnectionHeadPort == null)
            {
                connectionPoints.Insert(0, startpoint);
            }

            if (ConnectionTailPort == null)
            {
                connectionPoints.Add(endpoint);
            }
        }

        /// <summary>
        /// Gets the line points when tail node is null.
        /// </summary>
        /// <param name="source">The source node</param>
        /// <param name="sinkPoint">sink point</param>
        /// <param name="e">mouse event position</param>
        /// <returns>The collection of points</returns>
        internal List<Point> GetAdornerLinePoints(NodeInfo source, Point sinkPoint, MouseEventArgs e)
        {
            LineConnector lineconnector = this;
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
                        Point startPoint;

                        if (this.HitTesting(sinkPoint))
                        {
                            if (this.hitNodeConnector != null)
                            {
                                NodeInfo target = this.hitNodeConnector.GetInfo();
                                double hitwidth = this.hitNodeConnector.Width;// MeasureUnitsConverter.FromPixels(this.hitNodeConnector.Width, target.MeasurementUnit);
                                double hitheight = this.hitNodeConnector.Height;// MeasureUnitsConverter.FromPixels(this.hitNodeConnector.Height, target.MeasurementUnit);
                                Rect rectTarget = new Rect(this.hitNodeConnector.PxLogicalOffsetX, this.hitNodeConnector.PxLogicalOffsetY, hitwidth, hitheight);
                                if (this.headthumb)
                                {
                                    ConnectorBase.GetOrthogonalLineIntersect(target, source, rectTarget, rectSource, out b1, out b2, out b3, out b4, out b5, out b6, out b7, out b8, out startPoint, out endPoint);
                                }
                                else
                                {
                                    ConnectorBase.GetOrthogonalLineIntersect(source, target, rectSource, rectTarget, out b1, out b2, out b3, out b4, out b5, out b6, out b7, out b8, out startPoint, out endPoint);
                                }

                                linePoints.Add(startPoint);
                                linePoints.Add(endPoint);
                                linePoints = this.AddPoints(linePoints);
                                this.FindConnectionEnd(linePoints, startPoint, endPoint, b1, b2, b3, b4, b5, b6, b7, b8);
                            }
                        }
                        else
                        {
                            Point s;
                            if (!this.headthumb)
                            {
                                lineconnector.ConnectionTailPort = null;
                                endPoint = e.GetPosition(this);
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
                                    this.fixedNodeConnection = null;
                                }
                            }
                            else
                            {
                                lineconnector.ConnectionHeadPort = null;
                                s = e.GetPosition(this);
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
                                    this.fixedNodeConnection = null;
                                }
                            }

                            linePoints.Add(s);
                            linePoints.Add(endPoint);
                            linePoints = this.AddPoints(linePoints);
                        }
                    }
                    else
                    {
                        Point s;
                        if (!this.headthumb)
                        {
                            if (!this.HitTesting(sinkPoint))
                            {
                                lineconnector.ConnectionTailPort = null;
                            }

                            endPoint = e.GetPosition(this);
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
                        }
                        else
                        {
                            if (!this.HitTesting(sinkPoint))
                            {
                                lineconnector.ConnectionHeadPort = null;
                            }

                            s = e.GetPosition(this);
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
        /// Gets the line points when mouse event is raised.
        /// </summary>
        /// <param name="e">Mouse point</param>
        /// <returns>The collection of points</returns>
        internal List<Point> GetAdornerLinePoints(MouseEventArgs e)
        {
            LineConnector lineconnector = this;
            Point startPoint = new Point(0, 0);
            Point endPoint = new Point(0, 0);
            List<Point> connectionPoints = new List<Point>();
            if (this.headthumb)
            {
                startPoint = e.GetPosition(this);
                endPoint = new Point((lineconnector as LineConnector).PxEndPointPosition.X, (lineconnector as LineConnector).PxEndPointPosition.Y);
            }
            else
            {
                startPoint = new Point((lineconnector as LineConnector).PxStartPointPosition.X, (lineconnector as LineConnector).PxStartPointPosition.Y);
                endPoint = e.GetPosition(this);
            }

            connectionPoints.Add(startPoint);
            connectionPoints.Add(endPoint);
            if (lineconnector.ConnectorType == ConnectorType.Orthogonal)
            {
                connectionPoints = this.AddPoints(connectionPoints);
            }

            return connectionPoints;
        }

        /// <summary>
        /// Gets the line points when tail node is null.
        /// </summary>
        /// <param name="source">The source node</param>
        /// <returns>The collection of points</returns>
        internal List<Point> GetLinePoints(NodeInfo source)
        {
            double twozero = 20;// MeasureUnitsConverter.FromPixels(20, DiagramPage.Munits);
            double sourceleft = PxStartPointPosition.X;// MeasureUnitsConverter.FromPixels(StartPointPosition.X, DiagramPage.Munits);
            double sourcetop = PxStartPointPosition.Y;// MeasureUnitsConverter.FromPixels(StartPointPosition.Y, DiagramPage.Munits);
            double dropcentreX = DropPoint.X;// MeasureUnitsConverter.FromPixels(DropPoint.X, DiagramPage.Munits);
            double dropcentreY = DropPoint.Y;// MeasureUnitsConverter.FromPixels(DropPoint.Y, DiagramPage.Munits);
            double targetleft = PxEndPointPosition.X;// MeasureUnitsConverter.FromPixels(EndPointPosition.X, DiagramPage.Munits);
            double targettop = PxEndPointPosition.Y;// MeasureUnitsConverter.FromPixels(EndPointPosition.Y, DiagramPage.Munits);
            if (this.dview == null)
            {
                this.dview = Node.GetDiagramView(this);
            }

            double cs;
            double bl;
            bl = this.BendLength;// MeasureUnitsConverter.FromPixels(this.BendLength, source.MeasurementUnit);
            double endspace;

            cs = this.PxConnectionEndSpace;// MeasureUnitsConverter.FromPixels(this.ConnectionEndSpace, source.MeasurementUnit);
            if (this.ConnectorType == ConnectorType.Orthogonal)
            {
                endspace = bl + cs + 25;
            }
            else
            {
                endspace = cs;
            }

            bool b1, b2, b3, b4, b5, b6, b7, b8;
            Point s = new Point(0, 0);
            Point e = new Point(PxEndPointPosition.X, PxEndPointPosition.Y);
            Rect sourceRect = new Rect();
            Rect targetRect = new Rect();
            NodeInfo target = new NodeInfo();
            target.Position = new Point(PxEndPointPosition.X - 25, PxEndPointPosition.Y - 25);
            targetRect = new Rect(
                                   targetleft,
                                   targettop,
                                    twozero,
                                    twozero);
            ConnectionPoints = new List<Point>();
            if (HeadNode != null)
            {
                sourceRect = new Rect(
                                    source.Left - endspace,
                                    source.Top - endspace,
                                    source.Size.Width + endspace,
                                    source.Size.Height + endspace);
                s = new Point(source.Position.X, source.Position.Y);
            }
            else
            {
                s = new Point(PxStartPointPosition.X, PxStartPointPosition.Y);
                sourceRect = new Rect(
                                    sourceleft,
                                    sourcetop,
                                    twozero,
                                    twozero);
            }

            if (this.ConnectorType != ConnectorType.Orthogonal)
            {
                Point startPoint = new Point(0, 0);
                Point endPoint = new Point(0, 0);

                if (ConnectionHeadPort != null)
                {
                    double width = double.IsNaN(ConnectionHeadPort.Width) ? 2.5 : ConnectionHeadPort.Width / 2;
                    double height = double.IsNaN(ConnectionHeadPort.Height) ? 2.5 : ConnectionHeadPort.Height / 2;

                    startPoint = new Point(ConnectionHeadPort.PxCenterPosition.X + source.Left + width, height + ConnectionHeadPort.PxCenterPosition.Y + source.Top);
                    if ((this.HeadNode as Node).RenderTransform != null)
                    {
                        if ((this.HeadNode as Node).RenderTransform is RotateTransform)
                        {
                            startPoint = this.GeneralPointRotation(source.Position, startPoint, ((this.HeadNode as Node).RenderTransform as RotateTransform).Angle);
                        }

                    }
                }
                else
                {
                    startPoint = ConnectorBase.GetLineIntersect(source, s, e, sourceRect, out b1, out b2, out b3, out b4, this.ConnectorType);
                }

                if (ConnectionTailPort == null)
                {
                    endPoint = new Point(PxEndPointPosition.X, PxEndPointPosition.Y);
                }
                else
                {
                    endPoint = ConnectorBase.GetLineIntersect(source, s, e, sourceRect, out b1, out b2, out b3, out b4, this.ConnectorType);
                    if (this.TailNode!= null && (this.TailNode as Node).RenderTransform != null)
                    {
                        if ((this.TailNode as Node).RenderTransform is RotateTransform)
                        {
                            endPoint = this.GeneralPointRotation(target.Position, endPoint, ((this.TailNode as Node).RenderTransform as RotateTransform).Angle);
                        }

                    }
                }

                ConnectionPoints.Add(startPoint);
                ConnectionPoints.Add(endPoint);
            }
            else
            {
                Point startPoint, endPoint;

                if (!(Orientation == TreeOrientation.LeftRight || Orientation == TreeOrientation.RightLeft))
                {
                    ConnectorBase.GetOrthogonalLineIntersect(source, target, sourceRect, targetRect, out b1, out b2, out b3, out b4, out b5, out b6, out b7, out b8, out startPoint, out endPoint);
                }
                else
                {
                    ConnectorBase.GetTreeOrthogonalLineIntersect(source, target, sourceRect, targetRect, out b1, out b2, out b3, out b4, out b5, out b6, out b7, out b8, out startPoint, out endPoint);
                }

                if (ConnectionHeadPort != null)
                {

                    double width = double.IsNaN(ConnectionHeadPort.Width) ? 2.5 : ConnectionHeadPort.Width / 2;
                    double height = double.IsNaN(ConnectionHeadPort.Height) ? 2.5 : ConnectionHeadPort.Height / 2;

                    startPoint = new Point(ConnectionHeadPort.PxLeft + source.Left + width, height + ConnectionHeadPort.PxTop + source.Top);
                }



                if (this.HeadDecoratorGrid != null)
                {
                    if (b5)
                    {
                        (this as LineConnector).HeadDecoratorAngle = -90;
                    }
                    else if (b6)
                    {
                        (this as LineConnector).HeadDecoratorAngle = 90;
                    }
                    else if (b7)
                    {
                        (this as LineConnector).HeadDecoratorAngle = -180;
                    }
                    else if (b8)
                    {
                        (this as LineConnector).HeadDecoratorAngle = 0;
                    }
                }

                if (this.TailDecoratorGrid != null)
                {
                    if (b1)
                    {
                        (this as LineConnector).TailDecoratorAngle = -90;
                    }
                    else if (b2)
                    {
                        (this as LineConnector).TailDecoratorAngle = 90;
                    }
                    else if (b3)
                    {
                        (this as LineConnector).TailDecoratorAngle = -180;
                    }
                    else if (b4)
                    {
                        (this as LineConnector).TailDecoratorAngle = 0;
                    }
                }

                ConnectionPoints.Add(startPoint);
                endPoint = new Point(PxEndPointPosition.X, PxEndPointPosition.Y);
                ConnectionPoints.Add(endPoint);
                ConnectionPoints = this.AddLinePoints(ConnectionPoints);
                this.FindConnectionEnd(ConnectionPoints, startPoint, endPoint, b1, b2, b3, b4, b5, b6, b7, b8);
            }

            int count = ConnectionPoints.Count();
            double x = Math.Pow((ConnectionPoints[0].X - ConnectionPoints[count - 1].X), 2);
            double y = Math.Pow((ConnectionPoints[0].Y - ConnectionPoints[count - 1].Y), 2);
            Distance = Math.Sqrt(x + y);
            return ConnectionPoints;
        }

        /// <summary>
        /// Calculates the points which form the path geometry. 
        /// </summary>
        /// <param name="source">The head node</param>
        /// <param name="target">The tail node</param>
        /// <returns>Collection of points.</returns>
        internal List<Point> GetLinePoints(NodeInfo source, NodeInfo target)
        {
            double twozero = 20;// MeasureUnitsConverter.FromPixels(20, DiagramPage.Munits);
            double sourceleft = PxStartPointPosition.X;// MeasureUnitsConverter.FromPixels(StartPointPosition.X, DiagramPage.Munits);
            double sourcetop = PxStartPointPosition.Y;// MeasureUnitsConverter.FromPixels(StartPointPosition.Y, DiagramPage.Munits);
            double dropcentreX = DropPoint.X;// MeasureUnitsConverter.FromPixels(DropPoint.X, DiagramPage.Munits);
            double dropcentreY = DropPoint.Y;// MeasureUnitsConverter.FromPixels(DropPoint.Y, DiagramPage.Munits);
            if (this.dview == null)
            {
                this.dview = Node.GetDiagramView(this);
            }

            double cs;
            double bl;
            bl = this.BendLength;// MeasureUnitsConverter.FromPixels(this.BendLength, source.MeasurementUnit);
            double endspace;

            cs = this.PxConnectionEndSpace;// MeasureUnitsConverter.FromPixels(this.ConnectionEndSpace, source.MeasurementUnit);

            if (this.ConnectorType == ConnectorType.Orthogonal)
            {
                endspace = bl + cs + 25;
            }
            else
            {
                endspace = cs;
            }

            bool b1, b2, b3, b4, b5, b6, b7, b8;
            Point s = new Point(0, 0);
            Point e = new Point(0, 0);
            Rect sourceRect = new Rect();
            Rect targetRect = new Rect();
            List<Point> connectionPoints = new List<Point>();

            if (HeadNode != null)
            {
                if (this.ConnectionHeadPort == null)
                {
                    sourceRect = new Rect(
                                        source.Left - endspace,
                                        source.Top - endspace,
                                        source.Size.Width + endspace,
                                        source.Size.Height + endspace);

                    s = new Point(source.Position.X, source.Position.Y);

                }
                else
                {
                    sourceRect = new Rect(
                                     source.Left + ConnectionHeadPort.PxCenterPosition.X + ((double.IsNaN(ConnectionHeadPort.Width)) ? 2.5 : ConnectionHeadPort.Width / 2),
                                     source.Top + ConnectionHeadPort.PxCenterPosition.Y + ((double.IsNaN(ConnectionHeadPort.Height)) ? 2.5 : ConnectionHeadPort.Height / 2),
                                     source.Size.Width + endspace,
                                     source.Size.Height + endspace);

                    s = new Point(source.Left + ConnectionHeadPort.PxCenterPosition.X + ((double.IsNaN(ConnectionHeadPort.Width)) ? 2.5 : ConnectionHeadPort.Width / 2),
                                     source.Top + ConnectionHeadPort.PxCenterPosition.Y + ((double.IsNaN(ConnectionHeadPort.Height)) ? 2.5 : ConnectionHeadPort.Height / 2));
                }
            }
            else
            {
                s = new Point(PxStartPointPosition.X, PxStartPointPosition.Y);
                sourceRect = new Rect(
                                    sourceleft,
                                    sourcetop,
                                    twozero,
                                    twozero);
            }

            if (TailNode != null)
            {
                if (this.ConnectionTailPort == null)
                {
                    targetRect = new Rect(
                                     target.Left - endspace,
                                     target.Top - endspace,
                                     target.Size.Width + endspace,
                                     target.Size.Height + endspace);
                    e = new Point(target.Position.X, target.Position.Y);
                }
                else
                {
                    double width = double.IsNaN(ConnectionTailPort.Width) ? 2.5 : ConnectionTailPort.Width / 2;
                    double height = double.IsNaN(ConnectionTailPort.Height) ? 2.5 : ConnectionTailPort.Height / 2;

                    targetRect = new Rect(
                                     target.Left + ConnectionTailPort.PxCenterPosition.X + width,
                                     target.Top + ConnectionTailPort.PxCenterPosition.Y + height,
                                     target.Size.Width + endspace,
                                     target.Size.Height + endspace);

                    e = new Point(target.Left + ConnectionTailPort.PxCenterPosition.X + width,
                                     target.Top + ConnectionTailPort.PxCenterPosition.Y + height);
                }

            }
            else
            {
                e = new Point(PxEndPointPosition.X, PxEndPointPosition.Y);
                targetRect = new Rect(
                                    sourceleft,
                                    sourcetop,
                                    twozero,
                                    twozero);
            }

            if (this.ConnectorType != ConnectorType.Orthogonal)
            {
                Point startPoint = new Point(0, 0);
                Point endPoint = new Point(0, 0);

                if (ConnectionHeadPort != null)
                {
                    double width = double.IsNaN(ConnectionHeadPort.Width) ? 2.5 : ConnectionHeadPort.Width / 2;
                    double height = double.IsNaN(ConnectionHeadPort.Height) ? 2.5 : ConnectionHeadPort.Height / 2;

                    startPoint = new Point(source.Left + ConnectionHeadPort.PxCenterPosition.X + width,
                                     source.Top + ConnectionHeadPort.PxCenterPosition.Y + height);
                    if ((this.HeadNode as Node).RenderTransform != null)
                    {
                        if ((this.HeadNode as Node).RenderTransform is RotateTransform)
                        {
                            startPoint = this.GeneralPointRotation(source.Position, startPoint, ((this.HeadNode as Node).RenderTransform as RotateTransform).Angle);
                        }

                    }
                }
                else
                {
                    startPoint = ConnectorBase.GetLineIntersect(source, s, e, sourceRect, out b1, out b2, out b3, out b4, this.ConnectorType);
                }

                if (ConnectionTailPort != null)
                {
                    double width = double.IsNaN(ConnectionTailPort.Width) ? 2.5 : ConnectionTailPort.Width / 2;
                    double height = double.IsNaN(ConnectionTailPort.Height) ? 2.5 : ConnectionTailPort.Height / 2;

                    endPoint = new Point(target.Left + ConnectionTailPort.PxCenterPosition.X + width,
                                     target.Top + ConnectionTailPort.PxCenterPosition.Y + height);

                    if ((this.TailNode as Node).RenderTransform != null)
                    {
                        if ((this.TailNode as Node).RenderTransform is RotateTransform)
                        {
                            endPoint = this.GeneralPointRotation(target.Position, endPoint, ((this.TailNode as Node).RenderTransform as RotateTransform).Angle);
                        }

                    }
                }
                else
                {
                    endPoint = ConnectorBase.GetLineIntersect(target, s, e, targetRect, out b5, out b6, out b7, out b8, this.ConnectorType);
                }

                connectionPoints.Add(startPoint);
                connectionPoints.Add(endPoint);
            }
            else
            {
                Point startPoint, endPoint;

                if (!(Orientation == TreeOrientation.LeftRight || Orientation == TreeOrientation.RightLeft))
                {
                    ConnectorBase.GetOrthogonalLineIntersect(source, target, sourceRect, targetRect, out b1, out b2, out b3, out b4, out b5, out b6, out b7, out b8, out startPoint, out endPoint);
                }
                else
                {
                    ConnectorBase.GetTreeOrthogonalLineIntersect(source, target, sourceRect, targetRect, out b1, out b2, out b3, out b4, out b5, out b6, out b7, out b8, out startPoint, out endPoint);
                }

                if (ConnectionHeadPort != null)
                {
                    double width = double.IsNaN(ConnectionHeadPort.Width) ? 2.5 : ConnectionHeadPort.Width / 2;
                    double height = double.IsNaN(ConnectionHeadPort.Height) ? 2.5 : ConnectionHeadPort.Height / 2;

                    startPoint = new Point(source.Left + ConnectionHeadPort.PxCenterPosition.X + width,
                                     source.Top + ConnectionHeadPort.PxCenterPosition.Y + height);
                    if ((this.HeadNode as Node).RenderTransform != null)
                    {
                        if ((this.HeadNode as Node).RenderTransform is RotateTransform)
                        {
                            startPoint = this.GeneralPointRotation(source.Position, startPoint, ((this.HeadNode as Node).RenderTransform as RotateTransform).Angle);
                        }

                    }
                }

                if (ConnectionTailPort != null)
                {
                    double width = double.IsNaN(ConnectionTailPort.Width) ? 2.5 : ConnectionTailPort.Width / 2;
                    double height = double.IsNaN(ConnectionTailPort.Height) ? 2.5 : ConnectionTailPort.Height / 2;

                    endPoint = new Point(target.Left + ConnectionTailPort.PxCenterPosition.X + width, target.Top + ConnectionTailPort.PxCenterPosition.Y + height);
                    if ((this.TailNode as Node).RenderTransform != null)
                    {
                        if ((this.TailNode as Node).RenderTransform is RotateTransform)
                        {
                            endPoint = this.GeneralPointRotation(target.Position, endPoint, ((this.TailNode as Node).RenderTransform as RotateTransform).Angle);
                        }

                    }
                }

                if (this.HeadDecoratorGrid != null)
                {
                    if (b5)
                    {
                        (this as LineConnector).HeadDecoratorAngle = -90;
                    }
                    else if (b6)
                    {
                        (this as LineConnector).HeadDecoratorAngle = 90;
                    }
                    else if (b7)
                    {
                        (this as LineConnector).HeadDecoratorAngle = -180;
                    }
                    else if (b8)
                    {
                        (this as LineConnector).HeadDecoratorAngle = 0;
                    }
                }

                if (this.TailDecoratorGrid != null)
                {
                    if (b1)
                    {
                        (this as LineConnector).TailDecoratorAngle = -90;
                    }
                    else if (b2)
                    {
                        (this as LineConnector).TailDecoratorAngle = 90;
                    }
                    else if (b3)
                    {
                        (this as LineConnector).TailDecoratorAngle = -180;
                    }
                    else if (b4)
                    {
                        (this as LineConnector).TailDecoratorAngle = 0;
                    }
                }

                connectionPoints.Add(startPoint);
                connectionPoints.Add(endPoint);
                connectionPoints = this.AddLinePoints(connectionPoints);
                this.FindConnectionEnd(connectionPoints, startPoint, endPoint, b1, b2, b3, b4, b5, b6, b7, b8);
            }

            int count = connectionPoints.Count();
            double x = Math.Pow((connectionPoints[0].X - connectionPoints[count - 1].X), 2);
            double y = Math.Pow((connectionPoints[0].Y - connectionPoints[count - 1].Y), 2);
            Distance = Math.Sqrt(x + y);
            return connectionPoints;
        }

        /// <summary>
        /// Gets the line points when head node is null.
        /// </summary>
        /// <param name="target">The source node</param>
        /// <param name="istarget">true, if it is the target.</param>
        /// <returns>Collection Of points</returns>
        internal List<Point> GetLinePoints(NodeInfo target, bool istarget)
        {
            double twozero = 20;// MeasureUnitsConverter.FromPixels(20, DiagramPage.Munits);
            double sourceleft = PxStartPointPosition.X;// MeasureUnitsConverter.FromPixels(StartPointPosition.X, DiagramPage.Munits);
            double sourcetop = PxStartPointPosition.Y;// MeasureUnitsConverter.FromPixels(StartPointPosition.Y, DiagramPage.Munits);
            double dropcentreX = DropPoint.X;// MeasureUnitsConverter.FromPixels(DropPoint.X, DiagramPage.Munits);
            double dropcentreY = DropPoint.Y;// MeasureUnitsConverter.FromPixels(DropPoint.Y, DiagramPage.Munits);
            double targetleft = PxEndPointPosition.X;// MeasureUnitsConverter.FromPixels(EndPointPosition.X, DiagramPage.Munits);
            double targettop = PxEndPointPosition.Y;// MeasureUnitsConverter.FromPixels(EndPointPosition.Y, DiagramPage.Munits);
            if (this.dview == null)
            {
                this.dview = Node.GetDiagramView(this);
            }

            double cs;
            double bl;
            bl = this.BendLength;// MeasureUnitsConverter.FromPixels(this.BendLength, target.MeasurementUnit);
            double endspace;
            cs = this.PxConnectionEndSpace;// MeasureUnitsConverter.FromPixels(this.ConnectionEndSpace, target.MeasurementUnit);
            if (this.ConnectorType == ConnectorType.Orthogonal)
            {
                endspace = bl + cs + 25;
            }
            else
            {
                endspace = cs;
            }

            bool b1, b2, b3, b4, b5, b6, b7, b8;
            Point s = new Point(PxStartPointPosition.X, PxStartPointPosition.Y);
            Point e = new Point(0, 0);
            Rect targetRect = new Rect();
            Rect sourceRect = new Rect();
            sourceRect = new Rect(
                                   sourceleft,
                                   sourcetop,
                                   twozero,
                                   twozero);
            NodeInfo source = new NodeInfo();
            source.Position = new Point(PxStartPointPosition.X + 25, PxStartPointPosition.Y + 25);
            ConnectionPoints = new List<Point>();
            if (TailNode != null)
            {
                targetRect = new Rect(
                                      target.Left - endspace,
                                      target.Top - endspace,
                                      target.Size.Width + endspace,
                                      target.Size.Height + endspace);
                e = new Point(target.Position.X, target.Position.Y);
            }
            else
            {
                e = new Point(PxEndPointPosition.X, PxEndPointPosition.Y);
                targetRect = new Rect(
                                    targetleft,
                                    targettop,
                                    twozero,
                                    twozero);
            }

            if (this.ConnectorType != ConnectorType.Orthogonal)
            {
                Point startPoint = new Point(0, 0);
                Point endPoint = new Point(0, 0);

                if (ConnectionTailPort != null)
                {
                    double width = double.IsNaN(ConnectionTailPort.Width) ? 2.5 : ConnectionTailPort.Width / 2;
                    double height = double.IsNaN(ConnectionTailPort.Height) ? 2.5 : ConnectionTailPort.Height / 2;

                    endPoint = new Point(ConnectionTailPort.PxCenterPosition.X + target.Left + width, height + ConnectionTailPort.PxCenterPosition.Y + target.Top);
                    if ((this.TailNode as Node).RenderTransform != null)
                    {
                        if ((this.TailNode as Node).RenderTransform is RotateTransform)
                        {
                            endPoint = this.GeneralPointRotation(target.Position, endPoint, ((this.TailNode as Node).RenderTransform as RotateTransform).Angle);
                        }

                    }
                }
                else
                {
                    endPoint = ConnectorBase.GetLineIntersect(target, s, e, targetRect, out b5, out b6, out b7, out b8, this.ConnectorType);
                }

                startPoint = new Point(PxStartPointPosition.X, PxStartPointPosition.Y);
                ConnectionPoints.Add(startPoint);
                ConnectionPoints.Add(endPoint);
            }
            else
            {
                Point startPoint, endPoint;

                if (!(Orientation == TreeOrientation.LeftRight || Orientation == TreeOrientation.RightLeft))
                {
                    ConnectorBase.GetOrthogonalLineIntersect(source, target, sourceRect, targetRect, out b1, out b2, out b3, out b4, out b5, out b6, out b7, out b8, out startPoint, out endPoint);
                }
                else
                {
                    ConnectorBase.GetTreeOrthogonalLineIntersect(source, target, sourceRect, targetRect, out b1, out b2, out b3, out b4, out b5, out b6, out b7, out b8, out startPoint, out endPoint);
                }

                if (ConnectionTailPort != null)
                {
                    double width = double.IsNaN(ConnectionTailPort.Width) ? 2.5 : ConnectionTailPort.Width / 2;
                    double height = double.IsNaN(ConnectionTailPort.Height) ? 2.5 : ConnectionTailPort.Height / 2;

                    endPoint = new Point(ConnectionTailPort.PxCenterPosition.X + target.Left + width, height + ConnectionTailPort.PxCenterPosition.Y + target.Top);
                    if ((this.TailNode as Node).RenderTransform != null)
                    {
                        if ((this.TailNode as Node).RenderTransform is RotateTransform)
                        {
                            endPoint = this.GeneralPointRotation(target.Position, endPoint, ((this.TailNode as Node).RenderTransform as RotateTransform).Angle);
                        }

                    }
                }

                if (this.HeadDecoratorGrid != null)
                {
                    if (b5)
                    {
                        (this as LineConnector).HeadDecoratorAngle = -90;
                    }
                    else if (b6)
                    {
                        (this as LineConnector).HeadDecoratorAngle = 90;
                    }
                    else if (b7)
                    {
                        (this as LineConnector).HeadDecoratorAngle = -180;
                    }
                    else if (b8)
                    {
                        (this as LineConnector).HeadDecoratorAngle = 0;
                    }
                }

                if (this.TailDecoratorGrid != null)
                {
                    if (b1)
                    {
                        (this as LineConnector).TailDecoratorAngle = -90;
                    }
                    else if (b2)
                    {
                        (this as LineConnector).TailDecoratorAngle = 90;
                    }
                    else if (b3)
                    {
                        (this as LineConnector).TailDecoratorAngle = -180;
                    }
                    else if (b4)
                    {
                        (this as LineConnector).TailDecoratorAngle = 0;
                    }
                }

                startPoint = new Point(PxStartPointPosition.X, PxStartPointPosition.Y);
                ConnectionPoints.Add(startPoint);
                ConnectionPoints.Add(endPoint);
                ConnectionPoints = this.AddLinePoints(ConnectionPoints);
                this.FindConnectionEnd(ConnectionPoints, startPoint, endPoint, b1, b2, b3, b4, b5, b6, b7, b8);
            }

            int count = ConnectionPoints.Count();
            double x = Math.Pow((ConnectionPoints[0].X - ConnectionPoints[count - 1].X), 2);
            double y = Math.Pow((ConnectionPoints[0].Y - ConnectionPoints[count - 1].Y), 2);
            Distance = Math.Sqrt(x + y);
            return ConnectionPoints;
        }

        /// <summary>
        /// Gets the line points when both head node and tail node are not specified.
        /// </summary>
        /// <returns>The collection of points</returns>
        internal List<Point> GetLinePoints()
        {
            double twozero = 20;// MeasureUnitsConverter.FromPixels(20, DiagramPage.Munits);
            double sourceleft = PxStartPointPosition.X;// MeasureUnitsConverter.FromPixels(StartPointPosition.X, DiagramPage.Munits);
            double sourcetop = PxStartPointPosition.Y;// MeasureUnitsConverter.FromPixels(StartPointPosition.Y, DiagramPage.Munits);
            double dropcentreX = DropPoint.X;// MeasureUnitsConverter.FromPixels(DropPoint.X, DiagramPage.Munits);
            double dropcentreY = DropPoint.Y;// MeasureUnitsConverter.FromPixels(DropPoint.Y, DiagramPage.Munits);
            double targetleft = PxEndPointPosition.X;// MeasureUnitsConverter.FromPixels(EndPointPosition.X, DiagramPage.Munits);
            double targettop = PxEndPointPosition.Y;// MeasureUnitsConverter.FromPixels(EndPointPosition.Y, DiagramPage.Munits);

            bool b1, b2, b3, b4, b5, b6, b7, b8;
            Point s = new Point(PxStartPointPosition.X, PxStartPointPosition.Y);
            Point e = new Point(PxEndPointPosition.X, PxEndPointPosition.Y);
            Rect sourceRect = new Rect();
            Rect targetRect = new Rect();
            NodeInfo target = new NodeInfo();
            target.Position = new Point(PxEndPointPosition.X, PxEndPointPosition.Y);
            targetRect = new Rect(
                                   targetleft,
                                   targettop,
                                    twozero,
                                    twozero);

            NodeInfo source = new NodeInfo();
            source.Position = new Point(PxStartPointPosition.X, PxStartPointPosition.Y);
            sourceRect = new Rect(
                                sourceleft,
                                sourcetop,
                                twozero,
                                twozero);

            ConnectionPoints = new List<Point>();
            if (this.ConnectorType != ConnectorType.Orthogonal)
            {
                Point startPoint = ConnectorBase.GetLineIntersect(source, s, e, sourceRect, out b1, out b2, out b3, out b4, this.ConnectorType);

                Point endPoint = new Point(PxEndPointPosition.X, PxEndPointPosition.Y);

                ConnectionPoints.Add(startPoint);
                ConnectionPoints.Add(endPoint);
            }
            else
            {
                Point startPoint, endPoint;

                if (!(Orientation == TreeOrientation.LeftRight || Orientation == TreeOrientation.RightLeft))
                {
                    ConnectorBase.GetOrthogonalLineIntersect(source, target, sourceRect, targetRect, out b1, out b2, out b3, out b4, out b5, out b6, out b7, out b8, out startPoint, out endPoint);
                }
                else
                {
                    ConnectorBase.GetTreeOrthogonalLineIntersect(source, target, sourceRect, targetRect, out b1, out b2, out b3, out b4, out b5, out b6, out b7, out b8, out startPoint, out endPoint);
                }

                if (this.HeadDecoratorGrid != null)
                {
                    if (b5)
                    {
                        (this as LineConnector).HeadDecoratorAngle = -90;
                    }
                    else if (b6)
                    {
                        (this as LineConnector).HeadDecoratorAngle = 90;
                    }
                    else if (b7)
                    {
                        (this as LineConnector).HeadDecoratorAngle = -180;
                    }
                    else if (b8)
                    {
                        (this as LineConnector).HeadDecoratorAngle = 0;
                    }
                }

                if (this.TailDecoratorGrid != null)
                {
                    if (b1)
                    {
                        (this as LineConnector).TailDecoratorAngle = -90;
                    }
                    else if (b2)
                    {
                        (this as LineConnector).TailDecoratorAngle = 90;
                    }
                    else if (b3)
                    {
                        (this as LineConnector).TailDecoratorAngle = -180;
                    }
                    else if (b4)
                    {
                        (this as LineConnector).TailDecoratorAngle = 0;
                    }
                }

                ConnectionPoints.Add(startPoint);
                ConnectionPoints.Add(endPoint);
                ConnectionPoints = this.AddLinePoints(ConnectionPoints);
                this.FindConnectionEnd(ConnectionPoints, startPoint, endPoint, b1, b2, b3, b4, b5, b6, b7, b8);
            }

            int count = ConnectionPoints.Count();
            double x = Math.Pow((ConnectionPoints[0].X - ConnectionPoints[count - 1].X), 2);
            double y = Math.Pow((ConnectionPoints[0].Y - ConnectionPoints[count - 1].Y), 2);
            Distance = Math.Sqrt(x + y);
            return ConnectionPoints;
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
        /// <returns>The Bezier segment</returns>
        internal BezierSegment GetSegment(double x1, double y1, double x2, double y2, double temp1, double temp2, double num1, bool isTop, bool isBottom, bool isLeft, bool isRight)
        {
            if (isTop)
            {
                return this.Segment(x1, y1, x2, y2, temp1, temp2 - num1);
            }
            else if (isBottom)
            {
                return this.Segment(x1, y1, x2, y2, temp1, temp2 + num1);
            }
            else if (isLeft)
            {
                return this.Segment(x1, y1, x2, y2, temp1 - num1, temp2);
            }
            else
            {
                return this.Segment(x1, y1, x2, y2, temp1 + num1, temp2);
            }
        }

        /// <summary>
        /// Hides the adorner.
        /// </summary>
        protected override void HideAdorner()
        {
        }
        UIElement hitobject;

        /// <summary>
        /// Hittesting 
        /// </summary>
        /// <param name="hitPoint">hit point</param>
        /// <returns>Returns the boolean value whether the hit testing is performed</returns>
        internal bool HitTesting(Point hitPoint)
        {
            IEnumerable<UIElement> hitObjectcoll = VisualTreeHelper.FindElementsInHostCoordinates(hitPoint, Application.Current.RootVisual);
            if (hitObjectcoll.Count() > 0)
            {
                foreach (UIElement hitObject in hitObjectcoll)
                {
                    if (hitObject is ConnectionPort)
                    {
                        ConnectionPort port = hitObject as ConnectionPort;
                        if (port.CenterPortReferenceNo != 0)
                        {
                            //this.hitNodeConnector = null;
                            //this.previousHitNode = null;
                            hitobject = port;
                            if (this.headthumb)
                                this.ConnectionHeadPort = port;
                            else
                                this.ConnectionTailPort = port;

                        }

                        if (this.hitNodeConnector != this.fixedNodeConnection)
                        {
                            port.IsDragOverPort = true;
                        }

                        return true;
                    }
                    else if (hitObject is Node)
                    {
                        //hitobject = hitObject;
                        Node node = hitObject as Node;
                        this.hitNodeConnector = hitObject as Node;
                        this.previousHitNode = hitObject as Node;
                        node.portItems.Visibility = Visibility.Visible;
                        if (this.hitNodeConnector != this.fixedNodeConnection)
                        {
                            this.previousHitNode.IsDragConnectionOver = true;
                        }

                        return true;
                    }

                }
            }

            if (this.previousHitNode != null)
            {
                this.previousHitNode.IsDragConnectionOver = false;
            }

            this.hitNodeConnector = null;
            return false;
        }

        /// <summary>
        /// Invoked when Label editing is started.
        /// </summary>
        public void Labeledit()
        {
            if (IsLabelEditable)
            {
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
                this.UpdateConnectorPathGeometry();
            }
        }

        /// <summary>
        /// Handles the Loaded event of the LineConnector control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void LineConnector_Loaded(object sender, RoutedEventArgs e)
        {
            dc = DiagramPage.GetDiagramControl(this);
            if (this.LabelWidth == 0)
            {
                //this.LabelWidth = Distance;
                this.isdefaulted = true;
            }
            DiagramControl diagramCtrl = DiagramPage.GetDiagramControl(this);
            if (this.IsSelected)
            {
                if (!diagramCtrl.View.SelectionList.Contains(this))
                {
                    diagramCtrl.View.SelectionList.Add(this);
                    ConnectorRoutedEventArgs newEventArgs1 = new ConnectorRoutedEventArgs(this);
                    diagramCtrl.View.OnConnectorSelected(this as LineConnector, newEventArgs1);
                    diagramCtrl.View.oldselectionlist.Add(this);
                }
                else if (this.IsSelected)
                {
                    ConnectorRoutedEventArgs newEventArgs1 = new ConnectorRoutedEventArgs(this);
                    diagramCtrl.View.OnConnectorSelected(this as LineConnector, newEventArgs1);
                    diagramCtrl.View.oldselectionlist.Add(this);
                }
            }
            if (this.IsSelected && !dc.View.SelectionList.Contains(this))
            {
                dc.View.SelectionList.Add(this);
                ConnectorRoutedEventArgs newEventArgs1 = new ConnectorRoutedEventArgs(this);
                dc.View.OnConnectorSelected(this as LineConnector, newEventArgs1);
            }

            this.UpdateConnectorPathGeometry();
            if (this.HeadNode != null && this.TailNode != null)
            {
                this.HeadNodeReferenceNo = (this.HeadNode as Node).ReferenceNo;
                this.TailNodeReferenceNo = (this.TailNode as Node).ReferenceNo;
            }

            try
            {
                this.dview = Node.GetDiagramView(this);
                if (this.dview != null)
                {
                    this.dview.Islineloaded = true;
                }

                if (this.dc != null)
                {
                    if (this.dc.View.Scrollviewer != null && (this.dview.Page as DiagramPage).IsConnectorDropped && this.dc.Model.LayoutType != LayoutType.None)
                    {
                        if (this.dview.IsMouseScrolled)
                        {
                            if ((this.dview.Page as DiagramPage).Minleft >= 0)
                            {
                                this.dview.Scrollviewer.ScrollToHorizontalOffset(Math.Abs((this.dview.Page as DiagramPage).HorValue));
                            }
                            else
                            {
                                this.dview.Scrollviewer.ScrollToHorizontalOffset(Math.Abs((this.dview.Page as DiagramPage).HorValue * this.dview.CurrentZoom));
                            }

                            if ((this.dview.Page as DiagramPage).MinTop >= 0)
                            {
                                this.dview.Scrollviewer.ScrollToVerticalOffset(Math.Abs((this.dview.Page as DiagramPage).VerValue));
                            }
                            else
                            {
                                this.dview.Scrollviewer.ScrollToVerticalOffset(Math.Abs((this.dview.Page as DiagramPage).VerValue * this.dview.CurrentZoom));
                            }
                        }
                        else
                        {
                            this.dc.View.Scrollviewer.ScrollToHorizontalOffset(Math.Abs((this.dc.View.Page as DiagramPage).HorValue) * this.dc.View.CurrentZoom);
                            this.dc.View.Scrollviewer.ScrollToVerticalOffset(Math.Abs((this.dc.View.Page as DiagramPage).VerValue) * this.dc.View.CurrentZoom);
                        }
                    }
                }

                (this.dview.Page as DiagramPage).IsConnectorDropped = false;
                (this.dview.Page as DiagramPage).IsDiagrampageLoaded = false;
                if (!this.dview.IsPageEditable)
                {
                    DiagramView.PageEdit = false;
                    IsLabelEditable = false;
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// Called when the mouse button is clicked twice.
        /// </summary>
        /// <param name="position">Mouse Position</param>
        /// <returns>true if double clicked, false otherwise</returns>
        public bool IsDoubleClick(Point position)
        {
            if (((DateTime.Now.Subtract(this.lastLineClick).TotalMilliseconds < 500) && (Math.Abs((double)(this.lastLinePoint.X - position.X)) <= 2)) && (Math.Abs((double)(this.lastLinePoint.Y - position.Y)) <= 2))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Provides class handling for the MouseLeftButtonDown routed event that occurs when
        /// the mouse left button is released over this control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The MouseButtonEventArgs.</param>
        void LineConnector_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            mouseup = true;
            if (e.OriginalSource is Border)
            {
                if ((e.OriginalSource as Border).Name.Contains("PART_HeadBorder"))
                {
                    this.headthumb = true;
                }
                else if ((e.OriginalSource as Border).Name.Contains("PART_TailBorder"))
                {
                    this.tailthumb = true;
                }

                this.linedragging = true;
                this.CaptureMouse();
            }
            else
            {
                this.linedragging = false;
            }

            if (this.dc != null && this.dc.View.IsPageEditable && !this.IsGrouped)
            {
                DiagramPage diagramPanel = this.dc.View.Page as DiagramPage;

                if (this.IsDoubleClick(e.GetPosition(diagramPanel as Panel)) && this.IsLabelEditable)
                {
                    ConnRoutedEventArgs newEventArgs = new ConnRoutedEventArgs(this);
                    dview.OnConnectorDoubleClick(this, newEventArgs);
                    LabelEditConnRoutedEventArgs newEventArgs1 = new LabelEditConnRoutedEventArgs(this.linelabel.Text, this);
                    dview.OnConnectorStartLabelEdit(this, newEventArgs1);
                    this.linelabel.Visibility = Visibility.Collapsed;
                    this.linetext.Visibility = Visibility.Visible;
                    this.linetext.SelectionBackground = new SolidColorBrush(Colors.Blue);
                    this.linetext.Focus();
                    this.linetext.SelectAll();
                    this.LostFocus += new RoutedEventHandler(Node_LostFocus);

                }
                else
                {
                    if (diagramPanel != null)
                    {
                        if ((Keyboard.Modifiers & (ModifierKeys.Shift | ModifierKeys.Control)) != ModifierKeys.None)
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
                        this.lastLineClick = DateTime.Now;
                        this.lastLinePoint = e.GetPosition(diagramPanel as Panel);
                    }
                }
            }
        }

        void Node_LostFocus(object sender, RoutedEventArgs e)
        {
            this.linetext.Focus();
            this.linetext.SelectAll();
            this.linetext.LostFocus += new RoutedEventHandler(linetext_LostFocus);
        }


        void linetext_LostFocus(object sender, RoutedEventArgs e)
        {
            this.linetext.LostFocus -= new RoutedEventHandler(linetext_LostFocus);
            this.Label = this.linetext.Text;
            this.linelabel.Text = this.linetext.Text;
            this.linelabel.Visibility = Visibility.Visible;
            this.linetext.Visibility = Visibility.Collapsed;
            this.LostFocus -= new RoutedEventHandler(Node_LostFocus);

        }

        /// <summary>
        /// Provides class handling for the MouseLeftButtonUp routed event that occurs when
        /// the mouse left button is released over this control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The MouseButtonEventArgs.</param>
        private void Lineconnector_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (this.linedragging)
            {
                this.dview.tUndoStack.Push(new LineOperation(LineOperations.Dragged, this));
                if (IsLinedrag)
                {                    
                    //ConnDragEndRoutedEventArgs newEventArgs = new ConnDragEndRoutedEventArgs(this);
                    //dview.OnConnectorDragEnd(this, newEventArgs);                   
                    (dc.View.Page as DiagramPage).InvalidateMeasure();
                    IsLinedrag = false;
                }
                //else
                //{
                //    if (dc.View.IsPageEditable)
                //    {
                //        ConnectorRoutedEventArgs NewEvtArgs = new ConnectorRoutedEventArgs(this);
                //        dview.OnConnectorClick(this, NewEvtArgs);
                //    }
                //}

                if (this.LineCanvas.Children.ElementAt(this.LineCanvas.Children.Count() - 1) is Path)
                {
                    this.LineCanvas.Children.Remove(this.LineCanvas.Children.ElementAt(this.LineCanvas.Children.Count() - 1));
                }

                if (this.headthumb)
                {
                    if (this.hitNodeConnector == null)
                    {
                        this.PxStartPointPosition = e.GetPosition(this);
                        if (this.HeadNode != null)
                        {
                            this.HeadNode.Edges.Remove(this);
                            this.HeadNode.OutEdges.Remove(this);
                        }
                        this.ConnectionHeadPort = null;
                        this.HeadNode = null;
                    }
                    else
                    {
                        this.hitNodeConnector.IsDragConnectionOver = false;
                        this.HeadNode = this.hitNodeConnector;
                        this.HeadNodeReferenceNo = this.HeadNode.ReferenceNo;
                        if (this.ConnectionHeadPort != null)
                            this.HeadPortReferenceNo = this.ConnectionHeadPort.PortReferenceNo;
                    }
                }
                else
                {
                    if (this.hitNodeConnector == null)
                    {
                        this.PxEndPointPosition = e.GetPosition(this);
                        if (this.TailNode != null)
                        {
                            this.TailNode.Edges.Remove(this);
                            this.TailNode.OutEdges.Remove(this);
                        }
                        this.ConnectionTailPort = null;
                        this.TailNode = null;
                    }
                    else
                    {
                        this.hitNodeConnector.IsDragConnectionOver = false;                        
                        this.TailNode = this.hitNodeConnector;
                        this.TailNodeReferenceNo = this.TailNode.ReferenceNo;
                        if (this.ConnectionTailPort != null)
                            this.TailPortReferenceNo = this.ConnectionTailPort.PortReferenceNo;
                    }
                }
                ConnDragEndRoutedEventArgs newEventArgs;
                if (this.hitNodeConnector == null && this.fixedNodeConnection == null)
                {
                    newEventArgs = new ConnDragEndRoutedEventArgs(this);
                    dview.OnConnectorDragEnd(this, newEventArgs);
                }

                else if (this.hitNodeConnector != null && this.fixedNodeConnection == null)
                {
                    newEventArgs = new ConnDragEndRoutedEventArgs(this.hitNodeConnector as Node, this as LineConnector);
                    dview.OnConnectorDragEnd(this, newEventArgs);
                }

                else if (this.hitNodeConnector == null && this.fixedNodeConnection != null)
                {
                    newEventArgs = new ConnDragEndRoutedEventArgs(this as LineConnector, this.fixedNodeConnection as Node);
                    dview.OnConnectorDragEnd(this, newEventArgs);
                }

                else
                {
                    newEventArgs = new ConnDragEndRoutedEventArgs(this.fixedNodeConnection as Node, this.hitNodeConnector as Node, this as LineConnector);
                    dview.OnConnectorDragEnd(this, newEventArgs);
                }

                if (this.hitNodeConnector != null)
                {
                    foreach (ConnectionPort port in this.hitNodeConnector.Ports)
                    {
                        port.IsDragOverPort = false;
                    }
                }

                this.hitNodeConnector = null;
                this.ConnectorPathGeometry = null;
                this.LineStyle.StrokeDashArray = null;
                this.linedragging = false;
                this.ReleaseMouseCapture();
            }
            else
            {
                if (dc.View.IsPageEditable)
                {
                    ConnectorRoutedEventArgs NewEvtArgs = new ConnectorRoutedEventArgs(this);
                    dview.OnConnectorClick(this, NewEvtArgs);
                }
            }

            this.UpdateConnectorPathGeometry();
            this.headthumb = false;
            this.tailthumb = false;
            bool notselected = true;
            if (this.dc.View.IsPageEditable && this.IsGrouped)
            {
                if (!this.dc.View.IsPanEnabled && !this.isdoubleclicked)
                {
                    IDiagramPage mdiagramPage = VisualTreeHelper.GetParent(this) as IDiagramPage;
                    //// update selection
                    if (mdiagramPage != null)
                    {
                        if ((Keyboard.Modifiers & (ModifierKeys.Shift | ModifierKeys.Control)) != ModifierKeys.None)
                        {
                            if (this.IsSelected)
                            {
                                mdiagramPage.SelectionList.Remove(this);
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
                                            mdiagramPage.SelectionList.Add(this);
                                        }
                                        else
                                        {
                                            mdiagramPage.SelectionList.Add(this.Groups[this.Groups.Count - 1]);
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
                                                        mdiagramPage.SelectionList.Add(groupednodes[groupednodes.Count - 1]);
                                                    }
                                                    else
                                                    {
                                                        mdiagramPage.SelectionList.Add(groupednodes[index - 1]);
                                                    }

                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }
                                else if (this.IsGrouped)
                                {
                                    mdiagramPage.SelectionList.Add(this.Groups[this.Groups.Count - 1]);
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
                                        mdiagramPage.SelectionList.Select(this);
                                    }
                                    else
                                    {
                                        mdiagramPage.SelectionList.Select(this.Groups[this.Groups.Count - 1]);
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
                                                    mdiagramPage.SelectionList.Select(groupednodes[groupednodes.Count - 1]);
                                                }
                                                else
                                                {
                                                    mdiagramPage.SelectionList.Select(groupednodes[index - 1]);
                                                }

                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                            else if (this.IsGrouped)
                            {
                                mdiagramPage.SelectionList.Select(this.Groups[this.Groups.Count - 1]);
                            }
                        }
                    }
                }
            }
            mouseup = true;     
            this.isdoubleclicked = false;
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

        protected override void OnMouseRightButtonUp(MouseButtonEventArgs e)
        {
            if (dview != null)
            {
                if (dview.IsPageEditable && dview.LineConnectorContextMenu == null && this.ContextMenu == null)
                {
                    ContextMenuControl objContextmenu = new ContextMenuControl();
                    ContextMenu = objContextmenu;

                    ContextMenuControlItem m = new ContextMenuControlItem();
                    m.Header = ContextMenu_Order;
                    ContextMenuControlItem m1 = new ContextMenuControlItem();
                    m1.Header = ContextMenu_Order_BringToFront;
                    m1.Click += new RoutedEventHandler(M1_Click);
                    m.Items.Add(m1);
                    ContextMenuControlItem m2 = new ContextMenuControlItem();
                    m2.Header = ContextMenu_Order_BringForward;
                    m2.Click += new RoutedEventHandler(M2_Click);
                    m.Items.Add(m2);
                    ContextMenuControlItem m3 = new ContextMenuControlItem();
                    m3.Header = ContextMenu_Order_SendBackward;
                    m3.Click += new RoutedEventHandler(M3_Click);
                    m.Items.Add(m3);
                    ContextMenuControlItem m4 = new ContextMenuControlItem();
                    m4.Header = ContextMenu_Order_SendToBack;
                    m4.Click += new RoutedEventHandler(M4_Click);
                    m.Items.Add(m4);

                    ContextMenuControlItem group = new ContextMenuControlItem();
                    group.Header = ContextMenu_Grouping;
                    ContextMenuControlItem g1 = new ContextMenuControlItem();
                    g1.Header = ContextMenu_Grouping_Group;
                    g1.Click += new RoutedEventHandler(G1_Click);
                    group.Items.Add(g1);
                    ContextMenuControlItem g2 = new ContextMenuControlItem();
                    g2.Header = ContextMenu_Grouping_Ungroup;
                    g2.Click += new RoutedEventHandler(G2_Click);
                    group.Items.Add(g2);

                    ContextMenuControlItem del = new ContextMenuControlItem();
                    del.Header = ContextMenu_Delete;
                    del.Click += new RoutedEventHandler(Del_Click);

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

            if (this.ContextMenu != null)
            {
                linepopup = true;
                ContextMenu.OpenPopup(e.GetPosition(null));
            }
            base.OnMouseRightButtonUp(e);
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.UIElement.MouseRightButtonDown"/> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Input.MouseButtonEventArgs"/> that contains the event data.</param>
        protected override void OnMouseRightButtonDown(MouseButtonEventArgs e)
        {
            DiagramPage diagramPanel = this.dc.View.Page as DiagramPage;
            if (diagramPanel != null)
            {
                if ((Keyboard.Modifiers & (ModifierKeys.Shift | ModifierKeys.Control)) != ModifierKeys.None)
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
            dview.DeleteObjects(dview);
        }

        /// <summary>
        /// Handles the Click event of the bring to front menu item.
        /// </summary>
        /// <param name="sender">The diagram view.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void M1_Click(object sender, RoutedEventArgs e)
        {
            dc.BringToFront.Execute(dc.View);
        }

        /// <summary>
        /// Handles the Click event of the bring forward menu item.
        /// </summary>
        /// <param name="sender">The diagram view.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void M2_Click(object sender, RoutedEventArgs e)
        {
            dc.BringForward.Execute(dc.View);
        }

        /// <summary>
        /// Handles the Click event of the send backward menu item.
        /// </summary>
        /// <param name="sender">The diagram view.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void M3_Click(object sender, RoutedEventArgs e)
        {
            dc.SendBackward.Execute(dc.View);
        }

        /// <summary>
        /// Handles the Click event of the send to back menu item.
        /// </summary>
        /// <param name="sender">The diagram view.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void M4_Click(object sender, RoutedEventArgs e)
        {
            dc.SendToBack.Execute(dc.View);
        }

        /// <summary>
        /// Handles the Click event of the group menu item.
        /// </summary>
        /// <param name="sender">The diagram view.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void G1_Click(object sender, RoutedEventArgs e)
        {
            dc.Group.Execute(dc.View);
        }

        /// <summary>
        /// Handles the Click event of the ungroup menu item.
        /// </summary>
        /// <param name="sender">The diagram view.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void G2_Click(object sender, RoutedEventArgs e)
        {
            dc.UnGroup.Execute(dc.View);
        }

        /// <summary>
        /// Provides class handling for the MouseMove routed event that occurs when
        /// the mouse left button is released over this control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The MouseButtonEventArgs.</param>
        void LineConnector_MouseMove(object sender, MouseEventArgs e)
        {
            if (this.linedragging)
            {
                if (this.LineCanvas.Children.ElementAt(this.LineCanvas.Children.Count() - 1) is Path)
                {
                    this.LineCanvas.Children.Remove(this.LineCanvas.Children.ElementAt(this.LineCanvas.Children.Count() - 1));
                }

                this.Cursor = Cursors.Hand;
                DoubleCollection dcoll = new DoubleCollection();
                dcoll.Add(3);
                dcoll.Add(3);
                this.LineStyle.StrokeDashArray = dcoll;
                if (this.headthumb)
                {
                    if (this.TailNode != null)
                    {
                        this.fixedNodeConnection = this.TailNode as Node;
                    }
                    else
                    {
                        this.fixedNodeConnection = null;
                    }
                    if (this.HeadNode != null)
                    {
                        movableNodeConnection = this.HeadNode as Node;
                    }
                    else
                    {
                        movableNodeConnection = null;
                    }
                }
                else if (this.tailthumb)
                {
                    if (this.HeadNode != null)
                    {
                        this.fixedNodeConnection = this.HeadNode as Node;
                    }
                    else
                    {
                        this.fixedNodeConnection = null;
                    }
                    if (this.TailNode != null)
                    {
                        movableNodeConnection = this.TailNode as Node;
                    }
                    else
                    {
                        movableNodeConnection = null;
                    }
                }
                Point hp = e.GetPosition(Application.Current.RootVisual);
                bool foundNode = this.HitTesting(hp);
                Path path = new Path();
                path.Stroke = this.LineStyle.Stroke;
                path.StrokeThickness = this.LineStyle.StrokeThickness;

                if (foundNode && this.hitNodeConnector != null)
                {
                    path.Data = this.UpdateConnectorAdornerPathGeometry(e.GetPosition(Application.Current.RootVisual), e);
                }
                else
                {
                    if (this.previousHitNode != null)
                    {
                        this.previousHitNode.portItems.Visibility = Visibility.Collapsed;
                        foreach (ConnectionPort port in this.previousHitNode.Ports)
                        {
                            port.IsDragOverPort = false;
                        }
                    }
                    path.Data = this.UpdateConnectorAdornerPathGeometry(e.GetPosition(Application.Current.RootVisual), e);
                    foreach (Node n in this.dc.Model.Nodes)
                    {
                        n.IsDragConnectionOver = false;
                    }
                }

                this.LineCanvas.Children.Add(path);
                if (mouseup)
                {
                    ConnDragRoutedEventArgs newEventArgs;
                    if (fixedNodeConnection != null && movableNodeConnection != null)
                    {
                        newEventArgs = new ConnDragRoutedEventArgs(this.fixedNodeConnection as Node, this.movableNodeConnection as Node, this);
                    }
                    else
                        if (fixedNodeConnection == null && movableNodeConnection != null)
                        {
                            newEventArgs = new ConnDragRoutedEventArgs(this.movableNodeConnection as Node, this);
                        }
                        else if (fixedNodeConnection != null && movableNodeConnection == null)
                        {
                            newEventArgs = new ConnDragRoutedEventArgs(this, fixedNodeConnection as Node);
                        }
                        else
                        {
                            newEventArgs = new ConnDragRoutedEventArgs(this);
                        }
                    dview.OnConnectorDragStart(this, newEventArgs);
                    this.IsLinedrag = true;
                    mouseup = false;
                }
            }

            else
            {
                this.Cursor = Cursors.Arrow;

            }
        }

        //private double lineangle = 0;
        /// <summary>
        /// Invoked whenever application code or internal processes call
        /// <see cref="System.Windows.FrameworkElement.ApplyTemplate"/> method.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            this.dc = DiagramPage.GetDiagramControl((FrameworkElement)this);
            if (dc != null)
            {
                if (dc.View != null && dc.View.Page != null)
                {
                    //System.Windows.Data.Binding measure = new System.Windows.Data.Binding("MeasurementUnits");
                    //measure.Source = dc.View.Page;
                    //base.SetBinding(ConnectorBase.MeasurementUnitProperty, measure);
                }
            }
            this.HeadDecoratorGrid = GetTemplateChild("PART_HeadDecoratorGrid") as Grid;
            this.TailDecoratorGrid = GetTemplateChild("PART_TailDecoratorGrid") as Grid;
            this.LineSelection(this);
            this.LineCanvas = GetTemplateChild("PART_LineCanvas") as Canvas;
            this.HeadShape = GetTemplateChild("PART_HeadDecoratorAnchorPath") as Path;
            this.TailShape = GetTemplateChild("PART_SinkAnchorPath") as Path;
            this.linetext = GetTemplateChild("PART_TextBox1") as TextBox;
            this.linelabel = GetTemplateChild("PART_TextBlock1") as TextBlock;
            SetShape("Head");
            SetShape("Tail");
            this.labelGrid = GetTemplateChild("PART_LabelGrid") as Grid;
            RotateTransform transform = new RotateTransform();
            transform.Angle = this.LineAngle + this.LabelAngle;
            (this.linetext.Parent as Grid).RenderTransform = transform;
            this.labelGrid.RenderTransform = transform;
            if (this.dc != null)
            {
                if (DiagramControl.IsPageLoaded)
                {
                    (this.dc.View.Page as DiagramPage).IsDiagrampageLoaded = true;
                }
                else
                {
                    (this.dc.View.Page as DiagramPage).IsDiagrampageLoaded = false;
                }

                if (this.dc.View.Scrollviewer != null && this.dc.Model.LayoutType == LayoutType.None && !(this.dc.View.Page as DiagramPage).IsDiagrampageLoaded)
                {
                    this.dc.View.Scrollviewer.ScrollToHorizontalOffset(Math.Abs((this.dc.View.Page as DiagramPage).HorValue) * this.dc.View.CurrentZoom);
                    this.dc.View.Scrollviewer.ScrollToVerticalOffset(Math.Abs((this.dc.View.Page as DiagramPage).VerValue) * this.dc.View.CurrentZoom);
                }

                if (this.dc.View.Islineloaded)
                {
                    this.UpdateConnectorPathGeometry();
                    this.dc.View.Islineloaded = false;
                }
            }
            RotateTransform rottransform = new RotateTransform();
            rottransform.Angle = this.HeadDecoratorAngle;
            rottransform.CenterX = 5;
            rottransform.CenterY = 5;
            TranslateTransform transtransform = new TranslateTransform();
            transtransform.X = -5;
            transtransform.Y = -5;
            if (this.HeadDecoratorGrid != null)
            {
                TransformGroup group = new TransformGroup();
                group.Children.Add(rottransform);
                group.Children.Add(transtransform);
                this.HeadDecoratorGrid.RenderTransform = group;
            }

            rottransform = new RotateTransform();
            rottransform.Angle = this.TailDecoratorAngle;
            rottransform.CenterX = 5;
            rottransform.CenterY = 5;
            transtransform = new TranslateTransform();
            transtransform.X = -5;
            transtransform.Y = -5;
            if (this.TailDecoratorGrid != null)
            {
                TransformGroup group = new TransformGroup();
                group.Children.Add(rottransform);
                group.Children.Add(transtransform);
                this.TailDecoratorGrid.RenderTransform = group;
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

        /// <summary>
        /// Event raised when the connector path geometry is changed.
        /// </summary>
        /// <param name="d">object whose path geometry is changed</param>
        /// <param name="e">DependencyPropertyChanged event</param>
        private static void OnConnectorPathGeometryChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            LineConnector line = d as LineConnector;
            line.UpdateDecoratorPosition(line);
        }

        /// <summary>
        /// Event raised when the head decorator angel is changed.
        /// </summary>
        /// <param name="d">object whose head decorator angel is changed</param>
        /// <param name="e">DependencyPropertyChanged event</param>
        private static void OnHeadDecoratorAngleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            LineConnector line = d as LineConnector;
            RotateTransform rottransform = new RotateTransform();
            rottransform.Angle = line.HeadDecoratorAngle;
            rottransform.CenterX = 5;
            rottransform.CenterY = 5;
            TranslateTransform transtransform = new TranslateTransform();
            transtransform.X = -5;
            transtransform.Y = -5;
            if (line.HeadDecoratorGrid != null)
            {
                TransformGroup group = new TransformGroup();
                group.Children.Add(rottransform);
                group.Children.Add(transtransform);
                line.HeadDecoratorGrid.RenderTransform = group;
            }
        }

        /// <summary>
        /// Event raised when the IsSelected is changed.
        /// </summary>
        /// <param name="d">object whose IsSelected is changed</param>
        /// <param name="e">DependencyPropertyChanged event</param>
        private static void OnIsSelectedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            LineConnector line = d as LineConnector;
            line.LineSelection(line);
        }

        private static void OnLineAngleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            LineConnector line = d as LineConnector;
            RotateTransform rottransform = new RotateTransform();
            rottransform.Angle = line.LineAngle;
            if (line.linetext != null)
                (line.linetext.Parent as Grid).RenderTransform = rottransform;
            if (line.labelGrid != null)
                line.labelGrid.RenderTransform = rottransform;
        }

        private static void OnLabelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            string oldvalue = (string)e.OldValue;
            string newvalue = (string)e.NewValue;

        }

        /// <summary>
        /// Event raised when the tail decorator angel is changed.
        /// </summary>
        /// <param name="d">object whose tail decorator angel is changed</param>
        /// <param name="e">DependencyPropertyChanged event</param>
        private static void OnTailDecoratorAngleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            LineConnector line = d as LineConnector;
            RotateTransform rottransform = new RotateTransform();
            rottransform.Angle = line.TailDecoratorAngle;
            rottransform.CenterX = 5;
            rottransform.CenterY = 5;
            TranslateTransform transtransform = new TranslateTransform();
            transtransform.X = -5;
            transtransform.Y = -5;
            if (line.TailDecoratorGrid != null)
            {
                TransformGroup group = new TransformGroup();
                group.Children.Add(rottransform);
                group.Children.Add(transtransform);
                line.TailDecoratorGrid.RenderTransform = group;
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
        internal BezierSegment Segment(double x1, double y1, double x2, double y2, double temp1, double temp2)
        {
            BezierSegment segment = new BezierSegment
            {
                Point1 = new Point(x1, y1),
                Point2 = new Point(temp1, temp2),
                Point3 = new Point(x2, y2)
            };

            return segment;
        }

        /// <summary>
        /// Shows the adorner
        /// </summary>
        protected override void ShowAdorner()
        {
        }

        /// <summary>
        /// updates connector path geometry
        /// </summary>
        /// <param name="position">line connector </param>
        /// <param name="e">MouseEventArgs</param>
        /// <returns>PathGeometry</returns>
        internal PathGeometry UpdateConnectorAdornerPathGeometry(Point position, MouseEventArgs e)
        {
            LineConnector lineconnector = this;

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
                if (this.fixedNodeConnection != null)
                {
                    source = this.fixedNodeConnection.GetInfo();
                    sourceRect = new Rect(
                                          source.Left,
                                          source.Top,
                                          source.Size.Width,
                                          source.Size.Height);
                    p = position;// MeasureUnitsConverter.FromPixels(position, source.MeasurementUnit);
                    if (this.HitTesting(position))
                    {
                        if (this.hitNodeConnector != null && hitobject is Node)
                        {
                            NodeInfo target = this.hitNodeConnector.GetInfo();
                            double hitwidth = this.hitNodeConnector.Width;// MeasureUnitsConverter.FromPixels(this.hitNodeConnector.Width, target.MeasurementUnit);
                            double hitheight = this.hitNodeConnector.Height;// MeasureUnitsConverter.FromPixels(this.hitNodeConnector.Height, target.MeasurementUnit);
                            rectTarget = new Rect(this.hitNodeConnector.PxLogicalOffsetX, this.hitNodeConnector.PxLogicalOffsetY, hitwidth, hitheight);
                            if (this.headthumb)
                            {
                                ConnectorBase.GetOrthogonalLineIntersect(target, source, rectTarget, sourceRect, out isTop, out isBottom, out isLeft, out isRight, out tisTop, out tisBottom, out tisLeft, out tisRight, out startPoint, out endPoint);
                            }
                            else
                            {
                                ConnectorBase.GetOrthogonalLineIntersect(source, target, sourceRect, rectTarget, out isTop, out isBottom, out isLeft, out isRight, out tisTop, out tisBottom, out tisLeft, out tisRight, out startPoint, out endPoint);
                            }

                            x1 = startPoint.X;
                            y1 = startPoint.Y;
                            x2 = endPoint.X;
                            y2 = endPoint.Y;
                        }
                        else
                        {
                            if (!this.headthumb)
                            {
                                x1 = this.fixedNodeConnection.PxPosition.X;
                                y1 = this.fixedNodeConnection.PxPosition.Y;
                                x2 = e.GetPosition(this).X;
                                y2 = e.GetPosition(this).Y;
                            }
                            else
                            {
                                x2 = this.fixedNodeConnection.PxPosition.X;
                                y2 = this.fixedNodeConnection.PxPosition.Y;
                                x1 = e.GetPosition(this).X;
                                y1 = e.GetPosition(this).Y;
                            }
                        }
                    }
                    else
                    {
                        double twozero = 20;// MeasureUnitsConverter.FromPixels(20, DiagramPage.Munits);
                        double targetleft = e.GetPosition(this).X;// MeasureUnitsConverter.FromPixels(e.GetPosition(this).X, DiagramPage.Munits);
                        double targettop = e.GetPosition(this).Y;// MeasureUnitsConverter.FromPixels(e.GetPosition(this).Y, DiagramPage.Munits);
                        NodeInfo pointend = new NodeInfo();
                        Rect pointendRect = new Rect();
                        if (!this.headthumb)
                        {
                            lineconnector.ConnectionTailPort = null;
                            pointendRect = new Rect(
                                                  targetleft,
                                                  targettop,
                                                  twozero,
                                                  twozero);
                            pointend.Position = new Point(e.GetPosition(this).X + 25, e.GetPosition(this).Y + 25);

                            if ((lineconnector as LineConnector).HeadNode != null)
                            {
                                NodeInfo src = ((lineconnector as LineConnector).HeadNode as Node).GetInfo();
                                Rect rectsrc = new Rect(
                                    src.Left,
                                    src.Top,
                                    src.Size.Width,
                                    src.Size.Height);
                                Point sp1 = new Point(src.Position.X, src.Position.Y);

                                if (!this.headthumb)
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

                            endPoint = e.GetPosition(this);
                        }
                        else
                        {
                            lineconnector.ConnectionHeadPort = null;
                            pointendRect = new Rect(
                                                    targetleft,
                                                     targettop,
                                                     twozero,
                                                     twozero);
                            pointend.Position = new Point(e.GetPosition(this).X, e.GetPosition(this).Y);
                            if ((lineconnector as LineConnector).TailNode != null)
                            {
                                NodeInfo target = ((lineconnector as LineConnector).TailNode as Node).GetInfo();
                                Rect recttarget = new Rect(
                                                            target.Left,
                                                            target.Top,
                                                            target.Size.Width,
                                                            target.Size.Height);
                                Point ep1 = new Point(target.Position.X, target.Position.Y);
                                if (!this.headthumb)
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

                            startPoint = e.GetPosition(this);
                        }
                    }

                    x1 = startPoint.X;
                    y1 = startPoint.Y;
                    x2 = endPoint.X;
                    y2 = endPoint.Y;
                }
                else
                {
                    if (!this.headthumb)
                    {
                        x1 = (lineconnector as LineConnector).PxStartPointPosition.X;
                        y1 = (lineconnector as LineConnector).PxStartPointPosition.Y;
                        x2 = e.GetPosition(this).X;
                        y2 = e.GetPosition(this).Y;
                    }
                    else
                    {
                        x2 = (lineconnector as LineConnector).PxEndPointPosition.X;
                        y2 = (lineconnector as LineConnector).PxEndPointPosition.Y;
                        x1 = e.GetPosition(this).X;
                        y1 = e.GetPosition(this).Y;
                    }
                }

                PathFigure pathfigure = new PathFigure();
                double num = Math.Max((double)(Math.Abs((double)(x2 - x1)) / 2.0), (double)20.0);
                pathfigure.StartPoint = new Point(x1, y1);
                BezierSegment segment = new BezierSegment();
                if (isBottom)
                {
                    segment = this.GetSegment(x1, y1 + num, x2, y2, x2, y2, num, tisTop, tisBottom, tisLeft, tisRight);
                }
                else if (isTop)
                {
                    segment = this.GetSegment(x1, y1 - num, x2, y2, x2, y2, num, tisTop, tisBottom, tisLeft, tisRight);
                }
                else if (isRight)
                {
                    segment = this.GetSegment(x1 + num, y1, x2, y2, x2, y2, num, tisTop, tisBottom, tisLeft, tisRight);
                }
                else
                {
                    segment = this.GetSegment(x1 - num, y1, x2, y2, x2, y2, num, tisTop, tisBottom, tisLeft, tisRight);
                }

                pathfigure.Segments.Add(segment);
                pathgeometry.Figures.Add(pathfigure);
                return pathgeometry;
            }
            else
            {
                if (this.fixedNodeConnection != null)
                {
                    PathGeometry pathgeometry = new PathGeometry();
                    List<Point> connectionPoints = this.GetAdornerLinePoints(this.fixedNodeConnection.GetInfo(), position, e);
                    if (connectionPoints.Count > 0)
                    {
                        PathFigure pathfigure = new PathFigure();
                        pathfigure.StartPoint = connectionPoints[0];
                        PolyLineSegment polyline = new PolyLineSegment();
                        foreach (Point p in connectionPoints)
                        {
                            polyline.Points.Add(p);
                        }

                        pathfigure.Segments.Add(polyline);
                        pathgeometry.Figures.Add(pathfigure);
                    }

                    return pathgeometry;
                }
                else
                {
                    PathGeometry pathgeometry = new PathGeometry();
                    List<Point> connectionPoints = this.GetAdornerLinePoints(e);
                    if (connectionPoints.Count > 0)
                    {
                        PathFigure pathfigure = new PathFigure();
                        pathfigure.StartPoint = connectionPoints[0];
                        PolyLineSegment polyline = new PolyLineSegment();
                        foreach (Point p in connectionPoints)
                        {
                            polyline.Points.Add(p);
                        }

                        pathfigure.Segments.Add(polyline);
                        pathgeometry.Figures.Add(pathfigure);
                    }

                    return pathgeometry;
                }
            }
        }

        /// <summary>
        /// Called whenever the head node, tail node or position of the node is changed. 
        /// </summary>
        public override void
            UpdateConnectorPathGeometry()
        {
            if (this.isdefaulted)
            {
                //this.LabelWidth = Distance;
            }

            if (this.dview == null)
            {
                this.dview = Node.GetDiagramView(this);
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

            if (this.HeadNode != null)
            {
                source = (this.HeadNode as Node).GetInfo();

                cs = this.PxConnectionEndSpace;// MeasureUnitsConverter.FromPixels(this.ConnectionEndSpace, source.MeasurementUnit);

                sourceRect = new Rect(
                                   source.Left - 10,
                                   source.Top - 10,
                                   source.Size.Width + 10,
                                   source.Size.Height + 10);
                startPoint = new Point(source.Position.X, source.Position.Y);

            }
            else
            {
                startPoint = new Point(PxStartPointPosition.X, PxStartPointPosition.Y);
                s = new Point(PxStartPointPosition.X, PxStartPointPosition.Y);
                sourceRect = new Rect(
                                    sourceleft - 10,
                                    sourcetop - 10,
                                    twozero + 10,
                                    twozero + 10);
                source.Position = new Point(PxStartPointPosition.X + 25, PxStartPointPosition.Y + 25);
            }

            if (this.TailNode != null)
            {
                target = (this.TailNode as Node).GetInfo();
                cs = this.PxConnectionEndSpace;// MeasureUnitsConverter.FromPixels(this.ConnectionEndSpace, target.MeasurementUnit);
                targetRect = new Rect(
                                      target.Left - 10,
                                      target.Top - 10,
                                      target.Size.Width + 10,
                                      target.Size.Height + 10);
                endPoint = new Point(target.Position.X, target.Position.Y);
            }
            else
            {
                e = new Point(PxEndPointPosition.X, PxEndPointPosition.Y);
                endPoint = new Point(PxEndPointPosition.X, PxEndPointPosition.Y);
                targetRect = new Rect(
                                      targetleft - 10,
                                      targettop = 10,
                                      twozero + 10,
                                      twozero + 10);
                target.Position = new Point(PxEndPointPosition.X - 25, PxEndPointPosition.Y - 25);
            }
            if (this.linelabel == null) return;
            if (this.ConnectorType == ConnectorType.Bezier)
            {
                try
                {
                    if (this.HeadNode != null || this.TailNode != null)
                    {
                        sp = startPoint;// MeasureUnitsConverter.FromPixels(startPoint, source.MeasurementUnit);
                        ep = endPoint;// MeasureUnitsConverter.FromPixels(endPoint, source.MeasurementUnit);
                    }

                    if (!targetRect.Contains(sp) && !sourceRect.Contains(ep))
                    {
                        if (!(Orientation == TreeOrientation.LeftRight || Orientation == TreeOrientation.RightLeft))
                        {
                            ConnectorBase.GetOrthogonalLineIntersect(source, target, sourceRect, targetRect, out isTop, out isBottom, out isLeft, out isRight, out tisTop, out tisBottom, out tisLeft, out tisRight, out s, out e);
                        }
                        else
                        {
                            ConnectorBase.GetTreeOrthogonalLineIntersect(source, target, sourceRect, targetRect, out isTop, out isBottom, out isLeft, out isRight, out tisTop, out tisBottom, out tisLeft, out tisRight, out s, out e);
                        }
                    }

                    if (this.HeadNode != null)
                    {
                        if (this.ConnectionHeadPort == null)
                        {
                            sp = startPoint;// MeasureUnitsConverter.FromPixels(startPoint, source.MeasurementUnit);
                        }
                        else
                        {
                            s = new Point(source.Left + this.ConnectionHeadPort.PxCenterPosition.X + this.ConnectionHeadPort.Width / 2, source.Top + ConnectionHeadPort.PxCenterPosition.Y + this.ConnectionHeadPort.Height / 2);
                            if ((this.HeadNode as Node).RenderTransform != null)
                            {
                                if ((this.HeadNode as Node).RenderTransform is RotateTransform)
                                {
                                    s = this.GeneralPointRotation(source.Position, s, ((this.HeadNode as Node).RenderTransform as RotateTransform).Angle);
                                }

                            }

                        }
                    }

                    if (this.TailNode != null)
                    {
                        if (this.ConnectionTailPort == null)
                        {
                            ep = endPoint;// MeasureUnitsConverter.FromPixels(endPoint, source.MeasurementUnit);
                        }
                        else
                        {
                            e = new Point(target.Left + this.ConnectionTailPort.PxCenterPosition.X + this.ConnectionTailPort.Width / 2, target.Top + ConnectionTailPort.PxCenterPosition.Y + this.ConnectionTailPort.Height / 2);
                            if ((this.TailNode as Node).RenderTransform != null)
                            {
                                if ((this.TailNode as Node).RenderTransform is RotateTransform)
                                {
                                    e = this.GeneralPointRotation(target.Position, e, ((this.TailNode as Node).RenderTransform as RotateTransform).Angle);
                                }

                            }
                        }
                    }

                    if (this.HeadNode == null)
                    {
                        s = new Point(PxStartPointPosition.X, PxStartPointPosition.Y);
                    }

                    if (this.TailNode == null)
                    {
                        e = new Point(PxEndPointPosition.X, PxEndPointPosition.Y);
                    }

                    if (!targetRect.Contains(sp) && !sourceRect.Contains(ep))
                    {
                        x1 = s.X;
                        y1 = s.Y;
                        x2 = e.X;
                        y2 = e.Y;
                        PathGeometry pathgeometry = new PathGeometry();
                        PathFigure pathfigure = new PathFigure();
                        double num = 150.0;
                        pathfigure.StartPoint = new Point(x1, y1);
                        BezierSegment segment = new BezierSegment();
                        if (this.HeadDecoratorGrid != null)
                        {
                            if (isBottom)
                            {
                                (this as LineConnector).HeadDecoratorAngle = -90;
                                segment = this.GetSegment(x1, y1 + num, x2, y2, x2, y2, num, tisTop, tisBottom, tisLeft, tisRight);
                            }
                            else if (isTop)
                            {
                                (this as LineConnector).HeadDecoratorAngle = 90;
                                segment = this.GetSegment(x1, y1 - num, x2, y2, x2, y2, num, tisTop, tisBottom, tisLeft, tisRight);
                            }
                            else if (isRight)
                            {
                                (this as LineConnector).HeadDecoratorAngle = -180;
                                segment = this.GetSegment(x1 + num, y1, x2, y2, x2, y2, num, tisTop, tisBottom, tisLeft, tisRight);
                            }
                            else
                            {
                                (this as LineConnector).HeadDecoratorAngle = 0;
                                segment = this.GetSegment(x1 - num, y1, x2, y2, x2, y2, num, tisTop, tisBottom, tisLeft, tisRight);
                            }
                        }

                        if (this.TailDecoratorGrid != null)
                        {
                            if (tisBottom)
                            {
                                (this as LineConnector).TailDecoratorAngle = -90;
                            }
                            else if (tisTop)
                            {
                                (this as LineConnector).TailDecoratorAngle = 90;
                            }
                            else if (tisRight)
                            {
                                (this as LineConnector).TailDecoratorAngle = 0;
                            }
                            else
                            {
                                (this as LineConnector).TailDecoratorAngle = 0;
                            }
                        }

                        double x = Math.Pow((x1 - x2), 2);
                        double y = Math.Pow((y1 - y2), 2);
                        Distance = Math.Sqrt(x + y);
                        pathfigure.Segments.Add(segment);
                        pathgeometry.Figures.Add(pathfigure);
                        if (this.HeadNode != null && this.TailNode != null)
                        {
                            this.misoverlapped = false;
                            this.TailDecoratorShape = this.InternalTailShape;
                            this.HeadDecoratorShape = this.InternalHeadShape;
                        }

                        ConnectionPoints = new List<Point>();
                        ConnectionPoints.Add(s);
                        ConnectionPoints.Add(e);
                        this.PxStartPointPosition = s;
                        this.PxEndPointPosition = e;
                        this.LabelPosition = new Point((s.X + e.X) / 2, (s.Y + e.Y) / 2);
                        this.LabelTemplatePosition = new Point((s.X + e.X) / 2, (s.Y + e.Y) / 2);
                        this.ConnectorPathGeometry = pathgeometry;
                    }
                    else
                    {
                        if (this.HeadNode != null && this.TailNode != null)
                        {
                            this.misoverlapped = true;
                            this.ConnectorPathGeometry = null;
                            this.HeadDecoratorShape = DecoratorShape.None;
                            this.TailDecoratorShape = DecoratorShape.None;
                            this.LabelPosition = new Point((HeadNode as Node).PxPosition.X + (TailNode as Node).PxPosition.X, (HeadNode as Node).PxPosition.Y + (TailNode as Node).PxPosition.Y);
                            this.LineAngle = FindAngle((HeadNode as Node).Position, (TailNode as Node).Position);
                            this.LabelAngle = 0;
                            if (this.dc != null)
                            {
                                this.dc.View.SelectionList.Remove(this);
                            }
                        }
                    }
                }
                catch
                {
                }

                if (this.LabelHorizontalAlignment == HorizontalAlignment.Left)
                {
                    this.LabelPosition = PxStartPointPosition;

                    if (PxStartPointPosition.X > PxEndPointPosition.X)
                    {
                        this.LabelPosition = new Point(PxStartPointPosition.X - this.linelabel.ActualWidth, this.LabelPosition.Y);
                    }
                    if (PxStartPointPosition.Y > PxEndPointPosition.Y)
                    {
                        this.LabelPosition = new Point(LabelPosition.X, this.LabelPosition.Y - this.linelabel.ActualHeight);
                    }
                }
                else if (this.LabelHorizontalAlignment == HorizontalAlignment.Right)
                {
                    this.LabelPosition = PxEndPointPosition;

                    if (PxStartPointPosition.X < PxEndPointPosition.X)
                    {
                        this.LabelPosition = new Point(PxEndPointPosition.X - this.linelabel.ActualWidth, this.LabelPosition.Y);
                    }
                    if (PxStartPointPosition.Y < PxEndPointPosition.Y)
                    {
                        this.LabelPosition = new Point(LabelPosition.X, this.LabelPosition.Y - this.linelabel.ActualHeight);
                    }
                }
                else
                {
                    this.LabelAngle = 0;
                    this.LabelPosition = new Point((PxStartPointPosition.X + PxEndPointPosition.X) / 2, (PxStartPointPosition.Y + PxEndPointPosition.Y) / 2);

                }
                if (this.LabelTemplateHorizontalAlignment == HorizontalAlignment.Left)
                {
                    this.LabelTemplatePosition = PxStartPointPosition;

                    if (PxStartPointPosition.X > PxEndPointPosition.X)
                    {
                        this.LabelTemplatePosition = new Point(PxStartPointPosition.X - this.labelGrid.ActualWidth, this.LabelPosition.Y);
                    }
                    if (PxStartPointPosition.Y > PxEndPointPosition.Y)
                    {
                        this.LabelTemplatePosition = new Point(LabelTemplatePosition.X, this.LabelPosition.Y - this.labelGrid.ActualHeight);
                    }
                }
                else if (this.LabelTemplateHorizontalAlignment == HorizontalAlignment.Right)
                {
                    this.LabelTemplatePosition = PxEndPointPosition;

                    if (PxStartPointPosition.X < PxEndPointPosition.X)
                    {
                        this.LabelTemplatePosition = new Point(PxEndPointPosition.X - this.labelGrid.ActualWidth, this.LabelTemplatePosition.Y);
                    }
                    if (PxStartPointPosition.Y < PxEndPointPosition.Y)
                    {
                        this.LabelTemplatePosition = new Point(LabelTemplatePosition.X, this.LabelTemplatePosition.Y - this.labelGrid.ActualHeight);
                    }
                }

            }
            else
            {
                try
                {
                    if (this.HeadNode != null || this.TailNode != null)
                    {
                        Point zero = new Point(0, 0);
                        ConnectionPoints = new List<Point>();
                        sp = startPoint;// MeasureUnitsConverter.FromPixels(startPoint, source.MeasurementUnit);
                        ep = endPoint;// MeasureUnitsConverter.FromPixels(endPoint, source.MeasurementUnit);
                        if (!targetRect.Contains(sp) && !sourceRect.Contains(ep))
                        {
                            PathGeometry pathgeometry = new PathGeometry();
                            if (this.HeadNode == null && this.TailNode != null)
                            {
                                ConnectionPoints = this.GetLinePoints((this.TailNode as Node).GetInfo(), true);
                            }

                            if (this.TailNode == null && this.HeadNode != null)
                            {
                                ConnectionPoints = this.GetLinePoints((this.HeadNode as Node).GetInfo());
                            }

                            if (this.HeadNode != null && this.TailNode != null)
                            {
                                ConnectionPoints = this.GetLinePoints((this.HeadNode as Node).GetInfo(), (this.TailNode as Node).GetInfo());
                            }
                            Point EndPoint = new Point(0, 0);
                            Point StartPoint = new Point(0, 0);
                            if (ConnectionPoints.Count > 0)
                            {
                                this.misoverlapped = false;
                                PathFigure pathfigure = new PathFigure();
                                pathfigure.StartPoint = ConnectionPoints[0];
                                StartPoint = pathfigure.StartPoint;
                                PolyLineSegment polyline = new PolyLineSegment();
                                foreach (Point p in ConnectionPoints)
                                {
                                    polyline.Points.Add(p);
                                    EndPoint = p;
                                }
                                if (ConnectionPoints.Count > 2)
                                {
                                    EndPoint = ConnectionPoints[2];
                                }
                                pathfigure.Segments.Add(polyline);
                                pathgeometry.Figures.Add(pathfigure);
                                if (!this.linedragging && this.dview != null && !this.dview.IsDragging)
                                {
                                    this.PxStartPointPosition = ConnectionPoints[0];
                                    this.PxEndPointPosition = ConnectionPoints.ElementAt(ConnectionPoints.Count() - 1);
                                }

                                if (ConnectionPoints.Count == 2)
                                {
                                    this.LineAngle = FindAngle(StartPoint, EndPoint);
                                }
                                this.ConnectorPathGeometry = pathgeometry;
                            }
                            else
                            {
                                this.ConnectorPathGeometry = new PathGeometry();
                            }
                            if (this.LabelHorizontalAlignment == HorizontalAlignment.Left)
                            {
                                this.LabelPosition = StartPoint;
                                if (this.ConnectorType == ConnectorType.Orthogonal)
                                {
                                    if (this.ConnectionPoints[1].X > this.ConnectionPoints[2].X)
                                    {
                                        this.LabelPosition = new Point(StartPoint.X - this.linelabel.ActualWidth, this.LabelPosition.Y);
                                    }
                                    if (this.ConnectionPoints[0].Y > this.ConnectionPoints[1].Y)
                                    {
                                        this.LabelPosition = new Point(LabelPosition.X, this.LabelPosition.Y - this.linelabel.ActualHeight);
                                    }

                                }
                                if (this.ConnectorType == ConnectorType.Straight)
                                {
                                    if (this.LabelOrientation == LabelOrientation.Auto)
                                    {
                                        double angle = (this.LineAngle * Math.PI) / 180;
                                        if (LineAngle > 90 && LineAngle < 270)
                                        {
                                            LabelAngle = 180;
                                            this.LabelPosition = new Point(StartPoint.X + (Math.Cos(angle)) * (this.linelabel.DesiredSize.Width), StartPoint.Y + (Math.Sin(angle) * (this.linelabel.DesiredSize.Width)));
                                        }
                                        else
                                        {
                                            LabelAngle = 0;
                                            this.LabelPosition = new Point(StartPoint.X + (Math.Cos(angle)), StartPoint.Y + (Math.Sin(angle)));
                                        }
                                    }
                                    else if (this.LabelOrientation == LabelOrientation.Horizontal)
                                    {
                                        if (this.LineAngle > 0 && this.LineAngle <= 90)
                                        {
                                            this.LabelPosition = StartPoint;
                                        }
                                        else if (LineAngle > 90 && LineAngle <= 180)
                                        {
                                            this.LabelPosition = new Point(StartPoint.X - this.linelabel.DesiredSize.Width, StartPoint.Y);
                                        }
                                        else if (LineAngle > 180 && LineAngle <= 270)
                                        {
                                            this.LabelPosition = new Point(StartPoint.X - this.linelabel.DesiredSize.Width, StartPoint.Y - this.linelabel.DesiredSize.Height);

                                        }
                                        else if (LineAngle > 270 && LineAngle <= 360)
                                        {
                                            this.LabelPosition = new Point(StartPoint.X, StartPoint.Y - this.linelabel.DesiredSize.Height);
                                        }
                                    }
                                    else if (this.LabelOrientation == LabelOrientation.Vertical && this.ConnectorType == ConnectorType.Straight)
                                    {
                                        if (this.LineAngle > 0 && this.LineAngle <= 90)
                                        {
                                            this.LabelPosition = new Point(StartPoint.X, StartPoint.Y + this.linelabel.DesiredSize.Width);
                                        }
                                        else if (LineAngle > 90 && LineAngle <= 180)
                                        {
                                            this.LabelPosition = new Point(StartPoint.X - this.linelabel.DesiredSize.Height, StartPoint.Y + this.linelabel.DesiredSize.Width);
                                        }
                                        else if (LineAngle > 180 && LineAngle <= 270)
                                        {
                                            this.LabelPosition = new Point(StartPoint.X - this.linelabel.DesiredSize.Height, StartPoint.Y);

                                        }
                                        else if (LineAngle > 270 && LineAngle <= 360)
                                        {
                                            this.LabelPosition = StartPoint;
                                        }

                                    }
                                }
                            }
                            else if (this.LabelHorizontalAlignment == HorizontalAlignment.Right)
                            {
                                this.LabelPosition = EndPoint;
                                if (this.ConnectorType == ConnectorType.Orthogonal)
                                {
                                    EndPoint = this.ConnectionPoints[4];
                                    this.LabelPosition = this.ConnectionPoints[4];

                                    if (this.ConnectionPoints[4].Y > this.ConnectionPoints[3].Y)
                                    {
                                        this.LabelPosition = new Point(LabelPosition.X, this.LabelPosition.Y - this.linelabel.ActualHeight);
                                    }
                                    if (this.ConnectionPoints[1].X > this.ConnectionPoints[2].X)
                                    {
                                        this.LabelPosition = new Point(EndPoint.X - this.linelabel.ActualWidth, this.LabelPosition.Y);
                                    }
                                    if (this.ConnectionPoints[4].X > this.ConnectionPoints[3].X)
                                    {
                                        this.LabelPosition = new Point(LabelPosition.X - this.linelabel.ActualWidth, this.LabelPosition.Y);
                                    }
                                    else
                                    {
                                        if ((this.ConnectionPoints[3].Y == this.ConnectionPoints[4].Y) && (this.ConnectionPoints[4].X < this.ConnectionPoints[3].X))
                                            this.LabelPosition = new Point(LabelPosition.X + this.linelabel.ActualWidth, this.LabelPosition.Y);
                                    }
                                }
                                else if (this.ConnectorType == ConnectorType.Straight)
                                {
                                    if (this.LabelOrientation == LabelOrientation.Auto)
                                    {
                                        double angle = (this.LineAngle * Math.PI) / 180;
                                        if (LineAngle > 90 && LineAngle < 270)
                                        {
                                            LabelAngle = 180;
                                            this.LabelPosition = new Point(EndPoint.X - (Math.Cos(angle)), EndPoint.Y - (Math.Sin(angle)));
                                        }
                                        else
                                        {
                                            LabelAngle = 0;
                                            this.LabelPosition = new Point(EndPoint.X - (Math.Cos(angle) * (this.linelabel.DesiredSize.Width)), EndPoint.Y - (Math.Sin(angle) * (this.linelabel.DesiredSize.Width)));
                                        }
                                    }
                                    else if (this.LabelOrientation == LabelOrientation.Horizontal)
                                    {
                                        if (this.LineAngle > 0 && this.LineAngle <= 90)
                                        {
                                            this.LabelPosition = new Point(EndPoint.X - this.linelabel.DesiredSize.Width - 10, EndPoint.Y - this.linelabel.DesiredSize.Height);
                                        }
                                        else if (LineAngle > 90 && LineAngle <= 180)
                                        {
                                            this.LabelPosition = new Point(EndPoint.X + 5, EndPoint.Y - this.linelabel.DesiredSize.Height);
                                        }
                                        else if (LineAngle > 180 && LineAngle <= 270)
                                        {
                                            this.LabelPosition = new Point(EndPoint.X + 5, EndPoint.Y);

                                        }
                                        else if (LineAngle > 270 && LineAngle <= 360)
                                        {
                                            this.LabelPosition = new Point(EndPoint.X - this.linelabel.DesiredSize.Width, EndPoint.Y + 5);
                                        }

                                    }
                                    else if (this.LabelOrientation == LabelOrientation.Vertical)
                                    {
                                        if (this.LineAngle > 0 && this.LineAngle <= 90)
                                        {
                                            this.LabelPosition = new Point(EndPoint.X - this.linelabel.DesiredSize.Height, EndPoint.Y - 5);
                                        }
                                        else if (LineAngle > 90 && LineAngle <= 180)
                                        {
                                            this.LabelPosition = new Point(EndPoint.X, EndPoint.Y - 5);
                                        }
                                        else if (LineAngle > 180 && LineAngle <= 270)
                                        {
                                            this.LabelPosition = new Point(EndPoint.X + 5, EndPoint.Y + this.linelabel.DesiredSize.Width);

                                        }
                                        else if (LineAngle > 270 && LineAngle <= 360)
                                        {
                                            this.LabelPosition = new Point(EndPoint.X - this.linelabel.DesiredSize.Height - 5, EndPoint.Y + this.linelabel.DesiredSize.Width);
                                        }
                                    }

                                }
                                else
                                {
                                    double angle = (this.LineAngle * Math.PI) / 180;
                                    this.LabelPosition = new Point(EndPoint.X - (Math.Cos(angle) * (this.linelabel.DesiredSize.Width)), EndPoint.Y - (Math.Sin(angle) * (this.linelabel.DesiredSize.Width)));
                                }
                            }
                            else
                            {
                                double angle = (this.LineAngle * Math.PI) / 180;
                                if (this.ConnectorType != ConnectorType.Orthogonal)
                                {
                                    if (this.ConnectorType == ConnectorType.Bezier)
                                    {
                                        this.LabelAngle = 0;
                                        this.LabelPosition = new Point((StartPoint.X + EndPoint.X) / 2 - (Math.Cos(angle) * (this.linelabel.DesiredSize.Width + 10)), (StartPoint.Y + EndPoint.Y) / 2 - (Math.Sin(angle) * (this.linelabel.DesiredSize.Width + 10)));
                                    }
                                    else
                                    {
                                        if (LineAngle > 90 && LineAngle < 270)
                                        {
                                            LabelAngle = 180;
                                            this.LabelPosition = new Point((StartPoint.X + EndPoint.X) / 2 + (Math.Cos(angle) * (this.linelabel.DesiredSize.Width / 2)), (StartPoint.Y + EndPoint.Y) / 2 + (Math.Sin(angle) * (this.linelabel.DesiredSize.Width / 2)));
                                        }
                                        else
                                        {
                                            LabelAngle = 0;
                                            this.LabelPosition = new Point((StartPoint.X + EndPoint.X) / 2 - (Math.Cos(angle) * (this.linelabel.DesiredSize.Width / 2)), (StartPoint.Y + EndPoint.Y) / 2 - (Math.Sin(angle) * (this.linelabel.DesiredSize.Width / 2)));
                                        }
                                    }
                                }
                                else
                                {
                                    this.LabelAngle = 0;
                                    this.LabelPosition = new Point((StartPoint.X + EndPoint.X) / 2, (StartPoint.Y + EndPoint.Y) / 2 + ((Math.Abs(StartPoint.X - EndPoint.X) < this.linelabel.ActualWidth) ? (this.ConnectionPoints[3].Y - this.ConnectionPoints[0].Y) / 2 : 0));
                                }
                            }

                            if (this.LabelTemplateHorizontalAlignment == HorizontalAlignment.Left)
                            {
                                this.LabelTemplatePosition = StartPoint;
                                if (this.ConnectorType == ConnectorType.Orthogonal)
                                {
                                    if (this.ConnectionPoints[1].X > this.ConnectionPoints[2].X)
                                    {
                                        this.LabelTemplatePosition = new Point(StartPoint.X - this.labelGrid.ActualWidth, this.LabelTemplatePosition.Y);
                                    }
                                    if (this.ConnectionPoints[0].Y > this.ConnectionPoints[1].Y)
                                    {
                                        this.LabelTemplatePosition = new Point(LabelTemplatePosition.X, this.LabelTemplatePosition.Y - this.labelGrid.ActualHeight);
                                    }

                                }
                                if (this.ConnectorType == ConnectorType.Straight)
                                {
                                    if (this.LabelTemplateOrientation == LabelOrientation.Auto)
                                    {
                                        double angle = (this.LineAngle * Math.PI) / 180;
                                        if (LineAngle > 90 && LineAngle < 270)
                                        {
                                            LabelAngle = 180;
                                            this.LabelTemplatePosition = new Point(StartPoint.X + (Math.Cos(angle)) * (this.labelGrid.DesiredSize.Width), StartPoint.Y + (Math.Sin(angle) * (this.labelGrid.DesiredSize.Width)));
                                        }
                                        else
                                        {
                                            LabelAngle = 0;
                                            this.LabelTemplatePosition = new Point(StartPoint.X + (Math.Cos(angle)), StartPoint.Y + (Math.Sin(angle)));
                                        }
                                    }
                                    else if (this.LabelTemplateOrientation == LabelOrientation.Horizontal)
                                    {
                                        if (this.LineAngle > 0 && this.LineAngle <= 90)
                                        {
                                            this.LabelTemplatePosition = StartPoint;
                                        }
                                        else if (LineAngle > 90 && LineAngle <= 180)
                                        {
                                            this.LabelTemplatePosition = new Point(StartPoint.X - this.labelGrid.DesiredSize.Width, StartPoint.Y);
                                        }
                                        else if (LineAngle > 180 && LineAngle <= 270)
                                        {
                                            this.LabelTemplatePosition = new Point(StartPoint.X - this.labelGrid.DesiredSize.Width, StartPoint.Y - this.labelGrid.DesiredSize.Height);

                                        }
                                        else if (LineAngle > 270 && LineAngle <= 360)
                                        {
                                            this.LabelTemplatePosition = new Point(StartPoint.X, StartPoint.Y - this.labelGrid.DesiredSize.Height);
                                        }
                                    }
                                    else if (this.LabelTemplateOrientation == LabelOrientation.Vertical && this.ConnectorType == ConnectorType.Straight)
                                    {
                                        if (this.LineAngle > 0 && this.LineAngle <= 90)
                                        {
                                            this.LabelTemplatePosition = new Point(StartPoint.X, StartPoint.Y + this.labelGrid.DesiredSize.Width);
                                        }
                                        else if (LineAngle > 90 && LineAngle <= 180)
                                        {
                                            this.LabelTemplatePosition = new Point(StartPoint.X - this.labelGrid.DesiredSize.Height, StartPoint.Y + this.labelGrid.DesiredSize.Width);
                                        }
                                        else if (LineAngle > 180 && LineAngle <= 270)
                                        {
                                            this.LabelTemplatePosition = new Point(StartPoint.X - this.labelGrid.DesiredSize.Height, StartPoint.Y);

                                        }
                                        else if (LineAngle > 270 && LineAngle <= 360)
                                        {
                                            this.LabelTemplatePosition = StartPoint;
                                        }

                                    }

                                }
                            }
                            else if (this.LabelTemplateHorizontalAlignment == HorizontalAlignment.Right)
                            {
                                if (this.ConnectorType == ConnectorType.Orthogonal)
                                {
                                    EndPoint = this.ConnectionPoints[4];
                                    this.LabelTemplatePosition = this.ConnectionPoints[4];

                                    if (this.ConnectionPoints[4].Y > this.ConnectionPoints[3].Y)
                                    {
                                        this.LabelTemplatePosition = new Point(LabelTemplatePosition.X, this.LabelTemplatePosition.Y - this.labelGrid.ActualHeight);
                                    }
                                    if (this.ConnectionPoints[1].X > this.ConnectionPoints[2].X)
                                    {
                                        this.LabelTemplatePosition = new Point(EndPoint.X - this.labelGrid.ActualWidth, this.LabelTemplatePosition.Y);
                                    }
                                    if (this.ConnectionPoints[4].X > this.ConnectionPoints[3].X)
                                    {
                                        this.LabelTemplatePosition = new Point(LabelTemplatePosition.X - this.labelGrid.ActualWidth, this.LabelTemplatePosition.Y);
                                    }
                                    else
                                    {
                                        if ((this.ConnectionPoints[3].Y == this.ConnectionPoints[4].Y) && (this.ConnectionPoints[4].X < this.ConnectionPoints[3].X))
                                            this.LabelTemplatePosition = new Point(LabelTemplatePosition.X + this.labelGrid.ActualWidth, this.LabelTemplatePosition.Y);
                                    }
                                }
                                else if (this.ConnectorType == ConnectorType.Straight)
                                {
                                    if (this.LabelTemplateOrientation == LabelOrientation.Auto)
                                    {
                                        double angle = (this.LineAngle * Math.PI) / 180;
                                        if (LineAngle > 90 && LineAngle < 270)
                                        {
                                            LabelAngle = 180;
                                            this.LabelTemplatePosition = new Point(EndPoint.X - (Math.Cos(angle)), EndPoint.Y - (Math.Sin(angle)));
                                        }
                                        else
                                        {
                                            LabelAngle = 0;
                                            this.LabelTemplatePosition = new Point(EndPoint.X - (Math.Cos(angle) * (this.labelGrid.DesiredSize.Width)), EndPoint.Y - (Math.Sin(angle) * (this.labelGrid.DesiredSize.Width)));
                                        }
                                    }
                                    else if (this.LabelTemplateOrientation == LabelOrientation.Horizontal)
                                    {
                                        if (this.LineAngle > 0 && this.LineAngle <= 90)
                                        {
                                            this.LabelTemplatePosition = new Point(EndPoint.X - this.labelGrid.DesiredSize.Width - 10, EndPoint.Y - this.labelGrid.DesiredSize.Height);
                                        }
                                        else if (LineAngle > 90 && LineAngle <= 180)
                                        {
                                            this.LabelTemplatePosition = new Point(EndPoint.X + 5, EndPoint.Y - this.labelGrid.DesiredSize.Height);
                                        }
                                        else if (LineAngle > 180 && LineAngle <= 270)
                                        {
                                            this.LabelTemplatePosition = new Point(EndPoint.X + 5, EndPoint.Y);

                                        }
                                        else if (LineAngle > 270 && LineAngle <= 360)
                                        {
                                            this.LabelTemplatePosition = new Point(EndPoint.X - this.labelGrid.DesiredSize.Width, EndPoint.Y + 5);
                                        }

                                    }
                                    else if (this.LabelTemplateOrientation == LabelOrientation.Vertical)
                                    {
                                        if (this.LineAngle > 0 && this.LineAngle <= 90)
                                        {
                                            this.LabelTemplatePosition = new Point(EndPoint.X - this.labelGrid.DesiredSize.Height, EndPoint.Y - 5);
                                        }
                                        else if (LineAngle > 90 && LineAngle <= 180)
                                        {
                                            this.LabelTemplatePosition = new Point(EndPoint.X, EndPoint.Y - 5);
                                        }
                                        else if (LineAngle > 180 && LineAngle <= 270)
                                        {
                                            this.LabelTemplatePosition = new Point(EndPoint.X + 5, EndPoint.Y + this.labelGrid.DesiredSize.Width);

                                        }
                                        else if (LineAngle > 270 && LineAngle <= 360)
                                        {
                                            this.LabelTemplatePosition = new Point(EndPoint.X - this.labelGrid.DesiredSize.Height - 5, EndPoint.Y + this.labelGrid.DesiredSize.Width);
                                        }
                                    }

                                }

                                else
                                {
                                    double angle = (this.LineAngle * Math.PI) / 180;
                                    this.LabelTemplatePosition = new Point(EndPoint.X - (Math.Cos(angle) * (this.labelGrid.ActualWidth + 10)), EndPoint.Y - (Math.Sin(angle) * (this.labelGrid.ActualWidth + 10)));
                                }
                            }
                            else
                            {
                                double angle = (this.LineAngle * Math.PI) / 180;
                                if (this.ConnectorType != ConnectorType.Orthogonal)
                                    this.LabelTemplatePosition = new Point((StartPoint.X + EndPoint.X) / 2 - (Math.Cos(angle) * (this.labelGrid.ActualHeight + 10)), (StartPoint.Y + EndPoint.Y) / 2 - (Math.Sin(angle) * (this.labelGrid.ActualHeight + 10)));
                                else
                                {
                                    this.LabelTemplatePosition = new Point((StartPoint.X + EndPoint.X) / 2, (StartPoint.Y + EndPoint.Y) / 2 + ((Math.Abs(StartPoint.X - EndPoint.X) < this.labelGrid.ActualWidth) ? (this.ConnectionPoints[3].Y - this.ConnectionPoints[0].Y) / 2 : 0));
                                }
                            }
                        }
                        else
                        {
                            if (HeadNode != null && TailNode != null)
                            {
                                this.misoverlapped = true;
                                this.ConnectorPathGeometry = null;
                                this.LabelPosition = new Point((HeadNode as Node).PxPosition.X + (TailNode as Node).PxPosition.X, (HeadNode as Node).PxPosition.Y + (TailNode as Node).PxPosition.Y);
                                this.LineAngle = FindAngle((HeadNode as Node).PxPosition, (TailNode as Node).PxPosition);
                                this.LabelAngle = this.LineAngle;
                                if (this.dc != null)
                                {
                                    this.dc.View.SelectionList.Remove(this);
                                }
                            }
                        }

                    }
                    else
                    {
                        PathGeometry pathgeometry = new PathGeometry();
                        ConnectionPoints = this.GetLinePoints();
                        Point EndPoint = new Point(0, 0);
                        Point StartPoint = new Point(0, 0);
                        if (ConnectionPoints.Count > 0)
                        {
                            PathFigure pathfigure = new PathFigure();
                            pathfigure.StartPoint = ConnectionPoints[0];
                            StartPoint = pathfigure.StartPoint;
                            PolyLineSegment polyline = new PolyLineSegment();
                            foreach (Point p in ConnectionPoints)
                            {
                                polyline.Points.Add(p);
                                EndPoint = p;
                            }
                            if (ConnectionPoints.Count > 2)
                            {
                                EndPoint = ConnectionPoints[2];
                            }
                            if (ConnectionPoints.Count == 2)
                            {
                                this.LineAngle = FindAngle(StartPoint, EndPoint);
                            }
                            pathfigure.Segments.Add(polyline);
                            pathgeometry.Figures.Add(pathfigure);
                            this.ConnectorPathGeometry = pathgeometry;
                        }
                        if (this.LabelHorizontalAlignment == HorizontalAlignment.Left)
                        {
                            if (this.LabelOrientation == LabelOrientation.Horizontal && this.ConnectorType == ConnectorType.Straight)
                            {
                                if (this.LineAngle > 0 && this.LineAngle <= 90)
                                {
                                    this.LabelPosition = StartPoint;
                                }
                                else if (LineAngle > 90 && LineAngle <= 180)
                                {
                                    this.LabelPosition = new Point(StartPoint.X - this.linelabel.DesiredSize.Width, StartPoint.Y);
                                }
                                else if (LineAngle > 180 && LineAngle <= 270)
                                {
                                    this.LabelPosition = new Point(StartPoint.X - this.linelabel.DesiredSize.Width, StartPoint.Y - this.linelabel.DesiredSize.Height);

                                }
                                else if (LineAngle > 270 && LineAngle <= 360)
                                {
                                    this.LabelPosition = new Point(StartPoint.X, StartPoint.Y - this.linelabel.DesiredSize.Height);
                                }
                            }
                            else if (this.LabelOrientation == LabelOrientation.Vertical && this.ConnectorType == ConnectorType.Straight)
                            {
                                if (this.LineAngle > 0 && this.LineAngle <= 90)
                                {
                                    this.LabelPosition = new Point(StartPoint.X, StartPoint.Y + this.linelabel.DesiredSize.Width);
                                }
                                else if (LineAngle > 90 && LineAngle <= 180)
                                {
                                    this.LabelPosition = new Point(StartPoint.X - this.linelabel.DesiredSize.Height, StartPoint.Y + this.linelabel.DesiredSize.Width);
                                }
                                else if (LineAngle > 180 && LineAngle <= 270)
                                {
                                    this.LabelPosition = new Point(StartPoint.X - this.linelabel.DesiredSize.Height, StartPoint.Y);

                                }
                                else if (LineAngle > 270 && LineAngle <= 360)
                                {
                                    this.LabelPosition = StartPoint;
                                }

                            }
                            else
                            {
                                this.LabelPosition = StartPoint;
                            }


                        }
                        else if (this.LabelHorizontalAlignment == HorizontalAlignment.Right)
                        {
                            if (this.LabelOrientation == LabelOrientation.Horizontal && this.ConnectorType == ConnectorType.Straight)
                            {
                                if (this.LineAngle > 0 && this.LineAngle <= 90)
                                {
                                    this.LabelPosition = new Point(EndPoint.X - this.linelabel.DesiredSize.Width - 10, EndPoint.Y - this.linelabel.DesiredSize.Height);
                                }
                                else if (LineAngle > 90 && LineAngle <= 180)
                                {
                                    this.LabelPosition = new Point(EndPoint.X + 5, EndPoint.Y - this.linelabel.DesiredSize.Height);
                                }
                                else if (LineAngle > 180 && LineAngle <= 270)
                                {
                                    this.LabelPosition = new Point(EndPoint.X + 5, EndPoint.Y);

                                }
                                else if (LineAngle > 270 && LineAngle <= 360)
                                {
                                    this.LabelPosition = new Point(EndPoint.X - this.linelabel.DesiredSize.Width, EndPoint.Y + 5);
                                }

                            }
                            else if (this.LabelOrientation == LabelOrientation.Vertical && this.ConnectorType == ConnectorType.Straight)
                            {
                                if (this.LineAngle > 0 && this.LineAngle <= 90)
                                {
                                    this.LabelPosition = new Point(EndPoint.X - this.linelabel.DesiredSize.Height, EndPoint.Y - 5);
                                }
                                else if (LineAngle > 90 && LineAngle <= 180)
                                {
                                    this.LabelPosition = new Point(EndPoint.X, EndPoint.Y - 5);
                                }
                                else if (LineAngle > 180 && LineAngle <= 270)
                                {
                                    this.LabelPosition = new Point(EndPoint.X + 5, EndPoint.Y + this.linelabel.DesiredSize.Width);

                                }
                                else if (LineAngle > 270 && LineAngle <= 360)
                                {
                                    this.LabelPosition = new Point(EndPoint.X - this.linelabel.DesiredSize.Height - 5, EndPoint.Y + this.linelabel.DesiredSize.Width);
                                }
                            }

                            else
                            {
                                double angle = (this.LineAngle * Math.PI) / 180;
                                this.LabelPosition = new Point(EndPoint.X - (Math.Cos(angle) * (this.linelabel.ActualWidth + 10)), EndPoint.Y - (Math.Sin(angle) * (this.linelabel.ActualWidth + 10)));
                            }
                        }
                        else
                        {
                            if (LineAngle > 90 && LineAngle < 270)
                            {
                                double angle = (this.LineAngle * Math.PI) / 180;
                                LabelAngle = 180;
                                this.LabelPosition = new Point((StartPoint.X + EndPoint.X) / 2 + (Math.Cos(angle) * (this.linelabel.DesiredSize.Width / 2)), (StartPoint.Y + EndPoint.Y) / 2 + (Math.Sin(angle) * (this.linelabel.DesiredSize.Width / 2)));
                            }
                            else
                            {
                                double angle = (this.LineAngle * Math.PI) / 180;
                                LabelAngle = 0;
                                this.LabelPosition = new Point((StartPoint.X + EndPoint.X) / 2 - (Math.Cos(angle) * (this.linelabel.DesiredSize.Width / 2)), (StartPoint.Y + EndPoint.Y) / 2 - (Math.Sin(angle) * (this.linelabel.DesiredSize.Width / 2)));
                            }

                        }

                        if (this.LabelTemplateHorizontalAlignment == HorizontalAlignment.Left)
                        {
                            if (this.LabelTemplateOrientation == LabelOrientation.Horizontal && this.ConnectorType == ConnectorType.Straight)
                            {
                                if (this.LineAngle > 0 && this.LineAngle <= 90)
                                {
                                    this.LabelTemplatePosition = StartPoint;
                                }
                                else if (LineAngle > 90 && LineAngle <= 180)
                                {
                                    this.LabelTemplatePosition = new Point(StartPoint.X - this.labelGrid.ActualWidth, StartPoint.Y);
                                }
                                else if (LineAngle > 180 && LineAngle <= 270)
                                {
                                    this.LabelTemplatePosition = new Point(StartPoint.X - this.labelGrid.ActualWidth, StartPoint.Y - this.labelGrid.ActualHeight);

                                }
                                else if (LineAngle > 270 && LineAngle <= 360)
                                {
                                    this.LabelTemplatePosition = new Point(StartPoint.X, StartPoint.Y - this.labelGrid.ActualHeight);
                                }
                            }
                            else if (this.LabelTemplateOrientation == LabelOrientation.Vertical && this.ConnectorType == ConnectorType.Straight)
                            {
                                if (this.LineAngle > 0 && this.LineAngle <= 90)
                                {
                                    this.LabelTemplatePosition = new Point(StartPoint.X, StartPoint.Y + this.labelGrid.ActualWidth);
                                }
                                else if (LineAngle > 90 && LineAngle <= 180)
                                {
                                    this.LabelTemplatePosition = new Point(StartPoint.X - this.labelGrid.ActualHeight, StartPoint.Y + this.labelGrid.ActualWidth);
                                }
                                else if (LineAngle > 180 && LineAngle <= 270)
                                {
                                    this.LabelTemplatePosition = new Point(StartPoint.X - this.labelGrid.ActualHeight, StartPoint.Y);

                                }
                                else if (LineAngle > 270 && LineAngle <= 360)
                                {
                                    this.LabelTemplatePosition = StartPoint;
                                }

                            }
                            else
                            {
                                this.LabelTemplatePosition = StartPoint;
                            }
                        }
                        else if (this.LabelTemplateHorizontalAlignment == HorizontalAlignment.Right)
                        {
                            if (this.LabelTemplateOrientation == LabelOrientation.Horizontal && this.ConnectorType == ConnectorType.Straight)
                            {
                                if (this.LineAngle > 0 && this.LineAngle <= 90)
                                {
                                    this.LabelTemplatePosition = new Point(EndPoint.X - this.labelGrid.ActualWidth - 10, EndPoint.Y - this.labelGrid.ActualHeight);
                                }
                                else if (LineAngle > 90 && LineAngle <= 180)
                                {
                                    this.LabelTemplatePosition = new Point(EndPoint.X + 5, EndPoint.Y - this.labelGrid.ActualHeight);
                                }
                                else if (LineAngle > 180 && LineAngle <= 270)
                                {
                                    this.LabelTemplatePosition = new Point(EndPoint.X + 5, EndPoint.Y);

                                }
                                else if (LineAngle > 270 && LineAngle <= 360)
                                {
                                    this.LabelTemplatePosition = new Point(EndPoint.X - this.labelGrid.ActualWidth, EndPoint.Y + 5);
                                }

                            }
                            else if (this.LabelTemplateOrientation == LabelOrientation.Vertical && this.ConnectorType == ConnectorType.Straight)
                            {
                                if (this.LineAngle > 0 && this.LineAngle <= 90)
                                {
                                    this.LabelTemplatePosition = new Point(EndPoint.X - this.labelGrid.ActualHeight, EndPoint.Y - 5);
                                }
                                else if (LineAngle > 90 && LineAngle <= 180)
                                {
                                    this.LabelTemplatePosition = new Point(EndPoint.X, EndPoint.Y - 5);
                                }
                                else if (LineAngle > 180 && LineAngle <= 270)
                                {
                                    this.LabelTemplatePosition = new Point(EndPoint.X + 5, EndPoint.Y + this.labelGrid.ActualWidth);

                                }
                                else if (LineAngle > 270 && LineAngle <= 360)
                                {
                                    this.LabelTemplatePosition = new Point(EndPoint.X - this.labelGrid.ActualHeight - 5, EndPoint.Y + this.labelGrid.ActualWidth);
                                }
                            }
                            else
                            {
                                double angle = (this.LineAngle * Math.PI) / 180;
                                this.LabelTemplatePosition = new Point(EndPoint.X - (Math.Cos(angle) * (this.labelGrid.ActualWidth + 10)), EndPoint.Y - (Math.Sin(angle) * (this.labelGrid.ActualWidth + 10)));
                            }
                        }
                        else
                        {
                            if (LineAngle > 90 && LineAngle < 270)
                            {
                                double angle = (this.LineAngle * Math.PI) / 180;
                                LabelTemplateAngle = 180;
                                this.LabelTemplatePosition = new Point((StartPoint.X + EndPoint.X) / 2 + (Math.Cos(angle) * (this.labelGrid.DesiredSize.Width / 2)), (StartPoint.Y + EndPoint.Y) / 2 + (Math.Sin(angle) * (this.labelGrid.DesiredSize.Width / 2)));
                            }
                            else
                            {
                                double angle = (this.LineAngle * Math.PI) / 180;
                                LabelTemplateAngle = 0;
                                this.LabelTemplatePosition = new Point((StartPoint.X + EndPoint.X) / 2 - (Math.Cos(angle) * (this.labelGrid.DesiredSize.Width / 2)), (StartPoint.Y + EndPoint.Y) / 2 - (Math.Sin(angle) * (this.labelGrid.DesiredSize.Width / 2)));

                            }
                        }
                    }

                    RotateTransform transform = new RotateTransform();
                    RotateTransform ttransform = new RotateTransform();
                    if (this.LineAngle < 270 && this.LineAngle > 90)
                    {

                        if ((this.LabelOrientation == LabelOrientation.Vertical || this.LabelTemplateOrientation == LabelOrientation.Vertical) && this.ConnectorType == ConnectorType.Straight)
                        {
                            if (this.LabelOrientation == LabelOrientation.Vertical)
                            {
                                transform.Angle = 270;
                            }

                            if (this.LabelHorizontalAlignment == HorizontalAlignment.Center)
                            {
                                this.LabelPosition = new Point((startPoint.X + endPoint.X) / 2 - this.linelabel.DesiredSize.Height / 2, (startPoint.Y + endPoint.Y) / 2 + this.linelabel.DesiredSize.Width / 2);
                            }
                            if (this.LabelTemplateOrientation == LabelOrientation.Vertical)
                            {
                                ttransform.Angle = 270;
                            }

                            if (this.LabelTemplateHorizontalAlignment == HorizontalAlignment.Center)
                            {
                                this.LabelTemplatePosition = new Point((startPoint.X + endPoint.X) / 2 - this.labelGrid.ActualHeight / 2, (startPoint.Y + endPoint.Y) / 2 + this.labelGrid.ActualWidth / 2);

                            }

                        }
                        else if ((this.LabelOrientation == LabelOrientation.Horizontal || this.LabelTemplateOrientation == LabelOrientation.Horizontal) && this.ConnectorType == ConnectorType.Straight)
                        {
                            if (this.LabelOrientation == LabelOrientation.Horizontal)
                            {
                                transform.Angle = 0;
                            }
                            if (this.LabelHorizontalAlignment == HorizontalAlignment.Center)
                            {
                                this.LabelPosition = new Point((startPoint.X + endPoint.X) / 2 - this.linelabel.DesiredSize.Width / 2, (startPoint.Y + endPoint.Y) / 2 - this.linelabel.DesiredSize.Height / 2);
                            }
                            if (this.LabelTemplateOrientation == LabelOrientation.Horizontal)
                            {
                                ttransform.Angle = 0;
                            }
                            if (this.LabelTemplateHorizontalAlignment == HorizontalAlignment.Center)
                            {
                                this.LabelTemplatePosition = new Point((startPoint.X + endPoint.X) / 2 - this.labelGrid.ActualWidth / 2, (startPoint.Y + endPoint.Y) / 2 - this.labelGrid.ActualHeight / 2);
                            }

                        }
                        else
                        {
                            // transform.Angle = 180 + this.LineAngle + this.LabelAngle;
                            double angle = (this.LineAngle * Math.PI) / 180;
                            //this.LabelPosition = new Point(this.LabelPosition.X + (Math.Cos(angle) * this.linelabel.ActualWidth), this.LabelPosition.Y + (Math.Sin(angle) * this.linelabel.ActualWidth));
                            //  this.LabelTemplatePosition = new Point(this.LabelTemplatePosition.X + (Math.Cos(angle) * this.labelGrid.ActualWidth), this.LabelTemplatePosition.Y + (Math.Sin(angle) * this.labelGrid.ActualWidth));

                        }

                    }
                    else
                    {
                        if ((this.LabelOrientation == LabelOrientation.Vertical || this.LabelTemplateOrientation == LabelOrientation.Vertical) && this.ConnectorType == ConnectorType.Straight)
                        {

                            //  this.LabelAngle = 270;
                            if (this.LabelOrientation == LabelOrientation.Vertical)
                            {
                                transform.Angle = 270;
                            }

                            if (this.LabelHorizontalAlignment == HorizontalAlignment.Center)
                            {
                                this.LabelPosition = new Point((startPoint.X + endPoint.X) / 2 - this.linelabel.DesiredSize.Height / 2, (startPoint.Y + endPoint.Y) / 2 + this.linelabel.DesiredSize.Width / 2);
                            }
                            if (this.LabelTemplateOrientation == LabelOrientation.Vertical)
                            {
                                ttransform.Angle = 270;
                            }

                            if (this.LabelTemplateHorizontalAlignment == HorizontalAlignment.Center)
                            {
                                this.LabelTemplatePosition = new Point((startPoint.X + endPoint.X) / 2 - this.linelabel.DesiredSize.Height / 2, (startPoint.Y + endPoint.Y) / 2 + this.linelabel.DesiredSize.Width / 2);
                            }
                        }
                        else if ((this.LabelOrientation == LabelOrientation.Horizontal || this.LabelTemplateOrientation == LabelOrientation.Horizontal) && this.ConnectorType == ConnectorType.Straight)
                        {
                            //  this.LabelAngle = 270;
                            if (this.LabelOrientation == LabelOrientation.Horizontal)
                            {
                                transform.Angle = 0;
                            }
                            if (this.LabelHorizontalAlignment == HorizontalAlignment.Center)
                            {
                                this.LabelPosition = new Point((startPoint.X + endPoint.X) / 2 - this.linelabel.DesiredSize.Width / 2, (startPoint.Y + endPoint.Y) / 2 - this.linelabel.DesiredSize.Height / 2);
                            }
                            if (this.LabelTemplateOrientation == LabelOrientation.Horizontal)
                            {
                                ttransform.Angle = 0;
                            }
                            if (this.LabelTemplateHorizontalAlignment == HorizontalAlignment.Center)
                            {
                                this.LabelTemplatePosition = new Point((startPoint.X + endPoint.X) / 2 - this.linelabel.DesiredSize.Width / 2, (startPoint.Y + endPoint.Y) / 2 - this.linelabel.DesiredSize.Height / 2);

                            }
                        }
                        else
                        {
                            transform.Angle = this.LineAngle + this.LabelAngle;
                            ttransform.Angle = this.LineAngle + this.LabelAngle;
                        }
                    }
                    if (this.LabelOrientation == LabelOrientation.Auto)
                    {
                        transform.Angle = this.LineAngle + this.LabelAngle;
                    }
                    if (this.LabelTemplateOrientation == LabelOrientation.Auto)
                    {
                        ttransform.Angle = this.LineAngle + this.LabelAngle;
                    }
                    (this.linetext.Parent as Grid).RenderTransform = transform;
                    this.labelGrid.RenderTransform = ttransform;

                }
                catch
                {
                }
            }
        }



        internal Point GeneralPointRotation(Point originpoint, Point endpoint, double angle)
        {
            double ang = angle * Math.PI / 180;
            Point displacement = new Point(endpoint.X - originpoint.X, endpoint.Y - originpoint.Y);
            endpoint.X = (displacement.X * Math.Cos(ang)) - (displacement.Y * Math.Sin(ang));
            endpoint.Y = (displacement.Y * Math.Cos(ang)) + (displacement.X * Math.Sin(ang));
            endpoint.X += originpoint.X;
            endpoint.Y += originpoint.Y;
            return endpoint;
        }
        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (e.Key == Key.Enter)
            {
                if (this.LabelVisibility == Visibility.Visible)
                {
                    this.linelabel.Text = this.linetext.Text;
                    this.linelabel.Visibility = Visibility.Visible;
                    this.linetext.Visibility = Visibility.Collapsed;                                        
                }
            }

        }
        #region Class Fields

        #endregion

        #region Initialization

        #endregion


        #region Properties

        #endregion

        #region Class Override

        #endregion

        #region Methods
        private void LineSelection(LineConnector line)
        {
            if (line.IsSelected == true)
            {
                n++;
                if (line.HeadDecoratorGrid != null)
                {
                    Border outerborder = new Border();
                    outerborder.Name = "PART_HeadBorder" + n.ToString();
                    outerborder.Background = new SolidColorBrush(Colors.LightGray);
                    outerborder.BorderThickness = new Thickness(1);
                    outerborder.BorderBrush = new SolidColorBrush(Colors.DarkGray);
                    outerborder.Opacity = 0.7;
                    try
                    {
                        line.HeadDecoratorGrid.Children.Add(outerborder);
                    }
                    catch
                    {
                    }
                }

                if (line.TailDecoratorGrid != null)
                {
                    Border outerborder = new Border();
                    outerborder.Name = "PART_TailBorder" + n.ToString();
                    outerborder.Background = new SolidColorBrush(Colors.LightGray);
                    outerborder.BorderThickness = new Thickness(1);
                    outerborder.BorderBrush = new SolidColorBrush(Colors.Gray);
                    outerborder.Opacity = 0.7;
                    try
                    {
                        line.TailDecoratorGrid.Children.Add(outerborder);
                    }
                    catch
                    {
                    }
                }
            }
            else
            {
                if (line.HeadDecoratorGrid != null && line.HeadDecoratorGrid.Children.Count >= 2)
                {
                    line.HeadDecoratorGrid.Children.Remove(line.HeadDecoratorGrid.Children.ElementAt(1));
                }

                if (line.TailDecoratorGrid != null && line.TailDecoratorGrid.Children.Count >= 2)
                {
                    line.TailDecoratorGrid.Children.Remove(line.TailDecoratorGrid.Children.ElementAt(1));
                }
            }
        }
        #endregion
    }
}
