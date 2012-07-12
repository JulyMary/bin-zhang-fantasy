using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.ServiceModel;
using System.Data.SqlClient;

namespace Fantasy.Jobs.Demo
{
    [Task("reset", Consts.XNamespaceURI)]
    public class ResetTask : ObjectWithSite, ITask
    {


        [TaskMember("connectionString", Flags=TaskMemberFlags.Input | TaskMemberFlags.Required)]
        public string ConnectionString { get; set; }



        #region ITask Members

        public bool Execute()
        {
            ILogger logger = this.Site.GetService<ILogger>();
            logger.SafeLogMessage("demo", "Recreating datatable");

            Db.UsingCommand(this.ConnectionString, cmd => {
                cmd.CommandText = "if exists (select 1 from  sysobjects where  id = object_id('AddTable') and   type = 'U') drop table AddTable";
                cmd.ExecuteNonQuery();
                cmd.CommandText = @"CREATE TABLE [dbo].[AddTable](
	                                    [Id] [int] IDENTITY(0,1) NOT NULL,
	                                    [A] [int] NOT NULL,
	                                    [B] [int] NOT NULL,
	                                    [C] [int] NULL,
                                     CONSTRAINT [PK_AddTable] PRIMARY KEY CLUSTERED 
                                    (
	                                    [Id] ASC
                                    ))";
                cmd.ExecuteNonQuery();



                return null;
            });




            return true;
        }

        #endregion
    }
}
