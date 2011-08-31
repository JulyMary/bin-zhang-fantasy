using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Event;
using Fantasy.AddIns;
using System.ComponentModel;
using System.Reflection;

namespace Fantasy.BusinessEngine.Events
{
    class NHPostInsertEventListener : ObjectWithSite, IPostInsertEventListener
    {
        #region IPostInsertEventListener Members
        IAddInTreeNode _treeNode;
        public void OnPostInsert(PostInsertEvent @event)
        {
            IEntity entity = @event.Entity as IEntity;
            if (entity != null)
            {
                entity.EntityState = EntityState.Clean;
                if (_treeNode == null)
                {
                    _treeNode = AddInTree.Tree.GetTreeNode("fantasy/businessengine/entityhandlers/inserted");
                }


                

                    EntityEventArgs e = new EntityEventArgs(entity);
                    foreach (IEntityEventHandler handler in _treeNode.BuildChildItems(@event.Entity, this.Site))
                    {
                        handler.Execute(e);
                    }
                
            }
        }

        #endregion
    }
}
