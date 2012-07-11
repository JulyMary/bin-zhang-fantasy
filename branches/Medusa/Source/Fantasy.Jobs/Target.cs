using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.XSerialization;
using Fantasy.Jobs.Properties;
using System.Threading;
using Fantasy.ServiceModel;

namespace Fantasy.Jobs
{
    [Instruction]
    [XSerializable("target", NamespaceUri = Consts.XNamespaceURI)]
    internal class Target : Sequence, IConditionalObject
    {
        private bool _executing = false;

        public override void Execute()
        {

            if (!HasExecuted() && !_executing)
            {
                _executing = true;
                
                IJob job = (IJob)this.Site.GetService(typeof(IJob));

                ILogger logger = (ILogger)this.Site.GetService(typeof(ILogger));
                bool  hasError = false;
                try
                {
                    try
                    {

                        if (logger != null)
                        {
                            logger.LogMessage(LogCategories.Instruction, MessageImportance.Low, "Execute target {0}.", this.Name);
                        }

                        if (!job.RuntimeStatus.Local.GetValue("target.executingFinally", false))
                        {
                            if (job.RuntimeStatus.Local.GetValue("target.executingOnFail", false))
                            {
                                throw new JobException("Resuming onFail operation.");
                            }

                            IConditionService conditionsvc = (IConditionService)this.Site.GetService(typeof(IConditionService));
                            if (conditionsvc.Evaluate(this))
                            {
                                ExecuteDependsOnTargets();
                                base.ExecuteSequence();

                            }
                        }
                        
                    }
                    catch (ThreadAbortException)
                    {

                    }
                    catch (Exception error)
                    {
                        logger.SafeLogError(LogCategories.Instruction, error, "An error occurs when execute target {0}.", this.Name);
                        hasError = true;
                        
                    }
                    //bool rethrow = false;
                    if (hasError)
                    {
                        try
                        {
                            job.RuntimeStatus.Local["target.rethrow"] = true;
                            job.RuntimeStatus.Local["target.rethrow"] = OnError();
                           
                        }
                        catch (ThreadAbortException)
                        {

                        }
                        catch (Exception error)
                        {
                            logger.SafeLogError(LogCategories.Instruction, error, "An error occurs when execute onFailed of target {0}.", this.Name);
                            job.RuntimeStatus.Local["target.rethrow"] = true;
                        }
                    }

                   
                    OnFinal();


                    if (job.RuntimeStatus.Local.GetValue("target.rethrow", false))
                    {
                        throw new JobException(String.Format("An error occurs when execute target {0}.", this.Name));
                    }
                }
                finally
                {

                    _executing = false;
                }

            }
        }

        
        private void OnFinal()
        {
            try
            {
                IJob job = this.Site.GetRequiredService<IJob>();
                if (!string.IsNullOrEmpty(this.Finally))
                {
                    job.RuntimeStatus.Local["target.executingFinally"] = true;
                    job.ExecuteTarget(this.Finally);
                }
            }
            catch (ThreadAbortException)
            {

            }
            catch(Exception)
            {
                this.SetAsExecuted();
                throw;
            }
            this.SetAsExecuted();
            
        }

        private bool OnError()
        {
            IJob job = this.Site.GetRequiredService<IJob>();
            ILogger logger = this.Site.GetService<ILogger>();

            bool rs = false;
            switch (this.FailAction)
            {
                case FailActions.Terminate:

                    if (logger != null)
                    {
                        logger.LogError(LogCategories.Instruction,  Properties.Resources.TargetTermianteText, this.Name);
                    }
                    break;
                case FailActions.Continue:
                    if (logger != null)
                    {
                        logger.LogError(LogCategories.Instruction,  Properties.Resources.TargetContinueText, this.Name);
                    }
                    break;
            }


            if (!string.IsNullOrEmpty(this.OnFail))
            {
                job.RuntimeStatus.Local["target.executingOnFail"] = true;
                job.ExecuteTarget(this.OnFail);
            }

            switch (this.FailAction)
            {
                case FailActions.Throw:
                    rs = true;
                    break;
                case FailActions.Terminate:
                    IJobEngine engine = this.Site.GetRequiredService<IJobEngine>();
                    engine.Fail();
                    break;
            }
            return rs;
        }

        private void ExecuteDependsOnTargets()
        {
            IStringParser parser = (IStringParser)this.Site.GetService(typeof(IStringParser));
            IJob job = (IJob)this.Site.GetService(typeof(IJob));
            string[] targets;
            if (!string.IsNullOrEmpty(this.DependsOnTargets))
            {
                string s = parser.Parse(this.DependsOnTargets);
                if (!string.IsNullOrWhiteSpace(s))
                {
                    targets = parser.Parse(this.DependsOnTargets).Split(';');
                    int index = (int)job.RuntimeStatus.Local.GetValue("target.dependsOnTarget.index", 0);
                    while (index < targets.Length)
                    {
                        string name = targets[index];
                        if (!string.IsNullOrWhiteSpace(name))
                        {
                            job.ExecuteTarget(targets[index]);
                        }
                        index++;
                        job.RuntimeStatus.Local["target.dependsOnTarget.index"] = index;
                    }
                }
            }
        }

        private void SetAsExecuted()
        {
            IJob job = (IJob)this.Site.GetService(typeof(IJob));
            string[] oldValue = (string[])job.RuntimeStatus.Global.GetValue(ExecutedTargetsVarName, new string[0]);
            string[] newValue = new string[oldValue.Length + 1];
            Array.Copy(oldValue, newValue, oldValue.Length);
            newValue[newValue.Length - 1] = this.Name;
            job.RuntimeStatus.Global[ExecutedTargetsVarName] = newValue;
        }

        private const string ExecutedTargetsVarName = "targets.executedTargets";

        private bool HasExecuted()
        {
            IJob job = (IJob)this.Site.GetService(typeof(IJob));
            string[] executed = (string[])job.RuntimeStatus.Global[ExecutedTargetsVarName];

            return executed != null ? Array.IndexOf(executed, this.Name) >= 0 : false;
        }

        [XAttribute("dependsOnTargets", Order = 10)]
        public string DependsOnTargets { get; set; }

        [XAttribute(NameAttributeName, Order = 0)]
        public string Name { get; set; }

        internal const string NameAttributeName = "name";

        [XAttribute("condition", Order = 20)]
        public string Condition { get; set; }

        [XAttribute("onFail", Order = 30)]
        public string OnFail { get; set; }

        private FailActions _failAction = FailActions.Throw;

        [XAttribute("failAction", Order = 40)]
        public FailActions FailAction
        {
            get { return _failAction; }
            set { _failAction = value; }
        }

        [XAttribute("finally", Order = 50)]
        public string Finally { get; set; }

    }


}
