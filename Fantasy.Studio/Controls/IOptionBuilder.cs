using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fantasy.Studio.Controls
{
    public interface IOptionBuilder
    {
        IEnumerable<OptionNode> Build(object owner);
    }
}
