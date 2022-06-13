using System.Collections.Generic;

namespace AM.Application.Contracts.Nace
{
    public class GetDetailList
    {
        public List<string>? DetailBody { get; set; }
        public List<int> GroupSize { get; set; }
        public List<string>? ItemDetailList { get; set; }
    }

    public class CreateDetail
    {
        public string? DetailBody { get; set; }
        public List<string>? ItemDetailList { get; set; }
    }
}