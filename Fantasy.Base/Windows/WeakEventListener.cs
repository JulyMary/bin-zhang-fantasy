using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Fantasy.Windows
{
    public class WeakEventListener : IWeakEventListener
    {

        public WeakEventListener(Func<Type, object, EventArgs, bool> handler)
        {
            if (handler == null)
            {
                throw new ArgumentNullException("handler");
            }
            this._handler = handler;
        }

        private Func<Type, object, EventArgs, bool> _handler;

        #region IWeakEventListener Members

        public bool ReceiveWeakEvent(Type managerType, object sender, EventArgs e)
        {
            return this._handler(managerType, sender, e);
        }

        #endregion
    }
}
