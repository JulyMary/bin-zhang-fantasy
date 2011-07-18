using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fantasy.AddIns
{
    public interface IAddInTree
    {
        AddIn[] AddIns { get; }

        IAddInTreeNode GetTreeNode(string path);

        void InsertAddIn(AddIn addIn);

        void RemoveAddIn(AddIn addIn);

    }
}
