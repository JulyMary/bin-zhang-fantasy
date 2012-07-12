using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.ServiceModel;
using System.Data.SqlClient;

namespace Fantasy.Jobs.Demo
{
    [Task("insert", Consts.XNamespaceURI)]
    public class InsertTask : ObjectWithSite, ITask
    {
        public InsertTask()
        {
            this.Count = 1000;
        }

        #region ITask Members

        public bool Execute()
        {
            this.Site.GetService<ILogger>().SafeLogMessage("demo", "inserting data");

            Db.UsingCommand(this.ConnectionString, cmd => {
               
                cmd.CommandText = @"INSERT INTO [dbo].[AddTable]([A] ,[B]) VALUES (@A,@B)";
                SqlParameter pa = cmd.Parameters.Add("A", System.Data.SqlDbType.Int);
                SqlParameter pb = cmd.Parameters.Add("B", System.Data.SqlDbType.Int);

                IProgressMonitor progress = this.Site.GetService<IProgressMonitor>();
                if (progress != null)
                {
                    progress.Minimum = 0;
                    progress.Value = 0;
                    progress.Maximum = this.Count;
                }

                Random random = new Random();

                for (int i = 0; i < this.Count; i++)
                {
                    pa.Value = random.Next(10000);
                    pb.Value = random.Next(10000);
                    cmd.ExecuteNonQuery();
                    if (progress != null)
                    {
                        progress.Value++;
                    }
                }
                
                return null;
            });

            return true;
        }

        #endregion

        [TaskMember("count")]
        public int Count { get; set; }

        [TaskMember("connectionString", Flags = TaskMemberFlags.Input | TaskMemberFlags.Required)]
        public string ConnectionString { get; set; }
    }
}
