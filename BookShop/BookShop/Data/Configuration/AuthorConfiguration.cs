using BookShop.Data.Entetiy;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookShop.Data.Configuration
{
    public class AuthorConfiguration : IEntityTypeConfiguration<Author>
    {
        public void Configure(EntityTypeBuilder<Author> builder)
        {
            builder.Property(a => a.FullName).IsRequired(true).HasMaxLength(20);
            builder.Property(a => a.Image).IsRequired(false).HasMaxLength(100);
            builder.Property(a => a.DisplayStatus).HasDefaultValue(true);
        }
    }
}
