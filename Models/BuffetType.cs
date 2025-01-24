using Microsoft.EntityFrameworkCore;

namespace RRS.Models
{
    public class BuffetType
    {
        public int Id { get; set; }
        public string BuffetName { get; set; }
        public string? Description { get; set; }
        [Precision(10, 2)]
        public Decimal BuffetPrice { get; set; }
        public string? ImagePath { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

    }
}
