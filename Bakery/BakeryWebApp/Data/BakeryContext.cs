using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace BakeryWebApp
{
    public partial class BakeryContext : DbContext
    {


        public BakeryContext(DbContextOptions<BakeryContext> options)
            : base(options)
        {
        }

        public virtual DbSet<DayPlan> DayPlans { get; set; }
        public virtual DbSet<DayProduction> DayProductions { get; set; }
        public virtual DbSet<Material> Materials { get; set; }
        public virtual DbSet<Norm> Norms { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<ProductsMaterial> ProductsMaterials { get; set; }
        public virtual DbSet<ProductsPlan> ProductsPlans { get; set; }
        public virtual DbSet<ProductsProduction> ProductsProductions { get; set; }
        public virtual DbSet<Raw> Raws { get; set; }
    }
}
