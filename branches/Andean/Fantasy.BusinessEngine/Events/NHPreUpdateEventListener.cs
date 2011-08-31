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
    class NHPreUpdateEventListener : ObjectWithSite, IPreUpdateEventListener
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
                IEntity entity = @event.Entity as IEntity;
                INotifyPropertyChanged np = entity as INotifyPropertyChanged;

                PropertyChangedEventHandler propertyChagned = (sender, e) =>
                {
                    int index = Array.IndexOf(@event.Persister.PropertyNames, e.PropertyName);
                    if (index >= 0)
                    {
                        @event.State[index] = Invoker.Invoke(entity, e.PropertyName);
                    }
                };

                np.PropertyChanged += propertyChagned;
                try
                {
                    EntityCancelEventArgs e = new EntityCancelEventArgs((IEntity)@event.Entity);
                    foreach (IEntityCancelEventHandler handler in _treeNode.BuildChildItems(@event.Entity, this.Site))
                    {
                        handler.Execute(e);
                        if (e.Cancel)
                        {
                            break;
                        }
                    }
                    rs = e.Cancel;
                }
                finally
                {
                    np.PropertyChanged -= propertyChagned;
                }

            }

            return rs;
        }

        #endregion
    }
}
