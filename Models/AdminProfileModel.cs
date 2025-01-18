using Microsoft.AspNetCore.Mvc;

namespace RRS.Models
{
    public class AdminProfile
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string ProfilePicturePath { get; set; }
    }

    public class RestaurantSettings
    {
        public string RestaurantName { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string OperatingHours { get; set; }
        public string Holidays { get; set; }
    }
}

