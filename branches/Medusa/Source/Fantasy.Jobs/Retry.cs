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
    [XSerializable("retry", NamespaceUri = Consts.XNamespaceURI)] 
    internal class Retry : Sequence 
    {
        public Retry()
        {
            this.Count = "1";
            this.Sleep = "00:00:00";
        }

        public override void Execute()
        {
            IJob job = this.Site.GetRequiredService<IJob>();
            IJobEngine engine = this.Site.GetRequiredService<IJobEngine>();
            ILogger logger = this.Site.GetService<ILogger>();
            bool success = false;
            IStringParser parser = this.Site.GetRequiredService<IStringParser>();
            int count = Int32.Parse(parser.Parse(this.Count));
            int times = job.RuntimeStatus.Local.GetValue("retry.times", 0);
            TimeSpan sleep = TimeSpan.Parse(parser.Parse(this.Sleep)); 
            while (times < count && !success)
            {

                try
                {
                    this.ExecuteSequence();
                    success = true;
                }
                catch(Exception error)
                {
                    if (!(error is ThreadAbortException))
                    {
                        times++;
                        job.RuntimeStatus.Local["retry.times"] = times;
                        if (times < count)
                        {
                            job.RuntimeStatus.Local["sequence.current"] = 0;
                            logger.LogError(LogCategories.Instruction, error, "Retry instruction catchs a exception, will try again later.");
                            if (sleep > TimeSpan.Zero)
                            {
                               engine.Sleep(sleep);
                            }
                        }
                        else
                        {
                            logger.LogError(LogCategories.Instruction, error, "Retry instruction catchs a exception and repeat times exceed maximum number ({0}).", this.Count); 
                            throw ;
                        }
                    }
                }

                
            }
        }

        [XAttribute("count")]
        public string Count { get; set; }

        [XAttribute("sleep")]
        public string Sleep { get; set; }
        
    }
}
