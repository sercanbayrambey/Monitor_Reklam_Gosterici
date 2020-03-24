using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace TaninTicaret_Reklam
{
    partial class ReklamForm : Form
    {
        AnaForm anaForm;
        Thread yaziReklamTH;
        Thread resimReklamTH;
        ResimReklamlar ResimReklam;
        List<ucUrunOzellik> ucUrunList;

        public ReklamForm(AnaForm anaForm)
        {
            this.anaForm = anaForm;
            InitializeComponent();
        }

        private void ReklamForm_Load(object sender, EventArgs e)
        {
            this.WindowState = System.Windows.Forms.FormWindowState.Normal;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.ControlBox = true;
            
        }

        public void ReklamiTamEkranYap()
        {
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.ControlBox = false;
        }

        public void ReklamiYarimEkranYap()
        {
            this.WindowState = System.Windows.Forms.FormWindowState.Normal;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.ControlBox = true;
        }


        public void ResimReklamModunaGec()
        {
            if(yaziReklamTH!=null)
                YaziReklamDurdur();
            if (resimReklamTH != null)
                return;
            anaForm.UrunleriTabloyaCek();
            lblYaziReklam.Visible = false;
            panelBottom.Visible = true;
            panelContainer.Visible = true;
            panelLogo.Visible = true;
            this.BackColor = Color.White;
            panelContainer.BackColor = Color.White;
            anaForm.btnAnaSayfa_yaziReklamMod.Enabled = true;
            anaForm.btnAnaSayfa_resimReklamMod.Enabled = false;
            anaForm.tabControl.TabPages.Remove(anaForm.tabPageYaziReklam);
            anaForm.tabControl.TabPages.Add(anaForm.tabPageUrunReklam);
            anaForm.tabControl.SelectedTab = anaForm.tabPageUrunReklam;
        }

        public void YaziReklamModunaGec()
        {
            if (resimReklamTH != null)
                ResimReklamDurdur();

            anaForm.btnAnaSayfa_yaziReklamMod.Enabled = false;
            anaForm.btnAnaSayfa_resimReklamMod.Enabled = true;
            lblYaziReklam.Visible = true;
            lblYaziReklam.BringToFront();
            panelLogo.Visible = true;
            panelBottom.Visible = true;
           /* panelContainer.Visible = false;*/
            this.BackColor = Color.Black;
            panelContainer.BackColor = Color.Black;
            anaForm.tabControl.TabPages.Remove(anaForm.tabPageUrunReklam);
            if(!anaForm.tabControl.TabPages.Contains(anaForm.tabPageYaziReklam))
                anaForm.tabControl.TabPages.Add(anaForm.tabPageYaziReklam);
            anaForm.tabControl.SelectedTab = anaForm.tabPageYaziReklam;
            
            panelBottom.BringToFront();
            panelLogo.BringToFront();
           /* YaziReklamBaslat();*/
        }



        public void ResimReklamBaslat(int gecisSuresi)
        {
            if (resimReklamTH != null)
                ResimReklamDurdur();
            ResimReklam = new ResimReklamlar();
            ResimReklam.GecmeSuresi = gecisSuresi*1000;
            panelContainer.Controls.Clear();

            resimReklamTH = new Thread(new ThreadStart(ResimReklamGoster));
            resimReklamTH.Start();
            anaForm.btnResimReklamBaslat.Visible = false;
            anaForm.btnResimReklamDurdur.Visible = true;
        }


        public void ResimReklamGoster()
        {
            ucUrunList = ResimReklam.ResimReklamlariCek();

            while(true)
            {
                try
                {
                    int j = 0;
                    for (int i = 0; i < ucUrunList.Count; i++)
                    {
                        var ucItem = ucUrunList[i];
                        int b = this.Size.Width;
                        int a = ucItem.Size.Width * 4 + (20 * 3);
                        int baslangicDeger = (b - a) / 2;   
                        ucItem.Location = new Point(baslangicDeger + ucItem.Size.Width * j + (20 * j), 20);
                        panelContainer.BeginInvoke((Action)(() =>
                        {
                            panelContainer.Controls.Add(ucItem);
                        }));
                   
                        j++;
                        if((i+1)%4 == 0 && i!=0)
                        {
                            j = 0;

                            lblDate.BeginInvoke((Action)(() =>
                            {
                                lblDate.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
                            }));
                            Thread.Sleep(ResimReklam.GecmeSuresi);
                            panelContainer.BeginInvoke((Action)(() =>
                            {
                                panelContainer.Controls.Clear();
                            }));
                        }
                    }
                }
                catch
                {
                    resimReklamTH = null;
                    break;
                }
            }
        }

        public void ResimReklamDurdur()
        {
            panelContainer.Controls.Clear();
            if(resimReklamTH!=null)
                resimReklamTH.Abort();
            anaForm.btnResimReklamBaslat.Visible = true;
            anaForm.btnResimReklamDurdur.Visible = false;
        }


        public void UrunuEkrandaGoster(int urunID)
        {
            panelContainer.Controls.Clear();
            int j = 2;
            if (resimReklamTH != null)
                ResimReklamDurdur();
            else
                ResimReklam = new ResimReklamlar();
            ucUrunList = ResimReklam.ResimReklamlariCek();
            ucUrunOzellik ucItem = ucUrunList.Find(u => u.UrunID == urunID);
            int b = (this.Size.Width - ucItem.Size.Width)/2;
            ucItem.Location = new Point(b, 20);
            panelContainer.Controls.Add(ucItem);
           
        }

        public void YaziReklamBaslat()
        {
            if (yaziReklamTH != null)
                yaziReklamTH.Abort();
            anaForm.btnYaziReklamBaslat.Visible = false;
            anaForm.btnYaziReklamDurdur.Visible = true;
            yaziReklamTH = new Thread(new ThreadStart(YaziReklamGoster));
            yaziReklamTH.Start();
        }

        public void YaziReklamDurdur()
        {
            if(yaziReklamTH!=null)
                yaziReklamTH.Abort();
            anaForm.btnYaziReklamBaslat.Visible = true;
            anaForm.btnYaziReklamDurdur.Visible = false;
        }

        private void YaziReklamGoster()
        {
            while(true)
            {
                List<YaziReklam> yaziReklamlar = anaForm.yaziReklamlar.yaziReklamList;
                for (int i = 0; i < yaziReklamlar.Count; i++)
                {
                    lblYaziReklam.BeginInvoke((Action)(() =>
                    {
                        lblYaziReklam.Font = new Font(lblYaziReklam.Font.FontFamily, yaziReklamlar[i].YaziBoyutu);
                        lblYaziReklam.Size = new Size(this.Size.Width, (yaziReklamlar[i].SatirSayisi * Convert.ToInt32(lblYaziReklam.Font.Size*1.5)));
                        int x = (this.Size.Width - lblYaziReklam.Size.Width) / 2;
                        int y = (this.Size.Height - lblYaziReklam.Size.Height) / 2;
                        lblYaziReklam.Location = new Point(x, y);
                        lblYaziReklam.Text = yaziReklamlar[i].Yazi;
                        lblYaziReklam.Visible = true;
                        lblYaziReklam.ForeColor = yaziReklamlar[i].Renk;
                        lblYaziReklam.BackColor = yaziReklamlar[i].ArkaPlanRenk;
                        this.BackColor = yaziReklamlar[i].FormArkaPlanRenk;
                        this.panelContainer.BackColor = yaziReklamlar[i].FormArkaPlanRenk;
                    }));

                    if(yaziReklamlar[i].Efekt)
                    {
                        int j = 0;
                        while (j<=yaziReklamlar[i].Sure)
                        {
                            if (j % 2 == 0)
                            {
                                lblYaziReklam.BeginInvoke((Action)(() =>
                                {
                                    lblYaziReklam.Visible = true;
                                }));
                            }
                            else
                            {
                                lblYaziReklam.BeginInvoke((Action)(() =>
                                {
                                    lblYaziReklam.Visible = false;
                                }));
                            }
                            j++;
                            Thread.Sleep(1000);
                        }
                    }
                    else
                        Thread.Sleep(yaziReklamlar[i].Sure * 1000);
                }
          
            }

        }


        private void ReklamForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(yaziReklamTH!=null)
                yaziReklamTH.Abort();
            if(resimReklamTH!=null)
                resimReklamTH.Abort();
            Application.Exit();
        }

        private void panelBottom_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
