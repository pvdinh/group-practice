using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SellSmartPhone.Models.HandleCart
{
    public partial class ShoppingCart
    {
        SellphonesEntities dbsellphone = new SellphonesEntities();
        int ShoppingCartId { get; set; }
        public const string CartSession = "CartId";
        public static ShoppingCart Get_Cart(HttpContextBase context)

        {
            var cart = new ShoppingCart();
            return cart;
        }
        

        //method gọi lại giỏ hàng
        

        public void AddToCart1()
        {
            var donHangItem = dbsellphone.DonhangKHs.SingleOrDefault(s => s.MaDH == ShoppingCartId && s.Tinhtrangdonhang == 0);
            if (donHangItem == null)
            {
                //tạo đơn hàng mới nếu chưa có đơn hàng
            }
        }
    }
}