using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SellSmartPhone.Models
{
    public class AccLogin
    {
        public string EmailLogin { get; set; }
        public string Password { get; set; }
        public int id { get; set; }
        public string name { get; set; }
        public string username { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string phone { get; set; }
        public string address { get; set; }
        public string type { get; set; }
        public string avatar { get; set; }
        public string RePassword { get; set; }
        
    }
}