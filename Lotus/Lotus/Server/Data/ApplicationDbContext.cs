using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lotus.Shared.Identity;

namespace Lotus.Server.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {

        //Original DBContext Code
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

    }
}
