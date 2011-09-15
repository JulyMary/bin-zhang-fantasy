using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.ServiceModel;
using Fantasy.BusinessEngine.Services;
using System.Data;

namespace Fantasy.BusinessEngine.MSSQL
{
    public class SqlDDLService : ServiceBase, IDDLService
    {
        public override void InitializeService()
        {
            LoadDataTypes();
            base.InitializeService();
        }

        private void LoadDataTypes()
        {
            IEntityService es = this.Site.GetRequiredService<IEntityService>();
            using (IDbCommand cmd = es.CreateCommand())
            {
                cmd.CommandText = "select name from sys.types order by name";

                this.DataTypes = cmd.ExecuteList<string>().ToArray();

                cmd.CommandText = "select name from sys.schemas";

                this.Schemas = cmd.ExecuteList<string>().ToArray();

                cmd.CommandText = "select name from sys.filegroups";
                this.TableSpaces = cmd.ExecuteList<string>().ToArray();

            }
        }


        private int ExecuteSql(string sql)
        {
            IEntityService es = this.Site.GetRequiredService<IEntityService>();
            using (IDbCommand cmd = es.CreateCommand())
            {
                cmd.CommandText = sql;
                return cmd.ExecuteNonQuery();
            }
        }

        public string[] DataTypes { get; private set; }

        public string[] Schemas { get; private set; }

        public string[] TableSpaces { get; private set; }



       

        public void CreateClassTable(BusinessClass @class)
        {



            if (@class.PreviousState == EntityState.New)
            {
                if (@class.IsSimple)
                {
                    this.CreateSimpleClassTable(@class);
                }
                else
                {
                    this.CreateStandardClassTable(@class);
                }
            }
            else if (@class.PreviousState == EntityState.Clean)
            {
                if (@class.IsSimple)
                {
                    this.UpdateSimpleClassTable(@class);
                }
                else
                {
                    this.UpdateStandardClassTable(@class);
                }
            }
        }

        private void UpdateStandardClassTable(BusinessClass @class)
        {

            string alter = string.Format("alter table [{0}].[{1}]", @class.TableSchema, @class.TableName) ;

            foreach (BusinessProperty property in @class.PreviousProperties.Where(p => p.EntityState == EntityState.Deleted))
            {
                string sql = string.Format("{0} drop column [{1}] ", alter, property.FieldName);
                this.ExecuteSql(sql);

            }

            foreach (BusinessProperty property in @class.Properties.Where(p => p.PreviousState == EntityState.New))
            {
                string sql = string.Format("{0} add {1}", alter, this.GetColumnDefine(property));
                this.ExecuteSql(sql);
            }

            foreach (BusinessProperty property in @class.Properties.Where(p => p.PreviousState == EntityState.Clean))
            {
                string sql = string.Format("{0} alter column {1}", alter, this.GetColumnDefine(property));
                this.ExecuteSql(sql);
            }

        }

      
        private void UpdateSimpleClassTable(BusinessClass @class)
        {
            throw new NotImplementedException();
        }

        private void CreateStandardClassTable(BusinessClass @class)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("create table [{0}].[{1}] ( {2} )", @class.TableSchema, @class.TableName, this.GetCreateColumnsDefine(@class));
            if(! string.IsNullOrEmpty(@class.TableSpace))
            {
               sql.AppendFormat(" on [{0}]", @class.TableSpace);    
            }
            this.ExecuteSql(sql.ToString());


        }

        private string GetCreateColumnsDefine(BusinessClass @class)
        {
            StringBuilder rs = new StringBuilder();
            if (!@class.IsSimple)
            {
                rs.AppendFormat("[Id] {0} NOT NULL", this.Site.GetRequiredService<IBusinessDataTypeRepository>().Guid.DefaultDatabaseType);
            }
            foreach (BusinessProperty prop in @class.Properties)
            {
                if (rs.Length > 0)
                {
                    rs.Append(", ");
                }
                rs.Append(GetColumnDefine(prop));
            }
            //TODO: Add Constraint CHECK

            rs.AppendFormat(", CONSTRAINT [PK_{0}] PRIMARY KEY CLUSTERED ([Id] ASC)", @class.TableName);

            return rs.ToString();

        }

