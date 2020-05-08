using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SellSmartPhone.Models
{
    public class viewproductHome
    {
        private int masp;
        private string tensp;
        private double gia;
        private string anh;
        private string tenhangsx;
        private int? ishot;
        private int? isnew;

        public viewproductHome() { }
        public viewproductHome(int masp, string tensp, double gia, string anh, string tenhangsx,int? ishot,int? isnew) 
        { this.masp = masp; this.tensp = tensp; this.gia = gia; this.anh = anh; this.tenhangsx = tenhangsx; this.ishot = ishot;this.isnew = isnew; }

        public int Masp { get { return masp; } }
        public string Tensp { get { return tensp; } }
        public double Gia { get { return gia; } }
        public string Anh { get { return anh; } }
        public string Tenhangsx { get { return tenhangsx; } }
        public int? Ishot { get { return ishot; } }
        public int? Isnew { get { return isnew; } }
    }
}