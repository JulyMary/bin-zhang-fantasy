using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Fantasy.Jobs.Management;
using System.IO;

namespace Fantasy.Jobs.JobManagerWin
{
    public partial class JobManagerMainForm : Form
    {
        public JobManagerMainForm()
        {
            InitializeComponent();
        }

        private void JobManagerMainForm_Load(object sender, EventArgs e)
        {
            (new StartRemotingCommand()).Execute(null);
            JobManager.Default.Start(); 

        }

        private void JobManagerMainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            JobManager.Default.Stop();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            IJobQueue queue = (IJobQueue)JobManager.Default.GetService(typeof(IJobQueue));
            string si = File.ReadAllText("TestStartInfo.xml");
            JobMetaData job = queue.CreateJobMetaData();
            job.Id = Guid.NewGuid();
            job.Name = "Test2";
            job.Application = "Test";
            job.CreationTime = DateTime.Now;
            job.State = JobState.Unstarted;
            job.Template = "Test";
            job.User = "bin";
            job.StartInfo = si;

            queue.ApplyChange(job);
            
        }
    }
}
