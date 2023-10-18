using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TimeTracker.Models;

namespace TimeIn.Data
{
    public class TimeInContext : DbContext
    {
        public TimeInContext (DbContextOptions<TimeInContext> options)
            : base(options)
        {
        }

        public DbSet<TimeTracker.Models.Student> Student { get; set; } = default!;
    }
}
