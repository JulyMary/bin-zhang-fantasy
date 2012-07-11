using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.XSerialization;
using System.Threading;
using Fantasy.ServiceModel;

namespace Fantasy.Jobs
{

    [Instruction]
    [XSerializable("until-success", NamespaceUri = Consts.XNamespaceURI)]  
    internal class UntilSuccess : AbstractInstruction
    {

        [XAttribute("failIfAllSkipped")]
        private bool _failIfAllSkipped = true;

        public bool FailIfAllSkipped
        {
            get { return _failIfAllSkipped; }
            set { _failIfAllSkipped = value; }
        }

        public override void Execute()
        {
            if (this.Items.Count > 0)
            {
                IConditionService conditionSvc = this.Site.GetRequiredService<IConditionService>();
                IJob job = this.Site.GetRequiredService<IJob>();
                ILogger logger = this.Site.GetService<ILogger>();
                int index = job.RuntimeStatus.Local.GetValue("until-success.index", 0);
                bool success = false;
                bool hasException = false;
                while (index < this.Items.Count && !success)
                {
                    try
                    {
                        Try chance = this.Items[index];
                        if (conditionSvc.Evaluate(chance))
                        {
                            if (logger != null)
                            {
                                logger.LogMessage(LogCategories.Instruction, "Execute try No.{0}", index);
                            }
                            job.ExecuteInstruction(chance);
                            success = true;
                        }
                        else
                        {
                            logger.LogMessage(LogCategories.Instruction, "Skip try No.{0}", index);
                        }
                        
                    }
                    catch (ThreadAbortException)
                    {

                    }
                    catch (Exception error)
                    {
                        hasException = true;
                        job.RuntimeStatus.Local["until-success.index"] = index;
                        if (logger != null)
                        {
                            logger.LogError(LogCategories.Instruction, error,  "try instruction faild, try next one.");

                        }
                    }
                    index++;
                }

               
                if (!success)
                {
                   

                    if (hasException)
                    {
                        if (logger != null)
                        {
                            logger.LogError(LogCategories.Instruction, Properties.Resources.UnitlSuccessFailedText);
                        }
                        throw new JobException(Properties.Resources.UnitlSuccessFailedText);
                    }
                    else
                    {
                        if (logger != null)
                        {
                            logger.LogError(LogCategories.Instruction, Properties.Resources.UntilSuccessAllSkippedText);
                            
                        }
                        throw new JobException(Properties.Resources.UntilSuccessAllSkippedText);
                    }
                }
               
            }
        }

        private IList<Try> _items = new List<Try>();

        [XArray(Order = 10)]
        [XArrayItem(Name = "try", Type = typeof(Try))]
        public IList<Try> Items
        {
            get { return _items; }
        }
    }
}
