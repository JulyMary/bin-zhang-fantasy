using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Fantasy.XSerialization;

namespace Fantasy.Jobs.Scheduling
{
    [DataContract(Name="Time")]
    [XSerializable("timeTrigger", NamespaceUri = Consts.ScheduleNamespaceURI)]
    public class TimeTrigger : Trigger 
    {

        public TimeTrigger()
        {
            this.Date = DateTime.Now.Date;
        }

        [DataMember] 
        [XAttribute("date")]
        public DateTime Date { get; set; }

        public override TriggerType Type
        {
            get
            {
                return TriggerType.Time;
            }
        }



        protected override string TriggerTimeDescription
        {
            get 
            {
                DateTime t = this.Date + this.StartTime;
                return string.Format("At {0:t} on {0:d}.", t); 
            }
        }
    }
}
