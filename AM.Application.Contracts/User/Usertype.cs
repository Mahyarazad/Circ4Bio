using System.Collections.Generic;

namespace AM.Application.Contracts.User
{
    public class Usertype
    {
        public Usertype(int typeId, string typeName)
        {
            TypeId = typeId;
            TypeName = typeName;
        }

        public int TypeId { get; set; }
        public string TypeName { get; set; }

        public List<Usertype> GetUserTypeList()
        {
            return new List<Usertype> {
                new Usertype(2, "Technology Provider"),
                new Usertype(3, "Plant"),
                new Usertype(4, "Supplier of Raw Material"),
                new Usertype(5, "Customer of Raw Material")
            };
        }
    }
}