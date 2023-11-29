using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Domain
{
    public class ApplicationUser : IdentityUser
    {
        public string CustomTag { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }


    }
}
