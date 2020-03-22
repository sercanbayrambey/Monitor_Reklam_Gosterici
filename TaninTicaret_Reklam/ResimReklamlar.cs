﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace TaninTicaret_Reklam
{
    class ResimReklamlar
    {

        public List<ucUrunOzellik> ResimReklamList { get; set; }
        private DB db;
        public ResimReklamlar()
        {
            ResimReklamList = new List<ucUrunOzellik>();
            db = new DB();
        }

        public List<ucUrunOzellik> ResimReklamlariCek()
        {
            SqlDataReader dr;
            ResimReklamList.Clear();
            string query = "SELECT * FROM tblResimReklamlar";
            db.Connect();
            dr = db.GetQuery(query);
            while(dr.Read())
            {
                ucUrunOzellik urun = new ucUrunOzellik();
                urun.UrunAd = dr["urun_ad"].ToString();
                urun.UrunAciklama = dr["urun_aciklama"].ToString();
                urun.UrunFiyat = Convert.ToDecimal(dr["urun_fiyat"]);
                urun.UrunResimYol = dr["urun_resim_yol"].ToString();
                urun.BilgileriFormaCek();
                ResimReklamList.Add(urun);
            }
            db.Close();
            return ResimReklamList;

        }
    }
}