        private string GetColumnDefine(BusinessProperty prop)
        {
            StringBuilder rs = new StringBuilder();
            rs.AppendFormat("[{0}] [{1}]", prop.FieldName, prop.FieldType);
            if (prop.Length > 0)
            {
                if (prop.Precision > 0)
                {
                    rs.AppendFormat(" ({0}, {1})", prop.Length, prop.Precision);
                }
                else
                {
                    rs.AppendFormat(" ({0})", prop.Length);
                }
            }

            rs.AppendFormat(" {0} NULL", prop.IsNullable ? string.Empty : "NOT");
            return rs.ToString();
        }

        private void CreateSimpleClassTable(BusinessClass @class)
        {
            throw new NotImplementedException();
        }


        public void DeleteClassTable(BusinessClass @class)
        {
            string sql = String.Format("if exists (select 1 from sys.tables t join sys.schemas s on t.schema_id = s.schema_id where s.name='{0}' and t.name= '{1}') drop table [{0}].[{1}]" , @class.TableSchema, @class.TableName);
            this.ExecuteSql(sql);
        }

        public string[] GetTableFullNames()
        {
            string sql = "select t2.name + '.' + t1.name  from sys.tables t1, sys.schemas t2 where  t1.schema_id = t2.schema_id";

            IEntityService es = this.Site.GetRequiredService<IEntityService>();
            using (IDbCommand cmd = es.CreateCommand())
            {
                cmd.CommandText = sql;
                return cmd.ExecuteList<string>().ToArray();

            }
        }



        public string[] GetTableNames(string schema)
        {
            string sql = String.Format("select t1.name from sys.tables t1, sys.schemas t2 where  t1.schema_id = t2.schema_id and t2.name = '{0}'", schema) ;

            IEntityService es = this.Site.GetRequiredService<IEntityService>();
            using (IDbCommand cmd = es.CreateCommand())
            {
                cmd.CommandText = sql;
                return cmd.ExecuteList<string>().ToArray();

            }
        }



     

        public void CreateAssoicationTable(BusinessAssociation association)
        {
            string sql = String.Format("create table [{0}].[{1}] ( [LEFTID] uniqueidentifier not null, [RIGHTID] uniqueidentifier not null, CONSTRAINT [PK_{1}] PRIMARY KEY CLUSTERED ([LEFTID], [RIGHTID]) )",
                association.TableSchema, association.TableName);

            if (!string.IsNullOrEmpty(association.TableSpace))
            {
                sql += String.Format(" on [{0}]", association.TableSpace);
            }

            this.ExecuteSql(sql);

            sql = string.Format("alter table [{0}].[{1}] add constraint FK_{1}_LEFT foreign key (LEFTID) references [{2}].[{3}] ([ID]) on delete cascade",
                association.TableSchema, association.TableName, association.LeftClass.TableSchema, association.LeftClass.TableName);
            this.ExecuteSql(sql);

            sql = string.Format("alter table [{0}].[{1}] add constraint FK_{1}_RIGHT foreign key (RIGHTID) references [{2}].[{3}] ([ID]) on delete cascade",
               association.TableSchema, association.TableName, association.RightClass.TableSchema, association.RightClass.TableName);
            this.ExecuteSql(sql);
        }

        public void DeleteAssociationTable(BusinessAssociation association)
        {
            string sql = String.Format("if exists (select 1 from sys.tables t join sys.schemas s on t.schema_id = s.schema_id where s.name='{0}' and t.name= '{1}') drop table [{0}].[{1}]", association.TableSchema, association.TableName);
            this.ExecuteSql(sql);
        }

       
    }
}
