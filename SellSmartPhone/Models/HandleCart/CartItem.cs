using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SellSmartPhone.Models.HandleCart
{
    public class CartItem
    {
        public Sanpham sanPham { get; set; }
        public int soLuong { get; set; }
        private double tinhtien;
        public void tinhTien()
        {
            tinhtien = (double)sanPham.Gia * soLuong;
        }
        
    }
}