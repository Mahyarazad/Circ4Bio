using System;
using System.Collections.Generic;

namespace AM.Application.Contracts.Nace
{
    public class NaceViewModel
    {
        public DateTime CreationTime { get; set; }
        public long NaceId { get; set; }
        public string? Title { get; set; }
        public List<DetailViewModel>? Items { get; set; }
        public long ListingId { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class DetailViewModel
    {
        public DetailViewModel(int detailId, string? detailBody, bool isDeleted
            , List<ListItemsViewModel> listItems)
        {
            DetailId = detailId;
            DetailBody = detailBody;
            IsDeleted = isDeleted;
            ListItems = listItems;
        }

        public int DetailId { get; set; }
        public string? DetailBody { get; set; }
        public List<ListItemsViewModel> ListItems { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class ListItemsViewModel
    {
        public ListItemsViewModel(int listItemDetailId, bool isDeleted, string? listItemDetail)
        {
            ListItemDetailId = listItemDetailId;
            IsDeleted = isDeleted;
            ListItemDetail = listItemDetail;
        }

        public int ListItemDetailId { get; set; }
        public string? ListItemDetail { get; set; }
        public bool IsDeleted { get; set; }
    }
}