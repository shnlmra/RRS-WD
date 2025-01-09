using System.ComponentModel.DataAnnotations;

namespace RRS.Models
{
    public class Menu
    {
        [Key]
        public int Id { get; set; }  // Primary Key
        public string MenuName { get; set; }
        public string? Description { get; set; }
        public double Price { get; set; }
        public bool IsActive { get; set; }
    }
}