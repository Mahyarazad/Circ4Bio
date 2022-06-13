using System.Collections.Generic;

namespace AM.Application.Contracts.Nace
{
    public class NaceViewModel
    {
        public int NaceId { get; set; }
        public string? Title { get; set; }
        public List<DetailViewModel>? Items { get; set; }
        public long ListingId { get; set; }
    }

    public class DetailViewModel
    {
        public int DetailId { get; set; }
        public string? DetailBody { get; set; }
        public List<ListItemsViewModel> ListItems { get; set; }
    }

    public class ListItemsViewModel
    {
        public int ListItemDetailId { get; set; }
        public string? ListItemDetail { get; set; }
    }
}