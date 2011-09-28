﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.BusinessEngine;
using System.Collections.ObjectModel;
using Fantasy.BusinessEngine.Services;
using Fantasy.Studio.Services;
using Fantasy.ServiceModel;

namespace Fantasy.Studio.BusinessEngine.ApplicationEditing
{
    public class ParticipantPanelModel : ObjectWithSite
    {

        public ParticipantPanelModel(BusinessApplication application, IServiceProvider site)
        {
            this.Site = site;

            this._application = application;

            this._allParticipants.UnionWith(application.Participants); 

            this.Items = new ObservableCollection<ParticipantNode>();
            Refresh();
        }



        public void Refresh()
        {

            if (this.Items.Count == 1)
            {
                var query = from node in this.Items[0].Flatten(n => n.ChildNodes)
                            select node;
                foreach (ParticipantNode node in query)
                {
                    node.IsCheckedChanged -= NodeIsCheckedChanged;
                }

                this.Items.Clear();
            }


            BusinessApplicationParticipant rootParticipant = this._application.Participants.SingleOrDefault(p => p.IsEntry && p.Class.EntityState != EntityState.Deleted);
            if (rootParticipant != null)
            {
                ParticipantNode rootNode = this.CreateNode(rootParticipant.Class);
                this.Items.Add(rootNode);
                ShowParticipant(rootNode);
            }


            var orphans = this._application.Participants.Except(
                from root in this.Items
                          select root.Flatten(n => n.ChildNodes) into allnodes
                          from node in allnodes
                          where node.Participant != null
                          select node.Participant);

            foreach (BusinessApplicationParticipant orphan in orphans.ToArray())
            {
                this._application.Participants.Remove(orphan);
                orphan.Application = null;
            }


        }

        private void ShowParticipant(ParticipantNode node)
        {

            var query = from paricipant in this._application.Participants
                        where paricipant.Class == node.Class && node.Class.EntityState != EntityState.Deleted
                        select paricipant;
            if (query.Any())
            {
                node.IsChecked = true;
                foreach (ParticipantNode child in node.ChildNodes)
                {
                    ShowParticipant(child);
                }
            }


           
        }

        private HashSet<BusinessApplicationParticipant> _allParticipants = new HashSet<BusinessApplicationParticipant>();

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
            IMenuService ms = this.Site.GetRequiredService<IMenuService>();

           
            ParticipantNode rs = new ParticipantNode() { Class = @class};
            rs.ContextMenu = ms.CreateContextMenu("fantasy/studio/businessengine/applicationeditor/participantpanel/participant/contextmenu", rs, this.Site);
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
                this.ClearChildNodes(node);
            }
        }

        private void AddChildNodes(ParticipantNode node)
        {
            BusinessApplicationParticipant participant = this.GetParticipant(node.Class);

            node.Participant = participant;

            participant.IsEntry = this.Items.Contains(node);


            BusinessClass @class = node.Class;

            var childClasses = @class.Flatten(c => c.ChildClasses).Where(c=>c != @class);

            var propClass = from prop in
                                @class.AllProperties()
                                .Union(from childClass in childClasses
                                    from prop in childClass.Properties 
                                    select prop)
                            where prop.DataClassType != null
                            from cls in prop.DataClassType.Flatten(c => c.ChildClasses)
                            select cls;

            var leftAssnClass = from assn in
                                    @class.AllLeftAssociations()
                                        .Union(from childClass in childClasses
                                               from assn in childClass.LeftAssociations
                                               select assn)
                                from cls in assn.RightClass.Flatten(c => c.ChildClasses)
                                select cls;

            var rightAssnClass = from assn in
                                     @class.AllRightAssociations()
                                         .Union(from childClass in childClasses
                                                from assn in childClass.RightAssociations
                                                select assn)
                                 from cls in assn.LeftClass.Flatten(c => c.ChildClasses)
                                 select cls;

            var relatives = childClasses.Concat(propClass).Concat(leftAssnClass).Concat(rightAssnClass).Distinct();
                
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

        private void ClearChildNodes(ParticipantNode node)
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
            this._allParticipants.IntersectWith(this._application.Participants);
           
        }

        public  void ClearRootNode()
        {
            ParticipantNode root = this.Items[0];
            this.ClearChildNodes(root);
            this._application.Participants.Remove(root.Participant);
            root.Participant.Application = null;

            this.Items.Clear();


        }
    }
}
