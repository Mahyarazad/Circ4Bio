﻿using AM.Domain.NaceAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AM.Infrastructure.Mapping
{
    public class NaceMapping : IEntityTypeConfiguration<Nace>
    {

        public void Configure(EntityTypeBuilder<Nace> builder)
        {

            builder.ToTable("Nace", schema: "dbo");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.isDeleted);
            builder.Property(x => x.Title).HasMaxLength(50);
            builder.OwnsMany(x => x.Items, ModelBuilder =>
            {
                ModelBuilder.ToTable("NaceItems");
                ModelBuilder.HasKey(x => x.NaceDetailId);
                ModelBuilder.Property(x => x.DetailBody).HasMaxLength(500);
                ModelBuilder.OwnsMany(x => x.ListItems, z =>
                {
                    z.ToTable("ListItemsDetail");
                    z.HasKey(x => x.ListItemDetailId);
                    z.Property(x => x.ListItemDetail).HasMaxLength(200);
                });
            });

        }
    }
}