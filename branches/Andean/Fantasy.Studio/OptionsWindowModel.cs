using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.Windows;
using Fantasy.Studio.Controls;
using Fantasy.ServiceModel;

namespace Fantasy.Studio
{
    public class OptionsWindowModel : NotifyPropertyChangedObject
    {

        public OptionsWindowModel()
        {
            this.Nodes = new List<OptionNode>(AddIns.AddInTree.Tree.GetTreeNode("fantasy/sutdio/options").BuildChildItems<OptionNode>(this, ServiceManager.Services));

            var query = from root in this.Nodes
                        from node in root.Flatten(n => n.ChildNodes)
                        where node.Panel != null
                        select node.Panel;
            foreach (IOptionPanel panel in query)
            {
                panel.DirtyStateChanged += new EventHandler(PanelDirtyStateChanged);
            }

            if (this.Nodes.Count > 0)
            {
                this.SetSelectedNode(this.Nodes[0]);
            }
        }

        void PanelDirtyStateChanged(object sender, EventArgs e)
        {
            IOptionPanel panel = (IOptionPanel)sender;
            if (panel.DirtyState == EditingState.Dirty)
            {
                this.IsDirty = true;
            }
        }



        private bool _isDirty = false;

        public bool IsDirty
        {
            get { return _isDirty; }
            set
            {
                if (_isDirty != value)
                {
                    _isDirty = value;
                    this.OnPropertyChanged("IsDirty");
                }
            }
        }

        public void Save()
        {
            var query = from root in this.Nodes
                        from node in root.Flatten(n => n.ChildNodes)
                        where node.Panel != null && node.Panel.DirtyState == EditingState.Dirty
                        select node.Panel;
            foreach (IOptionPanel panel in query)
            {
                panel.Save();
            }
            this.IsDirty = false;
        }

        private IOptionPanel _selectedPanel;

        public IOptionPanel SelectedPanel
        {
            get { return _selectedPanel; }
            private set
            {
                if (_selectedPanel != value)
                {
                    _selectedPanel = value;
                    this.OnPropertyChanged("SelectedPanel");
                }
            }
        }

        public void SetSelectedNode(OptionNode node)
        {
            OptionNode cur = node;
            IOptionPanel panel = cur.Panel;
            while (panel == null && cur != null)
            {
                cur = cur.ChildNodes.FirstOrDefault();
                if (cur != null)
                {
                    panel = cur.Panel;
                }
            }
            this.SelectedPanel = panel;
        }


        public IList<OptionNode> Nodes { get; private set; }



        

    }
}
