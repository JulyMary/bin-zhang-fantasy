﻿using System;
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
                if (_treeNode == null)
                {
                    _treeNode = AddInTree.Tree.GetTreeNode("fantasy/businessengine/entityhandlers/deleting");
                }
                EntityCancelEventArgs e = new EntityCancelEventArgs((IEntity)@event.Entity);
                foreach (IEntityCancelEventHandler handler in _treeNode.BuildChildItems(@event.Entity,this.Site ))
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
