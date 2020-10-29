using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SQLite;
using System.IO;

namespace TaninTicaret_Reklam
{

     class DB
    {
        private SQLiteConnection con;
        private SQLiteCommand cmd;
        private SQLiteDataReader rd;

        public bool Connect()
        {
            string query = @"Data Source=TaninTicaret_Reklam.db;Version=3;";
            this.con = new SQLiteConnection(query);
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
            cmd = new SQLiteCommand();
            cmd.Connection = con;
            try
            {
                cmd.CommandText = query;
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

       


        public bool AyarlariKaydet(string sirketAdi, string site, string telefon)
        {

            string query = String.Format("UPDATE tblProgramAyar SET ayar_sirketad = '{0}',ayar_website ='{1}',ayar_telefon='{2}' WHERE ayar_id=1", sirketAdi, site, telefon);
            if (this.SetQuery(query) != 0)
                return true;
            return false;
        }

        public bool UrunEkle(string urunAdi,string urunAciklama,string urun_resim_yol)
        {
            string query = String.Format("INSERT INTO tblResimReklamlar (urun_ad,urun_aciklama,urun_resim_yol) VALUES('{0}','{1}','{2}')",urunAdi,urunAciklama,urun_resim_yol);
            if (this.SetQuery(query) != 0)
                return true;
            return false;


        }

        public bool UrunSil(int urunID)
        {
            string query = String.Format("DELETE FROM tblResimReklamlar WHERE reklam_id = {0}", urunID);
            string getImageQuery = String.Format("SELECT urun_resim_yol FROM tblResimReklamlar WHERE reklam_id = {0}", urunID);
            this.Connect();
            SQLiteDataReader dr = this.GetQuery(getImageQuery);
            dr.Read();
            string resim_yol = dr["urun_resim_yol"].ToString();
            DosyaSil(resim_yol);
            
            if (this.SetQuery(query) != 0)
            {
                this.Close();
                return true;
            }
            this.Close();
            return false;

        }

        public bool DosyaSil(string dir)
        {
            try
            {
                if (File.Exists(dir))
                {
                    File.Delete(dir);
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
            
        }

        public SQLiteDataReader GetQuery(string query)
        {
            try
            {
                cmd = new SQLiteCommand();
                cmd.CommandText = query;
                cmd.Connection = con;
                rd = cmd.ExecuteReader();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return rd;
        }

        public DataSet GetQueryDataTable(string query)
        {
            DataSet dataSet = new DataSet();
            try
            {
                this.Connect();
                SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter(query, con);
                dataAdapter.Fill(dataSet);
                return dataSet;
            }
            catch (Exception e)
            {
                this.Close();
                Console.WriteLine(e.Message);
            }
            
            return dataSet;
        }

        public Settings AyarlariProgramaCek()
        {
            var settings = new Settings();
            
            string query = "SELECT * FROM tblProgramAyar";
            this.Connect();
            SQLiteDataReader dr =this.GetQuery(query);
            while(dr.Read())
            {
                settings.SirketAdi = dr["ayar_sirketad"].ToString();
                settings.TelefonNumarasi = dr["ayar_telefon"].ToString();
                settings.WebSitesi = dr["ayar_website"].ToString();
            }
            this.Close();
            return settings;
        }


        public bool UrunuDuzenle(string urunAd, string urunAciklama, string urunResimYol,int id)
        {
            string query = String.Format("UPDATE tblResimReklamlar SET urun_ad = '{0}', urun_aciklama='{1}' , urun_resim_yol='{2}' WHERE reklam_id={3}", urunAd,urunAciklama,urunResimYol,id);
            if (this.SetQuery(query) != 0)
                return true;
            return false;
        }

        public void Close()
        {
            con.Close();
        }
    }
}
