using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using MySql.Data.MySqlClient;

namespace LibSrv
{
    public class DB_Writer
    {


        public static void start_write(LibHost.Host host, MySqlConnection connection)
        {
            try
            {
                connection.Open();




                connection.Close();
            }
            catch (Exception)
            {

                throw;
            }

        }


        private string sql_SELECT_Execute(string query, MySqlConnection connection)
        {
            MySqlCommand command = new MySqlCommand(query);
            return ((string)command.ExecuteScalar())??"-1";
        }

    }
}
