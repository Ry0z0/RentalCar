using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RentalCar.Entity.Entities;

namespace RentalCar.Entity.Configs
{
    public class BookingConfig : IEntityTypeConfiguration<Booking>
    {
        public void Configure(EntityTypeBuilder<Booking> builder)
        {
            builder.ToTable("Booking", "instance");
            builder.HasKey(e => e.Id);

            builder.HasOne(e => e.Customer)
                .WithMany(e => e.Bookings)
                .HasForeignKey(e => e.CustomerId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FKBooking_CustomerId");

            // Configure the one-to-many relationship
            builder.HasOne(e => e.Car)
                .WithMany(e => e.Bookings)
                .HasForeignKey(e => e.CarId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FKBooking_CarId");

            builder.HasOne(e => e.Feedback)
                .WithOne(e => e.Booking)
                .HasForeignKey<Feedback>(e => e.BookingId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FKFeedback_BookingId");
        }
    }
}

