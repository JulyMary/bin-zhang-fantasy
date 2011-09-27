using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.Studio.Controls;

namespace Fantasy.Studio
{
    public interface IToolBoxItemsBuilder
    {
        ToolBoxItemModel[] BuildItems(object owner); 
    }
}
