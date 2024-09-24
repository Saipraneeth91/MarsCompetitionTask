using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mars_Competition.Models
{
    public class LoginData
    {
        public LoginInfo ValidLogin { get; set; }
        public LoginInfo InvalidLogin { get; set; }
    }

    public class LoginInfo
    {
        public string Emailaddress { get; set; }
        public string Password { get; set; }
    }
}




