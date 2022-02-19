using BookShop.Data.Entetiy;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookShop.Data.Configuration
{
    public class JanrConfiguration : IEntityTypeConfiguration<Janr>
    {
        public void Configure(EntityTypeBuilder<Janr> builder)
        {
            builder.Property(j => j.Name).IsRequired(true).HasMaxLength(30);
            builder.Property(j => j.DisplayStatus).HasDefaultValue(true);
        }
    }
}
