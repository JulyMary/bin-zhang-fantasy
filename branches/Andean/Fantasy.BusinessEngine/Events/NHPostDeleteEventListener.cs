using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Event;
using Fantasy.AddIns;

namespace Fantasy.BusinessEngine.Events
{
    class NHPostDeleteEventListener : ObjectWithSite, IPostDeleteEventListener
    {
        #region IPostDeleteEventListener Members
        IAddInTreeNode _treeNode;
        public void OnPostDelete(PostDeleteEvent @event)
        {

            IEntity entity = @event.Entity as IEntity;

            if (entity != null)
            {
                if (_treeNode == null)
                {
                    _treeNode = AddInTree.Tree.GetTreeNode("fantasy/businessengine/entityhandlers/deleted");
                }

                entity.EntityState = EntityState.Deleted;
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
