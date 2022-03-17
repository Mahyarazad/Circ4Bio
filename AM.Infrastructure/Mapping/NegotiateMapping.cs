﻿using System.Security.Cryptography.Xml;
using AM.Domain.NegotiateAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AM.Infrastructure.Mapping
{
    public class NegotiateMapping : IEntityTypeConfiguration<Negotiate>
    {
        public void Configure(EntityTypeBuilder<Negotiate> builder)
        {
            builder.ToTable("Negotiate", schema: "dbo");
            builder.Property(x => x.IsCanceled).IsRequired();
            builder.Property(x => x.IsFinished).IsRequired();
            builder.Property(x => x.IsActive).IsRequired();
            builder.Property(x => x.QuatationConfirm).IsRequired();
            builder.Property(x => x.IsRejected).IsRequired();
            builder.HasKey(x => x.Id);
            builder.HasOne(x => x.Listing).WithMany(x => x.NegotiateList).HasForeignKey(x => x.ListingId);
            builder.HasOne(x => x.Deal).WithMany(x => x.Negotiates).HasForeignKey(x => x.DealId);
            builder.Property(x => x.IsFinished).IsRequired();
            builder.OwnsMany(x => x.Messages, ModelBuilder =>
            {
                ModelBuilder.ToTable("NegotoiateMessages");
                ModelBuilder.HasKey(x => x.Id);
                ModelBuilder.Property(x => x.MessageBody).IsRequired().HasMaxLength(500);
                ModelBuilder.Property(x => x.FilePathString).HasMaxLength(100);
                ModelBuilder.Property(x => x.UserEntity).IsRequired();
                ModelBuilder.Property(x => x.IsRead).IsRequired();
                ModelBuilder.WithOwner(x => x.Negotiate).HasForeignKey(x => x.NegotiateId);
                ModelBuilder.Property(x => x.UserId).IsRequired();
            });
        }
    }
}