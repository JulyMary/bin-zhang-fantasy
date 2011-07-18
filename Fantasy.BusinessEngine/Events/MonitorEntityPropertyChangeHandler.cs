using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.AddIns;

namespace Fantasy.BusinessEngine.Events
{
    public class MonitorEntityPropertyChangeHandler : IEntityEventHandler
    {

        #region IEntityEventHandler Members

        public void Execute(EntityEventArgs e)
        {
            e.Entity.PropertyChanged += new EventHandler<EntityPropertyChangedEventArgs>(EntityPropertyChanged);
            e.Entity.PropertyChanging += new EventHandler<EntityPropertyChangingEventArgs>(EntityPropertyChanging);
        }

        private IAddInTreeNode _changingTreeNode;

        void EntityPropertyChanging(object sender, EntityPropertyChangingEventArgs e)
        {
            if (this._changingTreeNode == null)
            {
                this._changingTreeNode = AddInTree.Tree.GetTreeNode("fantasy/businessengine/entityhandlers/propertychanging");
            }
            foreach (IEntityPropertyChangingEventHandler handler in _changingTreeNode.BuildChildItems(e.Entity))
            {
                handler.Execute(e);
            }
        }


        private IAddInTreeNode _changedTreeNode;

        void EntityPropertyChanged(object sender, EntityPropertyChangedEventArgs e)
        {
            if (_changedTreeNode == null)
            {
                _changedTreeNode = AddInTree.Tree.GetTreeNode("fantasy/businessengine/entityhandlers/propertychanged");
            }
            foreach (IEntityPropertyChangedEventHandler handler in _changedTreeNode.BuildChildItems(e.Entity))
            {
                handler.Execute(e);
            }
        }

        #endregion



      
    }
}
