using Microsoft.EntityFrameworkCore;

namespace RRS.Models
{
    public class Add_Menu_Modal
    {
        public int Id { get; set; }  // Primary Key
        public required string MenuName { get; set; }  // Name of the menu item
        public required string Description { get; set; }  // Description of the menu item
        public int Price { get; set; }  // Price of the menu item
        public required string Category { get; set; }

    }
}
