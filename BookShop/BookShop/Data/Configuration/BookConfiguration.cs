using BookShop.Data.Entetiy;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookShop.Data.Configuration
{
    public class BookConfiguration : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            builder.Property(b => b.Name).IsRequired().HasMaxLength(30);
            builder.Property(b => b.Title).IsRequired().HasMaxLength(70);
            builder.Property(b => b.Description).IsRequired().HasMaxLength(300);
            builder.Property(b => b.Image).IsRequired(false).HasMaxLength(100);
            builder.Property(b => b.InStock).HasDefaultValue(true);
            builder.Property(b => b.SalePrice).HasColumnType("decimal(18,2)").IsRequired();
            builder.Property(b => b.CostPrice).HasColumnType("decimal(18,2)").IsRequired();
            builder.Property(b => b.PubilisDate).HasDefaultValueSql("GETUTCDATE()");
        }
    }
}
