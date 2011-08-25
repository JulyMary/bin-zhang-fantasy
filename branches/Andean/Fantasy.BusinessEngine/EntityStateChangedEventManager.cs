using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Fantasy.BusinessEngine
{
    public class EntityStateChangedEventManager : WeakEventManager
    {
        private EntityStateChangedEventManager()
        {

        }

        protected override void StartListening(object source)
        {
            ((IEntity)source).EntityStateChanged += new EventHandler(EntityStateChangedEventManager_EntityStateChanged);
        }

        void EntityStateChangedEventManager_EntityStateChanged(object sender, EventArgs e)
        {
            base.DeliverEvent(sender, e);
        }

        protected override void StopListening(object source)
        {
            ((IEntity)source).EntityStateChanged -= new EventHandler(EntityStateChangedEventManager_EntityStateChanged);
        }


        public static void AddListener(IEntity source, IWeakEventListener listener)
        {
            CurrentManager.ProtectedAddListener(source, listener);
        }

        public static void RemoveListener(IEntity source, IWeakEventListener listener)
        {
            CurrentManager.ProtectedRemoveListener(source, listener);
        }


        private static EntityStateChangedEventManager CurrentManager
        {
            get
            {
                Type managerType = typeof(EntityStateChangedEventManager);
                EntityStateChangedEventManager currentManager = (EntityStateChangedEventManager)WeakEventManager.GetCurrentManager(managerType);
                if (currentManager == null)
                {
                    currentManager = new EntityStateChangedEventManager();
                    WeakEventManager.SetCurrentManager(managerType, currentManager);
                }
                return currentManager;
            }
        }
 

    }
}
