using System.Collections.Generic;

namespace AM.Application.Contracts.Nace
{
    public class CreateNaceData
    {
        public List<CreateNaceDetailData> NaceDetailDatas { get; set; }
        public long ListingId { get; set; }
    }
}