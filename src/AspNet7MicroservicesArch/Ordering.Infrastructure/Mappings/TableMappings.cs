using Microsoft.EntityFrameworkCore;
using Ordering.Domain.Entities;

namespace Ordering.Infrastructure.Mappings
{
    public static class TableMappings
    {

        public static ModelBuilder AddTableMappings(this ModelBuilder modelBuilder)
        {
            MapOrdersTable(modelBuilder);

            return modelBuilder;
        }

        private static void MapOrdersTable(ModelBuilder builder)
        {
            builder.Entity<Order>(cfg => {
                cfg.ToTable("Orders");

                cfg.HasKey(e => e.Id)
                  .Metadata
                  .IsPrimaryKey();

                cfg.Property(e => e.UserName)
                  .HasMaxLength(100)
                  .IsRequired();

                cfg.Property(e => e.TotalPrice)
                  .HasPrecision(10, 2)
                  .IsRequired();

                cfg.Property(e => e.FirstName)
                  .HasMaxLength(255)
                  .IsRequired();

                cfg.Property(e => e.LastName)
                  .HasMaxLength(255)
                  .IsRequired();

                cfg.Property(e => e.EmailAddress)
                  .HasMaxLength(100)
                  .IsRequired();

                cfg.Property(e => e.AddressLine)
                  .HasMaxLength(255)
                  .IsRequired(false);

                cfg.Property(e => e.Country)
                  .HasMaxLength(100)
                  .IsRequired(false);

                cfg.Property(e => e.State)
                  .HasMaxLength(100)
                  .IsRequired(false);

                cfg.Property(e => e.ZipCode)
                  .HasMaxLength(8)
                  .IsRequired(false);

                cfg.Property(e => e.CardName)
                  .HasMaxLength(100)
                  .IsRequired(false);

                cfg.Property(e => e.CardNumber)
                  .HasMaxLength(16)
                  .IsRequired(false);

                cfg.Property(e => e.Expiration)
                   .HasMaxLength(5)
                   .IsRequired(false);

                cfg.Property(e => e.CVV)
                    .HasMaxLength(3)
                    .IsRequired(false);

                cfg.Property(e => e.PaymentMethod);
            });
        }
    }
}
