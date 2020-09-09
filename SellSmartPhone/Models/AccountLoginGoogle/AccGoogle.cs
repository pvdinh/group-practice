using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SellSmartPhone.Models.AccountLoginGoogle
{
    public class AccGoogle
    {
        public string Name { set; get; }
        public string Id { set; get; }
        public string Email { set; get; }
        public string Image { set; get; }

        public static int LoginGoogle(AccGoogle accGoogle, SellphonesEntities db)
        {
            var acc = db.Accounts.Where(s => s.Email == accGoogle.Email).FirstOrDefault();
            if (acc == null)
            {
                var IDMax = db.Accounts.OrderByDescending(s => s.IDAccount).Take(1).FirstOrDefault();
                Account account = new Account();
                account.IDAccount = IDMax.IDAccount + 1;
                account.Password = "1111";
                account.Phonenumber = "";
                account.Type = "customer";
                account.Ngaysinh = DateTime.MinValue;
                account.Username = accGoogle.Name;
                account.Address = "";
                account.Avatar = accGoogle.Image;
                account.Email = accGoogle.Email;
                account.Hoten = accGoogle.Name;




                try
                {
                    db.Accounts.Add(account);
                    db.SaveChanges();
                }
                catch (Exception ee)
                {

                }

                return account.IDAccount;
            }
            else
            {
                return acc.IDAccount;
            }
        }
    }
}