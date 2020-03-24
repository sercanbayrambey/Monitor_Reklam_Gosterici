﻿using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace TaninTicaret_Reklam
{
    partial class AnaForm : MetroFramework.Forms.MetroForm
    {
        ReklamForm reklamForm;
        DB db;
        public YaziReklamlar yaziReklamlar;
        public Settings settings;
        private string UrunImageDir;
        private int SecilenUrunID;
        public AnaForm()
        {
            InitializeComponent();
        }

        private void AnaForm_Load(object sender, EventArgs e)
        {
            yaziReklamlar = new YaziReklamlar();
            db = new DB();
            lblTrackBar.Text = tbarYaziSure.Value.ToString() + " Saniye";
            lblYaziReklamTrackBarBoyut.Text = trackbarYaziBoyutu.Value.ToString() + " Pixel";
            btnYaziReklamDurdur.Visible = false;
            btnYaziReklamBaslat.Visible = true;
            btnResimReklamBaslat.Visible = true;
            btnResimReklamDurdur.Visible = false;
            tabPageAnaSayfa.Focus();
            tabPageAnaSayfa.Select();
            tabControl.TabPages.Remove(tabPageUrunReklam);
            tabControl.TabPages.Remove(tabPageYaziReklam);
            lblReklamGecisSuresi.Text = "Reklam Geçiş Süresi: " + tbarResimReklamSure.Value.ToString() + " Saniye";
            reklamForm = new ReklamForm(this);
            reklamForm.Show();
            this.WindowState = FormWindowState.Maximized;
            TabloTasarimiUygula();
            AyarlariProgramaCek();
            ReklamiKucukEkranYap();
        }

        private void AyarlariProgramaCek()
        {
            settings = db.AyarlariProgramaCek();
            tboxAnaSayfa_FirmaAdi.Text = settings.SirketAdi;
            tboxAnaSayfa_Telefon.Text = settings.TelefonNumarasi;
            tboxAnaSayfa_WebSite.Text = settings.WebSitesi;
            reklamForm.lblTaninTicaret.Text = settings.SirketAdi;
            reklamForm.lblTelefon.Text = settings.TelefonNumarasi;
            reklamForm.lblSite.Text = settings.WebSitesi;
        }

        private void btnYaziReklamEkle_Click(object sender, EventArgs e)
        {
            yaziReklamlar.YaziReklamEkle(tboxYaziReklam.Text, tbarYaziSure.Value,colorDialogYaziReklam.Color,colorDialogYaziArkaPlan.Color,tboxYaziReklam.Lines.Count(),cboxYanipSonmeEfekt.Checked,trackbarYaziBoyutu.Value,colorDialogFormArkaPlan.Color);
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

        public void UrunleriTabloyaCek(string query=null)
        {
            if (query == null)
                query = "SELECT TOP 50 * FROM tblResimReklamlar";
            dataGridUrunler.DataSource = db.GetQueryDataTable(query);
        }

        private void UrunAra(string urunAdi)
        {
            string query = String.Format("SELECT * FROM tblResimReklamlar WHERE urun_ad LIKE '%{0}%'", urunAdi);
            dataGridUrunler.DataSource = db.GetQueryDataTable(query);
        }

        private void TabloTasarimiUygula()
        {
            dataGridUrunler.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridUrunler.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(238, 239, 249);
            dataGridUrunler.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dataGridUrunler.DefaultCellStyle.SelectionBackColor = Color.DodgerBlue;
            dataGridUrunler.DefaultCellStyle.SelectionForeColor = Color.WhiteSmoke;
            dataGridUrunler.DefaultCellStyle.Font = new Font("Century Gothic", 15);
            dataGridUrunler.BackgroundColor = Color.White;

            dataGridUrunler.EnableHeadersVisualStyles = false;
            dataGridUrunler.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridUrunler.ColumnHeadersDefaultCellStyle.BackColor = Color.DodgerBlue;
            dataGridUrunler.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dataGridUrunler.AutoGenerateColumns = true;
        }



        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            lblTrackBar.Text = tbarYaziSure.Value.ToString() + " Saniye";
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
        }

        private void btnReklamDurdur_Click(object sender, EventArgs e)
        {
            reklamForm.YaziReklamDurdur();
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

        private void tboxAramaUrunAdi_TextChanged(object sender, EventArgs e)
        {
           UrunAra(tboxAramaUrunAdi.Text);
        }

        private void btnUrunEkle_Click(object sender, EventArgs e)
        {
            if (tboxUrunEkle_UrunAdi.Text == String.Empty || UrunImageDir == String.Empty)
            {
                MessageBox.Show("Lütfen tüm alanları doldurunuz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if(db.UrunEkle(tboxUrunEkle_UrunAdi.Text,tboxUrunEkle_UrunAciklama.Text,UrunImageDir))
            {
                MessageBox.Show("Ürün başarıyla eklendi.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                UrunleriTabloyaCek();
                reklamForm.ResimReklamDurdur();
                reklamForm.ResimReklamBaslat(tbarYaziSure.Value);
                pboxUrunOnizleme.Image.Save(UrunImageDir);
                tboxUrunEkle_UrunAdi.Text = String.Empty;
                pboxUrunOnizleme.Image = null;
                tboxUrunEkle_UrunAciklama.Text = String.Empty;
            }
            else
                MessageBox.Show("Ürün eklenirken bir hata oluştu.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

        }

        private void btnGozat_Click(object sender, EventArgs e)
        {
            Image File;
            OpenFileDialog f = new OpenFileDialog();
            f.Filter = "Resim Dosyalari (*.jpg, *.png) | *.jpg; *.png";

            if (f.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    File = Image.FromFile(f.FileName);
                    pboxUrunOnizleme.Image = File;
                    UrunImageDir = "images\\" + f.SafeFileName;
                    
                }
                catch
                {
                    MessageBox.Show("Resim eklenirken bir hata oluştu. Lütfen başka bir resim deneyin veya program yöneticisine ulaşın.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                
            }
        }

        private void dataGridUrunler_MouseClick(object sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Right)
            {
                try
                {
                    ContextMenuStrip clickMenu = new System.Windows.Forms.ContextMenuStrip();
                    int clickedIndex = dataGridUrunler.HitTest(e.X, e.Y).RowIndex;

                    if(clickedIndex>=0)
                    {
                        SecilenUrunID = Convert.ToInt32(dataGridUrunler.Rows[clickedIndex].Cells[0].Value);
                        string urunAd = dataGridUrunler.Rows[clickedIndex].Cells[1].Value.ToString();
                        ToolStripLabel tempToolStrip = new ToolStripLabel(urunAd);
                        tempToolStrip.ForeColor = Color.DodgerBlue;
                        tempToolStrip.Font = new Font("Century Gothic", 14,FontStyle.Bold);
                        clickMenu.Items.Insert(0, tempToolStrip);
                        clickMenu.Items.Insert(1, new ToolStripLabel("--------"));
                        clickMenu.Items.Add("Ürünü Sil");
                        clickMenu.Items.Add("Ürünü Ekranda Göster");
                    }
                    clickMenu.Show(dataGridUrunler, e.X, e.Y);
                    clickMenu.ItemClicked += new ToolStripItemClickedEventHandler(clickMenu_ItemCliked);
                }
                catch
                {
                    return;
                }
            }
        }

        private void clickMenu_ItemCliked(object sender,ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem.Text.Equals("Ürünü Sil"))
            {
                DialogResult dialogResult = MessageBox.Show("Silmek istediğinize emin misiniz?", "Silme İşlemi", MessageBoxButtons.YesNo,MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                {
                   if(db.UrunSil(SecilenUrunID))
                    {
                        MessageBox.Show("Ürün başarıyla silindi.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        UrunleriTabloyaCek();
                    }
                   else
                        MessageBox.Show("Ürün silinirken bir hata oluştu.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else if (dialogResult == DialogResult.No)
                {
                    return;
                }
            }
            else if(e.ClickedItem.Text.Equals("Ürünü Ekranda Göster"))
            {
                reklamForm.UrunuEkrandaGoster(SecilenUrunID);
            }
            return;
        }

        private void metroButton2_Click(object sender, EventArgs e)
        {
            reklamForm.ResimReklamModunaGec();
        }

        private void btnYaziArkaPlanRenk_Click(object sender, EventArgs e)
        {
            colorDialogFormArkaPlan.ShowDialog();
            btnFORM_ARKAPLANRENK.BackColor = colorDialogFormArkaPlan.Color;
        }

        private void btnResimReklamDurdur_Click(object sender, EventArgs e)
        {
            reklamForm.ResimReklamDurdur();
        }

        private void btnResimReklamBaslat_Click(object sender, EventArgs e)
        {
            reklamForm.ResimReklamBaslat(tbarResimReklamSure.Value);
        }

        private void tbarResimReklamSure_Scroll(object sender, EventArgs e)
        {
            lblReklamGecisSuresi.Text = "Reklam Geçiş Süresi: " + tbarResimReklamSure.Value.ToString() + " Saniye";
        }

        private void btnAnaSayfa_ayarKaydet_Click(object sender, EventArgs e)
        {
            if(db.AyarlariKaydet(tboxAnaSayfa_FirmaAdi.Text, tboxAnaSayfa_WebSite.Text, tboxAnaSayfa_Telefon.Text))
            {
                MessageBox.Show("Ayarlar başarıyla kaydedildi.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                AyarlariProgramaCek();
            }
            else
                MessageBox.Show("Ayarlar kaydedilirken bir hata oluştu veya hiç bir ayar değiştirmediniz!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private void btnAnaSayfa_yaziReklamMod_Click(object sender, EventArgs e)
        {
            reklamForm.YaziReklamModunaGec();
        }
    }
}
