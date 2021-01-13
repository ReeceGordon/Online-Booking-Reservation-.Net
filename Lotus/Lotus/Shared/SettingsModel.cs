using System;
using System.Collections.Generic;
using System.Text;

namespace Lotus.Shared
{
    public class SettingsModel
    {
        public string Email { get; set; }
        public string Address { get; set; }
        public long Phone_Number { get; set; }
        public string Currency { get; set; }
        public string Password { get; set; }
        public string Email_Sender { get; set; }
        public string SmtpServer { get; set; }
        public string Port { get; set; }
        public string Email_Pass { get; set; }
        public string SSL { get; set; }
        public string Company_Name { get; set; }
        public bool Require_Aproval { get; set; }
    }
}
