// <copyright file="ConnectorBase.cs" company="Syncfusion">
// Copyright Syncfusion Inc. 2001 - 2011. All rights reserved.
// Use of this code is subject to the terms of our license.
// A copy of the current license can be obtained at any time by e-mailing
// licensing@syncfusion.com. Any infringement will be prosecuted under
// applicable laws. 
// </copyright>

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
    using System.Windows.Media;
    using System.Windows.Shapes;

    /// <summary>
    /// Represents base abstract class for Connectors.
    /// </summary>
    public abstract class ConnectorBase : ContentControl, IEdge, ICommon, INodeGroup
    {
        /// <summary>
        /// Identifies the Center dependency property.
        /// </summary>
        public static readonly DependencyProperty CenterProperty = DependencyProperty.Register("Center", typeof(Point), typeof(ConnectorBase), new PropertyMetadata(new Point(0, 0)));

        /// <summary>
        /// Identifies the ConnectionHeadPort dependency property.
        /// </summary>
        public static readonly DependencyProperty ConnectionHeadPortProperty = DependencyProperty.Register("ConnectionHeadPort", typeof(ConnectionPort), typeof(ConnectorBase), new PropertyMetadata(null, new PropertyChangedCallback(OnHeadPortChanged)));

        /// <summary>
        /// Identifies the ConnectionPoints dependency property.
        /// </summary>
        public static readonly DependencyProperty ConnectionPointsProperty = DependencyProperty.Register("ConnectionPoints", typeof(List<Point>), typeof(ConnectorBase), new PropertyMetadata(new List<Point>()));

        /// <summary>
        /// Identifies the ConnectionTailPort dependency property.
        /// </summary>
        public static readonly DependencyProperty ConnectionTailPortProperty = DependencyProperty.Register("ConnectionTailPort", typeof(ConnectionPort), typeof(ConnectorBase), new PropertyMetadata(null, new PropertyChangedCallback(OnTailPortChanged)));

        /// <summary>
        /// Identifies current ConnectorType.  This is a dependency property.
        /// </summary>
        public static readonly DependencyProperty ConnectorTypeProperty = DependencyProperty.Register("ConnectorType", typeof(ConnectorType), typeof(ConnectorBase), new PropertyMetadata(ConnectorType.Orthogonal));

        /// <summary>
        /// Identifies the Distance dependency property.
        /// </summary>
        public static readonly DependencyProperty DistanceProperty = DependencyProperty.Register("Distance", typeof(double), typeof(ConnectorBase), new PropertyMetadata(0d));

        /// <summary>
        /// Identifies the LabelAngle dependency property.
        /// </summary>
        public static readonly DependencyProperty EditorAngleProperty = DependencyProperty.Register("EditorAngle", typeof(double), typeof(ConnectorBase), new PropertyMetadata(0d));

        /// <summary>
        /// Identifies current HeadDecoratorPosition.  This is a dependency property.
        /// </summary>
        public static readonly DependencyProperty HeadDecoratorPositionProperty = DependencyProperty.Register("HeadDecoratorPosition", typeof(Point), typeof(ConnectorBase), new PropertyMetadata(new Point(0, 0)));

        /// <summary>
        /// Identifies current HeadNode.  This is a dependency property.
        /// </summary>
        public static readonly DependencyProperty HeadNodeProperty = DependencyProperty.Register("HeadNode", typeof(Node), typeof(ConnectorBase), new PropertyMetadata(null, new PropertyChangedCallback(OnHeadNodeChanged)));

        /// <summary>
        /// Identifies the IsGroup dependency property.
        /// </summary>
        public static readonly DependencyProperty IsGroupedProperty = DependencyProperty.Register("IsGrouped", typeof(bool), typeof(ConnectorBase), new PropertyMetadata(false));

        /// <summary>
        /// Identifies the IsLabelEditable dependency property.
        /// </summary>
        public static readonly DependencyProperty IsLabelEditableProperty = DependencyProperty.Register("IsLabelEditable", typeof(bool), typeof(ConnectorBase), new PropertyMetadata(true));

        /// <summary>
        /// Identifies the LabelAngle dependency property.
        /// </summary>
        public static readonly DependencyProperty LabelAngleProperty = DependencyProperty.Register("LabelAngle", typeof(double), typeof(ConnectorBase), new PropertyMetadata(0d));

        /// <summary>
        ///  Identifies the LabelBackground dependency property.
        /// </summary>
        public static readonly DependencyProperty LabelBackgroundProperty = DependencyProperty.Register("LabelBackground", typeof(Brush), typeof(ConnectorBase), new PropertyMetadata(new SolidColorBrush(Colors.Transparent)));

        /// <summary>
        /// Identifies the LabelFontFamily dependency property.
        /// </summary>
        public static readonly DependencyProperty LabelFontFamilyProperty = DependencyProperty.Register("LabelFontFamily", typeof(FontFamily), typeof(ConnectorBase), new PropertyMetadata(new FontFamily("Verdana")));

        /// <summary>
        /// Identifies the LabelFontSize dependency property.
        /// </summary>
        public static readonly DependencyProperty LabelFontSizeProperty = DependencyProperty.Register("LabelFontSize", typeof(double), typeof(ConnectorBase), new PropertyMetadata(11d));

        /// <summary>
        /// Identifies the LabelFontStyle dependency property.
        /// </summary>
        public static readonly DependencyProperty LabelFontStyleProperty = DependencyProperty.Register("LabelFontStyle", typeof(FontStyle), typeof(ConnectorBase), new PropertyMetadata(FontStyles.Normal));

        /// <summary>
        /// Identifies the LabelFontWeight dependency property.
        /// </summary>
        public static readonly DependencyProperty LabelFontWeightProperty = DependencyProperty.Register("LabelFontWeight", typeof(FontWeight), typeof(ConnectorBase), new PropertyMetadata(FontWeights.SemiBold));

        /// <summary>
        ///  Identifies the LabelForeground dependency property.
        /// </summary>
        public static readonly DependencyProperty LabelForegroundProperty = DependencyProperty.Register("LabelForeground", typeof(Brush), typeof(ConnectorBase), new PropertyMetadata(new SolidColorBrush(Colors.Black)));

        /// <summary>
        /// Identifies the LabelHeight dependency property.
        /// </summary>
        public static readonly DependencyProperty LabelHeightProperty = DependencyProperty.Register("LabelHeight", typeof(double), typeof(ConnectorBase), new PropertyMetadata(0d));

        /// <summary>
        /// Identifies the LabelHorizontalAlignment dependency property.
        /// </summary>
        public static readonly DependencyProperty LabelHorizontalAlignmentProperty = DependencyProperty.Register("LabelHorizontalAlignment", typeof(HorizontalAlignment), typeof(ConnectorBase), new PropertyMetadata(HorizontalAlignment.Center));
        /// <summary>
        /// Identifies the LabelTemplatePosition dependency property.
        /// </summary>
        public static readonly DependencyProperty LabelTemplatePositionProperty = DependencyProperty.Register("LabelTemplatePosition", typeof(Point), typeof(ConnectorBase), new PropertyMetadata(new Point(0, 0), new PropertyChangedCallback(OnLabelTemplatePositionChanged)));

        /// <summary>
        /// Identifies the LabelTemplateAngle dependency property.
        /// </summary>
        public static readonly DependencyProperty LabelTemplateAngleProperty = DependencyProperty.Register("LabelTemplateAngle", typeof(double), typeof(ConnectorBase), new PropertyMetadata(0d));

        /// <summary>
        /// Identifies the LabelPosition dependency property.
        /// </summary>
        public static readonly DependencyProperty LabelPositionProperty = DependencyProperty.Register("LabelPosition", typeof(Point), typeof(ConnectorBase), new PropertyMetadata(new Point(0, 0), new PropertyChangedCallback(OnLabelPositionChanged)));

        /// <summary>
        /// Identifies the Label dependency property.
        /// </summary>
        public static readonly DependencyProperty LabelProperty = DependencyProperty.Register("Label", typeof(string), typeof(ConnectorBase), new PropertyMetadata(string.Empty, new PropertyChangedCallback(OnLabelChanged)));

        /// <summary>
        /// Identifies the LabelHorizontalAlignment dependency property.
        /// </summary>
        public static readonly DependencyProperty LabelTemplateHorizontalAlignmentProperty = DependencyProperty.Register("LabelTemplateHorizontalAlignment", typeof(HorizontalAlignment), typeof(ConnectorBase), new PropertyMetadata(HorizontalAlignment.Center));

        /// <summary>
        /// Identifies the Label Template.  This is a dependency property.
        /// </summary>
        public static readonly DependencyProperty LabelTemplateProperty = DependencyProperty.Register("LabelTemplate", typeof(DataTemplate), typeof(ConnectorBase), new PropertyMetadata(new DataTemplate()));

        /// <summary>
        /// Identifies the LabelVerticalAlignment dependency property.
        /// </summary>
        public static readonly DependencyProperty LabelTemplateVerticalAlignmentProperty = DependencyProperty.Register("LabelTemplateVerticalAlignment", typeof(VerticalAlignment), typeof(ConnectorBase), new PropertyMetadata(VerticalAlignment.Top));

        /// <summary>
        /// Identifies the LabelTextAlignment dependency property.
        /// </summary>
        public static readonly DependencyProperty LabelTextAlignmentProperty = DependencyProperty.Register("LabelTextAlignment", typeof(TextAlignment), typeof(ConnectorBase), new PropertyMetadata(TextAlignment.Center));

        /// <summary>
        /// Identifies the LabelTextWrapping dependency property.
        /// </summary>
        public static readonly DependencyProperty LabelTextWrappingProperty = DependencyProperty.Register("LabelTextWrapping", typeof(TextWrapping), typeof(ConnectorBase), new PropertyMetadata(TextWrapping.NoWrap));

        /// <summary>
        /// Identifies the LabelVerticalAlignment dependency property.
        /// </summary>
        public static readonly DependencyProperty LabelVerticalAlignmentProperty = DependencyProperty.Register("LabelVerticalAlignment", typeof(VerticalAlignment), typeof(ConnectorBase), new PropertyMetadata(VerticalAlignment.Top));

        /// <summary>
        /// Identifies the LabelVisibility dependency property.
        /// </summary>
        public static readonly DependencyProperty LabelVisibilityProperty = DependencyProperty.Register("LabelVisibility", typeof(Visibility), typeof(ConnectorBase), new PropertyMetadata(Visibility.Visible));

        /// <summary>
        /// Identifies the LabelWidth dependency property.
        /// </summary>
        public static readonly DependencyProperty LabelWidthProperty = DependencyProperty.Register("LabelWidth", typeof(double), typeof(ConnectorBase), new PropertyMetadata(double.NaN));

        /// <summary>
        /// Identifies the MeasurementUnit dependency property.
        /// </summary>
        public static readonly DependencyProperty MeasurementUnitProperty = DependencyProperty.Register("MeasurementUnit", typeof(MeasureUnits), typeof(ConnectorBase), new PropertyMetadata(MeasureUnits.Pixel, new PropertyChangedCallback(OnUnitsChanged)));

        /// <summary>
        /// Identifies the ParentId dependency property.
        /// </summary>
        public static readonly DependencyProperty ParentIDProperty = DependencyProperty.Register("ParentID", typeof(Guid), typeof(ConnectorBase), new PropertyMetadata(new Guid()));

        /// <summary>
        /// Identifies current TailDecoratorPosition.  This is a dependency property.
        /// </summary>
        public static readonly DependencyProperty TailDecoratorPositionProperty = DependencyProperty.Register("TailDecoratorPosition", typeof(Point), typeof(ConnectorBase), new PropertyMetadata(new Point(0, 0)));

        /// <summary>
        /// Identifies current TailNode.  This is a dependency property.
        /// </summary>
        public static readonly DependencyProperty TailNodeProperty = DependencyProperty.Register("TailNode", typeof(Node), typeof(ConnectorBase), new PropertyMetadata(null, new PropertyChangedCallback(OnTailNodeChanged)));

        /// <summary>
        /// Defines the TextWidth property.This is a dependency property.
        /// </summary>
        public static readonly DependencyProperty TextWidthProperty = DependencyProperty.Register("TextWidth", typeof(double), typeof(ConnectorBase), new PropertyMetadata(0d, new PropertyChangedCallback(OnTextWidthChanged)));


        /// <summary>
        /// Used to set LabelOrientation of the Line Connector.
        /// </summary>
        public static readonly DependencyProperty LabelOrientationProperty = DependencyProperty.Register("LabelOrientation", typeof(LabelOrientation), typeof(ConnectorBase), new PropertyMetadata(LabelOrientation.Auto, new PropertyChangedCallback(OnLabelOrientationChanged)));

        /// <summary>
        /// LabelTemplateOrientationProperty
        /// </summary>
        public static readonly DependencyProperty LabelTemplateOrientationProperty = DependencyProperty.Register("LabelTemplateOrientation", typeof(LabelOrientation), typeof(ConnectorBase), new PropertyMetadata(LabelOrientation.Auto, new PropertyChangedCallback(OnLabelTemplateOrientationChanged)));
       
        /// <summary>
        /// Used to store start point position value.
        /// </summary>
        internal Point mstartpointposition = new Point(0, 0);

        /// <summary>
        /// Used to store end point position value.
        /// </summary>
        internal Point mendpointposition = new Point(0, 0);

        /// <summary>
        /// Used to store the groups.
        /// </summary>
        private CollectionExt mgroups = new CollectionExt();

        /// <summary>
        /// Used to specify the head decorator shape.
        /// </summary>
        private DecoratorShape mheadDecoratorShape = DecoratorShape.None;

        /// <summary>
        /// Used to store the head decorator style property values.
        /// </summary>
        private DecoratorStyle mheadDecoratorStyle;

        /// <summary>
        /// Used to store the new ZIndex value.
        /// </summary>
        private int mnewindex = 0;

        /// <summary>
        /// Used to store the old ZIndex value.
        /// </summary>
        private int moldindex = 0;

        /// <summary>
        /// Used to store the view object.
        /// </summary>
        private DiagramView dview;

        /// <summary>
        /// Used to store head node reference no.
        /// </summary>
        private int hid = -1;

        /// <summary>
        /// Used to store head port reference no.
        /// </summary>
        private int hpid;

        /// <summary>
        /// Used to store ISelected property setting value.
        /// </summary>
        private bool isSelected;

        /// <summary>
        /// Used to store the line style property values.
        /// </summary>
        private LineStyle lineStyle;

        /// <summary>
        /// Used to store the connector drop point.
        /// </summary>
        private Point mdroppoint = new Point(0, 0);

        /// <summary>
        /// Used to specify the tail decorator shape.
        /// </summary>
        private DecoratorShape mtailDecoratorShape = DecoratorShape.Arrow;

        /// <summary>
        /// Used to store the tail decorator style property values.
        /// </summary>
        private DecoratorStyle mtailDecoratorStyle;

        /// <summary>
        /// Used to store IsDirected property information.
        /// </summary>
        private bool isDirected = false;

        /// <summary>
        /// Used to store the reference number of the nodes and connectors.
        /// </summary>
        private int no = -1;

        /// <summary>
        /// Used to store tail node reference no.
        /// </summary>
        private int tid = -1;

        /// <summary>
        /// Used to store tail port reference no.
        /// </summary>
        private int tpid;

        private DiagramModel m_Model;

        internal void setMode(DiagramModel model)
        {
            m_Model = model;
        }

        protected TreeOrientation Orientation
        {
            get
            {
                if (m_Model != null)
                {
                    return m_Model.Orientation;
                }
                else
                {
                    return TreeOrientation.TopBottom;
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConnectorBase"/> class.
        /// </summary>
        public ConnectorBase()
        {
            this.Unloaded += new RoutedEventHandler(ConnectorBase_Unloaded);
        }

        void ConnectorBase_Unloaded(object sender, RoutedEventArgs e)
        {
            this.Loaded += new RoutedEventHandler(ConnectorBase_Loaded);
            if (this.HeadNode != null)
            {
                (this.HeadNode as Node).PropertyChanged -= Line_PropertyChanged;
            }

            if (this.TailNode != null)
            {
                (this.TailNode as Node).PropertyChanged -= Line_PropertyChanged;
            }
        }

        void ConnectorBase_Loaded(object sender, RoutedEventArgs e)
        {
            this.Loaded -= ConnectorBase_Loaded;
            if (this.HeadNode != null)
            {
                (this.HeadNode as Node).PropertyChanged += Line_PropertyChanged;
            }

            if (this.TailNode != null)
            {
                (this.TailNode as Node).PropertyChanged += Line_PropertyChanged;
            }

        }

        /// <summary>
        /// Calls property changed event handler.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets or sets the head port of the connector.
        /// </summary>
        /// <value>The port to which the connection is to be made.</value>
        /// <remarks>
        /// When specifying the <see cref="ConnectionHeadPort"/>, the <see cref="HeadNode"/> should also be specified.
        /// </remarks>
        /// <example>
        /// <code language="C#">
        /// using Syncfusion.Windows.Diagram;
        ///namespace SilverlightApplication1
        /// {
        /// public partial class MainPage : UserControl
        /// {
        /// public DiagramControl Control;
        /// public DiagramModel Model;
        /// public DiagramView View;
        /// public MainPage()
        /// {
        /// InitializeComponent ();
        /// Control = new DiagramControl ();
        /// Model = new DiagramModel ();
        /// View = new DiagramView ();
        /// Control.View = View;
        /// Control.Model = Model;
        /// View.Bounds = new Thickness(0, 0, 1000, 1000);
        /// //Creating node
        /// Node n = new Node(Guid.NewGuid(), "Start");
        /// n.Shape = Shapes.FlowChart_Start;
        /// n.IsLabelEditable = true;
        /// n.Label = "Start";
        /// n.Level = 1;
        /// n.OffsetX = 150;
        /// n.OffsetY = 25;
        /// n.Width = 150;
        /// n.Height = 75;
        /// n.ToolTip="Start Node";
        /// Model.Nodes.Add(n);
        /// //Adding a port to the node
        /// ConnectionPort port = new ConnectionPort();
        /// port.Node=n;
        /// port.Left=75;
        /// port.Top=10;
        /// port.PortShape = PortShapes.Arrow;
        /// port.PortStyle.Fill = Brushes.Transparent;
        /// port.Height = 11;
        /// port.Width = 11;
        /// n.Ports.Add(port);
        /// Node n1 = new Node(Guid.NewGuid(), "Decision1");
        /// n1.Shape = Shapes.FlowChart_Process;
        /// n1.IsLabelEditable = true;
        /// n1.Label = "Alarm Rings";
        /// n1.Level = 2;
        /// n1.OffsetX = 150;
        /// n1.OffsetY = 125;
        /// n1.Width = 150;
        /// n1.Height = 75;
        /// Model.Nodes.Add(n1);
        /// ConnectionPort port1 = new ConnectionPort();
        /// port1.Node=n;
        /// port1.Left=75;
        /// port1.Top=50;
        /// port1.PortShape = PortShapes.Arrow;
        /// port1.PortStyle.Fill = Brushes.Transparent;
        /// port1.Height = 11;
        /// port1.Width = 11;
        /// n1.Ports.Add(port1);
        /// //Creating a connection.
        /// LineConnector o2 = new LineConnector();
        /// o2.ConnectorType = ConnectorType.Straight;
        /// o2.TailNode = n1;
        /// o2.HeadNode = n;
        /// o2.LabelHorizontalAlignment = HorizontalAlignment.Center;
        /// //Specifying the port to connect to.
        /// o2.ConnectionHeadPort = port;
        /// o2.ConnectionTailPort = port1;
        /// o2.HeadDecoratorShape=DecoratorShape.Arrow;
        /// o2.TailDecoratorShape=DecoratorShape.Arrow;
        /// Model.Connections.Add(o2);
        /// }
        /// }
        /// }
        /// </code>
        /// </example>
        /// <seealso cref="ConnectionPort"/>
        public ConnectionPort ConnectionHeadPort
        {
            get
            {
                return (ConnectionPort)GetValue(ConnectionHeadPortProperty);
            }

            set
            
            {
                SetValue(ConnectionHeadPortProperty, value);
                this.OnPropertyChanged("ConnectionHeadPort");
                this.UpdateConnectorPathGeometry();
            }
        }


        /// <summary>
        /// Gets or sets the tail port of the connector.
        /// </summary>
        /// <value>The port to which the connection is to be made.</value>
        /// <remarks>
        /// When specifying the <see cref="ConnectionTailPort"/>, the <see cref="TailNode"/> should also be specified.
        /// </remarks>
        /// <example>
        /// <code language="C#">
        /// using Syncfusion.Windows.Diagram;
        ///namespace SilverlightApplication1
        /// {
        /// public partial class MainPage : UserControl
        /// {
        /// public DiagramControl Control;
        /// public DiagramModel Model;
        /// public DiagramView View;
        /// public MainPage()
        /// {
        /// InitializeComponent ();
        /// Control = new DiagramControl ();
        /// Model = new DiagramModel ();
        /// View = new DiagramView ();
        /// Control.View = View;
        /// Control.Model = Model;
        /// View.Bounds = new Thickness(0, 0, 1000, 1000);
        /// //Creating node
        /// Node n = new Node(Guid.NewGuid(), "Start");
        /// n.Shape = Shapes.FlowChart_Start;
        /// n.IsLabelEditable = true;
        /// n.Label = "Start";
        /// n.Level = 1;
        /// n.OffsetX = 150;
        /// n.OffsetY = 25;
        /// n.Width = 150;
        /// n.Height = 75;
        /// n.ToolTip="Start Node";
        /// Model.Nodes.Add(n);
        /// //Adding a port to the node
        /// ConnectionPort port = new ConnectionPort();
        /// port.Node=n;
        /// port.Left=75;
        /// port.Top=10;
        /// port.PortShape = PortShapes.Arrow;
        /// port.PortStyle.Fill = Brushes.Transparent;
        /// port.Height = 11;
        /// port.Width = 11;
        /// n.Ports.Add(port);
        /// Node n1 = new Node(Guid.NewGuid(), "Decision1");
        /// n1.Shape = Shapes.FlowChart_Process;
        /// n1.IsLabelEditable = true;
        /// n1.Label = "Alarm Rings";
        /// n1.Level = 2;
        /// n1.OffsetX = 150;
        /// n1.OffsetY = 125;
        /// n1.Width = 150;
        /// n1.Height = 75;
        /// Model.Nodes.Add(n1);
        /// ConnectionPort port1 = new ConnectionPort();
        /// port1.Node=n;
        /// port1.Left=75;
        /// port1.Top=50;
        /// port1.PortShape = PortShapes.Arrow;
        /// port1.PortStyle.Fill = Brushes.Transparent;
        /// port1.Height = 11;
        /// port1.Width = 11;
        /// n1.Ports.Add(port1);
        /// //Creating a connection.
        /// LineConnector o2 = new LineConnector();
        /// o2.ConnectorType = ConnectorType.Straight;
        /// o2.TailNode = n1;
        /// o2.HeadNode = n;
        /// o2.LabelHorizontalAlignment = HorizontalAlignment.Center;
        /// //Specifying the port to connect to.
        /// o2.ConnectionHeadPort = port;
        /// o2.ConnectionTailPort = port1;
        /// Model.Connections.Add(o2);
        /// }
        /// }
        /// }
        /// </code>
        /// </example>
        public ConnectionPort ConnectionTailPort
        {
            get
            {
                return (ConnectionPort)GetValue(ConnectionTailPortProperty);
            }

            set
            {
                SetValue(ConnectionTailPortProperty, value);
                this.OnPropertyChanged("ConnectionTailPort");
                this.UpdateConnectorPathGeometry();
            }
        }

        /// <summary>
        /// Gets or sets the type of connection to be used.This is a dependency property.
        /// </summary>
        /// <value>
        /// Type: <see cref="ConnectorType"/>
        /// Enum specifying the type of the connector to be used.
        /// </value>
        /// <remarks>
        /// Three types of connectors are provided namely Orthogonal, Bezier and Straight. Default value is Orthogonal.
        /// </remarks>
        /// <example>
        /// <para/>This example shows how to set ConnectorType in C#.
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
        ///        Node n = new Node(Guid.NewGuid(), "Start");
        ///        n.Shape = Shapes.FlowChart_Start;
        ///        n.OffsetX = 150;
        ///        n.OffsetY = 25;
        ///        n.Width = 150;
        ///        n.Height = 75;
        ///        Model.Nodes.Add(n);
        ///         Node n1 = new Node(Guid.NewGuid(), "Decision1");
        ///         n1.Shape = Shapes.FlowChart_Process;
        ///         n1.Label = "Alarm Rings";
        ///         n1.OffsetX = 150;
        ///         n1.OffsetY = 125;
        ///         n1.Width = 150;
        ///         n1.Height = 75;
        ///         Model.Nodes.Add(n1);
        ///         LineConnector connObject = new LineConnector();
        ///         connObject.ConnectorType = ConnectorType.Straight;
        ///         connObject.TailNode = n1;
        ///         connObject.HeadNode = n;
        ///         connObject.ConnectorType = ConnectorType.Orthogonal;
        ///         Model.Connections.Add(connObject);
        ///    }
        ///    }
        ///    }
        /// </code>
        /// </example>
        /// <seealso cref="ConnectorType"/>
        public ConnectorType ConnectorType
        {
            get
            {
                return (ConnectorType)GetValue(ConnectorTypeProperty);
            }

            set
            {
                SetValue(ConnectorTypeProperty, value);
            }
        }

        internal Point PxEndPointPosition
        {
            get
            {
                return MeasureUnitsConverter.ToPixels(EndPointPosition, MeasurementUnit);
            }
            set
            {
                if (this.dview != null && (this as LineConnector).linedragging)
                {
                    if (this.dview.SnapToHorizontalGrid)
                    {
                        value.X = Node.Round(value.X, dview.PxSnapOffsetX);
                    }
                    if (this.dview.SnapToVerticalGrid)
                    {
                        value.Y = Node.Round(value.Y, dview.PxSnapOffsetY);
                    }
                }
                EndPointPosition = MeasureUnitsConverter.FromPixels(value, MeasurementUnit);
            }
        }

        /// <summary>
        /// Gets or sets the end point position.
        /// </summary>
        /// <value>The end point position.</value>
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
        ///         o.StartPointPosition=new Point(100,100);
        ///         o.EndPointPosition=new Point(200,200);
        ///         Model.Connections.Add(o);
        ///    }
        ///    }
        ///    }
        /// </code>
        /// </example>
        public Point EndPointPosition
        {
            get { return this.mendpointposition; }
            set
            {
                if (dview != null && EndPointPosition != value)
                {
                    if (dview.m_IsCommandInProgress)
                    {
                        if (!dview.Undone && !dview.Redone)
                        {
                            dview.tUndoStack.Push(new LineOperation(LineOperations.Dragged, this as LineConnector));
                        }
                    }
                } this.mendpointposition = value;
            }
        }

        /// <summary>
        /// Gets the groups to which the INodeGroup objects belong.
        /// </summary>
        /// <value>The groups.</value>
        public CollectionExt Groups
        {
            get
            {
                return this.mgroups;
            }
        }

        /// <summary>
        /// Gets or sets the point where the head decorator is to be positioned.
        /// </summary>
        /// <value>
        /// Type: <see cref="Point"/>
        /// The point of the head decorator position.
        /// </value>
        /// <example>
        /// <para/>This example shows how to set ConnectorType in C#.
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
        ///        Node n = new Node(Guid.NewGuid(), "Start");
        ///        n.Shape = Shapes.FlowChart_Start;
        ///        n.OffsetX = 150;
        ///        n.OffsetY = 25;
        ///        n.Width = 150;
        ///        n.Height = 75;
        ///        Model.Nodes.Add(n);
        ///         Node n1 = new Node(Guid.NewGuid(), "Decision1");
        ///         n1.Shape = Shapes.FlowChart_Process;
        ///         n1.Label = "Alarm Rings";
        ///         n1.OffsetX = 150;
        ///         n1.OffsetY = 125;
        ///         n1.Width = 150;
        ///         n1.Height = 75;
        ///         Model.Nodes.Add(n1);
        ///         LineConnector connObject = new LineConnector();
        ///         connObject.ConnectorType = ConnectorType.Straight;
        ///         connObject.TailNode = n1;
        ///         connObject.HeadNode = n;
        ///         connObject.HeadDecoratorPosition = new Point(100,100);
        ///         Model.Connections.Add(connObject);
        ///    }
        ///    }
        ///    }
        /// </code>
        /// </example>
        public Point HeadDecoratorPosition
        {
            get
            {
                return (Point)GetValue(HeadDecoratorPositionProperty);
            }

            set
            {
                SetValue(HeadDecoratorPositionProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the shape to be used as the head decorator.
        /// </summary>
        /// <value>
        /// Type: <see cref="DecoratorShape"/>
        /// Enum specifying the shape of the head decorator.
        /// </value>
        /// <example>
        /// <para/>This example shows how to set HeadDecoratorShape in C#.
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
        ///        Node n = new Node(Guid.NewGuid(), "Start");
        ///        n.Shape = Shapes.FlowChart_Start;
        ///        n.OffsetX = 150;
        ///        n.OffsetY = 25;
        ///        n.Width = 150;
        ///        n.Height = 75;
        ///        Model.Nodes.Add(n);
        ///         Node n1 = new Node(Guid.NewGuid(), "Decision1");
        ///         n1.Shape = Shapes.FlowChart_Process;
        ///         n1.Label = "Alarm Rings";
        ///         n1.OffsetX = 150;
        ///         n1.OffsetY = 125;
        ///         n1.Width = 150;
        ///         n1.Height = 75;
        ///         Model.Nodes.Add(n1);
        ///         LineConnector connObject = new LineConnector();
        ///         connObject.ConnectorType = ConnectorType.Straight;
        ///         connObject.TailNode = n1;
        ///         connObject.HeadNode = n;
        ///         connObject.ConnectorType = ConnectorType.Orthogonal;
        ///         connObject.HeadDecoratorShape = DecoratorShape.Arrow;
        ///         Model.Connections.Add(connObject);
        ///    }
        ///    }
        ///    }
        /// </code>
        /// </example>
        /// <remarks>
        /// Several shapes like None, Arrow, Diamond and Circle have been provided. Default shape is None.
        /// </remarks>
        /// <seealso cref="DecoratorShape"/>
        public DecoratorShape HeadDecoratorShape
        {
            get
            {
                return this.mheadDecoratorShape;
            }

            set
            {
                if (this.mheadDecoratorShape != value)
                {
                    this.mheadDecoratorShape = value;
                    if (!(this as LineConnector).IsOverlapped)
                    {
                        (this as LineConnector).InternalHeadShape = value;
                    }

                    this.SetShape("Head");
                    this.OnPropertyChanged("HeadDecoratorShape");
                }
            }
        }

        /// <summary>
        /// Gets or sets the  style to be used for the head decorator.
        /// </summary>
        /// <value>
        /// Type: <see cref="DecoratorStyle"/>
        /// HeadDecoratorStyle for the connector.
        /// </value>
        /// <remarks>
        /// The decorator shapes can be customized by using the various DecoratorStyle properties like Fill, Stroke, StrokeThickness, StrokeStartLineCap, StrokeEndLineCap, StrokeLineJoin .
        /// </remarks>
        /// <example>
        /// <para/>This example shows how to set HeadDecoratorStyle in C#.
        /// <code language="C#">
        /// using Syncfusion.Windows.Diagram;
        ///namespace SilverlightApplication1
        /// {
        /// public partial class MainPage : UserControl
        /// {
        /// public DiagramControl Control;
        /// public DiagramModel Model;
        /// public DiagramView View;
        /// public MainPage()
        /// {
        /// InitializeComponent ();
        /// Control = new DiagramControl ();
        /// Model = new DiagramModel ();
        /// View = new DiagramView ();
        /// Control.View = View;
        /// Control.Model = Model;
        /// View.Bounds = new Thickness(0, 0, 1000, 1000);
        /// Node n = new Node(Guid.NewGuid(), "Start");
        /// n.Shape = Shapes.FlowChart_Start;
        /// n.OffsetX = 150;
        /// n.OffsetY = 25;
        /// n.Width = 150;
        /// n.Height = 75;
        /// Model.Nodes.Add(n);
        /// Node n1 = new Node(Guid.NewGuid(), "Decision1");
        /// n1.Shape = Shapes.FlowChart_Process;
        /// n1.Label = "Alarm Rings";
        /// n1.OffsetX = 150;
        /// n1.OffsetY = 125;
        /// n1.Width = 150;
        /// n1.Height = 75;
        /// Model.Nodes.Add(n1);
        /// LineConnector connObject = new LineConnector();
        /// connObject.ConnectorType = ConnectorType.Straight;
        /// connObject.TailNode = n1;
        /// connObject.HeadNode = n;
        /// connObject.ConnectorType = ConnectorType.Orthogonal;
        /// connObject.LineStyle.Fill = Brushes.Red;
        /// connObject.HeadDecoratorStyle.Fill = Brushes.Orange; 
        /// Model.Connections.Add(connObject);
        /// }
        /// }
        /// }
        /// </code>
        /// </example>
        /// <seealso cref="DecoratorStyle"/>
        public DecoratorStyle HeadDecoratorStyle
        {
            get
            {
                if (this.mheadDecoratorStyle != null)
                {
                    return this.mheadDecoratorStyle;
                }
                else
                {
                    this.mheadDecoratorStyle = new DecoratorStyle();
                }

                return this.mheadDecoratorStyle;
            }

            set
            {
                this.mheadDecoratorStyle = value;
            }
        }

        /// <summary>
        /// Gets or sets the first, or source, node upon which this Edge is incident.
        /// </summary>
        /// <value>The head node of the connection</value>
        /// <remarks>
        /// Every node should have a unique name.
        /// </remarks>
        /// <example>
        /// <code language="C#">
        /// using Syncfusion.Windows.Diagram;
        ///namespace SilverlightApplication1
        /// {
        /// public partial class MainPage : UserControl
        /// {
        /// public DiagramControl Control;
        /// public DiagramModel Model;
        /// public DiagramView View;
        /// public MainPage()
        /// {
        /// InitializeComponent ();
        /// Control = new DiagramControl ();
        /// Model = new DiagramModel ();
        /// View = new DiagramView ();
        /// Control.View = View;
        /// Control.Model = Model;
        /// View.Bounds = new Thickness(0, 0, 1000, 1000);
        /// //Creating node
        /// Node n = new Node(Guid.NewGuid(), "Start");
        /// n.Shape = Shapes.FlowChart_Start;
        /// n.IsLabelEditable = true;
        /// n.Label = "Start";
        /// n.Level = 1;
        /// n.OffsetX = 150;
        /// n.OffsetY = 25;
        /// n.Width = 150;
        /// n.Height = 75;
        /// n.ToolTip="Start Node";
        /// Model.Nodes.Add(n);
        /// Node n1 = new Node(Guid.NewGuid(), "Decision1");
        /// n1.Shape = Shapes.FlowChart_Process;
        /// n1.IsLabelEditable = true;
        /// n1.Label = "Alarm Rings";
        /// n1.Level = 2;
        /// n1.OffsetX = 150;
        /// n1.OffsetY = 125;
        /// n1.Width = 150;
        /// n1.Height = 75;
        /// Model.Nodes.Add(n1);
        /// //Creating a connection.
        /// LineConnector o2 = new LineConnector();
        /// o2.ConnectorType = ConnectorType.Straight;
        /// o2.TailNode = n1;
        /// o2.HeadNode = n;
        /// Model.Connections.Add(o2);
        /// }
        /// }
        /// }
        /// </code>
        /// </example>
        public IShape HeadNode
        {
            get
            {
                return (IShape)GetValue(HeadNodeProperty);
            }

            set
            {
                SetValue(HeadNodeProperty, value);
                this.OnPropertyChanged("HeadNode");
                this.UpdateConnectorPathGeometry();
            }
        }




        public LabelOrientation LabelOrientation
        {
            get
            {
                return (LabelOrientation)GetValue(LabelOrientationProperty);
            }
            set
            {
                SetValue(LabelOrientationProperty, value);
            }
        }


        public LabelOrientation LabelTemplateOrientation
        {
            get
            {
                return (LabelOrientation)GetValue(LabelTemplateOrientationProperty);
            }
            set
            {
                SetValue(LabelTemplateOrientationProperty, value);
            }
        }




        /// <summary>
        /// Gets or sets the head node reference no.
        /// </summary>
        /// <value>The head node reference no.</value>
        /// <remarks>
        /// Used for serialization purpose.
        /// </remarks>
        public int HeadNodeReferenceNo
        {
            get { return this.hid; }
            set { this.hid = value; }
        }

        /// <summary>
        /// Gets or sets the head port reference no.
        /// </summary>
        /// <value>The head port reference no.</value>
        /// <remarks>
        /// Used for serialization purpose.
        /// </remarks>
        public int HeadPortReferenceNo
        {
            get { return this.hpid; }
            set { this.hpid = value; }
        }

        /// <summary>
        /// Gets or sets a unique identifier for the connector.
        /// </summary>
        /// <value>
        /// Type: <see cref="Guid"/>
        /// Unique ID for the connector.
        /// </value>
        public Guid ID { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the Layout is directed or not.
        /// </summary>
        /// <value>
        /// Type: <see cref="Boolean"/>
        /// True if the layout is directed, false otherwise.
        /// </value>
        public bool IsDirected
        {
            get { return this.isDirected; }
            set { this.isDirected = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the connector is grouped.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is grouped; otherwise, <c>false</c>.
        /// </value>
        public bool IsGrouped
        {
            get { return (bool)GetValue(IsGroupedProperty); }
            set { SetValue(IsGroupedProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is label editable.
        /// </summary>
        /// <value>
        /// Type: <see cref="bool"/>
        /// True, if it can be edited, false otherwise.
        /// </value>
        /// <example>
        /// <para/>This example shows how to set IsLabelEditable in C#.
        /// <code language="C#">
        /// using Syncfusion.Windows.Diagram;
        ///namespace SilverlightApplication1
        /// {
        /// public partial class MainPage : UserControl
        /// {
        /// public DiagramControl Control;
        /// public DiagramModel Model;
        /// public DiagramView View;
        /// public MainPage()
        /// {
        /// InitializeComponent ();
        /// Control = new DiagramControl ();
        /// Model = new DiagramModel ();
        /// View = new DiagramView ();
        /// Control.View = View;
        /// Control.Model = Model;
        /// View.Bounds = new Thickness(0, 0, 1000, 1000);
        /// Node n = new Node(Guid.NewGuid(), "Start");
        /// n.Shape = Shapes.FlowChart_Start;
        /// n.OffsetX = 150;
        /// n.OffsetY = 25;
        /// n.Width = 150;
        /// n.Height = 75;
        /// Model.Nodes.Add(n);
        /// Node n1 = new Node(Guid.NewGuid(), "Decision1");
        /// n1.Shape = Shapes.FlowChart_Process;
        /// n1.Label = "Alarm Rings";
        /// n1.OffsetX = 150;
        /// n1.OffsetY = 125;
        /// n1.Width = 150;
        /// n1.Height = 75;
        /// Model.Nodes.Add(n1);
        /// LineConnector connObject = new LineConnector();
        /// connObject.ConnectorType = ConnectorType.Straight;
        /// connObject.TailNode = n1;
        /// connObject.HeadNode = n;
        /// connObject.ConnectorType = ConnectorType.Orthogonal;
        /// connObject.LabelVisibility = Visibility.Visible;
        /// connObject.IsLabelEitable = true;
        /// Model.Connections.Add(connObject);
        /// }
        /// }
        /// }
        /// </code>
        /// </example>
        /// <remarks>
        /// Default Value is true. When this is false, HitTest is also set to false.
        /// When set to true, clicking on the label will make the editable textbox visible.
        /// Enter the new label and press ENTER to apply the changed label,
        /// or press ESC to ignore the new label and revert back to the old one.
        /// </remarks>
        public bool IsLabelEditable
        {
            get { return (bool)GetValue(IsLabelEditableProperty); }
            set { SetValue(IsLabelEditableProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the connector has been selected or not.
        /// </summary>
        /// <value>
        /// Type: <see cref="Boolean"/>
        /// True if the connector is selected, false otherwise.
        /// </value>
        /// <example>
        /// <para/>This example shows how to set  LabelTemplate   in C#.
        /// <code language="C#">
        /// using Syncfusion.Windows.Diagram;
        ///namespace SilverlightApplication1
        /// {
        /// public partial class MainPage : UserControl
        /// {
        /// public DiagramControl Control;
        /// public DiagramModel Model;
        /// public DiagramView View;
        /// public MainPage()
        /// {
        /// InitializeComponent ();
        /// Control = new DiagramControl ();
        /// Model = new DiagramModel ();
        /// View = new DiagramView ();
        /// Control.View = View;
        /// Control.Model = Model;
        /// View.Bounds = new Thickness(0, 0, 1000, 1000);
        /// Node n = new Node(Guid.NewGuid(), "Start");
        /// n.Shape = Shapes.FlowChart_Start;
        /// n.OffsetX = 150;
        /// n.OffsetY = 25;
        /// n.Width = 150;
        /// n.Height = 75;
        /// Model.Nodes.Add(n);
        /// Node n1 = new Node(Guid.NewGuid(), "Decision1");
        /// n1.Shape = Shapes.FlowChart_Process;
        /// n1.Label = "Alarm Rings";
        /// n1.OffsetX = 150;
        /// n1.OffsetY = 125;
        /// n1.Width = 150;
        /// n1.Height = 75;
        /// Model.Nodes.Add(n1);
        /// LineConnector connObject = new LineConnector();
        /// connObject.ConnectorType = ConnectorType.Straight;
        /// connObject.TailNode = n1;
        /// connObject.HeadNode = n;
        /// connObject.ConnectorType = ConnectorType.Orthogonal;
        /// connObject.IsSelected = true;
        /// Model.Connections.Add(connObject);
        /// }
        /// }
        /// }
        /// </code>
        /// </example>
        public bool IsSelected
        {
            get
            {
                return this.isSelected;
            }

            set
            {
                if (this.isSelected != value)
                {
                    this.isSelected = value;
                    this.OnPropertyChanged("IsSelected");
                }
            }
        }

        /// <summary>
        /// Gets or sets the Label for the connector.
        /// </summary>
        /// <value>
        /// Type: <see cref="object"/>
        /// Label for the connector.
        /// </value>
        /// <example>
        /// <para/>This example shows how to set Label in C#.
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
        ///        Node n = new Node(Guid.NewGuid(), "Start");
        ///        n.Shape = Shapes.FlowChart_Start;
        ///        n.OffsetX = 150;
        ///        n.OffsetY = 25;
        ///        n.Width = 150;
        ///        n.Height = 75;
        ///        Model.Nodes.Add(n);
        ///         Node n1 = new Node(Guid.NewGuid(), "Decision1");
        ///         n1.Shape = Shapes.FlowChart_Process;
        ///         n1.Label = "Alarm Rings";
        ///         n1.OffsetX = 150;
        ///         n1.OffsetY = 125;
        ///         n1.Width = 150;
        ///         n1.Height = 75;
        ///         Model.Nodes.Add(n1);
        ///         LineConnector connObject = new LineConnector();
        ///         connObject.ConnectorType = ConnectorType.Straight;
        ///         connObject.TailNode = n1;
        ///         connObject.HeadNode = n;
        ///         connObject.ConnectorType = ConnectorType.Orthogonal;
        ///         connObject.Label="Syncfusion";
        ///         Model.Connections.Add(connObject);
        ///    }
        ///    }
        ///    }
        /// </code>
        /// </example>
        public string Label
        {
            get
            {
                return (string)GetValue(LabelProperty);
            }

            set
            {
                SetValue(LabelProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the label background.
        /// </summary>
        /// <value>The label background. Default value is White</value>
        /// <example>
        /// <para/>This example shows how to set LabelBackground in C#.
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
        ///        Node n = new Node(Guid.NewGuid(), "Start");
        ///        n.Shape = Shapes.FlowChart_Start;
        ///        n.OffsetX = 150;
        ///        n.OffsetY = 25;
        ///        n.Width = 150;
        ///        n.Height = 75;
        ///        Model.Nodes.Add(n);
        ///         Node n1 = new Node(Guid.NewGuid(), "Decision1");
        ///         n1.Shape = Shapes.FlowChart_Process;
        ///         n1.Label = "Alarm Rings";
        ///         n1.OffsetX = 150;
        ///         n1.OffsetY = 125;
        ///         n1.Width = 150;
        ///         n1.Height = 75;
        ///         Model.Nodes.Add(n1);
        ///         LineConnector connObject = new LineConnector();
        ///         connObject.ConnectorType = ConnectorType.Straight;
        ///         connObject.TailNode = n1;
        ///         connObject.HeadNode = n;
        ///         connObject.ConnectorType = ConnectorType.Orthogonal;
        ///         connObject.LabelBackground=Brushes.Beige;
        ///         Model.Connections.Add(connObject);
        ///    }
        ///    }
        ///    }
        /// </code>
        /// </example>
        public Brush LabelBackground
        {
            get
            {
                return (Brush)GetValue(LabelBackgroundProperty);
            }

            set
            {
                SetValue(LabelBackgroundProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the label font family.
        /// </summary>
        /// <value>Default value is Arial.</value>
        /// <example>
        /// <code language="C#">
        /// using Syncfusion.Windows.Diagram;
        ///namespace SilverlightApplication1
        /// {
        /// public partial class MainPage : UserControl
        /// {
        /// public DiagramControl Control;
        /// public DiagramModel Model;
        /// public DiagramView View;
        /// public MainPage()
        /// {
        /// InitializeComponent ();
        /// Control = new DiagramControl ();
        /// Model = new DiagramModel ();
        /// View = new DiagramView ();
        /// Control.View = View;
        /// Control.Model = Model;
        /// View.Bounds = new Thickness(0, 0, 1000, 1000);
        /// Node n = new Node(Guid.NewGuid(), "Start");
        /// n.Shape = Shapes.FlowChart_Start;
        /// n.OffsetX = 150;
        /// n.OffsetY = 25;
        /// n.Width = 150;
        /// n.Height = 75;
        /// Model.Nodes.Add(n);
        /// Node n1 = new Node(Guid.NewGuid(), "Decision1");
        /// n1.Shape = Shapes.FlowChart_Process;
        /// n1.Label = "Alarm Rings";
        /// n1.OffsetX = 150;
        /// n1.OffsetY = 125;
        /// n1.Width = 150;
        /// n1.Height = 75;
        /// Model.Nodes.Add(n1);
        /// LineConnector connObject = new LineConnector();
        /// connObject.ConnectorType = ConnectorType.Straight;
        /// connObject.TailNode = n1;
        /// connObject.HeadNode = n;
        /// connObject.ConnectorType = ConnectorType.Orthogonal;
        /// connObject.LabelFontFamily = new FontFamily("Verdana");
        /// Model.Connections.Add(connObject);
        /// }
        /// }
        /// }
        /// </code>
        /// </example>
        public FontFamily LabelFontFamily
        {
            get
            {
                return (FontFamily)GetValue(LabelFontFamilyProperty);
            }

            set
            {
                SetValue(LabelFontFamilyProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the label font size.
        /// </summary>
        /// <value>Default value is 11d.</value>
        /// <example>
        /// <code language="C#">
        /// using Syncfusion.Windows.Diagram;
        ///namespace SilverlightApplication1
        /// {
        /// public partial class MainPage : UserControl
        /// {
        /// public DiagramControl Control;
        /// public DiagramModel Model;
        /// public DiagramView View;
        /// public MainPage()
        /// {
        /// InitializeComponent ();
        /// Control = new DiagramControl ();
        /// Model = new DiagramModel ();
        /// View = new DiagramView ();
        /// Control.View = View;
        /// Control.Model = Model;
        /// View.Bounds = new Thickness(0, 0, 1000, 1000);
        /// Node n = new Node(Guid.NewGuid(), "Start");
        /// n.Shape = Shapes.FlowChart_Start;
        /// n.OffsetX = 150;
        /// n.OffsetY = 25;
        /// n.Width = 150;
        /// n.Height = 75;
        /// Model.Nodes.Add(n);
        /// Node n1 = new Node(Guid.NewGuid(), "Decision1");
        /// n1.Shape = Shapes.FlowChart_Process;
        /// n1.Label = "Alarm Rings";
        /// n1.OffsetX = 150;
        /// n1.OffsetY = 125;
        /// n1.Width = 150;
        /// n1.Height = 75;
        /// Model.Nodes.Add(n1);
        /// LineConnector connObject = new LineConnector();
        /// connObject.ConnectorType = ConnectorType.Straight;
        /// connObject.TailNode = n1;
        /// connObject.HeadNode = n;
        /// connObject.ConnectorType = ConnectorType.Orthogonal;
        /// connObject.LabelFontSize = 14;
        /// Model.Connections.Add(connObject);
        /// }
        /// }
        /// }
        /// </code>
        /// </example>
        public double LabelFontSize
        {
            get
            {
                return (double)GetValue(LabelFontSizeProperty);
            }

            set
            {
                SetValue(LabelFontSizeProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the label font style.
        /// </summary>
        /// <value>Default value is Normal.</value>
        /// <example>
        /// <code language="C#">
        /// using Syncfusion.Windows.Diagram;
        ///namespace SilverlightApplication1
        /// {
        /// public partial class MainPage : UserControl
        /// {
        /// public DiagramControl Control;
        /// public DiagramModel Model;
        /// public DiagramView View;
        /// public MainPage()
        /// {
        /// InitializeComponent ();
        /// Control = new DiagramControl ();
        /// Model = new DiagramModel ();
        /// View = new DiagramView ();
        /// Control.View = View;
        /// Control.Model = Model;
        /// View.Bounds = new Thickness(0, 0, 1000, 1000);
        /// Node n = new Node(Guid.NewGuid(), "Start");
        /// n.Shape = Shapes.FlowChart_Start;
        /// n.OffsetX = 150;
        /// n.OffsetY = 25;
        /// n.Width = 150;
        /// n.Height = 75;
        /// Model.Nodes.Add(n);
        /// Node n1 = new Node(Guid.NewGuid(), "Decision1");
        /// n1.Shape = Shapes.FlowChart_Process;
        /// n1.Label = "Alarm Rings";
        /// n1.OffsetX = 150;
        /// n1.OffsetY = 125;
        /// n1.Width = 150;
        /// n1.Height = 75;
        /// Model.Nodes.Add(n1);
        /// LineConnector connObject = new LineConnector();
        /// connObject.ConnectorType = ConnectorType.Straight;
        /// connObject.TailNode = n1;
        /// connObject.HeadNode = n;
        /// connObject.ConnectorType = ConnectorType.Orthogonal;
        /// connObject.LabelFontStyle = FontStyles.Italic;
        /// Model.Connections.Add(connObject);
        /// }
        /// }
        /// }
        /// </code>
        /// </example>
        public FontStyle LabelFontStyle
        {
            get
            {
                return (FontStyle)GetValue(LabelFontStyleProperty);
            }

            set
            {
                SetValue(LabelFontStyleProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the label font weight.
        /// </summary>
        /// <value>Default value is SemiBold.</value>
        /// <example>
        /// <code language="C#">
        /// using Syncfusion.Windows.Diagram;
        ///namespace SilverlightApplication1
        /// {
        /// public partial class MainPage : UserControl
        /// {
        /// public DiagramControl Control;
        /// public DiagramModel Model;
        /// public DiagramView View;
        /// public MainPage()
        /// {
        /// InitializeComponent ();
        /// Control = new DiagramControl ();
        /// Model = new DiagramModel ();
        /// View = new DiagramView ();
        /// Control.View = View;
        /// Control.Model = Model;
        /// View.Bounds = new Thickness(0, 0, 1000, 1000);
        /// Node n = new Node(Guid.NewGuid(), "Start");
        /// n.Shape = Shapes.FlowChart_Start;
        /// n.OffsetX = 150;
        /// n.OffsetY = 25;
        /// n.Width = 150;
        /// n.Height = 75;
        /// Model.Nodes.Add(n);
        /// Node n1 = new Node(Guid.NewGuid(), "Decision1");
        /// n1.Shape = Shapes.FlowChart_Process;
        /// n1.Label = "Alarm Rings";
        /// n1.OffsetX = 150;
        /// n1.OffsetY = 125;
        /// n1.Width = 150;
        /// n1.Height = 75;
        /// Model.Nodes.Add(n1);
        /// LineConnector connObject = new LineConnector();
        /// connObject.ConnectorType = ConnectorType.Straight;
        /// connObject.TailNode = n1;
        /// connObject.HeadNode = n;
        /// connObject.ConnectorType = ConnectorType.Orthogonal;
        /// connObject.LabelFontWeight = FontWeights.Bold;
        /// Model.Connections.Add(connObject);
        /// }
        /// }
        /// }
        /// </code>
        /// </example>
        public FontWeight LabelFontWeight
        {
            get
            {
                return (FontWeight)GetValue(LabelFontWeightProperty);
            }

            set
            {
                SetValue(LabelFontWeightProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the Label Foreground.
        /// </summary>
        /// <value>The label foreground. Default value is Black.</value>
        /// <example>
        /// <para/>This example shows how to set LabelForeground in C#.
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
        ///        Node n = new Node(Guid.NewGuid(), "Start");
        ///        n.Shape = Shapes.FlowChart_Start;
        ///        n.OffsetX = 150;
        ///        n.OffsetY = 25;
        ///        n.Width = 150;
        ///        n.Height = 75;
        ///        Model.Nodes.Add(n);
        ///         Node n1 = new Node(Guid.NewGuid(), "Decision1");
        ///         n1.Shape = Shapes.FlowChart_Process;
        ///         n1.Label = "Alarm Rings";
        ///         n1.OffsetX = 150;
        ///         n1.OffsetY = 125;
        ///         n1.Width = 150;
        ///         n1.Height = 75;
        ///         Model.Nodes.Add(n1);
        ///         LineConnector connObject = new LineConnector();
        ///         connObject.ConnectorType = ConnectorType.Straight;
        ///         connObject.TailNode = n1;
        ///         connObject.HeadNode = n;
        ///         connObject.ConnectorType = ConnectorType.Orthogonal;
        ///         connObject.LabelForeground=Brushes.Beige;
        ///         Model.Connections.Add(connObject);
        ///    }
        ///    }
        ///    }
        /// </code>
        /// </example>
        public Brush LabelForeground
        {
            get
            {
                return (Brush)GetValue(LabelForegroundProperty);
            }

            set
            {
                SetValue(LabelForegroundProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets  the LabelTemplate for the connector.This is a dependency property.
        /// </summary>
        /// <value>
        /// Type: <see cref="ControlTemplate"/>
        /// LabelTemplate for the connector.
        /// </value>
        /// <example>
        /// <para/>This example shows how to set  LabelTemplate   in C#.
        /// <code language="C#">
        /// using Syncfusion.Windows.Diagram;
        ///namespace SilverlightApplication1
        /// {
        /// public partial class MainPage : UserControl
        /// {
        /// public DiagramControl Control;
        /// public DiagramModel Model;
        /// public DiagramView View;
        /// public MainPage()
        /// {
        /// InitializeComponent ();
        /// Control = new DiagramControl ();
        /// Model = new DiagramModel ();
        /// View = new DiagramView ();
        /// Control.View = View;
        /// Control.Model = Model;
        /// View.Bounds = new Thickness(0, 0, 1000, 1000);
        /// Node n = new Node(Guid.NewGuid(), "Start");
        /// n.Shape = Shapes.FlowChart_Start;
        /// n.OffsetX = 150;
        /// n.OffsetY = 25;
        /// n.Width = 150;
        /// n.Height = 75;
        /// Model.Nodes.Add(n);
        /// Node n1 = new Node(Guid.NewGuid(), "Decision1");
        /// n1.Shape = Shapes.FlowChart_Process;
        /// n1.Label = "Alarm Rings";
        /// n1.OffsetX = 150;
        /// n1.OffsetY = 125;
        /// n1.Width = 150;
        /// n1.Height = 75;
        /// Model.Nodes.Add(n1);
        /// LineConnector connObject = new LineConnector();
        /// connObject.ConnectorType = ConnectorType.Straight;
        /// connObject.TailNode = n1;
        /// connObject.HeadNode = n;
        /// connObject.ConnectorType = ConnectorType.Orthogonal;
        /// connObject.LabelTemplate = (ControlTemplate)FindResource( "LabelCustomTemplate" );
        /// Model.Connections.Add(connObject);
        /// }
        /// }
        /// }
        /// </code>
        /// <para/>This example shows how to write a  LabelTemplate in XAML.
        /// <code language="XAML">
        /// &lt;ControlTemplate x:Key="LabelCustomTemplate"&gt;
        /// &lt;StackPanel Orientation="Horizontal"&gt;
        /// &lt;Image Source="text.png" Width="20" Height="20"/&gt;
        /// &lt;TextBlock Text="Hello"/&gt;
        /// &lt;/StackPanel&gt;
        /// &lt;/ControlTemplate&gt;
        /// </code>
        /// </example>
        /// <seealso cref="ControlTemplate"/>
        public DataTemplate LabelTemplate
        {
            get
            {
                return (DataTemplate)GetValue(LabelTemplateProperty);
            }

            set
            {
                SetValue(LabelTemplateProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the label template horizontal alignment.
        /// </summary>
        /// <value>
        /// Type: <see cref="HorizontalAlignment"/>
        /// Enum specifying the alignment position.
        /// </value>
        /// <remarks>Default HorizontalAlignment is at the Center.</remarks>
        /// <example>
        /// <para/>This example shows how to set LabelTemplateHorizontalAlignment in C#.
        /// <code language="C#">
        /// using Syncfusion.Windows.Diagram;
        ///namespace SilverlightApplication1
        /// {
        /// public partial class MainPage : UserControl
        /// {
        /// public DiagramControl Control;
        /// public DiagramModel Model;
        /// public DiagramView View;
        /// public MainPage()
        /// {
        /// InitializeComponent ();
        /// Control = new DiagramControl ();
        /// Model = new DiagramModel ();
        /// View = new DiagramView ();
        /// Control.View = View;
        /// Control.Model = Model;
        /// View.Bounds = new Thickness(0, 0, 1000, 1000);
        /// Node n = new Node(Guid.NewGuid(), "Start");
        /// n.Shape = Shapes.FlowChart_Start;
        /// n.OffsetX = 150;
        /// n.OffsetY = 25;
        /// n.Width = 150;
        /// n.Height = 75;
        /// Model.Nodes.Add(n);
        /// Node n1 = new Node(Guid.NewGuid(), "Decision1");
        /// n1.Shape = Shapes.FlowChart_Process;
        /// n1.Label = "Alarm Rings";
        /// n1.OffsetX = 150;
        /// n1.OffsetY = 125;
        /// n1.Width = 150;
        /// n1.Height = 75;
        /// Model.Nodes.Add(n1);
        /// LineConnector connObject = new LineConnector();
        /// connObject.ConnectorType = ConnectorType.Straight;
        /// connObject.TailNode = n1;
        /// connObject.HeadNode = n;
        /// connObject.ConnectorType = ConnectorType.Orthogonal;
        /// connObject.Label="Syncfusion";
        /// connObject.LabelTemplateHorizontalAlignment= HorizontalAlignment.Left;
        /// Model.Connections.Add(connObject);
        /// }
        /// }
        /// }
        /// </code>
        /// </example>
        public HorizontalAlignment LabelTemplateHorizontalAlignment
        {
            get
            {
                return (HorizontalAlignment)GetValue(LabelTemplateHorizontalAlignmentProperty);
            }

            set
            {
                SetValue(LabelTemplateHorizontalAlignmentProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the label template vertical alignment.
        /// </summary>
        /// <value>
        /// Type: <see cref="VerticalAlignment"/>
        /// Enum specifying the alignment position.
        /// </value>
        /// <remarks>Default VerticalAlignment is at the Top.</remarks>
        /// <example>
        /// <para/>This example shows how to set LabelTemplateVerticalAlignment in C#.
        /// <code language="C#">
        /// using Syncfusion.Windows.Diagram;
        ///namespace SilverlightApplication1
        /// {
        /// public partial class MainPage : UserControl
        /// {
        /// public DiagramControl Control;
        /// public DiagramModel Model;
        /// public DiagramView View;
        /// public MainPage()
        /// {
        /// InitializeComponent ();
        /// Control = new DiagramControl ();
        /// Model = new DiagramModel ();
        /// View = new DiagramView ();
        /// Control.View = View;
        /// Control.Model = Model;
        /// View.Bounds = new Thickness(0, 0, 1000, 1000);
        /// Node n = new Node(Guid.NewGuid(), "Start");
        /// n.Shape = Shapes.FlowChart_Start;
        /// n.OffsetX = 150;
        /// n.OffsetY = 25;
        /// n.Width = 150;
        /// n.Height = 75;
        /// Model.Nodes.Add(n);
        /// Node n1 = new Node(Guid.NewGuid(), "Decision1");
        /// n1.Shape = Shapes.FlowChart_Process;
        /// n1.Label = "Alarm Rings";
        /// n1.OffsetX = 150;
        /// n1.OffsetY = 125;
        /// n1.Width = 150;
        /// n1.Height = 75;
        /// Model.Nodes.Add(n1);
        /// LineConnector connObject = new LineConnector();
        /// connObject.ConnectorType = ConnectorType.Straight;
        /// connObject.TailNode = n1;
        /// connObject.HeadNode = n;
        /// connObject.ConnectorType = ConnectorType.Orthogonal;
        /// connObject.LabelTemplateVerticalAlignment= VerticalAlignment.Left;
        /// Model.Connections.Add(connObject);
        /// }
        /// }
        /// }
        /// </code>
        /// </example>
        public VerticalAlignment LabelTemplateVerticalAlignment
        {
            get
            {
                return (VerticalAlignment)GetValue(LabelTemplateVerticalAlignmentProperty);
            }

            set
            {
                SetValue(LabelTemplateVerticalAlignmentProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the label text alignment.
        /// </summary>
        /// <value>Default value is Center.</value>
        /// <example>
        /// <code language="C#">
        /// using Syncfusion.Windows.Diagram;
        ///namespace SilverlightApplication1
        /// {
        /// public partial class MainPage : UserControl
        /// {
        /// public DiagramControl Control;
        /// public DiagramModel Model;
        /// public DiagramView View;
        /// public MainPage()
        /// {
        /// InitializeComponent ();
        /// Control = new DiagramControl ();
        /// Model = new DiagramModel ();
        /// View = new DiagramView ();
        /// Control.View = View;
        /// Control.Model = Model;
        /// View.Bounds = new Thickness(0, 0, 1000, 1000);
        /// Node n = new Node(Guid.NewGuid(), "Start");
        /// n.Shape = Shapes.FlowChart_Start;
        /// n.OffsetX = 150;
        /// n.OffsetY = 25;
        /// n.Width = 150;
        /// n.Height = 75;
        /// Model.Nodes.Add(n);
        /// Node n1 = new Node(Guid.NewGuid(), "Decision1");
        /// n1.Shape = Shapes.FlowChart_Process;
        /// n1.Label = "Alarm Rings";
        /// n1.OffsetX = 150;
        /// n1.OffsetY = 125;
        /// n1.Width = 150;
        /// n1.Height = 75;
        /// Model.Nodes.Add(n1);
        /// LineConnector connObject = new LineConnector();
        /// connObject.ConnectorType = ConnectorType.Straight;
        /// connObject.TailNode = n1;
        /// connObject.HeadNode = n;
        /// connObject.ConnectorType = ConnectorType.Orthogonal;
        /// connObject.LabelTextAlignment = TextAlignment.Left;
        /// Model.Connections.Add(connObject);
        /// }
        /// }
        /// }
        /// </code>
        /// </example>
        public TextAlignment LabelTextAlignment
        {
            get
            {
                return (TextAlignment)GetValue(LabelTextAlignmentProperty);
            }

            set
            {
                SetValue(LabelTextAlignmentProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the label text wrapping.
        /// </summary>
        /// <value>Default value is NoWrap.</value>
        /// <example>
        /// <code language="C#">
        /// using Syncfusion.Windows.Diagram;
        ///namespace SilverlightApplication1
        /// {
        /// public partial class MainPage : UserControl
        /// {
        /// public DiagramControl Control;
        /// public DiagramModel Model;
        /// public DiagramView View;
        /// public MainPage()
        /// {
        /// InitializeComponent ();
        /// Control = new DiagramControl ();
        /// Model = new DiagramModel ();
        /// View = new DiagramView ();
        /// Control.View = View;
        /// Control.Model = Model;
        /// View.Bounds = new Thickness(0, 0, 1000, 1000);
        /// Node n = new Node(Guid.NewGuid(), "Start");
        /// n.Shape = Shapes.FlowChart_Start;
        /// n.OffsetX = 150;
        /// n.OffsetY = 25;
        /// n.Width = 150;
        /// n.Height = 75;
        /// Model.Nodes.Add(n);
        /// Node n1 = new Node(Guid.NewGuid(), "Decision1");
        /// n1.Shape = Shapes.FlowChart_Process;
        /// n1.Label = "Alarm Rings";
        /// n1.OffsetX = 150;
        /// n1.OffsetY = 125;
        /// n1.Width = 150;
        /// n1.Height = 75;
        /// Model.Nodes.Add(n1);
        /// LineConnector connObject = new LineConnector();
        /// connObject.ConnectorType = ConnectorType.Straight;
        /// connObject.TailNode = n1;
        /// connObject.HeadNode = n;
        /// connObject.ConnectorType = ConnectorType.Orthogonal;
        /// connObject.LabelTextWrapping = TextWrapping.Wrap;
        /// Model.Connections.Add(connObject);
        /// }
        /// }
        /// }
        /// </code>
        /// </example>
        public TextWrapping LabelTextWrapping
        {
            get
            {
                return (TextWrapping)GetValue(LabelTextWrappingProperty);
            }

            set
            {
                SetValue(LabelTextWrappingProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the label vertical alignment.
        /// </summary>
        /// <value>
        /// Type: <see cref="VerticalAlignment"/>
        /// Enum specifying the alignment position.
        /// </value>
        /// <remarks>Default VerticalAlignment is at the Top.</remarks>
        /// <example>
        /// <para/>This example shows how to set LabelVerticalAlignment in C#.
        /// <code language="C#">
        /// using Syncfusion.Windows.Diagram;
        ///namespace SilverlightApplication1
        /// {
        /// public partial class MainPage : UserControl
        /// {
        /// public DiagramControl Control;
        /// public DiagramModel Model;
        /// public DiagramView View;
        /// public MainPage()
        /// {
        /// InitializeComponent ();
        /// Control = new DiagramControl ();
        /// Model = new DiagramModel ();
        /// View = new DiagramView ();
        /// Control.View = View;
        /// Control.Model = Model;
        /// View.Bounds = new Thickness(0, 0, 1000, 1000);
        /// Node n = new Node(Guid.NewGuid(), "Start");
        /// n.Shape = Shapes.FlowChart_Start;
        /// n.OffsetX = 150;
        /// n.OffsetY = 25;
        /// n.Width = 150;
        /// n.Height = 75;
        /// Model.Nodes.Add(n);
        /// Node n1 = new Node(Guid.NewGuid(), "Decision1");
        /// n1.Shape = Shapes.FlowChart_Process;
        /// n1.Label = "Alarm Rings";
        /// n1.OffsetX = 150;
        /// n1.OffsetY = 125;
        /// n1.Width = 150;
        /// n1.Height = 75;
        /// Model.Nodes.Add(n1);
        /// LineConnector connObject = new LineConnector();
        /// connObject.ConnectorType = ConnectorType.Straight;
        /// connObject.TailNode = n1;
        /// connObject.HeadNode = n;
        /// connObject.ConnectorType = ConnectorType.Orthogonal;
        /// connObject.Label="Syncfusion";
        /// connObject.LabelVerticalAlignment= VerticalAlignment.Left;
        /// Model.Connections.Add(connObject);
        /// }
        /// }
        /// }
        /// </code>
        /// </example>
        public VerticalAlignment LabelVerticalAlignment
        {
            get
            {
                return (VerticalAlignment)GetValue(LabelVerticalAlignmentProperty);
            }

            set
            {
                SetValue(LabelVerticalAlignmentProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the label visibility.
        /// </summary>
        /// <value>
        /// Type: <see cref="Visibility"/>
        /// Enum specifying the visibility.
        /// </value>
        /// <example>
        /// <para/>This example shows how to set LabelVisibility in C#.
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
        ///        Node n = new Node(Guid.NewGuid(), "Start");
        ///        n.Shape = Shapes.FlowChart_Start;
        ///        n.OffsetX = 150;
        ///        n.OffsetY = 25;
        ///        n.Width = 150;
        ///        n.Height = 75;
        ///        Model.Nodes.Add(n);
        ///         Node n1 = new Node(Guid.NewGuid(), "Decision1");
        ///         n1.Shape = Shapes.FlowChart_Process;
        ///         n1.Label = "Alarm Rings";
        ///         n1.OffsetX = 150;
        ///         n1.OffsetY = 125;
        ///         n1.Width = 150;
        ///         n1.Height = 75;
        ///         Model.Nodes.Add(n1);
        ///         LineConnector connObject = new LineConnector();
        ///         connObject.ConnectorType = ConnectorType.Straight;
        ///         connObject.TailNode = n1;
        ///         connObject.HeadNode = n;
        ///         connObject.ConnectorType = ConnectorType.Orthogonal;
        ///         connObject.LabelVisibility = Visibility.Visible;
        ///         Model.Connections.Add(connObject);
        ///    }
        ///    }
        ///    }
        /// </code>
        /// </example>
        /// <remarks>
        /// By default label visibility is set to visible.
        /// </remarks>
        public Visibility LabelVisibility
        {
            get
            {
                return (Visibility)GetValue(LabelVisibilityProperty);
            }

            set
            {
                SetValue(LabelVisibilityProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the width of the label.
        /// </summary>
        /// <value>The width of the label. By default it is set to the line width.</value>
        /// <example>
        /// <code language="C#">
        /// using Syncfusion.Windows.Diagram;
        ///namespace SilverlightApplication1
        /// {
        /// public partial class MainPage : UserControl
        /// {
        /// public DiagramControl Control;
        /// public DiagramModel Model;
        /// public DiagramView View;
        /// public MainPage()
        /// {
        /// InitializeComponent ();
        /// Control = new DiagramControl ();
        /// Model = new DiagramModel ();
        /// View = new DiagramView ();
        /// Control.View = View;
        /// Control.Model = Model;
        /// View.Bounds = new Thickness(0, 0, 1000, 1000);
        /// Node n = new Node(Guid.NewGuid(), "Start");
        /// n.Shape = Shapes.FlowChart_Start;
        /// n.OffsetX = 150;
        /// n.OffsetY = 25;
        /// n.Width = 150;
        /// n.Height = 75;
        /// Model.Nodes.Add(n);
        /// Node n1 = new Node(Guid.NewGuid(), "Decision1");
        /// n1.Shape = Shapes.FlowChart_Process;
        /// n1.Label = "Alarm Rings";
        /// n1.OffsetX = 150;
        /// n1.OffsetY = 125;
        /// n1.Width = 150;
        /// n1.Height = 75;
        /// Model.Nodes.Add(n1);
        /// LineConnector connObject = new LineConnector();
        /// connObject.ConnectorType = ConnectorType.Straight;
        /// connObject.TailNode = n1;
        /// connObject.HeadNode = n;
        /// connObject.ConnectorType = ConnectorType.Orthogonal;
        /// connObject.LabelWidth=50;
        /// Model.Connections.Add(connObject);
        /// }
        /// }
        /// }
        /// </code>
        /// </example>
        public double LabelWidth
        {
            get
            {
                return (double)GetValue(LabelWidthProperty);
            }

            set
            {
                SetValue(LabelWidthProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the line style to be used for the connector.
        /// </summary>
        /// <value>
        /// Type: <see cref="LineStyle"/>
        /// LineStyle for the connector.
        /// </value>
        /// <remarks>
        /// The line connectors can be customized by using the various LineStyle properties like Fill, Stroke, StrokeThickness, StrokeStartLineCap, StrokeEndLineCap, StrokeLineJoin .
        /// </remarks>
        /// <example>
        /// <para/>This example shows how to set LineStyle in C#.
        /// <code language="C#">
        /// using Syncfusion.Windows.Diagram;
        ///namespace SilverlightApplication1
        /// {
        /// public partial class MainPage : UserControl
        /// {
        /// public DiagramControl Control;
        /// public DiagramModel Model;
        /// public DiagramView View;
        /// public MainPage()
        /// {
        /// InitializeComponent ();
        /// Control = new DiagramControl ();
        /// Model = new DiagramModel ();
        /// View = new DiagramView ();
        /// Control.View = View;
        /// Control.Model = Model;
        /// View.Bounds = new Thickness(0, 0, 1000, 1000);
        /// Node n = new Node(Guid.NewGuid(), "Start");
        /// n.Shape = Shapes.FlowChart_Start;
        /// n.OffsetX = 150;
        /// n.OffsetY = 25;
        /// n.Width = 150;
        /// n.Height = 75;
        /// Model.Nodes.Add(n);
        /// Node n1 = new Node(Guid.NewGuid(), "Decision1");
        /// n1.Shape = Shapes.FlowChart_Process;
        /// n1.Label = "Alarm Rings";
        /// n1.OffsetX = 150;
        /// n1.OffsetY = 125;
        /// n1.Width = 150;
        /// n1.Height = 75;
        /// Model.Nodes.Add(n1);
        /// LineConnector connObject = new LineConnector();
        /// connObject.ConnectorType = ConnectorType.Straight;
        /// connObject.TailNode = n1;
        /// connObject.HeadNode = n;
        /// connObject.ConnectorType = ConnectorType.Orthogonal;
        /// connObject.LineStyle.Fill = Brushes.Red;
        /// Model.Connections.Add(connObject);
        /// }
        /// }
        /// }
        /// </code>
        /// </example>
        /// <seealso cref="LineStyle"/>
        public LineStyle LineStyle
        {
            get
            {
                if (this.lineStyle != null)
                {
                    return this.lineStyle;
                }
                else
                {
                    this.lineStyle = new LineStyle(this);
                }

                return this.lineStyle;
            }

            set
            {
                this.lineStyle = value;
            }
        }

        /// <summary>
        /// Gets or sets the label horizontal alignment.
        /// </summary>
        /// <value>
        /// Type: <see cref="HorizontalAlignment"/>
        /// Enum specifying the alignment position.
        /// </value>
        /// <remarks>Default HorizontalAlignment is at the Center. This property will take effect only if the LabelWidth is set.</remarks>
        /// <example>
        /// <para/>This example shows how to set LabelHorizontalAlignment in C#.
        /// <code language="C#">
        /// using Syncfusion.Windows.Diagram;
        ///namespace SilverlightApplication1
        /// {
        /// public partial class MainPage : UserControl
        /// {
        /// public DiagramControl Control;
        /// public DiagramModel Model;
        /// public DiagramView View;
        /// public MainPage()
        /// {
        /// InitializeComponent ();
        /// Control = new DiagramControl ();
        /// Model = new DiagramModel ();
        /// View = new DiagramView ();
        /// Control.View = View;
        /// Control.Model = Model;
        /// View.Bounds = new Thickness(0, 0, 1000, 1000);
        /// Node n = new Node(Guid.NewGuid(), "Start");
        /// n.Shape = Shapes.FlowChart_Start;
        /// n.OffsetX = 150;
        /// n.OffsetY = 25;
        /// n.Width = 150;
        /// n.Height = 75;
        /// Model.Nodes.Add(n);
        /// Node n1 = new Node(Guid.NewGuid(), "Decision1");
        /// n1.Shape = Shapes.FlowChart_Process;
        /// n1.Label = "Alarm Rings";
        /// n1.OffsetX = 150;
        /// n1.OffsetY = 125;
        /// n1.Width = 150;
        /// n1.Height = 75;
        /// Model.Nodes.Add(n1);
        /// LineConnector connObject = new LineConnector();
        /// connObject.ConnectorType = ConnectorType.Straight;
        /// connObject.TailNode = n1;
        /// connObject.HeadNode = n;
        /// connObject.ConnectorType = ConnectorType.Orthogonal;
        /// connObject.Label="Syncfusion";
        /// connObject.LabelHorizontalAlignment= HorizontalAlignment.Left;
        /// Model.Connections.Add(connObject);
        /// }
        /// }
        /// }
        /// </code>
        /// </example>
        public HorizontalAlignment LabelHorizontalAlignment
        {
            get
            {
                return (HorizontalAlignment)GetValue(LabelHorizontalAlignmentProperty);
            }

            set
            {
                SetValue(LabelHorizontalAlignmentProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the new ZIndex value.
        /// </summary>
        /// <value>The new ZIndex value.</value>
        public int NewZIndex
        {
            get { return this.mnewindex; }
            set { this.mnewindex = value; }
        }

        /// <summary>
        /// Gets or sets the old ZIndex value.
        /// </summary>
        /// <value>The old ZIndex value.</value>
        public int OldZIndex
        {
            get { return this.moldindex; }
            set { this.moldindex = value; }
        }

        /// <summary>
        /// Gets or sets the parent ID.
        /// </summary>
        /// <value>The parent ID.</value>
        public Guid ParentID
        {
            get { return (Guid)GetValue(ParentIDProperty); }
            set { SetValue(ParentIDProperty, value); }
        }

        /// <summary>
        /// Gets or sets the reference number of the INodeGroup objects. Used for serialization purposes..
        /// </summary>
        /// <value>The reference no.</value>
        public int ReferenceNo
        {
            get { return this.no; }
            set { this.no = value; }
        }

        internal Point PxStartPointPosition
        {
            get
            {
                return MeasureUnitsConverter.ToPixels(StartPointPosition, MeasurementUnit);
            }
            set
            {
                if (this.dview != null && (this as LineConnector).linedragging)
                {
                    if (this.dview.SnapToHorizontalGrid)
                    {
                        value.X = Node.Round(value.X, dview.PxSnapOffsetX);
                    }
                    if (this.dview.SnapToVerticalGrid)
                    {
                        value.Y = Node.Round(value.Y, dview.PxSnapOffsetY);
                    }
                }
                StartPointPosition = MeasureUnitsConverter.FromPixels(value, MeasurementUnit);
            }
        }

        /// <summary>
        /// Gets or sets the start point position.
        /// </summary>
        /// <value>The start point position.</value>
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
        ///         o.StartPointPosition=new Point(100,100);
        ///         o.EndPointPosition=new Point(200,200);
        ///         Model.Connections.Add(o);
        ///    }
        ///    }
        ///    }
        /// </code>
        /// </example>
        public Point StartPointPosition
        {
            get { return this.mstartpointposition; }
            set
            {
                if (dview != null && StartPointPosition!= value)
                {
                    if (dview.m_IsCommandInProgress)
                    {
                        if (!dview.Undone && !dview.Redone)
                        {
                            dview.tUndoStack.Push(new LineOperation(LineOperations.Dragged, this as LineConnector));
                        }
                    }
                }
                this.mstartpointposition = value;
            }
        }

        /// <summary>
        /// Gets or sets the point where the tail decorator is to be positioned.
        /// </summary>
        /// <value>
        /// Type: <see cref="Point"/>
        /// The point of the tail decorator position.
        /// </value>
        public Point TailDecoratorPosition
        {
            get
            {
                return (Point)GetValue(TailDecoratorPositionProperty);
            }

            set
            {
                SetValue(TailDecoratorPositionProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the shape to be used as the tail decorator.
        /// </summary>
        /// <value>
        /// Type: <see cref="DecoratorShape"/>
        /// Enum specifying the shape of the tail decorator.
        /// </value>
        /// <example>
        /// <para/>This example shows how to set TailDecoratorShape in C#.
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
        ///        Node n = new Node(Guid.NewGuid(), "Start");
        ///        n.Shape = Shapes.FlowChart_Start;
        ///        n.OffsetX = 150;
        ///        n.OffsetY = 25;
        ///        n.Width = 150;
        ///        n.Height = 75;
        ///        Model.Nodes.Add(n);
        ///         Node n1 = new Node(Guid.NewGuid(), "Decision1");
        ///         n1.Shape = Shapes.FlowChart_Process;
        ///         n1.Label = "Alarm Rings";
        ///         n1.OffsetX = 150;
        ///         n1.OffsetY = 125;
        ///         n1.Width = 150;
        ///         n1.Height = 75;
        ///         Model.Nodes.Add(n1);
        ///         LineConnector connObject = new LineConnector();
        ///         connObject.ConnectorType = ConnectorType.Straight;
        ///         connObject.TailNode = n1;
        ///         connObject.HeadNode = n;
        ///         connObject.ConnectorType = ConnectorType.Orthogonal;
        ///         connObject.TailDecoratorShape = DecoratorShape.Arrow;
        ///         Model.Connections.Add(connObject);
        ///    }
        ///    }
        ///    }
        /// </code>
        /// </example>
        /// <remarks>
        /// Several shapes like None, Arrow, Diamond and Circle have been provided. Default shape is Arrow.
        /// </remarks>
        /// <seealso cref="DecoratorShape"/>
        public DecoratorShape TailDecoratorShape
        {
            get
            {
                return this.mtailDecoratorShape;
            }

            set
            {
                if (this.mtailDecoratorShape != value)
                {
                    this.mtailDecoratorShape = value;
                    if (!(this as LineConnector).IsOverlapped)
                    {
                        (this as LineConnector).InternalTailShape = value;
                    }

                    this.SetShape("Tail");
                    this.OnPropertyChanged("TailDecoratorShape");
                }
            }
        }

        /// <summary>
        /// Gets or sets the  style to be used for the tail decorator.
        /// </summary>
        /// <value>
        /// Type: <see cref="DecoratorStyle"/>
        /// TailDecoratorStyle for the connector.
        /// </value>
        /// <remarks>
        /// The decorator shapes can be customized by using the various DecoratorStyle properties like Fill, Stroke, StrokeThickness, StrokeStartLineCap, StrokeEndLineCap, StrokeLineJoin .
        /// </remarks>
        /// <example>
        /// <para/>This example shows how to set TailDecoratorStyle in C#.
        /// <code language="C#">
        /// using Syncfusion.Windows.Diagram;
        ///namespace SilverlightApplication1
        /// {
        /// public partial class MainPage : UserControl
        /// {
        /// public DiagramControl Control;
        /// public DiagramModel Model;
        /// public DiagramView View;
        /// public MainPage()
        /// {
        /// InitializeComponent ();
        /// Control = new DiagramControl ();
        /// Model = new DiagramModel ();
        /// View = new DiagramView ();
        /// Control.View = View;
        /// Control.Model = Model;
        /// View.Bounds = new Thickness(0, 0, 1000, 1000);
        /// Node n = new Node(Guid.NewGuid(), "Start");
        /// n.Shape = Shapes.FlowChart_Start;
        /// n.OffsetX = 150;
        /// n.OffsetY = 25;
        /// n.Width = 150;
        /// n.Height = 75;
        /// Model.Nodes.Add(n);
        /// Node n1 = new Node(Guid.NewGuid(), "Decision1");
        /// n1.Shape = Shapes.FlowChart_Process;
        /// n1.Label = "Alarm Rings";
        /// n1.OffsetX = 150;
        /// n1.OffsetY = 125;
        /// n1.Width = 150;
        /// n1.Height = 75;
        /// Model.Nodes.Add(n1);
        /// LineConnector connObject = new LineConnector();
        /// connObject.ConnectorType = ConnectorType.Straight;
        /// connObject.TailNode = n1;
        /// connObject.HeadNode = n;
        /// connObject.ConnectorType = ConnectorType.Orthogonal;
        /// connObject.LineStyle.Fill = Brushes.Red;
        /// connObject.TailDecoratorStyle.Fill = Brushes.Orange; 
        /// Model.Connections.Add(connObject);
        /// }
        /// }
        /// }
        /// </code>
        /// </example>
        /// <seealso cref="DecoratorStyle"/>
        public DecoratorStyle TailDecoratorStyle
        {
            get
            {
                if (this.mtailDecoratorStyle != null)
                {
                    return this.mtailDecoratorStyle;
                }
                else
                {
                    this.mtailDecoratorStyle = new DecoratorStyle();
                }

                return this.mtailDecoratorStyle;
            }

            set
            {
                this.mtailDecoratorStyle = value;
            }
        }

        /// <summary>
        /// Gets or sets the second, or target, node upon which this Edge is incident.
        /// </summary>
        /// <value>The tail node of the connection</value>
        /// <remarks>
        /// Every Node should have unique name.
        /// </remarks>
        /// <example>
        /// <code language="C#">
        /// using Syncfusion.Windows.Diagram;
        ///namespace SilverlightApplication1
        /// {
        /// public partial class MainPage : UserControl
        /// {
        /// public DiagramControl Control;
        /// public DiagramModel Model;
        /// public DiagramView View;
        /// public MainPage()
        /// {
        /// InitializeComponent ();
        /// Control = new DiagramControl ();
        /// Model = new DiagramModel ();
        /// View = new DiagramView ();
        /// Control.View = View;
        /// Control.Model = Model;
        /// View.Bounds = new Thickness(0, 0, 1000, 1000);
        /// //Creating node
        /// Node n = new Node(Guid.NewGuid(), "Start");
        /// n.Shape = Shapes.FlowChart_Start;
        /// n.IsLabelEditable = true;
        /// n.Label = "Start";
        /// n.Level = 1;
        /// n.OffsetX = 150;
        /// n.OffsetY = 25;
        /// n.Width = 150;
        /// n.Height = 75;
        /// n.ToolTip="Start Node";
        /// Model.Nodes.Add(n);
        /// Node n1 = new Node(Guid.NewGuid(), "Decision1");
        /// n1.Shape = Shapes.FlowChart_Process;
        /// n1.IsLabelEditable = true;
        /// n1.Label = "Alarm Rings";
        /// n1.Level = 2;
        /// n1.OffsetX = 150;
        /// n1.OffsetY = 125;
        /// n1.Width = 150;
        /// n1.Height = 75;
        /// Model.Nodes.Add(n1);
        /// //Creating a connection.
        /// LineConnector o2 = new LineConnector();
        /// o2.ConnectorType = ConnectorType.Straight;
        /// o2.TailNode = n1;
        /// o2.HeadNode = n;
        /// Model.Connections.Add(o2);
        /// }
        /// }
        /// }
        /// </code>
        /// </example>
        public IShape TailNode
        {
            get
            {
                return (IShape)GetValue(TailNodeProperty);
            }

            set
            {
                SetValue(TailNodeProperty, value);
                this.OnPropertyChanged("TailNode");
                this.UpdateConnectorPathGeometry();
            }
        }

        /// <summary>
        /// Gets or sets the tail node reference no.
        /// </summary>
        /// <value>The tail node reference no.</value>
        /// <remarks>
        /// Used for serialization purpose.
        /// </remarks>
        public int TailNodeReferenceNo
        {
            get { return this.tid; }
            set { this.tid = value; }
        }

        /// <summary>
        /// Gets or sets the tail port reference no.
        /// </summary>
        /// <value>The tail port reference no.</value>
        /// <remarks>
        /// Used for serialization purpose.
        /// </remarks>
        public int TailPortReferenceNo
        {
            get { return this.tpid; }
            set { this.tpid = value; }
        }

        /// <summary>
        /// Gets or sets the list of connection points
        /// </summary>
        internal List<Point> ConnectionPoints
        {
            get
            {
                return (List<Point>)GetValue(ConnectionPointsProperty);
            }

            set
            {
                SetValue(ConnectionPointsProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the distance of the label from the nodes.
        /// </summary>
        internal double Distance
        {
            get
            {
                return (double)GetValue(DistanceProperty);
            }

            set
            {
                SetValue(DistanceProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the point at which the Connector was dropped.
        /// </summary>
        /// <value>The drop point.</value>
        internal Point DropPoint
        {
            get
            {
                return this.mdroppoint;
            }

            set
            {
                this.mdroppoint = value;
            }
        }

        /// <summary>
        /// Gets or sets the angle at which the Label is to be positioned.
        /// </summary>
        /// <value>
        /// Type: <see cref="Point"/>
        /// The angle .
        /// </value>
        public double LabelAngle
        {
            get
            {
                return (double)GetValue(LabelAngleProperty);
            }

            set
            {
                SetValue(LabelAngleProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the height of the label.
        /// </summary>
        /// <value>The height of the label.</value>
        public double LabelHeight
        {
            get
            {
                return (double)GetValue(LabelHeightProperty);
            }

            set
            {
                SetValue(LabelHeightProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the label template angle.
        /// </summary>
        /// <value>The label template angle.</value>
        internal double LabelTemplateAngle
        {
            get
            {
                return (double)GetValue(LabelTemplateAngleProperty);
            }

            set
            {
                SetValue(LabelTemplateAngleProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the point where the Label is to be positioned.
        /// </summary>
        /// <value>
        /// Type: <see cref="Point"/>
        /// The point of the Label position.
        /// </value>
        internal Point LabelPosition
        {
            get
            {
                return (Point)GetValue(LabelPositionProperty);
            }

            set
            {
                SetValue(LabelPositionProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the label template position.
        /// </summary>
        /// <value>The label template position.</value>
        internal Point LabelTemplatePosition
        {
            get
            {
                return (Point)GetValue(LabelTemplatePositionProperty);
            }

            set
            {
                SetValue(LabelTemplatePositionProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the measurement unit.
        /// <value>
        /// Type: <see cref="MeasureUnits"/>
        /// Current Measurement unit.
        /// </value>
        /// </summary>
        internal MeasureUnits MeasurementUnit
        {
            get
            {
                return (MeasureUnits)GetValue(MeasurementUnitProperty);
            }

            set
            {
                SetValue(MeasurementUnitProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the text width.
        /// </summary>
        /// <value>
        /// Type: <see cref="Point"/>
        /// </value>
        public double TextWidth
        {
            get
            {
                return (double)GetValue(TextWidthProperty);
            }

            set
            {
                SetValue(TextWidthProperty, value);
            }
        }

        /// <summary>
        /// Calculates the intersection point of the line with any of the node sides.
        /// </summary>
        /// <param name="node">The node with which the line intersects.</param>
        /// <param name="pt1">The start point of line.</param>
        /// <param name="pt2">The end point of the line.</param>
        /// <param name="rect">The rectangle which contains the node.</param>
        /// <param name="isTop">Flag to indicate the top side.</param>
        /// <param name="isBottom">Flag to indicate the bottom side.</param>
        /// <param name="isLeft">Flag to indicate the left side.</param>
        /// <param name="isRight">Flag to indicate the right side.</param>
        /// <param name="conType">Specifies the ConnectorType.</param>
        /// <returns>Intersection Point</returns>
        public static Point GetLineIntersect(NodeInfo node, Point pt1, Point pt2, Rect rect, out bool isTop, out bool isBottom, out bool isLeft, out bool isRight, ConnectorType conType)
        {
            NodeInfo n = node;
            Point intersectpoint = new Point(0, 0);
            isTop = false;
            isBottom = false;
            isLeft = false;
            isRight = false;
            bool isVerticalLine = false;
            double m = 0;
            double xintercept;
            double yintercept;

            double rheight = rect.Height;// MeasureUnitsConverter.ToPixels(rect.Height, n.MeasurementUnit);
            double rwidth = rect.Width;// MeasureUnitsConverter.ToPixels(rect.Width, n.MeasurementUnit);
            double rctop = node.Position.Y - (rheight / 2);
            double rcbottom = node.Position.Y + (rheight / 2);
            double rcleft = node.Position.X - (rwidth / 2);
            double rcright = node.Position.X + (rwidth / 2);

            if ((pt2.X - pt1.X) != 0)
            {
                m = LineSlope(pt1, pt2);
            }
            else
            {
                isVerticalLine = true;
            }

            ////Test top side.

            if ((pt1.Y <= rctop && pt2.Y >= rctop) || (pt2.Y <= rctop && pt1.Y >= rctop))
            {
                if (isVerticalLine)
                {
                    xintercept = pt1.X;
                }
                else
                {
                    xintercept = (double)((rctop + ((m * pt1.X) - pt1.Y)) / m);
                }

                if (xintercept >= rcleft && xintercept <= rcright)
                {
                    if (conType != ConnectorType.Straight)
                    {
                        intersectpoint = new Point(rcleft + (rwidth / 2), rctop);
                    }
                    else
                    {
                        intersectpoint = new Point(xintercept, rctop);
                    }

                    isTop = true;
                }
            }

            ////Test bottom side.

            if ((pt1.Y <= rcbottom && pt2.Y >= rcbottom) || (pt2.Y <= rcbottom && pt1.Y >= rcbottom))
            {
                if (isVerticalLine)
                {
                    xintercept = pt1.X;
                }
                else
                {
                    xintercept = (double)((rcbottom + ((m * pt1.X) - pt1.Y)) / m);
                }

                if (xintercept >= rcleft && xintercept <= rcright)
                {
                    if (conType != ConnectorType.Straight)
                    {
                        intersectpoint = new Point(rcleft + (rwidth / 2), rcbottom);
                    }
                    else
                    {
                        intersectpoint = new Point(xintercept, rcbottom);
                    }

                    isBottom = true;
                }
            }

            ////Test left side.

            if ((!isVerticalLine && (pt1.X <= rcleft && pt2.X >= rcleft)) || (pt2.X <= rcleft && pt1.X >= rcleft))
            {
                yintercept = (double)((m * (rcleft - pt1.X)) + pt1.Y);

                if (yintercept >= rctop && yintercept <= rcbottom)
                {
                    if (conType != ConnectorType.Straight)
                    {
                        intersectpoint = new Point(rcleft, rctop + (rheight / 2));
                    }
                    else
                    {
                        intersectpoint = new Point(rcleft, yintercept);
                    }

                    isLeft = true;
                }
            }

            //// Test right side.

            if ((!isVerticalLine && (pt1.X <= rcright && pt2.X >= rcright)) || (pt2.X <= rcright && pt1.X >= rcright))
            {
                yintercept = (double)((m * (rcright - pt1.X)) + pt1.Y);

                if (yintercept >= rctop && yintercept <= rcbottom)
                {
                    if (conType != ConnectorType.Straight)
                    {
                        intersectpoint = new Point(rcright, rctop + (rheight / 2));
                    }
                    else
                    {
                        intersectpoint = new Point(rcright, yintercept);
                    }

                    isRight = true;
                }
            }

            return intersectpoint;
        }

        /// <summary>
        /// Calculates the intersection point of the orthogonal or Bezier line with any of the node sides.
        /// </summary>
        /// <param name="source">The head node.</param>
        /// <param name="target">The tail node.</param>
        /// <param name="rect">The rectangle which contains the head node.</param>
        /// <param name="trect">The rectangle which contains the tail node.</param>
        /// <param name="isTop">Flag to indicate the top side of rect.</param>
        /// <param name="isBottom">Flag to indicate the bottom side of rect.</param>
        /// <param name="isLeft">Flag to indicate the left side of rect.</param>
        /// <param name="isRight">Flag to indicate the right side of rect.</param>
        /// <param name="tisTop">Flag to indicate the top side of target rectangle.</param>
        /// <param name="tisBottom">Flag to indicate the bottom side of target rectangle.</param>
        /// <param name="tisLeft">Flag to indicate the left side of target rectangle.</param>
        /// <param name="tisRight">Flag to indicate the right side of target rectangle.</param>
        /// <param name="si">The intersection point with respect to head node.</param>
        /// <param name="ti">The intersection point with respect to tail node.</param>
        public static void GetOrthogonalLineIntersect(NodeInfo source, NodeInfo target, Rect rect, Rect trect, out bool isTop, out bool isBottom, out bool isLeft, out bool isRight, out bool tisTop, out bool tisBottom, out bool tisLeft, out bool tisRight, out Point si, out Point ti)
        {
            si = new Point(0, 0);
            ti = new Point(0, 0);
            isTop = false;
            isBottom = false;
            isLeft = false;
            isRight = false;
            tisTop = false;
            tisBottom = false;
            tisLeft = false;
            tisRight = false;
            double sheight = rect.Height;// MeasureUnitsConverter.ToPixels(rect.Height, source.MeasurementUnit);
            double swidth = rect.Width;// MeasureUnitsConverter.ToPixels(rect.Width, source.MeasurementUnit);

            double theight = trect.Height;// MeasureUnitsConverter.ToPixels(trect.Height, target.MeasurementUnit);
            double twidth = trect.Width;// MeasureUnitsConverter.ToPixels(trect.Width, target.MeasurementUnit);
            double rctop = source.Position.Y - (sheight / 2);
            double rcbottom = source.Position.Y + (sheight / 2);
            double rcleft = source.Position.X - (swidth / 2);
            double rcright = source.Position.X + (swidth / 2);
            double trctop = target.Position.Y - (theight / 2);
            double trcbottom = target.Position.Y + (theight / 2);
            double trcleft = target.Position.X - (twidth / 2);
            double trcright = target.Position.X + (twidth / 2);

            ////Test top side.
            if (rctop >= trcbottom)
            {
                si = new Point(rcleft + (swidth / 2), rctop);
                isTop = true;
                ti = new Point(trcleft + (twidth / 2), trcbottom);
                tisBottom = true;
            }

            ////Test bottom side.
            if (rcbottom <= trctop)
            {
                si = new Point(rcleft + (swidth / 2), rcbottom);
                isBottom = true;
                ti = new Point(trcleft + (twidth / 2), trctop);
                tisTop = true;
            }

            if ((rcbottom > trctop) && (rctop < trcbottom))
            {
                if (rcright >= trcleft)
                {
                    if (rcleft <= trcleft)
                    {
                        ////left left
                        si = new Point(rcleft, rctop + (sheight / 2));
                        isLeft = true;
                        ti = new Point(trcleft, trctop + (theight / 2));
                        tisLeft = true;
                    }

                    if (rcleft <= trcright)
                    {
                        ////right right
                        si = new Point(rcright, rctop + (sheight / 2));
                        isRight = true;
                        ti = new Point(trcright, trctop + (theight / 2));
                        tisRight = true;
                    }
                    else
                    {
                        ////left right
                        si = new Point(rcleft, rctop + (sheight / 2));
                        isLeft = true;
                        ti = new Point(trcright, trctop + (theight / 2));
                        tisRight = true;
                    }
                }
                else
                {
                    {
                        ////rightleft
                        si = new Point(rcright, rctop + (sheight / 2));
                        isRight = true;
                        ti = new Point(trcleft, trctop + (theight / 2));
                        tisLeft = true;
                    }
                }
            }
        }

        /// <summary>
        /// Gets the tree orthogonal line intersect.
        /// </summary>
        /// <param name="source">The source node.</param>
        /// <param name="target">The target node.</param>
        /// <param name="rect">The source rect.</param>
        /// <param name="trect">The target target rectangle.</param>
        /// <param name="isTop">if set to <c>true</c> [is top].</param>
        /// <param name="isBottom">if set to <c>true</c> [is bottom].</param>
        /// <param name="isLeft">if set to <c>true</c> [is left].</param>
        /// <param name="isRight">if set to <c>true</c> [is right].</param>
        /// <param name="tisTop">if set to <c>true</c> [tis top].</param>
        /// <param name="tisBottom">if set to <c>true</c> [tis bottom].</param>
        /// <param name="tisLeft">if set to <c>true</c> [tis left].</param>
        /// <param name="tisRight">if set to <c>true</c> [tis right].</param>
        /// <param name="si">The start point.</param>
        /// <param name="ti">The end point.</param>
        public static void GetTreeOrthogonalLineIntersect(NodeInfo source, NodeInfo target, Rect rect, Rect trect, out bool isTop, out bool isBottom, out bool isLeft, out bool isRight, out bool tisTop, out bool tisBottom, out bool tisLeft, out bool tisRight, out Point si, out Point ti)
        {
            si = new Point(0, 0);
            ti = new Point(0, 0);
            isTop = false;
            isBottom = false;
            isLeft = false;
            isRight = false;
            tisTop = false;
            tisBottom = false;
            tisLeft = false;
            tisRight = false;
            double sheight = rect.Height;// MeasureUnitsConverter.ToPixels(rect.Height, source.MeasurementUnit);
            double swidth = rect.Width;// MeasureUnitsConverter.ToPixels(rect.Width, source.MeasurementUnit);
            double theight = trect.Height;// MeasureUnitsConverter.ToPixels(trect.Height, source.MeasurementUnit);
            double twidth = trect.Width;// MeasureUnitsConverter.ToPixels(trect.Width, target.MeasurementUnit);
            double rctop = source.Position.Y - (sheight / 2);
            double rcbottom = source.Position.Y + (sheight / 2);
            double rcleft = source.Position.X - (swidth / 2);
            double rcright = source.Position.X + (swidth / 2);
            double trctop = target.Position.Y - (theight / 2);
            double trcbottom = target.Position.Y + (theight / 2);
            double trcleft = target.Position.X - (twidth / 2);
            double trcright = target.Position.X + (twidth / 2);

            if (rcright >= trcleft)
            {
                if (rcleft <= trcleft)
                {
                    ////left left
                    si = new Point(rcleft, rctop + (sheight / 2));
                    isLeft = true;
                    ti = new Point(trcleft, trctop + (theight / 2));
                    tisLeft = true;
                }
                else if (rcleft <= trcright)
                {
                    ////right right
                    si = new Point(rcright, rctop + (sheight / 2));
                    isRight = true;
                    ti = new Point(trcright, trctop + (theight / 2));
                    tisRight = true;
                }
                else
                {
                    ////left right
                    si = new Point(rcleft, rctop + (sheight / 2));
                    isLeft = true;
                    ti = new Point(trcright, trctop + (theight / 2));
                    tisRight = true;
                }
            }
            else
            {
                ////rightleft
                si = new Point(rcright, rctop + (sheight / 2));
                isRight = true;
                ti = new Point(trcleft, trctop + (theight / 2));
                tisLeft = true;
            }
        }

        /// <summary>
        /// Given a Node upon which this Edge is incident, the opposite incident
        /// Node is returned. Throws an exception if the input node is not incident
        /// on this Edge.
        /// </summary>
        /// <param name="node">The node whose adjacent node is to be found</param>
        /// <returns>The node at the other end.</returns>
        /// <example>
        /// <code language="C#">
        /// using Syncfusion.Windows.Diagram;
        ///namespace SilverlightApplication1
        /// {
        /// public partial class MainPage : UserControl
        /// {
        /// public DiagramControl Control;
        /// public DiagramModel Model;
        /// public DiagramView View;
        /// public MainPage()
        /// {
        /// InitializeComponent ();
        /// Control = new DiagramControl ();
        /// Model = new DiagramModel ();
        /// View = new DiagramView ();
        /// Control.View = View;
        /// Control.Model = Model;
        /// View.Bounds = new Thickness(0, 0, 1000, 1000);
        /// //Creating node
        /// Node n = new Node(Guid.NewGuid(), "Start");
        /// n.Shape = Shapes.FlowChart_Start;
        /// n.IsLabelEditable = true;
        /// n.Label = "Start";
        /// n.Level = 1;
        /// n.OffsetX = 150;
        /// n.OffsetY = 25;
        /// n.Width = 150;
        /// n.Height = 75;
        /// n.ToolTip="Start Node";
        /// Model.Nodes.Add(n);
        /// Node n1 = new Node(Guid.NewGuid(), "Decision1");
        /// n1.Shape = Shapes.FlowChart_Process;
        /// n1.IsLabelEditable = true;
        /// n1.Label = "Alarm Rings";
        /// n1.Level = 2;
        /// n1.OffsetX = 150;
        /// n1.OffsetY = 125;
        /// n1.Width = 150;
        /// n1.Height = 75;
        /// Model.Nodes.Add(n1);
        /// //Creating a connection.
        /// LineConnector o2 = new LineConnector();
        /// o2.ConnectorType = ConnectorType.Straight;
        /// o2.TailNode = n1;
        /// o2.HeadNode = n;
        /// IShape node = o2.AdjacentNode(n1);
        /// Model.Connections.Add(o2);
        /// }
        /// }
        /// }
        /// </code>
        /// </example>
        public IShape AdjacentNode(IShape node)
        {
            if (this.HeadNode != null && (node as IShape) == this.HeadNode)
            {
                if (this.TailNode == null)
                {
                    return null;
                }
                else
                {
                    return this.TailNode as IShape;
                }
            }
            else if (this.TailNode != null && (node as IShape) == this.TailNode)
            {
                if (this.HeadNode == null)
                {
                    return null;
                }
                else
                {
                    return this.HeadNode as IShape;
                }
            }
            else
            {
                throw new Exception("The given node is not part of the edge.");
            }
        }

        private bool m_IsPixelDefultUnit = true;

        /// <summary>
        /// When overridden in a derived class, is invoked whenever application code or internal processes call <see cref="M:System.Windows.FrameworkElement.ApplyTemplate"/>.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            this.dview = DiagramPage.GetDiagramControl((FrameworkElement)this).View;
            //this.MeasurementUnit = (this.dview.Page as DiagramPage).MeasurementUnits;

            if (dview != null && dview.Page != null)
            {
                if (this.MeasurementUnit != (dview.Page as DiagramPage).MeasurementUnits)
                {
                    m_IsPixelDefultUnit = false;
                }
                System.Windows.Data.Binding measure = new System.Windows.Data.Binding("MeasurementUnits");
                measure.Source = dview.Page;
                this.SetBinding(ConnectorBase.MeasurementUnitProperty, measure);
            }
        }

        /// <summary>
        /// Updates the line geometry.
        /// </summary>
        public virtual void UpdateConnectorPathGeometry()
        {
        }

        /// <summary>
        /// Calculates the slope.
        /// </summary>
        /// <param name="pt1">The start Point </param>
        /// <param name="pt2">The end point</param>
        /// <returns>The Slope value</returns>
        internal static double LineSlope(Point pt1, Point pt2)
        {
            double m = 0;
            double dx = pt2.X - pt1.X;
            double dy = pt2.Y - pt1.Y;
            if (dx != 0)
            {
                m = dy / dx;
            }
            else
            {
                throw new SlopeUndefinedException();
            }

            return m;
        }

        /// <summary>
        /// Updates the position of the decorator.
        /// </summary>
        /// <param name="line">line connector</param>
        internal void UpdateDecoratorPosition(LineConnector line)
        {
            List<Point> pts = new List<Point>();
            pts = line.ConnectionPoints;

            if (line.ConnectionPoints.Count > 0)
            {
                line.HeadDecoratorPosition = pts[0];
                line.TailDecoratorPosition = pts[pts.Count - 1];

                if (this.ConnectorType == ConnectorType.Straight && line.HeadDecoratorGrid != null && line.TailDecoratorGrid != null)
                {
                    (line as LineConnector).HeadDecoratorAngle = this.FindAngle(pts[pts.Count - 1], pts[0]);
                    (line as LineConnector).TailDecoratorAngle = this.FindAngle(pts[0], pts[pts.Count - 1]);
                }
            }
        }

        /// <summary>
        /// Sets the shape.
        /// </summary>
        /// <param name="shape">sets the shape</param>
        internal void SetShape(string shape)
        {
            //<!-- base style for all arrow shapes -->
            //<Style x:Key="DecoratorBaseStyle" TargetType="Path">
            //    <Setter Property="Fill" Value="Gray"/>
            //    <Setter Property="Stretch" Value="Fill"/>
            //</Style>

            //<!-- Arrow -->
            //<Style x:Key="Arrow" TargetType="Path" BasedOn="{StaticResource DecoratorBaseStyle}">
            //    <Setter Property="Data"  Value="M0,0 8,4 0,8 Z"/>
            //</Style>

            //<!-- Diamond  -->
            //<Style x:Key="Diamond" TargetType="Path" BasedOn="{StaticResource DecoratorBaseStyle}">
            //    <Setter Property="Data" Value="M-5,0 0,-5 5,0 0,5 Z"/>
            //</Style>

            //<!-- Circle  -->
            //<Style x:Key="Circle" TargetType="Path" BasedOn="{StaticResource DecoratorBaseStyle}">
            //    <Setter Property="Data" Value="M5,3C5,4.10456949966159,4.10456949966159,5,3,5C1.89543050033841,5,1,4.10456949966159,1,3C1,1.89543050033841,1.89543050033841,1,3,1C4.10456949966159,1,5,1.89543050033841,5,3z"/>
            //</Style>

            //ResourceDictionary rs = new ResourceDictionary();
            //rs.Source = new Uri("/Syncfusion.Diagram.Silverlight;component/Themes/Generic.xaml", UriKind.RelativeOrAbsolute);

            Style ArrowStyle = new Style(typeof(Path));
            ArrowStyle.Setters.Add(new Setter(Path.DataProperty, "M0,0 8,4 0,8 Z"));
            ArrowStyle.Setters.Add(new Setter(Path.FillProperty, new SolidColorBrush(Colors.Gray)));
            ArrowStyle.Setters.Add(new Setter(Path.StretchProperty, Stretch.Fill));

            Style DiamondStyle = new Style(typeof(Path));
            DiamondStyle.Setters.Add(new Setter(Path.DataProperty, "M-5,0 0,-5 5,0 0,5 Z"));
            DiamondStyle.Setters.Add(new Setter(Path.FillProperty, new SolidColorBrush(Colors.Gray)));
            DiamondStyle.Setters.Add(new Setter(Path.StretchProperty, Stretch.Fill));

            Style CircleStyle = new Style(typeof(Path));
            CircleStyle.Setters.Add(new Setter(Path.DataProperty, "M5,3C5,4.10456949966159,4.10456949966159,5,3,5C1.89543050033841,5,1,4.10456949966159,1,3C1,1.89543050033841,1.89543050033841,1,3,1C4.10456949966159,1,5,1.89543050033841,5,3z"));
            CircleStyle.Setters.Add(new Setter(Path.FillProperty, new SolidColorBrush(Colors.Gray)));
            CircleStyle.Setters.Add(new Setter(Path.StretchProperty, Stretch.Fill));

            if (shape == "Head" && (this as LineConnector).HeadShape as Path != null)
            {
                if (this.HeadDecoratorShape == DecoratorShape.Arrow)
                {
                    ((this as LineConnector).HeadShape as Path).Style = ArrowStyle;//;rs["Arrow"] as Style;
                }
                else if (this.HeadDecoratorShape == DecoratorShape.Diamond)
                {
                    ((this as LineConnector).HeadShape as Path).Style = DiamondStyle;//rs["Diamond"] as Style;
                }
                else
                    if (this.HeadDecoratorShape == DecoratorShape.Circle)
                    {
                        ((this as LineConnector).HeadShape as Path).Style = CircleStyle;//rs["Circle"] as Style;
                    }
                    else
                    {
                        ((this as LineConnector).HeadShape as Path).Style = null;
                    }
            }
            else
            {
                if ((this as LineConnector).TailShape as Path != null)
                {
                    if (this.TailDecoratorShape == DecoratorShape.Arrow)
                    {
                        ((this as LineConnector).TailShape as Path).Style = ArrowStyle;// rs["Arrow"] as Style;
                    }
                    else if (this.TailDecoratorShape == DecoratorShape.Diamond)
                    {
                        ((this as LineConnector).TailShape as Path).Style = DiamondStyle;// rs["Diamond"] as Style;
                    }
                    else if (this.TailDecoratorShape == DecoratorShape.Circle)
                    {
                        ((this as LineConnector).TailShape as Path).Style = CircleStyle;// rs["Circle"] as Style;
                    }
                    else
                    {
                        ((this as LineConnector).TailShape as Path).Style = null;
                    }
                }
            }
        }

        /// <summary>
        /// Hides the adorner
        /// </summary>
        protected virtual void HideAdorner()
        {
        }

        /// <summary>
        /// Handles the PropertyChanged event of the Line control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.ComponentModel.PropertyChangedEventArgs"/> instance containing the event data.</param>
        protected virtual void Line_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
        }

        /// <summary>
        /// Called when [property changed].
        /// </summary>
        /// <param name="name">The name of the property.</param>
        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

        /// <summary>
        /// Shows the adorner
        /// </summary>
        protected virtual void ShowAdorner()
        {
        }

        /// <summary>
        /// Calls OnHeadNodeChanged method of the instance, notifies of the dependency property value changes .
        /// </summary>
        /// <param name="d">Dependency object, the change occurs on.</param>
        /// <param name="e">Property change details, such as old value and new value.</param>
        private static void OnHeadNodeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ConnectorBase cbase = d as ConnectorBase;
            DiagramView dview = Node.GetDiagramView(cbase);
            if (e.OldValue != null && e.OldValue is Node)
            {
                (e.OldValue as Node).Edges.Remove(cbase);
                (e.OldValue as Node).OutEdges.Remove(cbase);
            }
            cbase.HeadNode = (IShape)e.NewValue;
          
                if (cbase.HeadNode != null)
                {
                    NodeChangedRoutedEventArgs newEventArgs;
                    if ((Node)e.OldValue != null && (Node)e.NewValue != null)
                    {
                        newEventArgs = new NodeChangedRoutedEventArgs((Node)e.OldValue, (Node)e.NewValue, cbase as LineConnector);
                    }
                    else
                        if ((Node)e.OldValue == null && (Node)e.NewValue != null)
                        {
                            newEventArgs = new NodeChangedRoutedEventArgs((Node)e.NewValue, cbase as LineConnector);
                        }
                        else if ((Node)e.OldValue != null && (Node)e.NewValue == null)
                        {
                            newEventArgs = new NodeChangedRoutedEventArgs(cbase as LineConnector, (Node)e.OldValue);
                        }
                        else
                        {
                            newEventArgs = new NodeChangedRoutedEventArgs(cbase as LineConnector);
                        }
                    if (dview != null)
                    {
                        dview.OnHeadNodeChanged((cbase as LineConnector), newEventArgs);
                    }
                    (cbase as LineConnector).InternalHeadShape = cbase.HeadDecoratorShape;
                    cbase.HeadNode.OutEdges.Add(cbase);
                    cbase.HeadNode.Edges.Add(cbase);
                    (cbase.HeadNode as Node).PropertyChanged += new PropertyChangedEventHandler(cbase.Line_PropertyChanged);
                
            }
        }

        /// <summary>
        /// Called when [head port changed].
        /// </summary>
        /// <param name="d">The DependencyObject.</param>
        /// <param name="e">The <see cref="System.Windows.DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void OnHeadPortChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ConnectorBase line = d as ConnectorBase;
            line.UpdateConnectorPathGeometry();
        }

        /// <summary>
        /// Called when [label changed].
        /// </summary>
        /// <param name="d">The DependencyObject.</param>
        /// <param name="e">The <see cref="System.Windows.DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void OnLabelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            string oldvalue = (string)e.OldValue;
            string newvalue = (string)e.NewValue;
            ConnectorBase cbase = d as ConnectorBase;
            DiagramView dview = Node.GetDiagramView(cbase);
            LabelConnRoutedEventArgs newEventArgs;
            if (cbase.HeadNode as Node != null && cbase.TailNode as Node != null)
            {
                newEventArgs = new LabelConnRoutedEventArgs((string)e.OldValue, (string)e.NewValue, cbase.HeadNode as Node, cbase.TailNode as Node, cbase as LineConnector);
            }
            else
                if (cbase.HeadNode as Node == null && cbase.TailNode as Node != null)
                {
                    newEventArgs = new LabelConnRoutedEventArgs((string)e.OldValue, (string)e.NewValue, cbase.TailNode as Node, cbase as LineConnector);
                }
                else if (cbase.HeadNode as Node != null && cbase.TailNode as Node == null)
                {
                    newEventArgs = new LabelConnRoutedEventArgs((string)e.OldValue, (string)e.NewValue, cbase as LineConnector, cbase.HeadNode as Node);
                }
                else
                {
                    newEventArgs = new LabelConnRoutedEventArgs((string)e.OldValue, (string)e.NewValue, cbase as LineConnector);
                }
            if (dview != null)
            {
                dview.OnConnectorLabelChanged((cbase as LineConnector), newEventArgs);
            }
        }

        /// <summary>
        /// Called when [label position changed].
        /// </summary>
        /// <param name="d">The DependencyObject.</param>
        /// <param name="e">The <see cref="System.Windows.DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void OnLabelPositionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Point pt = (d as LineConnector).LabelPosition;
            if (pt != null)
            {
                //if (double.IsNaN(pt.X))
                {
                    (d as LineConnector).LabelPosition = new Point((double.IsNaN(pt.X) ? 0 : pt.X), (double.IsNaN(pt.Y) ? 0 : pt.Y));
                }
                //if(double.IsNaN(pt.Y))
                //{
                //    (d as LineConnector).LabelPosition.Y = 0;
                //}
            }
        }

        /// <summary>
        /// Called when [label template position changed].
        /// </summary>
        /// <param name="d">The d.</param>
        /// <param name="e">The <see cref="System.Windows.DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void OnLabelTemplatePositionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
        }

        /// <summary>
        /// Calls OnTailNodeChanged method of the instance, notifies of the dependency property value changes.
        /// </summary>
        /// <param name="d">Dependency object, the change occurs on.</param>
        /// <param name="e">Property change details, such as old value and new value.</param>
        private static void OnTailNodeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ConnectorBase cbase = d as ConnectorBase;
            DiagramView dview = Node.GetDiagramView(cbase);
            if (e.OldValue != null && e.OldValue is Node)
            {
                (e.OldValue as Node).Edges.Remove(cbase);
                (e.OldValue as Node).InEdges.Remove(cbase);
            }
            if (cbase.TailNode != null)
                {
                    NodeChangedRoutedEventArgs newEventArgs;
                    if ((Node)e.OldValue != null && (Node)e.NewValue != null)
                    {
                        newEventArgs = new NodeChangedRoutedEventArgs((Node)e.OldValue, (Node)e.NewValue, cbase as LineConnector);
                    }
                    else
                        if ((Node)e.OldValue == null && (Node)e.NewValue != null)
                        {
                            newEventArgs = new NodeChangedRoutedEventArgs((Node)e.NewValue, cbase as LineConnector);
                        }
                        else if ((Node)e.OldValue != null && (Node)e.NewValue == null)
                        {
                            newEventArgs = new NodeChangedRoutedEventArgs(cbase as LineConnector, (Node)e.OldValue);
                        }
                        else
                        {
                            newEventArgs = new NodeChangedRoutedEventArgs(cbase as LineConnector);
                       
                        }
                    if (dview != null)
                    {
                        dview.OnTailNodeChanged(cbase as LineConnector, newEventArgs);
                    }
                    (cbase as LineConnector).InternalTailShape = cbase.TailDecoratorShape;

                    cbase.TailNode.InEdges.Add(cbase);
                    cbase.TailNode.Edges.Add(cbase);
                    (cbase.TailNode as Node).PropertyChanged += new PropertyChangedEventHandler(cbase.Line_PropertyChanged);
                
            }
        }

        /// <summary>
        /// Called when [tail port changed].
        /// </summary>
        /// <param name="d">The DependencyObject.</param>
        /// <param name="e">The <see cref="System.Windows.DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void OnTailPortChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ConnectorBase line = d as ConnectorBase;
            line.UpdateConnectorPathGeometry();
        }

        /// <summary>
        /// Called when [text width changed].
        /// </summary>
        /// <param name="d">The DependencyObject.</param>
        /// <param name="e">The <see cref="System.Windows.DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void OnTextWidthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ConnectorBase cbase = d as ConnectorBase;
        }

        public static void OnLabelOrientationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            LineConnector lc=d as LineConnector;
            if (lc != null)
            {
                if(lc.LabelOrientation==LabelOrientation.Vertical)
                {
                  
                }
            }

        }
        public static void OnLabelTemplateOrientationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            LineConnector lc = d as LineConnector;
            if (lc != null)
            {
                if (lc.LabelOrientation == LabelOrientation.Vertical)
                {

                }
            }

        }

        /// <summary>
        /// Called when [units changed].
        /// </summary>
        /// <param name="d">The DependencyObject.</param>
        /// <param name="e">The <see cref="System.Windows.DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void OnUnitsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            LineConnector lc = d as LineConnector;
            if (lc != null)
            {
                if (lc.m_IsPixelDefultUnit)
                {
                    lc.StartPointPosition = MeasureUnitsConverter.Convert(lc.StartPointPosition, (MeasureUnits)e.OldValue, (MeasureUnits)e.NewValue);
                    lc.EndPointPosition = MeasureUnitsConverter.Convert(lc.EndPointPosition, (MeasureUnits)e.OldValue, (MeasureUnits)e.NewValue);
                }
                lc.ConnectionEndSpace = MeasureUnitsConverter.Convert(lc.ConnectionEndSpace, (MeasureUnits)e.OldValue, (MeasureUnits)e.NewValue);
                lc.m_IsPixelDefultUnit = true;
            }
        }
       
        /// <summary>
        /// finds the angle     
        /// </summary>
        /// <param name="s">start point to find angle between</param>
        /// <param name="e">end point to find angle between</param>
        /// <returns>angle between the points</returns>
        internal double FindAngle(Point s, Point e)
        {
            Point r = new Point(e.X, s.Y);
            double sr = this.FindHypo(s, r);
            double re = this.FindHypo(r, e);
            double es = this.FindHypo(e, s);
            double ang = Math.Asin(re / es);
            ang = ang * 180 / Math.PI;
            if (s.X < e.X)
            {
                if (s.Y < e.Y)
                {
                }
                else
                {
                    ang = 360 - ang;
                }
            }
            else
            {
                if (s.Y < e.Y)
                {
                    ang = 180 - ang;
                }
                else
                {
                    ang = 180 + ang;
                }
            }

            if (double.IsNaN(ang))
            {
                return 0;
            }
            else
            {
                return ang;
            }
        }

        /// <summary>
        /// Finds the hypotenuse
        /// </summary>
        /// <param name="s">start point to find angle between</param>
        /// <param name="e">end point to find angle between</param>
        /// <returns>angle between the points</returns>
        private double FindHypo(Point s, Point e)
        {
            double length;
            length = Math.Sqrt(Math.Pow((s.X - e.X), 2) + Math.Pow((s.Y - e.Y), 2));
            return length;
        }

        #region Class fields

        #endregion

        #region Initialization

        #endregion

        #region Properties

        #endregion

        #region Dependency Properties

        #endregion

        #region Events

        #endregion

        #region Virtual Methods

        #endregion

        #region INotifyPropertyChanged Members

        #endregion

        #region IEdge Members

        #endregion

        #region INodeGroup Members

        #endregion

        #region Methods

        /// <summary>
        /// Represents the slope undefined exception .
        /// </summary>
        internal class SlopeUndefinedException : System.Exception
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="SlopeUndefinedException"/> class.
            /// </summary>
            public SlopeUndefinedException()
            {
            }
        }

        #endregion
    }
}
