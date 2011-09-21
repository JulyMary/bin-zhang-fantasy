using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.BusinessEngine;
using Fantasy.Windows;
using System.Collections.ObjectModel;

namespace Fantasy.Studio.BusinessEngine.ApplicationEditing
{
    public class ParticipantNode : NotifyPropertyChangedObject
    {
        public ParticipantNode(BusinessApplicationParticipant participant, ParticipantPanelModel model)
        {
            this._model = model;
            this.Participant = Participant;
        }

        public BusinessApplicationParticipant Participant { get; private set; }

        ParticipantPanelModel _model;

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

       
        

        
    }
}
