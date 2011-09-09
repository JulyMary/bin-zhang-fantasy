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


        [XAttribute("id")]
        private Guid _id;

        public Guid Id
        {
            get { return _id; }
            set
            {
                if (_id != value)
                {
                    _id = value;
                    this.OnPropertyChanged("Id");
                }
            }
        }

        [XAttribute("left")]
        private double _left = 0;

        public double Left
        {
            get { return _left; }
            set
            {
                if (_left != value)
                {
                    _left = value;
                    this.OnPropertyChanged("Left");
                }
            }
        }

        [XAttribute("top")]
        private double _top = 0;
        public double Top
        {
            get { return _top; }
            set
            {
                if (_top != value)
                {
                    _top = value;
                    this.OnPropertyChanged("Top");
                }
            }
        }



        [XAttribute("width")]
        private double _width = 180;

        public double Width
        {
            get { return _width; }
            set
            {
                if (_width != value)
                {
                    _width = value;
                    this.OnPropertyChanged("Width");
                }
            }
        }

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
