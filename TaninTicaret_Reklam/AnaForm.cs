using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MetroFramework;
using MetroFramework.Forms;
using System.Threading;

namespace TaninTicaret_Reklam
{
    partial class AnaForm : MetroFramework.Forms.MetroForm
    {
        ReklamForm reklamForm;
        public YaziReklamlar yaziReklamlar;
        public AnaForm()
        {
            InitializeComponent();
            yaziReklamlar = new YaziReklamlar();
            reklamForm = new ReklamForm(this);
            reklamForm.Show();
            reklamForm.lblYaziReklam.Text = "TEST";
            lblTrackBar.Text = tbarYaziSure.Value.ToString() + " Saniye";
            lblYaziReklamTrackBarBoyut.Text = trackbarYaziBoyutu.Value.ToString() + " Pixel";
            btnReklamDurdur.Visible = false;
            btnYaziReklamBaslat.Visible = true;
            ReklamiKucukEkranYap();
        }

        private void btnYaziReklamEkle_Click(object sender, EventArgs e)
        {
            yaziReklamlar.YaziReklamEkle(tboxYaziReklam.Text, tbarYaziSure.Value,colorDialogYaziReklam.Color,colorDialogYaziArkaPlan.Color,tboxYaziReklam.Lines.Count(),cboxYanipSonmeEfekt.Checked,trackbarYaziBoyutu.Value);
            YaziReklamlariYenile();
        }

        private void YaziReklamlariYenile()
        {
            lboxYaziReklam.Items.Clear();
            foreach (var yaziReklam in yaziReklamlar.yaziReklamList)
            {
                lboxYaziReklam.Items.Add(yaziReklam);
            }
        }

        private void btnYaziReklamSil_Click(object sender, EventArgs e)
        {
            if(lboxYaziReklam.SelectedIndex>=0)
            {
                yaziReklamlar.YaziReklamSil((YaziReklam)lboxYaziReklam.SelectedItem);
            }
            YaziReklamlariYenile();
        }

 
     

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            lblTrackBar.Text = tbarYaziSure.Value.ToString() + " Saniye";
        }

        private void ReklamiTamEkranYap()
        {
            reklamForm.ReklamiTamEkranYap();
            btnReklamTamEkran.Visible = false;
            btnReklamKucult.Visible = true;
        }

        private void ReklamiKucukEkranYap()
        {
            reklamForm.ReklamiYarimEkranYap();
            btnReklamTamEkran.Visible = true;
            btnReklamKucult.Visible = false;
        }

        private void btnReklamTamEkran_Click(object sender, EventArgs e)
        {
            ReklamiTamEkranYap();
        }

        private void btnReklamKucult_Click(object sender, EventArgs e)
        {
            ReklamiKucukEkranYap();
        }

        private void btnYaziReklamRenkSec_Click(object sender, EventArgs e)
        {
            colorDialogYaziReklam.ShowDialog();
            btnRENK.BackColor = colorDialogYaziReklam.Color;
        }

        private void btnYaziReklamBaslat_Click(object sender, EventArgs e)
        {
            reklamForm.YaziReklamBaslat();
            btnYaziReklamBaslat.Visible = false;
            btnReklamDurdur.Visible = true;
        }

        private void btnReklamDurdur_Click(object sender, EventArgs e)
        {
            reklamForm.YaziReklamDurdur();
            btnYaziReklamBaslat.Visible = true;
            btnReklamDurdur.Visible = false;
        }

        private void AnaForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void btnYaziReklamYaziArkaPlanRenk_Click(object sender, EventArgs e)
        {
            colorDialogYaziArkaPlan.ShowDialog();
            btnYAZI_ARKAPLAN_RENK.BackColor = colorDialogYaziArkaPlan.Color;
        }

        private void gboxYaziReklamEkle_Enter(object sender, EventArgs e)
        {

        }

        private void trackbarYaziBoyutu_Scroll(object sender, EventArgs e)
        {
            lblYaziReklamTrackBarBoyut.Text = trackbarYaziBoyutu.Value.ToString() + " Pixel";
        }

        private void lboxYaziReklam_SelectedIndexChanged(object sender, EventArgs e)
        {
            YaziReklam SeciliItem = (YaziReklam)lboxYaziReklam.SelectedItem;
            if (SeciliItem == null)
                return;
            tboxYaziReklam.Text = SeciliItem.Yazi;
            tbarYaziSure.Value = SeciliItem.Sure;
            trackbarYaziBoyutu.Value = SeciliItem.YaziBoyutu;
            colorDialogYaziArkaPlan.Color = SeciliItem.ArkaPlanRenk;
            colorDialogYaziReklam.Color = SeciliItem.Renk;
            btnYAZI_ARKAPLAN_RENK.BackColor = colorDialogYaziArkaPlan.Color;
            cboxYanipSonmeEfekt.Checked = SeciliItem.Efekt;
            btnRENK.BackColor = colorDialogYaziReklam.Color;
            lblYaziReklamTrackBarBoyut.Text = trackbarYaziBoyutu.Value.ToString() + " Pixel";
            lblTrackBar.Text = tbarYaziSure.Value.ToString() + " Saniye";
        }
    }
}
