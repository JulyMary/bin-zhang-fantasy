using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Fantasy.Windows
{
    public class WeakEventListener : IWeakEventListener
    {

        public override bool Equals(object obj)
        {
            WeakEventListener we = obj as WeakEventListener;
            if (we != null)
            {
                return we._handler == this._handler;
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return this._handler.GetHashCode();
        }



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


        public static bool operator == (WeakEventListener x, WeakEventListener y)
        {
            return Object.Equals(x, y);
        }

        public static bool operator !=(WeakEventListener x, WeakEventListener y)
        {
            return ! Object.Equals(x, y);
        }
    }
}
