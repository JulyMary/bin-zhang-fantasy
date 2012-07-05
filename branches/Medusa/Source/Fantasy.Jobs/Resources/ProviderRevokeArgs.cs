using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fantasy.Jobs.Resources
{
    public class ProviderRevokeArgs : EventArgs
    {
        public object Resource { get; set; }
    }
}
