using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fantasy.BusinessEngine
{
    public class BusinessApplicationACL : BusinessEntity
    {
        public virtual BusinessEnumValue State
        {
            get
            {
                return (BusinessEnumValue)this.GetValue("State", null);
            }
            set
            {
                this.SetValue("State", value);
            }
        }

        public virtual BusinessRole Role
        {
            get
            {
                return (BusinessRole)this.GetValue("Role", null);
            }
            set
            {
                this.SetValue("Role", value);
            }
        }

        public virtual BusinessApplicationParticipant Participant
        {
            get
            {
                return (BusinessApplicationParticipant)this.GetValue("Participant", null);
            }
            set
            {
                this.SetValue("Participant", value);
            }
        }

        protected internal virtual string PersistedACL
        {
            get
            {
                return (string)this.GetValue("PersistedACL", null);
            }
            set
            {
                this.SetValue("PersistedACL", value);
            }
        }
    }
}
