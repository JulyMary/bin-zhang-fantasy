using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.BusinessEngine;
using Fantasy.Windows;

namespace Fantasy.Studio.BusinessEngine.ClassDiagramEditing.Model
{
    public class EnumValueNode : NotifyPropertyChangedObject
    {
        public EnumValueNode(BusinessEnumValue enumValue)
        {
            this.Entity = enumValue;
        }

        public BusinessEnumValue Entity { get; private set; }


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

    }
}
