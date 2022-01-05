namespace AM.Application.Contracts.User
{
    public class EditUser : RegisterUser
    {
        public long Id { get; set; }
        public string ActivationGuid { get; set; }
    }
}