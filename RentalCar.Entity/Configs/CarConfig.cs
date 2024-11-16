using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RentalCar.Entity.Entities;

namespace RentalCar.Entity.Configs
{
    public class CarConfig : IEntityTypeConfiguration<Car>
    {
        public void Configure(EntityTypeBuilder<Car> builder)
        {
            builder.ToTable("Car", "instance");
            builder.HasKey(e => e.Id);

            builder.Property(e => e.BasePrice)
            .HasColumnType("decimal(18,2)")
            .HasPrecision(18, 2);

            builder.Property(e => e.Deposit)
                .HasColumnType("decimal(18,2)")
                .HasPrecision(18, 2);

            //CarOwner 1 - n Car
            builder.HasOne(e => e.CarOwner)
                .WithMany(e => e.Cars)
                .HasForeignKey(e => e.CarOwnerId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FKCar_CarOwnerId");

            //Car 1 - n Booking
            builder.HasMany(e => e.Bookings)
                .WithOne(e => e.Car)
                .HasForeignKey(e => e.CarId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FKBooking_CarId");
        }
    }
}
