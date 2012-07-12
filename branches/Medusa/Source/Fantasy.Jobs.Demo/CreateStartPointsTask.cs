using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fantasy.Jobs.Demo
{
    [Task("createStartPoints", Consts.XNamespaceURI)]
    public class CreateStartPointsTask : ObjectWithSite, ITask
    {

        public CreateStartPointsTask()
        {
            this.Count = 1000000;
            this.Step = 10000;
        }

        #region ITask Members

        public bool Execute()
        {
            int len = this.Count / this.Step;
            this.StartPoint = new int[len];
           
            for (int i = 0; i < len; i++)
            {
                this.StartPoint[i] = i * this.Step;
            }

            return true;
        }

        #endregion

        [TaskMember("startPoints", Flags = TaskMemberFlags.Output)]
        public int[] StartPoint { get; set; }

        [TaskMember("count")]
        public int Count { get; set; }

        [TaskMember("step")]
        public int Step { get; set; }
    }
}
