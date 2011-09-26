﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fantasy.Studio.BusinessEngine.ApplicationEditing
{
    public class ParticipantACLEditor : MultiPageEditingViewContent
    {
        public override string EditingPanelPath
        {
            get { return "fantasy/studio/businessengine/participantacleditor/panels"; }
        }

        public override string CommandBindingPath
        {
            get { return "fantasy/studio/businessengine/participantacleditor/commandbindings"; }
        }

        public override string DocumentName
        {
            get
            {
                if (this.Data != null)
                {
                    ParticipantACL acl = (ParticipantACL)this.Data;
                    return acl.Entity.Application.FullName + "." + acl.Entity.Class.FullName + ".acl";
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        public override string DocumentType
        {
            get { return "participantacl"; }
        }




        public override void Load(object data)
        {
            base.Load(data);

            this.Title = ((ParticipantACL)data).Entity.Class.Name;
        }
    }
}
