﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Event;
using Fantasy.AddIns;

namespace Fantasy.BusinessEngine.Events
{
    class NHPostLoadEventListener : ObjectWithSite, IPostLoadEventListener 
    {

        IAddInTreeNode _treeNode;

        public void OnPostLoad(PostLoadEvent @event)
        {
            IEntity entity = @event.Entity as IEntity;
            if (entity != null)
            {
                entity.EntityState = EntityState.Clean;
                if (_treeNode == null)
                {
                    _treeNode = AddInTree.Tree.GetTreeNode("fantasy/businessengine/entityhandlers/load");
                }

                EntityEventArgs e = new EntityEventArgs(entity);
                foreach (IEntityEventHandler handler in _treeNode.BuildChildItems(@event.Entity, this.Site ))
                {
                    handler.Execute(e);
                }
            }
        }

        
    }
}