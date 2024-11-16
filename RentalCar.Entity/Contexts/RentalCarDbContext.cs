using ClosedXML.Excel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RentalCar.Entity.Configs;
using RentalCar.Entity.Entities;

namespace RentalCar.Entity.Contexts
{
    public class RentalCarDbContext : DbContext
    {
        public RentalCarDbContext()
        {

        }
        public RentalCarDbContext(DbContextOptions<RentalCarDbContext> options) : base(options)
        {
        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            if (!optionsBuilder.IsConfigured)
            {
                var basePath = Path.Combine(Directory.GetCurrentDirectory(), "..", "RentalCar.Server");

                var configuration = new ConfigurationBuilder()
                    .SetBasePath(basePath)
                    .AddJsonFile("appsettings.json")
                    .AddJsonFile("appsettings.Development.json", optional: true)
                    .Build();

                var sqlConnectionStr = configuration.GetConnectionString("MyCnn");

                optionsBuilder.UseSqlServer(sqlConnectionStr, config =>
                {
                    config.EnableRetryOnFailure();
                });
            }
        }

        // public virtual DbSet<UserAnswer> UserAnswers { get; set; }
        public virtual DbSet<CarOwner> CarOwners { get; set; }
        public virtual DbSet<Car> Cars { get; set; }
        public virtual DbSet<Feedback> Feedbacks { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Booking> Bookings { get; set; }
        public virtual DbSet<AddressCity> AddressCities { get; set; }
        public virtual DbSet<AddressDistrict> AddressDistricts { get; set; }
        public virtual DbSet<AddressWard> AddressWards { get; set; }
        public virtual DbSet<Brand> Brands { get; set; }
        public virtual DbSet<Model> Models { get; set; }
        public virtual DbSet<Admin> Admins { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfiguration(new BookingConfig());
            builder.ApplyConfiguration(new FeedbackConfig());
            builder.ApplyConfiguration(new CustomerConfig());
            builder.ApplyConfiguration(new CarOwnerConfig());
            builder.ApplyConfiguration(new CarConfig());

        }

        public void InitializeDatabase()
        {
            // Xóa database nếu đã tồn tại
            this.Database.EnsureDeleted();
            // Tạo lại database
            this.Database.EnsureCreated();
        }
    }
}
