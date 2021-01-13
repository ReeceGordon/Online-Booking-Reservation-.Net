using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Lotus.Shared.Identity
{
    public class EditRoleModel
    {
        public EditRoleModel()
        {
            Users = new List<UserRoleModel>();
        }
        public string Id { get; set; }

        [Required]
        public string RoleName { get; set; }

        public List<UserRoleModel> Users { get; set; }
    }
}
