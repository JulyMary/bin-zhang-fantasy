using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.BusinessEngine;
using Fantasy.Windows;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace Fantasy.Studio.BusinessEngine.ApplicationEditing
{
    public class ParticipantNode
    {
        public ParticipantNode()
        {
            
        }

        public BusinessApplicationParticipant Participant { get; set; }

        public BusinessClass Class { get; set; }

        private ObservableCollection<ParticipantNode> _childNodes = new ObservableCollection<ParticipantNode>(); 

        public ObservableCollection<ParticipantNode> ChildNodes
        {
            get
            {
                return this._childNodes;
            }
        }

        private bool _isChecked = false;

        public bool IsChecked 
        {
            get { return _isChecked; }
            set
            {
                if (_isChecked != value)
                {
                    _isChecked = value;
                    this.OnIsCheckedChanged(EventArgs.Empty);
                }
            }
        }

        public event EventHandler IsCheckedChanged;

        protected virtual void OnIsCheckedChanged(EventArgs e)
        {
            if (this.IsCheckedChanged != null)
            {
                this.IsCheckedChanged(this, e);
            }
        }

        public ContextMenu ContextMenu { get; set; }

        

    }
}
