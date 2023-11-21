using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;


namespace FSD08_AppDev2Project.Models
{
    public class ApplicationUser : IdentityUser
    {
        //Already in parent:  UserName, Id, Email, PasswordHash, PhoneNumber
        public bool Active { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string Zipcode { get; set; }
    }
}