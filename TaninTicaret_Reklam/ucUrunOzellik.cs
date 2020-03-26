﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TaninTicaret_Reklam
{
    public partial class ucUrunOzellik : UserControl
    {
        public ucUrunOzellik()
        {
            InitializeComponent();
        }
        public string UrunAd { get; set; }
        public int UrunID { get; set; }
        public string UrunAciklama { get; set; }
        public string UrunResimYol { get; set; }
        public PictureBox pboxUrun { get; set; }

        public void BilgileriFormaCek()
        {

            lblUrunAdi.Text = UrunAd;
            lblOzellikler.Text = UrunAciklama;
            //pboxUrun.ImageLocation = UrunResimYol;
        }


    }
}
