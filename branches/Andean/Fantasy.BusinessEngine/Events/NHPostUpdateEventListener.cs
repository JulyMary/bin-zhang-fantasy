﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Event;
using Fantasy.AddIns;

namespace Fantasy.BusinessEngine.Events
{
    class NHPostUpdateEventListener : ObjectWithSite, IPostUpdateEventListener
    {

        IAddInTreeNode _treeNode;

        public void OnPostUpdate(PostUpdateEvent @event)
        {
            IEntity entity = @event.Entity as IEntity;
            if (entity != null)
            {
                entity.EntityState = EntityState.Clean;
                if (_treeNode == null)
                {
                    _treeNode = AddInTree.Tree.GetTreeNode("fantasy/businessengine/entityhandlers/updated");
                }
                EntityEventArgs e = new EntityEventArgs((IEntity)@event.Entity);
                foreach (IEntityEventHandler handler in _treeNode.BuildChildItems(@event.Entity, this.Site))
                {
                    handler.Execute(e);
                }
            }
        }
    }
}