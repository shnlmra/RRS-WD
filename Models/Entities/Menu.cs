using Microsoft.EntityFrameworkCore;

namespace RRS.Models.Entities
{
    public class Menu
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        [Precision(10, 2)]
        public decimal Price { get; set; }
        public bool IsAvailable { get; set; }

        // Foreign key
        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
