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
        public void OnCreate(EntityCreateEventArgs e)
        {
            if (_treeNode == null)
            {
                _treeNode = AddInTree.Tree.GetTreeNode("fantasy/businessengine/entityhandlers/create");
            }

            e.Entity.EntityState = EntityState.New;
           

            foreach (IEntityEventHandler handler in _treeNode.BuildChildItems(e.Entity, this.Site))
            {
                handler.Execute(e);
            }
        }
    }
}
