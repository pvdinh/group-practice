using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using WebGrease.Css.Extensions;

namespace SellSmartPhone.Models.HandleCart
{
    public partial class Cart
    {
        private SellphonesEntities db = new SellphonesEntities();
        public void Creat(int _idKH)
        {
            DonhangKH donhang = new DonhangKH();
            donhang.MaDH = db.DonhangKHs.OrderByDescending(s => s.MaDH).Select(s => s.MaDH).FirstOrDefault() + 1;
            donhang.MaKH = _idKH;
            donhang.Phivanchuyen = 0;
            donhang.TongTien = 0;
            donhang.Ngaydatmua = DateTime.Now;
            donhang.PhuongthucTT = "null";
            donhang.Tinhtrangdonhang = 0;
            donhang.ghichu = "null";
            db.DonhangKHs.Add(donhang);
            db.SaveChanges();
        }
        public List<Cart> GetCart(int _idKH)
        {
            if (db.DonhangKHs.Where(x => x.MaKH == _idKH && x.Tinhtrangdonhang == 0).FirstOrDefault() == null)
            {
                Creat(_idKH);
            }
            return db.Database.SqlQuery<Cart>("Load_Cart @idKH", new SqlParameter("@idKH", _idKH)).ToList();
        }
        public void AddToCart(int _idSP,int _idKH)
        {
            var sp = db.Sanphams.Where(s => s.MaSP == _idSP).FirstOrDefault();
            var dh = db.DonhangKHs.Where(x => x.MaKH == _idKH && x.Tinhtrangdonhang == 0).FirstOrDefault();
            List<Cart> listCart = new Cart().GetCart(_idKH);
            if (listCart.Where(s => s.MaSP == _idSP).FirstOrDefault() != null)
            {
                int MaDH = listCart[0].MaDH;
                var x = db.ChitietDHs.Where(s => s.MaSP == _idSP && s.MaDH == MaDH).FirstOrDefault();
                x.Soluong += 1;
                x.Thanhtien += x.Thanhtien;
                db.SaveChanges();
            }
            else
            {
                ChitietDH ct = new ChitietDH();
                ct.MaChitietDH = db.ChitietDHs.OrderByDescending(s => s.MaChitietDH).Select(s => s.MaChitietDH).FirstOrDefault() + 1;
                ct.MaDH = dh.MaDH;
                ct.MaSP = _idSP;
                ct.Soluong = 1;
                ct.Thanhtien = sp.Gia;
                db.ChitietDHs.Add(ct);
                db.SaveChanges();
            }
        }
        public void SubCart(int _idKH, int _idSP)
        {
            List<Cart> list = new Cart().GetCart(_idKH); // load danh sách sản phẩm trong giỏ hàng
            int MaDH = list[0].MaDH;
            var x = db.ChitietDHs.Where(s => s.MaSP == _idSP && s.MaDH == MaDH).FirstOrDefault(); //tìm chi tiết đơnn hàng của sản phẩm
            if (x.Soluong > 1)
            {
                x.Soluong -= 1;
                x.Thanhtien -= x.Thanhtien;
                db.SaveChanges();
            }
            else
            {
                x.Soluong = 1;
                db.SaveChanges();
            }
        }
        public void DeleteCart(int _idSP, int _idKH)
        {
            List<Cart> list = new Cart().GetCart(_idKH); //load danh sách sản phẩm
            int MaDH = list[0].MaDH;
            var x = db.ChitietDHs.Where(s => s.MaSP == _idSP && s.MaDH == MaDH).FirstOrDefault(); //tìm chi tiết đơnn hàng của sản phẩm
            db.ChitietDHs.Remove(x);
            db.SaveChanges();
        }

        public double? TotalCart(int _idKH)
        {
            double? Total = 0;
            List<Cart> list = new Cart().GetCart(_idKH); //load danh sách sản phẩm
            foreach (var item in list)
            {
                if (item.Giamgia != null)
                {
                    Total += item.giá_mới;
                }
                else
                {
                    Total += item.Gia;
                }
            }
            return Total;
        }
       
    }
}