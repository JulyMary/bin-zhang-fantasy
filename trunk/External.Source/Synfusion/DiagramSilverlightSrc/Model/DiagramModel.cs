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
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.Text;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Interop;
    using System.Windows.Media;
    using Syncfusion.Windows.Diagram;

    /// <summary>
    /// Represents the Diagram Model.
    /// </summary>
    /// <remarks>
    /// <para>A model represents data for an application and contains the logic for
    /// adding, accessing, and manipulating the data. Nodes and connectors are added to
    /// the Diagram Control using the Model property. A predefined layout can be applied
    /// using the LayoutType property of DiagramModel or the position of the nodes can
    /// be manually specified.</para>
    /// </remarks>
    /// <example>
    /// <para>The following example shows how to create a <see
    /// cref="DiagramModel">DiagramModel</see> in XAML. </para>
    /// <para></para>
    /// <code>&lt;UserControl x:Class=&quot;Sample.MainPage&quot;
    /// xmlns=&quot;http://schemas.microsoft.com/winfx/2006/xaml/presentation&quot; 
    ///     xmlns:x=&quot;http://schemas.microsoft.com/winfx/2006/xaml&quot; 
    ///     xmlns:syncfusion=&quot;clr-namespace:Syncfusion.Windows.Diagram;assembly=Syncfusion.Diagram.Silverlight&quot;
    ///    xmlns:vsm=&quot;clr-namespace:System.Windows;assembly=System.Windows&quot; 
    ///             &gt;
    ///   &lt;syncfusion:DiagramControl Grid.Column=&quot;1&quot; Name=&quot;diagramControl&quot;
    ///                                IsSymbolPaletteEnabled=&quot;True&quot;
    ///                                Background=&quot;WhiteSmoke&quot;&gt;
    ///             &lt;syncfusion:DiagramControl.Model&gt;
    ///                  &lt;syncfusion:DiagramModel LayoutType=&quot;None&quot;  x:Name=&quot;diagramModel&quot; &gt;
    ///                  &lt;/syncfusion:DiagramModel&gt;
    ///             &lt;/syncfusion:DiagramControl.Model&gt;
    ///           &lt;syncfusion:DiagramControl.View&gt;
    ///              &lt;syncfusion:DiagramView   Background=&quot;LightGray&quot;
    ///                                        Bounds=&quot;0,0,12,12&quot;
    ///                                        Name=&quot;diagramView&quot;  &gt;
    ///              &lt;/syncfusion:DiagramView&gt;
    ///          &lt;/syncfusion:DiagramControl.View&gt;
    ///     &lt;/syncfusion:DiagramControl&gt;
    /// &lt;/UserControl&gt;</code>
    /// <para></para>
    /// <para>The following example shows how to create a <see
    /// cref="DiagramModel">DiagramModel</see> in C#. </para>
    /// <para></para>
    /// <code>using System;
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
    ///        Node n = new Node(Guid.NewGuid(), &quot;Start&quot;);
    ///        n.Shape = Shapes.FlowChart_Start;
    ///        n.IsLabelEditable = true;
    ///        n.Label = &quot;Start&quot;;
    ///        n.Level = 1;
    ///        n.OffsetX = 150;
    ///        n.OffsetY = 25;
    ///        n.Width = 150;
    ///        n.Height = 75;
    ///         Node n1 = new Node(Guid.NewGuid(), &quot;Decision1&quot;);
    ///         n1.Shape = Shapes.FlowChart_Process;
    ///         n1.IsLabelEditable = true;
    ///         n1.Label = &quot;Alarm Rings&quot;;
    ///         n1.Level = 2;
    ///         n1.OffsetX = 150;
    ///         n1.OffsetY = 125;
    ///         n1.Width = 150;
    ///         n1.Height = 75;
    ///        Model.Nodes.Add(n);
    ///        Model.Nodes.Add(n1);
    ///        LineConnector o = new LineConnector();
    ///        o.ConnectorType = ConnectorType.Straight;
    ///        o.TailNode = n1;
    ///        o.HeadNode = n;
    ///        o.HeadDecoratorShape = DecoratorShape.None;
    ///        Model.Connections.Add(o);
    ///    }
    ///    }
    ///    }</code>
    /// </example>
    /// <seealso cref="HierarchicalTreeLayout">HierarchicalTreeLayout</seealso>
    /// <seealso cref="DirectedTreeLayout">DirectedTreeLayout</seealso>
    public partial class DiagramModel : DependencyObject, IDiagramModel, INotifyPropertyChanged
    {

        internal DiagramControl dc;

        /// <summary>
        /// Defines the HierarchicalDataTemplate property.  This is a dependency property.
        /// </summary>
        public static readonly DependencyProperty HierarchicalDataTemplateProperty = DependencyProperty.Register("HierarchicalDataTemplate", typeof(HierarchicalDataTemplate), typeof(DiagramModel), new PropertyMetadata(null));

        /// <summary>
        ///  Defines the ItemsSource dependency property.
        /// </summary>
        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register("ItemsSource", typeof(IEnumerable), typeof(DiagramModel), new PropertyMetadata(OnItemSourceChanged));

        /// <summary>
        ///  Defines the ItemTemplate property.  This is a dependency property.
        /// </summary>
        public static readonly DependencyProperty ItemTemplateProperty = DependencyProperty.Register("ItemTemplate", typeof(DataTemplate), typeof(DiagramModel), new PropertyMetadata(null));

        /// <summary>
        /// Defines the LayoutType property.  This is a dependency property.
        /// </summary>
        public static readonly DependencyProperty LayoutTypeProperty = DependencyProperty.Register("LayoutType", typeof(LayoutType), typeof(DiagramModel), new PropertyMetadata(LayoutType.None));

        /// <summary>
        /// List of the elements that are treated as logical children of control.
        /// </summary>
        private readonly List<UIElement> mlogicalChildren = new List<UIElement>();

        /// <summary>
        /// Collection of all nodes that are given by user.
        /// </summary>
        private readonly CollectionExt m_Children = new CollectionExt();
        
        /// <summary>
        /// Collection of all connections that are to be linked between nodes.
        /// </summary>
        private readonly CollectionExt mconnection = new CollectionExt();

        /// <summary>
        /// Collection of all nodes that are to be managed by the container.
        /// </summary>
        internal CollectionExt m_internalChildren = new CollectionExt();

        /// <summary>
        /// Specifies if any nodes are added.
        /// </summary>
        private bool misch = false;
        
        /// <summary>
        /// Maintaining Root node list from node collection
        /// </summary>
        private CollectionExt mrootNodes = new CollectionExt();

        /// <summary>
        /// Used to store the units.
        /// </summary>
        private MeasureUnits munits;

        /// <summary>
        /// Initializes a new instance of the <see cref="DiagramModel"/> class.
        /// </summary>
        public DiagramModel()
        {
            this.m_Children.CollectionChanged += this.OnChildrenCollectionChanged;
            this.m_internalChildren.CollectionChanged += new NotifyCollectionChangedEventHandler(this.InternalChildren_CollectionChanged);
        }

        /// <summary>
        /// Calls propertychanged event handler.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets or sets the HierarchicalDataTemplate for items.
        /// </summary>
        /// <example>
        /// <para/>The following example shows how to create a <see cref="DiagramModel"/> in XAML.
        /// <code language="XAML">
        /// &lt;Window x:Class="Sample.Window1"
        /// xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        /// xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        /// xmlns:syncfusion="http://schemas.syncfusion.com/wpf"
        ///  WindowState="Maximized" Name="mainwindow" 
        ///  xmlns:local="clr-namespace:Sample" FontWeight="Bold"
        ///  Icon="Images/App.ico" &gt;
        ///  &lt;Window.Resources&gt;
        ///    &lt;!--The business object which contains the data to be binded.--&gt;
        ///    &lt;local:CountrySalesList x:Key="myList"/&gt;
        ///    &lt;!--Creating a hierarchical data from the XML data for generating a tree view--&gt;
        ///    &lt;HierarchicalDataTemplate x:Key="dataTemplate" ItemsSource="{Binding Path=RegionSales}" DataType="{x:Type local:CountrySale}"&gt;
        ///        &lt;HierarchicalDataTemplate.ItemTemplate&gt;
        ///           &lt;HierarchicalDataTemplate ItemsSource="{Binding Path=Earnings}"   DataType="{x:Type local:RegionSale}"&gt;
        ///               &lt;TextBlock Text="{Binding Path=Name}" /&gt;
        ///              &lt;HierarchicalDataTemplate.ItemTemplate&gt;
        ///                   &lt;DataTemplate DataType="{x:Type local:Sale}"&gt;
        ///                       &lt;TextBlock Text="{Binding Path=Name}" /&gt;
        ///                  &lt;/DataTemplate&gt;
        ///              &lt;/HierarchicalDataTemplate.ItemTemplate&gt;
        ///           &lt;/HierarchicalDataTemplate&gt;
        ///       &lt;/HierarchicalDataTemplate.ItemTemplate&gt;
        ///   &lt;/HierarchicalDataTemplate&gt;
        ///  &lt;/Window.Resources>
        ///   &lt;syncfusion:DiagramControl Grid.Column="1" Name="diagramControl" 
        ///                                IsSymbolPaletteEnabled="True" 
        ///                                Background="WhiteSmoke"&gt;
        ///             &lt;syncfusion:DiagramControl.Model&gt;
        ///                  &lt;syncfusion:DiagramModel LayoutType="None" "  ItemsSource="{Binding Source={StaticResource myList}}" HierarchicalDataTemplate="{StaticResource dataTemplate}"  x:Name="diagramModel" &gt;
        ///                  &lt;/syncfusion:DiagramModel&gt;
        ///             &lt;/syncfusion:DiagramControl.Model&gt;
        ///           &lt;syncfusion:DiagramControl.View&gt;
        ///              &lt;syncfusion:DiagramView   Background="LightGray"  
        ///                                        Bounds="0,0,12,12"  
        ///                                        Name="diagramView"  &gt;
        ///              &lt;/syncfusion:DiagramView&gt;
        ///          &lt;/syncfusion:DiagramControl.View&gt;
        ///     &lt;/syncfusion:DiagramControl&gt;
        /// &lt;/Window&gt;
        /// </code>
        /// <para/>The following example shows how to create a <see cref="DiagramModel"/> in C#.
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
        ///        CountrySalesList list= this.Resources["myList"] as CountrySalesList;
        ///        Model.ItemsSource=list;
        ///        Model.HierarchicalDataTemplate=this.Resources["dataTemplate"] as DataTemplate;
        ///    }
        ///    }
        ///    }
        /// </code>
        /// </example>
        /// <seealso cref="HierarchicalTreeLayout"/>
        /// <seealso cref="DirectedTreeLayout"/>
        public HierarchicalDataTemplate HierarchicalDataTemplate
        {
            get
            {
                return (HierarchicalDataTemplate)GetValue(HierarchicalDataTemplateProperty);
            }

            set
            {
                SetValue(HierarchicalDataTemplateProperty, value);
            }
        }       

        /// <summary>
        /// Gets or sets the source for the list of the items, the containers about to represent.
        /// </summary>
        /// <remarks>
        /// Items source can not be set while the Items collection contains manually added items.
        /// </remarks>
        /// <example>
        /// <para/>The following example shows how to create a <see cref="DiagramModel"/> in XAML.
        /// <code language="XAML">
        /// &lt;Window x:Class="Sample.Window1"
        /// xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        /// xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        /// xmlns:syncfusion="http://schemas.syncfusion.com/wpf"
        ///  WindowState="Maximized" Name="mainwindow" 
        ///  xmlns:local="clr-namespace:Sample" FontWeight="Bold"
        ///  Icon="Images/App.ico" &gt;
        ///  &lt;Window.Resources&gt;
        ///    &lt;!--The business object which contains the data to be binded.--&gt;
        ///    &lt;local:CountrySalesList x:Key="myList"/&gt;
        ///    &lt;!--Creating a hierarchical data from the XML data for generating a tree view--&gt;
        ///    &lt;HierarchicalDataTemplate x:Key="dataTemplate" ItemsSource="{Binding Path=RegionSales}" DataType="{x:Type local:CountrySale}"&gt;
        ///        &lt;HierarchicalDataTemplate.ItemTemplate&gt;
        ///           &lt;HierarchicalDataTemplate ItemsSource="{Binding Path=Earnings}"   DataType="{x:Type local:RegionSale}"&gt;
        ///               &lt;TextBlock Text="{Binding Path=Name}" /&gt;
        ///              &lt;HierarchicalDataTemplate.ItemTemplate&gt;
        ///                   &lt;DataTemplate DataType="{x:Type local:Sale}"&gt;
        ///                       &lt;TextBlock Text="{Binding Path=Name}" /&gt;
        ///                  &lt;/DataTemplate&gt;
        ///              &lt;/HierarchicalDataTemplate.ItemTemplate&gt;
        ///           &lt;/HierarchicalDataTemplate&gt;
        ///       &lt;/HierarchicalDataTemplate.ItemTemplate&gt;
        ///   &lt;/HierarchicalDataTemplate&gt;
        ///  &lt;/Window.Resources>
        ///   &lt;syncfusion:DiagramControl Grid.Column="1" Name="diagramControl" 
        ///                                IsSymbolPaletteEnabled="True" 
        ///                                Background="WhiteSmoke"&gt;
        ///             &lt;syncfusion:DiagramControl.Model&gt;
        ///                  &lt;syncfusion:DiagramModel LayoutType="None" "  ItemsSource="{Binding Source={StaticResource myList}}" ItemTemplate="{StaticResource dataTemplate}"  x:Name="diagramModel" &gt;
        ///                  &lt;/syncfusion:DiagramModel&gt;
        ///             &lt;/syncfusion:DiagramControl.Model&gt;
        ///           &lt;syncfusion:DiagramControl.View&gt;
        ///              &lt;syncfusion:DiagramView   Background="LightGray"  
        ///                                        Bounds="0,0,12,12"  
        ///                                        Name="diagramView"  &gt;
        ///              &lt;/syncfusion:DiagramView&gt;
        ///          &lt;/syncfusion:DiagramControl.View&gt;
        ///     &lt;/syncfusion:DiagramControl&gt;
        /// &lt;/Window&gt;
        /// </code>
        /// <para/>The following example shows how to create a <see cref="DiagramModel"/> in C#.
        /// <code language="C#">
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
        ///        CountrySalesList list= this.Resources["myList"] as CountrySalesList;
        ///        Model.ItemsSource=list;
        ///    }
        ///    }
        ///    }
        /// </code>
        /// </example>
        /// <seealso cref="HierarchicalTreeLayout"/>
        /// <seealso cref="DirectedTreeLayout"/>
        public IEnumerable ItemsSource
        {
            get
            {
                return this.m_Children.SourceCollection;
            }

            set
            {
                this.IsAddingNode = true;
                this.m_Children.SourceCollection = value;
                m_Children.CollectionChanged += new NotifyCollectionChangedEventHandler(m_Children_CollectionChanged);
            }
        }

        void m_Children_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (dc != null && dc.View != null)
            {
                ParseItemSourceIntoParser();
                parser.UpdateLayout();
                if (this.LayoutType == LayoutType.HierarchicalTreeLayout)
                {
                    HierarchicalTreeLayout layout = new HierarchicalTreeLayout(this, dc.View);
                    layout.RefreshLayout();
                }
                else
                {
                    DirectedTreeLayout layout = new DirectedTreeLayout(this, dc.View);
                    layout.RefreshLayout();
                }
                m_Children.CollectionChanged -= new NotifyCollectionChangedEventHandler(m_Children_CollectionChanged);
            }
        }

        /// <summary>
        /// Gets or sets the ItemTemplate for items.
        /// </summary>
        /// <example>
        /// <para/>The following example shows how to create a <see cref="DiagramModel"/> in XAML.
        /// <code language="XAML">
        /// &lt;Window x:Class="Sample.Window1"
        /// xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        /// xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        /// xmlns:syncfusion="http://schemas.syncfusion.com/wpf"
        ///  WindowState="Maximized" Name="mainwindow" 
        ///  xmlns:local="clr-namespace:Sample" FontWeight="Bold"
        ///  Icon="Images/App.ico" &gt;
        ///  &lt;Window.Resources&gt;
        ///    &lt;!--The business object which contains the data to be binded . --&gt;
        ///    &lt;local:CountrySalesList x:Key="myList"/&gt;
        ///    &lt;!--Creating a hierarchical data from the XML data for generating a tree view--&gt;
        ///    &lt;HierarchicalDataTemplate x:Key="dataTemplate" ItemsSource="{Binding Path=RegionSales}" DataType="{x:Type local:CountrySale}"&gt;
        ///        &lt;HierarchicalDataTemplate.ItemTemplate&gt;
        ///           &lt;HierarchicalDataTemplate ItemsSource="{Binding Path=Earnings}"   DataType="{x:Type local:RegionSale}"&gt;
        ///               &lt;TextBlock Text="{Binding Path=Name}" /&gt;
        ///              &lt;HierarchicalDataTemplate.ItemTemplate&gt;
        ///                   &lt;DataTemplate DataType="{x:Type local:Sale}"&gt;
        ///                       &lt;TextBlock Text="{Binding Path=Name}" /&gt;
        ///                  &lt;/DataTemplate&gt;
        ///              &lt;/HierarchicalDataTemplate.ItemTemplate&gt;
        ///           &lt;/HierarchicalDataTemplate&gt;
        ///       &lt;/HierarchicalDataTemplate.ItemTemplate&gt;
        ///   &lt;/HierarchicalDataTemplate&gt;
        ///  &lt;/Window.Resources>
        ///   &lt;syncfusion:DiagramControl Grid.Column="1" Name="diagramControl" 
        ///                                IsSymbolPaletteEnabled="True" 
        ///                                Background="WhiteSmoke"&gt;
        ///             &lt;syncfusion:DiagramControl.Model&gt;
        ///                  &lt;syncfusion:DiagramModel LayoutType="None" "  ItemsSource="{Binding Source={StaticResource myList}}" ItemTemplate="{StaticResource dataTemplate}"  x:Name="diagramModel" &gt;
        ///                  &lt;/syncfusion:DiagramModel&gt;
        ///             &lt;/syncfusion:DiagramControl.Model&gt;
        ///           &lt;syncfusion:DiagramControl.View&gt;
        ///              &lt;syncfusion:DiagramView   Background="LightGray"  
        ///                                        Bounds="0,0,12,12"  
        ///                                        Name="diagramView"  &gt;
        ///              &lt;/syncfusion:DiagramView&gt;
        ///          &lt;/syncfusion:DiagramControl.View&gt;
        ///     &lt;/syncfusion:DiagramControl&gt;
        /// &lt;/Window&gt;
        /// </code>
        /// <para/>The following example shows how to create a <see cref="DiagramModel"/> in C#.
        /// <code language="C#">
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
        ///        CountrySalesList list= this.Resources["myList"] as CountrySalesList;
        ///        Model.ItemsSource=list;
        ///        Model.ItemTemplate=this.Resources["dataTemplate"] as DataTemplate;
        ///    }
        ///    }
        ///    }
        /// </code>
        /// </example>
        /// <seealso cref="HierarchicalTreeLayout"/>
        /// <seealso cref="DirectedTreeLayout"/>
        public DataTemplate ItemTemplate
        {
            get
            {
                return (DataTemplate)GetValue(ItemTemplateProperty);
            }

            set
            {
                SetValue(ItemTemplateProperty, value);
            }
        }

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
                return this.munits;
            }

            set
            {
                if (this.munits != value)
                {
                    this.munits = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the root nodes.
        /// </summary>
        /// <value>The root nodes.</value>
        internal CollectionExt RootNodes
        {
            get
            {
                return this.mrootNodes;
            }

            set
            {
                this.mrootNodes = value;
            }
        }

        /// <summary>
        /// Gets or sets the Internal nodes.
        /// Maintaining Nodes Collection which is added through Nodes property and ItemsSource property.
        /// This will provide feature for adding nodes in diagram model through ItemSource and Nodes property at same time.
        /// Type: <see cref="CollectionExt"/>
        /// </summary>
        /// <value>The internal nodes.</value>
        internal CollectionExt InternalNodes
        {
            get
            {
                return this.m_internalChildren;
            }

            set
            {
                this.m_internalChildren = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is adding node.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is adding node; otherwise, <c>false</c>.
        /// </value>
        internal bool IsAddingNode
        {
            get { return this.misch; }
            set { this.misch = value; }
        }        

        /// <summary>
        /// Parses the items from the items source. Used in case of partial trust support.
        /// </summary>
        /// <param name="element">The element.</param>
       /* internal void ParseItemSourceIntoParser(UIElement element)
        {
            DataParser parser = new DataParser();
            parser.ItemsSource = this.ItemsSource;
            if (this.HierarchicalDataTemplate != null)
            {
                parser.ItemTemplate = this.HierarchicalDataTemplate;
            }
            else
            {
                parser.ItemTemplate = this.ItemTemplate;
            }

            Grid grid = new Grid();
            grid.Width = 10;
            grid.Height = 10;
            grid.Children.Add(parser);
            Popup popup = new Popup();
            popup.Width = 0;
            popup.Height = 0;
            popup.Child = grid;
            popup.IsOpen = true;
            popup.IsOpen = false;
            if (this.IsAddingNode && this.LayoutType != LayoutType.None)
            {
                this.Connections.Clear();
                this.Nodes.Clear();
            }

            this.RootNodes = this.ConvertToNodes(parser.ChildItems);
            this.IsAddingNode = false;
        }*/

        /// <summary>
        /// Raised when the appropriate property changes.
        /// </summary>
        /// <param name="name">The property name.</param>
        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

        /// <summary>
        /// Called when [connections changed].
        /// </summary>
        /// <param name="d">The DependencyObject.</param>
        /// <param name="e">The <see cref="System.Windows.DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void OnConnectionsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DiagramModel dcontrol = d as DiagramModel;
        }

        /// <summary>
        /// Called when [ItemSource  changed].
        /// </summary>
        /// <param name="d">Dependency object.</param>
        /// <param name="e">The <see cref="System.Collections.Specialized.NotifyCollectionChangedEventArgs"/> instance containing the event data.</param>
        private static void OnItemSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DiagramModel dcontrol = d as DiagramModel;
            if (e.NewValue != e.OldValue)
            {
                dcontrol.m_Children.SourceCollection = e.NewValue as IEnumerable;
            }
        }

        /// <summary>
        /// Called when [nodes changed].
        /// </summary>
        /// <param name="d">The DependencyObject.</param>
        /// <param name="e">The <see cref="System.Windows.DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void OnNodesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DiagramModel dcontrol = d as DiagramModel;
        }

        /// <summary>
        /// Adds the logical children.
        /// </summary>
        /// <param name="iList">The IList collection.</param>
        private void AddLogicalChildren(IList ilist)
        {
        }

        ///// <summary>
        ///// Converts the ChildDataParser items to nodes.
        ///// </summary>
        ///// <param name="items">ChildDataParser items.</param>
        ///// <returns>The converted nodes</returns>
        //private CollectionExt ConvertToNodes(List<ChildDataParser> items)
        //{
        //    CollectionExt nodes = new CollectionExt();
        //    CollectionExt childs = new CollectionExt();
        //    foreach (ChildDataParser child in items)
        //    {
        //        Node node = new Node();
        //        node.Content = child.Header;
        //        node.Model = this;
        //        childs = this.ConvertToNodes(child.ChildItems);
        //        nodes.Add(node);
        //        this.HookConnection(node, childs);
        //    }

        //    return nodes;
        //}

        //internal MeasureUnits MeasurementUnits
        //{
        //    get
        //    {
        //        return (MeasureUnits)GetValue(MeasurementUnitsProperty);
        //    }

        //    set
        //    {
        //        SetValue(MeasurementUnitsProperty, value);
        //    }
        //}

        //public static readonly DependencyProperty MeasurementUnitsProperty = DependencyProperty.Register("MeasurementUnits", typeof(MeasureUnits), typeof(DiagramView), new PropertyMetadata(MeasureUnits.Pixel, new PropertyChangedCallback(OnUnitsChanged)));

        private static void OnUnitsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DiagramModel model = d as DiagramModel;
        }

        internal bool m_IsPixelDefultUnit = true;

        /// <summary>
        /// Converts the ChildDataParser items to nodes.
        /// </summary>
        /// <param name="items">ChildDataParser items.</param>
        /// <returns>The converted nodes</returns>
        private CollectionExt ConvertToNodes(List<ChildDataParser> items, DataTemplate dt)
        {
            CollectionExt nodes = new CollectionExt();
            CollectionExt childs = new CollectionExt();
            foreach (ChildDataParser child in items)
            {
                Node node = new Node();
                node.Content = child.Header;
                if (dt != null && (dt.LoadContent() != null))
                {
                    node.ContentTemplate = dt;
                }
                child.node = node;
                node.Model = this;
                childs = this.ConvertToNodes(child.ChildItems, child.ItemTemplate);
                nodes.Add(node);
                HookConnection(node, childs);
            }

            return nodes;
        }

        /// <summary>
        /// Hooks a connection between the specified head node and tail node.
        /// </summary>
        /// <param name="parent">Head Node object</param>
        /// <param name="childs">The Tail Nodes </param>
        private void HookConnection(Node parent, CollectionExt childs)
        {
            this.m_internalChildren.Add(parent);
            foreach (Node node in childs)
            {
                LineConnector ortho;
                if (dc != null && dc.View != null)
                {
                    ortho = new LineConnector(dc.View);
                }
                else
                {
                    ortho = new LineConnector(null);
                }
                ortho.HeadNode = parent;
                ortho.TailNode = node;
                //ortho.Content = node.Content;
                this.Connections.Add(ortho);
            }
        }

        /// <summary>
        /// Handles the CollectionChanged event of the minternalChildren control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Collections.Specialized.NotifyCollectionChangedEventArgs"/> instance containing the event data.</param>
        private void InternalChildren_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (dc != null)
            {
                dc.View.UndoStack.Clear();
            }
        }

        /// <summary>
        /// Called when [children collection changed].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Collections.Specialized.NotifyCollectionChangedEventArgs"/> instance containing the event data.</param>
        private void OnChildrenCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            m_Children.CollectionChanged -= OnChildrenCollectionChanged;
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    this.AddLogicalChildren(e.NewItems);
                    break;

                case NotifyCollectionChangedAction.Remove:
                    this.RemoveLogicalChildren(e.OldItems);
                    break;

                case NotifyCollectionChangedAction.Replace:
                    this.RemoveLogicalChildren(e.OldItems);
                    this.AddLogicalChildren(e.NewItems);
                    break;

                case NotifyCollectionChangedAction.Reset:
                    break;

                default:
                    break;
            }

            //RemoveLogicalChildren(new System.Collections.ArrayList(m_LogicalChildren));
            //if (!BrowserInteropHelper.IsBrowserHosted)
            {
                ParseItemSourceIntoParser();
                //m_Children.CollectionChanged += new NotifyCollectionChangedEventHandler(m_Children_CollectionChanged);
            }
        }


        internal ChildDataParser parser;
        internal Grid grid;
        internal Popup popup;
        internal bool init = false;
        internal bool refresh = true;

        /// <summary>
        /// Parses the items from the items source.
        /// </summary>
        private void ParseItemSourceIntoParser()
        {
            init = false;
            parser = new ChildDataParser(this);
            parser.PropertyChanged += new PropertyChangedEventHandler(parser_PropertyChanged);
            parser.ItemsSource = this.ItemsSource;
            parser.LayoutUpdated += new EventHandler(parser_LayoutUpdated);
            if (this.HierarchicalDataTemplate != null)
            {
                parser.ItemTemplate = this.HierarchicalDataTemplate;
            }
            else
            {
                parser.ItemTemplate = this.ItemTemplate;
            }

            grid = new Grid();
            grid.Width = 100;
            grid.Height = 100;
            grid.Children.Add(parser);
            popup = new Popup();
            popup.Width = 0;
            popup.Height = 0;
            popup.Child = grid;
            popup.IsOpen = true;
        }

        void parser_LayoutUpdated(object sender, EventArgs e)
        {
            parser.LayoutUpdated -= parser_LayoutUpdated;
            popup.IsOpen = false;
            if (LayoutType != LayoutType.None)
            {
                this.Connections.Clear();
                this.Nodes.Clear();
            }
            RootNodes = null;
            RootNodes = ConvertToNodes(parser.ChildItems, this.ItemTemplate);
            if (LayoutType != LayoutType.None)
            {
                if ((LayoutType == LayoutType.DirectedTreeLayout && LayoutRoot != null))
                {
                    this.Nodes.Insert(Nodes.Count, LayoutRoot);

                    if (this.RootNodes.Count > 0)
                    {
                        foreach (Node node in this.RootNodes)
                        {
                            LineConnector ortho;
                            if (dc != null && dc.View != null)
                            {
                                ortho = new LineConnector(dc.View);
                            }
                            else
                            {
                                ortho = new LineConnector(null);
                            }
                            ortho.HeadNode = this.LayoutRoot;
                            ortho.TailNode = node;
                            ortho.Content = node.Content;
                            this.Connections.Add(ortho);
                        }
                    }
                }
                else if (LayoutType == LayoutType.HierarchicalTreeLayout && LayoutRoot != null)
                {
                    if (this.Nodes.Contains(LayoutRoot))
                    {
                        this.Nodes.Remove(LayoutRoot);
                    }
                }
            }
            init = true;
        }

        void parser_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (refresh)
            {
                refresh = false;
                dc.View.Page.InvalidateMeasure();
                dc.View.Page.UpdateLayout();
                //parser.PropertyChanged -= parser_PropertyChanged;
                this.RefreshLayout();
                //if (disp != null)
                //{
                //    disp.Abort();
                //}
                //disp = this.Dispatcher.BeginInvoke(
                //    System.Windows.Threading.DispatcherPriority.SystemIdle,
                //    new Initialize(this.RefreshLayout));

            }
            refresh = true;
        }


        internal void RefreshLayout()
        {
            if (dc != null && dc.View != null)
            {
                //ParseItemSourceIntoParser();
                if (this.LayoutType == LayoutType.HierarchicalTreeLayout)
                {
                    HierarchicalTreeLayout layout = new HierarchicalTreeLayout(this, dc.View);
                    layout.RefreshLayout();
                }
                else
                {
                    DirectedTreeLayout layout = new DirectedTreeLayout(this, dc.View);
                    layout.RefreshLayout();
                }
            }
            else
            {
                //ParseItemSourceIntoParser();
            }
        }
        
        /// <summary>
        /// Removes the logical children.
        /// </summary>
        /// <param name="iList">The IList collection.</param>
        /// <param name="detach">if set to <c>true</c> [detach].</param>
        private void RemoveLogicalChildren(IList ilist, bool detach)
        {
        }

        /// <summary>
        /// Removes the logical children.
        /// </summary>
        /// <param name="iList">The IList collection.</param>
        private void RemoveLogicalChildren(IList ilist)
        {
            this.RemoveLogicalChildren(ilist, true);
        }
        #region Class variables

        #endregion

        #region Initialization

        #endregion

        #region Class Properties

        #endregion

        #region Dependency Properties

        #endregion

        #region Implementation

        #endregion

        #region INotifyPropertyChanged Members

        #endregion
    }
}
