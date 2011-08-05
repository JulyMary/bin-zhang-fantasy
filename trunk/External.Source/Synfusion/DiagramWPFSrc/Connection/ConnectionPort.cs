// <copyright file="ConnectionPort.cs" company="Syncfusion">
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
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace Syncfusion.Windows.Diagram
{
    /// <summary>
    /// Represents a port which can be used to make connections to the node.
    /// </summary>
    /// <remarks>
    /// The <see cref="ConnectionPort"/> class can be used for defining custom ports on the nodes.
    /// <para/>
    /// Any number of ports can be defined on a node.  By default every node has a center port. 
    /// </remarks>
    /// <example>
    /// C#:
    /// <para/>
    /// The following example shows how to create a <see cref="ConnectionPort"/> in C#.
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
    ///        InitializeComponent ();
    ///        Control = new DiagramControl ();
    ///        Model = new DiagramModel ();
    ///        View = new DiagramView ();
    ///        Control.View = View;
    ///        Control.Model = Model;
    ///        View.Bounds = new Thickness(0, 0, 1000, 1000);
    ///        //Creates a node
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
    ///        //Define a Custom port for the node.
    ///        ConnectionPort port = new ConnectionPort();
    ///        port.Node=n;
    ///        port.Left=75;
    ///        port.Top=10;
    ///        port.PortShape = PortShapes.Arrow;
    ///        port.PortStyle.Fill = Brushes.Transparent;
    ///        port.Height = 11;
    ///        port.Width = 11;
    ///        n.Ports.Add(port);
    ///        Node n1 = new Node(Guid.NewGuid(), "Decision1");
    ///        n1.Shape = Shapes.FlowChart_Process;
    ///        n1.IsLabelEditable = true;
    ///        n1.Label = "Alarm Rings";
    ///        n1.Level = 2;
    ///        n1.OffsetX = 150;
    ///        n1.OffsetY = 125;
    ///        n1.Width = 150;
    ///        n1.Height = 75;
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
    ///        LineConnector o2 = new LineConnector();
    ///        o2.ConnectorType = ConnectorType.Straight;
    ///        o2.TailNode = n1;
    ///        o2.HeadNode = n;
    ///        o2.LabelHorizontalAlignment = HorizontalAlignment.Center;
    ///        //Making connection to the ports.
    ///        o2.ConnectionHeadPort = port;
    ///        o2.ConnectionTailPort = port1;
    ///        Model.Connections.Add(o2);
    ///    }
    ///    }
    ///    }
    /// </code>
    /// </example>
    /// <seealso cref="Node"/>
#if SyncfusionFramework4_0
    [DesignTimeVisible(false)]
#endif
    public class ConnectionPort : Thumb
    {
        #region Class Fields

        /// <summary>
        /// Represents the port pointer.
        /// </summary>
        private Border customportPointer;

        /// <summary>
        /// Represents the current port position.
        /// </summary>
        private Point endpoint;

        /// <summary>
        /// Represents the previous port position.
        /// </summary>
        private Point m_previousOriginPoint;

        /// <summary>
        /// Represents the current node.
        /// </summary>
        private Node node;

        /// <summary>
        /// Represents the Diagram Page.
        /// </summary>
        private DiagramPage diagramPage;

        /// <summary>
        /// Boolean value indicating mouse over the node.
        /// </summary>
        private bool m_mouseover = false;

        /// <summary>
        /// Represents the port shape.
        /// </summary>
        //private PortShapes m_shape = PortShapes.Diamond;

        /// <summary>
        /// Represents the port style.
        /// </summary>
        private PortStyle customPortStyle;

        /// <summary>
        /// Represents the current node .
        /// </summary>
        //private Node m_node;

        /// <summary>
        /// Represents the Diagram Control.
        /// </summary>
        private DiagramControl dc;

        /// <summary>
        /// Refers to the node reference number.
        /// </summary>
        private int noderef;

        /// <summary>
        /// Represents the port reference number.
        /// </summary>
        private int portref = -1;

        /// <summary>
        /// Refers to the name of the port.
        /// </summary>
        private string m_name;

        /// <summary>
        /// Refers to the center port reference number.
        /// </summary>
        private int cportref = -1;
        #endregion

        #region Initialization

        /// <summary>
        /// Initializes static members of the <see cref="ConnectionPort"/> class.
        /// </summary>
        static ConnectionPort()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ConnectionPort), new FrameworkPropertyMetadata(typeof(ConnectionPort)));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConnectionPort"/> class .
        /// </summary>
        public ConnectionPort()
        {
            DragDelta += new DragDeltaEventHandler(PortThumb_DragDelta);
            this.Loaded += new RoutedEventHandler(ConnectionPort_Loaded);
            this.LayoutUpdated += new EventHandler(ConnectionPort_LayoutUpdated);
            this.SizeChanged += new SizeChangedEventHandler(ConnectionPort_SizeChanged);
            PortStyle = new PortStyle();
        }

        void ConnectionPort_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ConnectionPort port = sender as ConnectionPort;
            try
            {
                if (port.Node != null)
                {
                    TranslateTransform tr = new TranslateTransform(port.Left - this.ActualWidth / 2, port.Top - this.ActualHeight / 2);
                    port.RenderTransform = tr;
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConnectionPort"/> class.
        /// </summary>
        /// <param name="node">The node hosting this port.</param>
        public ConnectionPort(Node node)
        {
            DragDelta += new DragDeltaEventHandler(PortThumb_DragDelta);
            this.Node = node;
            this.Loaded += new RoutedEventHandler(ConnectionPort_Loaded);
            this.LayoutUpdated += new EventHandler(ConnectionPort_LayoutUpdated);
            this.SizeChanged += new SizeChangedEventHandler(ConnectionPort_SizeChanged);
            PortStyle = new PortStyle();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConnectionPort"/> class .
        /// </summary>
        /// <param name="node">The node hosting this port.</param>
        /// <param name="position">Position of the port</param>
        public ConnectionPort(Node node, Point position)
        {
            DragDelta += new DragDeltaEventHandler(PortThumb_DragDelta);
            this.Node = node;
            this.Left = position.X;
            this.Top = position.Y;
            this.Loaded += new RoutedEventHandler(ConnectionPort_Loaded);
            this.LayoutUpdated += new EventHandler(ConnectionPort_LayoutUpdated);
            this.SizeChanged += new SizeChangedEventHandler(ConnectionPort_SizeChanged);
            PortStyle = new PortStyle();
        }

        /// <summary>
        /// Is invoked when the port is loaded.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void ConnectionPort_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Node != null && Node.Ports != null)
                {
                    foreach (ConnectionPort cport in Node.Ports)
                    {
                        double x = this.Left - this.ActualWidth / 2;
                        double y = this.Top - this.ActualHeight / 2;
                        this.PortNodeReference = this.Node.ReferenceNo;
                        TranslateTransform tr = new TranslateTransform(x, y);
                        this.RenderTransform = tr;
                    }
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// Is invoked when the port's layout is updated..
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void ConnectionPort_LayoutUpdated(object sender, EventArgs e)
        {
            if (!double.IsNaN(this.Width) && !double.IsNaN(this.Height))
            {
                this.CenterPosition = new Point(this.Left, this.Top);
            }
            else
            {
                this.CenterPosition = new Point(this.Left / 2, this.Top);
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the port shape.
        /// </summary>
        /// <value>
        /// Type: <see cref="PortShapes"/>
        /// Enum specifying the port shapes.
        /// </value>
        /// <remarks>
        /// By default the port shape is Diamond. 
        /// </remarks>
        /// <example>
        /// C#:
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
        ///        InitializeComponent ();
        ///        Control = new DiagramControl ();
        ///        Model = new DiagramModel ();
        ///        View = new DiagramView ();
        ///        Control.View = View;
        ///        Control.Model = Model;
        ///        View.Bounds = new Thickness(0, 0, 1000, 1000);
        ///        //Creates a node
        ///        Node n = new Node(Guid.NewGuid(), "NewNode");
        ///        n.Shape = Shapes.Rectangle;
        ///        n.IsLabelEditable = true;
        ///        n.Label = "Start";
        ///        n.OffsetX = 150;
        ///        n.OffsetY = 25;
        ///        n.Width = 150;
        ///        n.Height = 75;
        ///        n.ToolTip="New Node";
        ///        Model.Nodes.Add(n);
        ///        //Define a Custom port for the node.
        ///        ConnectionPort port = new ConnectionPort();
        ///        port.Node=n;
        ///        port.Left=75;
        ///        port.Top=10;
        ///        //Specifies the port shape
        ///        port.PortShape = PortShapes.Arrow;
        ///        n.Ports.Add(port);
        ///    }
        ///    }
        ///    }
        /// </code>
        /// </example>
        //public PortShapes PortShape
        //{
        //    get
        //    {
        //        return m_shape;
        //    }

        //    set
        //    {
        //        m_shape = value;
        //        OnPropertyChanged("PortShape");
        //    }
        //}

        /// <summary>
        /// Gets or sets the node which is hosting this port.
        /// </summary>
        /// <value>
        /// Type: <see cref="Node"/>
        /// Node object.
        /// </value>
        /// <remarks>
        /// Any number of <see cref="ConnectionPort"/> can be specified for a <see cref="Node"/>.
        /// <para/>
        /// By default every node has a center port.
        /// </remarks>
        /// <example>
        /// C#:
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
        ///        InitializeComponent ();
        ///        Control = new DiagramControl ();
        ///        Model = new DiagramModel ();
        ///        View = new DiagramView ();
        ///        Control.View = View;
        ///        Control.Model = Model;
        ///        View.Bounds = new Thickness(0, 0, 1000, 1000);
        ///        //Creates a node
        ///        Node n = new Node(Guid.NewGuid(), "NewNode");
        ///        n.Shape = Shapes.Rectangle;
        ///        n.IsLabelEditable = true;
        ///        n.Label = "Start";
        ///        n.OffsetX = 150;
        ///        n.OffsetY = 25;
        ///        n.Width = 150;
        ///        n.Height = 75;
        ///        n.ToolTip="New Node";
        ///        Model.Nodes.Add(n);
        ///        //Define a Custom port for the node.
        ///        ConnectionPort port = new ConnectionPort();
        ///        //Specifies the node
        ///        port.Node=n;
        ///        port.Left=75;
        ///        port.Top=10;
        ///        //Specifies the port shape
        ///        port.PortShape = PortShapes.Arrow;
        ///        n.Ports.Add(port);
        ///    }
        ///    }
        ///    }
        /// </code>
        /// </example>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Node Node
        {
            get
            {
                return (Node)GetValue(NodeProperty);
            }

            set
            {
                SetValue(NodeProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the left position of the port. This is a dependency property.
        /// </summary>
        /// <value>
        /// Type: <see cref="double"/>
        /// Port left position.
        /// </value>
        /// <example>
        /// C#:
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
        ///        InitializeComponent ();
        ///        Control = new DiagramControl ();
        ///        Model = new DiagramModel ();
        ///        View = new DiagramView ();
        ///        Control.View = View;
        ///        Control.Model = Model;
        ///        View.Bounds = new Thickness(0, 0, 1000, 1000);
        ///        //Creates a node
        ///        Node n = new Node(Guid.NewGuid(), "NewNode");
        ///        n.Shape = Shapes.Rectangle;
        ///        n.IsLabelEditable = true;
        ///        n.Label = "Start";
        ///        n.OffsetX = 150;
        ///        n.OffsetY = 25;
        ///        n.Width = 150;
        ///        n.Height = 75;
        ///        n.ToolTip="New Node";
        ///        Model.Nodes.Add(n);
        ///        //Define a Custom port for the node.
        ///        ConnectionPort port = new ConnectionPort();
        ///        //Specifies the node
        ///        port.Node=n;
        ///        //Specifies the left position of the port.
        ///        port.Left=75;
        ///        port.Top=10;
        ///        //Specifies the port shape
        ///        port.PortShape = PortShapes.Arrow;
        ///        n.Ports.Add(port);
        ///    }
        ///    }
        ///    }
        /// </code>
        /// </example>
        public double Left
        {
            get
            {
                return (double)GetValue(LeftProperty);
            }

            set
            {
                SetValue(LeftProperty, value);
            }
        }

        internal double PxLeft
        {
            get { return (double)GetValue(PxLeftProperty); }
            set { SetValue(PxLeftProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PxLeft.  This enables animation, styling, binding, etc...
        internal static readonly DependencyProperty PxLeftProperty =
            DependencyProperty.Register("PxLeft", typeof(double), typeof(ConnectionPort));

        public PortShapes PortShape
        {
            get
            {
                return (PortShapes)GetValue(PortShapeProperty);
            }
            set
            {
                SetValue(PortShapeProperty, value);

            }
        }
        /// <summary>
        /// Gets or sets the top position of the port. This is a dependency property.
        /// </summary>
        /// <value>
        /// Type: <see cref="double"/>
        /// Port top position.
        /// </value>
        /// <example>
        /// C#:
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
        ///        InitializeComponent ();
        ///        Control = new DiagramControl ();
        ///        Model = new DiagramModel ();
        ///        View = new DiagramView ();
        ///        Control.View = View;
        ///        Control.Model = Model;
        ///        View.Bounds = new Thickness(0, 0, 1000, 1000);
        ///        //Creates a node
        ///        Node n = new Node(Guid.NewGuid(), "NewNode");
        ///        n.Shape = Shapes.Rectangle;
        ///        n.IsLabelEditable = true;
        ///        n.Label = "Start";
        ///        n.OffsetX = 150;
        ///        n.OffsetY = 25;
        ///        n.Width = 150;
        ///        n.Height = 75;
        ///        n.ToolTip="New Node";
        ///        Model.Nodes.Add(n);
        ///        //Define a Custom port for the node.
        ///        ConnectionPort port = new ConnectionPort();
        ///        //Specifies the node
        ///        port.Node=n;
        ///        //Specifies the left position of the port.
        ///        port.Left=75;
        ///        //Specifies the top position of the port.
        ///        port.Top=10;
        ///        //Specifies the port shape
        ///        port.PortShape = PortShapes.Arrow;
        ///        n.Ports.Add(port);
        ///    }
        ///    }
        ///    }
        /// </code>
        /// </example>
        public double Top
        {
            get { return (double)GetValue(TopProperty); }
            set { SetValue(TopProperty, value); }
        }

        internal double PxTop
        {
            get { return (double)GetValue(PxTopProperty); }
            set { SetValue(PxTopProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PxTop.  This enables animation, styling, binding, etc...
        internal static readonly DependencyProperty PxTopProperty =
            DependencyProperty.Register("PxTop", typeof(double), typeof(ConnectionPort));

        /// <summary>
        /// Gets or sets a value indicating whether this instance is drag over port.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is drag over port; otherwise, <c>false</c>.
        /// </value>
        internal bool IsDragOverPort
        {
            get { return (bool)GetValue(IsDragOverPortProperty); }
            set { SetValue(IsDragOverPortProperty, value); }
        }


        /// <summary>
        /// Gets or sets the custom path style.
        /// </summary>
        /// <value>The custom path style.</value>
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
        /// <summary>
        /// Gets or sets the port style.
        /// </summary>
        /// <value>
        /// Type: <see cref="PortStyle"/>
        /// The style to be applied.</value>
        /// <remarks> Several customizable properties have been provided which can be accessed using the <see cref="PortStyle"/> class.</remarks>
        /// <example>
        /// C#:
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
        ///        InitializeComponent ();
        ///        Control = new DiagramControl ();
        ///        Model = new DiagramModel ();
        ///        View = new DiagramView ();
        ///        Control.View = View;
        ///        Control.Model = Model;
        ///        View.Bounds = new Thickness(0, 0, 1000, 1000);
        ///        //Creates a node
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
        ///        //Define a Custom port for the node.
        ///        ConnectionPort port = new ConnectionPort();
        ///        port.Node=n;
        ///        port.Left=75;
        ///        port.Top=10;
        ///        port.PortShape = PortShapes.Arrow;
        ///        //Specifies the port style.
        ///        port.PortStyle.Fill = Brushes.Red;
        ///        port.PortStyle.Stroke = Brushes.Orange;
        ///        port.PortStyle.StrokeThickness = 2;
        ///        n.Ports.Add(port);
        ///    }
        ///    }
        ///    }
        /// </code>
        /// </example>
        /// <seealso cref="PortStyle"/>
        public PortStyle PortStyle
        {
            get { return (PortStyle)GetValue(PortStyleProperty); }
            set { SetValue(PortStyleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PortStyle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PortStyleProperty =
            DependencyProperty.Register("PortStyle", typeof(PortStyle), typeof(ConnectionPort));

        //public PortStyle PortStyle
        //{
        //    get
        //    {
        //        if (customPortStyle != null)
        //        {
        //            return customPortStyle;
        //        }
        //        else
        //        {
        //            customPortStyle = new PortStyle();
        //        }

        //        return customPortStyle;
        //    }

        //    set
        //    {
        //        customPortStyle = value;
        //    }
        //}



        
        

        /// <summary>
        /// Gets or sets the center position.
        /// </summary>
        /// <value>The center position.</value>
        internal Point CenterPosition
        {
            get
            {
                return (Point)GetValue(CenterPositionProperty);
            }

            set
            {
                SetValue(CenterPositionProperty, value);
            }
        }

        internal Point PagePosition
        {
            get
            {
                if (Node != null && diagramPage != null)
                {
                    return Node.TranslatePoint(CenterPosition, this.diagramPage);
                }
                else if (Node != null)
                {
                    return new Point(Node.PxOffsetX + CenterPosition.X, Node.PxOffsetY + CenterPosition.Y);
                }
                else
                {
                    return new Point(0, 0);
                }
            }
        }

        /// <summary>
        /// Gets or sets the previous port point.
        /// </summary>
        /// <value>The previous port point.</value>
        internal Point PreviousPortPoint
        {
            get
            {
                return m_previousOriginPoint;
            }

            set
            {
                if (m_previousOriginPoint != value)
                {
                    m_previousOriginPoint = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the port pointer.
        /// </summary>
        /// <value>The port pointer.</value>
        internal Border PortPointer
        {
            get
            {
                return customportPointer;
            }

            set
            {
                if (customportPointer != value)
                {
                    customportPointer = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the current port point.
        /// </summary>
        /// <value>The current port point.</value>
        internal Point CurrentPortPoint
        {
            get
            {
                return endpoint;
            }

            set
            {
                if (endpoint != value)
                {
                    endpoint = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the identifying name of the element. The name provides a reference so that code-behind, such as event handler code, can refer to a markup element after it is constructed during processing by a XAML processor. This is a dependency property.
        /// </summary>
        /// <value></value>
        /// <returns>The name of the element. The default is an empty string.</returns>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new string Name
        {
            get { return m_name; }
            set { m_name = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the mouse is over <see cref="ConnectionPort"/> .
        /// </summary>
        /// <value><c>true</c> if the mouse is over <see cref="ConnectionPort"/>; otherwise, <c>false</c>.</value>
        internal bool Ismouseover
        {
            get { return m_mouseover; }
            set { m_mouseover = value; }
        }

        /// <summary>
        /// Gets or sets the port node reference.
        /// </summary>
        /// <value>The port node reference.</value>
        /// <remarks>Used for serialization purpose</remarks>
        public int PortNodeReference
        {
            get { return noderef; }
            set { noderef = value; }
        }

        /// <summary>
        /// Gets or sets the port reference no.
        /// </summary>
        /// <value>The port reference no.</value>
        /// <remarks>
        /// Used for serialization purpose.
        /// </remarks>
        public int PortReferenceNo
        {
            get { return portref; }
            set { portref = value; }
        }

        /// <summary>
        /// Gets or sets the center port reference no.
        /// </summary>
        /// <value>The center port reference no.</value>
        /// <remarks>
        /// Used for serialization purpose.
        /// </remarks>
        public int CenterPortReferenceNo
        {
            get { return cportref; }
            set { cportref = value; }
        }

        #endregion

        #region DPs

        /// <summary>
        /// Identifies the CenterPosition dependency property.
        /// </summary>
        public static readonly DependencyProperty CenterPositionProperty = DependencyProperty.Register("CenterPosition", typeof(Point), typeof(ConnectionPort), new UIPropertyMetadata(new Point(0, 0), new PropertyChangedCallback(OnCenterPositionChanged)));

        /// <summary>
        /// Identifies the IsDragOverPort dependency property.
        /// </summary>
        public static readonly DependencyProperty IsDragOverPortProperty = DependencyProperty.Register("IsDragOverPort", typeof(bool), typeof(ConnectionPort), new FrameworkPropertyMetadata(false));

        /// <summary>
        /// Identifies the Top dependency property.
        /// </summary>
        public static readonly DependencyProperty TopProperty = DependencyProperty.Register("Top", typeof(double), typeof(ConnectionPort), new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(OnOffsetYChanged)));

        /// <summary>
        /// Identifies the Left dependency property.
        /// </summary>
        public static readonly DependencyProperty LeftProperty = DependencyProperty.Register("Left", typeof(double), typeof(ConnectionPort), new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(OnOffsetXChanged)));
        /// <summary>
        /// Identifies the PortShape dependency property.
        /// </summary>
        public static readonly DependencyProperty PortShapeProperty = DependencyProperty.Register("PortShape", typeof(PortShapes), typeof(ConnectionPort), new FrameworkPropertyMetadata(PortShapes.Circle));

        public static readonly DependencyProperty CustomPathStyleProperty = DependencyProperty.Register("CustomPathStyle", typeof(Style), typeof(ConnectionPort), new FrameworkPropertyMetadata(null));
        public static readonly DependencyProperty NodeProperty = DependencyProperty.Register("Node", typeof(Node), typeof(ConnectionPort), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(OnNodeChanged)));
        #endregion

        #region Events
        /// <summary>
        /// Called when [offset X changed].
        /// </summary>
        /// <param name="d">The dependency Object.</param>
        /// <param name="e">The <see cref="System.Windows.DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void OnOffsetXChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ConnectionPort port = d as ConnectionPort;
            try
            {
                if (port.Node != null)
                {
                    foreach (IEdge line in port.Node.Edges)
                    {
                        (line as LineConnector).UpdateConnectorPathGeometry();
                    }
                    TranslateTransform tr = new TranslateTransform(port.Left - port.ActualWidth / 2, port.Top - port.ActualHeight / 2);
                    port.RenderTransform = tr;
                }
            }
            catch
            {
            }
        }

        private static void OnNodeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ConnectionPort port = d as ConnectionPort;

            if (port.PortReferenceNo < 1)
            {
                port.PortReferenceNo = port.Node.countpno + 1;
                port.Node.countpno++;
            }
        }

        /// <summary>
        /// Called when [offset Y changed].
        /// </summary>
        /// <param name="d">The dependency Object..</param>
        /// <param name="e">The <see cref="System.Windows.DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void OnOffsetYChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ConnectionPort port = d as ConnectionPort;
            try
            {
                if (port.Node != null)
                {
                    foreach (IEdge line in port.Node.Edges)
                    {
                        (line as LineConnector).UpdateConnectorPathGeometry();
                    }
                    TranslateTransform tr = new TranslateTransform(port.Left - port.ActualWidth / 2, port.Top - port.ActualHeight / 2);
                    port.RenderTransform = tr;
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// Called when [center position changed].
        /// </summary>
        /// <param name="d">The dependency Object.</param>
        /// <param name="e">The <see cref="System.Windows.DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void OnCenterPositionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ConnectionPort port = d as ConnectionPort;
            try
            {
                if (port.Node != null)
                {
                    foreach (IEdge line in port.Node.Edges)
                    {
                        (line as LineConnector).UpdateConnectorPathGeometry();
                    }

                    port.Node.InvalidateMeasure();
                    port.Node.InvalidateArrange();
                    port.Node.InvalidateVisual();
                }
            }
            catch
            {
            }
        }

        #endregion
        #region
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raised when the appropriate property changes.
        /// </summary>
        /// <param name="name">The property name </param>
        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
            DiagramView dview = Node.GetDiagramView(this);
            if (dview != null)
            {
                if (dview.IsPageSaved)
                {
                    foreach (DiagramProperty d in dview.DiagramProperties.Where(item => item.ObjectType.Equals(typeof(ConnectionPort))))
                    {
                        if (d.PropertyName.Equals(name))
                        {
                            dview.IsPageSaved = false;
                        }
                    }
                }
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
                handler(this, new PropertyChangedEventArgs(e.Property.ToString()));
            }
            DiagramView dview = Node.GetDiagramView(this);
            if (dview != null && this.IsLoaded)
            {
                if (dview.IsPageSaved)
                {
                    foreach (DiagramProperty d in dview.DiagramProperties.Where(item => item.ObjectType.Equals(typeof(ConnectionPort))))
                    {
                        if (d.PropertyName.Equals(e.Property.Name))
                        {
                            dview.IsPageSaved = false;
                        }
                    }
                }
            }
        }

        #endregion


        #region Overrides

        /// <summary>
        /// Invoked whenever application code or internal processes call
        /// <see cref="System.Windows.FrameworkElement.ApplyTemplate"/> method.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            dc = DiagramPage.GetDiagramControl((FrameworkElement)this);
        }

        /// <summary>
        /// Provides class handling for the <see cref="E:System.Windows.UIElement.MouseMove"/> event.
        /// </summary>
        /// <param name="e">The event data.</param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            try
            {
                foreach (ConnectionPort port in this.Node.Ports)
                {
                    if (port == this)
                    {
                        port.Ismouseover = true;
                    }
                    else
                    {
                        port.Ismouseover = false;
                    }
                }
            }
            catch
            {
            }
        }
        #endregion

        #region Methods

        internal Dock Direction()
        {
            //Dock d = Dock.Top;
            System.Windows.Shapes.Polygon poly = new System.Windows.Shapes.Polygon();
            Point one, two, three, four, center, port;
            one = new Point(Node.PxOffsetX, Node.PxOffsetY);
            two = new Point(Node.PxOffsetX + Node.ActualWidth, Node.PxOffsetY);
            three = new Point(Node.PxOffsetX + Node.ActualWidth, Node.PxOffsetY + Node.ActualHeight);
            four = new Point(Node.PxOffsetX, Node.PxOffsetY + Node.ActualHeight);
            port = new Point(Node.PxOffsetX + this.Left, Node.PxOffsetY + this.Top);
            center = new Point(Node.PxOffsetX + Node.ActualWidth / 2, Node.PxOffsetY + Node.ActualHeight / 2);
            ////Right
            //if (IsPointWithin(port, center, two, three))
            //{
            //    return Dock.Right;
            //}
            ////Bottom
            //else if (IsPointWithin(port, center, three, four))
            //{
            //    return Dock.Bottom;
            //}
            ////Left
            //else if (IsPointWithin(port, center, four, one))
            //{
            //    return Dock.Left;
            //}
            ////Top
            //else if (IsPointWithin(port, center, one, two))
            //{
            //    return Dock.Top;
            //}
            //else
            //{
            //    //Somthing wrong.
            //    return Dock.Right;
            //}
            double angle = LineConnector.findAngle(center, port);
            if (45 > angle || angle > 315)
            {
                return Dock.Right;
            }
            else if (135 > angle && angle > 45)
            {
                return Dock.Bottom;
            }
            else if (225 > angle && angle > 135)
            {
                return Dock.Left;
            }
            else if (315 > angle && angle > 225)
            {
                return Dock.Top;
            }
            else
            {
                //Somthing wrong.
                return Dock.Right;
            }
        }

        private bool IsPointWithin(Point port, Point A, Point B, Point C)
        {
            return (TriangleArea(A, B, C) ==
                TriangleArea(port, A, B) + TriangleArea(port, B, C) + TriangleArea(port, C, A));
        }

        private double TriangleArea(Point A, Point B, Point C)
        {
            double AB, BC, CA;
            AB = (A - B).Length;
            BC = (B - C).Length;
            CA = (C - A).Length;

            double s = (AB + BC + CA) / 2;
            return Math.Sqrt(s * (s - AB) * (s - BC) * (s - CA));
        }

        /// <summary>
        /// Handles the DragDelta event of the PortThumb control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Controls.Primitives.DragDeltaEventArgs"/> instance containing the event data.</param>
        private void PortThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            double x, y;
            if (dc.View.IsPageEditable)
            {
                node = this.DataContext as Node;
                if (node.AllowPortDrag)
                {
                    CurrentPortPoint = Mouse.GetPosition(this.DataContext as Node);
                    this.diagramPage = VisualTreeHelper.GetParent(node) as DiagramPage;
                    Left = CurrentPortPoint.X;
                    Top = CurrentPortPoint.Y;
                    if (CurrentPortPoint.X >= 0 && CurrentPortPoint.X < node.Width)
                    {
                        x = CurrentPortPoint.X;
                    }
                    else
                    {
                        if (CurrentPortPoint.X < 0)
                        {
                            x = 0;
                            Left = 0;
                        }
                        else
                        {
                            x = node.Width;
                            Left = node.Width;
                        }
                    }

                    if (CurrentPortPoint.Y >= 0 && CurrentPortPoint.Y < node.Height)
                    {
                        y = CurrentPortPoint.Y;
                    }
                    else
                    {
                        if (CurrentPortPoint.Y < 0)
                        {
                            y = 0;
                            Top = 0;
                        }
                        else
                        {
                            y = node.Height;
                            Top = node.Height;
                        }
                    }

                    TranslateTransform tr = new TranslateTransform(x - this.ActualWidth / 2, y - this.ActualHeight / 2);
                    this.RenderTransform = tr;
                }
            }
        }
        #endregion
    }
}
