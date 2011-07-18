using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Event;
using Fantasy.AddIns;

namespace Fantasy.BusinessEngine.Events
{
    class NHPreInsertEventListener : IPreInsertEventListener
    {
        #region IPreInsertEventListener Members
        IAddInTreeNode _treeNode;
        public bool OnPreInsert(PreInsertEvent @event)
        {
            bool rs = false;
            if (@event.Entity is IEntity)
            {
                if (_treeNode == null)
                {
                    _treeNode = AddInTree.Tree.GetTreeNode("fantasy/businessengine/entityhandlers/inserting");
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
