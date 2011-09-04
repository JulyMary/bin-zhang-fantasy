using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.Windows;
using Fantasy.XSerialization;

namespace Fantasy.Studio.BusinessEngine.ClassDiagramEditing.Model
{
    public class ClassDiagramGlyph : NotifyPropertyChangedObject, IObjectWithSite
    {

        public ClassDiagram Diagram { get; internal set; }

        public IServiceProvider Site { get; set; }

        private EditingState _editingState = EditingState.Clean;

        public EditingState EditingState
        {
            get { return _editingState; }
            set
            {
                if (_editingState != value)
                {
                    _editingState = value;
                    this.OnPropertyChanged("EditingState");
                }
            }
        }


        public virtual void SaveEntity()
        {
            this.EditingState = Studio.EditingState.Clean;
        }
    }
}
