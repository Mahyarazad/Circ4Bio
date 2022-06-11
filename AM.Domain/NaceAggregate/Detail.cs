using System.Collections.Generic;

namespace AM.Domain.NaceAggregate
{
    public class Detail
    {
        protected Detail() { }
        public int NaceDetailId { get; private set; }
        public string? DetailBody { get; private set; }
        public List<ListItems> ListItems { get; private set; }
    }

    public class ListItems
    {
        public int ListItemDetailId { get; private set; }
        public string? ListItemDetail { get; private set; }
    }
}