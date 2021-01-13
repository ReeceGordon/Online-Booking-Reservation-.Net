using System;
using System.Collections.Generic;
using System.Text;

namespace Lotus.Shared
{
    public class EmailModel
    {
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public bool HtmlBody { get; set; }
    }
}
