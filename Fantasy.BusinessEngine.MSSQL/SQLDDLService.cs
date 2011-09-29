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
            if (@class.IsSimple)
            {
                this.CreateSimpleClassTable(@class);
            }
            else
            {
                this.CreateStandardClassTable(@class);
            }
        }


        private List<string> GetColumns(BusinessClass @class)
        {
            string sql = string.Format("select name from sys.columns where t1.object_id = object_id('{0}.{1}')", @class.TableSchema, @class.TableName);

            IEntityService es = this.Site.GetRequiredService<IEntityService>();
            using (IDbCommand cmd = es.CreateCommand())
            {
                cmd.CommandText = sql;
                return cmd.ExecuteList<string>();
            }
        }


        public void CreateClassColumn(BusinessProperty property)
        {
            string sql = string.Format(@"if exists (select 1 from sys.tables where object_id('{0}.{1}') = [object_id]) 
                                            if not exists (select 1 from sys.columns where name='{2}' and object_id('{0}.{1}') = [object_id] ) 
	                                            alter table [{0}].[{1}] add {3}", 
                                        property.Class.TableSchema, property.Class.TableName, property.FieldName, this.GetColumnDefine(property));
            this.ExecuteSql(sql);
        }

        public void UpdateClassColumn(BusinessProperty property)
        {
            string sql = string.Format("alter table [{0}].[{1}] alter column {2}", property.TableSchema, property.TableName, this.GetColumnDefine(property));
            this.ExecuteSql(sql);
        }

        public void DropClassColumn(BusinessProperty property)
        {
            string sql = string.Format(@"if exists (select 1 from sys.columns where name='{2}' and object_id('{0}.{1}') = [object_id] ) 
	                                            alter table [{0}].[{1}] drop column {2}",
                                       property.TableSchema, property.TableName, property.FieldName, this.GetColumnDefine(property));
            this.ExecuteSql(sql);
        }

        private void CreateStandardClassTable(BusinessClass @class)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("create table [{0}].[{1}] ( {2} )", @class.TableSchema, @class.TableName, this.GetCreateColumnsDefine(@class));
            if (!string.IsNullOrEmpty(@class.TableSpace))
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


        public void DropClassTable(BusinessClass @class)
        {
            string sql = String.Format("if exists (select 1 from sys.tables t join sys.schemas s on t.schema_id = s.schema_id where s.name='{0}' and t.name= '{1}') drop table [{0}].[{1}]", @class.TableSchema, @class.TableName);
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
            string sql = String.Format("select t1.name from sys.tables t1, sys.schemas t2 where  t1.schema_id = t2.schema_id and t2.name = '{0}'", schema);

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

        public void DropAssociationTable(BusinessAssociation association)
        {
            string sql = String.Format("if exists (select 1 from sys.tables t join sys.schemas s on t.schema_id = s.schema_id where s.name='{0}' and t.name= '{1}') drop table [{0}].[{1}]", association.TableSchema, association.TableName);
            this.ExecuteSql(sql);
        }

        public long GetRecordCount(BusinessClass @class)
        {
            string sql = String.Format("if exists (select 1 from sys.tables where object_id=object_id('{0}.{1}')) select count(*) from [{0}].[{1}]", @class.TableSchema, @class.TableName);
            IEntityService es = this.Site.GetRequiredService<IEntityService>();
            using (IDbCommand cmd = es.CreateCommand())
            {
                cmd.CommandText = sql;
                object rs = cmd.ExecuteScalar();
                if (rs is DBNull)
                {
                    return -1;
                }
                else
                {
                    return Convert.ToInt64(rs);
                }
            }

        }

    }
}
