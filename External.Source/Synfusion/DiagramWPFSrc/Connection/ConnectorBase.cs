// <copyright file="ConnectorBase.cs" company="Syncfusion">
// Copyright Syncfusion Inc. 2001 - 2011. All rights reserved.
// Use of this code is subject to the terms of our license.
// A copy of the current license can be obtained at any time by e-mailing
// licensing@syncfusion.com. Any infringement will be prosecuted under
// applicable laws. 
// </copyright>

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Controls.Primitives;
using System.Collections.ObjectModel;
using Syncfusion.Windows.Shared;
using System.Diagnostics;

namespace Syncfusion.Windows.Diagram
{
    /// <summary>
    /// Represents base abstract class for Connectors.
    /// </summary>
#if SyncfusionFramework4_0
    [DesignTimeVisible(false)]
#endif
    public abstract class ConnectorBase : ContentControl, IEdge, ICommon, ICloneable, INotifyPropertyChanged, INodeGroup
    {
        #region Class fields

        /// <summary>
        /// Used to store head node reference no.
        /// </summary>
        private int hid = -1;

        /// <summary>
        /// Used to store tail node reference no.
        /// </summary>
        private int tid = -1;

        /// <summary>
        /// Used to store the reference number of the nodes and connectors.
        /// </summary>
        private int no = -1;

        /// <summary>
        /// Used to store head port reference no.
        /// </summary>
        private int hpid;

        /// <summary>
        /// Used to store tail port reference no.
        /// </summary>
        private int tpid;

        ///// <summary>
        ///// Used to specify the head decorator shape.
        ///// </summary>
        //private DecoratorShape m_headDecoratorShape = DecoratorShape.None;

        ///// <summary>
        ///// Used to specify the tail decorator shape.
        ///// </summary>
        //private DecoratorShape m_tailDecoratorShape = DecoratorShape.Arrow;

        /// <summary>
        /// Used to store IsDirected property information.
        /// </summary>
        private bool mIsDirected = false;

        /// <summary>
        /// Used to store the path geometry.
        /// </summary>
        private PathGeometry pathGeometry;

        /// <summary>
        /// Used to store the head decorator position
        /// </summary>
        private Point headDecoratorPosition;

        /// <summary>
        /// Used to store the head decorator angle.
        /// </summary>
        private double headDecoratorAngle = 0;

        /// <summary>
        /// Used to store the tail decorator position
        /// </summary>
        private Point tailDecoratorPosition;

        /// <summary>
        /// Used to store the tail decorator angle.
        /// </summary>
        private double tailDecoratorAngle = 0;

        ///// <summary>
        ///// Used to store ISelected property setting value.
        ///// </summary>
        //private bool isSelected;

        ///// <summary>
        ///// Used to store the line style property values.
        ///// </summary>
        //private LineStyle lineStyle;

        ///// <summary>
        ///// Used to store the head decorator style property values.
        ///// </summary>
        //private DecoratorStyle m_headDecoratorStyle;

        ///// <summary>
        ///// Used to store the tail decorator style property values.
        ///// </summary>
        //private DecoratorStyle m_tailDecoratorStyle;

        /// <summary>
        /// Used to store the connector drop point.
        /// </summary>
        private Point m_droppoint = new Point(0, 0);

        /// <summary>
        /// Used to store the old ZIndex value.
        /// </summary>
        private int m_oldindex = 0;

        /// <summary>
        /// Used to store the new ZIndex value.
        /// </summary>
        private int m_newindex = 0;

        /// <summary>
        /// Used to store the line adorner.
        /// </summary>
        private Adorner mlineadorn;

        /// <summary>
        /// Used to store the groups.
        /// </summary>
        private CollectionExt m_groups = new CollectionExt();

        /// <summary>
        /// Used to store end point position value.
        /// </summary>
        protected Point m_endpointposition = new Point(0, 0);

        /// <summary>
        /// Used to store start point position value.
        /// </summary>
        protected Point m_startpointposition = new Point(0, 0);

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

        #endregion

        #region Initialization

        /// <summary>
        /// Initializes a new instance of the <see cref="ConnectorBase"/> class.
        /// </summary>
        public ConnectorBase()
        {
            this.LineStyle = new LineStyle();
            this.HeadDecoratorStyle = new DecoratorStyle();
            this.TailDecoratorStyle = new DecoratorStyle();
            this.ConnectorPathGeometry = new PathGeometry();
            this.Loaded += new RoutedEventHandler(FirstLoaded);
        }

        private bool _FirstLoaded;

        internal bool m_LineDrawing=false;
        void FirstLoaded(object sender, RoutedEventArgs e)
        {
            this.Loaded -= new RoutedEventHandler(FirstLoaded);
            _FirstLoaded = true;
        }

        #endregion

        #region Properties

        public TextDecorationCollection LabelTextDecorations
        {
            get { return (TextDecorationCollection)GetValue(LabelTextDecorationsProperty); }
            set { SetValue(LabelTextDecorationsProperty, value); }
        }

        public double FirstSegmentLength
        {
            get
            {
                return (double)GetValue(FirstSegmentLengthProperty);
            }

            set
            {
                SetValue(FirstSegmentLengthProperty, value);
            }
        }

        public double LastSegmentLength
        {
            get
            {
                return (double)GetValue(LastSegmentLengthProperty);
            }

            set
            {
                SetValue(LastSegmentLengthProperty, value);
            }
        }

