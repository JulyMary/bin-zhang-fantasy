using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Syncfusion.Windows.Diagram;

namespace Fantasy.Studio.BusinessEngine.ClassDiagramEditing
{
    public class ClassDiagramPanelModel : ObjectWithSite
    {
        public ClassDiagramPanelModel()
        {
            this.DiagramModel = new DiagramModel();
            this.ViewModel = new ClassDiagramViewModel();
        }

        public ClassDiagramViewModel ViewModel { get; private set; }

        public DiagramModel DiagramModel { get; private set; } 
        
    }
}
