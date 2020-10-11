using AspNetCore.Identity.DocumentDb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TwitterBot.Web.Identity
{
    public class ApplicationUser : DocumentDbIdentityUser<DocumentDbIdentityRole>
    {
    }
}
