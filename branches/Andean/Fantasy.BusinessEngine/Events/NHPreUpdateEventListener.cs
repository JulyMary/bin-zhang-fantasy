using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Event;
using Fantasy.AddIns;

namespace Fantasy.BusinessEngine.Events
{
    class NHPreUpdateEventListener : IPreUpdateEventListener
    {
        #region IPreUpdateEventListener Members
        private IAddInTreeNode _treeNode = null;
        public bool OnPreUpdate(PreUpdateEvent @event)
        {
            bool rs = false;
            
            if (@event.Entity is IEntity)
            {
                if (_treeNode == null)
                {
                    _treeNode = AddInTree.Tree.GetTreeNode("fantasy/businessengine/entityhandlers/updating");
                }
                EntityCancelEventArgs e = new EntityCancelEventArgs((IEntity)@event.Entity);
                foreach (IEntityCancelEventHandler handler in _treeNode.BuildChildItems(@event.Entity))
                {
                    handler.Execute(e);
                    if (e.Cancel)
                    {
                        break;
                    }
                }
                rs = e.Cancel;

            }

            return rs;
        }

        #endregion
    }
}
