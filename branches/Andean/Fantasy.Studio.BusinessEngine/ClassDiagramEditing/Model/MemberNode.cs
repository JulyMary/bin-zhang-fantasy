using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.Windows;
using Fantasy.BusinessEngine;

namespace Fantasy.Studio.BusinessEngine.ClassDiagramEditing.Model
{
    public abstract class MemberNode : NotifyPropertyChangedObject, IObjectWithSite 
    {
       

        public ClassDiagram Diagram { get; private set; }

        public abstract string Name { get; set; }

        public abstract long DisplayOrder { get; set; }

        private bool _canMoveUp;

        public bool CanMoveUp
        {
            get { return _canMoveUp; }
            set
            {
                if (_canMoveUp != value)
                {
                    _canMoveUp = value;
                    this.OnPropertyChanged("CanMoveUp");
                }
            }
        }

        private bool _canMoveDown;

        public bool CanMoveDown
        {
            get { return _canMoveDown; }
            set
            {
                if (_canMoveDown != value)
                {
                    _canMoveDown = value;
                    this.OnPropertyChanged("CanMoveDown");
                }
            }
        }

        public abstract bool IsSystem {get;}

        public IServiceProvider Site { get; set; }

        private EditingState _editingState = EditingState.Clean;

        public virtual EditingState EditingState
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


        public abstract void SaveEntity();
       

        public bool IsInherited { get; set; }
    }
}
