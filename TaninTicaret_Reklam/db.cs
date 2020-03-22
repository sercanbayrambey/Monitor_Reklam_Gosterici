using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace TaninTicaret_Reklam
{

     public class DB
    {
        private SqlConnection con;
        private SqlCommand cmd;
        private SqlDataReader rd;

        public bool Connect()
        {
            string query = "server = SERCAN\\MSSQLSERVER01; database = TaninTicaret_Reklam; integrated security = true";
            this.con = new SqlConnection(query);
            try
            {
                this.con.Open();
                return true;
            }

            catch (Exception e)
            {
                return false;
            }
        }

        public int SetQuery(string query)
        {
            cmd = new SqlCommand(query, con);
            try
            {
                return cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return 0;
            }
        }

        public SqlDataReader GetQuery(string query)
        {
            try
            {
                cmd = new SqlCommand(query, con);
                rd = cmd.ExecuteReader();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return rd;
        }

        public void Close()
        {
            con.Close();
        }
    }
}
