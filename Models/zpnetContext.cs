using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using zpnet.Models;

namespace zpnet.Models
{
    public class zpnetContext : DbContext
    {
        public zpnetContext (DbContextOptions<zpnetContext> options)
            : base(options)
        {
        }

        public DbSet<zpnet.Models.Student> Student { get; set; } = default!;
        public DbSet<zpnet.Models.Field> Field { get; set; } = default!;
        public DbSet<zpnet.Models.Course> Course { get; set; } = default!;
        public DbSet<zpnet.Models.Grade> Grade { get; set; } = default!;
        public DbSet<zpnet.Models.GradeDetail> GradeDetail {get;set;} = default!;
    }
}
