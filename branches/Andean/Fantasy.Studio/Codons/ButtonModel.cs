using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.AddIns;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Fantasy.Collection;
using Fantasy.Windows;
using System.Windows.Input;

namespace Fantasy.Studio.Codons
{
    public class ButtonModel : NotifyPropertyChangedObject, IUpdateStatus
    {
        public ButtonModel ()
	    {
            this.ChildItems = new UnionedObservableCollection<object>();
            this.ChildUpdateStatus = new List<IUpdateStatus>();
	    }

        public ConditionCollection Conditions { get; set; }


        public object Owner { get; set; }


        private bool _hasIcon = false;

        public bool HasIcon
        {
            get { return _hasIcon; }
            private set
            {
                if (_hasIcon != value)
                {
                    _hasIcon = value;
                    this.OnPropertyChanged("HasIcon");
                }
            }
        }

        private object _icon;
        public object Icon
        {
            get
            {
                return _icon;
            }

            set
            {
                if (_icon != value)
                {
                    _icon = value;
                    this.OnPropertyChanged("Icon");
                    this.HasIcon = _icon != null;
                }
            }
        }

        private string _text;
        public string Text
        {
            get
            {
                return _text;
            }
            set
            {
                if (_text != value)
                {
                    _text = value;
                    this.OnPropertyChanged("Text");
                }
            }
        }

        private System.Windows.Input.ICommand _command;
        public System.Windows.Input.ICommand Command
        {
            get
            {
                return _command;
            }
            set
            {
                if (_command != value)
                {
                    _command = value;
                    this.OnPropertyChanged("Command");
                    HandleCheckedData();
                }
            }
        }

        private bool _isCheckable = false;

        public bool IsCheckable
        {
            get { return _isCheckable; }
            set
            {
                if (_isCheckable != value)
                {
                    _isCheckable = value;
                    HandleCheckedData();
                }
            }
        }

        private void HandleCheckedData()
        {
            if (_isCheckable && this.Command != null)
            {
                ICommandData data = this.Command.CommandData();
                this.IsChecked = object.Equals(true, data.Value);
                data.PropertyChanged += new PropertyChangedEventHandler(CommandDataChanged);
            }
        }

        private void CommandDataChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Value")
            {
                this.IsChecked = object.Equals(true, ((ICommandData)sender).Value);
            }
        }

        private bool _isChecked = false;

        public bool IsChecked
        {
            get { return _isChecked; }
            private set
            {
                if (_isChecked != value)
                {
                    _isChecked = value;
                    this.OnPropertyChanged("IsChecked");
                }
            }
        }

        private bool _visible = true;
        public bool Visible
        {
            get
            {
                return _visible;
            }
            set
            {
                if (_visible != value)
                {
                    _visible = value;
                    this.OnPropertyChanged("Visible");
                }
            }
        }

        public UnionedObservableCollection<object> ChildItems { get; private set; }

        public IList<IUpdateStatus> ChildUpdateStatus { get; private set; }

        #region IUpdateStatus Members

        public void Update(object caller)
        {
            this.Visible = this.Conditions.GetCurrentConditionFailedAction(caller) != ConditionFailedAction.Exclude;
            if (this.Visible)
            {
                foreach(IUpdateStatus child in this.ChildUpdateStatus)
                {
                    child.Update(caller);
                }
            }
        }

        #endregion
    }
}
