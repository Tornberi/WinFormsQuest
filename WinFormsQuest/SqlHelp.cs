using System;
using System.Data;
using System.Data.SqlClient;

namespace WinFormsQuest
{
    public class SqlHelp
    {
        private static SqlCommand command = new SqlCommand();
        private static SqlConnection connect = new SqlConnection(@"Data Source=.\SQLEXPRESS;Initial Catalog=Products;Integrated Security=True;");
        private static SqlDataAdapter adapter = new SqlDataAdapter();
        private static DataTable table;

        public static DataTable load_table(string cmd)
        {
            try
            {
                table = new DataTable();
                command.Connection = connect;
                command.CommandText = cmd;
                command.CommandTimeout = Int32.MaxValue;
                adapter.SelectCommand = command;
                connect.Open();
                adapter.Fill(table);
                return table;

            }
            catch (SqlException)
            {
                throw;
            }
            finally 
            { 
                connect.Close(); 
            }
        }

        public static string string_table(string cmd)
        {
            string s;
            table = new DataTable();
            command.Connection = connect;
            command.CommandText = cmd;
            command.CommandTimeout = Int32.MaxValue;
            adapter.SelectCommand = command;
            connect.Open();
            s = Convert.ToString(command.ExecuteScalar());
            connect.Close();
            return s;
        }

        
    }
}
