using Microsoft.EntityFrameworkCore;
using radaway_surcharge_calc_HUN.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace radaway_surcharge_calc_HUN.Data
{
    public class dataContext : DbContext
    {
        public DbSet<ProductFamilies> ProductFamilies { get; set; }
        public DbSet<GlassSurcharges> GlassSurcharges { get; set; }

        public dataContext(DbContextOptions<dataContext> options)
            : base(options)
        {
        }
    }
}
