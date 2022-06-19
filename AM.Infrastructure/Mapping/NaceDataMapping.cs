using AM.Domain.NaceAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AM.Infrastructure.Mapping
{
    public class NaceDataMapping : IEntityTypeConfiguration<NaceData>
    {
        public void Configure(EntityTypeBuilder<NaceData> builder)
        {
            builder.ToTable("NaceData", schema: "dbo");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.NaceId);
            builder.Property(x => x.ListingId);
            builder.OwnsMany(x => x.NaceDetailDatas, ModelBuilder =>
            {
                ModelBuilder.ToTable("NaceDataDetail", schema: "dbo");
                ModelBuilder.HasKey(x => x.Id);
                ModelBuilder.Property(x => x.ItemId);
                ModelBuilder.Property(x => x.NaceData).HasMaxLength(100);
            });
        }
    }
}