using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SellSmartPhone.Areas.ADMIN.model
{
    public class ThongTinBaoHanh
    {
        public string Hoten { get; set; }
        public string Phonenumber { get; set; }
        public DateTime Ngaydatmua { get; set; }
        public string TenSP { get; set; }
        public int Soluong { get; set; }
    }
}