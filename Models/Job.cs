using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;


namespace FSD08_AppDev2Project.Models
{
    public class Job
    {
        public int Id { get; set; }
        public Company JobCompany { get; set; }
        public string JobCategory { get; set; }
        public string JobDescription { get; set; }
        public string JobTitle { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}