using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.AddIns;

namespace Fantasy.BusinessEngine.Events
{
    class EntityCreateEventListener : ObjectWithSite
    {
        private IAddInTreeNode _treeNode;
        public void OnCreate(IEntity entity)
        {
            if (_treeNode == null)
            {
                _treeNode = AddInTree.Tree.GetTreeNode("fantasy/businessengine/entityhandlers/create");
            }

            entity.EntityState = EntityState.New;
            EntityEventArgs e = new EntityEventArgs(entity);

            foreach (IEntityEventHandler handler in _treeNode.BuildChildItems(entity, this.Site))
            {
                handler.Execute(e);
            }
        }
    }
}
