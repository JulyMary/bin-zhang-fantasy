using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Fantasy.Studio.Controls
{
    public interface IChildItemsProvider
    {
        IEnumerable<object> GetChildren(object parent);
    }
}
