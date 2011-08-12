using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.Studio.Controls;

namespace Fantasy.Studio.Codons
{
    public interface IToolBoxItemsBuilder
    {
        IEnumerable<ToolBoxItemModel> BuildItems(object owner); 
    }
}
