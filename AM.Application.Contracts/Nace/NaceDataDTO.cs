using System.Collections.Generic;

namespace AM.Application.Contracts.Nace
{
    public class NaceDataDTO
    {
        public long Id { get; set; }
        public long NaceId { get; set; }
        public long ListingId { get; set; }
        public List<int> SelectItemDetails { get; set; }
        public List<int> ItemdetailIndex { get; set; }
        public List<string> ItemdetailValues { get; set; }
    }

    public class NaceDataViewModel
    {
        public long Id { get; set; }
        public long NaceId { get; set; }
        public long ListingId { get; set; }
        public List<NaceDataDetail> NaceDataDetails { get; set; }
    }

    public class NaceDataDetail
    {
        public NaceDataDetail(int itemdetailIndex, string itemdetailValues)
        {
            ItemdetailIndex = itemdetailIndex;
            ItemdetailValues = itemdetailValues;
        }

        public int ItemdetailIndex { get; set; }
        public string ItemdetailValues { get; set; }
    }
}