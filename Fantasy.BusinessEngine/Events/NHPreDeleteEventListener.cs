using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Event;
using Fantasy.AddIns;

namespace Fantasy.BusinessEngine.Events
{
    class NHPreDeleteEventListener : ObjectWithSite, IPreDeleteEventListener
    {
        #region IPreUpdateEventListener Members
        private IAddInTreeNode _treeNode = null;
        public bool OnPreDelete(PreDeleteEvent @event)
        {
            bool rs = false;
            
            if (@event.Entity is IEntity)
            {
                rs = OnPreDelete((IEntity)@event.Entity);

            }

            return rs;
        }

        #endregion

        public bool OnPreDelete(IEntity entity)
        {
            if (_treeNode == null)
            {
                _treeNode = AddInTree.Tree.GetTreeNode("fantasy/businessengine/entityhandlers/deleting");
            }
            EntityCancelEventArgs e = new EntityCancelEventArgs(entity);
            foreach (IEntityCancelEventHandler handler in _treeNode.BuildChildItems(entity, this.Site))
            {
                handler.Execute(e);
                if (e.Cancel)
                {
                    break;
                }
            }
            return e.Cancel;
        }
    }
}
