// <copyright file="CustomEventHandlers.cs" company="Syncfusion">
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
using System.Collections.ObjectModel;
using System.Collections;

namespace Syncfusion.Windows.Diagram
{
    /// <summary>
    /// Represents PreviewNodeDrop Event Handler. Invoked just before the node object is created in the Drop opeartion.
    /// </summary>
    /// <param name="sender">The sender Object.</param>
    /// <param name="evtArgs">The <see cref="T:Syncfusion.Windows.Diagram.LabelRoutedEventArgs"/> instance containing the event data.</param>
    public delegate void PreviewNodeDropEventHandler(object sender, PreviewNodeDropEventRoutedEventArgs evtArgs);

    /// <summary>
    /// Represents PreviewNodeDrop Event argument class.
    /// </summary>
    public class PreviewNodeDropEventRoutedEventArgs : RoutedEventArgs
    {
        /// <summary>
        /// Used to store the head node
        /// </summary>
        private IShape h_node;

        /// <summary>
        /// Initializes a new instance of the <see cref="PreviewNodeDropEventRoutedEventArgs"/> class.
        /// </summary>
        public PreviewNodeDropEventRoutedEventArgs()
        {
        }

        /// <summary>
        /// Gets or sets the Node object.
        /// <value>
        /// Node object.<see cref="Node"/>
        /// </value>
        /// </summary>
        public IShape Node
        {
            get
            {
                return h_node;
            }

            set
            {
                h_node = value;
            }
        }
    }

    /// <summary>
    /// Represents PreviewConnectorDrop Event Handler. Invoked just before the LineConnector object is created in the Drop opeartion.
    /// </summary>
    /// <param name="sender">The sender Object.</param>
    /// <param name="evtArgs">The <see cref="T:Syncfusion.Windows.Diagram.LabelRoutedEventArgs"/> instance containing the event data.</param>
    public delegate void PreviewConnectorDropEventHandler(object sender, PreviewConnectorDropEventRoutedEventArgs evtArgs);

    /// <summary>
    /// Represents PreviewConnectorDrop Event argument class.
    /// </summary>
    public class PreviewConnectorDropEventRoutedEventArgs : RoutedEventArgs
    {
        /// <summary>
        /// Used to store the head node
        /// </summary>
        private IEdge conn;

        /// <summary>
        /// Initializes a new instance of the <see cref="PreviewConnectorDropEventRoutedEventArgs"/> class.
        /// </summary>
        public PreviewConnectorDropEventRoutedEventArgs()
        {
        }

        /// <summary>
        /// Gets or sets the Node object.
        /// <value>
        /// Node object.<see cref="Node"/>
        /// </value>
        /// </summary>
        public IEdge Connector
        {
            get
            {
                return conn;
            }

            set
            {
                conn = value;
            }
        }
    }

    /// <summary>
    /// Represents Node Label Changed Event Handler.
    /// </summary>
    /// <param name="sender">The sender Object.</param>
    /// <param name="evtArgs">The <see cref="T:Syncfusion.Windows.Diagram.LabelRoutedEventArgs"/> instance containing the event data.</param>
    public delegate void LabelChangedEventHandler(object sender, LabelRoutedEventArgs evtArgs);

    /// <summary>
    /// Represents Node Label Changed Event argument class.
    /// </summary>
    public class LabelRoutedEventArgs : RoutedEventArgs
    {
        /// <summary>
        /// Used to store old value
        /// </summary>
        private string oldval;

        /// <summary>
        /// Used to store new value.
        /// </summary>
        private string newval;

        /// <summary>
        /// Used to store the node
        /// </summary>
        private Node c_node;

        /// <summary>
        /// Initializes a new instance of the <see cref="LabelRoutedEventArgs"/> class.
        /// </summary>
        /// <param name="oldvalue">The old value.</param>
        /// <param name="newvalue">The new value.</param>
        /// <param name="node">The node object.</param>
        public LabelRoutedEventArgs(string oldvalue, string newvalue, Node node)
        {
            this.OldLabelValue = oldvalue;
            this.NewLabelValue = newvalue;
            this.Node = node;
        }

        /// <summary>
        /// Gets or sets the old label value
        /// </summary>
        public string OldLabelValue
        {
            get
            {
                return oldval;
            }

            set
            {
                oldval = value;
            }
        }

        /// <summary>
        /// Gets or sets the new label value.
        /// </summary>
        public string NewLabelValue
        {
            get
            {
                return newval;
            }

            set
            {
                newval = value;
            }
        }

        /// <summary>
        /// Gets or sets the Node object.
        /// <value>
        /// Node object.<see cref="Node"/>
        /// </value>
        /// </summary>
        public Node Node
        {
            get
            {
                return c_node;
            }

            set
            {
                c_node = value;
            }
        }
    }

