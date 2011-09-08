using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fantasy.Studio.BusinessEngine.ClassDiagramEditing
{


    public class DragNewBusinessEnumData
    {
        private DragNewBusinessEnumData()
        {

        }

        static DragNewBusinessEnumData()
        {
            Value = new DragNewBusinessEnumData();
        }

        public static DragNewBusinessEnumData Value { get; private set; }
    }
}
