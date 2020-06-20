using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SellSmartPhone.Models.HandleCart
{
    public class AllCart
    {
        public List<CartItem> cart = new List<CartItem>();
        public List<CartItem> getCart()
        {
            return cart;
        }
    }
}