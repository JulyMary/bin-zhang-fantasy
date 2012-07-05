using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ClickView.XSerialization;
using System.Threading;

namespace ClickView.Jobs
{

    [Instruction]
    [XSerializable("chances", NamespaceUri = Consts.XNamespaceURI)]  
    public class Chances : AbstractInstruction
    {

        public override void Execute()
        {
            if (this.Items.Count > 0)
            {
                IJob job = this.Engine.GetRequiredService<IJob>();
                ILogger logger = this.Engine.GetService<ILogger>();
                int index = job.RuntimeStatus.Local.GetValue("chances.index", 0);
                bool success = false;
                while (index < this.Items.Count && !success)
                {
                    try
                    {
                        Chance chance = this.Items[index];
                        job.ExecuteInstruction(chance); 
                        success = true;
                    }
                    catch(Exception error)
                    {
                        if (!(error is ThreadAbortException))
                        {
                            index++;
                            job.RuntimeStatus.Local["chances.index"] = index;
                            if (index < this.Items.Count)
                            {
                                logger.LogMessage(LogCategories.Instruction, MessageImportance.Low, "chance instruction faild, try next one. {0}", error.ToString());

                            }
                            else
                            {
                                logger.LogMessage(LogCategories.Instruction, MessageImportance.Low, "All chance faild. {0}", error.ToString());
                                throw error;
                            }
                        }
                    }
                }
            }
        }

        private IList<Chance> _items = new List<Chance>();

        [XArray(Order = 10)]
        [XArrayItem(Name = "chance", Type = typeof(Chance))]
        public IList<Chance> Items
        {
            get { return _items; }
        }
    }
}
