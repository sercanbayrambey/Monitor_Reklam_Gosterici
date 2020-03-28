using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Data.SQLite;

namespace TaninTicaret_Reklam
{
    partial class AnaForm : MetroFramework.Forms.MetroForm
    {
        ReklamForm reklamForm;
        DB db;
        public YaziReklamlar yaziReklamlar;
        public Settings settings;
        private string UrunImageDir,EskiResimYolu;
        private int SecilenUrunID;
        private int SecilenUrunRowIndex;

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
            //this.WindowState = FormWindowState.Maximized;
            TabloTasarimiUygula();
            AyarlariProgramaCek();
            ReklamiKucukEkranYap();
           
        }

        private void AyarlariProgramaCek()
        {
            try
            {
                settings = db.AyarlariProgramaCek();
                tboxAnaSayfa_FirmaAdi.Text = settings.SirketAdi;
                tboxAnaSayfa_Telefon.Text = settings.TelefonNumarasi;
                tboxAnaSayfa_WebSite.Text = settings.WebSitesi;
                reklamForm.lblTaninTicaret.Text = settings.SirketAdi;
                reklamForm.lblTelefon.Text = settings.TelefonNumarasi;
                reklamForm.lblSite.Text = settings.WebSitesi;
                this.Text = settings.SirketAdi + " Reklam Yönetim Uygulaması";
            }
            catch
            {
                MessageBox.Show("Veritabanı bağlantısı sırasında bir hata oluştu." ,"Hata", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

            }
        }

        private void DuzenlemePaneliniAc()
        {
            try
            {
                UrunImageDir = dataGridUrunler.Rows[SecilenUrunRowIndex].Cells[3].Value.ToString();
                tbox_AnaSayfaDuzenle_urunAd.Text= dataGridUrunler.Rows[SecilenUrunRowIndex].Cells[1].Value.ToString();
                tbox_AnaSayfaDuzenle_urunAciklama.Text = dataGridUrunler.Rows[SecilenUrunRowIndex].Cells[2].Value.ToString();
                pboxAnaSayfa_urunDuzenle_resim.ImageLocation = dataGridUrunler.Rows[SecilenUrunRowIndex].Cells[3].Value.ToString();
                EskiResimYolu = pboxAnaSayfa_urunDuzenle_resim.ImageLocation;
                gboxUrunDuzenle.Visible = true;
            }
            catch
            {
                pboxAnaSayfa_urunDuzenle_resim.ImageLocation = null;
            }


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
            try
            {

            if (query == null)
                query = "SELECT * FROM tblResimReklamlar LIMIT 20";
            dataGridUrunler.DataSource = db.GetQueryDataTable(query).Tables[0].DefaultView;
            }
            catch
            {
                MessageBox.Show("Veritabanı bağlantısı sırasında bir hata oluştu." ,"Hata", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void UrunAra(string urunAdi)
        {
            string query = String.Format("SELECT * FROM tblResimReklamlar WHERE urun_ad LIKE '%{0}%'", urunAdi);
            dataGridUrunler.DataSource = db.GetQueryDataTable(query).Tables[0].DefaultView;
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
                if(reklamForm.resimReklamTH!=null)
                {
                    reklamForm.ResimReklamDurdur();
                    reklamForm.ResimReklamBaslat(tbarYaziSure.Value);
                }
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
            Button btn = (Button)sender;
            f.Filter = "Resim Dosyalari (*.jpg, *.png) | *.jpg; *.png";

            if (f.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    File = Image.FromFile(f.FileName);
                    if (btn == btnGozat)
                    {
                        CorrectExifOrientation(File);
                        pboxUrunOnizleme.Image = File;
                    }
                    else
                    {
                        CorrectExifOrientation(File);
                        pboxAnaSayfa_urunDuzenle_resim.Image = File;
                    }
                    Random rnd = new Random();
                    UrunImageDir = "images\\" + rnd.Next(0, 10000) + f.SafeFileName;
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
                        SecilenUrunRowIndex = clickedIndex;
                        string urunAd = dataGridUrunler.Rows[clickedIndex].Cells[1].Value.ToString();
                        ToolStripLabel tempToolStrip = new ToolStripLabel(urunAd);
                        tempToolStrip.ForeColor = Color.DodgerBlue;
                        tempToolStrip.Font = new Font("Century Gothic", 14,FontStyle.Bold);
                        clickMenu.Items.Insert(0, tempToolStrip);
                        clickMenu.Items.Insert(1, new ToolStripLabel("--------"));
                        clickMenu.Items.Add("Ürünü Sil");
                        clickMenu.Items.Add("Ürünü Düzenle");
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
                        reklamForm.ResimReklamDurdur();
                        reklamForm.ResimReklamBaslat(tbarYaziSure.Value);
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
            else if (e.ClickedItem.Text.Equals("Ürünü Düzenle"))
            {
                DuzenlemePaneliniAc();
            }
            return;
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

        private void btnAnaSayfa_resimReklamMod_Click(object sender, EventArgs e)
        {
            reklamForm.ResimReklamModunaGec();
        }

        private void btn_AnaSayfaDuzenle_Kaydet_Click(object sender, EventArgs e)
        {
            gboxUrunDuzenle.Visible = false;
            if(tbox_AnaSayfaDuzenle_urunAd.Text == String.Empty || pboxAnaSayfa_urunDuzenle_resim.ImageLocation ==String.Empty)
            {
                MessageBox.Show("Lütfen tüm alanları doldurunuz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if(db.UrunuDuzenle(tbox_AnaSayfaDuzenle_urunAd.Text,tbox_AnaSayfaDuzenle_urunAciklama.Text,UrunImageDir,SecilenUrunID))
            {
                MessageBox.Show("Ürün başarıyla kaydedildi.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                reklamForm.ResimReklamDurdur();
                reklamForm.ResimReklamBaslat(tbarYaziSure.Value);
                pboxAnaSayfa_urunDuzenle_resim.Image.Save(UrunImageDir);
                this.UrunleriTabloyaCek();
                gboxUrunDuzenle.Visible = false;
                if(UrunImageDir != EskiResimYolu)
                    db.DosyaSil(EskiResimYolu);
                return;
            }
            else
            {
                MessageBox.Show("Ürün kaydedilirken bir hata oluştu veya hiç bir özellik değiştirmediniz!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
        }

        private void btnTumUrunleriGoruntule_Click(object sender, EventArgs e)
        {
            UrunleriTabloyaCek("SELECT * FROM tblResimReklamlar");
            btnTumUrunleriGoruntule.Enabled = false;
        }

        private void lblUrunListeBilgi_Click(object sender, EventArgs e)
        {

        }

        private void CorrectExifOrientation(Image image)
        {
            if (image == null) return;
            int orientationId = 0x0112;
            if (image.PropertyIdList.Contains(orientationId))
            {
                var orientation = (int)image.GetPropertyItem(orientationId).Value[0];
                var rotateFlip = RotateFlipType.RotateNoneFlipNone;
                switch (orientation)
                {
                    case 1: rotateFlip = RotateFlipType.RotateNoneFlipNone; break;
                    case 2: rotateFlip = RotateFlipType.RotateNoneFlipX; break;
                    case 3: rotateFlip = RotateFlipType.Rotate180FlipNone; break;
                    case 4: rotateFlip = RotateFlipType.Rotate180FlipX; break;
                    case 5: rotateFlip = RotateFlipType.Rotate90FlipX; break;
                    case 6: rotateFlip = RotateFlipType.Rotate90FlipNone; break;
                    case 7: rotateFlip = RotateFlipType.Rotate270FlipX; break;
                    case 8: rotateFlip = RotateFlipType.Rotate270FlipNone; break;
                    default: rotateFlip = RotateFlipType.RotateNoneFlipNone; break;
                }
                if (rotateFlip != RotateFlipType.RotateNoneFlipNone)
                {
                    image.RotateFlip(rotateFlip);
                    image.RemovePropertyItem(orientationId);
                }
            }
        }


    }
}
