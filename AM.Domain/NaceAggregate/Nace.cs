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
            ListingId = 0;
            isDeleted = false;
        }

        public string? Title { get; private set; }
        public bool isDeleted { get; private set; }
        public List<Detail>? Items { get; private set; }
        public long? ListingId { get; private set; }
        public Listing? Listing { get; private set; }

        public void DeleteNace()
        {
            isDeleted = true;
        }
    }
}