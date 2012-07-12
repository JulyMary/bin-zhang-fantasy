using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.ServiceModel;
using System.Data.SqlClient;
using System.Data;

namespace Fantasy.Jobs.Demo
{
    [Task("sum", Consts.XNamespaceURI)]
    public class SumTask : ObjectWithSite, ITask
    {

        #region ITask Members

        public bool Execute()
        {
            this.Site.GetService<ILogger>().SafeLogMessage("demo", "Calculating summary from {0} to {1}.", this.Start, this.Start + this.Count - 1);
           
            Db.UsingCommand(this.ConnectionString, cmd =>
            {
                DataTable table = new DataTable();
                cmd.CommandText = String.Format("select [ID], [A], [B] from [dbo].[AddTable] WHERE ID BETWEEN {0} and {1}", this.Start, this.Start + this.Count - 1);
                using (IDataReader reader = cmd.ExecuteReader())
                {
                    table.Load(reader);
                }


                IProgressMonitor progress = this.Site.GetService<IProgressMonitor>();
                if (progress != null)
                {
                    progress.Minimum = 0;
                    progress.Value = 0;
                    progress.Maximum = this.Count;
                }

                cmd.CommandText = "Update [dbo].[AddTable] set C = @C where id=@id";
                SqlParameter pc = cmd.Parameters.Add("C", SqlDbType.Int);
                SqlParameter pid = cmd.Parameters.Add("id", SqlDbType.Int);

                foreach (DataRow row in table.Rows)
                {
                    int id = (int)row["Id"];
                    int a = (int)row["A"];
                    int b = (int)row["B"];
                    pc.Value = a + b;
                    pid.Value = id;

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


        [TaskMember("start", Flags= TaskMemberFlags.Input | TaskMemberFlags.Required)]
        public int Start { get; set; }

        [TaskMember("count")]
        public int Count { get; set; }

        [TaskMember("connectionString", Flags = TaskMemberFlags.Input | TaskMemberFlags.Required)]
        public string ConnectionString { get; set; }



        #endregion
    }
}
