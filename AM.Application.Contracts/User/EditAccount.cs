﻿namespace AM.Application.Contracts.User
{
    public class EditUser : RegisterUser
    {
        public long Id { get; set; }
        public string? ActivationGuid { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }
        public long PostalCode { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string? Description { get; set; }
        public string? CompanyName { get; set; }
        public long VatNumber { get; set; }
        public int Status { get; set; }
        public string? Avatar { get; set; }
        public string? WebUrl { get; set; }
        public string? LinkdinUrl { get; set; }
        public string? TwitterUrl { get; set; }
        public string? InstagramUrl { get; set; }
        public string? FaceBookUrl { get; set; }
    }
}