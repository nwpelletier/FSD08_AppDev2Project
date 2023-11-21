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
    public class AppliedJob
    {
        public int Id { get; set; }
        public ApplicationUser Applicant { get; set; }
        public Job Job { get; set; }
        public DateTime AppliedDate { get; set; }
        
    }
}