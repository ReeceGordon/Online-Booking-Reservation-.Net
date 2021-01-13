using System;
using System.Collections.Generic;
using System.Text;

namespace Lotus.Shared.Identity
{
    public class UserRoleModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public bool IsSelected { get; set; }
    }
}
