using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Fantasy.Properties;

namespace Fantasy.AddIns
{
    public class DefaultAddInTreeNode : IAddInTreeNode
    {

        public DefaultAddInTreeNode()
        {
            this.InsertAfter = new string[0];
            this.InsertBefore = new string[0];
            this.ChildNodes.Inserted += new EventHandler<CollectionEventArgs<DefaultAddInTreeNode>>(ChildNodes_Inserted);
        }

        void ChildNodes_Inserted(object sender, CollectionEventArgs<DefaultAddInTreeNode> e)
        {
            _sortedChildNodes = null;
        }
        public string ID { get; set; }

        private DefaultAddInTreeNode[] _sortedChildNodes = null;

        #region IAddInTreeNode Members

        public DefaultAddInTreeNode FindChild(string ID)
        {
            return _childNodes.FirstOrDefault(c => c.ID == ID);
        }

        public DefaultAddInTreeNode Parent { get; set; }

        public string FullPath
        {
            get
            {
                if (Parent != null)
                {
                    return Parent.FullPath + "/" + this.ID;
                }
                else
                {
                    return this.ID;
                }
            }
        }



        private Collection<DefaultAddInTreeNode> _childNodes = new Collection<DefaultAddInTreeNode>();
        public Collection<DefaultAddInTreeNode> ChildNodes
        {
            get { return _childNodes; }
        }

        IAddInTreeNode[] IAddInTreeNode.ChildNodes
        {
            get
            {
                return _childNodes.ToArray();
            }
        }

        public ICodon Codon
        {
            get;
            set;
        }

        public ConditionCollection Condition
        {
            get;
            set;
        }

        public ConditionFailedAction GetCurrentConditionFailedAction(object caller)
        {
            throw new NotImplementedException();
        }

        public IEnumerable BuildChildItems(object caller, IServiceProvider site)
        {
            if (this._sortedChildNodes == null)
            {
                this.CreateSortedChildItems();
            }
            foreach (DefaultAddInTreeNode childNode in this._sortedChildNodes)
            {
                if (childNode.Codon.HandleCondition || childNode.Condition.GetCurrentConditionFailedAction(caller, site) == ConditionFailedAction.Nothing)
                {
                    IEnumerable subItems = childNode.BuildChildItems(caller, site);
                    object rs = childNode.Codon.BuildItem(caller, subItems, childNode.Condition, site);
                    if (rs != null)
                    {
                        if (rs is IObjectWithSite)
                        {
                            ((IObjectWithSite)rs).Site = site;
                        }
                        yield return rs;
                    }
                }

            }

        }

        private void CreateSortedChildItems()
        {
            Dictionary<DefaultAddInTreeNode, List<string>> afters = this.CreateAllAfters();
            Dictionary<DefaultAddInTreeNode, int> order = new Dictionary<DefaultAddInTreeNode, int>();
            foreach (DefaultAddInTreeNode node in this.ChildNodes)
            {
                EvaluateOrder(afters, order, node);
            }

            this._sortedChildNodes = this.ChildNodes.OrderBy(n => order.GetValueOrDefault(n, 0)).ToArray();
        }

        private int EvaluateOrder(Dictionary<DefaultAddInTreeNode, List<string>> afters, Dictionary<DefaultAddInTreeNode, int> order, DefaultAddInTreeNode node)
        {
           
            if (order.ContainsKey(node))
            {
                int rs = order[node];
                if (rs < 0)
                {
                    throw new AddInException(string.Format(Resources.AddInTreePathRecursiveOrderText, node.FullPath));
                }
                else
                {
                    return rs;
                }
            }
            else
            {
                
                order.Add(node, -1);
                int rs = 0;
                List<int> afterOrders = new List<int>();
                foreach (string id in afters[node])
                {
                    DefaultAddInTreeNode n = this.FindChild(id);
                    if(n != null)
                    {
                        afterOrders.Add(EvaluateOrder(afters, order, n));
                    }
                }

                if (afterOrders.Count > 0)
                {
                    rs = afterOrders.Max() + 1;
                }
                order[node] = rs;
                return rs;
            }
        }

        private Dictionary<DefaultAddInTreeNode, List<string>> CreateAllAfters()
        {
            Dictionary<DefaultAddInTreeNode, List<string>> rs = new Dictionary<DefaultAddInTreeNode, List<string>>();
            foreach (DefaultAddInTreeNode node in this.ChildNodes)
            {
                rs.Add(node, new List<string>(node.InsertAfter));
            }
            foreach (DefaultAddInTreeNode node in this.ChildNodes)
            {
                foreach (string before in node.InsertBefore)
                {
                    DefaultAddInTreeNode beforeNode = this.FindChild(before);
                    if (beforeNode != null)
                    {
                        List<string> afters = rs[beforeNode];
                        afters.Add(node.ID);
                    }
                }
            }
            return rs;
        }

        public IEnumerable<T> BuildChildItems<T>(object caller, IServiceProvider site)
        {
            foreach (T item in this.BuildChildItems(caller, site))
            {
                yield return item;
            }
        }

        public AddIn AddIn { get; set; }

        public string[] InsertBefore { get; set; }

        public string[] InsertAfter { get; set; }

        #endregion

        #region IAddInTreeNode Members


        public object BuildItem(object caller, IServiceProvider site)
        {
            object rs = null;
            if (this.Codon.HandleCondition || this.Condition.GetCurrentConditionFailedAction(caller, site) == ConditionFailedAction.Nothing)
            {
                IEnumerable subItems = this.BuildChildItems(caller, site);
                rs = this.Codon.BuildItem(caller, subItems, this.Condition, site);
                if (rs != null)
                {
                    if (rs is IObjectWithSite)
                    {
                        ((IObjectWithSite)rs).Site = site;
                    }
                  
                }
            }

            return rs;
        }

        public T BuildItem<T>(object caller, IServiceProvider site)
        {
            return (T)BuildItem(caller, site);
        }

        #endregion
    }
}
