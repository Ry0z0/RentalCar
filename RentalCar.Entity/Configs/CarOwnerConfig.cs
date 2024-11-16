using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RentalCar.Entity.Entities;

namespace RentalCar.Entity.Configs
{
    public class CarOwnerConfig : IEntityTypeConfiguration<CarOwner>
    {
        public void Configure(EntityTypeBuilder<CarOwner> builder)
        {
            builder.ToTable("CarOwner", "instance");
            builder.HasKey(e => e.Id);

            builder.HasMany(e => e.Cars)
                .WithOne(e => e.CarOwner)
                .HasForeignKey(e => e.CarOwnerId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FKCar_CarOwnerId");
        }
    }
}
