using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FSD08_AppDev2Project.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FSD08_AppDev2Project.Data
{
    public class AppDev2DbContext : IdentityDbContext
    {
        public AppDev2DbContext(DbContextOptions<AppDev2DbContext> options) : base (options){
            
        }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Company> Companys { get; set; }
        public DbSet<AppliedJob> AppliedJobs { get; set; }
        
    }
}