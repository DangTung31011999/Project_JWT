using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JWT.Model
{
    public class UserModel
    {
        public string UserName { get; set; }
        public string PassWord { get; set; }
        public string EmailAddress { get; set; }
        public string Role { get; set; }
    }

    public class UserConstants
    {
        public static List<UserModel> Users = new List<UserModel>()
        {
            new UserModel(){UserName="admin",PassWord="123456", EmailAddress="admin@gmail.com", Role="admin"},
            new UserModel(){UserName="member",PassWord="123456", EmailAddress="member@gmail.com", Role="member"}
        };
    }
}
