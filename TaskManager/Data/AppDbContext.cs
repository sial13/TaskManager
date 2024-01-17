using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<TaskSampleDb> TaskSamples { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost; Database=TaskManagement; Username=postgres; password=admin");
        }
    }
}
//Host=localhost; Database=TaskManagement; Username=postgeres; password=admin