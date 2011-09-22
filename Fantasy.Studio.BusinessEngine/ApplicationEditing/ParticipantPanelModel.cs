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

        public ParticipantPanelModel(BusinessApplication application, IServiceProvider site)
        {
            this.Site = site;

            this._application = application;

            this._allParticipants.AddRange(application.Participants); 

            this.Items = new ObservableCollection<ParticipantNode>();
        }


        private List<BusinessApplicationParticipant> _allParticipants = new List<BusinessApplicationParticipant>();

        private BusinessApplication _application;

        public void AddRootNodeForClass(BusinessClass @class)
        {
            if (this._application.Participants.Count == 0)
            {
                ParticipantNode node = this.CreateNode(@class);
                this.Items.Add(node);
                node.IsChecked = true;
            }
        }

        private BusinessApplicationParticipant GetParticipant(BusinessClass @class)
        {
            BusinessApplicationParticipant participant = _allParticipants.SingleOrDefault(x => x.Class == @class);
            if (participant == null)
            {
                participant = this.Site.GetRequiredService<IEntityService>().AddBusinessApplicationParticipant(this._application, @class);
                this._allParticipants.Add(participant);
            }
            else
            {
                if (!this._application.Participants.Contains(participant))
                {
                    participant.Application = this._application;
                    this._application.Participants.Add(participant);
                }
            }
            return participant;
        }

       

        private ParticipantNode CreateNode(BusinessClass @class)
        {
            ParticipantNode rs = new ParticipantNode() { Class = @class };
            rs.IsCheckedChanged += new EventHandler(NodeIsCheckedChanged);
            return rs;

        }


        private void NodeIsCheckedChanged(object sender, EventArgs e)
        {
            ParticipantNode node = (ParticipantNode)sender;
            if (node.IsChecked)
            {
                this.OnNodeChecked(node);
            }
            else
            {
                this.OnNodeUnchecked(node);
            }
        }

        private void OnNodeChecked(ParticipantNode node)
        {
            BusinessApplicationParticipant participant = this.GetParticipant(node.Class);

            node.Participant = participant;

            participant.IsEntry = this.Items.Contains(node);


            BusinessClass @class = node.Class;
            var relatives =
                (from prop in @class.AllProperties() where prop.DataClassType != null 
                     from cls in prop.DataClassType.Flatten(c=>c.ChildClasses)
                     select cls)
                .Union(from assn in @class.AllLeftAssociations()
                           from cls in assn.RightClass.Flatten(c => c.ChildClasses)
                           select cls)
                .Union(from assn in @class.AllRightAssociations()
                           from cls in assn.LeftClass.Flatten(c =>c.ChildClasses)
                           select cls)
                .Distinct();
                
            var added = from root in this.Items
                        from n in root.Flatten(n => n.ChildNodes)
                        select n.Class;

            var candidates = relatives.Except(added).OrderBy(c=>c.Name);

            foreach (BusinessClass relClass in candidates)
            {
                ParticipantNode childNode = this.CreateNode(relClass);
                node.ChildNodes.Add(childNode);
            }
        }

        private void OnNodeUnchecked(ParticipantNode node)
        {
            var participants = from decendent in node.Flatten(n => n.ChildNodes)
                               where decendent.Participant != null
                               select decendent.Participant;

            foreach (BusinessApplicationParticipant participant in participants.ToArray())
            {
                this._application.Participants.Remove(participant);
                participant.Application = null;
            }

            var childNodes = from child in node.Flatten(n => n.ChildNodes)
                             where child != node
                             select child;

            foreach (ParticipantNode childNode in childNodes)
            {
                childNode.IsCheckedChanged -= new EventHandler(this.NodeIsCheckedChanged);
            }
            node.ChildNodes.Clear();

        }

        public ObservableCollection<ParticipantNode> Items { get; private set; }



        public void Save()
        {
            
        }
    }
}
