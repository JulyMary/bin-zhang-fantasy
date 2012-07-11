using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.XSerialization;
using Fantasy.Jobs.Properties;
using System.Xml;

namespace Fantasy.Jobs
{
    [Instruction]
    [XSerializable("choose", NamespaceUri = Consts.XNamespaceURI)]  
    internal class Choose : AbstractInstruction
    {
        public override void Execute()
        {
            IJob job = (Job)this.Site.GetService(typeof(Job));
            IConditionService conditionService = (IConditionService)this.Site.GetService(typeof(IConditionService));
            When chose = null;
            int index = (int)job.RuntimeStatus.Local.GetValue("chose", -1);
            if (index == -1)
            {
                for (int i = 0; i < this.Cases.Count; i++)
                {
                    if (!String.IsNullOrWhiteSpace(this.Cases[i].Condition))
                    {
                        if (conditionService.Evaluate(this.Cases[i].Condition))
                        {
                            job.RuntimeStatus.Local["chose"] = i;
                            chose = this.Cases[i];
                            break;
                        }
                    }
                    else
                    {
                        throw new JobException(Properties.Resources.WhenRequireConditionText);
                    }
                }

                if (chose == null && this.Otherwise != null)
                {
                    job.RuntimeStatus.Local["chose"] = Int32.MaxValue;
                    chose = this.Otherwise;
                }

            }
            else if (index == Int32.MaxValue)
            {
                chose = this.Otherwise;
            }
            else
            {
                chose = this.Cases[index]; 
            }

            if (chose != null)
            {
                job.ExecuteInstruction(chose); 
            }

            
        }

        private IList<When> _cases = new List<When>();

        [XArray(Order=10)]
        [XArrayItem(Name="when", Type = typeof(When))]
        public IList<When> Cases
        {
            get { return _cases; }
        }

        [XElement("otherwise", Order=20)]
        public When Otherwise { get; set; }

#pragma warning disable 169
        [XNamespace]
        private XmlNamespaceManager _namespaces;
#pragma warning restore 169

    }


    
}
