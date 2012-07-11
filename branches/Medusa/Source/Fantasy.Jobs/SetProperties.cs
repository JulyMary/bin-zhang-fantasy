using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.XSerialization;
using Fantasy.ServiceModel;

namespace Fantasy.Jobs
{
    [Instruction]
    [XSerializable("properties", NamespaceUri = Consts.XNamespaceURI)]  
    internal class SetProperties : AbstractInstruction, IConditionalObject
    {

        [XArray(Serializer = typeof(JobPropertiesSerializer))]
        private List<JobProperty> _list = new List<JobProperty>();

        public override void Execute()
        {
            IStringParser parser = this.Site.GetRequiredService<IStringParser>();
            ILogger logger = this.Site.GetService<ILogger>();
            IJob job = this.Site.GetRequiredService<IJob>();
            IConditionService conditionSvc = this.Site.GetRequiredService<IConditionService>();
            if (conditionSvc.Evaluate(this))
            {
                int index = job.RuntimeStatus.Local.GetValue("setproperties.index", 0);
                while (index < _list.Count)
                {
                    JobProperty prop = this._list[index];
                    if (conditionSvc.Evaluate(prop))
                    {
                        string value = parser.Parse(prop.Value);
                        job.SetProperty(prop.Name, value);
                        if (logger != null)
                        {
                            logger.LogMessage("property", MessageImportance.Low, "set property {0} as {1}", prop.Name, value);
                        }
                    }
                    index++;
                    job.RuntimeStatus.Local["setproperties.index"] = index;
                    
                }
            }

        }

        [XAttribute("condition")]
        public string Condition { get; set; }
    }
}
