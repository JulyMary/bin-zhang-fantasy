using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fantasy.Jobs.Resources
{
    public class RevokeArgs : EventArgs
    {
        internal RevokeArgs ()
	    {
            this.SuspendEngine = true;
	    }

        public bool SuspendEngine { get; set; }
    }
}
