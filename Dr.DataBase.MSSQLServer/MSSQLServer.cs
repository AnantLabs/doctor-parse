using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Dr.DataBase
{
    public class MSSQLServer : IDataBaseProvider, IDisposable
    {
        private SqlConnection connection;

        public bool Execute(string query)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = query;
            cmd.Connection = connection;

            if (connection.State == ConnectionState.Closed)
                 connection.Open();

            if (cmd.ExecuteNonQuery() > 0)
                return true;
            
            return false;
        }

        public MSSQLServer(MSSQLServerSettings settings)
        {
            connection = new SqlConnection(settings.Connection);
        }

        public void Dispose()
        {
            if ((connection != null) && 
                (connection.State == ConnectionState.Open))
                connection.Close();
        }
    }
}
