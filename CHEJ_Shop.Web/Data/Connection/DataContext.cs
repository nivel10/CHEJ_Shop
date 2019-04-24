namespace CHEJ_Shop.Web.Data
{
    using Entities;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using System.Linq;

    //  CHEJ - Implementation Security User ASP.Net
    //public class DataContext : DbContext
    public class DataContext : IdentityDbContext<User>
    {
        #region Properties

        public DbSet<Product> Products { get; set; }

        public DbSet<Country> Countries { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderDetail> OrderDetails { get; set; }

        public DbSet<OrderDetailTemp> OrderDetailTemps { get; set; }

        public DbSet<City> Cities { get; set; }

        #endregion Properties

        #region Constrtructor

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        #endregion Constructor

        #region Methods

        protected override void OnModelCreating(
            ModelBuilder _modelBuilder)
        {
            //  Define Decimal Longht
            _modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasColumnType("decimal(18,2)");

            //  Disable Cascade Delete Rule
            var cascadeFKs = _modelBuilder.Model
                .G­etEntityTypes()
                .SelectMany(t => t.GetForeignKeys())
                .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Casca­de);

            foreach (var fk in cascadeFKs)
            {
                fk.DeleteBehavior = DeleteBehavior.Restr­ict;
            }

            base.OnModelCreating(_modelBuilder);
        }

        #endregion Methods
    }
}