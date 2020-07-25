using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Numerics;
using System.Web;

namespace SellSmartPhone.Models.HandleCart
{
    public partial class Cart
    {
        //private SellphonesEntities db = new SellphonesEntities();
        public Sanpham sanPham { get; set; }
        public ChitietDH chitietdonhang { get; set; }
        public int MaChitietDH { get; set; }
        public int MaDH { get; set; }
        public int MaSP { get; set; }
        public string TenSP { get; set; }
        public int LoaiSP { get; set; }
        public int HangXS { get; set; }
        public string TenLoai { get; set; }
        public double Gia { get; set; }
        public Nullable<int> Isnew { get; set; }
        public Nullable<int> Ishot { get; set; }
        public string tenhang { get; set; }
        public string Anh { get; set; }
        public int Soluong { get; set; }
        public Nullable<int> Giamgia { get; set; }
        public Nullable<double> giamoi { get; set; }
        public double Phivanchuyen { get; set; }

        //private double thanhTien;
        //public double ThanhTien
        //{
        //    get { return (double)sanPham.Gia * Soluong; }
        //    set { thanhTien = value; }
        //}

        public List<Cart> ListCart { get; set; }

        //public double Tinhtongtiensanpham()
        //{
        //    double count = 0;
        //    foreach (var temp in ListCart)
        //    {
        //        count += temp.thanhTien;
        //    }
        //    return count;
        //}
        //public void TinhtongtienCart()
        //{
        //    if (Giamgia != null)
        //    {
        //        ThanhTien = (double)giá_mới * Soluong;
        //    }
        //    else
        //    {
        //        ThanhTien = (double)Gia * Soluong;
        //    }
        //}
    }
}