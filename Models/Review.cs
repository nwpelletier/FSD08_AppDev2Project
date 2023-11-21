using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace FSD08_AppDev2Project.Models
{
    public class Review
    {
        public int Id { get; set; }
        public ApplicationUser User { get; set; }
        public Company Company { get; set; }
        public string Reviews { get; set; }
        public int Stars { get; set; }
        
    }
}