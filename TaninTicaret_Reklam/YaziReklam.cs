using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace TaninTicaret_Reklam
{
    class YaziReklam
    {
        public string Yazi { get;  set; }
        public int Sure { get;  set; }
        public Color Renk { get;  set; }
        public Color ArkaPlanRenk { get; set; }
        public Color FormArkaPlanRenk { get; set; }
        public bool Efekt = true;
        public int SatirSayisi { get; set; }
        public int YaziBoyutu { get; set; }

   
        public override string ToString()
        {
            return this.Yazi + " (" + this.Sure + " Saniye)" + (this.Efekt ? " (Efektli)" : " (Efektsiz)") +" (Yazı Rengi:" + this.Renk.ToKnownColor()+ ") (Yazı Arkaplan Rengi:" + this.ArkaPlanRenk.ToKnownColor()+")"; 
        }

    }
}
