using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.BusinessEngine;
using System.Collections.ObjectModel;
using Fantasy.BusinessEngine.Services;

namespace Fantasy.Studio.BusinessEngine.ApplicationEditing
{
    public class ParticipantPanelModel : ObjectWithSite
    {

        public ParticipantPanelModel(BusinessApplication application)
        {

            this._application = application;

            this._allParticipants.AddRange(application.Participants); 

            this.Items = new ObservableCollection<ParticipantNode>();
        }


        private List<BusinessApplicationParticipant> _allParticipants = new List<BusinessApplicationParticipant>();

        private BusinessApplication _application;

        public void AddRootParticipantForClass(BusinessClass @class)
        {
            if (!this._application.Participants.Any(x => x.Class == @class))
            {
                BusinessApplicationParticipant participant = _allParticipants.SingleOrDefault(x => x.Class == @class);
                if (participant == null)
                {
                    participant = this.Site.GetRequiredService<IEntityService>().AddBusinessApplicationParticipant(this._application, @class);
                    participant.IsEntry = true;
                }

                this.AddRootPaticipantNode(participant);
            }


        }

        private void AddRootPaticipantNode(BusinessApplicationParticipant participant)
        {
            ParticipantNode node = this.CreateNode(participant);
            this.Items.Add(node);
        }

        private ParticipantNode CreateNode(BusinessApplicationParticipant participant)
        {
            ParticipantNode rs = new ParticipantNode(participant, this);
            rs.IsCheckedChanged += new EventHandler(NodeIsCheckedChanged);
            return rs;

        }


        private void NodeIsCheckedChanged(object sender, EventArgs e)
        {
            ParticipantNode node = (ParticipantNode)sender;
            if (node.IsChecked)
            {
                this.AddChildNodes(node);
            }
            else
            {
                this.RemoveChildNodes(node);
            }
        }

        private void AddChildNodes(ParticipantNode node)
        {
            BusinessClass @class = node.Participant.Class;

            
            

        }

        private void RemoveChildNodes(ParticipantNode node)
        {
            throw new NotImplementedException();
        }

        public ObservableCollection<ParticipantNode> Items { get; private set; }

        public IEnumerable<ParticipantNode> GetChildNodes(ParticipantNode participantNode)
        {
            throw new NotImplementedException();
        }
    }
}
