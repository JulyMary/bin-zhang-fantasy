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
                   
                    _connectionString = "Data Source=.\\SQLEXPRESS;Database=Fantasy;Initial Catalog=Fantasy;Integrated Security=True;";
                }
                return _connectionString;
            }
        }

      
    }
}
