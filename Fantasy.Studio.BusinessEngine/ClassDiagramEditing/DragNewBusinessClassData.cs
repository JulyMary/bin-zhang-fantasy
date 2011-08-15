using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fantasy.Studio.BusinessEngine.ClassDiagramEditing
{
   

    public class DragNewBusinessClassData
    {
        private DragNewBusinessClassData()
        {

        }

        static DragNewBusinessClassData()
        {
            Value = new DragNewBusinessClassData();
        }

        public static DragNewBusinessClassData Value { get; private set; }
    }
}