    /// <summary>
    /// Represents Connector Label Changed Event Handler.
    /// </summary>
    /// <param name="sender">The sender Object.</param>
    /// <param name="evtArgs">The <see cref="Syncfusion.Windows.Diagram.LabelConnRoutedEventArgs"/> instance containing the event data.</param>
    public delegate void LabelConnChangedEventHandler(object sender, LabelConnRoutedEventArgs evtArgs);

    /// <summary>
    /// Represents Connector Label Changed Event argument class.
    /// </summary>
    public class LabelConnRoutedEventArgs : RoutedEventArgs
    {
        /// <summary>
        /// Used to store old value
        /// </summary>
        private string oldval;

        /// <summary>
        /// Used to store new value.
        /// </summary>
        private string newval;

        /// <summary>
        /// Used to store the head node
        /// </summary>
        private Node h_node;

        /// <summary>
        /// Used to store the tail node
        /// </summary>
        private Node t_node;

        /// <summary>
        /// Used to store the connector
        /// </summary>
        private LineConnector conn;

        /// <summary>
        /// Initializes a new instance of the <see cref="LabelConnRoutedEventArgs"/> class.
        /// </summary>
        /// <param name="oldvalue">The old value.</param>
        /// <param name="newvalue">The new value.</param>
        /// <param name="hnode">The head node.</param>
        /// <param name="tnode">The tail node.</param>
        /// <param name="connector">The connector.</param>
        public LabelConnRoutedEventArgs(string oldvalue, string newvalue, Node hnode, Node tnode, LineConnector connector)
        {
            this.OldLabelValue = oldvalue;
            this.NewLabelValue = newvalue;
            this.HeadNode = hnode;
            this.TailNode = tnode;
            this.Connector = connector;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LabelConnRoutedEventArgs"/> class.
        /// </summary>
        /// <param name="oldvalue">The old value.</param>
        /// <param name="newvalue">The new value.</param>
        /// <param name="tnode">The tail node.</param>
        /// <param name="connector">The connector.</param>
        public LabelConnRoutedEventArgs(string oldvalue, string newvalue, Node tnode, LineConnector connector)
        {
            this.OldLabelValue = oldvalue;
            this.NewLabelValue = newvalue;
            this.TailNode = tnode;
            this.Connector = connector;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LabelConnRoutedEventArgs"/> class.
        /// </summary>
        /// <param name="oldvalue">The old value.</param>
        /// <param name="newvalue">The new value.</param>
        /// <param name="connector">The connector.</param>
        /// <param name="hnode">The head node.</param>
        public LabelConnRoutedEventArgs(string oldvalue, string newvalue, LineConnector connector, Node hnode)
        {
            this.OldLabelValue = oldvalue;
            this.NewLabelValue = newvalue;
            this.HeadNode = hnode;
            this.Connector = connector;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LabelConnRoutedEventArgs"/> class.
        /// </summary>
        /// <param name="oldvalue">The old value.</param>
        /// <param name="newvalue">The new value.</param>
        /// <param name="connector">The connector.</param>
        public LabelConnRoutedEventArgs(string oldvalue, string newvalue, LineConnector connector)
        {
            this.OldLabelValue = oldvalue;
            this.NewLabelValue = newvalue;
            this.Connector = connector;
        }

        /// <summary>
        /// Gets or sets the Connector object.
        /// <value>
        /// Connector object.<see cref="LineConnector"/>
        /// </value>
        /// </summary>
        public LineConnector Connector
        {
            get
            {
                return conn;
            }

            set
            {
                conn = value;
            }
        }

        /// <summary>
        /// Gets or sets the old label value.
        /// </summary>
        public string OldLabelValue
        {
            get
            {
                return oldval;
            }

            set
            {
                oldval = value;
            }
        }

        /// <summary>
        /// Gets or sets the new label value.
        /// </summary>
        public string NewLabelValue
        {
            get
            {
                return newval;
            }

            set
            {
                newval = value;
            }
        }

        /// <summary>
        /// Gets or sets the HeadNode object.
        /// <value>
        /// Node object.<see cref="Node"/>
        /// </value>
        /// </summary>
        public Node HeadNode
        {
            get
            {
                return h_node;
            }

            set
            {
                h_node = value;
            }
        }

        /// <summary>
        /// Gets or sets the TailNode object.
        /// <value>
        /// Node object.<see cref="Node"/>
        /// </value>
        /// </summary>
        public Node TailNode
        {
            get
            {
                return t_node;
            }

            set
            {
                t_node = value;
            }
        }
    }

    /// <summary>
    /// Represents  Label Edit Event Handler.
    /// </summary>
    /// <param name="sender">The sender Object.</param>
    /// <param name="evtArgs">The <see cref="Syncfusion.Windows.Diagram.LabelEditConnRoutedEventArgs"/> instance containing the event data.</param>
    public delegate void LabelEditConnChangedEventHandler(object sender, LabelEditConnRoutedEventArgs evtArgs);

    /// <summary>
    /// Represents  Label Edit Event argument class.
    /// </summary>
    public class LabelEditConnRoutedEventArgs : RoutedEventArgs
    {
        /// <summary>
        /// Used to store old value
        /// </summary>
        private string oldval;

        /// <summary>
        /// Used to store the head node
        /// </summary>
        private Node h_node;

        /// <summary>
        /// Used to store the tail node
        /// </summary>
        private Node t_node;

        /// <summary>
        /// Used to store the connector
        /// </summary>
        private LineConnector conn;

        /// <summary>
        /// Initializes a new instance of the <see cref="LabelEditConnRoutedEventArgs"/> class.
        /// </summary>
        /// <param name="oldvalue">The old value.</param>
        /// <param name="connector">The connector.</param>
        public LabelEditConnRoutedEventArgs(string oldvalue, LineConnector connector)
        {
            this.OldLabelValue = oldvalue;
            this.Connector = connector;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LabelEditConnRoutedEventArgs"/> class.
        /// </summary>
        /// <param name="oldvalue">The old value.</param>
        /// <param name="connector">The connector.</param>
        /// <param name="hnode">The head node.</param>
        public LabelEditConnRoutedEventArgs(string oldvalue, LineConnector connector, Node hnode)
        {
            this.OldLabelValue = oldvalue;
            this.HeadNode = hnode;
            this.Connector = connector;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LabelEditConnRoutedEventArgs"/> class.
        /// </summary>
        /// <param name="oldvalue">The old value.</param>
        /// <param name="tnode">The tail node.</param>
        /// <param name="connector">The connector.</param>
        public LabelEditConnRoutedEventArgs(string oldvalue, Node tnode, LineConnector connector)
        {
            this.OldLabelValue = oldvalue;
            this.TailNode = tnode;
            this.Connector = connector;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LabelEditConnRoutedEventArgs"/> class.
        /// </summary>
        /// <param name="oldvalue">The old value.</param>
        /// <param name="hnode">The head node.</param>
        /// <param name="tnode">The tail node.</param>
        /// <param name="connector">The connector.</param>
        public LabelEditConnRoutedEventArgs(string oldvalue, Node hnode, Node tnode, LineConnector connector)
        {
            this.OldLabelValue = oldvalue;
            this.HeadNode = hnode;
            this.TailNode = tnode;
            this.Connector = connector;
        }

        /// <summary>
        /// Gets or sets the old label value.
        /// </summary>
        public string OldLabelValue
        {
            get
            {
                return oldval;
            }

            set
            {
                oldval = value;
            }
        }

        /// <summary>
        /// Gets or sets the HeadNode object.
        /// <value>
        /// Node object.<see cref="Node"/>
        /// </value>
        /// </summary>
        public Node HeadNode
        {
            get
            {
                return h_node;
            }

            set
            {
                h_node = value;
            }
        }

        /// <summary>
        /// Gets or sets the TailNode object.
        /// <value>
        /// Node object.<see cref="Node"/>
        /// </value>
        /// </summary>
        public Node TailNode
        {
            get
            {
                return t_node;
            }

            set
            {
                t_node = value;
            }
        }

        /// <summary>
        /// Gets or sets the Connector object.
        /// <value>
        /// Connector object.<see cref="LineConnector"/>
        /// </value>
        /// </summary>
        public LineConnector Connector
        {
            get
            {
                return conn;
            }

            set
            {
                conn = value;
            }
        }
    }

    /// <summary>
    /// Represents Node Event Handler.
    /// </summary>
    /// <param name="sender">The sender Object.</param>
    /// <param name="evtArgs">The <see cref="Syncfusion.Windows.Diagram.NodeRoutedEventArgs"/> instance containing the event data.</param>
    public delegate void NodeEventHandler(object sender, NodeRoutedEventArgs evtArgs);

    /// <summary>
    /// Represents Node Event argument class.
    /// </summary>
    public class NodeRoutedEventArgs : RoutedEventArgs
    {
        /// <summary>
        /// Used to store the head node
        /// </summary>
        private Node h_node;

        /// <summary>
        /// Initializes a new instance of the <see cref="NodeRoutedEventArgs"/> class.
        /// </summary>
        /// <param name="hnode">The head node.</param>
        public NodeRoutedEventArgs(Node hnode)
        {
            this.Node = hnode;
        }

        /// <summary>
        /// Gets or sets the Node object.
        /// <value>
        /// Node object.<see cref="Node"/>
        /// </value>
        /// </summary>
        public Node Node
        {
            get
            {
                return h_node;
            }

            set
            {
                h_node = value;
            }
        }
    }

    /// <summary>
    /// Represents Node Changed Event Handler.
    /// </summary>
    /// <param name="sender">The sender Object.</param>
    /// <param name="evtArgs">The <see cref="Syncfusion.Windows.Diagram.NodeChangedRoutedEventArgs"/> instance containing the event data.</param>
    public delegate void NodeChangedEventHandler(object sender, NodeChangedRoutedEventArgs evtArgs);

    /// <summary>
    /// Represents  Node Changed Event argument class.
    /// </summary>
    public class NodeChangedRoutedEventArgs : RoutedEventArgs
    {
        /// <summary>
        /// Used to store the head node
        /// </summary>
        private Node h_node;

        /// <summary>
        /// Used to store the tail node
        /// </summary>
        private Node t_node;

        /// <summary>
        /// Used to store the line connector.
        /// </summary>
        private LineConnector conn;

        /// <summary>
        /// Initializes a new instance of the <see cref="NodeChangedRoutedEventArgs"/> class.
        /// </summary>
        /// <param name="hnode">The head node.</param>
        /// <param name="tnode">The tail node.</param>
        /// <param name="connector">The connector.</param>
        public NodeChangedRoutedEventArgs(Node hnode, Node tnode, LineConnector connector)
        {
            this.PreviousNode = hnode;
            this.CurrentNode = tnode;
            this.Connector = connector;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NodeChangedRoutedEventArgs"/> class.
        /// </summary>
        /// <param name="tnode">The tail node.</param>
        /// <param name="connector">The connector.</param>
        public NodeChangedRoutedEventArgs(Node tnode, LineConnector connector)
        {
            this.CurrentNode = tnode;
            this.Connector = connector;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NodeChangedRoutedEventArgs"/> class.
        /// </summary>
        /// <param name="connector">The connector.</param>
        /// <param name="hnode">The head node.</param>
        public NodeChangedRoutedEventArgs(LineConnector connector, Node hnode)
        {
            this.PreviousNode = hnode;
            this.Connector = connector;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NodeChangedRoutedEventArgs"/> class.
        /// </summary>
        /// <param name="connector">The connector.</param>
        public NodeChangedRoutedEventArgs(LineConnector connector)
        {
            this.Connector = connector;
        }

        /// <summary>
        /// Gets or sets the Connector object.
        /// <value>
        /// Connector object.<see cref="LineConnector"/>
        /// </value>
        /// </summary>
        public LineConnector Connector
        {
            get
            {
                return conn;
            }

            set
            {
                conn = value;
            }
        }

        /// <summary>
        /// Gets or sets the Node object.
        /// <value>
        /// The previous  Node object.<see cref="Node"/>
        /// </value>
        /// </summary>
        public Node PreviousNode
        {
            get
            {
                return h_node;
            }

            set
            {
                h_node = value;
            }
        }

        /// <summary>
        /// Gets or sets the Node object.
        /// <value>
        /// The current Node object.<see cref="Node"/>
        /// </value>
        /// </summary>
        public Node CurrentNode
        {
            get
            {
                return t_node;
            }

            set
            {
                t_node = value;
            }
        }
    }

    /// <summary>
    /// Represents Node Drop Event Handler.
    /// </summary>
    /// <param name="sender">The sender Object.</param>
    /// <param name="evtArgs">The <see cref="Syncfusion.Windows.Diagram.NodeDroppedRoutedEventArgs"/> instance containing the event data.</param>
    public delegate void NodeDroppedEventHandler(object sender, NodeDroppedRoutedEventArgs evtArgs);

    /// <summary>
    /// Represents  Node Drop Event argument class.
    /// </summary>
    public class NodeDroppedRoutedEventArgs : RoutedEventArgs
    {
        /// <summary>
        /// Used to store the head node.
        /// </summary>
        private IShape h_node;

        /// <summary>
        /// Used to store the <see cref="SymbolPaletteItem"/> name.
        /// </summary>
        private string itemname;

        /// <summary>
        /// Initializes a new instance of the <see cref="NodeDroppedRoutedEventArgs"/> class.
        /// </summary>
        /// <param name="node">The node object.</param>
        /// <param name="name">The symbolpalette item name.</param>
        public NodeDroppedRoutedEventArgs(IShape node, string name)
        {
            this.DroppedNode = node;
            this.SymbolPaletteItemName = name;
        }

        /// <summary>
        /// Gets or sets the Node object.
        /// <value>
        /// The dropped Node .<see cref="Node"/>
        /// </value>
        /// </summary>
        public IShape DroppedNode
        {
            get
            {
                return h_node;
            }

            set
            {
                h_node = value;
            }
        }

        /// <summary>
        /// Gets or sets the name of the symbol palette item.
        /// </summary>
        /// <value>The name of the symbol palette item.</value>
        public string SymbolPaletteItemName
        {
            get
            {
                return itemname;
            }

            set
            {
                itemname = value;
            }
        }
    }

    /// <summary>
    /// Represents Connector Drop Event Handler.
    /// </summary>
    /// <param name="sender">The sender Object.</param>
    /// <param name="evtArgs">The <see cref="Syncfusion.Windows.Diagram.ConnectorDroppedRoutedEventArgs"/> instance containing the event data.</param>
    public delegate void ConnectorDroppedEventHandler(object sender, ConnectorDroppedRoutedEventArgs evtArgs);

    /// <summary>
    /// Represents  Connector Drop Event argument class.
    /// </summary>
    public class ConnectorDroppedRoutedEventArgs : RoutedEventArgs
    {
        /// <summary>
        /// Used to store the line connector
        /// </summary>
        private ConnectorBase line;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConnectorDroppedRoutedEventArgs"/> class.
        /// </summary>
        /// <param name="connector">The connector.</param>
        public ConnectorDroppedRoutedEventArgs(ConnectorBase connector)
        {
            this.DroppedConnector = connector;
        }

        /// <summary>
        /// Gets or sets the LineConnector object.
        /// <value>
        /// The dropped Node .<see cref="LineConnector"/>
        /// </value>
        /// </summary>
        public ConnectorBase DroppedConnector
        {
            get
            {
                return line;
            }

            set
            {
                line = value;
            }
        }
    }

    /// <summary>
    /// Represents Connector Changed Event Handler.
    /// </summary>
    /// <param name="sender">The sender Object.</param>
    /// <param name="evtArgs">The <see cref="Syncfusion.Windows.Diagram.ConnRoutedEventArgs"/> instance containing the event data.</param>
    public delegate void ConnChangedEventHandler(object sender, ConnRoutedEventArgs evtArgs);

    /// <summary>
    /// Represents  Connector Event argument class.
    /// </summary>
    public class ConnRoutedEventArgs : RoutedEventArgs
    {
        /// <summary>
        /// Used to store the head node
        /// </summary>
        private Node h_node;

        /// <summary>
        /// Used to store the tail node
        /// </summary>
        private Node t_node;

        /// <summary>
        /// Used to store the line connector.
        /// </summary>
        private LineConnector conn;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConnRoutedEventArgs"/> class.
        /// </summary>
        /// <param name="connector">The connector.</param>
        public ConnRoutedEventArgs(LineConnector connector)
        {
            this.Connector = connector;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConnRoutedEventArgs"/> class.
        /// </summary>
        /// <param name="hnode">The head node.</param>
        /// <param name="tnode">The tail node .</param>
        /// <param name="connector">The connector.</param>
        public ConnRoutedEventArgs(Node hnode, Node tnode, LineConnector connector)
        {
            this.HeadNode = hnode;
            this.TailNode = tnode;
            this.Connector = connector;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConnRoutedEventArgs"/> class.
        /// </summary>
        /// <param name="tnode">The tail node.</param>
        /// <param name="connector">The connector.</param>
        public ConnRoutedEventArgs(Node tnode, LineConnector connector)
        {
            this.TailNode = tnode;
            this.Connector = connector;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConnRoutedEventArgs"/> class.
        /// </summary>
        /// <param name="connector">The connector.</param>
        /// <param name="hnode">The head node.</param>
        public ConnRoutedEventArgs(LineConnector connector, Node hnode)
        {
            this.HeadNode = hnode;
            this.Connector = connector;
        }

        /// <summary>
        /// Gets or sets the Connector object.
        /// <value>
        /// Connector object.<see cref="LineConnector"/>
        /// </value>
        /// </summary>
        public LineConnector Connector
        {
            get
            {
                return conn;
            }

            set
            {
                conn = value;
            }
        }

        /// <summary>
        /// Gets or sets the HeadNode .
        /// <value>
        /// Node object.<see cref="Node"/>
        /// </value>
        /// </summary>
        public Node HeadNode
        {
            get
            {
                return h_node;
            }

            set
            {
                h_node = value;
            }
        }

        /// <summary>
        /// Gets or sets the TailNode .
        /// <value>
        /// Node object.<see cref="Node"/>
        /// </value>
        /// </summary>
        public Node TailNode
        {
            get
            {
                return t_node;
            }

            set
            {
                t_node = value;
            }
        }
    }

    /// <summary>
    /// Represents Connection Drag Start Event Handler.
    /// </summary>
    /// <param name="sender">The sender Object.</param>
    /// <param name="evtArgs">The <see cref="Syncfusion.Windows.Diagram.ConnDragRoutedEventArgs"/> instance containing the event data.</param>
    public delegate void ConnDragChangedEventHandler(object sender, ConnDragRoutedEventArgs evtArgs);

    /// <summary>
    /// Represents  Connector Drag Start Event argument class.
    /// </summary>
    public class ConnDragRoutedEventArgs : RoutedEventArgs
    {
        /// <summary>
        /// Used to store the head node
        /// </summary>
        private Node h_node;

        /// <summary>
        /// Used to store the tail node
        /// </summary>
        private Node t_node;

        /// <summary>
        /// Used to store the line connector.
        /// </summary>
        private LineConnector conn;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConnDragRoutedEventArgs"/> class.
        /// </summary>
        /// <param name="connector">The connector.</param>
        public ConnDragRoutedEventArgs(LineConnector connector)
        {
            this.Connector = connector;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConnDragRoutedEventArgs"/> class.
        /// </summary>
        /// <param name="connector">The connector.</param>
        /// <param name="tnode">The tail node.</param>
        public ConnDragRoutedEventArgs(LineConnector connector, Node tnode)
        {
            this.FixedNodeEnd = tnode;
            this.Connector = connector;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConnDragRoutedEventArgs"/> class.
        /// </summary>
        /// <param name="tnode">The tail node.</param>
        /// <param name="connector">The connector.</param>
        public ConnDragRoutedEventArgs(Node tnode, LineConnector connector)
        {
            this.MovableNodeEnd = tnode;
            this.Connector = connector;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConnDragRoutedEventArgs"/> class.
        /// </summary>
        /// <param name="hnode">The head node.</param>
        /// <param name="tnode">The tail node.</param>
        /// <param name="connector">The connector.</param>
        public ConnDragRoutedEventArgs(Node hnode, Node tnode, LineConnector connector)
        {
            this.FixedNodeEnd = hnode;
            this.MovableNodeEnd = tnode;
            this.Connector = connector;
        }

        /// <summary>
        /// Gets or sets the Connector object.
        /// <value>
        /// Connector object.<see cref="LineConnector"/>
        /// </value>
        /// </summary>
        public LineConnector Connector
        {
            get
            {
                return conn;
            }

            set
            {
                conn = value;
            }
        }

        /// <summary>
        /// Gets or sets the Fixed Node .
        /// <value>
        /// Node object.<see cref="Node"/>
        /// </value>
        /// </summary>
        public Node FixedNodeEnd
        {
            get
            {
                return h_node;
            }

            set
            {
                h_node = value;
            }
        }

        /// <summary>
        /// Gets or sets the MovableNode .
        /// <value>
        /// Node object.<see cref="Node"/>
        /// </value>
        /// </summary>
        public Node MovableNodeEnd
        {
            get
            {
                return t_node;
            }

            set
            {
                t_node = value;
            }
        }
    }

    /// <summary>
    /// Represents Connection Drag End Event Handler.
    /// </summary>
    /// <param name="sender">The sender Object.</param>
    /// <param name="evtArgs">The <see cref="Syncfusion.Windows.Diagram.ConnDragEndRoutedEventArgs"/> instance containing the event data.</param>
    public delegate void ConnDragEndChangedEventHandler(object sender, ConnDragEndRoutedEventArgs evtArgs);

    /// <summary>
    /// Represents  Connector Drag End Event argument class.
    /// </summary>
    public class ConnDragEndRoutedEventArgs : RoutedEventArgs
    {
        /// <summary>
        /// Used to store the head node
        /// </summary>
        private Node h_node;

        /// <summary>
        /// Used to store the tail node
        /// </summary>
        private Node t_node;

        /// <summary>
        /// Used to store the line connector.
        /// </summary>
        private LineConnector conn;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConnDragEndRoutedEventArgs"/> class.
        /// </summary>
        /// <param name="connector">The connector.</param>
        public ConnDragEndRoutedEventArgs(LineConnector connector)
        {
            this.Connector = connector;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConnDragEndRoutedEventArgs"/> class.
        /// </summary>
        /// <param name="connector">The connector.</param>
        /// <param name="tnode">The tail node.</param>
        public ConnDragEndRoutedEventArgs(LineConnector connector, Node tnode)
        {
            this.FixedNodeEnd = tnode;
            this.Connector = connector;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConnDragEndRoutedEventArgs"/> class.
        /// </summary>
        /// <param name="tnode">The tail node.</param>
        /// <param name="connector">The connector.</param>
        public ConnDragEndRoutedEventArgs(Node tnode, LineConnector connector)
        {
            this.HitNodeEnd = tnode;
            this.Connector = connector;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConnDragEndRoutedEventArgs"/> class.
        /// </summary>
        /// <param name="hnode">The head node.</param>
        /// <param name="tnode">The tail node.</param>
        /// <param name="connector">The connector.</param>
        public ConnDragEndRoutedEventArgs(Node hnode, Node tnode, LineConnector connector)
        {
            this.FixedNodeEnd = hnode;
            this.HitNodeEnd = tnode;
            this.Connector = connector;
        }

        /// <summary>
        /// Gets or sets the Connector object.
        /// <value>
        /// Connector object.<see cref="LineConnector"/>
        /// </value>
        /// </summary>
        public LineConnector Connector
        {
            get
            {
                return conn;
            }

            set
            {
                conn = value;
            }
        }

        /// <summary>
        /// Gets or sets the Fixed Node .
        /// <value>
        /// Node object.<see cref="Node"/>
        /// </value>
        /// </summary>
        public Node FixedNodeEnd
        {
            get
            {
                return h_node;
            }

            set
            {
                h_node = value;
            }
        }

        /// <summary>
        /// Gets or sets the HitNode .
        /// <value>
        /// Node object.<see cref="Node"/>
        /// </value>
        /// </summary>
        public Node HitNodeEnd
        {
            get
            {
                return t_node;
            }

            set
            {
                t_node = value;
            }
        }
    }

    /// <summary>
    /// Represents Before Connection Create Event Handler.Invoked just before the connection is created.
    /// </summary>
    /// <param name="sender">The sender Object.</param>
    /// <param name="evtArgs">The <see cref="Syncfusion.Windows.Diagram.BeforeCreateConnectionRoutedEventArgs"/> instance containing the event data.</param>
    public delegate void BeforeCreateConnectionEventHandler(object sender, BeforeCreateConnectionRoutedEventArgs evtArgs);

    /// <summary>
    /// Represents  BeforeCreateConnection Event argument class.
    /// </summary>
    public class BeforeCreateConnectionRoutedEventArgs : RoutedEventArgs
    {
        /// <summary>
        /// Used to store the Line connector
        /// </summary>
        private LineConnector conn;

        /// <summary>
        /// Initializes a new instance of the <see cref="BeforeCreateConnectionRoutedEventArgs"/> class.
        /// </summary>
        /// <param name="connector">The connector.</param>
        public BeforeCreateConnectionRoutedEventArgs(LineConnector connector)
        {
            this.Connector = connector;
        }

        /// <summary>
        /// Gets or sets the Connector object.
        /// <value>
        /// Connector object.<see cref="LineConnector"/>
        /// </value>
        /// </summary>
        public LineConnector Connector
        {
            get
            {
                return conn;
            }

            set
            {
                conn = value;
            }
        }
    }

    /// <summary>
    /// Represents Connection Delete Event Handler.
    /// </summary>
    /// <param name="sender">The sender Object.</param>
    /// <param name="evtArgs">The <see cref="Syncfusion.Windows.Diagram.ConnectionDeleteRoutedEventArgs"/> instance containing the event data.</param>
    public delegate void ConnectionDeleteEventHandler(object sender, ConnectionDeleteRoutedEventArgs evtArgs);

    /// <summary>
    /// Represents  Connector Delete Event argument class.
    /// </summary>
    public class ConnectionDeleteRoutedEventArgs : RoutedEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConnectionDeleteRoutedEventArgs"/> class.
        /// </summary>
        public ConnectionDeleteRoutedEventArgs()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConnectionDeleteRoutedEventArgs"/> class.
        /// </summary>
        /// <param name="line">The line object.</param>
        public ConnectionDeleteRoutedEventArgs(LineConnector line)
        {
            this.DeletedLineConnector = line;
        }

        /// <summary>
        /// Refers to the line instance.
        /// </summary>
        private LineConnector line;

        /// <summary>
        /// Gets or sets the deleted line connector.
        /// </summary>
        /// <value>The deleted line connector.</value>
        public LineConnector DeletedLineConnector
        {
            get
            {
                return line;
            }

            set
            {
                line = value;
            }
        }
    }

    /// <summary>
    /// Represents Node Delete Event Handler.
    /// </summary>
    /// <param name="sender">The sender Object.</param>
    /// <param name="evtArgs">The <see cref="Syncfusion.Windows.Diagram.NodeDeleteRoutedEventArgs"/> instance containing the event data.</param>
    public delegate void NodeDeleteEventHandler(object sender, NodeDeleteRoutedEventArgs evtArgs);

    /// <summary>
    /// Represents  Node Delete Event argument class.
    /// </summary>
    public class NodeDeleteRoutedEventArgs : RoutedEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NodeDeleteRoutedEventArgs"/> class.
        /// </summary>
        /// <param name="deletednode">The deleted node.</param>
        public NodeDeleteRoutedEventArgs(Node deletednode)
        {
            this.DeletedNode = deletednode;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NodeDeleteRoutedEventArgs"/> class.
        /// </summary>
        public NodeDeleteRoutedEventArgs()
        {
        }

        /// <summary>
        /// Refers to the <see cref="Node"/> instance.
        /// </summary>
        private Node node;

        /// <summary>
        /// Gets or sets the deleted node.
        /// </summary>
        /// <value>The deleted node.</value>
        public Node DeletedNode
        {
            get
            {
                return node;
            }

            set
            {
                node = value;
            }
        }
    }


    /// <summary>
    /// Represents Node Nudge Event Handler.
    /// </summary>
    /// <param name="sender">The sender Object.</param>
    /// <param name="evtArgs">The <see cref="Syncfusion.Windows.Diagram.NodeDeleteRoutedEventArgs"/> instance containing the event data.</param>
    public delegate void NodeNudgeEventHandler(object sender, NodeNudgeEventArgs evtArgs);

    /// <summary>
    /// Represents Line Nudge Event Handler.
    /// </summary>
    /// <param name="sender">The sender Object.</param>
    /// <param name="evtArgs">The <see cref="Syncfusion.Windows.Diagram.NodeDeleteRoutedEventArgs"/> instance containing the event data.</param>    
    public delegate void LineNudgeEventHandler(object sender, LineNudgeEventArgs evtArgs);

    /// <summary>
    /// Represents Node Nudge Event argument class.
    /// </summary>
    public class LineNudgeEventArgs : RoutedEventArgs
    {
        public LineConnector Lineconnector;
        public LineNudgeEventArgs()
        {

        }
        public LineNudgeEventArgs(LineConnector sender)
        {
            Lineconnector = sender;
        }

    }

    /// <summary>
    /// Represents Line Nudge Event argument class.
    /// </summary>    
    public class NodeNudgeEventArgs : RoutedEventArgs
    {
        /*public Node HeadNode;
        public Node TailNode;
        public LineConnector lc;*/

        public Node Node;
        public Point oldOffset;
        public Point newOffset;
        public NodeNudgeEventArgs()
        {
        }



        public NodeNudgeEventArgs(Node sender, int dir)
        {
            Node = sender;
            oldOffset = new Point(sender.PxOffsetX, sender.PxOffsetY);
            switch (dir)
            {
                case 1:
                    newOffset = new Point(oldOffset.X, oldOffset.Y - 1);
                    break;
                case 2:
                    newOffset = new Point(oldOffset.X + 1, oldOffset.Y);
                    break;
                case 3:
                    newOffset = new Point(oldOffset.X, oldOffset.Y + 1);
                    break;
                case 4:
                    newOffset = new Point(oldOffset.X - 1, oldOffset.Y);
                    break;
            }
        }
    }



    public delegate void GroupEventHandler(object sender, GroupEventArgs evtArgs);

    public class GroupEventArgs : RoutedEventArgs
    {
        public Group m_Group;
        public Node m_GroupNode;
        public LineConnector m_GroupLineConnector;
        public GroupEventArgs(Group group, Node node,LineConnector line)
        {
            this.Group = group;
            this.GroupNode = node;
            this.GroupLineConnector = line;
        }
        public Group Group
        {
            get
            {
                return (Group)m_Group;
            }
            set
            {
                m_Group = value;
            }
        }
        public Node GroupNode
        {
            get
            {
                return (Node)m_GroupNode;
            }
            set
            {
                m_GroupNode = value;
            }
        }
        public LineConnector GroupLineConnector
        {
            get
            {
                return (LineConnector)m_GroupLineConnector;
            }
            set
            {
                m_GroupLineConnector = value;
            }
        } 
    }

    public delegate void UnGroupEventHandler(object sender, UnGroupEventArgs evtArgs);
    public class UnGroupEventArgs : RoutedEventArgs
    {
        private Group m_Group;
        private Node m_GroupNode;
        private LineConnector m_GroupLineConnector;

        public UnGroupEventArgs(Group group, Node node,LineConnector line)
        {
            this.Group = group;
            this.GroupNode = node;
            this.GroupLineConnector = line;
        }
        public Group Group
        {
            get
            {
                return (Group)m_Group;
            }
            set
            {
                m_Group = value;
            }
        }
        public Node GroupNode
        {
            get
            {
                return (Node)m_GroupNode;
            }
            set
            {
                m_GroupNode = value;
            }
        }
        public LineConnector GroupLineConnector
        {
            get
            {
                return (LineConnector)m_GroupLineConnector;
            }
            set
            {
                m_GroupLineConnector = value;
            }
        } 
    }
    public delegate void ConnectorRoutedEventHandler(object sender, ConnectorRoutedEventArgs evtArgs);
    public delegate void ConnectorSelectedEventHandler(object sender, ConnectorRoutedEventArgs evtArgs);
    public delegate void ConnectorUnSelectedEventHandler(object sender, ConnectorRoutedEventArgs evtArgs);
    public class ConnectorRoutedEventArgs : RoutedEventArgs
    {
        public LineConnector m_Connector;
        public ConnectorRoutedEventArgs(LineConnector connector)
        {
            this.Connector = connector;
        }
        public LineConnector Connector
        {
            get
            {
                return (LineConnector)m_Connector;
            }
            set
            {
                m_Connector = value;
            }
        }
    }

   
    internal delegate void LayerPropertyChangedEventHandler(object sender, LayerPropertyChangedEventArgs evtArgs);

    internal class LayerPropertyChangedEventArgs : RoutedEventArgs
    {
        public string PropertyName;
        public bool PreState;
        public ObservableCollection<Node> Nodes;
        public ObservableCollection<LineConnector> Lines;
        public LayerPropertyChangedEventArgs(string prop, bool pre)
        {
            PropertyName = prop;
            PreState = pre;
        }
        public LayerPropertyChangedEventArgs(string prop, IList a)
        {
            PropertyName = prop;
            object[] o = a.SyncRoot as object[];
            if (o[0] is Node)
            {
                IEnumerable<Node> n = o.Cast<Node>();
                Nodes = new ObservableCollection<Node>(n);
            }
            if (o[0] is LineConnector)
            {
                IEnumerable<LineConnector> l = o.Cast<LineConnector>();
                Lines = new ObservableCollection<LineConnector>(l);
            }
        }
        public LayerPropertyChangedEventArgs(string prop, ObservableCollection<Node> a)
        {
            PropertyName = prop;
            Nodes = a;
        }
        public LayerPropertyChangedEventArgs(string prop, ObservableCollection<LineConnector> a)
        {
            PropertyName = prop;
            Lines = a;
        }
    }
}
