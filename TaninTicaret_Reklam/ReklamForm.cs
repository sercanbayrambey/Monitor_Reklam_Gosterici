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

        public ReklamForm(AnaForm anaForm)
        {
            this.anaForm = anaForm;
            InitializeComponent();
        }

        private void ReklamForm_Load(object sender, EventArgs e)
        {
            this.WindowState = System.Windows.Forms.FormWindowState.Normal;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
        }

        public void ReklamiTamEkranYap()
        {
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            ResimReklamBaslat();
        }


        public void ReklamiYarimEkranYap()
        {
            this.WindowState = System.Windows.Forms.FormWindowState.Normal;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
        }


        public void ResimReklamBaslat()
        {
            resimReklamTH = new Thread(new ThreadStart(ResimReklamGoster));
            resimReklamTH.Start();
        }


        public void ResimReklamGoster()
        {
            ResimReklam = new ResimReklamlar();
            List<ucUrunOzellik> ucUrunList = ResimReklam.ResimReklamlariCek();

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
                        
                            Thread.Sleep(5000);
                            panelContainer.BeginInvoke((Action)(() =>
                            {
                                panelContainer.Controls.Clear();
                            }));
                        }
                    }
                }
                catch
                {
                    break;
                }
            }
        }

        public void YaziReklamBaslat()
        {
            yaziReklamTH = new Thread(new ThreadStart(YaziReklamGoster));
            yaziReklamTH.Start();
        }

        public void YaziReklamDurdur()
        {
            yaziReklamTH.Abort();
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
    }
}
