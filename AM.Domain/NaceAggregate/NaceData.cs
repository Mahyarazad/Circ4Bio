using System.Collections.Generic;
using _0_Framework.Domain;
using AM.Domain.ListingAggregate;

namespace AM.Domain.NaceAggregate
{
    public class NaceData: BaseEntity<long>
    {
        protected NaceData() { }

        public NaceData(List<NaceDetailData> naceDetailDatas, long listingId)
        {
            NaceDetailDatas = naceDetailDatas;
            ListingId = listingId;
        }

        public List<NaceDetailData> NaceDetailDatas { get; private set; }
        public long ListingId { get; private set; }
        public Listing? Listing { get; private set; }
    }

    public class NaceDetailData
    {
        protected NaceDetailData()
        {
        }

        public int Id { get; private set; }
        public string? NaceData { get; private set; }
    }
}