        public bool AutoAdjustPoints
        {
            get
            {
                return (bool)GetValue(AutoAdjustPointsProperty);
            }

            set
            {
                SetValue(AutoAdjustPointsProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the bridge spacing.
        /// </summary>
        /// <value>The bridge spacing.</value>
        internal double BridgeSpacing
        {
            get
            {
                return (double)GetValue(BridgeSpacingProperty);
            }

            set
            {
                SetValue(BridgeSpacingProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [line bridging is enabled].
        /// </summary>
        /// <value><c>true</c> if [line bridging is enabled]; otherwise, <c>false</c>.</value>
        public bool LineBridgingEnabled
        {
            get
            {
                return (bool)GetValue(LineBridgingEnabledProperty);
            }

            set
            {
                SetValue(LineBridgingEnabledProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is vertex visible.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is vertex visible; otherwise, <c>false</c>.
        /// </value>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsVertexVisible
        {
            get
            {
                return (bool)GetValue(IsVertexVisibleProperty);
            }

            set
            {
                SetValue(IsVertexVisibleProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is vertex movable.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is vertex movable; otherwise, <c>false</c>.
        /// </value>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsVertexMovable
        {
            get
            {
                return (bool)GetValue(IsVertexMovableProperty);
            }

            set
            {
                SetValue(IsVertexMovableProperty, value);
            }
        }




        public bool IsHeadMovable
        {
            get { return (bool)GetValue(IsHeadMovableProperty); }
            set { SetValue(IsHeadMovableProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsHeadMovable.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsHeadMovableProperty =
            DependencyProperty.Register("IsHeadMovable", typeof(bool), typeof(ConnectorBase), new UIPropertyMetadata(true));



        public bool IsTailMovable
        {
            get { return (bool)GetValue(IsTailMovableProperty); }
            set { SetValue(IsTailMovableProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsTailMovable.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsTailMovableProperty =
            DependencyProperty.Register("IsTailMovable", typeof(bool), typeof(ConnectorBase), new UIPropertyMetadata(true));



        /// <summary>
        /// Gets or sets the vertex style.
        /// </summary>
        /// <value>The vertex style.</value>
        public Style VertexStyle
        {
            get
            {
                return (Style)GetValue(VertexStyleProperty);
            }

            set
            {
                SetValue(VertexStyleProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the decorator adorner style.
        /// </summary>
        /// <value>The decorator adorner style.</value>
        public Style DecoratorAdornerStyle
        {
            get
            {
                return (Style)GetValue(DecoratorAdornerStyleProperty);
            }

            set
            {
                SetValue(DecoratorAdornerStyleProperty, value);
            }
        }
        /// <summary>
        /// Gets or sets the old ZIndex value.
        /// </summary>
        /// <value>The old ZIndex value.</value>
        public int OldZIndex
        {
            get { return m_oldindex; }
            set { m_oldindex = value; }
        }

        /// <summary>
        /// Gets or sets the new ZIndex value.
        /// </summary>
        /// <value>The new ZIndex value.</value>
        public int NewZIndex
        {
            get { return m_newindex; }
            set { m_newindex = value; }
        }

        /// <summary>
        /// Gets or sets the point at which the Connector was dropped.
        /// </summary>
        /// <value>The drop point.</value>
        internal Point DropPoint
        {
            get
            {
                return m_droppoint;
            }

            set
            {
                m_droppoint = value;
            }
        }

        /// <summary>
        /// Gets or sets the end point position.
        /// </summary>
        /// <value>The end point position.</value>
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
            get { return (Point)GetValue(EndPointPositionProperty); }
            set { SetValue(EndPointPositionProperty, value); }
        }

        public static readonly DependencyProperty EndPointPositionProperty =
            DependencyProperty.Register("EndPointPosition", typeof(Point), typeof(ConnectorBase));


        //public Point EndPointPosition
        //{
        //    get { return m_endpointposition; }
        //    set { m_endpointposition = value; }
        //}

        /// <summary>
        /// Gets or sets the start point position.
        /// </summary>
        /// <value>The start point position.</value>
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
            get { return (Point)GetValue(StartPointPositionProperty); }
            set { SetValue(StartPointPositionProperty, value); }
        }

        // Using a DependencyProperty as the backing store for StartPointPosition.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StartPointPositionProperty =
            DependencyProperty.Register("StartPointPosition", typeof(Point), typeof(ConnectorBase));


        //public Point StartPointPosition
        //{
        //    get { return m_startpointposition; }
        //    set { m_startpointposition = value; }
        //}

        /// <summary>
        /// Gets or sets the line adorner.
        /// </summary>
        /// <value>The line adorner.</value>
        protected Adorner LineAdorner
        {
            get { return mlineadorn; }
            set { mlineadorn = value; }
        }

        /// <summary>
        /// Gets or sets a unique identifier for the connector.
        /// </summary>
        /// <value>
        /// Type: <see cref="Guid"/>
        /// Unique ID for the connector.
        /// </value>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Guid ID { get; set; }

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

        /// <summary>
        /// Gets or sets the intermediate points.
        /// </summary>
        /// <value>The intermediate points.</value>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public List<Point> IntermediatePoints
        {
            get
            {
                return (List<Point>)GetValue(IntermediatePointsProperty);
            }

            set
            {
                SetValue(IntermediatePointsProperty, value);
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
                return headDecoratorPosition;
            }

            set
            {
                if (headDecoratorPosition != value)
                {
                    headDecoratorPosition = value;
                    OnPropertyChanged("HeadDecoratorPosition");
                }
            }
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
                return headDecoratorAngle;
            }

            set
            {
                if (headDecoratorAngle != value)
                {
                    headDecoratorAngle = value;
                    OnPropertyChanged("HeadDecoratorAngle");
                }
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
                return tailDecoratorPosition;
            }

            set
            {
                if (tailDecoratorPosition != value)
                {
                    tailDecoratorPosition = value;
                    OnPropertyChanged("TailDecoratorPosition");
                }
            }
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
                return tailDecoratorAngle;
            }

            set
            {
                if (tailDecoratorAngle != value)
                {
                    tailDecoratorAngle = value;
                    OnPropertyChanged("TailDecoratorAngle");
                }
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
                //return m_headDecoratorShape;
                return (DecoratorShape)GetValue(HeadDecoratorShapeProperty);
            }

            set
            {
                SetValue(HeadDecoratorShapeProperty, value); if (!(this as LineConnector).IsOverlapped)
                {
                    (this as LineConnector).InternalHeadShape = value;
                }
                //if (m_headDecoratorShape != value)
                //{
                //    //m_headDecoratorShape = value;
                //    SetValue(HeadDecoratorShapeProperty, value);
                //    if (!(this as LineConnector).IsOverlapped)
                //    {
                //        (this as LineConnector).InternalHeadShape = value;
                //    }

                //    OnPropertyChanged("HeadDecoratorShape");
                //}
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
                //return m_tailDecoratorShape;
                return (DecoratorShape)GetValue(TailDecoratorShapeProperty);
            }

            set
            {
                SetValue(TailDecoratorShapeProperty, value);
                if (!(this as LineConnector).IsOverlapped)
                {
                    (this as LineConnector).InternalTailShape = value;
                }
                //if (m_tailDecoratorShape != value)
                //{
                //    //m_tailDecoratorShape = value;
                //    SetValue(TailDecoratorShapeProperty, value);
                //    if (!(this as LineConnector).IsOverlapped)
                //    {
                //        (this as LineConnector).InternalTailShape = value;
                //    }

                //    OnPropertyChanged("TailDecoratorShape");
                //}
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
        /// Gets or sets the text width.
        /// </summary>
        /// <value>
        /// Type: <see cref="Point"/>
        /// </value>
        internal double TextWidth
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
        /// Gets or sets the label background.
        /// </summary>
        /// <value>The label background. Default value is White</value>
        /// <example>
        /// <para/>This example shows how to set LabelBackground in C#.
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
        /// Gets or sets the Label Foreground.
        /// </summary>
        /// <value>The label foreground. Default value is Black.</value>
        /// <example>
        /// <para/>This example shows how to set LabelForeground in C#.
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
        /// Gets or sets the width of the label.
        /// </summary>
        /// <value>The width of the label. By default it is set to the line width.</value>
        /// <example>
        /// <code language="C#">
        /// using Syncfusion.Windows.Diagram;
        /// namespace WpfApplication1
        /// {
        /// public partial class Window1 : Window
        /// {
        /// public DiagramControl Control;
        /// public DiagramModel Model;
        /// public DiagramView View;
        /// public Window1 ()
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
        /// Gets or sets the height of the label.
        /// </summary>
        /// <value>The height of the label.</value>
        internal double LabelHeight
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
        /// Gets or sets the label template distance.
        /// </summary>
        /// <value>The label template distance.</value>
        internal double LabelTemplateDistance
        {
            get
            {
                return (double)GetValue(LabelTemplateDistanceProperty);
            }

            set
            {
                SetValue(LabelTemplateDistanceProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the angle at which the Label is to be positioned.
        /// </summary>
        /// <value>
        /// Type: <see cref="Point"/>
        /// The angle .
        /// </value>
        internal double LabelAngle
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
        /// namespace WpfApplication1
        /// {
        /// public partial class Window1 : Window
        /// {
        /// public DiagramControl Control;
        /// public DiagramModel Model;
        /// public DiagramView View;
        /// public Window1 ()
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
        /// namespace WpfApplication1
        /// {
        /// public partial class Window1 : Window
        /// {
        /// public DiagramControl Control;
        /// public DiagramModel Model;
        /// public DiagramView View;
        /// public Window1 ()
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
        /// namespace WpfApplication1
        /// {
        /// public partial class Window1 : Window
        /// {
        /// public DiagramControl Control;
        /// public DiagramModel Model;
        /// public DiagramView View;
        /// public Window1 ()
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
        //[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        // public bool IsSelected
        //  {
        //   get
        //  {
        //      return isSelected;
        // }

        // set
        // {
        //     if (isSelected != value)
        //    {
        //       isSelected = value;
        //      OnPropertyChanged("IsSelected");
        //      {
        //         ShowAdorner();
        //   }
        //   else
        //   {
        //       HideAdorner();
        //   }
        // }
        //}
        // }

        /// <summary>
        /// Gets or sets the PathGeometry for the connector.
        /// </summary>
        /// <value>
        /// Type: <see cref="PathGeometry"/>
        /// PathGeometry of the connector.
        /// </value>
        public PathGeometry ConnectorPathGeometry
        {
            get
            {
                return pathGeometry;
            }

            set
            {
                if (pathGeometry != value)
                {
                    pathGeometry = value;
                    UpdateDecoratorPosition();
                    OnPropertyChanged("ConnectorPathGeometry");
                }
            }
        }


        /// <summary>
        /// Gets or sets the virtual connector path geometry.
        /// </summary>
        /// <value>The virtual connector path geometry.</value>
        internal PathGeometry VirtualConnectorPathGeometry
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the z order.
        /// </summary>
        /// <value>The z order.</value>
        internal int zOrder
        {
            get
            {
                return Canvas.GetZIndex(this);
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
        /// namespace WpfApplication1
        /// {
        /// public partial class Window1 : Window
        /// {
        /// public DiagramControl Control;
        /// public DiagramModel Model;
        /// public DiagramView View;
        /// public Window1 ()
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
                return (LineStyle)GetValue(LineStyleProperty);
                //if (lineStyle != null)
                //{
                //    return lineStyle;
                //}
                //else
                //{
                //    lineStyle = new LineStyle(this);
                //}

                //return lineStyle;
            }

            set
            {
                //lineStyle = value;
                SetValue(LineStyleProperty, value);
            }
        }

        public Style CustomPathStyle
        {
            get
            {
                return (Style)GetValue(CustomPathStyleProperty);
            }

            set
            {
                SetValue(CustomPathStyleProperty, value);
            }
        }

        public Style CustomHeadDecoratorStyle
        {
            get
            {
                return (Style)GetValue(CustomHeadDecoratorStyleProperty);
            }

            set
            {
                SetValue(CustomHeadDecoratorStyleProperty, value);
            }
        }

        public Style CustomTailDecoratorStyle
        {
            get
            {
                return (Style)GetValue(CustomTailDecoratorStyleProperty);
            }

            set
            {
                SetValue(CustomTailDecoratorStyleProperty, value);
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
        /// namespace WpfApplication1
        /// {
        /// public partial class Window1 : Window
        /// {
        /// public DiagramControl Control;
        /// public DiagramModel Model;
        /// public DiagramView View;
        /// public Window1 ()
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
                return (DecoratorStyle)GetValue(HeadDecoratorStyleProperty);
                //if (m_headDecoratorStyle != null)
                //{
                //    return m_headDecoratorStyle;
                //}
                //else
                //{
                //    m_headDecoratorStyle = new DecoratorStyle();
                //}

                //return m_headDecoratorStyle;
            }

            set
            {
                SetValue(HeadDecoratorStyleProperty, value);
                //m_headDecoratorStyle = value;
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
        /// namespace WpfApplication1
        /// {
        /// public partial class Window1 : Window
        /// {
        /// public DiagramControl Control;
        /// public DiagramModel Model;
        /// public DiagramView View;
        /// public Window1 ()
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
                return (DecoratorStyle)GetValue(TailDecoratorStyleProperty);
                //if (m_tailDecoratorStyle != null)
                //{
                //    return m_tailDecoratorStyle;
                //}
                //else
                //{
                //    m_tailDecoratorStyle = new DecoratorStyle();
                //}

                //return m_tailDecoratorStyle;
            }

            set
            {
                SetValue(TailDecoratorStyleProperty, value);
                //m_tailDecoratorStyle = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [enable multiline label].
        /// </summary>
        /// <value>
        /// 	<c>true</c> if [enable multiline label]; otherwise, <c>false</c>.
        /// </value>
        public bool EnableMultilineLabel
        {
            get
            {
                return (bool)GetValue(EnableMultilineLabelProperty);
            }
            set
            {
                SetValue(EnableMultilineLabelProperty, value);
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
        /// namespace WpfApplication1
        /// {
        /// public partial class Window1 : Window
        /// {
        /// public DiagramControl Control;
        /// public DiagramModel Model;
        /// public DiagramView View;
        /// public Window1 ()
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
        /// namespace WpfApplication1
        /// {
        /// public partial class Window1 : Window
        /// {
        /// public DiagramControl Control;
        /// public DiagramModel Model;
        /// public DiagramView View;
        /// public Window1 ()
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
        /// namespace WpfApplication1
        /// {
        /// public partial class Window1 : Window
        /// {
        /// public DiagramControl Control;
        /// public DiagramModel Model;
        /// public DiagramView View;
        /// public Window1 ()
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
        /// namespace WpfApplication1
        /// {
        /// public partial class Window1 : Window
        /// {
        /// public DiagramControl Control;
        /// public DiagramModel Model;
        /// public DiagramView View;
        /// public Window1 ()
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
        /// Gets or sets the label text wrapping.
        /// </summary>
        /// <value>Default value is NoWrap.</value>
        /// <example>
        /// <code language="C#">
        /// using Syncfusion.Windows.Diagram;
        /// namespace WpfApplication1
        /// {
        /// public partial class Window1 : Window
        /// {
        /// public DiagramControl Control;
        /// public DiagramModel Model;
        /// public DiagramView View;
        /// public Window1 ()
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
        /// Gets or sets the label font size.
        /// </summary>
        /// <value>Default value is 11d.</value>
        /// <example>
        /// <code language="C#">
        /// using Syncfusion.Windows.Diagram;
        /// namespace WpfApplication1
        /// {
        /// public partial class Window1 : Window
        /// {
        /// public DiagramControl Control;
        /// public DiagramModel Model;
        /// public DiagramView View;
        /// public Window1 ()
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
        /// Gets or sets the label font family.
        /// </summary>
        /// <value>Default value is Arial.</value>
        /// <example>
        /// <code language="C#">
        /// using Syncfusion.Windows.Diagram;
        /// namespace WpfApplication1
        /// {
        /// public partial class Window1 : Window
        /// {
        /// public DiagramControl Control;
        /// public DiagramModel Model;
        /// public DiagramView View;
        /// public Window1 ()
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
        /// Gets or sets the label font weight.
        /// </summary>
        /// <value>Default value is SemiBold.</value>
        /// <example>
        /// <code language="C#">
        /// using Syncfusion.Windows.Diagram;
        /// namespace WpfApplication1
        /// {
        /// public partial class Window1 : Window
        /// {
        /// public DiagramControl Control;
        /// public DiagramModel Model;
        /// public DiagramView View;
        /// public Window1 ()
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
        /// Gets or sets the label font style.
        /// </summary>
        /// <value>Default value is Normal.</value>
        /// <example>
        /// <code language="C#">
        /// using Syncfusion.Windows.Diagram;
        /// namespace WpfApplication1
        /// {
        /// public partial class Window1 : Window
        /// {
        /// public DiagramControl Control;
        /// public DiagramModel Model;
        /// public DiagramView View;
        /// public Window1 ()
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
        /// Gets or sets the label text trimming.
        /// </summary>
        /// <value>Default value is CharacterEllipsis.</value>
        /// <example>
        /// <code language="C#">
        /// using Syncfusion.Windows.Diagram;
        /// namespace WpfApplication1
        /// {
        /// public partial class Window1 : Window
        /// {
        /// public DiagramControl Control;
        /// public DiagramModel Model;
        /// public DiagramView View;
        /// public Window1 ()
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
        /// connObject.LabelTextTrimming = TextTrimming.None;
        /// Model.Connections.Add(connObject);
        /// }
        /// }
        /// }
        /// </code>
        /// </example>
        public TextTrimming LabelTextTrimming
        {
            get
            {
                return (TextTrimming)GetValue(LabelTextTrimmingProperty);
            }

            set
            {
                SetValue(LabelTextTrimmingProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the label text alignment.
        /// </summary>
        /// <value>Default value is Center.</value>
        /// <example>
        /// <code language="C#">
        /// using Syncfusion.Windows.Diagram;
        /// namespace WpfApplication1
        /// {
        /// public partial class Window1 : Window
        /// {
        /// public DiagramControl Control;
        /// public DiagramModel Model;
        /// public DiagramView View;
        /// public Window1 ()
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

        #endregion

        #region Dependency Properties

        public static readonly DependencyProperty LabelTextDecorationsProperty = DependencyProperty.Register("LabelTextDecorations", typeof(TextDecorationCollection), typeof(ConnectorBase));
        public static readonly DependencyProperty LineStyleProperty = DependencyProperty.Register("LineStyle", typeof(LineStyle), typeof(ConnectorBase), new PropertyMetadata(null));
        public static readonly DependencyProperty HeadDecoratorStyleProperty = DependencyProperty.Register("HeadDecoratorStyle", typeof(DecoratorStyle), typeof(ConnectorBase), new PropertyMetadata(null));
        public static readonly DependencyProperty TailDecoratorStyleProperty = DependencyProperty.Register("TailDecoratorStyle", typeof(DecoratorStyle), typeof(ConnectorBase), new PropertyMetadata(null));

        public static readonly DependencyProperty CustomPathStyleProperty = DependencyProperty.Register("CustomPathStyle", typeof(Style), typeof(ConnectorBase), new PropertyMetadata(null, OnCustomPathStyleChanged));
        public static readonly DependencyProperty CustomHeadDecoratorStyleProperty = DependencyProperty.Register("CustomHeadDecoratorStyle", typeof(Style), typeof(ConnectorBase), new PropertyMetadata(null));
        public static readonly DependencyProperty CustomTailDecoratorStyleProperty = DependencyProperty.Register("CustomTailDecoratorStyle", typeof(Style), typeof(ConnectorBase), new PropertyMetadata(null));

        public static readonly DependencyProperty HeadDecoratorShapeProperty = DependencyProperty.Register("HeadDecoratorShape", typeof(DecoratorShape), typeof(ConnectorBase), new PropertyMetadata(DecoratorShape.None));
        public static readonly DependencyProperty TailDecoratorShapeProperty = DependencyProperty.Register("TailDecoratorShape", typeof(DecoratorShape), typeof(ConnectorBase), new PropertyMetadata(DecoratorShape.Arrow));
        /// <summary>
        /// Identifies the LabelWidth LineBridgingEnabled dependency property.
        /// </summary>
        public static readonly DependencyProperty LineBridgingEnabledProperty = DependencyProperty.Register("LineBridgingEnabled", typeof(bool), typeof(ConnectorBase), new PropertyMetadata(false, new PropertyChangedCallback(OnLineBridgingEnabledChanged)));

        /// <summary>
        /// Identifies the IsVertexVisibleProperty dependency property.
        /// </summary>
        public static readonly DependencyProperty IsVertexVisibleProperty = DependencyProperty.Register("IsVertexVisible", typeof(bool), typeof(ConnectorBase), new PropertyMetadata(true, new PropertyChangedCallback(OnIsVertexVisibleChanged)));

        public static readonly DependencyProperty AutoAdjustPointsProperty = DependencyProperty.Register("AutoAdjustPoints", typeof(bool), typeof(ConnectorBase), new PropertyMetadata(false));

        public static readonly DependencyProperty FirstSegmentLengthProperty = DependencyProperty.Register("FirstSegmentLength", typeof(double), typeof(ConnectorBase), new PropertyMetadata(double.NaN));

        public static readonly DependencyProperty LastSegmentLengthProperty = DependencyProperty.Register("LastSegmentLength", typeof(double), typeof(ConnectorBase), new PropertyMetadata(double.NaN));

        /// <summary>
        /// Identifies the BridgeSpacing dependency property.
        /// </summary>
        public static readonly DependencyProperty BridgeSpacingProperty = DependencyProperty.Register("BridgeSpacing", typeof(double), typeof(ConnectorBase), new PropertyMetadata(15d, new PropertyChangedCallback(OnBridgeSpacingChanged)));

        /// <summary>
        /// Identifies the IsVertexMovable dependency property.
        /// </summary>
        public static readonly DependencyProperty IsVertexMovableProperty = DependencyProperty.Register("IsVertexMovable", typeof(bool), typeof(ConnectorBase), new PropertyMetadata(true, new PropertyChangedCallback(OnIsVertexMovableChanged)));

        /// <summary>
        /// Identifies the DecoratorAdornerStyle dependency property.
        /// </summary>
        public static readonly DependencyProperty DecoratorAdornerStyleProperty = DependencyProperty.Register("DecoratorAdorner", typeof(Style), typeof(ConnectorBase), new UIPropertyMetadata(new PropertyChangedCallback(OnTerminalsTemplateChanged)));

        /// <summary>
        /// Identifies the VertexStyle dependency property.
        /// </summary>
        public static readonly DependencyProperty VertexStyleProperty = DependencyProperty.Register("VertexStyle", typeof(Style), typeof(ConnectorBase), new UIPropertyMetadata(new PropertyChangedCallback(OnVertexTemplateChanged)));

        /// <summary>
        /// Identifies the LabelWidth dependency property.
        /// </summary>
        public static readonly DependencyProperty LabelWidthProperty = DependencyProperty.Register("LabelWidth", typeof(double), typeof(ConnectorBase), new FrameworkPropertyMetadata(double.NaN));

        /// <summary>
        /// Identifies the LabelHeight dependency property.
        /// </summary>
        public static readonly DependencyProperty LabelHeightProperty = DependencyProperty.Register("LabelHeight", typeof(double), typeof(ConnectorBase), new FrameworkPropertyMetadata(0d));

        /// <summary>
        /// Identifies the LabelTextWrapping dependency property.
        /// </summary>
        public static readonly DependencyProperty LabelTextWrappingProperty = DependencyProperty.Register("LabelTextWrapping", typeof(TextWrapping), typeof(ConnectorBase), new FrameworkPropertyMetadata(TextWrapping.NoWrap));

        /// <summary>
        /// Identifies the LabelFontSize dependency property.
        /// </summary>
        public static readonly DependencyProperty LabelFontSizeProperty = DependencyProperty.Register("LabelFontSize", typeof(double), typeof(ConnectorBase), new FrameworkPropertyMetadata(11d));

        /// <summary>
        /// Identifies the LabelFontFamily dependency property.
        /// </summary>
        public static readonly DependencyProperty LabelFontFamilyProperty = DependencyProperty.Register("LabelFontFamily", typeof(FontFamily), typeof(ConnectorBase), new FrameworkPropertyMetadata(new FontFamily("Verdana")));

        /// <summary>
        /// Identifies the LabelFontWeight dependency property.
        /// </summary>
        public static readonly DependencyProperty LabelFontWeightProperty = DependencyProperty.Register("LabelFontWeight", typeof(FontWeight), typeof(ConnectorBase), new FrameworkPropertyMetadata(FontWeights.SemiBold));

        /// <summary>
        /// Identifies the LabelFontStyle dependency property.
        /// </summary>
        public static readonly DependencyProperty LabelFontStyleProperty = DependencyProperty.Register("LabelFontStyle", typeof(FontStyle), typeof(ConnectorBase), new FrameworkPropertyMetadata(FontStyles.Normal));

        /// <summary>
        /// Identifies the LabelTextTrimming dependency property.
        /// </summary>
        public static readonly DependencyProperty LabelTextTrimmingProperty = DependencyProperty.Register("LabelTextTrimming", typeof(TextTrimming), typeof(ConnectorBase), new FrameworkPropertyMetadata(TextTrimming.CharacterEllipsis));

        /// <summary>
        /// Identifies the LabelTextAlignment dependency property.
        /// </summary>
        public static readonly DependencyProperty LabelTextAlignmentProperty = DependencyProperty.Register("LabelTextAlignment", typeof(TextAlignment), typeof(ConnectorBase), new FrameworkPropertyMetadata(TextAlignment.Center));

        /// <summary>
        /// Identifies the IsLabelEditable dependency property.
        /// </summary>
        public static readonly DependencyProperty IsLabelEditableProperty = DependencyProperty.Register("IsLabelEditable", typeof(bool), typeof(ConnectorBase), new UIPropertyMetadata(true));

        /// <summary>
        /// Identifies the ConnectionTailPort dependency property.
        /// </summary>
        public static readonly DependencyProperty ConnectionTailPortProperty = DependencyProperty.Register("ConnectionTailPort", typeof(ConnectionPort), typeof(ConnectorBase), new UIPropertyMetadata(null, new PropertyChangedCallback(OnTailPortChanged)));

        /// <summary>
        /// Identifies the ConnectionHeadPort dependency property.
        /// </summary>
        public static readonly DependencyProperty ConnectionHeadPortProperty = DependencyProperty.Register("ConnectionHeadPort", typeof(ConnectionPort), typeof(ConnectorBase), new UIPropertyMetadata(null, new PropertyChangedCallback(OnHeadPortChanged)));

        /// <summary>
        /// Identifies the Label dependency property.
        /// </summary>
        public static readonly DependencyProperty LabelProperty = DependencyProperty.Register("Label", typeof(string), typeof(ConnectorBase), new UIPropertyMetadata(string.Empty, new PropertyChangedCallback(OnLabelChanged)));

        /// <summary>
        /// Identifies the MeasurementUnit dependency property.
        /// </summary>
        public static readonly DependencyProperty MeasurementUnitProperty = DependencyProperty.Register("MeasurementUnit", typeof(MeasureUnits), typeof(ConnectorBase), new PropertyMetadata(MeasureUnits.Pixel, new PropertyChangedCallback(OnUnitsChanged)));

        /// <summary>
        /// Identifies the Label Template.  This is a dependency property.
        /// </summary>
        public static readonly DependencyProperty LabelTemplateProperty = DependencyProperty.Register("LabelTemplate", typeof(DataTemplate), typeof(ConnectorBase));

        /// <summary>
        /// Identifies current TailNode.  This is a dependency property.
        /// </summary>
        public static readonly DependencyProperty TailNodeProperty = DependencyProperty.Register("TailNode", typeof(Node), typeof(ConnectorBase), new UIPropertyMetadata(null, new PropertyChangedCallback(OnTailNodeChanged)));

        /// <summary>
        /// Identifies current HeadNode.  This is a dependency property.
        /// </summary>
        public static readonly DependencyProperty HeadNodeProperty = DependencyProperty.Register("HeadNode", typeof(Node), typeof(ConnectorBase), new UIPropertyMetadata(null, new PropertyChangedCallback(OnHeadNodeChanged)));

        /// <summary>
        /// Identifies current ConnectorType.  This is a dependency property.
        /// </summary>
        public static readonly DependencyProperty ConnectorTypeProperty = DependencyProperty.Register("ConnectorType", typeof(ConnectorType), typeof(ConnectorBase), new PropertyMetadata(ConnectorType.Orthogonal, new PropertyChangedCallback(OnConnectorTypeChanged)));

        /// <summary>
        /// Identifies current IntermediatePoints.  This is a dependency property.
        /// </summary>
        public static readonly DependencyProperty IntermediatePointsProperty = DependencyProperty.Register("IntermediatePoints", typeof(List<Point>), typeof(ConnectorBase), new PropertyMetadata(null, new PropertyChangedCallback(OnIntermediatePointsChanged)));

        /// <summary>
        /// Identifies the LabelVisibility dependency property.
        /// </summary>
        public static readonly DependencyProperty LabelVisibilityProperty = DependencyProperty.Register("LabelVisibility", typeof(Visibility), typeof(ConnectorBase), new UIPropertyMetadata(Visibility.Visible));

        /// <summary>
        /// Identifies the LabelAngle dependency property.
        /// </summary>
        public static readonly DependencyProperty LabelAngleProperty = DependencyProperty.Register("LabelAngle", typeof(double), typeof(ConnectorBase), new UIPropertyMetadata(0d));

        /// <summary>
        /// Identifies the LabelTemplateAngle dependency property.
        /// </summary>
        public static readonly DependencyProperty LabelTemplateAngleProperty = DependencyProperty.Register("LabelTemplateAngle", typeof(double), typeof(ConnectorBase), new UIPropertyMetadata(0d));

        /// <summary>
        /// Identifies the LabelAngle dependency property.
        /// </summary>
        public static readonly DependencyProperty EditorAngleProperty = DependencyProperty.Register("EditorAngle", typeof(double), typeof(ConnectorBase), new UIPropertyMetadata(0d));

        /// <summary>
        /// Identifies the LabelVerticalAlignment dependency property.
        /// </summary>
        public static readonly DependencyProperty LabelVerticalAlignmentProperty = DependencyProperty.Register("LabelVerticalAlignment", typeof(VerticalAlignment), typeof(ConnectorBase), new FrameworkPropertyMetadata(VerticalAlignment.Top));

        /// <summary>
        /// Identifies the LabelHorizontalAlignment dependency property.
        /// </summary>
        public static readonly DependencyProperty LabelHorizontalAlignmentProperty = DependencyProperty.Register("LabelHorizontalAlignment", typeof(HorizontalAlignment), typeof(ConnectorBase), new FrameworkPropertyMetadata(HorizontalAlignment.Center, new PropertyChangedCallback(OnLabelHorizontalAlignmentChanged)));

        /// <summary>
        /// Identifies the LabelVerticalAlignment dependency property.
        /// </summary>
        public static readonly DependencyProperty LabelTemplateVerticalAlignmentProperty = DependencyProperty.Register("LabelTemplateVerticalAlignment", typeof(VerticalAlignment), typeof(ConnectorBase), new FrameworkPropertyMetadata(VerticalAlignment.Top));

        /// <summary>
        /// Identifies the LabelHorizontalAlignment dependency property.
        /// </summary>
        public static readonly DependencyProperty LabelTemplateHorizontalAlignmentProperty = DependencyProperty.Register("LabelTemplateHorizontalAlignment", typeof(HorizontalAlignment), typeof(ConnectorBase), new FrameworkPropertyMetadata(HorizontalAlignment.Center));

        /// <summary>
        /// Defines the TextWidth property.This is a dependency property.
        /// </summary>
        public static readonly DependencyProperty TextWidthProperty = DependencyProperty.Register("TextWidth", typeof(double), typeof(ConnectorBase), new FrameworkPropertyMetadata(0d, new PropertyChangedCallback(OnTextWidthChanged)));

        /// <summary>
        /// Identifies the LabelPosition dependency property.
        /// </summary>
        public static readonly DependencyProperty LabelPositionProperty = DependencyProperty.Register("LabelPosition", typeof(Point), typeof(ConnectorBase), new FrameworkPropertyMetadata(new Point(0, 0), new PropertyChangedCallback(OnLabelPositionChanged)));

        /// <summary>
        /// Identifies the LabelTemplatePosition dependency property.
        /// </summary>
        public static readonly DependencyProperty LabelTemplatePositionProperty = DependencyProperty.Register("LabelTemplatePosition", typeof(Point), typeof(ConnectorBase), new FrameworkPropertyMetadata(new Point(0, 0), new PropertyChangedCallback(OnLabelTemplatePositionChanged)));

        /// <summary>
        /// Identifies the Distance dependency property.
        /// </summary>
        public static readonly DependencyProperty DistanceProperty = DependencyProperty.Register("Distance", typeof(double), typeof(ConnectorBase), new FrameworkPropertyMetadata(0d));

        /// <summary>
        /// Identifies the LabelTemplateDistance dependency property.
        /// </summary>
        public static readonly DependencyProperty LabelTemplateDistanceProperty = DependencyProperty.Register("LabelTemplateDistance", typeof(double), typeof(ConnectorBase), new FrameworkPropertyMetadata(0d));

        /// <summary>
        /// Identifies the Center dependency property.
        /// </summary>
        public static readonly DependencyProperty CenterProperty = DependencyProperty.Register("Center", typeof(Point), typeof(ConnectorBase), new FrameworkPropertyMetadata(new Point(0, 0)));

        /// <summary>
        ///  Identifies the LabelBackground dependency property.
        /// </summary>
        public static readonly DependencyProperty LabelBackgroundProperty = DependencyProperty.Register("LabelBackground", typeof(Brush), typeof(ConnectorBase), new FrameworkPropertyMetadata(Brushes.Transparent));

        /// <summary>
        ///  Identifies the LabelForeground dependency property.
        /// </summary>
        public static readonly DependencyProperty LabelForegroundProperty = DependencyProperty.Register("LabelForeground", typeof(Brush), typeof(ConnectorBase), new FrameworkPropertyMetadata(Brushes.Black));

        /// <summary>
        /// Identifies the IsGroup dependency property.
        /// </summary>
        public static readonly DependencyProperty IsGroupedProperty = DependencyProperty.Register("IsGrouped", typeof(bool), typeof(ConnectorBase));

        /// <summary>
        /// Identifies the ParentId dependency property.
        /// </summary>
        public static readonly DependencyProperty ParentIDProperty = DependencyProperty.Register("ParentID", typeof(Guid), typeof(ConnectorBase));
        public static readonly DependencyProperty IsSelectedProperty = DependencyProperty.Register("IsSelected", typeof(bool), typeof(LineConnector), new FrameworkPropertyMetadata(false, new PropertyChangedCallback(OnIsSelectedChanged)));
        public static readonly DependencyProperty EnableMultilineLabelProperty = DependencyProperty.Register("EnableMultilineLabel", typeof(bool), typeof(ConnectorBase), new PropertyMetadata(false));

        #endregion

        #region Events

        //bool m_ApplyingStyle = false;



        private static void OnIsSelectedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

            LineConnector n = d as LineConnector;
            n.ConnectorSelection();
        }



        private static void OnCustomPathStyleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //ConnectorBase cb = d as ConnectorBase;
            //if (!cb.m_ApplyingStyle)
            //{
            //    cb.m_ApplyingStyle = true;
            //    if ((d as LineConnector).IsLoaded)
            //    {
            //        System.Windows.Data.Binding bin = new System.Windows.Data.Binding("LineStyle.Stroke");
            //        bin.RelativeSource = new System.Windows.Data.RelativeSource(System.Windows.Data.RelativeSourceMode.FindAncestor, typeof(LineConnector), 10);
            //        bin.Mode = System.Windows.Data.BindingMode.TwoWay;
            //        //bin.Source=(e.NewValue as Style);
            //        //(d as LineConnector).SetBinding(LineConnector.LineStyleProperty,bin);

            //        //Setter Stroke = new Setter(Path.StrokeProperty, Brushes.Green);
            //        //(e.NewValue as Style).Setters.Add(Stroke);


            //        Style resStyle = (((d as LineConnector).Template.FindName("PART_ConnectionPath", (d as LineConnector)) as Path).Resources["setBind"] as Style);
            //        Style modStyle = new Style();
            //        foreach (Setter x in resStyle.Setters)
            //        {
            //            modStyle.Setters.Add(x);
            //        }
            //        foreach (Setter s in (e.NewValue as Style).Setters)
            //        {
            //            for (int i = 0; i < modStyle.Setters.Count; i++)
            //            {
            //                Setter m = modStyle.Setters[i] as Setter;
            //                if (s.Property.Name.Equals(m.Property.Name))
            //                {
            //                    modStyle.Setters.Remove(m);
            //                    break;
            //                }
            //            }
            //        }
            //        foreach (Setter s in (e.NewValue as Style).Setters)
            //        {
            //            modStyle.Setters.Add(s);
            //        }
            //        //modStyle.Setters.Add(Stroke);
            //        (d as LineConnector).CustomPathStyle = modStyle;
            //    }
            //}
            //cb.m_ApplyingStyle = false;
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
        /// Called when [bridge spacing changed].
        /// </summary>
        /// <param name="d">The d.</param>
        /// <param name="e">The <see cref="System.Windows.DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void OnBridgeSpacingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
        }

        /// <summary>
        /// Called when [line bridging enabled changed].
        /// </summary>
        /// <param name="d">The d.</param>
        /// <param name="e">The <see cref="System.Windows.DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void OnLineBridgingEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            LineConnector l = d as LineConnector;
            if (l != null && l.bridged)
            {
                l.UpdateConnectorPathGeometry();
                if (l.dc != null && l.dc.View != null && l.dc.View.Page != null)
                {
                    l.dc.View.Page.InvalidateMeasure();
                }
            }
        }

        /// <summary>
        /// Called when [is vertex visible changed].
        /// </summary>
        /// <param name="d">The d.</param>
        /// <param name="e">The <see cref="System.Windows.DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void OnIsVertexVisibleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d != null && d is LineConnector && (d as LineConnector).LineAdorner is LineConnectorAdorner)
            {
                ((d as LineConnector).LineAdorner as LineConnectorAdorner).InvalidateVertexs();
            }
        }


        /// <summary>
        /// Called when [is vertex movable changed].
        /// </summary>
        /// <param name="d">The d.</param>
        /// <param name="e">The <see cref="System.Windows.DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void OnIsVertexMovableChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d != null && d is LineConnector && (d as LineConnector).LineAdorner is LineConnectorAdorner)
            {
                ((d as LineConnector).LineAdorner as LineConnectorAdorner).InvalidateVertexs();
            }
        }


        /// <summary>
        /// Called when [terminals template changed].
        /// </summary>
        /// <param name="d">The d.</param>
        /// <param name="e">The <see cref="System.Windows.DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void OnTerminalsTemplateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ConnectorBase line = d as ConnectorBase;
            // line.UpdateConnectorPathGeometry();
            if (line.LineAdorner != null)
            {
                (line.LineAdorner as LineConnectorAdorner).updateThumbStyle();
            }
        }

        /// <summary>
        /// Called when [vertex template changed].
        /// </summary>
        /// <param name="d">The d.</param>
        /// <param name="e">The <see cref="System.Windows.DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void OnVertexTemplateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ConnectorBase line = d as ConnectorBase;
            if (line.LineAdorner != null)
            {
                (line.LineAdorner as LineConnectorAdorner).InvalidateVertexs();
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
        /// Called when [units changed].
        /// </summary>
        /// <param name="d">The DependencyObject.</param>
        /// <param name="e">The <see cref="System.Windows.DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void OnUnitsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            LineConnector n = d as LineConnector;
            if (n._FirstLoaded)
            {
                n.StartPointPosition = MeasureUnitsConverter.Convert(n.StartPointPosition, (MeasureUnits)e.OldValue, (MeasureUnits)e.NewValue);
                n.EndPointPosition = MeasureUnitsConverter.Convert(n.EndPointPosition, (MeasureUnits)e.OldValue, (MeasureUnits)e.NewValue);
                n.ConnectionEndSpace = MeasureUnitsConverter.Convert(n.ConnectionEndSpace, (MeasureUnits)e.OldValue, (MeasureUnits)e.NewValue);
            }
            else if(n.ConnectionEndSpace == 6)
            {
                n.ConnectionEndSpace = MeasureUnitsConverter.Convert(n.ConnectionEndSpace, (MeasureUnits)e.OldValue, (MeasureUnits)e.NewValue);
            }
            //if (!lc.StartPointPosition.Equals(new Point(0, 0)))
            //{
            //    lc.StartPointPosition = MeasureUnitsConverter.ToPixels(lc.StartPointPosition, (MeasureUnits)e.OldValue);
            //    lc.StartPointPosition = MeasureUnitsConverter.FromPixels(lc.StartPointPosition, (MeasureUnits)e.NewValue);
            //}
            //if (!lc.EndPointPosition.Equals(new Point(0, 0)))
            //{
            //    lc.EndPointPosition = MeasureUnitsConverter.ToPixels(lc.EndPointPosition, (MeasureUnits)e.OldValue);
            //    lc.EndPointPosition = MeasureUnitsConverter.FromPixels(lc.EndPointPosition, (MeasureUnits)e.NewValue);
            //}

            ConnectorBase b = (ConnectorBase)d;
            LineConnector lc = (LineConnector)b;
            for (int i = 0; i < lc.IntermediatePoints.Count; i++)
            {
                //lc.IntermediatePoints[i] = MeasureUnitsConverter.ToPixels(lc.IntermediatePoints[i], (MeasureUnits)e.OldValue);
                //lc.IntermediatePoints[i] = MeasureUnitsConverter.FromPixels(lc.IntermediatePoints[i], (MeasureUnits)e.NewValue);
            }
        }

        /// <summary>
        /// Called when [label horizontal alignment changed].
        /// </summary>
        /// <param name="d">The d.</param>
        /// <param name="e">The <see cref="System.Windows.DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void OnLabelHorizontalAlignmentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as LineConnector).UpdateConnectorPathGeometry();
        }
        /// <summary>
        /// Called when [label position changed].
        /// </summary>
        /// <param name="d">The DependencyObject.</param>
        /// <param name="e">The <see cref="System.Windows.DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void OnLabelPositionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
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
        /// Called when [text width changed].
        /// </summary>
        /// <param name="d">The DependencyObject.</param>
        /// <param name="e">The <see cref="System.Windows.DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void OnTextWidthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //ConnectorBase cbase = d as ConnectorBase;
            //Point p, tp;
            //if (cbase.ConnectorPathGeometry != null)
            //{
            //    cbase.ConnectorPathGeometry.GetPointAtFractionLength(.5, out p, out tp);
            //}
        }

        private static void OnIntermediatePointsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ConnectorBase cbase = d as ConnectorBase;
            if (e.NewValue is string)
            {

            }
        }

        /// <summary>
        /// Called when [connector type changed].
        /// </summary>
        /// <param name="d">The d.</param>
        /// <param name="e">The <see cref="System.Windows.DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void OnConnectorTypeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue is ConnectorType && e.NewValue is ConnectorType)
            {
                if (e.OldValue.Equals(ConnectorType.Orthogonal) && e.NewValue.Equals(ConnectorType.Straight))
                {
                    (d as LineConnector).IntermediatePoints = new List<Point>();

                }
                if (((d as LineConnector).LineAdorner as LineConnectorAdorner) != null)
                    ((d as LineConnector).LineAdorner as LineConnectorAdorner).InvalidateVertexs();
            }
            (d as LineConnector).UpdateConnectorPathGeometry();
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
            cbase.HeadNode = (IShape)e.NewValue;
            if (dview != null && dview.UndoRedoEnabled && !dview.Undone && !dview.IsLayout)
            {
                if (!(dview.undo || dview.redo))
                {
                    dview.RedoStack.Clear();
                }
                if ((Node)e.OldValue != null)
                {
                    dview.UndoStack.Push((Node)e.OldValue);
                    dview.UndoStack.Push("Headend");
                    dview.UndoStack.Push(cbase as LineConnector);
                    dview.UndoStack.Push("Dragged");
                }
                else
                {
                    //dview.UndoStack.Push(MeasureUnitsConverter.ToPixels((cbase as LineConnector).StartPointPosition, (dview.Page as DiagramPage).MeasurementUnits));
                    dview.UndoStack.Push((cbase as LineConnector).PxStartPointPosition);
                    dview.UndoStack.Push("Headend");
                    dview.UndoStack.Push(cbase as LineConnector);
                    dview.UndoStack.Push("Dragged");
                }
            }

            if (cbase.HeadNode != null)
            {
                (cbase as LineConnector).InternalHeadShape = cbase.HeadDecoratorShape;
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

                newEventArgs.RoutedEvent = DiagramView.HeadNodeChangedEvent;
                cbase.RaiseEvent(newEventArgs);
                cbase.HeadNode.OutEdges.Add(cbase);
                cbase.HeadNode.Edges.Add(cbase);
                (cbase.HeadNode as Node).PropertyChanged += new PropertyChangedEventHandler(cbase.Line_PropertyChanged);
            }
            else
            {
                (cbase as LineConnector).HeadNodeReferenceNo = -1;
                (e.OldValue as Node).PropertyChanged -= new PropertyChangedEventHandler(cbase.Line_PropertyChanged);
            }

            if (e.OldValue != null)
            {
                Node oldNode = e.OldValue as Node;
                oldNode.Edges.Remove(cbase);
                oldNode.OutEdges.Remove(cbase);
            }
            cbase.UpdateConnectorPathGeometry();
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
            if (dview != null && dview.UndoRedoEnabled && !dview.Undone && !dview.IsLayout)
            {
                if (!(dview.undo || dview.redo))
                {
                    dview.RedoStack.Clear();
                }
                if ((Node)e.OldValue != null)
                {
                    dview.UndoStack.Push((Node)e.OldValue);
                    dview.UndoStack.Push("Tailend");
                    dview.UndoStack.Push(cbase as LineConnector);
                    dview.UndoStack.Push("Dragged");
                }
                else
                {
                    dview.UndoStack.Push((cbase as LineConnector).PxEndPointPosition);
                    dview.UndoStack.Push("Tailend");
                    dview.UndoStack.Push(cbase as LineConnector);
                    dview.UndoStack.Push("Dragged");
                }
            }

            if (cbase.TailNode != null)
            {
                (cbase as LineConnector).InternalTailShape = cbase.TailDecoratorShape;
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

                newEventArgs.RoutedEvent = DiagramView.TailNodeChangedEvent;
                cbase.RaiseEvent(newEventArgs);
                cbase.TailNode.InEdges.Add(cbase);
                cbase.TailNode.Edges.Add(cbase);
                (cbase.TailNode as Node).PropertyChanged += new PropertyChangedEventHandler(cbase.Line_PropertyChanged);
            }
            else
            {
                (cbase as LineConnector).TailNodeReferenceNo = -1;
                (e.OldValue as Node).PropertyChanged -= new PropertyChangedEventHandler(cbase.Line_PropertyChanged);
            }

            if (e.OldValue != null)
            {
                Node oldNode = e.OldValue as Node;
                oldNode.Edges.Remove(cbase);
                oldNode.InEdges.Remove(cbase);
            }

            cbase.UpdateConnectorPathGeometry();
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

            newEventArgs.RoutedEvent = DiagramView.ConnectorLabelChangedEvent;
            cbase.RaiseEvent(newEventArgs);
            if ((d as LineConnector) != null)
            {
                (d as LineConnector).UpdateDecoratorPosition();
            }
#if SyncfusionFramework3_5
            if ((cbase as LineConnector).editor != null)
            {
                System.Windows.Data.BindingExpression expression = (cbase as LineConnector).editor.GetBindingExpression(LabelEditor.LabelProperty);
                if (expression != null)
                {
                    expression.UpdateTarget();
                }
            }
#endif
        }

        /// <summary>
        /// Handles the PropertyChanged event of the Line control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.ComponentModel.PropertyChangedEventArgs"/> instance containing the event data.</param>
        protected virtual void Line_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
        }
        #endregion

        #region Virtual Methods

        /// <summary>
        /// Updates the line geometry.
        /// </summary>
        public virtual void UpdateConnectorPathGeometry()
        {
        }

        /// <summary>
        /// internal properties
        /// </summary>
        Point startPt, centerPt, endPt, tangentAtStartPoint, tangentAtCenterPoint, tangentAtEndPoint;

        /// <summary>
        /// internal properties
        /// </summary>
        double textWidth, templateWidth;

        /// <summary>
        /// internal properties
        /// </summary>
        List<double> length = new List<double>();

        /// <summary>
        /// internal properties
        /// </summary>
        List<Point> pts = new List<Point>();

        /// <summary>
        /// Updates the position of the decorator.
        /// </summary>
        internal void UpdateDecoratorPosition()
        {
            if (this.ConnectorPathGeometry == null || this.ConnectorPathGeometry.Bounds == Rect.Empty)
            {
                return;
            }
            if (LabelTemplate != null && LabelTemplate.LoadContent() != null && LabelTemplate.LoadContent() is UIElement)
            {
                if (this.IsLoaded)
                {
                    object o = this.Template.FindName("PART_ConnectorLabelTemplateEditor", this);
                    if (o != null && o is FrameworkElement)
                    {
                        (o as FrameworkElement).Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                        templateWidth = (o as ContentPresenter).DesiredSize.Width;
                    }
                }
            }

            if (this.ConnectorPathGeometry.Bounds.Size.Height != 0 || this.ConnectorPathGeometry.Bounds.Size.Width != 0)
            {
                this.ConnectorPathGeometry.GetPointAtFractionLength(0, out startPt, out tangentAtStartPoint);
                this.ConnectorPathGeometry.GetPointAtFractionLength(0.5, out centerPt, out tangentAtCenterPoint);
                this.ConnectorPathGeometry.GetPointAtFractionLength(1, out endPt, out tangentAtEndPoint);
                if (ConnectorType == ConnectorType.Orthogonal || ConnectorType == ConnectorType.Straight)
                {
                    try
                    {
                        //String strPoints = this.ConnectorPathGeometry.ToString();
                        //strPoints = strPoints.Replace('M', ' ');
                        //strPoints = strPoints.Replace('L', ' ');
                        //StringToPoints stp = new StringToPoints();
                        //pts = (List<Point>)stp.Convert(strPoints, typeof(List<Point>), null, null);
                        if (IntermediatePoints != null && IntermediatePoints.Count > 0)
                        {
                            pts = getLinePts();
                        }
                        else if (this.ConnectorPathGeometry != null && this.ConnectorPathGeometry.Figures.Count > 0)
                        {
                            //pts = LineConnector.getLinePts(this.ConnectorPathGeometry.Figures[0]);
                            if (this.VirtualConnectorPathGeometry != null)
                            {
                                pts = LineConnector.getLinePts(this.VirtualConnectorPathGeometry.Figures[0]);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        e.ToString();
                    }
                }
                if (this.ConnectorType == ConnectorType.Straight || this.ConnectorType == ConnectorType.Orthogonal)
                {
                    if (IntermediatePoints != null && IntermediatePoints.Count >= 1)
                    {
                        length = findLength(pts);
                        //double mid = length[length.Count - 1] / 2d;
                        //for (int i = 0; i < length.Count; i++)
                        //{
                        //    if (mid < length[i])
                        //    {
                        //        midIndex = i;
                        //        break;
                        //    }
                        //}
                    }
                }

            }
            else
            {
                return;
            }
            HeadDecoratorAngle = Math.Atan2(-tangentAtStartPoint.Y, -tangentAtStartPoint.X) * (180 / Math.PI);
            TailDecoratorAngle = Math.Atan2(tangentAtEndPoint.Y, tangentAtEndPoint.X) * (180 / Math.PI);
            HeadDecoratorPosition = startPt;
            TailDecoratorPosition = endPt;
            if (this.GetTemplateChild("PART_HeadDecoratorAnchorPath") as Path != null)
            {
                TranslateTransform tth = new TranslateTransform(-(this.GetTemplateChild("PART_HeadDecoratorAnchorPath") as Path).ActualWidth + 2, -(this.GetTemplateChild("PART_HeadDecoratorAnchorPath") as Path).ActualHeight / 2);
                RotateTransform headAngle = new RotateTransform(this.HeadDecoratorAngle, 0, 0);
                TransformGroup tthg = new TransformGroup();
                tthg.Children.Add(tth);
                tthg.Children.Add(headAngle);
                (this.GetTemplateChild("PART_HeadDecoratorAnchorPath") as Path).RenderTransform = tthg;
            }
            if (this.GetTemplateChild("PART_SinkAnchorPath") as Path != null)
            {
                TranslateTransform ttt = new TranslateTransform(-(this.GetTemplateChild("PART_SinkAnchorPath") as Path).ActualWidth + 2, -(this.GetTemplateChild("PART_SinkAnchorPath") as Path).ActualHeight / 2);
                RotateTransform tailAngle = new RotateTransform(this.TailDecoratorAngle, 0, 0);
                TransformGroup tttg = new TransformGroup();
                tttg.Children.Add(ttt);
                tttg.Children.Add(tailAngle);
                (this.GetTemplateChild("PART_SinkAnchorPath") as Path).RenderTransform = tttg;
            }
            double len = GetPathFigureLength(this.ConnectorPathGeometry.Figures[0]);
            this.ConnectorPathGeometry.GetPointAtFractionLength(10 / len, out startPt, out tangentAtStartPoint);
            this.ConnectorPathGeometry.GetPointAtFractionLength(0.5, out centerPt, out tangentAtCenterPoint);
            len = (len - 10) / len;
            this.ConnectorPathGeometry.GetPointAtFractionLength(len, out endPt, out tangentAtEndPoint);

            TextBlock tb = new TextBlock();
            tb.Text = Label;
            tb.FontFamily = LabelFontFamily;
            tb.FontSize = LabelFontSize;
            tb.FontStyle = LabelFontStyle;
            tb.FontWeight = LabelFontWeight;
            tb.TextWrapping = LabelTextWrapping;
            //if (tb.TextWrapping == TextWrapping.Wrap || tb.TextWrapping == TextWrapping.WrapWithOverflow)
            //{
            //    tb.Width = LabelWidth;
            //}
            //tb.Width = LabelWidth;
            tb.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            textWidth = tb.DesiredSize.Width + 10;
            if (textWidth > LabelWidth)
            {
                textWidth = LabelWidth;
            }
            //pts.Clear();
            if (ConnectorType == ConnectorType.Orthogonal || ConnectorType == ConnectorType.Straight)
            {
                if (pts.Count < 2)
                {
                    return;
                }
            }
            if (ConnectorType == ConnectorType.Straight)
            {
                doStraightPosition();
            }
            else if (ConnectorType == ConnectorType.Bezier || ConnectorType == ConnectorType.Orthogonal)
            {
                switch (LabelHorizontalAlignment)
                {
                    case HorizontalAlignment.Left:
                        LabelPosition = startPt;
                        break;
                    case HorizontalAlignment.Center:
                        LabelPosition = new Point(centerPt.X - textWidth / 2, centerPt.Y);
                        break;
                    case HorizontalAlignment.Right:
                        LabelPosition = endPt;
                        break;
                }
                switch (LabelTemplateHorizontalAlignment)
                {
                    case HorizontalAlignment.Left:
                        LabelTemplatePosition = startPt;
                        break;
                    case HorizontalAlignment.Center:
                        LabelTemplatePosition = new Point(centerPt.X - templateWidth / 2, centerPt.Y);
                        break;
                    case HorizontalAlignment.Right:
                        LabelTemplatePosition = endPt;
                        break;
                }
                if (ConnectorType == ConnectorType.Orthogonal)
                {
                    if ((pts[0].Y > pts[1].Y && LabelPosition.Y == startPt.Y) || (pts[pts.Count - 1].Y > pts[pts.Count - 2].Y && LabelPosition.Y == endPt.Y))
                    {
                        if ((this as LineConnector).editor != null)
                        {
                            LabelPosition = new Point(LabelPosition.X, LabelPosition.Y - (this as LineConnector).editor.DesiredSize.Height);

                            if (LabelPosition.Y < this.ConnectorPathGeometry.Bounds.Top)
                            {
                                LabelPosition = new Point(LabelPosition.X, this.ConnectorPathGeometry.Bounds.Top);
                            }
                        }
                    }
                    if ((pts[0].Y > pts[1].Y && LabelTemplatePosition.Y == startPt.Y) || (pts[pts.Count - 1].Y > pts[pts.Count - 2].Y && LabelTemplatePosition.Y == endPt.Y))
                    {
                        if ((this as LineConnector).LabelTemplate != null)
                        {
                            object o = (this as LineConnector).LabelTemplate.LoadContent();
                            if (o is UIElement)
                            {
                                (o as FrameworkElement).Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                                LabelTemplatePosition = new Point(LabelTemplatePosition.X, LabelTemplatePosition.Y - (o as FrameworkElement).DesiredSize.Height);
                                if (LabelPosition.Y < this.ConnectorPathGeometry.Bounds.Top)
                                {
                                    LabelTemplatePosition = new Point(LabelTemplatePosition.X, this.ConnectorPathGeometry.Bounds.Top);
                                }
                            }
                        }
                    }
                }
                else
                {
                    Point s, e;
                    this.ConnectorPathGeometry.GetPointAtFractionLength(0, out s, out tangentAtStartPoint);
                    this.ConnectorPathGeometry.GetPointAtFractionLength(1, out e, out tangentAtStartPoint);
                    if (((LabelPosition.Y == startPt.Y) && s.Y > e.Y) || ((LabelPosition.Y == endPt.Y) && e.Y > s.Y))
                    {
                        if ((this as LineConnector).editor != null)
                        {
                            LabelPosition = new Point(LabelPosition.X, LabelPosition.Y - (this as LineConnector).editor.DesiredSize.Height);

                            if (LabelPosition.Y < this.ConnectorPathGeometry.Bounds.Top)
                            {
                                LabelPosition = new Point(LabelPosition.X, this.ConnectorPathGeometry.Bounds.Top);
                            }
                        }
                    }
                    if (((LabelTemplatePosition.Y == startPt.Y) && s.Y > e.Y) || ((LabelTemplatePosition.Y == endPt.Y) && e.Y > s.Y))
                    {
                        if ((this as LineConnector).LabelTemplate != null)
                        {
                            object o = (this as LineConnector).LabelTemplate.LoadContent();
                            if (o is UIElement)
                            {
                                (o as FrameworkElement).Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                                LabelTemplatePosition = new Point(LabelTemplatePosition.X, LabelTemplatePosition.Y - (o as FrameworkElement).DesiredSize.Height);
                                if (LabelPosition.Y < this.ConnectorPathGeometry.Bounds.Top)
                                {
                                    LabelTemplatePosition = new Point(LabelTemplatePosition.X, this.ConnectorPathGeometry.Bounds.Top);
                                }
                            }
                        }
                    }
                }
                if (LabelPosition.X + textWidth > this.ConnectorPathGeometry.Bounds.Right)
                {
                    LabelPosition = new Point(this.ConnectorPathGeometry.Bounds.Right - textWidth, LabelPosition.Y);
                    if (LabelPosition.X < this.ConnectorPathGeometry.Bounds.Left)
                    {
                        LabelPosition = new Point(this.ConnectorPathGeometry.Bounds.Left, LabelPosition.Y);
                    }
                }
                if (LabelTemplatePosition.X + templateWidth > this.ConnectorPathGeometry.Bounds.Right)
                {
                    LabelTemplatePosition = new Point(this.ConnectorPathGeometry.Bounds.Right - templateWidth, LabelTemplatePosition.Y);
                    if (LabelTemplatePosition.X < this.ConnectorPathGeometry.Bounds.Left)
                    {
                        LabelTemplatePosition = new Point(this.ConnectorPathGeometry.Bounds.Left, LabelTemplatePosition.Y);
                    }
                }
                Distance = this.ConnectorPathGeometry.Bounds.Width;
                if (Distance < 50)
                {
                    Distance = 50;
                }
            }
        }

        /// <summary>
        /// Gets the length at fraction point.
        /// </summary>
        /// <param name="pathFigure">The path figure.</param>
        /// <param name="at">At.</param>
        /// <param name="fullLength">The full length.</param>
        /// <param name="segmentIndex">Index of the segment.</param>
        /// <returns></returns>
        internal double GetLengthAtFractionPoint(PathFigure pathFigure, Point at, out double fullLength, out int segmentIndex)
        {
            double confirm = 100d;
            fullLength = 0;
            segmentIndex = -1;
            int count = 0;
            double LengthAtFractionPoint = 0;
            if (pathFigure == null)
            {
                return 0;
            }
            bool isAlreadyFlattened = true;

            foreach (PathSegment pathSegment in pathFigure.Segments)
            {
                if (!(pathSegment is PolyLineSegment) && !(pathSegment is LineSegment))
                {
                    isAlreadyFlattened = false;
                    break;
                }
            }

            PathFigure pathFigureFlattened = isAlreadyFlattened ? pathFigure : pathFigure.GetFlattenedPathFigure();
            Point pt1 = pathFigureFlattened.StartPoint;
            Point previouspt2 = pt1;

            foreach (PathSegment pathSegment in pathFigureFlattened.Segments)
            {
                if (pathSegment is LineSegment)
                {
                    Point pt2 = (pathSegment as LineSegment).Point;
                    double suspect = getSlope(pt2, pt1, at);
                    if (suspect < confirm)
                    {
                        confirm = suspect;
                        LengthAtFractionPoint = fullLength + (at - previouspt2).Length;
                        segmentIndex = count;
                    }
                    previouspt2 = pt2;
                    fullLength += (pt2 - pt1).Length;
                    pt1 = pt2;
                }
                else if (pathSegment is PolyLineSegment)
                {
                    PointCollection pointCollection = (pathSegment as PolyLineSegment).Points;
                    foreach (Point pt2 in pointCollection)
                    {
                        double suspect = getSlope(pt2, pt1, at);
                        if (suspect < confirm)
                        {
                            confirm = suspect;
                            LengthAtFractionPoint = fullLength + (at - previouspt2).Length;
                            segmentIndex = count;
                        }
                        fullLength += (pt2 - pt1).Length;
                        pt1 = pt2;
                        previouspt2 = pt2;
                    }
                }
                count++;
            }
            return LengthAtFractionPoint;
        }

        /// <summary>
        /// Gets the slope.
        /// </summary>
        /// <param name="st">The st.</param>
        /// <param name="en">The en.</param>
        /// <param name="point">The point.</param>
        /// <returns></returns>
        internal double getSlope(Point st, Point en, Point point)
        {
            double three = 3.0;// MeasureUnitsConverter.FromPixels(3.0, this.MeasurementUnit);
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
                        return 10000d;
                    }
                    else if (((st.Y > point.Y) && (point.Y > en.Y)) || ((st.Y < point.Y) && (point.Y < en.Y)))
                    {
                        return Math.Abs(st.X - point.X);
                    }
                    else
                    {
                        return 10000d;
                    }
                }
                else if (st.Y == en.Y)
                {
                    if (((st.X > point.X) && (point.X > en.X)) || ((st.X < point.X) && (point.X < en.X)))
                    {
                        return Math.Abs(st.Y - point.Y);
                    }
                    else
                    {
                        return 10000d;
                    }
                }
                else
                {
                    return 10000d;
                }
            }
            else if (ConnectorType != ConnectorType.Orthogonal)
            {
                if ((st.X >= point.X && point.X >= en.X) || (st.X <= point.X && point.X <= en.X) || delx < three)
                {
                    if ((st.Y >= point.Y && point.Y >= en.Y) || (st.Y <= point.Y && point.Y <= en.Y) || dely < three)
                    {
                        return Math.Abs(lhs - rhs);
                    }
                    else
                    {
                        return 10000d;
                    }
                }
                else
                {
                    return 10000d;
                }
            }
            else
            {
                return 10000d;
            }
        }

        /// <summary>
        /// Gets the length of the path figure.
        /// </summary>
        /// <param name="pathFigure">The path figure.</param>
        /// <returns></returns>
        public double GetPathFigureLength(PathFigure pathFigure)
        {
            if (pathFigure == null)
                return 0;

            bool isAlreadyFlattened = true;

            foreach (PathSegment pathSegment in pathFigure.Segments)
            {
                if (!(pathSegment is PolyLineSegment) && !(pathSegment is LineSegment))
                {
                    isAlreadyFlattened = false;
                    break;
                }
            }

            PathFigure pathFigureFlattened = isAlreadyFlattened ? pathFigure : pathFigure.GetFlattenedPathFigure();
            double length = 0;
            Point pt1 = pathFigureFlattened.StartPoint;

            foreach (PathSegment pathSegment in pathFigureFlattened.Segments)
            {
                if (pathSegment is LineSegment)
                {
                    Point pt2 = (pathSegment as LineSegment).Point;
                    length += (pt2 - pt1).Length;
                    pt1 = pt2;
                }
                else if (pathSegment is PolyLineSegment)
                {
                    PointCollection pointCollection = (pathSegment as PolyLineSegment).Points;
                    foreach (Point pt2 in pointCollection)
                    {
                        length += (pt2 - pt1).Length;
                        pt1 = pt2;
                    }
                }
            }
            return length;
        }

        /// <summary>
        /// Does the straight position.
        /// </summary>
        private void doStraightPosition()
        {
            switch (LabelHorizontalAlignment)
            {
                case HorizontalAlignment.Left:
                    LabelPosition = startPt;
                    LabelAngle = findAngle(pts[0], pts[1]);
                    if (LabelAngle > 90 && LabelAngle < 270)
                    {
                        Point st;
                        double len = findHypo(pts[0], pts[1]);
                        if (length.Count >= 1)
                        {
                            len = length[length.Count - 1];
                        }
                        double div = 1 - ((len - textWidth) / len);
                        this.ConnectorPathGeometry.GetPointAtFractionLength(div, out st, out tangentAtEndPoint);
                        LabelPosition = st;
                        LabelAngle = LabelAngle + 180;
                    }
                    break;
                case HorizontalAlignment.Center:
                    if (IntermediatePoints == null || IntermediatePoints.Count < 1)
                    {
                        LabelAngle = findAngle(pts[0], pts[1]);
                        if (LabelAngle > 90 && LabelAngle < 270)
                        {
                            Point st;
                            double len = findHypo(pts[0], pts[1]);
                            if (length.Count >= 1)
                            {
                                len = length[length.Count - 1];
                            }
                            double div = 1 - ((len / 2 - textWidth / 2) / len);
                            this.ConnectorPathGeometry.GetPointAtFractionLength(div, out st, out tangentAtEndPoint);
                            LabelPosition = st;
                            LabelAngle = LabelAngle + 180;
                        }
                        else
                        {
                            Point st;
                            double len = findHypo(pts[0], pts[1]);
                            if (length.Count >= 1)
                            {
                                len = length[length.Count - 1];
                            }
                            double div = ((len / 2 - textWidth / 2) / len);
                            this.ConnectorPathGeometry.GetPointAtFractionLength(div, out st, out tangentAtEndPoint);
                            LabelPosition = st;
                        }
                    }
                    else
                    {
                        Point mid;
                        double len = 0;
                        if (length.Count >= 1)
                        {
                            len = length[length.Count - 1];
                        }
                        //double div = ((len / 2 - textWidth) / len);
                        this.ConnectorPathGeometry.GetPointAtFractionLength(0.5, out mid, out tangentAtEndPoint);
                        LabelPosition = mid;
                        int midIndex = findCenterIntermediatePoints(mid);
                        if (pts.Count > midIndex + 1 && midIndex >= 0)
                        {
                            LabelAngle = findAngle(pts[midIndex], pts[midIndex + 1]);
                        }
                        if (LabelAngle > 90 && LabelAngle < 270)
                        {
                            LabelAngle = LabelAngle + 180;
                            //LabelPosition = pts[midIndex + 1];
                        }
                    }
                    break;
                case HorizontalAlignment.Right:
                    LabelPosition = endPt;
                    LabelAngle = findAngle(pts[pts.Count - 2], pts[pts.Count - 1]);
                    if (LabelAngle > 90 && LabelAngle < 270)
                    {
                        LabelAngle = LabelAngle + 180;
                    }
                    else
                    {
                        Point end;
                        double len = findHypo(pts[pts.Count - 2], pts[pts.Count - 1]);
                        if (length.Count >= 1)
                        {
                            len = length[length.Count - 1];
                        }
                        double div = ((len - textWidth) / len);
                        this.ConnectorPathGeometry.GetPointAtFractionLength(div, out end, out tangentAtEndPoint);
                        LabelPosition = end;
                    }
                    break;
            }
            switch (LabelTemplateHorizontalAlignment)
            {
                case HorizontalAlignment.Left:
                    LabelTemplatePosition = startPt;
                    LabelTemplateAngle = findAngle(pts[0], pts[1]);
                    if (LabelTemplateAngle > 90 && LabelTemplateAngle < 270)
                    {
                        Point st;
                        double len = findHypo(pts[0], pts[1]);
                        if (length.Count >= 1)
                        {
                            len = length[length.Count - 1];
                        }
                        double div = 1 - ((len - templateWidth) / len);
                        this.ConnectorPathGeometry.GetPointAtFractionLength(div, out st, out tangentAtEndPoint);
                        LabelTemplatePosition = st;
                        LabelTemplateAngle = LabelTemplateAngle + 180;
                    }
                    break;
                case HorizontalAlignment.Center:
                    if (IntermediatePoints == null || IntermediatePoints.Count < 1)
                    {
                        LabelTemplateAngle = findAngle(pts[0], pts[1]);
                        if (LabelTemplateAngle > 90 && LabelTemplateAngle < 270)
                        {
                            Point st;
                            double len = findHypo(pts[0], pts[1]);
                            if (length.Count >= 1)
                            {
                                len = length[length.Count - 1];
                            }
                            double div = 1 - ((len / 2 - templateWidth / 2) / len);
                            this.ConnectorPathGeometry.GetPointAtFractionLength(div, out st, out tangentAtEndPoint);
                            LabelTemplatePosition = st;
                            LabelTemplateAngle = LabelTemplateAngle + 180;
                        }
                        else
                        {
                            Point st;
                            double len = findHypo(pts[0], pts[1]);
                            if (length.Count >= 1)
                            {
                                len = length[length.Count - 1];
                            }
                            double div = ((len / 2 - templateWidth / 2) / len);
                            this.ConnectorPathGeometry.GetPointAtFractionLength(div, out st, out tangentAtEndPoint);
                            LabelTemplatePosition = st;
                        }
                    }
                    else
                    {
                        Point mid;
                        double len = 0;
                        if (length.Count >= 1)
                        {
                            len = length[length.Count - 1];
                        }
                        //double div = ((len / 2 - textWidth) / len);
                        this.ConnectorPathGeometry.GetPointAtFractionLength(0.5, out mid, out tangentAtEndPoint);
                        LabelTemplatePosition = mid;
                        int midIndex = findCenterIntermediatePoints(mid);
                        if (pts.Count > midIndex + 1 && midIndex >= 0)
                        {
                            LabelTemplateAngle = findAngle(pts[midIndex], pts[midIndex + 1]);
                        }
                        if (LabelTemplateAngle > 90 && LabelTemplateAngle < 270)
                        {
                            LabelTemplateAngle = LabelTemplateAngle + 180;
                            //LabelPosition = pts[midIndex + 1];
                        }
                    }
                    break;
                case HorizontalAlignment.Right:
                    LabelTemplatePosition = endPt;
                    LabelTemplateAngle = findAngle(pts[pts.Count - 2], pts[pts.Count - 1]);

                    if (LabelTemplateAngle > 90 && LabelTemplateAngle < 270)
                    {
                        LabelTemplateAngle = LabelTemplateAngle + 180;
                    }
                    else
                    {
                        Point end;
                        double len = findHypo(pts[pts.Count - 2], pts[pts.Count - 1]);
                        if (length.Count >= 1)
                        {
                            len = length[length.Count - 1];
                        }
                        double div = ((len - templateWidth) / len);
                        this.ConnectorPathGeometry.GetPointAtFractionLength(div, out end, out tangentAtEndPoint);
                        LabelTemplatePosition = end;
                    }
                    break;
            }
        }

        /// <summary>
        /// Finds the center intermediate points.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <returns></returns>
        private int findCenterIntermediatePoints(Point point)
        {
            int selectedIndex = 0;
            try
            {
                LineConnector lc = this as LineConnector;
                //point = MeasureUnitsConverter.FromPixels(point, this.MeasurementUnit);
                double three = 3.0;// MeasureUnitsConverter.FromPixels(3.0, this.MeasurementUnit);
                if (IntermediatePoints == null)
                {
                    IntermediatePoints = new List<Point>();
                }
                List<Point> points = new List<Point>();
                if (HeadNode != null)
                {
                    NodeInfo ni;
                    Rect sourceRect = lc.getNodeRect(HeadNode as Node);
                    ni = lc.getRectAsNodeInfo(sourceRect);

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
                    Rect targetRect = lc.getNodeRect(TailNode as Node);
                    ni = lc.getRectAsNodeInfo(targetRect);

                    points.Add(ni.Position);
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
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return selectedIndex;
        }

        /// <summary>
        /// Sets the decorator angle.
        /// </summary>
        /// <param name="pts">The PTS.</param>
        private void setDecoratorAngle(List<Point> pts)
        {
            if (!(ConnectorType == ConnectorType.Bezier))
            {
                if (pts.Count > 0)
                {
                    this.HeadDecoratorPosition = pts[0];
                    this.TailDecoratorPosition = pts[pts.Count - 1];

                    if (IntermediatePoints != null && IntermediatePoints.Count > 0)
                    {
                        HeadDecoratorAngle = findAngle(pts[1], pts[0]);
                        TailDecoratorAngle = findAngle(pts[pts.Count - 2], pts[pts.Count - 1]);
                    }
                    else
                    {
                        HeadDecoratorAngle = findAngle(pts[pts.Count - 1], pts[0]);
                        TailDecoratorAngle = findAngle(pts[0], pts[pts.Count - 1]);
                    }
                }
            }
        }

        /// <summary>
        /// Finds the pointof intersections.
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <returns></returns>
        private List<Point> findPointofIntersections(Point start, Point end)
        {
            Point startRect = new Point((this as LineConnector).minx, (this as LineConnector).miny);
            Point endRect = new Point((this as LineConnector).maxx, (this as LineConnector).maxy);
            Point left, bottom, right, top;
            left = FindPOIBetweenTwoLines2(startRect, new Point(startRect.X, endRect.Y), start, end);
            bottom = FindPOIBetweenTwoLines2(new Point(startRect.X, endRect.Y), endRect, start, end);
            right = FindPOIBetweenTwoLines2(endRect, new Point(endRect.X, startRect.Y), start, end);
            top = FindPOIBetweenTwoLines2(new Point(endRect.X, startRect.Y), startRect, start, end);
            List<Point> intersections = new List<Point>();
            intersections.Add(left);
            intersections.Add(bottom);
            intersections.Add(right);
            intersections.Add(top);
            return intersections;
        }

        /// <summary>
        /// Finds the POI between two poly line.
        /// </summary>
        /// <param name="poly1PathFigure">The poly1 path figure.</param>
        /// <param name="poly2PathFigure">The poly2 path figure.</param>
        /// <returns></returns>
        static internal List<Point> FindPOIBetweenTwoPolyLine(PathFigure poly1PathFigure, PathFigure poly2PathFigure)
        {
            List<Point> intersect = new List<Point>();
            List<Point> poly1 = new List<Point>();
            List<Point> poly2 = new List<Point>();
            poly1.Add(poly1PathFigure.StartPoint);
            poly2.Add(poly2PathFigure.StartPoint);
            foreach (PathSegment pathSegment in poly1PathFigure.Segments)
            {
                if (pathSegment is LineSegment)
                {
                    poly1.Add((pathSegment as LineSegment).Point);
                }
                else if (pathSegment is PolyLineSegment)
                {
                    poly1.AddRange((pathSegment as PolyLineSegment).Points);
                }
            }
            foreach (PathSegment pathSegment in poly2PathFigure.Segments)
            {
                if (pathSegment is LineSegment)
                {
                    poly2.Add((pathSegment as LineSegment).Point);
                }
                else if (pathSegment is PolyLineSegment)
                {
                    poly2.AddRange((pathSegment as PolyLineSegment).Points);
                }
            }
            //poly2.Add(polyLine2Start);
            //poly2.AddRange(polyLine2.Points);

            intersect = FindPOIBetweenTwoPolyLine(poly1, poly2, false);
            return intersect;
        }

        /// <summary>
        /// Finds the POI between two poly line.
        /// </summary>
        /// <param name="polyLine1Start">The poly line1 start.</param>
        /// <param name="polyLine1">The poly line1.</param>
        /// <param name="polyLine2Start">The poly line2 start.</param>
        /// <param name="polyLine2">The poly line2.</param>
        /// <param name="self">if set to <c>true</c> [self].</param>
        /// <returns></returns>
        static internal List<Point> FindPOIBetweenTwoPolyLine(Point polyLine1Start, List<Point> polyLine1, Point polyLine2Start, List<Point> polyLine2, bool self)
        {
            List<Point> intersect = new List<Point>();
            List<Point> poly1 = new List<Point>();
            poly1.Add(polyLine1Start);
            poly1.AddRange(polyLine1);

            List<Point> poly2 = new List<Point>();
            poly2.Add(polyLine2Start);
            poly2.AddRange(polyLine2);

            intersect = FindPOIBetweenTwoPolyLine(poly1, poly2, self);
            return intersect;
        }

        /// <summary>
        /// Finds the POI between two poly line.
        /// </summary>
        /// <param name="polyLine1">The poly line1.</param>
        /// <param name="polyLine2">The poly line2.</param>
        /// <param name="self">if set to <c>true</c> [self].</param>
        /// <returns></returns>
        static internal List<Point> FindPOIBetweenTwoPolyLine(List<Point> polyLine1, List<Point> polyLine2, bool self)
        {
            if (self && polyLine2.Count >= 2)
            {
                polyLine2.RemoveAt(0);
                polyLine2.RemoveAt(0);
            }
            List<Point> intersect = new List<Point>();
            for (int i = 0; i < polyLine1.Count - 1; i++)
            {
                intersect.AddRange(FindPOIBetweenLineAndPolyLine(polyLine1[i], polyLine1[i + 1], polyLine2));
                if (self && polyLine2.Count >= 1)
                {
                    polyLine2.RemoveAt(0);
                }
            }
            return intersect;
        }

        /// <summary>
        /// Finds the POI between line and poly line.
        /// </summary>
        /// <param name="lineStart">The line start.</param>
        /// <param name="lineEnd">The line end.</param>
        /// <param name="polyLine">The poly line.</param>
        /// <returns></returns>
        static private List<Point> FindPOIBetweenLineAndPolyLine(Point lineStart, Point lineEnd, List<Point> polyLine)
        {
            List<Point> intersect = new List<Point>();
            for (int i = 0; i < polyLine.Count - 1; i++)
            {
                Point p = FindPOIBetweenTwoLines2(lineStart, lineEnd, polyLine[i], polyLine[i + 1]);
                if (!p.Equals(new Point(0, 0)))
                {
                    intersect.Add(p);
                }
            }
            return intersect;
        }

        //static private Point FindPOIBetweenTwoLines(Point startRect, Point endRect, Point start, Point end)
        //{
        //    double m1, m2, c1, c2;
        //    m1 = (startRect.Y - endRect.Y) / (startRect.X - endRect.X);
        //    m2 = (start.Y - end.Y) / (start.X - end.X);
        //    c1 = endRect.Y - (m1 * endRect.X);
        //    c2 = end.Y - (m2 * end.X);
        //    double x, y;
        //    x = (c2 - c1) / (m1 - m2);
        //    y = m1 * x + c1;
        //    if (double.IsInfinity(m1) || double.IsInfinity(m2))
        //    {
        //        m1 = (startRect.X - endRect.X) / (startRect.Y - endRect.Y);
        //        c1 = endRect.X - (m1 * endRect.Y);
        //        m2 = (start.X - end.X) / (start.Y - end.Y);
        //        c2 = end.X - (m2 * end.Y);

        //        y = (c2 - c1) / (m1 - m2);
        //        x = m1 * y + c1;
        //    }

        //    if (LiesWithin(startRect, endRect, new Point(x, y)) && LiesWithin(start, end, new Point(x, y)))
        //    {
        //        return new Point(x, y);
        //    }
        //    else
        //    {
        //        return new Point(0, 0);
        //    }
        //}


        //static private Point FindPOIBetweenTwoLines2(Point startRect, Point endRect, Point start, Point end)
        //{
        //    double m1, m2, c1, c2;
        //    double x, y;
        //    m1 = (startRect.Y - endRect.Y) / (startRect.X - endRect.X);
        //    m2 = (start.Y - end.Y) / (start.X - end.X);
        //    if (double.IsInfinity(m1))
        //    {
        //        m1 = (startRect.X - endRect.X) / (startRect.Y - endRect.Y);
        //        c1 = endRect.X - (m1 * endRect.Y);
        //    }
        //    else
        //    {
        //        c1 = endRect.Y - (m1 * endRect.X);
        //    }
        //    if (double.IsInfinity(m2))
        //    {
        //        m2 = (start.X - end.X) / (start.Y - end.Y);
        //        c2 = end.X - (m2 * end.Y);
        //    }
        //    else
        //    {
        //        c2 = end.Y - (m2 * end.X);
        //    }
        //    if (double.IsInfinity(m1) || double.IsInfinity(m2))
        //    {
        //        y = (c2 - c1) / (m1 - m2);
        //        x = m1 * y + c1;
        //    }
        //    x = (c2 - c1) / (m1 - m2);
        //    y = m1 * x + c1;
        //    if (m1 == 0)
        //    {
        //        y = c1;
        //    }
        //    if (m2 == 0)
        //    {
        //        x = c2;
        //    }

        //    if (LiesWithin(startRect, endRect, new Point(x, y), m1) && LiesWithin(start, end, new Point(x, y), m2))
        //    {
        //        return new Point(x, y);
        //    }
        //    else
        //    {
        //        return new Point(0, 0);
        //    }
        //}

        /// <summary>
        /// Finds the POI between two lines2.
        /// </summary>
        /// <param name="startRect">The start rect.</param>
        /// <param name="endRect">The end rect.</param>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <returns></returns>
        static private Point FindPOIBetweenTwoLines2(Point startRect, Point endRect, Point start, Point end)
        {
            double m1, m2, c1, c2;
            double x, y;
            x = double.NaN;
            y = double.NaN;
            m1 = (startRect.Y - endRect.Y) / (startRect.X - endRect.X);
            m2 = (start.Y - end.Y) / (start.X - end.X);
            if (double.IsInfinity(m1))
            {
                m1 = 0;
                c1 = endRect.X;
                x = c1;
            }
            else if (m1 == 0)
            {
                c1 = endRect.Y;
                y = c1;
            }
            else
            {
                c1 = endRect.Y - (m1 * endRect.X);
            }
            if (double.IsInfinity(m2))
            {
                m2 = 0;
                c2 = end.X;
                x = c2;
            }
            else if (m2 == 0)
            {
                c2 = end.Y;
                y = c2;
            }
            else
            {
                c2 = end.Y - (m2 * end.X);
            }
            //if (double.IsInfinity(m1))
            //{
            //    m1 = (startRect.X - endRect.X) / (startRect.Y - endRect.Y);
            //    c1 = endRect.X - (m1 * endRect.Y);
            //}
            //else
            //{
            //    c1 = endRect.Y - (m1 * endRect.X);
            //}
            //if (double.IsInfinity(m2))
            //{
            //    m2 = (start.X - end.X) / (start.Y - end.Y);
            //    c2 = end.X - (m2 * end.Y);
            //}
            //else
            //{
            //    c2 = end.Y - (m2 * end.X);
            //}

            //if (double.IsInfinity(m1) || double.IsInfinity(m2))
            //{
            //    y = (c2 - c1) / (m1 - m2);
            //    x = m1 * y + c1;
            //}
            if (double.IsNaN(x))
            {
                x = (c2 - c1) / (m1 - m2);
            }
            if (double.IsNaN(y))
            {
                y = m1 * x + c1;
            }
            //if (m1 == 0)
            //{
            //    y = c2;
            //}
            ////else if (double.IsInfinity(m1))
            ////{
            ////    y = c2;
            ////    m1 = 0;
            ////}
            //if (m2 == 0)
            //{
            //    x = c1;
            //}
            ////else if (double.IsInfinity(m2))
            ////{
            ////    x = c1;
            ////    m2 = 0;
            ////}

            //if (m1 == 0 && m2 == 0)
            //{
            //    y = c1;
            //    x = c2;
            //}
            //else
            //{
            //    y = c2;
            //    x = c1;
            //    m1 = 0;
            //    m2 = 0;
            //}
            //else if (double.IsInfinity(m1))
            //{
            //    y = c2;
            //    m1 = 0;
            //}
            //if (m2 == 0)
            //{
            //}
            //else if (double.IsInfinity(m2))
            //{
            //    x = c1;
            //    m2 = 0;
            //}
            if (LiesWithin(startRect, endRect, new Point(x, y), m1) && LiesWithin(start, end, new Point(x, y), m2))
            {
                return new Point(x, y);
            }
            else
            {
                return new Point(0, 0);
            }
        }

        /// <summary>
        /// Lieses the within.
        /// </summary>
        /// <param name="startRect">The start rect.</param>
        /// <param name="endRect">The end rect.</param>
        /// <param name="point">The point.</param>
        /// <param name="m">The m.</param>
        /// <returns></returns>
        private static bool LiesWithin(Point startRect, Point endRect, Point point, double m)
        {
            if (m != 0)
            {
                if (((startRect.X < point.X && point.X < endRect.X) || (startRect.X > point.X && point.X > endRect.X)) &&
                    ((startRect.Y < point.Y && point.Y < endRect.Y) || (startRect.Y > point.Y && point.Y > endRect.Y)))
                {
                    return true;
                }
            }
            else
            {
                if (((startRect.X <= point.X && point.X <= endRect.X) || (startRect.X >= point.X && point.X >= endRect.X)) &&
                    ((startRect.Y <= point.Y && point.Y <= endRect.Y) || (startRect.Y >= point.Y && point.Y >= endRect.Y)))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Finds the angle.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <param name="e">The e.</param>
        /// <returns></returns>
        internal static double findAngle(Point s, Point e)
        {
            Point r = new Point(e.X, s.Y);
            double sr = findHypo(s, r);
            double re = findHypo(r, e);
            double es = findHypo(e, s);
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
            return ang;
        }

        /// <summary>
        /// Finds the length.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <returns></returns>
        private static List<double> findLength(List<Point> list)
        {
            List<double> length = new List<double>();
            length.Add(0);
            for (int i = 0; i < list.Count - 1; i++)
            {
                length.Add(length[length.Count - 1] +
                    findHypo(list[i], list[i + 1]));
            }
            return length;
        }

        /// <summary>
        /// Finds the hypo.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <param name="e">The e.</param>
        /// <returns></returns>
        private static double findHypo(Point s, Point e)
        {
            double length;
            length = Math.Sqrt(Math.Pow((s.X - e.X), 2) + Math.Pow((s.Y - e.Y), 2));
            return length;
        }

        /// <summary>
        /// Shows the adorner
        /// </summary>
        protected virtual void ShowAdorner()
        {
        }

        /// <summary>
        /// Hides the adorner
        /// </summary>
        protected virtual void HideAdorner()
        {
        }

        #endregion

        #region INotifyPropertyChanged Members

        /// <summary>
        /// Calls property changed event handler.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Called when [property changed].
        /// </summary>
        /// <param name="name">The name of the property.</param>
        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                DiagramView dview = Node.GetDiagramView(this);
                if (dview != null)
                {
                    if (dview.IsPageSaved)
                    {
                        foreach (DiagramProperty d in dview.DiagramProperties.Where(item => item.ObjectType.Equals(typeof(ConnectorBase))))
                        {
                            if (d.PropertyName.Equals(name))
                            {
                                dview.IsPageSaved = false;
                            }
                        }
                    }
                }
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
        
        /// <summary>
        /// Invoked whenever the effective value of any dependency property on this <see cref="T:System.Windows.FrameworkElement"/> has been updated. The specific dependency property that changed is reported in the arguments parameter. Overrides <see cref="M:System.Windows.DependencyObject.OnPropertyChanged(System.Windows.DependencyPropertyChangedEventArgs)"/>.
        /// </summary>
        /// <param name="e">The event data that describes the property that changed, as well as old and new values.</param>
        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                DiagramView dview = Node.GetDiagramView(this);
                if (dview != null)
                {
                    if (dview.IsPageSaved)
                    {
                        foreach (DiagramProperty d in dview.DiagramProperties.Where(item => item.ObjectType.Equals(typeof(ConnectorBase))))
                        {
                            if (d.PropertyName.Equals(e.Property.Name))
                            {
                                dview.IsPageSaved = false;
                            }
                        }
                    }
                }
                handler(this, new PropertyChangedEventArgs(e.Property.ToString()));
            }
        }

        #endregion

        #region IEdge Members

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
        /// namespace WpfApplication1
        /// {
        /// public partial class Window1 : Window
        /// {
        /// public DiagramControl Control;
        /// public DiagramModel Model;
        /// public DiagramView View;
        /// public Window1 ()
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
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ConnectionPort ConnectionHeadPort
        {
            get
            {
                return (ConnectionPort)GetValue(ConnectionHeadPortProperty);
            }

            set
            {
                SetValue(ConnectionHeadPortProperty, value);
                OnPropertyChanged("ConnectionHeadPort");
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
        /// namespace WpfApplication1
        /// {
        /// public partial class Window1 : Window
        /// {
        /// public DiagramControl Control;
        /// public DiagramModel Model;
        /// public DiagramView View;
        /// public Window1 ()
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
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ConnectionPort ConnectionTailPort
        {
            get
            {
                return (ConnectionPort)GetValue(ConnectionTailPortProperty);
            }

            set
            {
                SetValue(ConnectionTailPortProperty, value);
                OnPropertyChanged("ConnectionTailPort");
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
        /// namespace WpfApplication1
        /// {
        /// public partial class Window1 : Window
        /// {
        /// public DiagramControl Control;
        /// public DiagramModel Model;
        /// public DiagramView View;
        /// public Window1 ()
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
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IShape HeadNode
        {
            get
            {
                return (IShape)GetValue(HeadNodeProperty);
            }

            set
            {
                SetValue(HeadNodeProperty, value);
                OnPropertyChanged("HeadNode");
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
            get { return hid; }
            set { hid = value; }
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
            get { return tid; }
            set { tid = value; }
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
            get { return hpid; }
            set { hpid = value; }
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
            get { return tpid; }
            set { tpid = value; }
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
        /// namespace WpfApplication1
        /// {
        /// public partial class Window1 : Window
        /// {
        /// public DiagramControl Control;
        /// public DiagramModel Model;
        /// public DiagramView View;
        /// public Window1 ()
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
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IShape TailNode
        {
            get
            {
                return (IShape)GetValue(TailNodeProperty);
            }

            set
            {
                SetValue(TailNodeProperty, value);
                OnPropertyChanged("TailNode");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the Layout is directed or not.
        /// </summary>
        /// <value>
        /// Type: <see cref="Boolean"/>
        /// True if the layout is directed, false otherwise.
        /// </value>
        public bool IsDirected
        {
            get { return mIsDirected; }
            set { mIsDirected = value; }
        }

        #endregion

        #region INodeGroup Members

        /// <summary>
        /// Gets the groups to which the INodeGroup objects belong.
        /// </summary>
        /// <value>The groups.</value>
        public CollectionExt Groups
        {
            get
            {
                return m_groups;
            }
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
        /// Gets or sets the reference number of the INodeGroup objects. Used for serialization purposes..
        /// </summary>
        /// <value>The reference no.</value>
        public int ReferenceNo
        {
            get { return no; }
            set { no = value; }
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

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsSelected
        {
            get { return (bool)GetValue(IsSelectedProperty); }
            set { SetValue(IsSelectedProperty, value); }
        }

        private bool LineVirtual = true;

        public bool AllowVirtualization
        {
            get { return LineVirtual; }
            set { LineVirtual = value; }

        }
        #endregion

        #region Methods

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
        /// namespace WpfApplication1
        /// {
        /// public partial class Window1 : Window
        /// {
        /// public DiagramControl Control;
        /// public DiagramModel Model;
        /// public DiagramView View;
        /// public Window1 ()
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
            if (HeadNode != null && (node as IShape) == HeadNode)
            {
                if (TailNode == null)
                {
                    return null;
                }
                else
                {
                    return TailNode as IShape;
                }
            }
            else if (TailNode != null && (node as IShape) == TailNode)
            {
                if (HeadNode == null)
                {
                    return null;
                }
                else
                {
                    return HeadNode as IShape;
                }
            }
            else
            {
                throw new Exception("The given node is not part of the edge.");
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
            double xIntercept;
            double yIntercept;

            double rheight = rect.Height;
            double rwidth = rect.Width;
            double rcTop = node.Position.Y - (rheight / 2);
            double rcBottom = node.Position.Y + (rheight / 2);
            double rcLeft = node.Position.X - (rwidth / 2);
            double rcRight = node.Position.X + (rwidth / 2);

            if ((pt2.X - pt1.X) != 0)
            {
                m = LineSlope(pt1, pt2);
            }
            else
            {
                isVerticalLine = true;
            }

            ////Test top side.

            if ((pt1.Y <= rcTop && pt2.Y >= rcTop) || (pt2.Y <= rcTop && pt1.Y >= rcTop))
            {
                if (isVerticalLine)
                {
                    xIntercept = pt1.X;
                }
                else
                {
                    xIntercept = (double)((rcTop + ((m * pt1.X) - pt1.Y)) / m);
                }

                if (xIntercept >= rcLeft && xIntercept <= rcRight)
                {
                    if (conType != ConnectorType.Straight)
                    {
                        intersectpoint = new Point(rcLeft + (rwidth / 2), rcTop);
                    }
                    else
                    {
                        intersectpoint = new Point(xIntercept, rcTop);
                    }

                    isTop = true;
                }
            }

            ////Test bottom side.

            if ((pt1.Y <= rcBottom && pt2.Y >= rcBottom) || (pt2.Y <= rcBottom && pt1.Y >= rcBottom))
            {
                if (isVerticalLine)
                {
                    xIntercept = pt1.X;
                }
                else
                {
                    xIntercept = (double)((rcBottom + ((m * pt1.X) - pt1.Y)) / m);
                }

                if (xIntercept >= rcLeft && xIntercept <= rcRight)
                {
                    if (conType != ConnectorType.Straight)
                    {
                        intersectpoint = new Point(rcLeft + (rwidth / 2), rcBottom);
                    }
                    else
                    {
                        intersectpoint = new Point(xIntercept, rcBottom);
                    }

                    isBottom = true;
                }
            }

            ////Test left side.

            if ((!isVerticalLine && (pt1.X <= rcLeft && pt2.X >= rcLeft)) || (pt2.X <= rcLeft && pt1.X >= rcLeft))
            {
                yIntercept = (double)((m * (rcLeft - pt1.X)) + pt1.Y);

                if (yIntercept >= rcTop && yIntercept <= rcBottom)
                {
                    if (conType != ConnectorType.Straight)
                    {
                        intersectpoint = new Point(rcLeft, rcTop + rheight / 2);
                    }
                    else
                    {
                        intersectpoint = new Point(rcLeft, yIntercept);
                    }

                    isLeft = true;
                }
            }

            //// Test right side.

            if ((!isVerticalLine && (pt1.X <= rcRight && pt2.X >= rcRight)) || (pt2.X <= rcRight && pt1.X >= rcRight))
            {
                yIntercept = (double)((m * (rcRight - pt1.X)) + pt1.Y);

                if (yIntercept >= rcTop && yIntercept <= rcBottom)
                {
                    if (conType != ConnectorType.Straight)
                    {
                        intersectpoint = new Point(rcRight, rcTop + (rheight / 2));
                    }
                    else
                    {
                        intersectpoint = new Point(rcRight, yIntercept);
                    }

                    isRight = true;
                }
            }

            return intersectpoint;
        }

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
            double sheight = rect.Height;
            double swidth = rect.Width;

            double theight = trect.Height;
            double twidth = trect.Width;
            double rcTop = source.Position.Y - sheight / 2;
            double rcBottom = source.Position.Y + sheight / 2;
            double rcLeft = source.Position.X - swidth / 2;
            double rcRight = source.Position.X + swidth / 2;
            double trcTop = target.Position.Y - theight / 2;
            double trcBottom = target.Position.Y + theight / 2;
            double trcLeft = target.Position.X - twidth / 2;
            double trcRight = target.Position.X + twidth / 2;

            ////Test top side.
            if (rcTop >= trcBottom)
            {
                si = new Point(rcLeft + swidth / 2, rcTop);
                isTop = true;
                ti = new Point(trcLeft + twidth / 2, trcBottom);
                tisBottom = true;
            }

            ////Test bottom side.
            if (rcBottom <= trcTop)
            {
                si = new Point(rcLeft + swidth / 2, rcBottom);
                isBottom = true;
                ti = new Point(trcLeft + twidth / 2, trcTop);
                tisTop = true;
            }

            if ((rcBottom > trcTop) && (rcTop < trcBottom))
            {
                if (rcRight >= trcLeft)
                {
                    if (rcLeft <= trcLeft)
                    {
                        ////left left
                        si = new Point(rcLeft, rcTop + sheight / 2);
                        isLeft = true;
                        ti = new Point(trcLeft, trcTop + theight / 2);
                        tisLeft = true;
                    }

                    if (rcLeft <= trcRight)
                    {
                        ////right right
                        si = new Point(rcRight, rcTop + sheight / 2);
                        isRight = true;
                        ti = new Point(trcRight, trcTop + theight / 2);
                        tisRight = true;
                    }
                    else
                    {
                        ////left right
                        si = new Point(rcLeft, rcTop + sheight / 2);
                        isLeft = true;
                        ti = new Point(trcRight, trcTop + theight / 2);
                        tisRight = true;
                    }
                }
                else
                {
                    {
                        ////rightleft
                        si = new Point(rcRight, rcTop + sheight / 2);
                        isRight = true;
                        ti = new Point(trcLeft, trcTop + theight / 2);
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
            double rcTop = source.Position.Y - sheight / 2;
            double rcBottom = source.Position.Y + sheight / 2;
            double rcLeft = source.Position.X - swidth / 2;
            double rcRight = source.Position.X + swidth / 2;
            double trcTop = target.Position.Y - theight / 2;
            double trcBottom = target.Position.Y + theight / 2;
            double trcLeft = target.Position.X - twidth / 2;
            double trcRight = target.Position.X + twidth / 2;

            if (rcRight >= trcLeft)
            {
                if (rcLeft <= trcLeft)
                {
                    ////left left
                    si = new Point(rcLeft, rcTop + sheight / 2);
                    isLeft = true;
                    ti = new Point(trcLeft, trcTop + theight / 2);
                    tisLeft = true;
                }
                else if (rcLeft <= trcRight)
                {
                    ////right right
                    si = new Point(rcRight, rcTop + sheight / 2);
                    isRight = true;
                    ti = new Point(trcRight, trcTop + theight / 2);
                    tisRight = true;
                }
                else
                {
                    ////left right
                    si = new Point(rcLeft, rcTop + sheight / 2);
                    isLeft = true;
                    ti = new Point(trcRight, trcTop + theight / 2);
                    tisRight = true;
                }
            }
            else
            {
                ////rightleft
                si = new Point(rcRight, rcTop + sheight / 2);
                isRight = true;
                ti = new Point(trcLeft, trcTop + theight / 2);
                tisLeft = true;
            }
        }

        #endregion

        #region ICloneable Members

        /// <summary>
        /// Creates a clone.
        /// </summary>
        /// <returns>The cloned object</returns>
        public object Clone()
        {
            throw new NotImplementedException();
        }

        #endregion

        /// <summary>
        /// Gets the line PTS.
        /// </summary>
        /// <returns></returns>
        internal List<Point> getLinePts()
        {
            List<Point> pts = new List<Point>();
            if (this.ConnectorPathGeometry != null && this.ConnectorPathGeometry.Bounds != Rect.Empty)
            {
                pts.Add(this.ConnectorPathGeometry.Figures[0].StartPoint);
                pts.AddRange(IntermediatePoints);
                if (this.ConnectorPathGeometry.Figures[0].Segments.Last<PathSegment>() is LineSegment)
                {
                    pts.Add((this.ConnectorPathGeometry.Figures[0].Segments.Last<PathSegment>() as LineSegment).Point);
                }
                else
                {
                    pts.Add((this.ConnectorPathGeometry.Figures[0].Segments.Last<PathSegment>() as PolyLineSegment).Points.Last<Point>());
                }
            }
            return pts;
        }

        /// <summary>
        /// Gets the line PTS.
        /// </summary>
        /// <param name="ConnectorPathGeometry">The connector path geometry.</param>
        /// <returns></returns>
        internal static List<Point> getLinePts(PathFigure ConnectorPathGeometry)
        {
            List<Point> pts = new List<Point>();
            //foreach (PathSegment ps in ConnectorPathGeometry.Segments)
            if (ConnectorPathGeometry != null)
            {
                PathFigure pf = ConnectorPathGeometry.Clone();
                for (int i = 0; i < pf.Segments.Count; i++)
                {
                    if (pf.Segments[i] is ArcSegment)
                    {
                        if (i > 0)
                        {
                            if (pf.Segments[i - 1] is LineSegment)
                            {
                                //(pf.Segments[i - 1] as LineSegment).Point = (pf.Segments[i] as ArcSegment).Point;
                                pf.Segments.RemoveAt(i - 1);
                                pf.Segments.RemoveAt(i - 1);
                                i -= 2;
                            }
                            else if (pf.Segments[i - 1] is PolyLineSegment)
                            {
                                //PointCollection temppts = (pf.Segments[i - 1] as PolyLineSegment).Points;
                                //temppts[temppts.Count-1] = (pf.Segments[i] as ArcSegment).Point;
                                PointCollection temppts = (pf.Segments[i - 1] as PolyLineSegment).Points;
                                (pf.Segments[i - 1] as PolyLineSegment).Points.RemoveAt(temppts.Count - 1);
                                pf.Segments.RemoveAt(i);
                                //pf.Segments.RemoveAt(i);
                                i--;
                            }
                        }
                    }
                }
                pts.Add(pf.StartPoint);
                foreach (PathSegment ps in pf.Segments)
                {
                    if (ps is LineSegment)
                    {
                        pts.Add((ps as LineSegment).Point);
                    }
                    else if (ps is PolyLineSegment)
                    {
                        pts.AddRange((ps as PolyLineSegment).Points.ToList<Point>());
                    }
                }
            }
            return pts;
        }

        /// <summary>
        /// Parses the points.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static List<Point> ParsePoints(string input)
        {
            List<Point> pts = new List<Point>();

            String strPoints = input.ToString();
            StringToPoints stp = new StringToPoints();
            pts = (List<Point>)stp.Convert(strPoints, typeof(List<Point>), null, null);

            return pts;
        }

        internal Point PxStartPointPosition
        {
            get
            {
                return MeasureUnitsConverter.ToPixels(StartPointPosition, this.MeasurementUnit);
            }
            set
            {
                StartPointPosition = MeasureUnitsConverter.FromPixels(value, this.MeasurementUnit);
            }
        }

        internal Point PxEndPointPosition
        {
            get
            {
                return MeasureUnitsConverter.ToPixels(EndPointPosition, this.MeasurementUnit);
            }
            set
            {
                EndPointPosition = MeasureUnitsConverter.FromPixels(value, this.MeasurementUnit);
            }
        }

        //internal Point PxStartPointPosition
        //{
        //    get { return (Point)GetValue(PxStartPointPositionProperty); }
        //    set { SetValue(PxStartPointPositionProperty, value); }
        //}

        //internal static readonly DependencyProperty PxStartPointPositionProperty =
        //    DependencyProperty.Register("PxStartPointPosition", typeof(Point), typeof(ConnectorBase));

        //internal Point PxEndPointPosition
        //{
        //    get { return (Point)GetValue(PxEndPointPositionProperty); }
        //    set { SetValue(PxEndPointPositionProperty, value); }
        //}

        //// Using a DependencyProperty as the backing store for PxEndPointPosition.  This enables animation, styling, binding, etc...
        //internal static readonly DependencyProperty PxEndPointPositionProperty =
        //    DependencyProperty.Register("PxEndPointPosition", typeof(Point), typeof(ConnectorBase));

    }
}
