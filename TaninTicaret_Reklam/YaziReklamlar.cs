using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace TaninTicaret_Reklam
{
    class YaziReklamlar
    {
        public List<YaziReklam> yaziReklamList{ get; set; }
        public YaziReklamlar()
        {
            yaziReklamList = new List<YaziReklam>();
        }

        public bool YaziReklamEkle(string yazi,int sure,Color renk,Color arkaPlanRenk,int satirSayisi,bool yanipSonmeEfekti,int boyut)
        {
            if (yazi == String.Empty || sure < 0)
            {
                return false;
            }
            else
            {
                YaziReklam yaziReklam = new YaziReklam
                {
                    Yazi = yazi,
                    Sure = sure,
                    Renk = renk,
                    ArkaPlanRenk = arkaPlanRenk,
                    Efekt = yanipSonmeEfekti,
                    SatirSayisi = satirSayisi,
                    YaziBoyutu = boyut
                };
                yaziReklamList.Add(yaziReklam);
                return true;
            }
        }

        public bool YaziReklamSil(YaziReklam yaziReklam)
        {
            yaziReklamList.Remove(yaziReklam);
            return true;
        }



    }
}
