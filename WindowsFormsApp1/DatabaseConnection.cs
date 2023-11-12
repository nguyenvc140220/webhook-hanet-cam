using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    public class DatabaseConnection
    {
        private SqlConnection connection;

        public DatabaseConnection(string connectionString)
        {
            this.connection = new SqlConnection(connectionString);
        }

        public void Open()
        {
            connection.Open();
        }

        public void Close()
        {
            connection.Close();
        }

        public DataTable QueryData(string sql)
        {
            SqlCommand command = new SqlCommand(sql, connection);
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataTable table = new DataTable();
            adapter.Fill(table);
            return table;
        }
    }
}
