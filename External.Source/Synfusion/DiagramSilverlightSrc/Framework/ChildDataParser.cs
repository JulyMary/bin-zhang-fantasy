// <copyright file="ChildDataParser.cs" company="Syncfusion">
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
using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Controls.Primitives;

namespace Syncfusion.Windows.Diagram
{
    /// <summary>
    /// Converts collection of objects to Hierarchical Data format.
    /// </summary>
    internal class ChildDataParser : TreeViewItem
    {
        internal DiagramModel dm;
        internal Node node;

        #region Initialization

        /// <summary>
        /// Initializes a new instance of the <see cref="ChildDataParser"/> class.
        /// </summary>
        public ChildDataParser()
        {
            this.ChildItems = new List<ChildDataParser>();
            this.IsExpanded = true;
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="ChildDataParser"/> class.
        /// </summary>
        public ChildDataParser(DiagramModel model)
        {
            this.ChildItems = new List<ChildDataParser>();
            this.IsExpanded = true;
            this.dm = model;
        }

        #endregion

        #region  Properties

        /// <summary>
        /// Gets or sets the child items.
        /// </summary>
        /// <value>The child items.</value>
        public List<ChildDataParser> ChildItems { get; set; }

        #endregion

        #region Properties

        /// <summary>
        /// Creates or identifies the element that is used to display the given item.
        /// </summary>
        /// <returns>The element that is used to display the given item.</returns>
        protected override DependencyObject GetContainerForItemOverride()
        {
            base.GetContainerForItemOverride();
            ChildDataParser newItem = new ChildDataParser(dm);
            this.ChildItems.Add(newItem);
            //foreach (ChildDataParser ch in ChildItems)
            {
                newItem.PropertyChanged -= new PropertyChangedEventHandler(ch_PropertyChanged);
                newItem.PropertyChanged += new PropertyChangedEventHandler(ch_PropertyChanged);
            }
            return newItem;
        }

        void ch_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs("ChildChanged"));
            }
        }

        protected override void OnItemsChanged(System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            bool refresh = true;
            base.OnItemsChanged(e);
            if (dm != null && dm.dc != null && dm.dc.View != null && dm.init)
            {
                switch (e.Action)
                {
                    case System.Collections.Specialized.NotifyCollectionChangedAction.Add:
                        doAdd(e);
                        break;
                        //case System.Collections.Specialized.NotifyCollectionChangedAction.Move:
                        ////Node n;
                        //ChildDataParser temp = ChildItems[e.NewStartingIndex];
                        ////n = temp.node;
                        ////temp.node = ChildItems[e.OldStartingIndex].node;
                        ////ChildItems[e.OldStartingIndex].node = ChildItems[e.NewStartingIndex].node;
                        ////ChildItems[e.NewStartingIndex].node = n;
                        //ChildItems[e.NewStartingIndex] = ChildItems[e.OldStartingIndex];
                        //ChildItems[e.OldStartingIndex] = temp;
                        //break;
                    case System.Collections.Specialized.NotifyCollectionChangedAction.Remove:
                        RemoveChildItems(ChildItems[e.OldStartingIndex]);
                        dm.m_internalChildren.Remove(ChildItems[e.OldStartingIndex].node);
                        ChildItems.Remove(ChildItems[e.OldStartingIndex]);
                        break;
                    case System.Collections.Specialized.NotifyCollectionChangedAction.Replace:
                        //ChildDataParser c = null;
                        //foreach (ChildDataParser child in ChildItems)
                        //{
                        //    if (child.Header.Equals(e.OldItems[0]))
                        //    {
                        //        c = child;
                        //    }
                        //}
                        //if (c != null)
                        //{
                        //    RemoveChildItems(c);
                        //}

                        this.
                        doAdd(e);
                        if (e.OldItems != null)
                        {
                            foreach (object obj in e.OldItems)
                            {
                                if (obj != null)
                                {
                                    //this.ChildItems.Remove(obj as ChildDataParser);
                                    for (int i = 0; i < ChildItems.Count; i++)
                                    {
                                        ChildDataParser child = ChildItems[i];
                                        if (child.Header.Equals(obj))
                                        {
                                            //if (node != null)
                                            {
                                                RemoveChildItems(child);
                                                dm.m_internalChildren.Remove(child.node);
                                                ChildItems.Remove(child);
                                                //this.ChildItems.Remove(child);
                                                i--;
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        /*RemoveChildItems(ChildItems[e.OldStartingIndex + 1]);
                        dm.m_internalChildren.Remove(ChildItems[e.OldStartingIndex + 1].node);
                        ChildItems.Remove(ChildItems[e.OldStartingIndex + 1]);*/
                        /*RemoveChildItems(ChildItems[1]);
                        dm.m_internalChildren.Remove(ChildItems[1].node);
                        ChildItems.Remove(ChildItems[1]);*/
                        break;
                    case System.Collections.Specialized.NotifyCollectionChangedAction.Reset:
                        //foreach (ChildDataParser child in ChildItems)
                        //{
                        //    if (child.Header.ToString() != "{DisconnectedItem}")
                        //    {
                        //        if (node != null)
                        //        {
                        //            refresh = false;
                        //        }
                        //    }
                        //}
                        if (ChildItems.Count == 0)
                        {
                            refresh = false;
                        }
                        RemoveChildItems(this);
                        //dm.m_internalChildren.Remove(this.node);
                        //ChildItems.Remove(this);
                        //foreach (ChildDataParser child in ChildItems)
                        //{
                        //    dm.m_internalChildren.Remove(child.node);
                        //}
                        //this.ChildItems.Clear();
                        break;
                }
                if (PropertyChanged != null && refresh)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("ChildChanged"));
                }

            }
        }

        protected override void ClearContainerForItemOverride(DependencyObject element, object item)
        {
            base.ClearContainerForItemOverride(element, item);
        }

        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            base.PrepareContainerForItemOverride(element, item);
            ChildDataParser ele = element as ChildDataParser;
            if (ele != null && ele.node != null)
            {
                if (ele.dm.m_internalChildren.Contains(ele.node))
                {
                    ele.dm.m_internalChildren.Remove(ele.node);
                }
            }
        }

        private void doAdd(System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            int startCount = ChildItems.Count;
            ChildDataParser parser = dm.parser;
            Grid grid = dm.grid;//new Grid();
            grid.UpdateLayout();
            grid.Width = 10;
            grid.Height = 10;
            //grid.Children.Add(parser);
            Popup popup = dm.popup;//new Popup();
            popup.Width = 0;
            popup.Height = 0;
            popup.Child = grid;
            popup.IsOpen = true;
            parser.UpdateLayout();
            popup.UpdateLayout();
            popup.IsOpen = false;

            //foreach (ChildDataParser child in ChildItems)
            for (int i = 0; i < ChildItems.Count; i++)
            {
                ChildDataParser child = ChildItems[i];
                if (child.Header.ToString() == "{DisconnectedItem}")
                {
                    if (node != null)
                    {
                        this.ChildItems.Remove(child);
                        i--;
                    }
                }
            }

            //if (e.OldItems != null)
            //{
            //    foreach (object obj in e.OldItems)
            //    {
            //        if (obj != null)
            //        {
            //            //this.ChildItems.Remove(obj as ChildDataParser);
            //            for (int i = 0; i < ChildItems.Count; i++)
            //            {
            //                ChildDataParser child = ChildItems[i];
            //                if (child.Header.Equals(obj))
            //                {
            //                    //if (node != null)
            //                    {
            //                        RemoveChildItems(child);
            //                        //dm.m_internalChildren.Remove(child.node);
            //                        dm.Nodes.Remove(child.node);
            //                        ChildItems.Remove(child);
            //                        //this.ChildItems.Remove(child);
            //                        i--;
            //                    }
            //                }
            //            }
            //        }
            //    }
            //}

            if (node == null && dm.LayoutType == LayoutType.DirectedTreeLayout)
            {
                node = dm.LayoutRoot as Node;
            }

            if (e.NewItems != null)
            {
                foreach (object obj in e.NewItems)
                {
                    if (obj != null)
                    {
                        //this.ChildItems.Remove(obj as ChildDataParser);
                        for (int i = 0; i < ChildItems.Count; i++)
                        {
                            ChildDataParser child = ChildItems[i];
                            if (child.Header.Equals(obj))
                            {
                                Node cnode = new Node();
                                cnode.Content = child.Header;
                                if (ItemTemplate != null && (ItemTemplate.LoadContent() != null))
                                {
                                    cnode.ContentTemplate = this.ItemTemplate;
                                }
                                child.node = cnode;
                                cnode.Model = dm;
                                dm.m_internalChildren.Add(cnode);

                                if (node != null)
                                {
                                    LineConnector ortho;
                                    if (dm != null && dm.dc != null && dm.dc.View != null)
                                    {
                                        ortho = new LineConnector(dm.dc.View);
                                    }
                                    else
                                    {
                                        ortho = new LineConnector(null);
                                    }
                                    ortho.HeadNode = this.node;
                                    ortho.TailNode = cnode;
                                    ortho.Content = node.Content;
                                    dm.Connections.Add(ortho);
                                }
                                AddChildItems(child);
                            }
                        }
                    }
                }
            }

            //if (ChildItems.Count > e.NewStartingIndex)
            //{
            //    Node cnode = new Node();
            //    cnode.Content = ChildItems[e.NewStartingIndex].Header;
            //    if (ItemTemplate != null && (ItemTemplate.LoadContent() != null))
            //    {
            //        cnode.ContentTemplate = this.ItemTemplate;
            //    }
            //    ChildItems[e.NewStartingIndex].node = cnode;
            //    cnode.Model = dm;
            //    dm.m_internalChildren.Add(cnode);

            //    if (node != null)
            //    {
            //        LineConnector ortho;
            //        if (dm != null && dm.dc != null && dm.dc.View != null)
            //        {
            //            ortho = new LineConnector(dm.dc.View);
            //        }
            //        else
            //        {
            //            ortho = new LineConnector(null);
            //        }
            //        ortho.HeadNode = this.node;
            //        ortho.TailNode = cnode;
            //        ortho.Content = node.Content;
            //        dm.Connections.Add(ortho);
            //    }
            //    AddChildItems(ChildItems[e.NewStartingIndex]);
            //}
        }

        private void RemoveChildItems(ChildDataParser childDataParser)
        {
            foreach (ChildDataParser child in childDataParser.ChildItems)
            {
                RemoveChildItems(child);
                if (child.node != null)
                {
                    dm.m_internalChildren.Remove(child.node);
                }
            }
            childDataParser.ChildItems.Clear();
        }

        private CollectionExt getNodes(ChildDataParser child)
        {
            CollectionExt nodes = new CollectionExt();
            foreach (ChildDataParser c in child.ChildItems)
            {
                nodes.Add(c.node);
            }
            return nodes;
        }

        private void AddChildItems(ChildDataParser childDataParser)
        {
            if (childDataParser.ChildItems.Count > 0)
            {
                CollectionExt RootNodes = ConvertToNodes(childDataParser.ChildItems, childDataParser.ChildItems[0].ItemTemplate);

                foreach (Node cnode in RootNodes)
                {
                    cnode.Model = dm;
                    LineConnector ortho;
                    if (dm != null && dm.dc != null && dm.dc.View != null)
                    {
                        ortho = new LineConnector(dm.dc.View);
                    }
                    else
                    {
                        ortho = new LineConnector(null);
                    }
                    ortho.HeadNode = childDataParser.node;
                    ortho.TailNode = cnode;
                    ortho.Content = childDataParser.node.Content;
                    dm.Connections.Add(ortho);
                }
            }
        }

        private CollectionExt ConvertToNodes(List<ChildDataParser> items, DataTemplate dataTemplate)
        {
            CollectionExt nodes = new CollectionExt();
            CollectionExt childs = new CollectionExt();
            foreach (ChildDataParser child in items)
            {
                if (child.Header.ToString() != "{DisconnectedItem}")
                {
                    Node node = new Node();
                    node.Content = child.Header;
                    if (dataTemplate != null && (dataTemplate.LoadContent() != null))
                    {
                        node.ContentTemplate = dataTemplate;
                    }
                    child.node = node;
                    node.Model = dm;
                    childs = this.ConvertToNodes(child.ChildItems, child.ItemTemplate);
                    nodes.Add(node);
                    HookConnection(node, childs);
                }
            }
            return nodes;
        }

        private void HookConnection(Node parent, CollectionExt childs)
        {
            dm.m_internalChildren.Add(parent);
            foreach (Node node in childs)
            {
                LineConnector ortho;
                if (dm != null && dm.dc != null && dm.dc.View != null)
                {
                    ortho = new LineConnector(dm.dc.View);
                }
                else
                {
                    ortho = new LineConnector(null);
                }
                ortho.HeadNode = parent;
                ortho.TailNode = node;
                ortho.Content = node.Content;
                dm.Connections.Add(ortho);
            }
        }

        internal event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}
