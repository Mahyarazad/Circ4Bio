using System.Collections.Generic;
using _0_Framework.Domain;
using AM.Domain.ListingAggregate;

namespace AM.Domain.NaceAggregate
{
    public class NaceData : BaseEntity<long>
    {
        protected NaceData() { }

        public NaceData(List<NaceDetailData> naceDetailDatas, long listingId, long naceId)
        {
            NaceDetailDatas = naceDetailDatas;
            ListingId = listingId;
            NaceId = naceId;
        }

        public List<NaceDetailData> NaceDetailDatas { get; private set; }
        public long ListingId { get; private set; }
        public long NaceId { get; private set; }
    }

    public class NaceDetailData
    {
        protected NaceDetailData()
        {
        }


        public NaceDetailData(int itemId, string? naceData)
        {
            ItemId = itemId;
            NaceData = naceData;
        }

        public int Id { get; private set; }
        public int ItemId { get; private set; }
        public string? NaceData { get; private set; }
    }
}
