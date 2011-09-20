using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.BusinessEngine;
using System.Collections.ObjectModel;

namespace Fantasy.Studio.BusinessEngine.ApplicationEditing
{
    public class ParticipantPanelModel
    {

        public ParticipantPanelModel(BusinessApplication application)
        {
            this.Items = new ObservableCollection<ParticipantNode>();

        }

        public ObservableCollection<ParticipantNode> Items { get; private set; }
    }
}
