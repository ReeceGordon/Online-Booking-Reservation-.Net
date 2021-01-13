using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Lotus.Shared.Identity
{
    public class CreateRoleModel
    {
        [Required]
        public string RoleName { get; set; }
    }
}
