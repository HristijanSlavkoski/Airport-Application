using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AirportApplication.Models;

namespace AirportApplication.Data
{
    public class AirportApplicationContext : DbContext
    {
        public AirportApplicationContext (DbContextOptions<AirportApplicationContext> options)
            : base(options)
        {
        }

        public DbSet<AirportApplication.Models.Company> Company { get; set; }

        public DbSet<AirportApplication.Models.Pilot> Pilot { get; set; }

        public DbSet<AirportApplication.Models.Flight> Flight { get; set; }

        public DbSet<AirportApplication.Models.CompanyFlight> CompanyFlight { get; set; }
    }
}
