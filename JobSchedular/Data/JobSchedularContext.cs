using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using JobSchedular.Model;

namespace JobSchedular.Data
{
    public class JobSchedularContext : DbContext
    {
        public JobSchedularContext (DbContextOptions<JobSchedularContext> options)
            : base(options)
        {
        }

        public DbSet<JobSchedular.Model.Employees> Employees { get; set; } = default!;
    }
}
