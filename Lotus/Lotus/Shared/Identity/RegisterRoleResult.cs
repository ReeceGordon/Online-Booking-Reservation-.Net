using System;
using System.Collections.Generic;
using System.Text;

namespace Lotus.Shared.Identity
{
    public class RegisterRoleResult
    {
        public bool Successful { get; set; }
        public IEnumerable<string> Errors { get; set; }
    }
}
