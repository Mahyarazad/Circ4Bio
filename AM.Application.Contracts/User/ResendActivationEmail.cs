using System;

namespace AM.Application.Contracts.User
{
    public class ResendActivationEmail
    {
        public string? Email { get; set; }
        public Guid ActivationGuid { get; set; }
    }
}