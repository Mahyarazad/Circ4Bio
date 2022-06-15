﻿using System.Collections.Generic;

namespace AM.Domain.NaceAggregate
{
    public class Detail
    {
        protected Detail() { }

        public Detail(string? detailBody, List<ListItems> listItems)
        {
            DetailBody = detailBody;
            ListItems = listItems;
            IsDeleted = false;
        }

        public int NaceDetailId { get; private set; }
        public string? DetailBody { get; private set; }
        public List<ListItems> ListItems { get; private set; }
        public bool IsDeleted { get; private set; }
        public void DeleteNaceDetail()
        {
            IsDeleted = true;
        }
    }

    public class ListItems
    {
        public ListItems(string? listItemDetail)
        {
            ListItemDetail = listItemDetail;
            IsDeleted = false;
        }

        public int ListItemDetailId { get; private set; }
        public string? ListItemDetail { get; private set; }
        public bool IsDeleted { get; private set; }
        public void DeleteNaceListDetail()
        {
            IsDeleted = true;
        }
    }
}