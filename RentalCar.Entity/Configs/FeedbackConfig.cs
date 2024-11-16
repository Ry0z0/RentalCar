using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RentalCar.Entity.Entities;

namespace RentalCar.Entity.Configs
{
    public class FeedbackConfig : IEntityTypeConfiguration<Feedback>
    {
        public void Configure(EntityTypeBuilder<Feedback> builder)
        {
            builder.ToTable("Feedback", "instance");
            builder.HasKey(e => e.Id);

            builder.HasOne(e => e.Booking)
                .WithOne(e => e.Feedback)
                .HasForeignKey<Feedback>(e => e.BookingId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FKFeedback_BookingId");
        }
    }
}
