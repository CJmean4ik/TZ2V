using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TZ2V.Repositories
{
    public abstract class DataBaseConfig
    {
        public SqlConnection Connection { get; set; }
        public string connString { get; set; }

        protected DataBaseConfig(string connString)
        {
            Connection = new SqlConnection(connString);
            
            this.connString = connString;
        }
        public void OpenSqlConnection()
        {
            if (Connection.State == System.Data.ConnectionState.Open) return;

            Connection.Open();
        }
        public void CloseSqlConnection()
        {
            if (Connection.State != System.Data.ConnectionState.Open) return;

            Connection.Close();
            Connection.Dispose();           
        }
    }
}
