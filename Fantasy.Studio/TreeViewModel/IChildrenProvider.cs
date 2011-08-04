using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Fantasy.Studio.TreeViewModel
{
    public interface IChildrenProvider
    {
        IEnumerable<object> GetChildren(object parent);
    }
}
