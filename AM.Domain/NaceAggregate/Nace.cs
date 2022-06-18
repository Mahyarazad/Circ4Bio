using System.Collections.Generic;
using _0_Framework.Domain;
using AM.Domain.ListingAggregate;

namespace AM.Domain.NaceAggregate
{
    public class Nace : BaseEntity<long>
    {
        protected Nace() { }

        public Nace(string? title, List<Detail>? items)
        {
            Title = title;
            Items = items;
            ListingId = null;
            IsDeleted = false;
        }

        public void Edit(string? title)
        {
            Title = title;
        }

        public void AddDetail(Detail detail)
        {
            Items.Add(new Detail(detail.DetailBody, new List<ListItems>()));
        }

        public string? Title { get; private set; }
        public bool IsDeleted { get; private set; }
        public List<Detail>? Items { get; private set; }
        public long? ListingId { get; private set; }
        public Listing? Listing { get; private set; }

        public void DeleteNace()
        {
            IsDeleted = true;
        }
        public void UndeleteNace()
        {
            IsDeleted = false;
        }
    }
}