using System;
using System.Collections.Generic;
using System.Text;

namespace Lotus.Shared
{
    public class EmailTesterModel
    {
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public bool HtmlBody { get; set; }

        public string Email_Sender { get; set; }
        public string SmtpServer { get; set; }
        public string Port { get; set; }
        public string Email_Pass { get; set; }
        public string SSL { get; set; }
    }
}
