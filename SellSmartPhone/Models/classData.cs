using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SellSmartPhone.Models
{
    public class classData
    {
        public List<Sanpham> allsanphams { get; set; }
        public List<HangSX> allhangsxs { get; set; }
        public List<LoaiSP> allloaisxs { get; set; }
    }
}