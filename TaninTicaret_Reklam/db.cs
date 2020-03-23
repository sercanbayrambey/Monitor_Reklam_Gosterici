using System;
using System.IO;
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
            this.Connect();
            cmd = new SqlCommand(query, con);
            try
            {
                int a = cmd.ExecuteNonQuery();
                this.Close();
                return a;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                this.Close();
                return 0;
            }
        }

        public bool UrunEkle(string urunAdi,string urunAciklama,string urun_resim_yol)
        {
            string query = String.Format("INSERT INTO tblResimReklamlar VALUES('{0}','{1}','{2}')",urunAdi,urunAciklama,urun_resim_yol);
            if (this.SetQuery(query) != 0)
                return true;
            return false;


        }

        public bool UrunSil(int urunID)
        {
            string query = String.Format("DELETE FROM tblResimReklamlar WHERE reklam_id = {0}", urunID);
            string getImageQuery = String.Format("SELECT urun_resim_yol FROM tblResimReklamlar WHERE reklam_id = {0}", urunID);
            this.Connect();
            SqlDataReader dr = this.GetQuery(getImageQuery);
            dr.Read();
            string resim_yol = dr["urun_resim_yol"].ToString();
            if(File.Exists(resim_yol))
                File.Delete(resim_yol);
            if (this.SetQuery(query) != 0)
            {
                this.Close();
                return true;
            }
            this.Close();
            return false;

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

        public DataTable GetQueryDataTable(string query)
        {
            DataTable dt = new DataTable();
            try
            {
                this.Connect();
                cmd = new SqlCommand(query, con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                this.Close();
                return dt;
            }
            catch (Exception e)
            {
                this.Close();
                Console.WriteLine(e.Message);
            }
            
            return dt;
        }

        public void Close()
        {
            con.Close();
        }
    }
}
