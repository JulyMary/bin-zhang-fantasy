using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.IO;

namespace Fantasy.BusinessEngine.Services
{
    public class DebugConnectionStringProvider : IConnectionStringProvider
    {
        
        private string _connectionString;

        public string ConnectionString
        {
            get 
            {
                if (_connectionString == null)
                {
                    string entryDir = LongPath.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().CodeBase);
                    string dbFile = LongPath.Combine(entryDir, @"..\data\fantasy.mdf");
                    _connectionString = string.Format("AttachDbFilename={0};Data Source=.\\SQLEXPRESS;Initial Catalog=Fantasy;Integrated Security=True;", dbFile);
                }
                return _connectionString;
            }
        }

      
    }
}
